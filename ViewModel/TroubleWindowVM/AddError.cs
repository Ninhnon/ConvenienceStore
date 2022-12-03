using ConvenienceStore.Model;
using ConvenienceStore.Utils;
using ConvenienceStore.ViewModel.Lam.Helpers;
using ConvenienceStore.ViewModel.MainBase;
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
    public partial class TroublePageViewModel : MainBase.BaseViewModel
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
                Random random = new();
                string idd = random.Next(100).ToString();
                Report trouble = new()
                {
                    Id = idd,
                    Title = Title,
                    Level = Level.Content.ToString(),
                    Status = Status,
                    Description = Description,
                    RepairCost = 100,
                    SubmittedAt = DateTime.Now,
                    Image = Image,
                    StaffId = MainStaffViewModel.StaffCurrent.Id,
                };

                IsSaving = false;
                MessageBoxCustom mb = new("Thông báo", "Thêm sự cố thành công", MessageType.Success, MessageButtons.OK);
                ThemErorr(trouble);
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
        readonly string insertErorrs = "insert into Report(Id, Title, Description, Status, RepairCost, SubmittedAt, StaffId, Level, Image) select N'{0}',N'{1}',N'{2}',N'Chờ tiếp nhận',{3},N'{4}',N'{5}',N'{6}', BulkColumn FROM Openrowset(Bulk N'{7}', Single_Blob) as img";
        public void ThemErorr(Report t)
        {
            
            var strCmd = string.Format(insertErorrs, t.Id, t.Title, t.Description, t.RepairCost, t.SubmittedAt, t.StaffId, t.Level, filepath);
            DatabaseHelper.sqlCon.Open();
            SqlCommand cmd = new(strCmd, DatabaseHelper.sqlCon);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            DatabaseHelper.sqlCon.Close();
        }
    }
}
