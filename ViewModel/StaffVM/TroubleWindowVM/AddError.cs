using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Staff.TroubleWindow;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.TroubleWindowVM
{
    public partial class TroublePageViewModel : StaffVM.BaseViewModel
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
                    Level = Level.Content.ToString(),
                    Status = Status,
                    Description = Description,
                    RepairCost = 100,
                    SubmittedAt = DateTime.Now,
                    Image = Image,
                    StaffId = CurrentAccount.idAccount,
                };

                IsSaving = false;
                MessageBoxCustom mb = new("Thông báo", "Thêm sự cố thành công", MessageType.Success, MessageButtons.OK);
                DatabaseHelper.ThemErorr(trouble,filepath);
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
    }
}
