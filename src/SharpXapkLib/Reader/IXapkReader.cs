/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Media.Imaging;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Reader
{
    public interface IXapkReader
    {
        public string ReadAsString(string fileInXapk);
        public BitmapImage GetIcon(string icon);
        public XapkManifest ReadManifest();
    }
}
