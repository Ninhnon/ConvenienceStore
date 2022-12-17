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
using ConvenienceStore.Utils.Helpers;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Ink;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class ReportViewModel : BaseViewModel
    {
        public ReportViewModel()
        {
         
            InitColumnChartTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartToday(parameter));
            InitColumnChartMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartMonth(parameter));
            InitColumnChartYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartYear(parameter));
            InitLineChartCommand = new RelayCommand<ReportView>(parameter => true, parameter => InitLineChart(parameter));
            LoadCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadDefaultChart(parameter));
            InitTopSaleCommand = new RelayCommand<ReportView>(parameter => true, parameter => LoadTopSale(parameter));
            InitPieChartCommand = new RelayCommand<ReportView>(parameter => true, parameter => InitPieChart(parameter));

        }


        private string _hello;
        public string Hello { get => _hello; set { _hello = value; OnPropertyChanged(); } }
        // Doanh thu tháng này 
        private string thisMonth;
        public string ThisMonth { get => thisMonth; set { thisMonth = value; OnPropertyChanged(); } }


        private string thisMonth1;
        public string ThisMonth1 { get => thisMonth1; set { thisMonth1 = value; OnPropertyChanged(); } }

        private string thisYear;
        public string ThisYear{ get => thisYear; set { thisYear = value; OnPropertyChanged(); } }

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
            Hello = "Hello, " + CurrentAccount.Name;
         ThisYear= DateTime.Now.ToString("yyyy");
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
            FoodRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryFoodRevenueInMonth(currentMonth,currentYear)) ).ToString() +" VND";
            DrinkRevenue= string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryDrinkRevenueInMonth(currentMonth,currentYear))).ToString()+" VND";
            OtherRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryOtherRevenueInMonth(currentMonth,currentYear))).ToString()+" VND";


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

        public ICommand InitTopSaleCommand { get; set; }
        

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
                       ,
                         new ColumnSeries 
                        {
                            Title = "Đơn hàng",
                      Fill=(Brush)new BrushConverter().ConvertFrom("#E44D26"),
                            Values = ReportDAL.Instance.QueryNumOfSoldBillToday(today,thismonth),
                        }
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
                        },
                         new ColumnSeries
                      {
                          Title="Đơn hàng",
                          Fill=(Brush)new BrushConverter().ConvertFrom("#E44D26"),
                          Values=ReportDAL.Instance.QueryRevenueNumOfSoldBillEachDayInMonth(selectedMonth,currentYear)
        }

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
                            , new ColumnSeries
                      {
                          Title="Đơn hàng",
                          Fill=(Brush)new BrushConverter().ConvertFrom("#E44D26"),
                          Values=ReportDAL.Instance.QueryRevenueNumOfSoldBillInYear(selectedYear)
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


        private string foodRevenue_;
        public string FoodRevenue
        {
            get { return foodRevenue_; }
            set
            {
                foodRevenue_ = value;
                OnPropertyChanged();
            }
            }
        private string drinkRevenue_;
        public string DrinkRevenue
        {
            get { return drinkRevenue_; }
            set
            {
                drinkRevenue_ = value;
                OnPropertyChanged();
            }
        }
        private string otherRevenue_;
        public string OtherRevenue
        {
            get { return otherRevenue_; }
            set
            {
                otherRevenue_ = value;
                OnPropertyChanged();
            }
        }


        private string foodRevenue_1;
        public string FoodRevenue1
        {
            get { return foodRevenue_1; }
            set
            {
                foodRevenue_1 = value;
                OnPropertyChanged();
            }
        }
        private string drinkRevenue_1;
        public string DrinkRevenue1
        {
            get { return drinkRevenue_1; }
            set
            {
                drinkRevenue_1 = value;
                OnPropertyChanged();
            }
        }
        private string otherRevenue_1;
        public string OtherRevenue1
        {
            get { return otherRevenue_1; }
            set
            {
                otherRevenue_1 = value;
                OnPropertyChanged();
            }
        }


        private SeriesCollection lineSeriesCollection;
        public SeriesCollection LineSeriesCollection { get => lineSeriesCollection; set { lineSeriesCollection = value; OnPropertyChanged(); } }
        private SeriesCollection pieSeriesCollection_;
        public SeriesCollection PieSeriesCollection
        {
            get { return pieSeriesCollection_; }
            set { pieSeriesCollection_ = value; OnPropertyChanged(); }
        }

        public ICommand InitLineChartCommand { get; set; }
        private string[] lineLabels;
        public string[] LineLabels { get => lineLabels; set { lineLabels = value; OnPropertyChanged(); } }

       public ICommand InitPieChartCommand { get; set; }


        public void InitPieChart(ReportView parameter)
        {
            string currentMonth = DateTime.Now.Month.ToString();
     
            string currentYear = DateTime.Now.Year.ToString();
            FoodRevenue1 =(ReportDAL.Instance.QueryFoodRevenueInYear( currentYear)).ToString();
            DrinkRevenue1 = (ReportDAL.Instance.QueryDrinkRevenueInYear( currentYear)).ToString();
            OtherRevenue1 = (ReportDAL.Instance.QueryOtherRevenueInYear(currentYear)).ToString();

            ChartValues<long> food = new ChartValues<long>();
            food.Add(long.Parse(FoodRevenue1));
            ChartValues<long> drink = new ChartValues<long>();
            drink.Add(long.Parse(DrinkRevenue1));
            ChartValues<long> other = new ChartValues<long>();
            other.Add(long.Parse(OtherRevenue1));
            PieSeriesCollection = new SeriesCollection()
            {
                new PieSeries
                {
                    Title="Đồ ăn",
            Values= food,
              Fill = (Brush)new BrushConverter().ConvertFrom("#6254F9"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                },
                 new PieSeries
                {
                    Title="Thức uống",
            Values= drink,
              Fill = (Brush)new BrushConverter().ConvertFrom("#FFBE41"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                },
                  new PieSeries
                {
                    Title="Khác",
            Values= other,
              Fill = (Brush)new BrushConverter().ConvertFrom("#DFE931"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                }


            };
        }



        public void InitLineChart(ReportView parameter)
        {



            string selectedYear = DateTime.Now.Year.ToString();

            LineSeriesCollection = new SeriesCollection
                      {
                     new LineSeries
                          {
                              Title="Đơn hàng",
                               PointForeground=(Brush)new BrushConverter().ConvertFrom("#ffffff00"),
                              Values=ReportDAL.Instance.QueryRevenueNumOfSoldBillInYear(selectedYear),
                        
                          },
                          new LineSeries
                          {
                              Title = "Doanh thu",
                              PointForeground=(Brush)new BrushConverter().ConvertFrom("#FE6C47"),
                               Stroke=(Brush)new BrushConverter().ConvertFrom("#FE6C47"),
                             Fill = (Brush)new BrushConverter().ConvertFrom("#FE6C47"),
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
        public BitmapImage ConvertByteToBitmapImage(Byte[] image)
        {
            BitmapImage bi = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            if (image == null)
            {
                return null;
            }
            stream.Write(image, 0, image.Length);
            stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public void LoadTopSale(ReportView parameter)
        {
            string currentMonth = DateTime.Now.Month.ToString();
    
            string currentYear = DateTime.Now.Year.ToString();
            List<Account> accounts = ReportDAL.Instance.QueryTopSaleMonth(currentMonth, currentYear);

            if (accounts.Count==3)
            {
                parameter.top1.Title = accounts[0].Name;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", accounts[0].Tong).ToString()+" VND";
                parameter.top1.avatarUser.Source =ConvertByteToBitmapImage( DatabaseHelper.LoadAvatar(accounts[0].IdAccount));
      
                parameter.top2.Title = accounts[1].Name;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", accounts[1].Tong).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(accounts[1].IdAccount));


                parameter.top3.Title = accounts[2].Name;
                parameter.top3.UpPrice = string.Format("{0:#,##0}", accounts[2].Tong).ToString() + " VND";
                parameter.top3.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(accounts[2].IdAccount));

            }


            if (accounts.Count == 2)
            {
                parameter.top1.Title = accounts[0].Name;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", accounts[0].Tong).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(accounts[0].IdAccount));


                parameter.top2.Title = accounts[1].Name;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", accounts[1].Tong).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(accounts[1].IdAccount));


                parameter.top3.Visibility = Visibility.Hidden;
            }


            if (accounts.Count == 1)
            {
                parameter.top1.Title = accounts[0].Name;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", accounts[0].Tong).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(accounts[0].IdAccount));



                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }

            if (accounts.Count == 0)
            {

                parameter.top1.Visibility = Visibility.Hidden;

                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }



        }



    }
}