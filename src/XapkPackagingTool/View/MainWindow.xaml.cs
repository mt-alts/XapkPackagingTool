/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using HandyControl.Controls;
using XapkPackagingTool.ViewModel.Main;

namespace XapkPackagingTool.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var dataContext = DataContext as MainViewModel;
            this.DataContext = dataContext;
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}