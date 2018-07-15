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

namespace LootConsoleApp
{
    class ClanDetails
    {
        public static int clanLevel, clanPoints, members, warWinStreak, warWins, requiredTrophies;
        public static string tag, name, badgeUrls, description, type, warFrequency;

        public ClanDetails()
        {

        }


        public void GetClanDetails()
        {
            SqlConnection con;      // connection variables
            SqlCommand com;
            DateTime localDate = DateTime.Now;  // date now for loot logging date 


            try   // connect to clash API
            {
                Console.Write("Connecting to Clash API...");
                // set clash api connection token
                string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImFlNjY2YzJmLWUyNGQtNGUwZS1hNzU5LTcyZjU4NjRjODY5NSIsImlhdCI6MTUzMDMxMTE3Niwic3ViIjoiZGV2ZWxvcGVyLzA5MGJlMzBlLWFhNjEtN2Y0YS1iMjY1LTk3Mjg1NmEzZDVhOSIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE4LjE5MS4xMjcuODgiLCI3NS4xMzQuOTYuNDciXSwidHlwZSI6ImNsaWVudCJ9XX0.3-S8KOl2DjyO9dhPq4aNjfpyc3Mfei6a_YevQXV0btVU_-d6bJO1pBfuM-7LEtYN5ypXrjAyY5nDjtrHdbJBuQ";
                Funq.Container container = CocCore.Instance(token).Container;  // build new container with api token

                ICocCoreClans clansCore = container.Resolve<ICocCoreClans>();  // CoCNET interface config to clans

                Console.WriteLine(" Connection Established!");

                try  // fetch loot data
                {
                    Console.Write("Fetching clan details...");

                    var clan = clansCore.GetClans("#99L9R088");   //get JSON data for clan tag

                    tag = clan.Tag;
                    name = clan.Name;
                    badgeUrls = clan.BadgeUrls["large"];
                    clanLevel = clan.ClanLevel;
                    clanPoints = clan.ClanPoints;
                    members = clan.Members;
                    warWinStreak = clan.WarWinStreak;
                    warWins = clan.WarWins;
                    description = clan.Decsription;
                    type = clan.Type;
                    requiredTrophies = clan.RequiredTrophies;
                    warFrequency = clan.WarFrequency;
                    

                    Console.WriteLine("Clan details fetched!");
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


            try
            {
                using (con = new SqlConnection(Properties.Settings.Default.newLootConStr)) // new sql connection using the db connection string
                {
                    Console.Write("Establishing db connection...");
                    con.Open();     // open db for use
                    Console.WriteLine(" Connected to db!");

                    Console.Write("Creating sql command...");
                    // build sql insert command with loot values
                    using (com = new SqlCommand("AddClanDetails", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@tag", tag);  // add values to sql command
                        com.Parameters.AddWithValue("@name", name);
                        com.Parameters.AddWithValue("@badgeUrls", badgeUrls);
                        com.Parameters.AddWithValue("@clanLevel", clanLevel);
                        com.Parameters.AddWithValue("@clanPoints", clanPoints);
                        com.Parameters.AddWithValue("@members", members);
                        com.Parameters.AddWithValue("@warWinStreak", warWinStreak);
                        com.Parameters.AddWithValue("@warWins", warWins);
                        com.Parameters.AddWithValue("@description", description);
                        com.Parameters.AddWithValue("@type", type);
                        com.Parameters.AddWithValue("@requiredTrophies", requiredTrophies);
                        com.Parameters.AddWithValue("@warFrequency", warFrequency);
                        com.Parameters.AddWithValue("@dateNow", localDate);

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

        }
    }
}
