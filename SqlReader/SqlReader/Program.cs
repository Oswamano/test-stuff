using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string queryString = "SELECT TOP 10000 [Speed (mph)],[Mean TT (Min)],[DATETIME] FROM[BWT].[dbo].['PBCA Cars$']";
            string connectionString = "Data Source=SYSTEMS2A\\SQLXSYSTEMS2A;Initial Catalog=BWT;Integrated Security=True";
            List<string> dates = new List<string>();
            List<string> minutes = new List<string>();
            List<string> speeds = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

               
                int x = 0;
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format(reader[0].ToString() + "/" + reader[1].ToString() + "/" + reader[2].ToString()));// etc
                        x++;
                        speeds.Add(reader[0].ToString());
                        minutes.Add(reader[1].ToString());
                        dates.Add(reader[2].ToString());
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
                int counter = 0;
                int yes = 0;
                int no = 0;
                while (counter < dates.Count)
                    
                {

                    string sql = @"INSERT INTO [BWT].[dbo].[inserts](NAME,MINUTES,DATE) Values(@name, @minutes, @date)";
                    
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    string insertSql = @"INSERT INTO Comment(TicketID, Name, Comments, UserID, Date)
                     Values(@ID, @Name, @Comment, @UniqueID, @Date)";
                    int number = 0;
                    bool result = Int32.TryParse(speeds[counter], out number);
                    if (result)
                    {
                        if (number > 0)
                        {
                            cmd.Parameters.AddWithValue("@name", speeds[counter]);
                            yes++;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@name", "1");
                            no++;
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@name", "1");
                        no++;
                    }
                    cmd.Parameters.AddWithValue("@minutes",minutes[counter]);
                    cmd.Parameters.AddWithValue("@date", dates[counter]);
                    Console.WriteLine(sql);
                    
                    cmd.ExecuteNonQuery();
                    counter++;
                }
                

                Console.WriteLine("The value of x is" + x.ToString());
                Console.WriteLine("The length of dates is: " + dates.Count.ToString());
                Console.WriteLine("value 69 of dates is: " + dates[69]);
                Console.WriteLine("Yes: " + yes);
                Console.WriteLine("No: " + no);
                Console.ReadKey();
            }
        }
    }
}


