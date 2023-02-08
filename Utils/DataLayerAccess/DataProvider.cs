using System;
using System.Data;
using System.Data.SqlClient;

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
            catch (Exception ex)
            {
                throw ex;

            }
            return dt;

        }
    }
}
