using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.Views.Admin.ProductWindow;
using FluentValidation;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.EditProductCommand
{
    public class UpdateProductButtonCommand : ICommand
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
            EditProductWindow window = parameter as EditProductWindow;

            window.TitleErrorMessage.Text = string.Empty;
            window.CostErrorMessage.Text = string.Empty;
            window.PriceErrorMessage.Text = string.Empty;
            window.ManufacturingDateErrorMessage.Text = string.Empty;
            window.ExpiryDateErrorMessage.Text = string.Empty;
            window.DiscountErrorMessage.Text = string.Empty;

            // Pre Validation
            bool isValid = true;

            if (string.IsNullOrEmpty(window.CostTextBox.Text))
            {
                window.CostErrorMessage.Text = "Chưa nhập Giá nhập";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(window.CostTextBox.Text, out int Stock))
                {
                    window.CostErrorMessage.Text = "Giá nhập không hợp lệ";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(window.PriceTextBox.Text))
            {
                window.PriceErrorMessage.Text = "Chưa nhập Giá bán";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(window.PriceTextBox.Text, out int Stock))
                {
                    window.PriceErrorMessage.Text = "Giá bán không hợp lệ";
                    isValid = false;
                }
            }

            if (window.ManufacturingDate.SelectedDate.HasValue && window.ExpiryDate.SelectedDate.HasValue && window.ManufacturingDate.SelectedDate >= window.ExpiryDate.SelectedDate)
            {
                window.ManufacturingDateErrorMessage.Text = "NSX phải bé hơn HSD";
                isValid = false;
            }

            if (!isValid) return;

            var newProduct = new Product()
            {
                Barcode = window.BarcodeTextBlock.Text,
                Title = window.TitleTextBox.Text,
                Type = window.TypeComboBox.Text,
                Cost = int.Parse(window.CostTextBox.Text),
                Price = int.Parse(window.PriceTextBox.Text),
                ManufacturingDate = (DateTime)window.ManufacturingDate.SelectedDate,
                ExpiryDate = (DateTime)window.ExpiryDate.SelectedDate,
                Discount = double.Parse(window.DiscountTextBox.Text),
            };

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(window.ImageProduct.ImageSource as BitmapSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                newProduct.Image = ms.ToArray();
            }

            // Start Validation
            ProductValidator validator = new ProductValidator
            {
                // Note For Me: stops executing a rule as soon as a validator fails
                // See more: https://docs.fluentvalidation.net/en/latest/cascade.html
                RuleLevelCascadeMode = CascadeMode.Stop
            };

            var results = validator.Validate(newProduct);

            if (results.IsValid == false)
            {
                foreach (var error in results.Errors)
                {
                    if (error.PropertyName == "Title")
                        window.TitleErrorMessage.Text = error.ErrorMessage;

                    //if (error.PropertyName == "ImageUrl")
                    //    window.ImageUrlErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Cost")
                        window.CostErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Price")
                        window.PriceErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Discount")
                        window.DiscountErrorMessage.Text = error.ErrorMessage;
                }
                return;
            }

            // Update on Program
            // Note for me: Nên nhớ C# là truyền tham chiếu

            var curProduct = window.DataContext as Product;

            curProduct.Title = newProduct.Title;
            curProduct.Type = newProduct.Type;
            curProduct.Image = newProduct.Image;
            curProduct.Cost = newProduct.Cost;
            curProduct.Price = newProduct.Price;
            curProduct.ManufacturingDate = newProduct.ManufacturingDate;
            curProduct.ExpiryDate = newProduct.ExpiryDate;
            curProduct.Discount = newProduct.Discount;

            window.Close();
        }
    }
}
