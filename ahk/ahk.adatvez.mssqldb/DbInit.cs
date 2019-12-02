using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace ahk.adatvez.mssqldb
{
    public static class DbInit
    {
        public static void WaitForSqlServer()
        {
            Console.WriteLine("MSSQL szerver kapcsolodas folyamatban...");

            var connStr = new SqlConnectionStringBuilder(DbFactory.GetConnectionString());
            connStr.InitialCatalog = ""; // force connection to the server and not database; there is no database yet

            var waitUntil = DateTime.UtcNow.AddMinutes(1);
            while (true)
            {
                try
                {
                    using (var conn = new SqlConnection(connStr.ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "select 1 from sys.tables";
                            cmd.ExecuteNonQuery();
                        }

                        Console.WriteLine("MSSQL szerver kapcsolodas sikeres");
                        return;
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(5);

                    if (DateTime.UtcNow > waitUntil)
                    {
                        Console.WriteLine("MSSQL szerver kapcsolodas SIKERTELEN; ez valoszinuleg nem a megoldas hibaja");
                        throw;
                    }
                }
            }
        }

        public static void InitializeDatabase()
        {
            WaitForSqlServer();

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
