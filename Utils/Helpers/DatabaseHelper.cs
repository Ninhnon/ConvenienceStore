using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using static Emgu.CV.BarcodeDetector;

namespace ConvenienceStore.Utils.Helpers
{
    public partial class DatabaseHelper
    {
        static readonly string strCon = @ConfigurationManager.ConnectionStrings["Default"].ToString();
        public static SqlConnection sqlCon = new SqlConnection(strCon);

        static readonly string queryInputInfo = @"select InputInfo.Id, InputDate, InputInfo.UserId, Users.Name, Users.Email, Users.Phone, Avatar, Supplier.Name
                                                  from InputInfo, Users, Supplier
                                                  where InputInfo.UserId = Users.Id and InputInfo.SupplierId = Supplier.Id
                                                  order by InputDate asc";

        static readonly string queryProducts = @"select Barcode, Title, ProductionSite, Image, Stock, InputPrice, OutputPrice, ManufacturingDate, ExpiryDate, Discount
                                                 from Consignment, Product
                                                 where InputInfoId = {0} and ProductId = Barcode";

        static readonly string queryProductTableViaBarcode = @"select Title, Image 
                                                               from Product 
                                                               where Barcode = '{0}'";

        static readonly string querySupllier = "select * from Supplier";

        static readonly string queryManagers = "select * from Users where UserRole = 1";

        static readonly string queryNewestInputInfoId = "select MAX(Id) from InputInfo";

        static readonly string queryNewestSupplierId = "select MAX(Id) from Supplier";

        static readonly string queryStaffOnTeam = @"select Name, Avatar from Users
                                                    where ManagerId = {0}";

        static readonly string queryAccountUsers = "select * from Users";

        static readonly string insertInputInfo = "insert InputInfo values ('{0}', {1}, {2})";

        static readonly string insertProduct = "insert Product values (@Barcode, @Title, @Image, @Type, @ProductionSite)";

        static readonly string insertConsignment = "insert Consignment values ({0}, '{1}', {2}, '{3}', '{4}', {5}, {6}, {7})";

        static readonly string insertSupplier = "insert Supplier values (N'{0}', N'{1}', '{2}', '{3}')";

        static readonly string updateProduct = @"update Product set Title = @Title, Image = @Image
                                                 where BarCode = @Barcode";

        static readonly string updateConsignment = @"Update Consignment
                                                     set Stock = {0}, ManufacturingDate = '{1}', ExpiryDate = '{2}', 
                                                         InputPrice = {3}, OutputPrice = {4}, Discount = {5}
                                                     where InputInfoId = {6} and ProductId = '{7}'";

        static readonly string updateSupplier = @"update Supplier
                                                  set Name = N'{0}', Address = N'{1}', Phone = '{2}', Email = '{3}'
                                                  where Id = {4}";

        static readonly string updateAvatar = @"update Users
                                                set Avatar = @Avatar
                                                where Id = @Id";

        /* Delete toàn bộ Consignment liên trong InputInfo trước
         * Xong Delete InputInfo */
        static readonly string deleteInputInfo = @"delete Consignment where InputInfoId = {0}
                                                   delete InputInfo where Id = {0}";

        static readonly string deleteProduct = @"delete Consignment 
                                                 where InputInfoId = {0} and ProductId = '{1}'";

        static readonly string countInputInfoHasSupplierId = @"select COUNT(*) from InputInfo
                                                               where SupplierId = {0}";

        static readonly string deleteSupplier = "delete Supplier where Id = {0}";

        public static List<InputInfo> FetchingInputInfo()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryInputInfo, sqlCon);

