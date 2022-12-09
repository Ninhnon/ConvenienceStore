using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using ConvenienceStore.Views;
using System.Windows;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Model;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class ReportViewModel : BaseViewModel
    {
        public ReportViewModel()
        {
            Hello = "Hello, " + CurrentAccount.Name;
            InitColumnChartTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartToday(parameter));
            InitColumnChartMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartMonth(parameter));
            InitColumnChartYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartYear(parameter));
            InitLineChartCommand = new RelayCommand<ReportView>(parameter => true, parameter => InitLineChart(parameter));
            LoadCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadDefaultChart(parameter));


        }


        private string _hello;
        public string Hello { get => _hello; set { _hello = value; OnPropertyChanged(); } }
        // Doanh thu tháng này 
        private string thisMonth;
        public string ThisMonth { get => thisMonth; set { thisMonth = value; OnPropertyChanged(); } }
        private string thisMonth1;
        public string ThisMonth1 { get => thisMonth1; set { thisMonth1 = value; OnPropertyChanged(); } }

        private string thisMonthRevenue = "0 đồng";
        public string ThisMonthRevenue { get => thisMonthRevenue; set { thisMonthRevenue = value; OnPropertyChanged(); } }

        //Doanh thu tháng này

        //Số đơn bán được

        private string numOfSoldBill;
        public string NumOfSoldBill { get => numOfSoldBill; set { numOfSoldBill = value; OnPropertyChanged(); } }




        //Số đơn bán được


        //Phần trăm tăng trưởng
        private string increasingPercent = "0%";
        public string IncreasingPercent { get => increasingPercent; set { increasingPercent = value; OnPropertyChanged(); } }


        //Phần trăm tăng trưởng


        //Constructor

        //Định nghĩa command gộp cả 3 miếng
        public ICommand LoadCommand { get; set; }
        public void LoadDefaultChart(HomeView homeWindow)
        {
            ThisMonth = "This month Profit " + DateTime.Now.ToString("MM/yyyy");
            ThisMonth1 = DateTime.Now.ToString("MM/yyyy");
            string currentDay = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();
            string lastMonth = (int.Parse(currentMonth) - 1).ToString();
            string currentYear = DateTime.Now.Year.ToString();
            ThisMonthRevenue = string.Format("{0:n0}", ReportDAL.Instance.QueryRevenueInMonth(currentMonth, currentYear)).ToString() + " đồng";
            try
            {
                double res = 0;
                if (currentMonth != "1")
                {
                    res = ReportDAL.Instance.QueryRevenueInMonth(currentMonth, currentYear) / (double)ReportDAL.Instance.QueryRevenueInMonth(lastMonth, currentYear) * 100;
                }
                else
                {
                    res = ReportDAL.Instance.QueryRevenueInMonth("1", currentYear) / (double)ReportDAL.Instance.QueryRevenueInMonth("12", (int.Parse(currentYear) - 1).ToString()) * 100;
                }
                IncreasingPercent = "Increased by " + Math.Round(res, 2).ToString() + "%";
            }
            catch
            {
                IncreasingPercent = "100%";
            }
            NumOfSoldBill = ReportDAL.Instance.QueryRevenueNumOfSoldBillInMonth(currentMonth, currentYear).ToString() + " đơn";




        }
        //Định nghĩa command gộp cả 3 miếng

        //Chu kỳ và thời gian select
        private ObservableCollection<string> itemSourceTime = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceTime { get => itemSourceTime; set { itemSourceTime = value; OnPropertyChanged(); } }


        //Chu kỳ và thời gian select 




        //Khởi tạo biểu đồ cột
        private string axisXTitle;
        public string AxisXTitle { get => axisXTitle; set { axisXTitle = value; OnPropertyChanged(); } }
        private string axisYTitle;
        public string AxisYTitle { get => axisYTitle; set { axisYTitle = value; OnPropertyChanged(); } }
        private SeriesCollection columnSeriesCollection;
        public SeriesCollection ColumnSeriesCollection { get => columnSeriesCollection; set { columnSeriesCollection = value; OnPropertyChanged(); } }
        private string[] labels;
        public string[] Labels { get => labels; set { labels = value; OnPropertyChanged(); } }
        private Func<double, string> formatter;
        public Func<double, string> Formatter { get => formatter; set { formatter = value; OnPropertyChanged(); } }
        public ICommand InitColumnChartTodayCommand { get; set; }
        public ICommand InitColumnChartMonthCommand { get; set; }
        public ICommand InitColumnChartYearCommand { get; set; }


        //Khởi tạo biểu đồ cột


        //Định nghĩa command khởi tạo biểu đồ cột
        public void InitColumnChartToday(HomeView homeWindow)
        {

            AxisXTitle = "Ngày";
            string today = DateTime.Now.Day.ToString();
            string thismonth = DateTime.Now.Month.ToString();
            string thisyear = DateTime.Now.Year.ToString();
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);
            gradient.GradientStops.Add(new GradientStop((Color)new ColorConverter().ConvertFrom("#B397E2"), 0.4));
            string currentYear = DateTime.Now.Year.ToString();
            ColumnSeriesCollection = new SeriesCollection
                    {
                        new ColumnSeries //cột doanh thu
                        {
                            Title = "Doanh thu",
                            Fill = gradient,
                            Values = ReportDAL.Instance.QueryRevenueByDay(today,thismonth,thisyear),
                        }
                       /* new ColumnSeries //cột chi phí
                        {
                            Title = "Chi phí",
                            Fill = (Brush)new BrushConverter().ConvertFrom("#FFF44336"),
                            Values = ReportDAL.Instance.QueryOutcomeByMonth(selectedMonth, currentYear),
                        }
                       */
                    };
            List<string> res = new List<string>();
            res.Add(today);

            Labels = res.ToArray();
            Formatter = value => string.Format("{0:N0}", value); //Format dấu ,

        }
        public void InitColumnChartMonth(HomeView homeWindow)
        {

            AxisXTitle = "Ngày";

            string selectedMonth = DateTime.Now.Month.ToString();
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);
            gradient.GradientStops.Add(new GradientStop((Color)new ColorConverter().ConvertFrom("#B397E2"), 0.4));
            string currentYear = DateTime.Now.Year.ToString();
            ColumnSeriesCollection = new SeriesCollection
                    {
                        new ColumnSeries //cột doanh thu
                        {
                            Title = "Doanh thu",
                            Fill = gradient,
                            Values = ReportDAL.Instance.QueryRevenueByMonth(selectedMonth, currentYear),
                        }
                       /* new ColumnSeries //cột chi phí
                        {
                            Title = "Chi phí",
                            Fill = (Brush)new BrushConverter().ConvertFrom("#FFF44336"),
                            Values = ReportDAL.Instance.QueryOutcomeByMonth(selectedMonth, currentYear),
                        }
                       */
                    };
            Labels = ReportDAL.Instance.QueryDayInMonth(selectedMonth, currentYear);
            Formatter = value => string.Format("{0:N0}", value); //Format dấu ,
            /*
                  
       */

        }
        public void InitColumnChartYear(HomeView homeWindow)
        {

            AxisXTitle = "Tháng";

            string selectedYear = DateTime.Now.Year.ToString();
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);
            gradient.GradientStops.Add(new GradientStop((Color)new ColorConverter().ConvertFrom("#B397E2"), 0.4));
            ColumnSeriesCollection = new SeriesCollection
                      {
                          new ColumnSeries
                          {
                              Title = "Doanh thu",
                              Fill = gradient,
                              Values = ReportDAL.Instance.QueryRevenueByYear(selectedYear),
                          }
            };
            /*
            new ColumnSeries
            {
                Title = "Chi phí",
                Fill = (Brush)new BrushConverter().ConvertFrom("#FFF44336"),
                Values = ReportDAL.Instance.QueryOutcomeByYear(selectedYear)
            }

        };
            */
            Labels = ReportDAL.Instance.QueryMonthInYear(selectedYear);
            Formatter = value => string.Format("{0:N0}", value);


        }



        private SeriesCollection lineSeriesCollection;
        public SeriesCollection LineSeriesCollection { get => lineSeriesCollection; set { lineSeriesCollection = value; OnPropertyChanged(); } }


        public ICommand InitLineChartCommand { get; set; }
        private string[] lineLabels;
        public string[] LineLabels { get => lineLabels; set { lineLabels = value; OnPropertyChanged(); } }

        public void InitLineChart(ReportView parameter)
        {



            string selectedYear = DateTime.Now.Year.ToString();

            LineSeriesCollection = new SeriesCollection
                      {
                     new LineSeries
                          {
                              Title="Đơn hàng",
                              Values=new ChartValues<int>{5,7,12,9,10},
                              Stroke=(Brush)new BrushConverter().ConvertFrom("#0000ffff")
                          },
                          new LineSeries
                          {
                              Title = "Doanh thu",
                              PointForeground=(Brush)new BrushConverter().ConvertFrom("#FE6C47"),
                               Stroke=(Brush)new BrushConverter().ConvertFrom("#FE6C47"),
                             Fill = (Brush)new BrushConverter().ConvertFrom("#0000ffff"),
                              Values = ReportDAL.Instance.QueryRevenueByYear(selectedYear),
                          }

                     


        };
            /*
            new ColumnSeries
            {
                Title = "Chi phí",
                Fill = (Brush)new BrushConverter().ConvertFrom("#FFF44336"),
                Values = ReportDAL.Instance.QueryOutcomeByYear(selectedYear)
            }

        };
            */
            LineLabels = ReportDAL.Instance.QueryMonthInYear(selectedYear);
            Formatter = value => string.Format("{0:N0}", value);


        }


    }
}