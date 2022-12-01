using ConvenienceStore.Model.Lam;
using ConvenienceStore.Validation.Lam;
using ConvenienceStore.ViewModel.Lam.Helpers;
using ConvenienceStore.Views.Lam.ProductWindow;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Lam.Command.ProductCommand.AddNewProductCommand
{
    public class SaveNewProductCommand : ICommand
    {
        InputInfoVM VM;

        public SaveNewProductCommand(InputInfoVM VM)
        {
            this.VM = VM;
        }

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
            var window = parameter as AddNewProductWindow;

            window.BarcodeErrorMessage.Text = string.Empty;
            window.TitleErrorMessage.Text = string.Empty;
            window.CostErrorMessage.Text = string.Empty;
            window.PriceErrorMessage.Text = string.Empty;
            window.ManufacturingDateErrorMessage.Text = string.Empty;
            window.ExpiryDateErrorMessage.Text = string.Empty;
            window.DiscountErrorMessage.Text = string.Empty;
            window.StockErrorMessage.Text = string.Empty;

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

            if (string.IsNullOrEmpty(window.StockTextBox.Text))
            {
                window.StockErrorMessage.Text = "Chưa nhập số lượng";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(window.StockTextBox.Text, out int Stock))
                {
                    window.StockErrorMessage.Text = "Số lượng không hợp lệ";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(window.ManufacturingDate.Text))
            {
                window.ManufacturingDateErrorMessage.Text = "Chưa chọn NSX";
                isValid = false;
            }

            if (string.IsNullOrEmpty(window.ExpiryDateTextBlock.Text))
            {
                window.ExpiryDateErrorMessage.Text = "Chưa chọn HSD";
                isValid = false;
            }

            if (!isValid) return;
            // Pre Validation Done 


            var newProduct = new Product()
            {
                InputInfoId = VM.SelectedInputInfo.Id,
                Barcode = window.BarcodeTextBox.Text,
                Title = window.TitleTextBox.Text,
                ProductionSite = window.CountryTextBlock.Text,
                Cost = int.Parse(window.CostTextBox.Text),
                Price = int.Parse(window.PriceTextBox.Text),
                Stock = int.Parse(window.StockTextBox.Text),
                ManufacturingDate = (DateTime)window.ManufacturingDate.SelectedDate,
                ExpiryDate = (DateTime)window.ExpiryDate.SelectedDate,
                Discount = int.Parse(window.DiscountTextBox.Text)
            };

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(window.ImageProduct.ImageSource as BitmapSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                newProduct.Image = ms.ToArray();
            }

            ProductValidator validator = new ProductValidator();

            // Note For Me: stops executing a rule as soon as a validator fails
            // See more: https://docs.fluentvalidation.net/en/latest/cascade.html
            validator.RuleLevelCascadeMode = CascadeMode.Stop;

            var results = validator.Validate(newProduct);

            if (results.IsValid == false)
            {
                foreach (var error in results.Errors)
                {
                    if (error.PropertyName == "Barcode")
                        window.BarcodeErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Title")
                        window.TitleErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Cost")
                        window.CostErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Price")
                        window.PriceErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Discount")
                        window.DiscountErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Stock")
                        window.StockErrorMessage.Text = error.ErrorMessage;
                }
                return;
            }

            VM.products.Add(newProduct);
            if (newProduct.Barcode.Contains(VM.SearchContent))
            {
                VM.ObservableProducts.Add(newProduct);
            }
            DatabaseHelper.InsertProduct(newProduct);
            window.Close();
        }
    }
}
