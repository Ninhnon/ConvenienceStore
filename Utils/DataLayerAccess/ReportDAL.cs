using ConvenienceStore.Model.Admin;
using LiveCharts;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ConvenienceStore.Utils.DataLayerAccess
{
    public class ReportDAL : DataProvider
    {
        private static ReportDAL instance; //tạo ra instance để truy cập vào các method

        public static ReportDAL Instance
        {
            get { if (instance == null) instance = new ReportDAL(); return instance; }
            private set { instance = value; }
        }
        private ReportDAL()
        {

        }
        public string[] QueryDayInMonth(string month, string year) //query các ngày có trong Bill
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryString = string.Format("select distinct day(BillDate) as day from Bill " +
                    "where month(BillDate) = {0} and year(BillDate) = {1} ", month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader(); //dùng reader vì chỉ có 1 cột
                while (rdr.Read())
                {
                    res.Add(rdr["day"].ToString());
                }
                return res.ToArray();
            }
            catch
            {
                return res.ToArray();
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> QueryRevenueByMonth(string month, string year) //query lọi nhuận trong tháng
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] daysOfMonth = Instance.QueryDayInMonth(month, year); //mảng chứa các ngày trong tháng, ngày có thể trùng

                OpenConnection();
                string queryString = string.Format("select day(BillDate), sum(Price) from Bill where month(BillDate) = {0} " +
                    "and year(BillDate) = {1} group by day(BillDate)", month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable(); //tạo 1 table mới rồi lấy adapter fill các cột vào table đó
                adapter.Fill(dataTable);

                long[] revenue = new long[daysOfMonth.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;
                for (int i = 0; i < daysOfMonth.Length && j < numOfRows; i++)
                {
                    if (daysOfMonth[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> QueryRevenueByDay(string day, string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            List<long> res1 = new List<long>();
            try
            {
                string[] daysOfMonth = Instance.QueryDayInMonth(month, year);

                OpenConnection();
                string queryString = string.Format("select sum(Price) as revenue from Bill where day(BillDate)={0} " + "and month(BillDate) = {1} " +
                    "and year(BillDate) = {2} ", day, month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res1.Add(long.Parse(rdr["revenue"].ToString()));
                }
                res = new ChartValues<long>(res1);
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRevenueInToday(string day, string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(Price) as revenue from Bill where day(billdate)={0} and year(BillDate) = {1} and month(BillDate) = {2}", day, year, month);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["revenue"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRevenueInMonth(string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(Price) as revenue from Bill where year(BillDate) = {0} and month(BillDate) = {1}", int.Parse(year), int.Parse(month));
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["revenue"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRevenueInYear(string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(Price) as revenue from Bill where year(BillDate) = {0}", year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["revenue"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryConsignmentInToday(string day, string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(inputprice*stock) as tong  from inputinfo inner join consignment" +
                    " on inputinfo.id=consignment.inputinfoid where day(inputdate)={0} and month(inputdate)={1} and year(inputdate)={2} ", day, month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryConsignmentInMonth(string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(inputprice*stock) as tong from inputinfo inner join consignment " +
                    "on inputinfo.id=consignment.inputinfoid where month(inputdate)={0} and year(inputdate)={1} ", month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryConsignmentInYear(string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(inputprice*stock) as tong  from inputinfo inner join consignment on inputinfo.id=consignment.inputinfoid  where year(inputdate)={0}", year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRepairCostToday(string day, string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(repaircost) as tong from report where status=N\'Đã giải quyết\' " +
                    "and day(finishdate)={0} and month(finishdate)={1} and year(finishdate)={2} ", day, month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRepairCostMonth(string month, string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(repaircost) as tong from report where status=N\'Đã giải quyết\'" +
                    " and month(finishdate)={0} and year(finishdate)={1} ", month, year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int QueryRepairCostYear(string year)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(repaircost) as tong from report where status=N\'Đã giải quyết\' and year(finishdate)={0} ", year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res = int.Parse(rdr["tong"].ToString());
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<int> QueryNumOfSoldBillToday(string today, string month)
        {
            ChartValues<int> res = new ChartValues<int>();



            OpenConnection();

            string queryString = string.Format("select count(Id) as numOfSoldBill from Bill " +
      "where day(BillDate) = {0} and month(BillDate) = {1}", int.Parse(today), int.Parse(month));
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                res.Add(int.Parse(rdr["numOfSoldBill"].ToString()));
            }





            return res;

            CloseConnection();

        }
        public ChartValues<int> QueryRevenueNumOfSoldBillEachDayInMonth(string month, string year)
        {
            ChartValues<int> res = new ChartValues<int>();


            List<string> list = QueryDayInMonth(month, year).ToList();
            OpenConnection();
            foreach (string s in list)
            {
                string queryString = string.Format("select count(Id) as numOfSoldBill from Bill " +
          "where day(BillDate) = {0} and month(BillDate) = {1}", int.Parse(s), int.Parse(month));
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(int.Parse(rdr["numOfSoldBill"].ToString()));
                }


            }



            return res;

            CloseConnection();

        }
        public ChartValues<int> QueryRevenueNumOfSoldBillInYear(string year)
        {
            ChartValues<int> res = new ChartValues<int>();


            List<string> list = QueryMonthInYearList(year);
            OpenConnection();
            foreach (string s in list)
            {
                string queryString = string.Format("select count(Id) as numOfSoldBill from Bill " +
          "where year(BillDate) = {0} and month(BillDate) = {1}", int.Parse(year), int.Parse(s));
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(int.Parse(rdr["numOfSoldBill"].ToString()));
                }


            }



            return res;

            CloseConnection();

        }
        //public ChartValues<int > QueryNumofSoldBillEachMonth(string year)
        //{
        //    ChartValues<int> res = new ChartValues<int>();

        //}
        public List<Product> QueryTopSaleToday(string day, string month, string year)
        {
            List<Product> products = new List<Product>();
            OpenConnection();
            string queryString = string.Format("select top 3 barcode,title, sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                "join product on billdetail.productid = product.barcode " +
                "where day(billdate)={0} and month(billdate) = {1} and year(billdate) = {2} " +
                "group by barcode,title order by sum(price) desc", day, month, year);
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader read = command.ExecuteReader();
            int i = 0;
            while (read.Read() && i < 3)
            {

                products.Add(new Product()

                {
                    Barcode = read.GetString(0),
                    Title = read.GetString(1),
                    Total = read.GetInt32(2)


                }
                );

            }

            CloseConnection();
            return products;
        }
        public List<Product> QueryTopSaleMonth(string month, string year)
        {
            List<Product> products = new List<Product>();
            OpenConnection();
            string queryString = string.Format("select top 3 barcode,title, sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                "join product on billdetail.productid = product.barcode " +
                "where month(billdate) = {0} and year(billdate) = {1} " +
                "group by barcode,title order by sum(price) desc", month, year);
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader read = command.ExecuteReader();
            int i = 0;
            while (read.Read() && i < 3)
            {

                products.Add(new Product()

                {
                    Barcode = read.GetString(0),
                    Title = read.GetString(1),
                    Total = read.GetInt32(2)


                }
                );

            }

            CloseConnection();
            return products;
        }
        public List<Product> QueryTopSaleYear(string year)
        {
            List<Product> products = new List<Product>();
            OpenConnection();
            string queryString = string.Format("select top 3 barcode,title, sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                "join product on billdetail.productid = product.barcode " +
                "where year(billdate) = {0} " +
                "group by barcode,title order by sum(price) desc", year);
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader read = command.ExecuteReader();
            int i = 0;
            while (read.Read() && i < 3)
            {

                products.Add(new Product()

                {
                    Barcode = read.GetString(0),
                    Title = read.GetString(1),
                    Total = read.GetInt32(2)


                }
                );

            }

            CloseConnection();
            return products;
        }
        public string QueryFoodRevenueInYear(string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                       "join product on billdetail.productid = product.barcode where product.type = N\'Đồ ăn\'  and year(billdate) = {0}", int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }

            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }

        }
        public string QueryDrinkRevenueInYear(string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                  "join product on billdetail.productid = product.barcode where product.type = N\'Thức uống\'  and year(billdate) = {0}", int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }

            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }

        }
        public string QueryOtherRevenueInYear(string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
              "join product on billdetail.productid = product.barcode where product.type = N\'Khác\'  and year(billdate) = {0}", int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }

            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }

        }
        public string QueryFoodRevenueInMonth(string month, string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                 "join product on billdetail.productid = product.barcode where product.type = N\'Đồ ăn\' and month(billdate)={0} and year(billdate) = {1}", int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }

            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }

        }
        public string QueryDrinkRevenueInMonth(string month, string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                  "join product on billdetail.productid = product.barcode where product.type = N\'Thức uống\' and month(billdate)={0} and year(billdate) = {1}", int.Parse(month), int.Parse(year));

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }

            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }
        public string QueryOtherRevenueInMonth(string month, string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
              "join product on billdetail.productid = product.barcode where product.type = N\'Khác\' and month(billdate)={0} and year(billdate) = {1}", int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }

        }

        public string QueryFoodRevenueInDay(string day, string month, string year)
        {
            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                    "join product on billdetail.productid = product.barcode where product.type = N\'Đồ ăn\' and day(billdate)={0} and month(billdate)={1} and year(billdate) = {2}", int.Parse(day), int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }
        public string QueryDrinkRevenueInDay(string day, string month, string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                    "join product on billdetail.productid = product.barcode where product.type = N\'Thức uống\' and day(billdate)={0} and month(billdate)={1} and year(billdate) = {2}", int.Parse(day), int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }
        public string QueryOtherRevenueInDay(string day, string month, string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(price) as tong  from bill inner join billdetail on bill.id = billdetail.billid inner " +
                    "join product on billdetail.productid = product.barcode where product.type = N\'Khác\' and day(billdate)={0} and month(billdate)={1} and year(billdate) = {2}", int.Parse(day), int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }

        public string QuerySalaryToday(string day, string month, string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(totalmoney) as tong from salarybill where day(salarybilldate)={0} and month(salarybilldate)={1} and year(salarybilldate)={2}", int.Parse(day), int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }
        public string QuerySalaryMonth(string month, string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(totalmoney) as tong from salarybill where month(salarybilldate)={0} and year(salarybilldate)={1}", int.Parse(month), int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }
        public string QuerySalaryYear(string year)
        {

            string s = "0";
            try
            {

                OpenConnection();
                string queryString = string.Format("select sum(totalmoney) as tong from salarybill where year(salarybilldate)={0}", int.Parse(year));
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    s = rdr.GetInt32(0).ToString();
                }
                return s;
            }
            catch
            {
                return s;

            }



            finally
            {

                CloseConnection();


            }
        }

        public int QueryRevenueNumOfSoldBillInMonth(string month, string year)
        {
            int res = 0;

            OpenConnection();
            string queryString = string.Format("select count(Id) as numOfSoldBill from Bill " +
                "where year(BillDate) = {0} and month(BillDate) = {1}", int.Parse(year), int.Parse(month));
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                res = int.Parse(rdr["numOfSoldBill"].ToString());
            }
            return res;

            CloseConnection();

        }
        public string[] QueryMonthInYear(string year)
        {
            List<string> res = new List<string>();

            OpenConnection();
            string queryString = string.Format("select distinct month(BillDate) as month from Bill where year(BillDate) = {0} ", year);
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                res.Add(rdr["month"].ToString());
            }
            return res.ToArray();

            CloseConnection();

        }
        public List<string> QueryMonthInYearList(string year)
        {
            List<string> res = new List<string>();

            OpenConnection();
            string queryString = string.Format("select distinct month(BillDate) as month from Bill where year(BillDate) = {0} ", year);
            SqlCommand command = new SqlCommand(queryString, conn);

            SqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                res.Add(rdr["month"].ToString());
            }
            return res;

            CloseConnection();

        }

        public ChartValues<long> QueryRevenueByYear(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] monthsOfYear = Instance.QueryMonthInYear(year);

                OpenConnection();
                string queryString = string.Format("select month(BillDate), sum(Price) from Bill where year(BillDate) = {0} " +
                    "group by month(BillDate)", year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[monthsOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < monthsOfYear.Length && j < numOfRows; i++)
                {
                    if (monthsOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
