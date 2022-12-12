
using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Staff.TroubleWindow;
using FluentValidation;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : StaffVM.BaseViewModel
    {
        public ICommand LoadEditErrorCM { get; set; }
        public ICommand UpdateErrorCM { get; set; }

        private string troubleID;
        public string TroubleID
        {
            get { return troubleID; }
            set { troubleID = value; }
        }

        public void LoadEditError(EditTrouble w1)
        {
            IsImageChanged = false;
            w1.CostTextBox.Text = SelectedItem.RepairCost.ToString();
            w1.StaffName.Text = DatabaseHelper.GetName(SelectedItem.StaffId);
            w1.cbxStatus.Text = SelectedItem.Status;
            w1.submitdate.Text = SelectedItem.SubmittedAt.ToShortDateString();
            Description = SelectedItem.Description;
            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.StreamSource = new MemoryStream(SelectedItem.Image);
            //image.EndInit();
            //ImageSource = image; 
            //ImageSource = new BitmapImage(new System.Uri(filepath)) ;
        }
        public void Update(EditTrouble p)
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
                SubmittedAt = DateTime.Now,
                StaffId = CurrentAccount.idAccount,
            };
            if (p.ImageReport.ImageSource != null)
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(p.ImageReport.ImageSource as BitmapImage));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    newReport.Image = ms.ToArray();
                }
            }
            else newReport.Image = SelectedItem.Image;
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

                    if (error.PropertyName == "RepairCost")
                        p.CostErrorMessage.Text = error.ErrorMessage;
                    return;
                }
            }
            if (tmpReport.Title != newReport.Title ||
                tmpReport.Image != newReport.Image ||
                tmpReport.RepairCost != newReport.RepairCost)
            {
                // Sau khi cửa sổ Edit đóng thì "currentProduct" đã được update
                DatabaseHelper.UpdateReport(newReport);
                ListError.Clear();
                danhsach = DatabaseHelper.FetchingReportData();
                ListError = new ObservableCollection<Report>(danhsach);
            }
            p.Close();
        }
        public void UpdateErrorFunc(EditError p)
        {
            if (TroubleID != null && IsValidData())
            {

                Report tb = new Report
                {
                    Title = Title,
                    Description = Description,
                    StaffId = CurrentAccount.idAccount,

                };

                if (IsImageChanged)
                {
                    //Task<string> uploadImage = CloudinaryService.Ins.UploadImage(filepath);
                    //if (SelectedItem.Image != null)
                    //{
                    //    await CloudinaryService.Ins.DeleteImage(SelectedItem.Image);
                    //}

                    //tb.Image = await uploadImage;


                    //if (tb.Image is null)
                    //{
                    //    MessageBoxCustom mb = new MessageBoxCustom("Thông báo", "Lỗi phát sinh trong quá trình lưu ảnh. Vui lòng thử lại", MessageType.Error, MessageButtons.OK);
                    //    return;
                    //}
                }
                else
                {
                    tb.Image = SelectedItem.Image;
                }

                //(bool successUpdateTB, string messageFromUpdateTB) = await TroubleService.Ins.UpdateTroubleInfo(tb);

                //if (successUpdateTB)
                //{
                //    isSaving = false;

                MessageBoxCustom mb = new MessageBoxCustom("", "Cập nhật thành công!", MessageType.Success, MessageButtons.OK);
                danhsach = DatabaseHelper.FetchingReportData();

                ListError = new ObservableCollection<Report>(danhsach);
                mb.ShowDialog();
                MaskName.Visibility = Visibility.Collapsed;
                p.Close();

                //    await GetData();


                //    p.Close();
                //}
                //else
                //{
                //    MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                //    mb.ShowDialog();
                //}
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Vui lòng nhập đủ thông tin", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }

    }
}
