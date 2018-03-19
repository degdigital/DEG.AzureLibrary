using System;

namespace DEG.AzureLibrary.Exceptions
{
    class BlobParseException : Exception
    {
        public BlobParseException(string message)
            : base(message) { }
    }
}
