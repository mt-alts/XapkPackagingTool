/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XapkPackagingTool.Utility.Converters
{
    internal class ImagePathToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string path && !string.IsNullOrWhiteSpace(path))
                {
                    if (path.Contains('>'))
                    {
                        if (File.Exists(path.Substring(0, path.IndexOf('>'))))
                            return GetImageFromCompressedFile(path);
                    }
                    else if (File.Exists(path))
                    {
                        return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    }
                }

                return GetEmptyDrawingImageFromResource();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DrawingImage drawingImage && IsEmptyDrawingImage(drawingImage))
                return Binding.DoNothing;

            throw new NotImplementedException();
        }

        private static DrawingImage GetEmptyDrawingImageFromResource()
        {
            if (Application.Current.Resources.Contains("NoAppIconSelectedDraw"))
                return Application.Current.Resources["NoAppIconSelectedDraw"] as DrawingImage;

            throw new Exception("EmptyImage resource not found.");
        }

        private static BitmapImage GetImageFromCompressedFile(string path)
        {
            var parsedPath = ParseCompressedFilePath(path);
            var compressedFile = parsedPath.Key;
            var entry = parsedPath.Value;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.Default;
            bitmapImage.StreamSource = GetStreamFromCompressedFile(compressedFile, entry);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private static Stream GetStreamFromCompressedFile(string path, string entry)
        {
            var zipReader = new Common.Utility.ZipUtility.ZipReader(path);
            return zipReader.ReadAsStream(entry);
        }

        public static KeyValuePair<string, string> ParseCompressedFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            int separatorIndex = path.IndexOf('>');

            if (separatorIndex == -1)
                throw new ArgumentException("Path does not contain a '>' separator.", nameof(path));

            string zipPath = path.Substring(0, separatorIndex);
            string entryPath = path.Substring(separatorIndex + 1);

            return new KeyValuePair<string, string>(zipPath, entryPath);
        }

        private static bool IsEmptyDrawingImage(DrawingImage drawingImage)
        {
            if (drawingImage?.Drawing is GeometryDrawing geometryDrawing)
            {
                return geometryDrawing.Brush == Brushes.Wheat;
            }
            return false;
        }
    }

}
