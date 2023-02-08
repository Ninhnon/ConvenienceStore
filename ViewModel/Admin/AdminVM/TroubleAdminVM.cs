using ConvenienceStore.Model;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Utils.Validation;
using ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand.DeleteInputInfoCommand;
using ConvenienceStore.ViewModel.StaffVM;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Admin.TroubleWindow;
using ConvenienceStore.Views.Staff;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    class TroubleAdminVM : StaffVM.BaseViewModel
    {
        private string curAccountName;

        public string CurAccountName
        {
            get { return curAccountName; }
            set
            {
                curAccountName = value;
                OnPropertyChanged("CurAccountName");
            }
        }

        public List<string> Statuses { get; set; }
        public List<Report> Reports { get; set; }
        public ObservableCollection<Report> ObservableReports { get; set; }

        private int isDesc;

        public int IsDesc
        {

            get { return isDesc; }
            set
            {
                isDesc = value;
                OnPropertyChanged("IsDesc");

                OrderBySubmittedAt();
            }
        }


        private string selectedStatus;
        public string SelectedStatus
        {

            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged("selectedStatus");

                if (selectedStatus != null)
                {
                    SetReportsCoresspondManager();
                }
            }
        }

        private Report selectedReport;

        public Report SelectedReport
        {
            get { return selectedReport; }
            set
            {
                selectedReport = value;
                OnPropertyChanged("SelectedReport");
            }
        }

        private Report deletedReport;

        public Report DeletedReport
        {
            get { return deletedReport; }
            set
            {
                deletedReport = value;
                OnPropertyChanged("DeletedReport");
            }
        }

        public ICommand CancelCM { get; set; }
        //public ICommand FirstLoadCM { get; set; }
        public ICommand FilterListErrorCommand { get; set; }
        public ICommand LoadDetailWindowCM { get; set; }
        public ICommand OpenAddErrorCommand { get; set; }
        public ICommand MaskNameCM { get; set; }
        public ICommand UploadImageCM { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand SaveNewTroubleCommand { get; set; }
        public ICommand UpdateReportButtonCommand { get; set; }
        public Report tmpReport { get; set; }
        public static Grid MaskName { get; set; }
        // Command

        //public OpenReportCommand OpenReportCommand { get; set; }
        public OpenAlertDialog OpenAlertDialog { get; set; }
        //public DeleteReportCommand DeleteReportCommand { get; set; }
        public TroubleAdminVM()
        {
            CurAccountName = CurrentAccount.Name;

            Reports = DatabaseHelper.FetchingReportData();
            ObservableReports = new ObservableCollection<Report>();
            
            IsDesc = 0;
            Statuses = new List<string> { "Chờ tiếp nhận", "Đang giải quyết", "Đã giải quyết", "Đã hủy" };
            selectedStatus = Statuses[0];
            for (int i = 0; i < Reports.Count; ++i)
            {
                if (Reports[i].Status == selectedStatus)
                {
                    ObservableReports.Add(Reports[i]);
                }
            }
            UpdateReportButtonCommand = new RelayCommand<EditTrouble>((p) => { return true; }, (p) =>
            {
                tmpReport = SelectedReport;
                Update(p);
                //MaskName.Visibility = Visibility.Collapsed;
            });
            LoadEditErrorCM = new RelayCommand<Report>((p) => { return true; }, (p) =>
            {
                SelectedReport = p;
                tmpReport = SelectedReport;
                EditTrouble w1 = new();
                LoadEditError(w1);
                //MaskName.Visibility = Visibility.Visible;
                w1.ShowDialog();
            });
            MaskNameCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                MaskName = p;
            });
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                //MaskName.Visibility = Visibility.Collapsed;
                p.Close();
            });
            MouseMoveCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
            {
                Window w = Window.GetWindow(p);
                w?.DragMove();
            });
            //CreateReportButtonCommand = new CreateReportButtonCommand(this);
            //OpenAlertDialog = new OpenAlertDialog(this);
            //DeleteReportCommand = new DeleteReportCommand(this);
        }


        public void SetReportsCoresspondManager()
        {
            ObservableReports.Clear();

            if (isDesc == 1)
            {
                for (int i = Reports.Count - 1; i >= 0; --i)
                {
                    if (Reports[i].Status == selectedStatus)
                    {
                        ObservableReports.Add(Reports[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Reports.Count; ++i)
                {
                    if (Reports[i].Status == selectedStatus)
                    {
                        ObservableReports.Add(Reports[i]);
                    }
                }
            }
        }

        public void OrderBySubmittedAt()
        {
            if (isDesc == 1)
            {
                var descReports = ObservableReports.OrderByDescending(e => e.SubmittedAt).ToList();
                ObservableReports.Clear();
                for (int i = 0; i < descReports.Count; ++i)
                {
                    ObservableReports.Add(descReports[i]);
                }
            }
            else
            {
                var ascReports = ObservableReports.OrderBy(e => e.SubmittedAt).ToList();
                ObservableReports.Clear();
                for (int i = 0; i < ascReports.Count; ++i)
                {
                    ObservableReports.Add(ascReports[i]);
                }
            }

        }
        public ICommand LoadEditErrorCM { get; set; }
        public ICommand UpdateErrorCM { get; set; }

        private string troubleID;
        public string TroubleID
        {
            get { return troubleID; }
            set { troubleID = value; }
        }
        public string Description { get; set; }
        public void LoadEditError(EditTrouble w1)
        {
            w1.CostTextBox.Text = tmpReport.RepairCost.ToString();
            w1.StaffName.Text = DatabaseHelper.GetName(tmpReport.StaffId);
            w1.cbxStatus.Text = tmpReport.Status;
            w1.submitdate.Text = tmpReport.SubmittedAt.ToShortDateString();
            if (tmpReport.StartDate.HasValue) w1.startdate.Text = String.Format("{0:dd/MM/yyyy}", tmpReport.StartDate);
            if (tmpReport.FinishDate.HasValue) w1.finishdate.Text = String.Format("{0:dd/MM/yyyy}", tmpReport.FinishDate);
            Description = tmpReport.Description;
        }
        public void Update(EditTrouble p)
        {
            p.TitleErrorMessage.Text = string.Empty;
            p.CostErrorMessage.Text = string.Empty;
            p.startErrorMessage.Text = string.Empty;
            p.submitErrorMessage.Text = string.Empty;
            p.finishErrorMessage.Text = string.Empty;

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

            if (p.StartDate.SelectedDate.HasValue && p.FinishDate.SelectedDate.HasValue && p.StartDate.SelectedDate > p.FinishDate.SelectedDate)
            {
                p.startErrorMessage.Text = "Ngày bắt đầu phải bé hơn ngày kết thúc";
                isValid = false;
            }
            if (p.FinishDate.SelectedDate.HasValue && p.FinishDate.SelectedDate < SelectedReport.SubmittedAt)
            {
                p.finishErrorMessage.Text = "Ngày kết thúc phải lớn hơn ngày báo cáo";
                isValid = false;
            }
            if (p.StartDate.SelectedDate.HasValue && p.StartDate.SelectedDate < SelectedReport.SubmittedAt)
            {
                p.startErrorMessage.Text = "Ngày bắt đầu phải lớn hơn ngày báo cáo";
                isValid = false;
            }
            if (!p.StartDate.SelectedDate.HasValue && (p.cbxStatus.Text == "Đang giải quyết" || p.cbxStatus.Text == "Đã giải quyết"))
            {
                p.startErrorMessage.Text = "Chưa nhập ngày bắt đầu";
                isValid = false;
            }
            if (!p.FinishDate.SelectedDate.HasValue && p.cbxStatus.Text == "Đã giải quyết")
            {
                p.finishErrorMessage.Text = "Chưa nhập ngày kết thúc";
                isValid = false;
            }
            if (!isValid) return;
            // Pre Validation Done 
            var t = SelectedReport.Image;
            var newReport = new Report()
            {
                Title = p.TitleTextBox.Text,
                Status = p.cbxStatus.Text,
                Description = p.cbxDecription.Text,
                SubmittedAt = tmpReport.SubmittedAt,
                RepairCost = int.Parse(p.CostTextBox.Text),
                StaffId = CurrentAccount.idAccount,
                Id = tmpReport.Id,
            };
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapSource src = (BitmapSource)p.ImageProduct.ImageSource;
            encoder.Frames.Add(BitmapFrame.Create(src));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                newReport.Image = ms.ToArray();
            }
            if (!p.StartDate.SelectedDate.HasValue || p.cbxStatus.Text == "Chờ tiếp nhận")
                newReport.StartDate = null;
            else newReport.StartDate = p.StartDate.SelectedDate;
            if (!p.FinishDate.SelectedDate.HasValue || p.cbxStatus.Text == "Chờ tiếp nhận" || p.cbxStatus.Text == "Đang giải quyết")
                newReport.FinishDate = null;
            else newReport.FinishDate = p.FinishDate.SelectedDate;
            //}
            //else
            //newReport.Image = SelectedReport.Image;
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
            // Ảnh lấy từ datasource có vấn đề khác dữ liệu với trong sql
            if (tmpReport.Title != newReport.Title ||
                tmpReport.Image != newReport.Image ||
                tmpReport.RepairCost != newReport.RepairCost ||
                tmpReport.Description != newReport.Description ||
                tmpReport.StartDate != newReport.StartDate ||
                tmpReport.FinishDate != newReport.FinishDate)
            {
                DatabaseHelper.UpdateReportAD(newReport);
                for (int i = 0; i < Reports.Count; i++)
                {
                    if (Reports[i].Id == newReport.Id)
                    {
                        Reports[i] = newReport;
                        break;
                    }
                }
                SetReportsCoresspondManager();
            }
            p.Close();
        }
    }
}
