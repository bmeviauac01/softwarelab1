using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ahk.adatvez.mssqldb
{
    public static class DbFactory
    {
        public static DatabaseContext.AdatvezDbContext GetDatabase()
        {
            var options = GetDbOptions<DatabaseContext.AdatvezDbContext>();
            return new DatabaseContext.AdatvezDbContext(options);
        }

        public static DbContextOptions<TDbContext> GetDbOptions<TDbContext>()
            where TDbContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseSqlServer(GetConnectionString(), options => options.CommandTimeout(10).EnableRetryOnFailure());
            return optionsBuilder.Options;
        }

        public static string GetConnectionString()
        {
#if DEBUG
            return @"Server=(localdb)\mssqllocaldb;Database=adatvez;Trusted_Connection=True;";
#endif

            var environmentConnString = System.Environment.GetEnvironmentVariable("DATABASE_CONNECTIONSTRING");
            if (!string.IsNullOrEmpty(environmentConnString))
            {
                var connStrBuilder = new SqlConnectionStringBuilder(environmentConnString);
                connStrBuilder.ConnectTimeout = 3;
                connStrBuilder.InitialCatalog = "adatvez";
                return connStrBuilder.ConnectionString;
            }

            throw new System.Exception("Nincs konfiguralva az adatbazis eleres.");
        }
    }
}
