using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Utility.Storage
{
    class FileCreator
    {
        public static void CreateFileIfNotExists(string file)
        {
            var fileInDirectory = Path.GetDirectoryName(file);

            if (!File.Exists(file))
            {
                if (!Directory.Exists(fileInDirectory))
                    Directory.CreateDirectory(fileInDirectory);

                using (File.Create(file)) { }
            }
        }
    }
}
