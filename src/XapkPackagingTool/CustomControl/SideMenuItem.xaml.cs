/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XapkPackagingTool.CustomControl
{
    /// <summary>
    /// SideMenuItem.xaml etkileşim mantığı
    /// </summary>
    public partial class SideMenuItem : RadioButton
    {
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(SideMenuItem));


        public SideMenuItem()
        {
            InitializeComponent();
        }
    }
}
