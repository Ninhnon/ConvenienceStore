using ConvenienceStore.Model.Lam;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ConvenienceStore.ViewModel.Lam.Helpers
{
    public class DatabaseHelper
    {
        /* strCon của Lâm
         * static readonly string strCon = @"Data Source=LAPTOP-JF6O07NR\SQLEXPRESS;Initial Catalog=ConvenienceStore;Integrated Security=True"; */
        static readonly string strCon = @"Data Source=LAPTOP-JF6O07NR\SQLEXPRESS;Initial Catalog = ConvenienceStore; Integrated Security = True";
        public static SqlConnection sqlCon = new SqlConnection(strCon);

        static readonly string queryInputInfo = @"select InputInfo.Id, InputDate, InputInfo.UserId, Users.Name, Users.Email, Users.Phone, Avatar, Supplier.Id, Supplier.Name
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

        static readonly string insertInputInfo = "insert InputInfo values ('{0}', {1}, {2})";

        static readonly string insertProduct = "insert Product values (@Barcode, @Title, @Image, @Type, @ProductionSite)";

        static readonly string insertConsignment = "insert Consignment values ({0}, '{1}', {2}, '{3}', '{4}', {5}, {6}, {7})";

        static readonly string insertSupplier = "insert Supplier values (N'{0}', N'{1}', '{2}', '{3}')";

        static readonly string insertUser = "insert Users values (@Role, @Name, @Addres, @Phone, @Email, @UserName, @Password, @Image)";

        static readonly string updateProduct = @"update Product set Title = @Title, Image = @Image
                                                 where BarCode = @Barcode";

        static readonly string updateConsignment = @"Update Consignment
                                                     set Stock = {0}, ManufacturingDate = '{1}', ExpiryDate = '{2}', 
                                                         InputPrice = {3}, OutputPrice = {4}, Discount = {5}
                                                     where InputInfoId = {6} and ProductId = '{7}'";

        static readonly string updateUser = @"update Users
                                              set UserRole = @UserRole, Name = @Name, Address = @Addres, Phone = @Phone, Email = @Email, Password = @Password, Image = @Image
                                              where Id = @Id";

        /* Delete toàn bộ Consignment liên trong InputInfo trước
         * Xong Delete InputInfo */
        static readonly string deleteInputInfo = @"delete Consignment where InputInfoId = {0}
                                                   delete InputInfo where Id = {0}";

        static readonly string deleteProduct = @"delete Consignment 
                                                 where InputInfoId = {0} and ProductId = '{1}'";

        static readonly string deleteSupplier = "delete Supplier where Id = {1}";

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
                        // Còn Avatar nữa nè
                        SupplerId = reader.GetInt32(7),
                        SupplierName = reader.GetString(8),
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

        public static ObservableCollection<Supplier> FetchingSupplier()
        {
            sqlCon.Open();
            var cmd = new SqlCommand(querySupllier, sqlCon);

            ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();

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

                    Address = reader.IsDBNull(3)?null:reader.GetString(3),
                    Phone = reader.IsDBNull(4) ? null:reader.GetString(4),
                    Email = reader.IsDBNull(5) ? null:reader.GetString(5),
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

        public static void Update(Product editedProduct)
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

        public static void InsertSupplier(Supplier supplier)
        {
            sqlCon.Open();

            var strCmd = string.Format(insertSupplier, supplier.Name, supplier.Address, supplier.Phone, supplier.Email);
            var cmd = new SqlCommand(strCmd, sqlCon);
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

        public static void DeleteSupplier(int supplierId)
        {
            sqlCon.Open();

            var strCmd = string.Format(deleteSupplier, supplierId);
            var cmd = new SqlCommand(strCmd, sqlCon);
            cmd.ExecuteNonQuery();

            sqlCon.Close();
        }
    }
}
