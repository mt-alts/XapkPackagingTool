/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.CustomControl
{
    class AutoNavigateHyperLink : Hyperlink
    {
        public AutoNavigateHyperLink()
        {
            this.RequestNavigate += AutoNavigateHyperLink_RequestNavigate;
        }

        private void AutoNavigateHyperLink_RequestNavigate(
            object sender,
            System.Windows.Navigation.RequestNavigateEventArgs e
        )
        {
            try
            {
                var uri = e.Uri.ToString();
                var isUrl = uri.IsUrl();

                if (isUrl)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = e.Uri.AbsoluteUri,
                    });
                }
                else
                {
                    var _dialogService = App.ServiceProvider.GetRequiredService<IDialogService>();
                    _dialogService.ShowDialogWithoutResult<DocumentViewerVM>(EnvironmentPaths.GetBaseDirectoryFilePath(uri));
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    messageBoxText: $"Error opening uri: {ex.Message}",
                    caption: "Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Warning
                );
            }
        }
    }
}
