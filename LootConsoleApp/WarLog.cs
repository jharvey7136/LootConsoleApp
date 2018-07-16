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
    class WarLog
    {
        public List<War> myWarLogList = new List<War>(); // list to hold each member
        

        public WarLog()
        {

        }


        public void GetWarLog()
        {
            SqlConnection con;      // connection variables
            SqlCommand com;
            DateTime localDate = DateTime.Now;  // date now for loot logging date 

            try   // connect to clash API
            {
                Console.WriteLine();
                Console.Write("Connecting to Clash API...");
                // set clash api connection token
                string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImFlNjY2YzJmLWUyNGQtNGUwZS1hNzU5LTcyZjU4NjRjODY5NSIsImlhdCI6MTUzMDMxMTE3Niwic3ViIjoiZGV2ZWxvcGVyLzA5MGJlMzBlLWFhNjEtN2Y0YS1iMjY1LTk3Mjg1NmEzZDVhOSIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE4LjE5MS4xMjcuODgiLCI3NS4xMzQuOTYuNDciXSwidHlwZSI6ImNsaWVudCJ9XX0.3-S8KOl2DjyO9dhPq4aNjfpyc3Mfei6a_YevQXV0btVU_-d6bJO1pBfuM-7LEtYN5ypXrjAyY5nDjtrHdbJBuQ";
                Funq.Container container = CocCore.Instance(token).Container;  // build new container with api token
                ICocCoreClans clansCore = container.Resolve<ICocCoreClans>();  // CoCNET interface config to clans
                Console.WriteLine("     Connection Established!");

                try  // fetch loot data
                {
                    Console.WriteLine();
                    Console.Write("Fetching war logs...");
                    var clanWarLog = clansCore.GetClanWarLogs("#99L9R088");   //get JSON data for clan tag 

                    foreach (var obj in clanWarLog)    // loop through each members details
                    {                        
                        War log = new War();                        
                        log.Result = obj.Result;
                        log.EndTime = obj.EndTime;
                        log.TeamSize = obj.TeamSize;
                        log.ClanTag = obj.Clan.Tag;
                        log.ClanName = obj.Clan.Name;
                        log.ClanBadge = obj.Clan.BadgeUrls["medium"];
                        log.ClanLevel = obj.Clan.ClanLevel;
                        log.Attacks = obj.Clan.Attacks;
                        log.Stars = obj.Clan.Stars;
                        log.ExpEarned = obj.Clan.ExpEarned;                        
                        log.OppTag = obj.Opponent.Tag;
                        log.OppName = obj.Opponent.Name;
                        log.OppBadge = obj.Opponent.BadgeUrls["medium"];
                        log.OppClanLevel = obj.Opponent.ClanLevel;
                        log.OppAttacks = obj.Opponent.Attacks;
                        log.OppStars = obj.Opponent.Stars;
                        log.OppExpEarned = obj.Opponent.ExpEarned;
                        log.DateNow = localDate;
                        myWarLogList.Add(log);
                    }
                    //Console.WriteLine();
                    Console.WriteLine("     War logs fetched!");
                }
                catch (Exception e)
                {
                    Console.WriteLine();
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
            //Console.WriteLine();

            /*
            foreach (var member in myMemberList)
            {
                Console.WriteLine(member.Name);
            }
            */

            try
            {
                using (con = new SqlConnection(Properties.Settings.Default.newLootConStr)) // new sql connection using the db connection string
                {
                    Console.Write("Establishing db connection...");
                    con.Open();     // open db for use
                    Console.WriteLine("     Connected to db!");

                    foreach (var war in myWarLogList)
                    {
                        //Console.WriteLine();
                        Console.Write("Creating sql command...");
                        // build sql insert command with loot values

                        using (com = new SqlCommand("AddWarLog", con))
                        {
                            com.CommandType = CommandType.StoredProcedure;

                            com.Parameters.AddWithValue("@result", war.Result);  // add values to sql command
                            com.Parameters.AddWithValue("@endTime", war.EndTime);
                            com.Parameters.AddWithValue("@teamSize", war.TeamSize);
                            com.Parameters.AddWithValue("@clanTag", war.ClanTag);
                            com.Parameters.AddWithValue("@clanName", war.ClanName);

                            com.Parameters.AddWithValue("@clanBadge", war.ClanBadge);
                            com.Parameters.AddWithValue("@clanLevel", war.ClanLevel);
                            com.Parameters.AddWithValue("@attacks", war.Attacks);
                            com.Parameters.AddWithValue("@stars", war.Stars);
                            com.Parameters.AddWithValue("@expEarned", war.ExpEarned);

                            com.Parameters.AddWithValue("@oppTag", war.OppTag);
                            com.Parameters.AddWithValue("@oppName", war.OppName);
                            com.Parameters.AddWithValue("@oppBadge", war.OppBadge);
                            com.Parameters.AddWithValue("@oppClanLevel", war.OppClanLevel);
                            com.Parameters.AddWithValue("@oppAttacks", war.OppAttacks);

                            com.Parameters.AddWithValue("@oppStars", war.OppStars);
                            com.Parameters.AddWithValue("@oppExpEarned", war.OppExpEarned);
                            com.Parameters.AddWithValue("@dateNow", localDate);

                            Console.WriteLine("     Values added to sql command!");
                            try
                            {
                                //Console.WriteLine();
                                Console.Write("Inserting log into db...");
                                com.ExecuteNonQuery();  // execute INSERT command
                                Console.WriteLine("    Log successfully inserted to db!");

                            }
                            catch (SqlException e)
                            {
                                Console.WriteLine();
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Failed to insert data to db");
                            }
                        }

                    }
                    con.Close();
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


    public class War
    {
        public string Result { get; set; }
        public string EndTime { get; set; }
        public int TeamSize { get; set; }
        public string ClanTag { get; set; }
        public string ClanName { get; set; }
        public string ClanBadge { get; set; }
        public int ClanLevel { get; set; }
        public int Attacks { get; set; }
        public int Stars { get; set; }
        public int ExpEarned { get; set; }
        public string OppTag { get; set; }
        public string OppName { get; set; }
        public string OppBadge { get; set; }
        public int OppClanLevel { get; set; }
        public int OppAttacks { get; set; }
        public int OppStars { get; set; }
        public int OppExpEarned { get; set; }
        public DateTime DateNow { get; set; }
    }
}
