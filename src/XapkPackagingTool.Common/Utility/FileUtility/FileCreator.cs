using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Common.Utility.FileUtility
{
    class FileCreator
    {
        public static void CreateFileIfNotExists(string path)
        {
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (!File.Exists(path))
                File.Create(path);
        }
    }
}
