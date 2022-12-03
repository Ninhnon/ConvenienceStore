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
    public class DataProvider : SQLConnect
    {
        public DataTable LoadData(string tableName)
        {
            DataTable dt = new DataTable();
            CloseConnection();
            try
            {
                OpenConnection();
                string query = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                CloseConnection();
                return dt;
            }
            catch
            {
                MessageBox.Show("Mat ket noi den CSDL");

            }
            return dt;

        }
    }
}
