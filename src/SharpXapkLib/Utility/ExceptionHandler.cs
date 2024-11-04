/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using SharpXapkLib.Exceptions;
using XapkPackagingTool.Common.Exceptions;

namespace SharpXapkLib.Utility
{
    internal class ExceptionHandler
    {
        public static string GetExceptionMessage(Exception exception)
        {
            switch (exception)
            {
                case InsertException:
                case MetadataConvertException:
                case MetadataReadException:
                    return exception.Message;
                case UnauthorizedAccessException:
                    return string.Format(
                        "AccessDeniedMessage".Localize(),
                        exception.Message.GetValueBetweenQuotes()
                    );
                case ZipEntryNotFoundException zenfe:
                    return string.Format(
                        "ZipEntryNotFound".Localize(),
                        zenfe.ZipFile,
                        zenfe.ZipEntry
                    );
                case FileNotFoundException fnfe:
                    return string.Format("FileNotFoundExceptionMessage".Localize(), fnfe.FileName);
                case IOException:
                    return string.Format(
                        "IOExceptionMessage".Localize(),
                        exception.Message.GetValueBetweenQuotes()
                    );
                case OperationCanceledException:
                    return "OperationCancelled".Localize();
                default:
                    return string.Format("UnexpectedErrorMessage".Localize(), exception.Message);
            }
        }
    }
}
