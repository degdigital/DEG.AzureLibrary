using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DEG.AzureLibrary.Storage
{
    /// <summary>
    /// Class EntityPropertyConverterEx.
    /// </summary>
    public static class EntityPropertyConverterEx
    {
        static readonly MethodInfo SetPropertyMethod = typeof(EntityPropertyConverter).GetMethod("SetProperty", BindingFlags.Static | BindingFlags.NonPublic, null,
            new[] { typeof(object), typeof(string), typeof(object), typeof(EntityPropertyConverterOptions), typeof(OperationContext) }, null);
        static readonly MethodInfo FlattenMethod = typeof(EntityPropertyConverter).GetMethod("Flatten", BindingFlags.Static | BindingFlags.NonPublic, null,
            new[] { typeof(Dictionary<string, EntityProperty>), typeof(object), typeof(string), typeof(HashSet<object>), typeof(EntityPropertyConverterOptions), typeof(OperationContext) }, null);

        private class ObjectReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);
            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        #region ConvertBack

        /// <summary>
        /// Converts the back by attribute.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="current">The current.</param>
        /// <param name="objectPath">The object path.</param>
        /// <param name="operationContext">The operation context.</param>
        public static void ConvertBackByAttribute(IDictionary<string, EntityProperty> properties, object current, string objectPath, OperationContext operationContext)
        {
            var converterOptions = new EntityPropertyConverterOptions { PropertyNameDelimiter = "_" };
            ConvertBackByAttribute(properties, current, objectPath, converterOptions, operationContext);
        }

        /// <summary>
        /// Converts the back by attribute.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="current">The current.</param>
        /// <param name="objectPath">The object path.</param>
        /// <param name="converterOptions">The converter options.</param>
        /// <param name="operationContext">The operation context.</param>
        public static void ConvertBackByAttribute(IDictionary<string, EntityProperty> properties, object current, string objectPath, EntityPropertyConverterOptions converterOptions, OperationContext operationContext)
        {
            if (properties == null)
                return;
            var propertyNameDelimiter = converterOptions != null ? converterOptions.PropertyNameDelimiter : "_";
            foreach (var item in current.GetType().GetProperties().SelectMany(prop =>
                 prop.CustomAttributes.Where(x => x.AttributeType == typeof(FlattenPropertyAttribute)).Select(attrib => new { prop, attrib })
            ))
            {
                var prop = item.prop;
                if (item.attrib.AttributeType == typeof(FlattenPropertyAttribute))
                {
                    var attrib = prop.GetCustomAttributes<FlattenPropertyAttribute>().FirstOrDefault();
                    SetProperty(properties, attrib, prop, current, string.IsNullOrWhiteSpace(objectPath) ? prop.Name : objectPath + propertyNameDelimiter + prop.Name, converterOptions, operationContext);
                }
            }
            return;
        }

        private static bool SetProperty(IDictionary<string, EntityProperty> properties, FlattenPropertyAttribute attrib, PropertyInfo root, object current, string objectPath, EntityPropertyConverterOptions converterOptions, OperationContext operationContext)
        {
            if (root == null)
                return true;
            if (!properties.TryGetValue(objectPath, out EntityProperty prop))
                return false;
            var type = root.PropertyType;
            if (typeof(ISerializable).IsAssignableFrom(type))
                using (var b = new MemoryStream(!attrib.Compress ?
                    prop.BinaryValue :
                    AzureLibraryExtensions.Decompress(prop.BinaryValue)))
                {
                    var bf = new BinaryFormatter();
                    var value = bf.Deserialize(b);
                    root.SetValue(current, value);
                }
            else if (type.IsArray)
            {
                switch (attrib.ArrayMethod)
                {
                    case FlattenArrayMethod.SpreadFlatten:
                        {
                            var array = (Array)Activator.CreateInstance(type, new object[] { prop.Int32Value.Value });
                            for (var i = 0; i < array.Length; i++)
                            {
                                var key = $"{objectPath}_{i}";
                                var seed = Activator.CreateInstance(type.GetElementType());
                                var value = properties.Where(x => x.Key.StartsWith(key)).Aggregate(seed, (c, kvp) => SetPropertyMethod.Invoke(null, new object[] { c, kvp.Key.Substring(key.Length + 1), kvp.Value.PropertyAsObject, converterOptions, operationContext }));
                                array.SetValue(value, i);
                            }
                            root.SetValue(current, array);
                            break;
                        }
                    case FlattenArrayMethod.SpreadJson:
                        {
                            var array = (Array)Activator.CreateInstance(type, new object[] { prop.Int32Value.Value });
                            for (var i = 0; i < array.Length; i++)
                            {
                                if (!properties.TryGetValue($"{objectPath}_{i}", out EntityProperty propValue))
                                    continue;
                                var value = JsonConvert.DeserializeObject(!attrib.Compress ?
                                    propValue.StringValue :
                                    AzureLibraryExtensions.DecompressString(propValue.BinaryValue), type.GetElementType());
                                array.SetValue(value, i);
                            }
                            root.SetValue(current, array);
                            break;
                        }
                    case FlattenArrayMethod.SpreadBinary:
                        {
                            var array = (Array)Activator.CreateInstance(type, new object[] { prop.Int32Value.Value });
                            for (var i = 0; i < array.Length; i++)
                            {
                                if (!properties.TryGetValue($"{objectPath}_{i}", out EntityProperty propValue))
                                    continue;
                                var value = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(!attrib.Compress ?
                                    propValue.BinaryValue :
                                    AzureLibraryExtensions.Decompress(propValue.BinaryValue)), type.GetElementType());
                                array.SetValue(value, i);
                            }
                            root.SetValue(current, array);
                            break;
                        }
                    case FlattenArrayMethod.SpreadSerialize:
                        {
                            var bf = new BinaryFormatter();
                            var array = (Array)Activator.CreateInstance(type, new object[] { prop.Int32Value.Value });
                            for (var i = 0; i < array.Length; i++)
                            {
                                if (!properties.TryGetValue($"{objectPath}_{i}", out EntityProperty propValue))
                                    continue;
                                using (var b2 = new MemoryStream(!attrib.Compress ?
                                    propValue.BinaryValue :
                                    AzureLibraryExtensions.Decompress(propValue.BinaryValue)))
                                {
                                    var value = bf.Deserialize(b2);
                                    array.SetValue(value, i);
                                }
                            }
                            root.SetValue(current, array);
                            break;
                        }
                    case FlattenArrayMethod.Single:
                        {
                            var value = JsonConvert.DeserializeObject(!attrib.Compress ?
                                prop.StringValue :
                                AzureLibraryExtensions.DecompressString(prop.BinaryValue), type);
                            root.SetValue(current, value);
                            break;
                        }
                    case FlattenArrayMethod.Binary:
                        {
                            using (var b = new MemoryStream(!attrib.Compress ?
                                prop.BinaryValue :
                                AzureLibraryExtensions.Decompress(prop.BinaryValue)))
                            {
                                var br = new BinaryReader(b);
                                var array = (Array)Activator.CreateInstance(type, new object[] { br.ReadInt32() });
                                for (var i = 0; i < array.Length; i++)
                                {
                                    var value = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(br.ReadBytes(br.ReadInt32())), type.GetElementType());
                                    array.SetValue(value, i);
                                }
                                root.SetValue(current, array);
                            }
                            break;
                        }
                    case FlattenArrayMethod.Serialize:
                        {
                            var bf = new BinaryFormatter();
                            using (var b = new MemoryStream(!attrib.Compress ?
                                prop.BinaryValue :
                                AzureLibraryExtensions.Decompress(prop.BinaryValue)))
                            {
                                var br = new BinaryReader(b);
                                var array = (Array)Activator.CreateInstance(type, new object[] { br.ReadInt32() });
                                for (var i = 0; i < array.Length; i++)
                                {
                                    var bytes = br.ReadBytes(br.ReadInt32());
                                    using (var b2 = new MemoryStream(bytes))
                                    {
                                        var value = bf.Deserialize(b2);
                                        array.SetValue(value, i);
                                    }
                                }
                                root.SetValue(current, array);
                            }
                            break;
                        }
                    default: throw new ArgumentOutOfRangeException("attrib.ArrayMethod");
                }
            }
            return true;
        }

        #endregion

        #region Flatten

        /// <summary>
        /// Flattens the by attribute.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="current">The current.</param>
        /// <param name="objectPath">The object path.</param>
        /// <param name="operationContext">The operation context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool FlattenByAttribute(IDictionary<string, EntityProperty> properties, object current, string objectPath, OperationContext operationContext)
        {
            var converterOptions = new EntityPropertyConverterOptions { PropertyNameDelimiter = "_" };
            return FlattenByAttribute(properties, current, objectPath, converterOptions, operationContext);
        }

        /// <summary>
        /// Flattens the by attribute.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="current">The current.</param>
        /// <param name="objectPath">The object path.</param>
        /// <param name="converterOptions">The converter options.</param>
        /// <param name="operationContext">The operation context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool FlattenByAttribute(IDictionary<string, EntityProperty> properties, object current, string objectPath, EntityPropertyConverterOptions converterOptions, OperationContext operationContext)
        {
            if (current == null)
                return true;
            var propertyNameDelimiter = converterOptions != null ? converterOptions.PropertyNameDelimiter : "_";
            foreach (var item in current.GetType().GetProperties().SelectMany(prop =>
                 prop.CustomAttributes.Where(x => x.AttributeType == typeof(FlattenPropertyAttribute)).Select(attrib => new { prop, attrib })
            ))
            {
                var prop = item.prop;
                if (item.attrib.AttributeType == typeof(FlattenPropertyAttribute))
                {
                    var attrib = prop.GetCustomAttributes<FlattenPropertyAttribute>().FirstOrDefault();
                    Flatten(properties, attrib, prop.GetValue(current, null), string.IsNullOrWhiteSpace(objectPath) ? prop.Name : objectPath + propertyNameDelimiter + prop.Name, converterOptions, operationContext);
                }
            }
            return true;
        }

        private static bool Flatten(IDictionary<string, EntityProperty> properties, FlattenPropertyAttribute attrib, object root, string objectPath, EntityPropertyConverterOptions converterOptions, OperationContext operationContext)
        {
            if (root == null)
                return true;
            var type = root.GetType();
            if (typeof(ISerializable).IsAssignableFrom(type))
                using (var b = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(b, root);
                    b.Position = 0;
                    properties.Add(objectPath, !attrib.Compress ?
                        new EntityProperty(b.ToArray()) :
                        new EntityProperty(AzureLibraryExtensions.Compress(b.ToArray())));
                }
            else if (type.IsArray)
            {
                var array = (Array)root;
                switch (attrib.ArrayMethod)
                {
                    case FlattenArrayMethod.SpreadFlatten:
                        {
                            properties.Add(objectPath, new EntityProperty(array.Length));
                            var antecedents = new HashSet<object>(new ObjectReferenceEqualityComparer());
                            for (var i = 0; i < array.Length; i++)
                                FlattenMethod.Invoke(null, new object[] { properties, array.GetValue(i), $"{objectPath}_{i}", antecedents, converterOptions, operationContext });
                            break;
                        }
                    case FlattenArrayMethod.SpreadJson:
                        {
                            properties.Add(objectPath, new EntityProperty(array.Length));
                            for (var i = 0; i < array.Length; i++)
                            {
                                var value = JsonConvert.SerializeObject(array.GetValue(i));
                                properties.Add($"{objectPath}_{i}", !attrib.Compress ?
                                    new EntityProperty(value) :
                                    new EntityProperty(AzureLibraryExtensions.CompressString(value)));
                            }
                            break;
                        }
                    case FlattenArrayMethod.SpreadBinary:
                        {
                            properties.Add(objectPath, new EntityProperty(array.Length));
                            for (var i = 0; i < array.Length; i++)
                            {
                                var value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(array.GetValue(i)));
                                properties.Add($"{objectPath}_{i}", !attrib.Compress ?
                                    new EntityProperty(value) :
                                    new EntityProperty(AzureLibraryExtensions.Compress(value)));
                            }
                            break;
                        }
                    case FlattenArrayMethod.SpreadSerialize:
                        {
                            var bf = new BinaryFormatter();
                            properties.Add(objectPath, new EntityProperty(array.Length));
                            using (var b = new MemoryStream())
                            {
                                var bw = new BinaryWriter(b);
                                for (var i = 0; i < array.Length; i++)
                                {
                                    var value = array.GetValue(i);
                                    using (var b2 = new MemoryStream())
                                    {
                                        bf.Serialize(b2, value);
                                        bw.Write(b2.Length);
                                        bw.Write(b2.ToArray());
                                    }
                                    bw.Flush();
                                    b.Position = 0;
                                    properties.Add($"{objectPath}_{i}", !attrib.Compress ?
                                        new EntityProperty(b.ToArray()) :
                                        new EntityProperty(AzureLibraryExtensions.Compress(b.ToArray())));
                                }
                            }
                            break;
                        }
                    case FlattenArrayMethod.Single:
                        {
                            var value = JsonConvert.SerializeObject(array);
                            properties.Add(objectPath, !attrib.Compress ?
                                new EntityProperty(value) :
                                new EntityProperty(AzureLibraryExtensions.CompressString(value)));
                            break;
                        }
                    case FlattenArrayMethod.Binary:
                        {
                            using (var b = new MemoryStream())
                            {
                                var bw = new BinaryWriter(b);
                                bw.Write(array.Length);
                                for (var i = 0; i < array.Length; i++)
                                {
                                    var value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(array.GetValue(i)));
                                    bw.Write(value.Length);
                                    bw.Write(value);
                                }
                                bw.Flush();
                                b.Position = 0;
                                properties.Add(objectPath, !attrib.Compress ?
                                    new EntityProperty(b.ToArray()) :
                                    new EntityProperty(AzureLibraryExtensions.Compress(b.ToArray())));
                            }
                            break;
                        }
                    case FlattenArrayMethod.Serialize:
                        {
                            var bf = new BinaryFormatter();
                            using (var b = new MemoryStream())
                            {
                                var bw = new BinaryWriter(b);
                                bw.Write(array.Length);
                                for (var i = 0; i < array.Length; i++)
                                {
                                    var value = array.GetValue(i);
                                    using (var b2 = new MemoryStream())
                                    {
                                        bf.Serialize(b2, value);
                                        bw.Write(b2.Length);
                                        bw.Write(b2.ToArray());
                                    }
                                }
                                bw.Flush();
                                b.Position = 0;
                                properties.Add(objectPath, !attrib.Compress ?
                                    new EntityProperty(b.ToArray()) :
                                    new EntityProperty(AzureLibraryExtensions.Compress(b.ToArray())));
                            }
                            break;
                        }
                    default: throw new ArgumentOutOfRangeException("attrib.ArrayMethod");
                }
            }
            return true;
        }
        #endregion
    }
}