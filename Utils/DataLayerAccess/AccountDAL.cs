using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ConvenienceStore.Model;
using ConvenienceStore.Utils.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ConvenienceStore.Utils.DataLayerAccess
{
    public class AccountDAL : DataProvider
    {
        private static AccountDAL instance;
        public static AccountDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountDAL();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public AccountDAL()
        {

        }
        public List<string> ConvertDBToListString()
        {

            List<string> accounts = new List<string>();
            DataTable dt = new DataTable();
            CloseConnection();
            try
            {
                OpenConnection();
                string sql = "SELECT name FROM Users where userrole=1";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                CloseConnection();

            }
            catch
            {

            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string acc = dt.Rows[i].ItemArray[0].ToString();
                accounts.Add(acc);
            }
            return accounts;
        }
        public List<Account> ConvertDataTableToListEmployee()
        {
            List<Account> accounts = DatabaseHelper.FetchingAccountEmployeeData();
            return accounts;
        }
        public List<Account> ConvertDataTableToList()
        {
            List<Account> accounts = DatabaseHelper.FetchingAccountData();
            return accounts;
        }
        public void UpdatePassword(string newPass, string email)
        {
            OpenConnection();
            string query = "use conveniencestore Update Users set Password=@pass where email=@email";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pass", newPass);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.ExecuteNonQuery();
            CloseConnection();

        }
        public void UpdateManager (int id, int newId)
        {
            OpenConnection();
            string query = "Update Users set ManagerId=@newId where id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@newId", newId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public void AddIntoDataBase(Account account)
        {
            OpenConnection();
            string query = "INSERT INTO Users(UserRole,Name,Address,Phone,Email,UserName,Password,Avatar,ManagerId) VALUES (@userrole, @name,@address,@phone,@email,@username,@pass,@avatar,@managerid)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userrole", account.UserRole.ToString());
            cmd.Parameters.AddWithValue("@name", account.Name);
            cmd.Parameters.AddWithValue("@address", account.Address);
            cmd.Parameters.AddWithValue("@phone", account.Phone);
            cmd.Parameters.AddWithValue("@email", account.Email);
            cmd.Parameters.AddWithValue("@username", account.UserName);
            cmd.Parameters.AddWithValue("@pass", account.Password);
            cmd.Parameters.AddWithValue("@avatar", account.Avatar);
            cmd.Parameters.AddWithValue("@managerid", account.ManagerId);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
       
        public void DeleteAccount(int idAccount)
        {
         
                OpenConnection();
                string query = "delete from Users where Id = " + idAccount;
                SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
                   
                CloseConnection();
            
        }
     
        public void setNewPass(string pass, string email)
        {
            OpenConnection();
            string query = String.Format("Update Users set Password=\'{0}\' where Email=\'{1}\'",pass , email);
           

          
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public int GetSalary(int id)
        {
            int salary = 0;
            try
            {
                OpenConnection();
                string queryString = String.Format("select salary from Users where id={0}",id.ToString());
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataReader reader = command.ExecuteReader();
               while(reader.Read())
                {
                    salary = reader.GetInt32(0);
                }    


                    return salary;
                
            }
            catch
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
        public void SetNewSalary(int salary, int id)
        {

            OpenConnection();
            string query = string.Format("update users set salary={0} where id={1}", salary.ToString(), id.ToString());
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            CloseConnection();
        }
        public int SetNewID()
        {
            try
            {
                OpenConnection();
                string queryString = "select max(idAccount) from Account";
                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return int.Parse(dataTable.Rows[0].ItemArray[0].ToString()) + 1;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
