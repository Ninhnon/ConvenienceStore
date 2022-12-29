using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;

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

        static readonly string queryProducts = @"select Barcode, Title, ProductionSite, Image, Stock, InputPrice, OutputPrice, ManufacturingDate, ExpiryDate, Discount, Type, InStock
                                                 from Consignment, Product
                                                 where InputInfoId = {0} and ProductId = Barcode";

        static readonly string queryProductTableViaBarcode = @"select Title, Image, Type
                                                               from Product 
                                                               where Barcode = '{0}'";

        static readonly string querySupllier = "select * from Supplier";

        static readonly string queryManagers = "select * from Users where UserRole = 1";

        static readonly string queryNewestInputInfoId = "select MAX(Id) from InputInfo";

        static readonly string queryNewestSupplierId = "select MAX(Id) from Supplier";

        static readonly string queryNewestBlockVoucherId = "select Max(Id) from BlockVoucher";

        static readonly string queryStaffOnTeam = @"select Name, Avatar from Users
                                                    where ManagerId = {0} and Id <> {0}";

        static readonly string queryAccountUsers = "select * from Users";
        static readonly string queryAccountUsersEmployee = "select * from Users where UserRole=0 ";
        static readonly string queryAccountAdmin = "select * from Users where Id={0} ";

        static readonly string queryBlockVoucher = @"select * from BlockVoucher
                                                     order by FinishDate";

        static readonly string queryVoucherViaBlockId = @"select Code, Status from Voucher
                                                          where BlockId = {0}";

        static readonly string querySmallProductWithOutImage = @"select Barcode, Title, Type, ProductionSite, SUM(InStock), COUNT(InputInfoId) from Product, Consignment
                                                                 where ProductId = Barcode
                                                                 group by Barcode, Title, Type, ProductionSite";

        static readonly string queryImageProductViaBarcode = @"select Image from Product
                                                               where Barcode = '{0}'";

        static readonly string queryConsignmentViaBarcode = @"select Id, InStock, ExpiryDate, Discount 
                                                              from InputInfo, Consignment 
                                                              where Id = InputInfoId and ProductId = '{0}'";

        static readonly string insertInputInfo = "insert InputInfo values ('{0}', {1}, {2})";

        static readonly string insertProduct = "insert Product values (@Barcode, @Title, @Image, @Type, @ProductionSite)";

        static readonly string insertConsignment = "insert Consignment values ({0}, '{1}', {2}, '{3}', '{4}', {5}, {6}, {7}, {8})";

        static readonly string insertSupplier = "insert Supplier values (N'{0}', N'{1}', '{2}', '{3}')";

        static readonly string insertBlockVoucher = "insert BlockVoucher values ('{0}', {1}, {2},'{3}', '{4}')";

        static readonly string insertVoucher = "insert Voucher values ('{0}', {1}, {2})";

        static readonly string updateProduct = @"update Product set Title = @Title, Image = @Image, Type = @Type
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

        static readonly string deleteBlockVoucher = @"delete Voucher where BlockId = {0}
                                                      delete BlockVoucher where Id = {0}";

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
                        Avatar = (byte[])reader["Avatar"],
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
                    Discount = reader.GetDouble(9) * 100,
                    Type = reader.GetString(10),
                    InStock = reader.GetInt32(11)
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
            reader.Close();

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
            reader.Close();
            sqlCon.Close();
            return managers;
        }

        public static (string, byte[], string) FetchingProductTableViaBarcode(string Barcode)
        {
            sqlCon.Open();

            var strCmd = string.Format(queryProductTableViaBarcode, Barcode);
            var cmd = new SqlCommand(strCmd, sqlCon);
            var reader = cmd.ExecuteReader();

            string title = null;
            byte[] image = null;
            string type = null;

            if (reader.Read())
            {
                title = reader.GetString(0);
                image = (byte[])reader["Image"];
                type = reader.GetString(2);
            }

            reader.Close();
            sqlCon.Close();
            return (title, image, type);
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
        public static void UpdateEmployee(string name, string address, string phone, string email, byte[] avatar, int managerid, int id)
        {
            sqlCon.Open();
            string query = "update users set Name=@name,Address=@address,Phone=@phone,Email=@email,Avatar=@avatar,ManagerId=@managerid where id=@id";


            var cmd = new SqlCommand(query, sqlCon);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@email", email);

            cmd.Parameters.AddWithValue("@avatar", avatar);
            cmd.Parameters.AddWithValue("@managerid", managerid);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static Account FetchingAccounAdminWithIdData(int Id)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryAccountAdmin, Id);
            var cmd = new SqlCommand(strCmd, sqlCon);

            Account account = new Account();

            SqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {
                account = new Account()
                {
                    IdAccount = read.GetInt32(0),

                    UserRole = read.GetString(1),
                    Name = read.GetString(2),
                    Address = read.GetString(3),
                    Phone = read.GetString(4),
                    Email = read.GetString(5),
                    UserName = read.GetString(6),
                    Password = read.GetString(7),
                    Avatar = (byte[])(read["Avatar"]),
                    ManagerId = read.GetInt32(9),
                };
            }
            read.Close();

            sqlCon.Close();
            return account;
        }
        public static List<Account> FetchingAccountEmployeeData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryAccountUsersEmployee, sqlCon);
            List<Account> accounts = new List<Account>();
            int i = 1;
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
                    Avatar = (byte[])(read["Avatar"]),
                    ManagerId = read.GetInt32(9),
                    Salary = read.GetInt32(10)

                });
                accounts[i - 1].Number = i;
                i++;
            }
            read.Close();

            sqlCon.Close();
            return accounts;
        }
        public static List<Account> FetchingAccountEmployeeAdminData()
        {
            string query = string.Format("select * from users where managerid={0}", CurrentAccount.idAccount);
            sqlCon.Open();
            var cmd = new SqlCommand(query, sqlCon);
            List<Account> accounts = new List<Account>();
            int i = 1;
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
                    Avatar = (byte[])(read["Avatar"]),
                    ManagerId = read.GetInt32(9),
                    Salary = read.GetInt32(10)

                });
                accounts[i - 1].Number = i;
                i++;
            }
            read.Close();

            sqlCon.Close();
            return accounts;
        }
        public static List<Account> FetchingAccountData()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(queryAccountUsers, sqlCon);
            List<Account> accounts = new List<Account>();
            int i = 1;
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
                    Avatar = (byte[])(read["Avatar"]),
                    ManagerId = read.GetInt32(9)
                });
                accounts[i - 1].Number = i;
                i++;
            }
            read.Close();

            sqlCon.Close();
            return accounts;
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

        public static List<BlockVoucher> FetchingBlockVoucherData()
        {
            sqlCon.Open();

            var cmd = new SqlCommand(queryBlockVoucher, sqlCon);

            var blockVouchers = new List<BlockVoucher>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                blockVouchers.Add(new BlockVoucher()
                {
                    Id = reader.GetInt32(0),
                    ReleaseName = reader.GetString(1),
                    Type = reader.GetInt32(2),
                    ParValue = reader.GetInt32(3),
                    StartDate = reader.GetDateTime(4),
                    FinishDate = reader.GetDateTime(5),
                });
            }
            reader.Close();

            for (int i = 0; i < blockVouchers.Count; i++)
            {
                blockVouchers[i].vouchers = FetchingVoucherData(blockVouchers[i].Id);
            }

            sqlCon.Close();

            return blockVouchers;
        }

        public static List<Voucher> FetchingVoucherData(int blockId)
        {
            var strCmd = string.Format(queryVoucherViaBlockId, blockId);
            var cmd = new SqlCommand(strCmd, sqlCon);

            var vouchers = new List<Voucher>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vouchers.Add(new Voucher()
                {
                    Code = reader.GetString(0),
                    Status = reader.GetInt32(1)
                });
            }
            reader.Close();

            return vouchers;
        }

        public static List<SmallProduct> FetchingSmallProductData()
        {
            sqlCon.Open();

            List<SmallProduct> smallProducts = new List<SmallProduct>();

            var cmd = new SqlCommand(querySmallProductWithOutImage, sqlCon);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var smallProduct = new SmallProduct()
                {
                    Barcode = reader.GetString(0),
                    Title = reader.GetString(1),
                    Type = reader.GetString(2),
                    ProductionSite = reader.GetString(3),
                    Stock = reader.GetInt32(4),
                };

                smallProducts.Add(smallProduct);
            }
            reader.Close();
            sqlCon.Close();

            for (int i = 0; i < smallProducts.Count; ++i)
            {
                smallProducts[i].ObservableSmallConsignments = FetchingConsignmentViaBarcode(smallProducts[i].Barcode);
                smallProducts[i].Image = FetchingImageViaBarcode(smallProducts[i].Barcode);
            }

            return smallProducts;
        }

        static ObservableCollection<SmallConsignment> FetchingConsignmentViaBarcode(string Barcode)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryConsignmentViaBarcode, Barcode);
            var cmd = new SqlCommand(strCmd, sqlCon);

            var list = new ObservableCollection<SmallConsignment>();

            SqlDataReader reader = cmd.ExecuteReader();

            int index = 0;

            while (reader.Read())
            {
                list.Add(new SmallConsignment()
                {
                    Id = reader.GetInt32(0),
                    InStock = reader.GetInt32(1),
                    ExperyDate = reader.GetDateTime(2),
                    Discount = reader.GetDouble(3) * 100
                });
            }

            reader.Close();
            sqlCon.Close();

            return list;
        }

        static byte[] FetchingImageViaBarcode(string Barcode)
        {
            sqlCon.Open();
            var strCmd = string.Format(queryImageProductViaBarcode, Barcode);
            var cmd = new SqlCommand(strCmd, sqlCon);

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            byte[] image = (byte[])reader["Image"];

            reader.Close();
            sqlCon.Close();

            return image;
        }

        public static void InsertInputInfo(DateTime dateTime, int UserId, int SupplierId)
        {
            sqlCon.Open();

            var strCmd = string.Format(insertInputInfo, dateTime, UserId, SupplierId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
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
                cmd.Parameters.AddWithValue("@Type", product.Type);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);

                cmd.ExecuteNonQuery();
            }
            else // Nếu chưa, tạo mới loại sản phẩm
            {
                cmd = new SqlCommand(insertProduct, sqlCon);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@Title", product.Title);
                cmd.Parameters.AddWithValue("@Image", product.Image);
                cmd.Parameters.AddWithValue("@Type", product.Type);
                cmd.Parameters.AddWithValue("@ProductionSite", product.ProductionSite);

                cmd.ExecuteNonQuery();
            }

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
                product.Discount,
                // Ban đầu InStock = Stock
                product.Stock
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

        public static int NewestBlockVoucherId()
        {
            sqlCon.Open();

            var cmd = new SqlCommand(queryNewestBlockVoucherId, sqlCon);
            var reader = cmd.ExecuteReader();

            reader.Read();
            var newestId = reader.GetInt32(0);

            reader.Close();
            sqlCon.Close();
            return newestId;
        }

        public static void InsertBlockVoucher(BlockVoucher blockVoucher)
        {
            sqlCon.Open();

            var strCmd = string.Format(insertBlockVoucher,
                blockVoucher.ReleaseName,
                blockVoucher.Type,
                blockVoucher.ParValue,
                blockVoucher.StartDate,
                blockVoucher.FinishDate
            );
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
            blockVoucher.Id = NewestBlockVoucherId();

            sqlCon.Open();
            for (int i = 0; i < blockVoucher.vouchers.Count; i++)
            {
                InsertVoucher(blockVoucher.vouchers[i].Code, blockVoucher.Id);
            }

            sqlCon.Close();
        }

        public static void InsertVoucher(string Code, int BlockId)
        {
            var strCmd = string.Format(insertVoucher, Code, 0, BlockId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateProduct(Product editedProduct)
        {
            sqlCon.Open();

            // Update values in Product table
            var cmd = new SqlCommand(updateProduct, sqlCon);

            cmd.Parameters.AddWithValue("@Title", editedProduct.Title);
            cmd.Parameters.AddWithValue("@Image", editedProduct.Image);
            cmd.Parameters.AddWithValue("@Type", editedProduct.Type);
            cmd.Parameters.AddWithValue("@Barcode", editedProduct.Barcode);

            cmd.ExecuteNonQuery();

            // Update values in Consignment table
            var strCmd = string.Format(updateConsignment,
                editedProduct.Stock,
                editedProduct.ManufacturingDate,
                editedProduct.ExpiryDate,
                editedProduct.Cost,
                editedProduct.Price,
                editedProduct.Discount * 1.0 / 100,
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

        public static void DeleteBlockVoucher(int BlockId)
        {
            sqlCon.Open();

            var strCmd = string.Format(deleteBlockVoucher, BlockId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }
    }
}
