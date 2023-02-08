
using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.Views.Staff.TroubleWindow;
using FluentValidation;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : StaffVM.BaseViewModel
    {
        public ICommand LoadEditErrorCM { get; set; }

        private string troubleID;
        public string TroubleID
        {
            get { return troubleID; }
            set { troubleID = value; }
        }

        public void LoadEditError(EditTrouble w1)
        {
            w1.CostTextBox.Text = SelectedItem.RepairCost.ToString();
            w1.StaffName.Text = CurrentAccount.Name;
            w1.cbxStatus.Text = SelectedItem.Status;
            w1.submitdate.Text = SelectedItem.SubmittedAt.ToShortDateString();
            w1.cbxDecription.Text = SelectedItem.Description;
            tmpReport = SelectedItem;
        }
        public void Update(EditTrouble p, Report tmpReport, Snackbar TroubleSnackbar)
        {
            p.TitleErrorMessage.Text = string.Empty;
            p.CostErrorMessage.Text = string.Empty;
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


            if (!isValid) return;
            // Pre Validation Done 

            var newReport = new Report()
            {
                Title = p.TitleTextBox.Text,
                Status = p.cbxStatus.Text,
                Description = p.cbxDecription.Text,
                RepairCost = int.Parse(p.CostTextBox.Text),
                SubmittedAt = Se,
                StaffId = CurrentAccount.idAccount,
                Id = Id,
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
            validator.RuleLevelCascadeMode = CascadeMode.Stop;

            var results = validator.Validate(newReport);

            if (results.IsValid == false)
            {
                foreach (var error in results.Errors)
                {

                    if (error.PropertyName == "Title")
                        p.TitleErrorMessage.Text = error.ErrorMessage;

                    if (error.PropertyName == "RepairCost")
                        p.CostErrorMessage.Text = error.ErrorMessage;
                    return;
                }
            }
            if (tmpReport.Title != newReport.Title ||
                tmpReport.Image != newReport.Image ||
                tmpReport.Description != newReport.Description ||
                tmpReport.RepairCost != newReport.RepairCost
                )
            {
                // Sau khi cửa sổ Edit đóng thì "currentProduct" đã được update
                DatabaseHelper.UpdateReport(newReport);
                for (int i = 0; i < ListError.Count; i++)
                {
                    if (ListError[i].Id == newReport.Id)
                    {
                        ListError[i] = newReport;
                        break;
                    }
                }

                TroubleSnackbar.MessageQueue?.Enqueue($"Đã cập nhật Sự cố \"{tmpReport.Title}\"", null, null, null, false, true, TimeSpan.FromSeconds(1));
            }
            p.Close();
        }
    }
}
