/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using XapkPackagingTool.ViewModel;

namespace XapkPackagingTool.View
{
    /// <summary>
    /// AboutWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as AboutViewModel;
            if (dataContext != null)
            {
                dataContext.RequestClose += () => { this.Close(); };
                this.DataContext = dataContext;
            }
        }
    }
}
