
using ConvenienceStore.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConvenienceStore.Views.Staff.TroubleWindow;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Model.Staff;

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

        public void LoadEditError(EditError w1)
        {
            IsImageChanged = false;
            Title = SelectedItem.Title.ToString();
            w1.staffname.Text = MainStaffViewModel.StaffCurrent.Name;
            w1.cbxStatusError.Text = SelectedItem.Status;
            w1.submitdate.Text = SelectedItem.SubmittedAt.ToShortDateString();
            Level.Content = SelectedItem.Level;
            Description = SelectedItem.Description;
            TroubleID = SelectedItem.Id;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(SelectedItem.Image);
            image.EndInit();
            ImageSource = image;

            //ImageSource = new BitmapImage(new System.Uri(filepath)) ;
        }
        public void UpdateErrorFunc(EditError p)
        {
            if (TroubleID != null && IsValidData())
            {

                Report tb = new Report
                {
                    Id = TroubleID,
                    Title = Title,
                    Level = Level.Content.ToString(),
                    Description = Description,
                    StaffId = MainStaffViewModel.StaffCurrent.Id,

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
