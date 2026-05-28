using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShoeStore
{
    public static class DatabaseHelper
    {
        public static SqlConnection CreateConnection()
        {
            string cs = ConfigurationManager.ConnectionStrings["ShoeStoreDB"].ConnectionString;
            return new SqlConnection(cs);
        }

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            SqlConnection connection = CreateConnection();
            SqlCommand command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            connection.Close();
            return table;
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            SqlConnection connection = CreateConnection();
            SqlCommand command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            SqlConnection connection = CreateConnection();
            SqlCommand command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            connection.Open();
            object result = command.ExecuteScalar();
            connection.Close();
            return result;
        }
    }
}
