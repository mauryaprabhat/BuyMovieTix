using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BuyTixApi.Data
{
    public static class BaseDB
    {
        public const string CONN = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Prabhat\MicrosoftTrainingPractice\BuyTix.Api\App_Data\BuyTixDB.mdf';Integrated Security=True; Connect Timeout=30";
        

        public static DataTable GetDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(CONN))
            {
                using (DataSet ds = new DataSet())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        public static bool ExecuteNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(CONN))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        public static bool ExecuteNonQuery(SqlConnection conn, string sql, SqlTransaction tr = null)
        {
            SqlCommand command = null;
            try
            {
                if (tr != null)
                    command = new SqlCommand(sql, conn, tr);
                else
                    command = new SqlCommand(sql, conn);

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            finally
            {
                if (command != null)
                    command.Dispose();
            }
        }

        public static Object ExecuteScalar(SqlConnection conn, string sql, SqlTransaction tr = null)
        {
            SqlCommand command = null;

            try
            {
                if (tr != null)
                    command = new SqlCommand(sql, conn, tr);
                else
                    command = new SqlCommand(sql, conn);

                command.CommandType = CommandType.Text;
                return command.ExecuteScalar();
            }
            finally
            {
                command.Dispose();
            }
        }
    }
}