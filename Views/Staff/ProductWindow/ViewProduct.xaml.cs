﻿
using ConvenienceStore.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ConvenienceStore.Views.Staff.ProductWindow
{
    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Window
    {
        public ViewProduct()
        {
            InitializeComponent();
        }

        //private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        //{
        //    Button btn = sender as Button;

        //    btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
        //    btn.Background = new SolidColorBrush(Colors.OrangeRed);
        //}
        //private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        //{
        //    Button btn = sender as Button;
        //    btn.Background = new SolidColorBrush(Colors.Transparent);
        //}

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
