using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Funq;
using CocNET;
using CocNET.Interfaces;

// Console App to Log CoC Loot values and store in DB
// John Harvey

namespace LootConsoleApp
{
    class Program
    {
        // class level variables to hold fetched loot values
        public static int totalGold, totalElixer, totalDark, trophies;
        
        static void Main(string[] args)
        {
            SqlConnection con;      // connection variables
            SqlCommand com;
            DateTime localDate = DateTime.Now;  // date now for loot logging date


            // helper code to delete rows when needed
            /*             
            string line;
            int rowRemove;

            do
            {
                Console.Write("Enter row Id to delete, or type exit: ");
                line = Console.ReadLine();
                if (line == "exit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    rowRemove = Convert.ToInt32(line);
                    DeleteRow(rowRemove);
                }
            } while (line != "exit");
            */


            try   // connect to clash API
            {
                Console.Write("Connecting to Clash API...");
                // set clash api connection token
                string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImFlNjY2YzJmLWUyNGQtNGUwZS1hNzU5LTcyZjU4NjRjODY5NSIsImlhdCI6MTUzMDMxMTE3Niwic3ViIjoiZGV2ZWxvcGVyLzA5MGJlMzBlLWFhNjEtN2Y0YS1iMjY1LTk3Mjg1NmEzZDVhOSIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE4LjE5MS4xMjcuODgiLCI3NS4xMzQuOTYuNDciXSwidHlwZSI6ImNsaWVudCJ9XX0.3-S8KOl2DjyO9dhPq4aNjfpyc3Mfei6a_YevQXV0btVU_-d6bJO1pBfuM-7LEtYN5ypXrjAyY5nDjtrHdbJBuQ";
                Funq.Container container = CocCore.Instance(token).Container;  // build new container with api token
                ICocCorePlayers playersCore = container.Resolve<ICocCorePlayers>();  // CoCNET interface config to players core
                Console.WriteLine(" Connection Established!");

                try  // fetch loot data
                {                   
                    Console.Write("Fetching loot data...");
                    var player = playersCore.GetPlayer("#98QGLCJCR");   //get JSON data for player tag
                    totalGold = player.Achievements.Find(x => x.Name == "Gold Grab").Value; // fetch gold grab achievement  
                    totalElixer = player.Achievements.Find(x => x.Name == "Elixir Escapade").Value; // fetch elixer escapade achievement
                    totalDark = player.Achievements.Find(x => x.Name == "Heroic Heist").Value; // fetch elixer escapade achievement
                    trophies = player.Trophies; // fetch player trophies
                    Console.WriteLine(" Loot data fetched!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Failed to fetch data");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to connect to API");
            }


            Console.WriteLine();
            Console.WriteLine("Fetched Loot Data to be logged - " + localDate.ToString());
            Console.WriteLine("Total gold value: " + totalGold.ToString());
            Console.WriteLine("Total elixer value: " + totalElixer.ToString());
            Console.WriteLine("Total dark value: " + totalDark.ToString());
            Console.WriteLine("Total trophies value: " + trophies.ToString());
            Console.WriteLine();

            try
            {
                using (con = new SqlConnection(Properties.Settings.Default.newLootConStr)) // new sql connection using the db connection string
                {
                    Console.Write("Establishing db connection...");
                    con.Open();     // open db for use
                    Console.WriteLine(" Connected to db!");

                    Console.Write("Creating sql command...");
                    // build sql insert command with loot values
                    using (com = new SqlCommand("INSERT INTO LootRecords(dateNow, gold, elixer, dark, trophies) VALUES(" + 
                        "@dateNow, @gold, @elixer, @dark, @trophies)", con))
                    {
                        com.Parameters.AddWithValue("dateNow", localDate);  // add values to sql command
                        com.Parameters.AddWithValue("gold", totalGold);
                        com.Parameters.AddWithValue("elixer", totalElixer);
                        com.Parameters.AddWithValue("dark", totalDark);
                        com.Parameters.AddWithValue("trophies", trophies);
                        Console.WriteLine("Values added to sql command!");
                        try
                        {
                            Console.Write("Inserting data into db...");
                            com.ExecuteNonQuery();  // execute INSERT command
                            Console.WriteLine(" Loot data successfully inserted to db!");
                            con.Close();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Failed to insert data to db");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to connect to db");
            }

            Console.WriteLine();
            Console.WriteLine("Operation Complete");
            Environment.Exit(0);

        }  // end main

        private static void DeleteRow(int id)
        {
            SqlConnection con;      // connection variables
            SqlCommand com;
            try
            {
                using (con = new SqlConnection(Properties.Settings.Default.newLootConStr)) // new sql connection using the db connection string
                {
                    Console.Write("Establishing db connection...");
                    con.Open();     // open db for use
                    Console.WriteLine(" Connected to db!");

                    Console.Write("Creating sql command...");
                    // build sql insert command with loot values
                    using (com = new SqlCommand("DELETE FROM LootRecords " +
                        "WHERE Id=@Id", con))
                    {
                        com.Parameters.AddWithValue("@Id", id);  // add values to sql command
                        
                        Console.WriteLine("Values added to sql command!");
                        try
                        {
                            Console.Write("Deleting row from db...");
                            com.ExecuteNonQuery();  // execute INSERT command
                            Console.WriteLine(" Loot data successfully removed from db!");
                            con.Close();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Failed to delete row from db");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to connect to db");
            }
        }



    }  // end class Program
}  // end namespace
