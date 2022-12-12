using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using ConvenienceStore.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        static readonly string queryCustomerData = @"select * from [Customer]";
        static readonly string queryBillData = @"select * from [Bill]";
        static readonly string queryInsertBill = @"insert into Bill(BillDate, CustomerId, UserId, Price) Values (@billDate, @cusId, @userId, @price)";
        static readonly string queryAvatar = @"select Avatar from [Users] where Id={0}";
        static readonly string queryName = @"select Name from [Users] where Id={0}";
        static readonly string insertErorrs = "insert into Report(Title, Description, Status, RepairCost, SubmittedAt, StaffId, Image) select N'{0}',N'{1}',N'{2}',{3},N'{4}',N'{5}', BulkColumn FROM Openrowset(Bulk N'{6}', Single_Blob) as img";
        static readonly string queryInsertBillDetail = @"insert into BillDetail(BillId, ProductId, Quantity, TotalPrice) values (@billId, @productId, @quantity, @totalPrice)";
        static readonly string querySearchVoucher = @"select Code, Status, TypeVoucher, ParValue, StartDate, FinishDate
                                                        from Voucher v join BlockVoucher b on v.BlockId = b.Id
                                                        where Code = @code AND Status = 0";
        static readonly string queryUpdateVoucherStatus = @"update Voucher set Status = 1 where Code = @code";
        static readonly string queryVoucherDetail = @"select Code, Status, TypeVoucher, ParValue, StartDate, FinishDate from Voucher v join BlockVoucher b on v.BlockId = b.Id";

        static readonly string insertReport = "insert Report values (@Title, @Description, @Status, @SubmittedAt,@RepairCost,Null,Null,@StaffId, @Image)";
        static readonly string insertBillData = @"insert into Bill(BillDate, CustomerId, UserId, Price) Values (@billDate, @cusId, @userId, @price)";
        static readonly string updateReport = @"update Report set Title = @Title, Image = @Image, RepairCost = @RepairCost
                                                 where SubmittedAt=@SubmittedAt";
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
                    CustomerId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
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
                    Image = (byte[])(read["Avatar"])
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
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Status = reader.GetString(3),
                    SubmittedAt = reader.GetDateTime(4),
                    RepairCost = reader.GetInt32(5),
                    StartDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    FinishDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    StaffId = reader.IsDBNull(8) ? 1 : reader.GetInt32(8),
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
                    ReleaseId = reader.GetString(1),
                    ReleaseName = reader.GetString(2),
                    StartDate = reader.GetDateTime(3),
                    FinishDate = reader.GetDateTime(4),
                    ParValue = reader.GetInt32(5),
                    Status = reader.GetInt32(6),
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

        public static void ThemErorr(Report t, string filepath)
        {
            var strCmd = string.Format(insertErorrs, t.Title, t.Description,t.Status, t.RepairCost, t.SubmittedAt, t.StaffId, filepath);
            sqlCon.Open();
            SqlCommand cmd = new(strCmd, sqlCon);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlCon.Close();
        }
        public static void ThemErorr(Report t)
        {
            var strCmd = string.Format(insertErorrs, t.Title, t.Description, t.RepairCost, t.SubmittedAt, t.StaffId, t.Image);
            sqlCon.Open();
            SqlCommand cmd = new(strCmd, sqlCon);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlCon.Close();
        }
        public static List<Customer> FetchingCustomerData()
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryCustomerData, sqlCon);

            List<Customer> customers = new List<Customer>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Point = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                });

            }
            reader.Close();

            sqlCon.Close();
            return customers;
        }

        public static void InsertBill(int? customerId, int? price)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryInsertBill, sqlCon);
            cmd.Parameters.AddWithValue("@billDate", System.DateTime.Now);
            cmd.Parameters.AddWithValue("@cusId", (customerId == null ? DBNull.Value : customerId));
            cmd.Parameters.AddWithValue("@userId", CurrentAccount.idAccount);
            cmd.Parameters.AddWithValue("@price", price);

            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        public static void InsertBillDetail(BillDetails b)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryInsertBillDetail, sqlCon);
            cmd.Parameters.AddWithValue("@billId", b.BillId);
            cmd.Parameters.AddWithValue("@productId", b.ProductId);
            cmd.Parameters.AddWithValue("@quantity", b.Quantity);
            cmd.Parameters.AddWithValue("@totalPrice", b.TotalPrice);

            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        public static byte[] LoadAvatar(int id)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryAvatar, id);

            byte[] Avatar = new byte[byte.MaxValue];
            SqlCommand cmd = new(strCmd, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Avatar = reader.IsDBNull(0) ? null : (Byte[])reader["Avatar"];
            }
            reader.Close();
            sqlCon.Close();

            return Avatar;
        }
        public static string GetName(int id)
        {
            var strCmd = string.Format(queryName, id);
            sqlCon.Open();
            string s="Nguyễn Trọng Ninh";
            SqlCommand cmd = new(strCmd, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                s = reader.GetString(0);
            }
            reader.Close();
            sqlCon.Close();
            return s;
        }
        public static void InsertReport(Report report)
        {
            sqlCon.Open();

            // Bảng Consignment chứa khóa ngoại tham chiếu với khóa chính Barcode trong bảng Product

            // Xem thử loại sản phẩm đó đã tồn tại trong bảng Product hay chưa
            var strCmd = $"select * from Report where Title = '{report.Title}'";
            var cmd = new SqlCommand(strCmd, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            bool isExisted;
            try
            {
                isExisted = reader.Read();
            }
            catch
            {
                isExisted = false;
            }
            reader.Close();

            //if (isExisted) // Nếu đã tồn tại, chỉ cần update lại 1 vài thông tin về loại sản phẩm
            //{
            //    cmd = new SqlCommand(updateProduct, sqlCon);
            //    cmd.Parameters.AddWithValue("@RepairCost", report.RepairCost);
            //    cmd.Parameters.AddWithValue("@StaffId", report.StaffId);
            //    cmd.Parameters.AddWithValue("@Status", report.Status);
            //    cmd.Parameters.AddWithValue("@Description", report.Description);
            //    cmd.Parameters.AddWithValue("@Image", report.Image);
            //    cmd.Parameters.AddWithValue("@SubmittedAt", report.SubmittedAt);
            //    cmd.ExecuteNonQuery();
            //}
            //else // Nếu chưa, tạo mới loại sản phẩm
            //{
                cmd = new SqlCommand(insertReport, sqlCon);
                cmd.Parameters.AddWithValue("@Title", report.Title);
                cmd.Parameters.AddWithValue("@RepairCost", report.RepairCost);
                cmd.Parameters.AddWithValue("@StaffId", report.StaffId);
                cmd.Parameters.AddWithValue("@Status", report.Status);
                cmd.Parameters.AddWithValue("@Description", report.Description);
                cmd.Parameters.AddWithValue("@Image", report.Image);
                cmd.Parameters.AddWithValue("@SubmittedAt", report.SubmittedAt);

                cmd.ExecuteNonQuery();
            //}

            reader.Close();

            // Xử lí trường hợp trong Title có dấu nháy đơn (')
            int i = 0;
            string title = report.Title;
            while (i < title.Length)
            {
                if (title[i] == '\'')
                {
                    title = title.Insert(i, "'");
                    i++;
                }
                i++;
            }
            sqlCon.Close();
        }

        public static int ApplyVoucher(int totalPrice, string code, ref int error) //Hàm lấy ra giá trị discount
        {
            sqlCon.Open();

            //Tìm kiếm voucher
            var cmd = new SqlCommand(querySearchVoucher, sqlCon);
            cmd.Parameters.AddWithValue("@code", code);
            List<Model.Staff.Vouchers> vouchers = new List<Model.Staff.Vouchers>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vouchers.Add(new Model.Staff.Vouchers()
                {
                    ReleaseId = reader.GetString(0),
                    Status = reader.GetInt32(1),
                    Type = reader.GetInt32(2),
                    ParValue = reader.GetInt32(3),
                    StartDate = reader.GetDateTime(4),
                    FinishDate = reader.GetDateTime(5),
                });

            }
            reader.Close();

            int discount = 0;
            if (vouchers.Count != 0)
            {
                if (vouchers[0].Type == 0)  //Giảm tiền mặt
                    discount = vouchers[0].ParValue.HasValue ? Convert.ToInt32(vouchers[0].ParValue) : 0;
                else if (vouchers[0].Type == 1) //Giảm %
                    discount = vouchers[0].ParValue.HasValue ? Convert.ToInt32(totalPrice * vouchers[0].ParValue / 100) : 0;

                if (vouchers[0].FinishDate < System.DateTime.Now)  //Quá hạn sử dụng
                    error = 1;
            }
            else
                error = 0;

            sqlCon.Close();
            return discount;
        }

        public static void UpdateVoucherStatus(string code)
        {
            if (code != null)
            {
                sqlCon.Open();
                //Tìm kiếm voucher
                var cmd = new SqlCommand(queryUpdateVoucherStatus, sqlCon);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }
        public static void UpdateReport(Report editedReport)
        {
            sqlCon.Open();

            // Update values in Product table
            var cmd = new SqlCommand(updateReport, sqlCon);

            cmd.Parameters.AddWithValue("@Title", editedReport.Title);
            cmd.Parameters.AddWithValue("@Image", editedReport.Image);
            cmd.Parameters.AddWithValue("@RepairCost", editedReport.RepairCost);
            cmd.Parameters.AddWithValue("@SubmittedAt", editedReport.SubmittedAt);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }
    }
}
