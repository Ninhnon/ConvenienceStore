using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ConvenienceStore.Utils.Helpers
{
    public partial class DatabaseHelper
    {
        static readonly string queryProduct = @"select Barcode,Title,ProductionSite,Image,InputPrice,OutputPrice,InStock,ManufacturingDate,ExpiryDate,Discount,Type, InputInfoId
        from Consignment c,Product p, InputInfo iii,
        ( 
        select  distinct cc.ProductId, min([ExpiryDate]) e, min(InputDate) ii
        from Consignment cc, InputInfo i
        where InStock>0 AND ExpiryDate > GETDATE() and i.Id=cc.InputInfoId
        group by ProductId
        ) h
        where c.ProductId=p.Barcode and h.ProductId=c.ProductId and h.e = c.ExpiryDate and iii.id= c.InputInfoId and iii.InputDate=h.ii 
        order by ExpiryDate";
        static readonly string queryProductT = @"select Barcode,Title,ProductionSite,Image,InputPrice,OutputPrice,InStock,ManufacturingDate,ExpiryDate,Discount,Type,InputInfoId
        from Consignment c,Product p
        where c.ProductId=p.Barcode and InStock>0
        order by ExpiryDate";
        static readonly string queryVoucher = @"select * from [Voucher]";
        static readonly string queryReport = @"select * from [Report] order by SubmittedAt desc";
        static readonly string queryUser = @"select * from [Users]";
        static readonly string queryConsingment = @"select * from [Consignment]";
        static readonly string queryCustomerData = @"select * from [Customer]";
        static readonly string queryBillData = @"select * from [Bill]";
        static readonly string queryInsertBill = @"insert into Bill(BillDate, CustomerId, UserId, Price, Discount) Values (@billDate, @cusId, @userId, @price, @discount)";
        static readonly string queryAvatar = @"select Avatar from [Users] where Id={0}";
        static readonly string queryName = @"select Name from [Users] where Id={0}";
        static readonly string insertErorrs = "insert into Report(Title, Description, Status, RepairCost, SubmittedAt, StaffId, Image) select N'{0}',N'{1}',N'{2}',{3},N'{4}',N'{5}', BulkColumn FROM Openrowset(Bulk N'{6}', Single_Blob) as img";
        static readonly string queryInsertBillDetail = @"insert into BillDetail(BillId, ProductId, Quantity, TotalPrice) values (@billId, @productId, @quantity, @totalPrice)";
        static readonly string querySearchVoucher = @"select Code, Status, TypeVoucher, ParValue, StartDate, FinishDate
                                                        from Voucher v join BlockVoucher b on v.BlockId = b.Id
                                                        where Code = @code AND Status = 0";
        static readonly string queryUpdateVoucherStatus = @"update Voucher set Status = 1 where Code = @code";
        static readonly string queryVoucherDetail = @"select Code, Status, TypeVoucher, ParValue, StartDate, FinishDate from Voucher v join BlockVoucher b on v.BlockId = b.Id";
        static readonly string queryUpdateConsignmentInStock = @"update Consignment
                                                                set InStock = InStock - @quantity
                                                                where InputInfoId = @inputInfoId and ProductId = @productId";

        static readonly string insertReport = "insert Report values (@Title, @Description, @Status, @SubmittedAt,@RepairCost,Null,Null,@StaffId, @Image)";
        static readonly string insertBillData = @"insert into Bill(BillDate, CustomerId, UserId, Price) Values (@billDate, @cusId, @userId, @price)";
        static readonly string updateReport = @"update Report set Title = @Title, Image = @Image, RepairCost = @RepairCost, Description = @Description
                                                 where Id=@Id";
        static readonly string queryBillsData = @"select b.Id, u.Name, c.Name, b.BillDate, b.Price, u.Id, c.Id, b.Discount
                                                    from Bill b left join Customer c on b.CustomerId = c.Id
                                                                left join Users u on b.UserId = u.Id
                                                    order by b.Id DESC";
        static readonly string queryBillDetailsData = @"select bd.Quantity, p.Title, bd.TotalPrice
                                                        from BillDetail bd join Product p on bd.ProductId = p.Barcode
                                                        where BillId = @id";
        static readonly string updateReportAD = @"update Report set Title = @Title, Image = @Image, RepairCost = @RepairCost,Status = @Status, Description = @Description, StartDate = NULL, FinishDate = NULL where Id=@Id";
        static readonly string updateReportADS = @"update Report set Title = @Title, Image = @Image, RepairCost = @RepairCost,Status = @Status, StartDate = @StartDate, FinishDate = NULL,Description = @Description where Id=@Id";
        static readonly string updateReportADSF = @"update Report set Title = @Title, Image = @Image, RepairCost = @RepairCost,Status = @Status, StartDate = @StartDate, FinishDate = @FinishDate,Description = @Description where Id=@Id";

        static readonly string updateReportFULL = @"update Report set Title = N'{0}', [Image] = {1}, RepairCost = {2},[Status] = '{3}',StartDate = '{4}',FinishDate = '{5}',Description = N'{6}' where [Id]={7}";

        static readonly string queryCustomerPoint = @"select Point
                                                        from Customer
                                                        where Id = @customerId";

        static readonly string queryUpdateCustomerPoint = @"update Customer
                                                        set Point = @customerPoint
                                                        where Id = @customerId";

        static readonly string queryInsertCustomer = @"insert into Customer(Name, Address, Phone, Email) values (@name, @address, @phone, @email)";

        static readonly string queryTeamMembers = @"select Name, Avatar, UserRole from Users
                                                    where ManagerId = @managerId and Id != @id";
        static readonly string updateSL = @"update Consignment
		                                    set InStock = 0
		                                    where InputInfoId = {0} and ProductId = N'{1}'";
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
                    Id = reader.GetInt32(0),
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
                    Title = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    ProductionSite = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Image = reader.IsDBNull(3) ? null : (byte[])(reader["Image"]),
                    Cost = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    Stock = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    ManufacturingDate = reader.GetDateTime(7),
                    ExpiryDate = reader.GetDateTime(8),
                    Discount = reader.IsDBNull(9) ? null : reader.GetDouble(9),
                    Type = reader.IsDBNull(10) ? null : reader.GetString(10),
                    InputInfoId = reader.GetInt32(11),
                });

            }
            reader.Close();

            sqlCon.Close();
            return Products;
        }
        public static List<Products> FetchingProductDataT()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryProductT, sqlCon);

            List<Products> Products = new List<Products>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Products()
                {
                    BarCode = reader.GetString(0),
                    Title = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    ProductionSite = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Image = reader.IsDBNull(3) ? null : (byte[])(reader["Image"]),
                    Cost = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    Stock = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    ManufacturingDate = reader.GetDateTime(7),
                    ExpiryDate = reader.GetDateTime(8),
                    Discount = reader.IsDBNull(9) ? null : reader.GetDouble(9),
                    Type = reader.IsDBNull(10) ? null : reader.GetString(10),
                    InputInfoId = reader.GetInt32(11),
                });

            }
            reader.Close();

            sqlCon.Close();
            return Products;
        }
        public static void ThemErorr(Report t, string filepath)
        {
            var strCmd = string.Format(insertErorrs, t.Title, t.Description, t.Status, t.RepairCost, t.SubmittedAt, t.StaffId, filepath);
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

        public static void InsertBill(int? customerId, int? price, int? discount)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryInsertBill, sqlCon);
            cmd.Parameters.AddWithValue("@billDate", System.DateTime.Now);
            cmd.Parameters.AddWithValue("@cusId", (customerId == null ? DBNull.Value : customerId));
            cmd.Parameters.AddWithValue("@userId", CurrentAccount.idAccount);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@discount", discount);


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
        public static byte[] LoadProductAvatar(string id)
        {
            sqlCon.Open();
            var strCmd = string.Format(@"select Image from [Product] where Barcode=N'{0}'", id);

            byte[]? Avatar = new byte[byte.MaxValue];
            SqlCommand cmd = new(strCmd, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Avatar = reader.IsDBNull(0) ? null : (Byte[])reader["Image"];
            }
            reader.Close();
            sqlCon.Close();

            return Avatar;
        }
        public static void InsertReport(Report report)
        {
            sqlCon.Open();
            var cmd = new SqlCommand(insertReport, sqlCon);
            cmd.Parameters.AddWithValue("@Title", report.Title);
            cmd.Parameters.AddWithValue("@Description", report.Description);
            cmd.Parameters.AddWithValue("@Status", report.Status);
            cmd.Parameters.AddWithValue("@SubmittedAt", report.SubmittedAt);
            cmd.Parameters.AddWithValue("@RepairCost", report.RepairCost);
            cmd.Parameters.AddWithValue("@StaffId", report.StaffId);
            cmd.Parameters.AddWithValue("@Image", report.Image);
            cmd.ExecuteNonQuery();
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
                vouchers.Add(new Vouchers()
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
                if (vouchers[0].FinishDate < System.DateTime.Now)  //Quá hạn sử dụng
                    error = 1;
                else if (vouchers[0].StartDate > System.DateTime.Now)  //Chưa tới ngày áp dụng
                    error = 2;
                else if (vouchers[0].Type == 0)  //Giảm tiền mặt
                    discount = vouchers[0].ParValue.HasValue ? Convert.ToInt32(vouchers[0].ParValue) : 0;
                else if (vouchers[0].Type == 1) //Giảm %
                    discount = vouchers[0].ParValue.HasValue ? Convert.ToInt32(totalPrice * vouchers[0].ParValue / 100) : 0;
            }
            else
                error = 0;

            sqlCon.Close();
            return discount;
        }

        public static void UpdateVoucherStatus(string? code)
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

        public static void UpdateCustomerPointStatus(int? customerId, int? customerPoint)
        {
            if (customerId != null && customerPoint != null)
            {
                sqlCon.Open();
                //Tìm kiếm voucher
                var cmd = new SqlCommand(queryUpdateCustomerPoint, sqlCon);
                cmd.Parameters.AddWithValue("@customerId", customerId);
                cmd.Parameters.AddWithValue("@customerPoint", customerPoint);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        public static void UpdateReport(Report editedReport)
        {
            sqlCon.Open();
            var cmd = new SqlCommand(updateReport, sqlCon);
            cmd.Parameters.AddWithValue("@Title", editedReport.Title);
            cmd.Parameters.AddWithValue("@Image", editedReport.Image);
            cmd.Parameters.AddWithValue("@RepairCost", editedReport.RepairCost);
            cmd.Parameters.AddWithValue("@Description", editedReport.Description);
            cmd.Parameters.AddWithValue("@Id", editedReport.Id);
            //cmd.Parameters.AddWithValue("@SubmittedAt", editedReport.SubmittedAt);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static void UpdateReportAD(Report editedReport)
        {
            sqlCon.Open();
            //var strCmd = string.Format(updateReportFULL, editedReport.Title, editedReport.Image, editedReport.RepairCost, editedReport.Status, editedReport.StartDate, editedReport.FinishDate, editedReport.Description, editedReport.Id);
            //SqlCommand cmd = new(strCmd, sqlCon);
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();
            var cmd = new SqlCommand();
            if (editedReport.Status == "Chờ tiếp nhận")
                cmd = new SqlCommand(updateReportAD, sqlCon);
            else if (editedReport.Status == "Đang giải quyết")
            {
                cmd = new SqlCommand(updateReportADS, sqlCon);
                cmd.Parameters.AddWithValue("@StartDate", editedReport.StartDate);
            }
            else if (editedReport.Status == "Đã giải quyết")
            {
                cmd = new SqlCommand(updateReportADSF, sqlCon);
                cmd.Parameters.AddWithValue("@StartDate", editedReport.StartDate);
                cmd.Parameters.AddWithValue("@FinishDate", editedReport.FinishDate);
            }
            else
            {
                if (editedReport.StartDate == null && editedReport.FinishDate == null)
                    cmd = new SqlCommand(updateReportAD, sqlCon);
                else if (editedReport.FinishDate == null)
                {
                    cmd = new SqlCommand(updateReportADS, sqlCon);
                    cmd.Parameters.AddWithValue("@StartDate", editedReport.StartDate);
                }
                else
                {
                    cmd = new SqlCommand(updateReportADSF, sqlCon);
                    cmd.Parameters.AddWithValue("@StartDate", editedReport.StartDate);
                    cmd.Parameters.AddWithValue("@FinishDate", editedReport.FinishDate);
                }
            }
            cmd.Parameters.AddWithValue("@Title", editedReport.Title);
            cmd.Parameters.AddWithValue("@Image", editedReport.Image);
            cmd.Parameters.AddWithValue("@RepairCost", editedReport.RepairCost);
            cmd.Parameters.AddWithValue("@Status", editedReport.Status);
            cmd.Parameters.AddWithValue("@Description", editedReport.Description);
            cmd.Parameters.AddWithValue("@Id", editedReport.Id);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static List<Bills> FetchingBillsData()
        {
            sqlCon.Open();
            SqlCommand command = new SqlCommand(queryBillsData, sqlCon);
            List<Bills> list = new List<Bills>();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Bills()
                {
                    BillId = reader.GetInt32(0),
                    UserName = reader.IsDBNull(1) ? null : reader.GetString(1),
                    CustomerName = reader.IsDBNull(2) ? "Khách vãng lai" : reader.GetString(2),
                    BillDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    TotalPrice = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    UserId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    CustomerId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    Discount = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                });
            }

            sqlCon.Close();
            return list;
        }
        public static List<BillDetails> FetchingBillDetailsData(Bills BillInfo)
        {
            sqlCon.Open();
            SqlCommand command = new SqlCommand(queryBillDetailsData, sqlCon);
            command.Parameters.AddWithValue("@id", BillInfo.BillId);
            List<Model.Staff.BillDetails> list = new List<Model.Staff.BillDetails>();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Model.Staff.BillDetails()
                {
                    Quantity = reader.IsDBNull(0) ? null : reader.GetInt32(0),
                    Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                    TotalPrice = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                });
            }
            reader.Close();
            sqlCon.Close();
            return list;
        }

        public static void UpdateConsignmentStock(BillDetails b)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryUpdateConsignmentInStock, sqlCon);
            cmd.Parameters.AddWithValue("@quantity", b.Quantity);
            cmd.Parameters.AddWithValue("@productId", b.ProductId);
            cmd.Parameters.AddWithValue("@inputInfoId", b.InputInfoId);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        public static int GetCustomerPoint(int? customerId)
        {
            if (customerId == null)
                return 0;
            int customerPoint;
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryCustomerPoint, sqlCon);
            cmd.Parameters.AddWithValue("@customerId", customerId);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            customerPoint = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            reader.Close();
            sqlCon.Close();
            return customerPoint;
        }

        public static void InsertCustomerData(Customer customer)
        {
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand(queryInsertCustomer, sqlCon);
            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@address", customer.Address == null ? DBNull.Value : customer.Address);
            cmd.Parameters.AddWithValue("@phone", customer.Phone);
            cmd.Parameters.AddWithValue("@email", customer.Email == null ? DBNull.Value : customer.Email);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        public static ObservableCollection<Member> FetchTeamMembers(int id, int managerId)
        {
            sqlCon.Open();

            var cmd = new SqlCommand(queryTeamMembers, sqlCon);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@managerId", managerId);

            ObservableCollection<Member> members = new ObservableCollection<Member>();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                members.Add(new Member()
                {
                    Name = reader.GetString(0),
                    Avatar = (byte[])reader["Avatar"],
                    UserRole = reader.GetString(2),
                });
            }

            reader.Close();
            sqlCon.Close();
            return members;
        }
        public static void Throw(int i, string id)
        {
            var strCmd = string.Format(updateSL, i, id);
            sqlCon.Open();
            SqlCommand cmd = new(strCmd, sqlCon);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlCon.Close();
        }
        public static string GetName(int id)
        {
            var strCmd = string.Format(queryName, id);
            sqlCon.Open();
            string s = "Nguyễn Trọng Ninh";
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
    }
}
