using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace ConvenienceStore.Utils.Helpers
{
    public partial class DatabaseHelper
    {
        static readonly string queryProduct = @"select Barcode,Title,ProductionSite,Image,InputPrice,OutputPrice,Stock,ManufacturingDate,ExpiryDate,Discount,Type
        from Consignment c,Product p,
        ( 
        select ProductId, min([ExpiryDate]) e
        from Consignment
        where Stock>0
        group by ProductId
        ) h
        where c.ProductId=p.Barcode and h.ProductId=c.ProductId and h.e = c.ExpiryDate
        order by ExpiryDate";
        static readonly string queryVoucher = @"select * from [Voucher]";
        static readonly string queryReport = @"select * from [Report]";
        static readonly string queryUser = @"select * from [Users]";
        static readonly string queryConsingment = @"select * from [Consignment]";
        static readonly string queryBillData = @"select * from [Bill]";

        public static List<Model.Staff.Bill> FetchingBillData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryBillData, sqlCon);

            List<Model.Staff.Bill> reports = new List<Model.Staff.Bill>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(new Model.Staff.Bill()
                {
                    Id = reader.GetInt32(0),
                    BillDate = reader.GetDateTime(1),
                    CustomerId = reader.GetInt32(2),
                    UserId = reader.GetInt32(3),
                    Price = reader.GetInt32(3),
                });

            }
            reader.Close();
            sqlCon.Close();
            return reports;
        } 
        public static List<Consignment> FetchingConsignmentData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryConsingment, sqlCon);

            List<Consignment> reports = new List<Consignment>();

            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                reports.Add(new Consignment()
                {
                    InputInfoId = read.GetInt32(0),
                    ProductId = read.GetString(1),
                    Stock = read.GetInt32(2),
                    ManufacturingDate = read.GetDateTime(3),
                    ExpiryDate = read.GetDateTime(4),
                    InputPrice = read.GetInt32(5),
                    OutputPrice = read.GetInt32(6),
                    Discount = read.GetInt32(7),
                });

            }
            read.Close();

            sqlCon.Close();
            return reports;
        }
        public static List<User> FetchingUserData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryUser, sqlCon);

            List<User> reports = new List<User>();

            SqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {
                reports.Add(new User()
                {
                    Id = read.GetInt32(0),
                    UserRole = read.GetString(1),
                    Name = read.GetString(2),
                    Address = read.IsDBNull(3) ? null : read.GetString(3),
                    Phone = read.IsDBNull(4) ? null : read.GetString(4),
                    Email = read.IsDBNull(5) ? null : read.GetString(5),
                    UserName = read.GetString(6),
                    Password = read.GetString(7),
                    Image = Convert.FromBase64String(read["Avatar"].ToString())
                });

            }
            read.Close();

            sqlCon.Close();
            return reports;
        }
        public static List<Report> FetchingReportData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryReport, sqlCon);

            List<Report> reports = new List<Report>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reports.Add(new Report()
                {
                    Id = reader.GetString(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Status = reader.GetString(3),
                    SubmittedAt = reader.GetDateTime(4),
                    RepairCost = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                    StartDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    FinishDate = reader.IsDBNull(7) ? null : reader.GetDateTime(6),
                    StaffId = reader.GetInt32(8),
                    Level = reader.GetString(9),
                    Image = (byte[])(reader["Image"]),
                });

            }
            reader.Close();

            sqlCon.Close();
            return reports;
        }
        public static List<Vouchers> FetchingVoucherData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryVoucher, sqlCon);

            List<Vouchers> vouchers = new List<Vouchers>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vouchers.Add(new Vouchers()
                {
                    ReleaseId = reader.GetString(0),
                    ReleaseName = reader.GetString(1),
                    StartDate = reader.GetDateTime(2),
                    FinishDate = reader.GetDateTime(3),
                    ParValue = reader.GetInt32(4),
                    Status = reader.GetBoolean(5),
                });

            }
            reader.Close();

            sqlCon.Close();
            return vouchers;
        }
        public static List<Products> FetchingProductData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryProduct, sqlCon);

            List<Products> Products = new List<Products>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Products()
                {
                    BarCode = reader.GetString(0),
                    Title = reader.GetString(1),
                    ProductionSite = reader.GetString(2),
                    Image = (byte[])(reader["Image"]),
                    Cost = reader.GetInt32(4),
                    Price = reader.GetInt32(5),
                    Stock = reader.GetInt32(6),
                    ManufacturingDate = reader.GetDateTime(7),
                    ExpiryDate = reader.GetDateTime(8),
                    Discount = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Type = reader.IsDBNull(10) ? null : reader.GetString(10),
                });

            }
            reader.Close();

            sqlCon.Close();
            return Products;
        }
        
    }
}
