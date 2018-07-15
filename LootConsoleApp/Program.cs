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
        
        static void Main(string[] args)
        {
            
            int seletion;
            /*
            int b = Convert.ToInt32(args[0]);

            if (b == 1)
            {
                GetLootRecords();
                Environment.Exit(0);
            }
            */

            // simple menu
            Console.WriteLine("1. GetLootRecords");     
            Console.WriteLine("2. GetClanDetails");
            Console.WriteLine("3. GetMemberList");
            Console.Write("Enter Selection: ");
            string line = Console.ReadLine();            

            switch (seletion = Convert.ToInt32(line))
            {
                case 1:
                    GetLootRecords();
                    break;
                case 2:
                    GetClanDetails();
                    break;
                case 3:
                    GetMemberList();
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }  // end main

        //*********************** GET CLAN DETAILS ***********************
        private static void GetClanDetails()
        {
            ClanDetails fetchClan = new ClanDetails();
            try
            {
                fetchClan.GetClanDetails();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to execute 'GetClanDetails'");
            }
        }

        //*********************** GET LOOT RECORDS ***********************
        private static void GetLootRecords()
        {
            LootRecords fetchLoot = new LootRecords();
            try
            {
                fetchLoot.GetLootRecords();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to execute 'GetLootRecords'");
            }
        }

        //*********************** GET LOOT RECORDS ***********************
        private static void GetMemberList()
        {
            MemberList fetchMembers = new MemberList();
            try
            {
                fetchMembers.GetMemberList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to execute 'GetMemberList'");
            }
        }




    }  // end class Program
}  // end namespace
