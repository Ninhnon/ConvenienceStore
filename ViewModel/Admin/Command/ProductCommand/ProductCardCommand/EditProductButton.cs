﻿using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin.ProductWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.ProductCardCommand
{
    public class EditProductButton : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var currentProduct = (Product)parameter;

            var coppyCurProduct = new Product()
            {
                Title = currentProduct.Title,
                Image = currentProduct.Image,
                Cost = currentProduct.Cost,
                Price = currentProduct.Price,
                Stock = currentProduct.Stock,
                ManufacturingDate = currentProduct.ManufacturingDate,
                ExpiryDate = currentProduct.ExpiryDate,
                Discount = currentProduct.Discount
            };

            // currentProduct là 1 biến tham thiếu tới product thật nằm trong "products" ở ProductsVM.
            // sửa giá trị trên currentProduct thì giá trị Product thật cũng thay đổi theo !!
            // Nên copy ra 1 biến mới để không ảnh hướng đến Product thật

            EditProductWindow editProductWindow = new EditProductWindow();

            editProductWindow.DataContext = currentProduct;


            editProductWindow.ShowDialog();

            if (currentProduct.Title != coppyCurProduct.Title ||
                currentProduct.Image != coppyCurProduct.Image ||
                currentProduct.Cost != coppyCurProduct.Cost ||
                currentProduct.Price != coppyCurProduct.Price ||
                currentProduct.Stock != coppyCurProduct.Stock ||
                currentProduct.ManufacturingDate != coppyCurProduct.ManufacturingDate ||
                currentProduct.ExpiryDate != coppyCurProduct.ExpiryDate ||
                currentProduct.Discount != coppyCurProduct.Discount)
            {
                // Sau khi cửa sổ Edit đóng thì "currentProduct" đã được update
                DatabaseHelper.UpdateProduct(currentProduct);
            }
        }
    }
}
