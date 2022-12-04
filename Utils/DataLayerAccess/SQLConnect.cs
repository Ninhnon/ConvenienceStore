﻿using ConvenienceStore.Utils.Helpers;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

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
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.ConnectionString = DatabaseHelper.sqlCon.ConnectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
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
