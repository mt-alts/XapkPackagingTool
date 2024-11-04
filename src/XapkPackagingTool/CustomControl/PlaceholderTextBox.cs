/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XapkPackagingTool.CustomControl
{
    internal class PlaceholderTextBox : TextBox
    {
        public static readonly DependencyProperty IsEmptyProperty = DependencyProperty.Register(
            "IsEmpty",
            typeof(bool),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(false)
        );

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(PlaceholderTextBox),
                new UIPropertyMetadata(new CornerRadius(4))
            );

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush",
            typeof(Brush),
            typeof(PlaceholderTextBox),
            new UIPropertyMetadata(new SolidColorBrush(Colors.LightGray))
        );

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness",
                typeof(Thickness),
                typeof(PlaceholderTextBox),
                new UIPropertyMetadata(new Thickness(1))
            );

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            "Placeholder",
            typeof(string),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(string.Empty)
        );

        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PlaceholderTextBox),
                new FrameworkPropertyMetadata(typeof(PlaceholderTextBox))
            );
        }

        public bool IsEmpty
        {
            get => (bool)GetValue(IsEmptyProperty);
            private set => SetValue(IsEmptyProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            UpdateIsEmpty();
            base.OnInitialized(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            UpdateIsEmpty();
            base.OnTextChanged(e);
        }

        private void UpdateIsEmpty()
        {
            IsEmpty = string.IsNullOrWhiteSpace(Text);
        }
    }
}
