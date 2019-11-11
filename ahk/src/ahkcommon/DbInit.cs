using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace adatvez
{
    internal static class DbInit
    {
        public static void InitializeDatabase()
        {
            var createDbSql = getDatabaseInitScript();
            createAndInitDatabase(createDbSql);
            validateDatabase();
        }

        private static string getDatabaseInitScript()
        {
            string createDbSql;
            if (File.Exists("/mssql.sql"))
            {
                createDbSql = File.ReadAllText("/mssql.sql");
                Console.WriteLine("Adatbazis init script beolvasva");
            }
            else
            {
                using (var rdr = new StreamReader(new HttpClient().GetStreamAsync(@"https://raw.githubusercontent.com/bmeviauac01/gyakorlatok/master/mssql.sql").Result))
                    createDbSql = rdr.ReadToEnd();
                Console.WriteLine("Adatbazis init script letoltve");
            }

            return createDbSql;
        }

        private static void createAndInitDatabase(string createDbSql)
        {
            try
            {
                using (var db = DbFactory.GetDatabase())
                {
                    db.Database.EnsureCreated();
                    Console.WriteLine("Adatbazis letrehozva");

                    db.Database.ExecuteSqlCommand(createDbSql);
                    Console.WriteLine("Adatbazis init script lefuttatva");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Az adatbazis letrehozasa nem sikerult", ex);
            }
        }

        private static void validateDatabase()
        {
            try
            {
                using (var db = DbFactory.GetDatabase())
                {
                    if (!db.Termek.Any())
                    {
                        throw new Exception("Az adatbazis ures?!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Adatbazis nem erheto el", ex);
            }
        }
    }
}
