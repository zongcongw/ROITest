using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using DryIoc;
using System.Data;
using System.Data.SqlClient;

namespace Rehm2024.Tools
{
    public class Sqlhelp
    {

        public static void UpdateTable()
        {


            string dbPath = @"D:\Rehm\Rehm.DB"; // 注意前面的@符号，它允许你使用单个反斜杠  
            string connectionString = $"Data Source={dbPath}";

            
            DataTable dt = ExecuteDataTable(connectionString, "SELECT * FROM Statetime");

            int count=int.Parse( dt.Rows[0][3].ToString());

            count++;

            ExecuteDataTable(connectionString, "UPDATE Statetime SET times='"+count+"' WHERE status= 'Running'");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();



                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Statetime", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader[0]}, {reader[1]}");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                
                using (SQLiteDataAdapter ap = new SQLiteDataAdapter("SELECT * FROM Statetime", conn))
                {
                    DataSet ds = new DataSet();
                    ap.Fill(ds);

                    
                }
            }


        }


        public static DataTable ExecuteDataTable(string connString, string cmdTxt, params SqlParameter[] sp)
        {
            DataTable table = new DataTable();

            try
            {
                using (SQLiteConnection sqlConn = new SQLiteConnection(connString))
                {
                    sqlConn.Open();
                    using (SQLiteCommand comm = new SQLiteCommand(cmdTxt, sqlConn))
                    {
                        comm.CommandType = System.Data.CommandType.Text;
                        comm.Parameters.AddRange(sp);
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm);
                        adapter.Fill(table);

                    }
                }
            }
            catch (Exception )
            {

            }
            return table;
        }

    }
}