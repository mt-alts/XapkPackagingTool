/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using XapkPackagingTool.Helper;

namespace XapkPackagingTool.CustomControl
{
    class AutoNavigateHyperLink : Hyperlink
    {
        private const string LOCAL_WEB_PAGE_VIEWER = "mshta.exe";

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
                var isHttp = uri.StartsWith("http");

                var pinfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = isHttp ? e.Uri.AbsoluteUri : LOCAL_WEB_PAGE_VIEWER,
                    Arguments = isHttp ? null : PathHelper.GetFullPath(uri)
                };

                Process.Start(pinfo);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    messageBoxText: $"Error opening link: {ex.Message}",
                    caption: "Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Asterisk
                );
            }
        }
    }
}
