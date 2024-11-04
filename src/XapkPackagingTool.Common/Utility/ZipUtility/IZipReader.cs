/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows.Media.Imaging;

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public interface IZipReader
    {
        string ReadAsString(string fileInZip);
        BitmapImage ReadImage(string imageInZip);
    }
}