            List<InputInfo> inputInfos = new List<InputInfo>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                inputInfos.Add(
                    new InputInfo()
                    {
                        Id = reader.GetInt32(0),
                        InputDate = reader.GetDateTime(1),
                        UserId = reader.GetInt32(2),
                        UserName = reader.GetString(3),
                        Email = reader.GetString(4),
                        Phone = reader.GetString(5),
                        Avatar =  (byte[])reader["Avatar"],
                        SupplierName = reader.GetString(7),
                    }
                );
            }
            reader.Close();

            sqlCon.Close();

            for (int i = 0; i < inputInfos.Count; ++i)
            {
                inputInfos[i].products = FetchingProductsData(inputInfos[i].Id);
            }

            return inputInfos;
        }

        //public static DateTime? SafeGetDateTime(SqlDataReader reader, int colIndex)
        //{
        //    if (!reader.IsDBNull(colIndex))
        //        return reader.GetDateTime(colIndex);
        //    return null;
        //}

        public static List<Product> FetchingProductsData(int InputInfoId)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryProducts, InputInfoId);
            var cmd = new SqlCommand(strCmd, sqlCon);

            List<Product> Products = new List<Product>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Product()
                {
                    InputInfoId = InputInfoId,
                    Barcode = reader.GetString(0),
                    Title = reader.GetString(1),
                    ProductionSite = reader.GetString(2),
                    Image = (byte[])reader["Image"],
                    Stock = reader.GetInt32(4),
                    Cost = reader.GetInt32(5),
                    Price = reader.GetInt32(6),
                    ManufacturingDate = reader.GetDateTime(7),
                    ExpiryDate = reader.GetDateTime(8),
                    Discount = reader.GetInt32(9),
                });

            }
            reader.Close();

            sqlCon.Close();
            return Products;
        }

        public static List<Supplier> FetchingSupplier()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(querySupllier, sqlCon);

            List<Supplier> suppliers = new List<Supplier>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                suppliers.Add(
                    new Supplier()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        Phone = reader.GetString(3),
                        Email = reader.GetString(4),
                    });
            }

            for (int i = 0; i < suppliers.Count; ++i)
            {
                suppliers[i].Number = i + 1;
            }

            sqlCon.Close();
            return suppliers;
        }

        public static ObservableCollection<Manager> FetchingManagers()
        {
            sqlCon.Open();

            var managers = new ObservableCollection<Manager>();

            var cmd = new SqlCommand(queryManagers, sqlCon);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                managers.Add(new Manager()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(2),
                    Address = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Email = reader.GetString(5),
                });
            }

            sqlCon.Close();
            return managers;
        }

        public static (string, byte[]) FetchingProductTableViaBarcode(string Barcode)
        {
            sqlCon.Open();

            var strCmd = string.Format(queryProductTableViaBarcode, Barcode);
            var cmd = new SqlCommand(strCmd, sqlCon);
            var reader = cmd.ExecuteReader();

            string title = null;
            byte[] image = null;

            if (reader.Read())
            {
                title = reader.GetString(0);
                image = (byte[])reader["Image"];
            }

            reader.Close();
            sqlCon.Close();
            return (title, image);
        }

        public static int NewestInputInfoId()
        {
            sqlCon.Open();

            var cmd = new SqlCommand(queryNewestInputInfoId, sqlCon);

            var reader = cmd.ExecuteReader();

            reader.Read();
            var newestId = reader.GetInt32(0);
            reader.Close();

            sqlCon.Close();
            return newestId;
        }

        public static List<Account> FetchingAccountData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryUser, sqlCon);
            List<Account> accounts = new List<Account>();

            SqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {
                accounts.Add(new Account()
                {
                    IdAccount = read.GetInt32(0),
                    UserRole = read.GetString(1),
                    Name = read.GetString(2),
                    Address = read.GetString(3),
                    Phone = read.GetString(4),
                    Email = read.GetString(5),
                    UserName = read.GetString(6),
                    Password = read.GetString(7),
                    Avatar = (byte[])(read["Avatar"])
                });

            }

            sqlCon.Close();
            return accounts;
        }

        public static void InsertInputInfo(DateTime dateTime, int UserId, int SupplierId)
        {
            sqlCon.Open();

            var strCmd = string.Format(insertInputInfo, dateTime, UserId, SupplierId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static ObservableCollection<Member> QueryStaffOnTeam(int ManagerId)
        {
            sqlCon.Open();

            var strCmd = string.Format(queryStaffOnTeam, ManagerId);
            var cmd = new SqlCommand(strCmd, sqlCon);

            ObservableCollection<Member> staffs = new ObservableCollection<Member>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                staffs.Add(new Member()
                {
                    Name = reader.GetString(0),
                    Avatar = (byte[])reader["Avatar"],
                });
            }
            reader.Close();
            sqlCon.Close();
            return staffs;
        }

        public static void InsertProduct(Product product)
        {
            sqlCon.Open();

            // Bảng Consignment chứa khóa ngoại tham chiếu với khóa chính Barcode trong bảng Product

            // Xem thử loại sản phẩm đó đã tồn tại trong bảng Product hay chưa
            var strCmd = $"select * from Product where Barcode = '{product.Barcode}'";
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

            if (isExisted) // Nếu đã tồn tại, chỉ cần update lại 1 vài thông tin về loại sản phẩm
            {
                cmd = new SqlCommand(updateProduct, sqlCon);
                cmd.Parameters.AddWithValue("@Title", product.Title);
                cmd.Parameters.AddWithValue("@Image", product.Image);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);

                cmd.ExecuteNonQuery();
            }
            else // Nếu chưa, tạo mới loại sản phẩm
            {
                cmd = new SqlCommand(insertProduct, sqlCon);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@Title", product.Title);
                cmd.Parameters.AddWithValue("@Image", product.Image);
                cmd.Parameters.AddWithValue("@Type", "Khác");
                cmd.Parameters.AddWithValue("@ProductionSite", product.ProductionSite);

                cmd.ExecuteNonQuery();
            }

            reader.Close();

            // Xử lí trường hợp trong Title có dấu nháy đơn (')
            int i = 0;
            string title = product.Title;
            while (i < title.Length)
            {
                if (title[i] == '\'')
                {
                    title = title.Insert(i, "'");
                    i++;
                }
                i++;
            }

            // Thêm Consignment vào bảng Consignment
            strCmd = string.Format(insertConsignment,
                product.InputInfoId,
                product.Barcode,
                product.Stock,
                product.ManufacturingDate,
                product.ExpiryDate,
                product.Cost,
                product.Price,
                product.Discount
            );

            cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static int NewestSupplierId()
        {
            sqlCon.Open();

            var cmd = new SqlCommand(queryNewestSupplierId, sqlCon);

            var reader = cmd.ExecuteReader();

            reader.Read();
            var newestId = reader.GetInt32(0);

            reader.Close();
            sqlCon.Close();
            return newestId;

        }

        public static void InsertSupplier(Supplier supplier)
        {
            sqlCon.Open();

            var strCmd = string.Format(insertSupplier, supplier.Name, supplier.Address, supplier.Phone, supplier.Email);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static void UpdateProduct(Product editedProduct)
        {
            sqlCon.Open();

            // Update values in Product table
            var cmd = new SqlCommand(updateProduct, sqlCon);

            cmd.Parameters.AddWithValue("@Title", editedProduct.Title);
            cmd.Parameters.AddWithValue("@Image", editedProduct.Image);
            cmd.Parameters.AddWithValue("@Barcode", editedProduct.Barcode);

            cmd.ExecuteNonQuery();

            // Update values in Consignment table
            var strCmd = string.Format(updateConsignment,
                editedProduct.Stock,
                editedProduct.ManufacturingDate,
                editedProduct.ExpiryDate,
                editedProduct.Cost,
                editedProduct.Price,
                editedProduct.Discount,
                editedProduct.InputInfoId,
                editedProduct.Barcode);

            cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static void UpdateSupplier(Supplier editedSupplier)
        {
            sqlCon.Open();
            var strCmd = string.Format(updateSupplier, 
                editedSupplier.Name, 
                editedSupplier.Address, 
                editedSupplier.Phone, 
                editedSupplier.Email,
                editedSupplier.Id);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static void UpdateProfileAvatar(byte[] image)
        {
            sqlCon.Open();

            var cmd = new SqlCommand(updateAvatar, sqlCon);

            cmd.Parameters.AddWithValue("@Avatar", image);
            cmd.Parameters.AddWithValue("@Id", CurrentAccount.idAccount);

            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }
        public static void DeleteInputInfo(int inputInfoId)
        {
            sqlCon.Open();

            var strCmd = string.Format(deleteInputInfo, inputInfoId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static void DeleteProduct(int InputInfoId, string Barcode)
        {
            sqlCon.Open();

            var strCmd = string.Format(deleteProduct, InputInfoId, Barcode);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }

        public static bool CanDeleteSupplier(int supplierId)
        {
            sqlCon.Open();

            var strCmd = string.Format(countInputInfoHasSupplierId, supplierId);
            var cmd = new SqlCommand(strCmd, sqlCon);

            var reader = cmd.ExecuteReader();

            reader.Read();
            var count = reader.GetInt32(0);

            reader.Close();
            sqlCon.Close();

            return count == 0;
        }

        public static void DeleteSupplier(int supplierId)
        {
            sqlCon.Open();

            var strCmd = string.Format(deleteSupplier, supplierId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }
    
        public static void GetAvatarViaId(int Id)
        {
            sqlCon.Open();

            

            sqlCon.Close();
        }
    }
}
