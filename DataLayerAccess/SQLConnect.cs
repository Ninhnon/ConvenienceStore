using System;
using System.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConvenienceStore.DataLayerAccess
{
    public class SQLConnect
    {
        private string strConn;
        public SqlConnection conn;
        public SQLConnect()
        {
            try
            {
                strConn = "Data Source=DESKTOP-RTH9F0I;Initial Catalog=ConvenienceStore;Integrated Security=True";
            }
            catch
            {
                MessageBox.Show("Mat ket noi CSDL");
                return;
            }
            conn = new SqlConnection(strConn);
        }
        public void OpenConnection()
        {
            try
            {
                if(conn.State!=System.Data.ConnectionState.Open)
                {
                    conn.ConnectionString = "Data Source=DESKTOP-RTH9F0I;Initial Catalog=ConvenienceStore;Integrated Security=True";
                    conn.Open();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Mat ket noi CSDL");
                throw ex;
            }
        }
        public void CloseConnection()
        {
            conn.Close();
        }
    }
}
