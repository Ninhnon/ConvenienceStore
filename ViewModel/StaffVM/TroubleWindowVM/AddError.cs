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
        public ICommand SaveErrorCM { get; set; }

        public void SaveErrorFunc(AddError p)
        {
            if (filepath != null && Title != null && Level != null && Description != null && IsValidData())
            {
                Report trouble = new()
                {
                    Title = Title,
                    Status = Status,
                    Description = Description,
                    RepairCost = 100,
                    SubmittedAt = DateTime.Now,
                    Image = Image,
                    StaffId = CurrentAccount.idAccount,
                };

                IsSaving = false;
                MessageBoxCustom mb = new("Thông báo", "Thêm sự cố thành công", MessageType.Success, MessageButtons.OK);
                DatabaseHelper.ThemErorr(trouble, filepath);
                ListError.Add(trouble);
                mb.ShowDialog();
                MaskName.Visibility = Visibility.Collapsed;
                p.Close();
            }
            else
            {
                MessageBoxCustom mb = new("Cảnh báo", "Vui lòng nhập đủ thông tin!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
        public void Save(AddTrouble p)
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
                SubmittedAt = DateTime.Now,
                StaffId = CurrentAccount.idAccount,
            };
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(p.ImageProduct.ImageSource as BitmapImage));
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
                ListError.Add(newReport);

                DatabaseHelper.InsertReport(newReport);
                p.Close();
            }
        }
    } 
}
