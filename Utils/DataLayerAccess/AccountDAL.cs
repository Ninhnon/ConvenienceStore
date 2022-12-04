using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvenienceStore.Model;

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
        public List<Account> ConvertDataTableToList()
        {
            DataTable dt = new DataTable();

            List<Account> accounts = new List<Account>();
            try
            {
                dt = LoadData("Users");
            }
            catch
            {
                CloseConnection();
                dt = LoadData("Users");

            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Account acc = new Account(int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                                          dt.Rows[i].ItemArray[1].ToString(),
                                          dt.Rows[i].ItemArray[2].ToString(),
                                          dt.Rows[i].ItemArray[3].ToString(),
                                          dt.Rows[i].ItemArray[4].ToString(),
                                          dt.Rows[i].ItemArray[5].ToString(),
                                          dt.Rows[i].ItemArray[6].ToString(),
                                          dt.Rows[i].ItemArray[7].ToString(),
                                          (byte[])dt.Rows[i].ItemArray[8]
                );
                accounts.Add(acc);
            }
            return accounts;
        }
        public void AddIntoDataBase(Account account)
        {
            OpenConnection();
            string query = "INSERT INTO Account(idAccount, username, password, type) VALUES (@id, @username, @pass, @type)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", account.idAccount.ToString());
            cmd.Parameters.AddWithValue("@username", account.UserName);
            cmd.Parameters.AddWithValue("@pass", account.Password);

            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public bool DeleteAccount(string idAccount)
        {
            try
            {
                OpenConnection();
                string query = "delete from Account where IdAccount = " + idAccount;
                SqlCommand command = new SqlCommand(query, conn);
                if (command.ExecuteNonQuery() > 0)
                    return true;
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
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
