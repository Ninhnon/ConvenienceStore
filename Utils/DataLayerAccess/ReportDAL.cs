using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                string queryString = string.Format("select day(BillDate) as day from Bill " +
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
        public int QueryRevenueNumOfSoldBillInMonth(string month, string year)
        {
            int res = 0;
            try
            {
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
        public string[] QueryMonthInYear(string year)
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryString = string.Format("select distinct month(BillDate) as month from Bill where year(BillDate) = {0} ", year);
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(rdr["month"].ToString());
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
