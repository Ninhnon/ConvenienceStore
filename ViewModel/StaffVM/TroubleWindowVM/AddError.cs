using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Staff.ProductWindow;
using ConvenienceStore.Views.Staff.TroubleWindow;
using FluentValidation;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConvenienceStore.ViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : BaseViewModel
    {
        private DateTime getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return getCurrentDate; }
            set { getCurrentDate = value; OnPropertyChanged(); }
        }
        public void Save(AddTrouble p, Snackbar TroubleSnackbar)
        {
            p.TitleErrorMessage.Text = string.Empty;
            p.CostErrorMessage.Text = string.Empty;
            p.ImageProductErrorMessage.Text = string.Empty;

            // Pre Validation
            bool isValid = true;

            if (string.IsNullOrEmpty(p.CostTextBox.Text))
            {
                p.CostErrorMessage.Text = "Chưa nhập Chi phí dự kiến";
                isValid = false;
            }
            else
            {
                if (!int.TryParse(p.CostTextBox.Text, out int a))
                {
                    p.CostErrorMessage.Text = "Chi phí dự kiến không hợp lệ";
                    isValid = false;
                }
            }

            // Check xem manager đã upload ảnh lên chưa
            if (p.ImageProduct.ImageSource == null)
            {
                p.ImageProductErrorMessage.Text = "Chưa tải ảnh lên";
                isValid = false;
            }

            if (!isValid) return;
            // Pre Validation Done 


            var newReport = new Report()
            {
                Title = p.TitleTextBox.Text,
                Status = p.cbxStatus.Text,
                Description = p.cbxDecription.Text,
                RepairCost = int.Parse(p.CostTextBox.Text),
                SubmittedAt = DateTime.Today,
                StaffId = CurrentAccount.idAccount,
            };
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapSource src = (BitmapSource)p.ImageProduct.ImageSource;
            encoder.Frames.Add(BitmapFrame.Create(src));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                newReport.Image = ms.ToArray();
            }
            ReportValidator validator = new ReportValidator();

            // Note For Me: stops executing a rule as soon as a validator fails
            // See more: https://docs.fluentvalidation.net/en/latest/cascade.html
            validator.RuleLevelCascadeMode = CascadeMode.Stop;

            var results = validator.Validate(newReport);

            if (results.IsValid == false)
            {
                foreach (var error in results.Errors)
                {

                    if (error.PropertyName == "Title")
                        p.TitleErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "Cost")
                        p.CostErrorMessage.Text = error.ErrorMessage;
                    return;
                }
            }
            else
            {
                danhsach.Add(newReport);
                ListError.Add(newReport);
                DatabaseHelper.InsertReport(newReport);
                TroubleSnackbar.MessageQueue?.Enqueue($"Đã tạo Sự cố hàng ngày {newReport.Title}", null, null, null, false, true, TimeSpan.FromSeconds(1));
                p.Close();
            }
        }
    } 
}
