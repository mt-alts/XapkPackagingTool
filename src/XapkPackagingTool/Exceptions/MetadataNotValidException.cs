using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Exceptions
{
    internal class MetadataNotValidException : Exception
    {
        public MetadataNotValidException() { }

        public MetadataNotValidException(string? message) : base(message)
        {
        }

        public MetadataNotValidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
