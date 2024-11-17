/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using Microsoft.Extensions.DependencyInjection;
using XapkPackagingTool.Exceptions;
using XapkPackagingTool.Service.Interfaces;

namespace XapkPackagingTool.Helper
{
    internal class ExceptionHandler
    {
        private static readonly IMessageDialogService _messageDialogService =
            App.ServiceProvider.GetRequiredService<IMessageDialogService>();

        public static void HandleException(Exception exception)
        {
            switch (exception)
            {
                case UnsupportedFileFormatException:
                case BaseApkNotFoundException:
                case MetadataNotValidException:
                    _messageDialogService.ShowWarning(exception.Message, "StrAppName".Localize());
                    break;
                case AssetLoadException:
                    _messageDialogService.ShowError(exception.Message, "StrAppName".Localize());
                    break;
                case FileNotFoundException:
                    _messageDialogService.ShowError(
                        exception.Message,
                        "StrFileNotFoundMessageTitle".Localize()
                    );
                    break;
                case UnableToReadConfigurationException:
                    _messageDialogService.ShowError(
                        exception.Message,
                        "StrConfigFileErrorMessageTitle".Localize()
                    );
                    break;
                case FileProcessingException:
                    _messageDialogService.ShowError(
                        exception.Message,
                        "StrAccessDeniedTitle".Localize()
                    );
                    break;
                case PackageImportException:
                    _messageDialogService.ShowError(
                        exception.Message,
                        "StrImportErrorMessageTitle".Localize()
                    );
                    break;
                case SharpXapkLib.Exceptions.MetadataReadException:
                    _messageDialogService.ShowError(
                        $": {exception.Message}",
                        "StrAppName".Localize()
                    );
                    break;
                default:
                    _messageDialogService.ShowError(
                        "StrUnknownError".Localize() + $": {exception.Message}",
                        "StrAppName".Localize()
                    );
                    break;
            }
        }
    }
}
