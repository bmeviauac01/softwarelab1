using Microsoft.EntityFrameworkCore;

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
            var environmentConnString = System.Environment.GetEnvironmentVariable("DATABASE_CONNECTIONSTRING");
            if (!string.IsNullOrEmpty(environmentConnString))
                return environmentConnString;

#if DEBUG
            return @"Server=(localdb)\mssqllocaldb;Database=adatvez;Trusted_Connection=True;";
#else
            return "Server=mssqlserver;Database=adatvez;Trusted_Connection=True;";
#endif
        }
    }
}
