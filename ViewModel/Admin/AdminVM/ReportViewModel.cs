using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views.Admin;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class ReportViewModel : BaseViewModel
    {
        public ReportViewModel()
        {

            InitColumnChartTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartToday(parameter));
            InitColumnChartMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartMonth(parameter));
            InitColumnChartYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitColumnChartYear(parameter));
            LoadTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadToday(parameter));
            LoadMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadMonth(parameter));

            LoadYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadYear(parameter));

            InitTopSaleTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadTopSaleToday(parameter));
            InitTopSaleMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadTopSaleMonth(parameter));
            InitTopSaleYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => LoadTopSaleYear(parameter));

            InitPieChartTodayCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitPieChartToday(parameter));
            InitPieChartMonthCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitPieChartMonth(parameter));
            InitPieChartYearCommand = new RelayCommand<HomeView>(parameter => true, parameter => InitPieChartYear(parameter));
        }



        private string thisYear;
        public string ThisYear { get => thisYear; set { thisYear = value; OnPropertyChanged(); } }

        private string revenue = "0 đồng";
        public string Revenue { get => revenue; set { revenue = value; OnPropertyChanged(); } }

        private string consignment = "0 đồng";
        public string Consignment { get => consignment; set { consignment = value; OnPropertyChanged(); } }
        private string repairCost = "0 đồng";
        public string RepairCost { get => repairCost; set { repairCost = value; OnPropertyChanged(); } }

        //Doanh thu tháng này

        //Số đơn bán được

        private string salary = "0 đồng";
        public string Salary { get => salary; set { salary = value; OnPropertyChanged(); } }




        //Số đơn bán được







        //Constructor

        //Định nghĩa command gộp cả 3 miếng
        public ICommand LoadTodayCommand { get; set; }
        public ICommand LoadMonthCommand { get; set; }
        public ICommand LoadYearCommand { get; set; }
        public void LoadToday(HomeView homeWindow)
        {

            ThisYear = DateTime.Now.ToString("yyyy");


            string currentDay = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            Revenue = string.Format("{0:n0}", ReportDAL.Instance.QueryRevenueInToday(currentDay, currentMonth, currentYear)).ToString() + " đồng";
            Consignment = string.Format("{0:n0}", ReportDAL.Instance.QueryConsignmentInToday(currentDay, currentMonth, currentYear)).ToString() + " đồng";
            RepairCost = string.Format("{0:n0}", ReportDAL.Instance.QueryRepairCostToday(currentDay, currentMonth, currentYear)).ToString() + " đồng";

            Salary = ReportDAL.Instance.QuerySalaryToday((int.Parse(currentDay) + 1).ToString(), currentMonth, currentYear).ToString() + " đồng";



        }

        public void LoadMonth(HomeView homeWindow)
        {

            ThisYear = DateTime.Now.ToString("yyyy");


            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            Revenue = string.Format("{0:n0}", ReportDAL.Instance.QueryRevenueInMonth(currentMonth, currentYear)).ToString() + " đồng";
            Consignment = string.Format("{0:n0}", ReportDAL.Instance.QueryConsignmentInMonth(currentMonth, currentYear)).ToString() + " đồng";
            RepairCost = string.Format("{0:n0}", ReportDAL.Instance.QueryRepairCostMonth(currentMonth, currentYear)).ToString() + " đồng";

            Salary = ReportDAL.Instance.QuerySalaryMonth(currentMonth, currentYear).ToString() + " đồng";


        }

        public void LoadYear(HomeView homeWindow)
        {

            ThisYear = DateTime.Now.ToString("yyyy");



            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            Revenue = string.Format("{0:n0}", ReportDAL.Instance.QueryRevenueInYear(currentYear)).ToString() + " đồng";
            Consignment = string.Format("{0:n0}", ReportDAL.Instance.QueryConsignmentInYear(currentYear)).ToString() + " đồng";
            RepairCost = string.Format("{0:n0}", ReportDAL.Instance.QueryRepairCostYear(currentYear)).ToString() + " đồng";

            Salary = ReportDAL.Instance.QuerySalaryYear(currentYear).ToString() + " đồng";



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

        public ICommand InitTopSaleTodayCommand { get; set; }

        public ICommand InitTopSaleMonthCommand { get; set; }

        public ICommand InitTopSaleYearCommand { get; set; }


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
            FoodRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryFoodRevenueInDay(today, thismonth, currentYear))).ToString() + " VND";
            DrinkRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryDrinkRevenueInDay(today, thismonth, currentYear))).ToString() + " VND";
            OtherRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryOtherRevenueInDay(today, thismonth, currentYear))).ToString() + " VND";


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
            FoodRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryFoodRevenueInMonth(selectedMonth, currentYear))).ToString() + " VND";
            DrinkRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryDrinkRevenueInMonth(selectedMonth, currentYear))).ToString() + " VND";
            OtherRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryOtherRevenueInMonth(selectedMonth, currentYear))).ToString() + " VND";
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




            Labels = ReportDAL.Instance.QueryMonthInYear(selectedYear);
            Formatter = value => string.Format("{0:N0}", value);
            FoodRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryFoodRevenueInYear(selectedYear))).ToString() + " VND";
            DrinkRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryDrinkRevenueInYear(selectedYear))).ToString() + " VND";
            OtherRevenue = string.Format("{0:#,##0}", long.Parse(ReportDAL.Instance.QueryOtherRevenueInYear(selectedYear))).ToString() + " VND";

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



        private SeriesCollection pieSeriesCollection_;
        public SeriesCollection PieSeriesCollection
        {
            get { return pieSeriesCollection_; }
            set { pieSeriesCollection_ = value; OnPropertyChanged(); }
        }


        public ICommand InitPieChartTodayCommand { get; set; }
        public ICommand InitPieChartMonthCommand { get; set; }
        public ICommand InitPieChartYearCommand { get; set; }

        public void InitPieChartToday(HomeView parameter)
        {
            string today = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            FoodRevenue1 = (ReportDAL.Instance.QueryFoodRevenueInDay(today, currentMonth, currentYear)).ToString();
            DrinkRevenue1 = (ReportDAL.Instance.QueryDrinkRevenueInDay(today, currentMonth, currentYear)).ToString();
            OtherRevenue1 = (ReportDAL.Instance.QueryOtherRevenueInDay(today, currentMonth, currentYear)).ToString();

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
                },
                  new PieSeries
                {
                    Title="Đồ ăn",
            Values= food,
              Fill = (Brush)new BrushConverter().ConvertFrom("#6254F9"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                }


            };
        }


        public void InitPieChartMonth(HomeView parameter)
        {
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            FoodRevenue1 = (ReportDAL.Instance.QueryFoodRevenueInMonth(currentMonth, currentYear)).ToString();
            DrinkRevenue1 = (ReportDAL.Instance.QueryDrinkRevenueInMonth(currentMonth, currentYear)).ToString();
            OtherRevenue1 = (ReportDAL.Instance.QueryOtherRevenueInMonth(currentMonth, currentYear)).ToString();

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
                },
                  new PieSeries
                {
                    Title="Đồ ăn",
            Values= food,
              Fill = (Brush)new BrushConverter().ConvertFrom("#6254F9"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                }


            };
        }




        public void InitPieChartYear(HomeView parameter)
        {
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            FoodRevenue1 = (ReportDAL.Instance.QueryFoodRevenueInYear(currentYear)).ToString();
            DrinkRevenue1 = (ReportDAL.Instance.QueryDrinkRevenueInYear(currentYear)).ToString();
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
                },
                  new PieSeries
                {
                    Title="Đồ ăn",
            Values= food,
              Fill = (Brush)new BrushConverter().ConvertFrom("#6254F9"),
              LabelPoint=chartPoint => string.Format("{0:N0}", chartPoint.Y)
                }


            };
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
        public void LoadTopSaleMonth(HomeView parameter)
        {
            parameter.top1.Visibility = Visibility.Visible;

            parameter.top2.Visibility = Visibility.Visible;

            parameter.top3.Visibility = Visibility.Visible;
            string today = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            List<Product> products = ReportDAL.Instance.QueryTopSaleMonth(currentMonth, currentYear);

            if (products.Count == 3)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));


                parameter.top3.Title = products[2].Title;
                parameter.top3.UpPrice = string.Format("{0:#,##0}", products[2].Total).ToString() + " VND";
                parameter.top3.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[2].Barcode));

            }


            if (products.Count == 2)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));



                parameter.top3.Visibility = Visibility.Hidden;
            }


            if (products.Count == 1)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));




                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }

            if (products.Count == 0)
            {

                parameter.top1.Visibility = Visibility.Hidden;

                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }


        }

        public void LoadTopSaleToday(HomeView parameter)
        {
            parameter.top1.Visibility = Visibility.Visible;

            parameter.top2.Visibility = Visibility.Visible;

            parameter.top3.Visibility = Visibility.Visible;
            string today = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            List<Product> products = ReportDAL.Instance.QueryTopSaleToday(today, currentMonth, currentYear);

            if (products.Count == 3)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));


                parameter.top3.Title = products[2].Title;
                parameter.top3.UpPrice = string.Format("{0:#,##0}", products[2].Total).ToString() + " VND";
                parameter.top3.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[2].Barcode));

            }


            if (products.Count == 2)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));



                parameter.top3.Visibility = Visibility.Hidden;
            }


            if (products.Count == 1)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));




                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }

            if (products.Count == 0)
            {

                parameter.top1.Visibility = Visibility.Hidden;

                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }



        }

        public void LoadTopSaleYear(HomeView parameter)
        {
            parameter.top1.Visibility = Visibility.Visible;

            parameter.top2.Visibility = Visibility.Visible;

            parameter.top3.Visibility = Visibility.Visible;
            string today = DateTime.Now.Day.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            string currentYear = DateTime.Now.Year.ToString();
            List<Product> products = ReportDAL.Instance.QueryTopSaleYear(currentYear);

            if (products.Count == 3)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));


                parameter.top3.Title = products[2].Title;
                parameter.top3.UpPrice = string.Format("{0:#,##0}", products[2].Total).ToString() + " VND";
                parameter.top3.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[2].Barcode));

            }


            if (products.Count == 2)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));

                parameter.top2.Title = products[1].Title;
                parameter.top2.UpPrice = string.Format("{0:#,##0}", products[1].Total).ToString() + " VND";
                parameter.top2.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[1].Barcode));



                parameter.top3.Visibility = Visibility.Hidden;
            }


            if (products.Count == 1)
            {
                parameter.top1.Title = products[0].Title;
                parameter.top1.UpPrice = string.Format("{0:#,##0}", products[0].Total).ToString() + " VND";
                parameter.top1.avatarUser.Source = ConvertByteToBitmapImage(DatabaseHelper.LoadProductAvatar(products[0].Barcode));




                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }

            if (products.Count == 0)
            {

                parameter.top1.Visibility = Visibility.Hidden;

                parameter.top2.Visibility = Visibility.Hidden;

                parameter.top3.Visibility = Visibility.Hidden;
            }




        }








    }
}