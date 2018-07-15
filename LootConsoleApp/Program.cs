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

            GetClanDetails();

            Console.WriteLine("1. GetLootRecords");
            Console.WriteLine("2. GetClanDetails");




        }  // end main

        


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






    }  // end class Program
}  // end namespace
