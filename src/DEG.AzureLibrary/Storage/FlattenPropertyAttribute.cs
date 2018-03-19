using System;

namespace DEG.AzureLibrary.Storage
{
    /// <summary>
    /// Class FlattenPropertyAttribute.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FlattenPropertyAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlattenPropertyAttribute"/> class.
        /// </summary>
        /// <param name="arrayMethod">The array method.</param>
        /// <param name="compress">if set to <c>true</c> [compress].</param>
        public FlattenPropertyAttribute(FlattenArrayMethod arrayMethod = FlattenArrayMethod.Single, bool compress = false)
        {
            ArrayMethod = arrayMethod;
            Compress = compress;
        }

        /// <summary>
        /// Gets the array method.
        /// </summary>
        /// <value>The array method.</value>
        public FlattenArrayMethod ArrayMethod { get; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="FlattenPropertyAttribute"/> is compress.
        /// </summary>
        /// <value><c>true</c> if compress; otherwise, <c>false</c>.</value>
        public bool Compress { get; }
    }
}