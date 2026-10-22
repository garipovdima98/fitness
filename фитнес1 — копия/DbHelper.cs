using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Фитнес
{
    public static class DbHelper
    {
        private static readonly string ConnStr =
            ConfigurationManager.ConnectionStrings["FitnessDB"].ConnectionString;

        public static DataTable Query(string sql, params object[] parameters)
        {
            using (var conn = new OleDbConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue("?", p);

                    var dt = new DataTable();
                    new OleDbDataAdapter(cmd).Fill(dt);

                    conn.Close();
                    return dt;
                }
            }
        }

        public static int Execute(string sql, params object[] parameters)
        {
            using (var conn = new OleDbConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue("?", p);

                    int result = cmd.ExecuteNonQuery();

                    conn.Close();
                    return result;
                }
            }
        }

        public static object Scalar(string sql, params object[] parameters)
        {
            using (var conn = new OleDbConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue("?", p);

                    object result = cmd.ExecuteScalar();

                    conn.Close();
                    return result;
                }
            }
        }
    }
}