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
    class MemberList
    {
        //public static int expLevel, leagueID, trophies, clanRank, donations, donationsReceived, versusTrophies;
        //public static string tag, name, leagueName, leagueIcon, role;

        public List<Member> myMemberList = new List<Member>(); // list to hold each member

        public MemberList()
        {

        }

        public void GetMemberList()
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
                    Console.Write("Fetching member details...");

                    var clan = clansCore.GetClans("#99L9R088");   //get JSON data for clan tag

                    

                    foreach (var obj in clan.MemberList)    // loop through each members details
                    {
                        Member member = new Member()
                        {
                            Tag = obj.Tag,
                            Name = obj.Name,
                            ExpLevel = obj.ExpLevel,
                            LeagueID = obj.League.Id,
                            LeagueName = obj.League.Name,
                            LeagueIcon = obj.League.IconUrls["medium"],
                            Trophies = obj.Trophies,
                            Role = obj.Role,
                            ClanRank = obj.ClanRank,
                            Donations = obj.Donations,
                            DonationsReceived = obj.DonationsReceived,
                            LocalDate = localDate
                        };   // new member for each loop, set the following details
                        myMemberList.Add(member);
                    }

                    Console.WriteLine("Member details fetched!");
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
                    using (com = new SqlCommand("AddMemberDetails", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;

                        foreach (var member in myMemberList)
                        {
                            com.Parameters.AddWithValue("@tag", member.Tag);  // add values to sql command
                            com.Parameters.AddWithValue("@name", member.Name);  // add values to sql command
                            com.Parameters.AddWithValue("@expLevel", member.ExpLevel);  // add values to sql command
                            com.Parameters.AddWithValue("@leagueID", member.LeagueID);  // add values to sql command
                            com.Parameters.AddWithValue("@leagueName", member.LeagueName);  // add values to sql command
                            com.Parameters.AddWithValue("@leagueIcon", member.LeagueIcon);  // add values to sql command
                            com.Parameters.AddWithValue("@trophies", member.Trophies);  // add values to sql command
                            com.Parameters.AddWithValue("@role", member.Role);  // add values to sql command
                            com.Parameters.AddWithValue("@clanRank", member.ClanRank);  // add values to sql command
                            com.Parameters.AddWithValue("@donations", member.Donations);  // add values to sql command
                            com.Parameters.AddWithValue("@donationsReceived", member.DonationsReceived);  // add values to sql command
                            com.Parameters.AddWithValue("@dateNow", member.LocalDate);  // add values to sql command

                            Console.WriteLine("Values added to sql command!");
                            try
                            {
                                Console.Write("Inserting member into db...");
                                com.ExecuteNonQuery();  // execute INSERT command
                                Console.WriteLine("Member successfully inserted to db!");
                                
                            }
                            catch (SqlException e)
                            {
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


    public class Member
    {        
        public string Tag { get; set; }       
        public string Name { get; set; }
        public int ExpLevel { get; set; }
        public int LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string LeagueIcon { get; set; }
        public int Trophies { get; set; }
        public string Role { get; set; }
        public int ClanRank { get; set; }
        public int Donations { get; set; }
        public int DonationsReceived { get; set; }        
        public DateTime LocalDate { get; set; }
    }

}
