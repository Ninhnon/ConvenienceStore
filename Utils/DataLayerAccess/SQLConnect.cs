using ConvenienceStore.Utils.Helpers;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ConvenienceStore.Utils.DataLayerAccess
{
    public class SQLConnect
    {
        private string strConn;
        public SqlConnection conn;
        public SQLConnect()
        {
            try
            {
                strConn = @ConfigurationManager.ConnectionStrings["Default"].ToString(); ;
            }
            catch (Exception ex)
            {
                throw ex;
                return;
            }
            conn = new SqlConnection(strConn);
        }
        public void OpenConnection()
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.ConnectionString = DatabaseHelper.sqlCon.ConnectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void CloseConnection()
        {
            conn.Close();
        }
    }
}
