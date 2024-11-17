using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Exceptions
{
    public class FileProcessingException : Exception
    {
        public string FilePath { get; }

        public FileProcessingException(string filePath, string message)
            : base(message)
        {
            FilePath = filePath;
        }

        public FileProcessingException(string filePath, string message, Exception innerException)
            : base(message, innerException)
        {
            FilePath = filePath;
        }
    }
}
