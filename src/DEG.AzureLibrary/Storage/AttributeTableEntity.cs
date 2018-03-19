using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace DEG.AzureLibrary.Storage
{
    /// <summary>
    /// Class AttributeTableEntity.
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class AttributeTableEntity : TableEntity
    {
        /// <summary>
        /// Deserializes the entity using the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> that maps property names to typed <see cref="T:Microsoft.WindowsAzure.Storage.Table.EntityProperty" /> values.
        /// </summary>
        /// <param name="properties">An <see cref="T:System.Collections.Generic.IDictionary`2" /> object that maps property names to typed <see cref="T:Microsoft.WindowsAzure.Storage.Table.EntityProperty" /> values.</param>
        /// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            EntityPropertyConverterEx.ConvertBackByAttribute(properties, this, string.Empty, null, operationContext);
            base.ReadEntity(properties, operationContext);
        }

        /// <summary>
        /// Serializes the <see cref="T:System.Collections.Generic.IDictionary`2" /> of property names mapped to <see cref="T:Microsoft.WindowsAzure.Storage.Table.EntityProperty" /> data values from this <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableEntity" /> instance.
        /// </summary>
        /// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IDictionary`2" /> object that maps string property names to <see cref="T:Microsoft.WindowsAzure.Storage.Table.EntityProperty" /> typed values created by serializing this table entity instance.</returns>
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = base.WriteEntity(operationContext);
            EntityPropertyConverterEx.FlattenByAttribute(properties, this, string.Empty, null, operationContext);
            return properties;
        }
    }
}