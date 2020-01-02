using api.DAL;
using api.DAL.EfDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace adatvez
{
    internal static class DbHelper
    {
        public static TasksDbContext GetDbContext(this TestRequestScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<TasksDbContext>();

            if (dbContext == null)
                throw new Exception("DbContext nem hozhato letre. Cannot instantiate the DbContext.");

            return dbContext;
        }

        public static DbSet<DbStatus> GetStatusesDbSet(this TasksDbContext dbContext)
            => dbContext.Set<DbStatus>();

        public static int AddStatusRecord(this TasksDbContext dbContext, string name)
        {
            var nameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (nameProperty == null)
                throw new Exception("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");

            var idProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(int) && p.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (idProperty == null)
                throw new Exception("DbStatus.Id property nem talalhato. DbStatus.Id property does not exist.");

            var newInstance = new DbStatus();
            nameProperty.SetValue(newInstance, name);

            dbContext.GetStatusesDbSet().Add(newInstance);
            dbContext.SaveChanges();

            var idValue = idProperty.GetValue(newInstance);
            if (idValue is int)
                return (int)idValue;
            else
                throw new Exception("DbStatus.Id property megfelelo. DbStatus.Id property invalid.");
        }

        public static string ReadStatusRecordName(this DbStatus value)
        {
            var nameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (nameProperty == null)
                throw new Exception("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");

            var nameValue = nameProperty.GetValue(value);
            if (nameValue is string)
                return (string)nameValue;
            else
                throw new Exception("DbStatus.Name property megfelelo. DbStatus.Name property invalid.");
        }

        public static IStatusesRepository GetStatusesRepository(this TestRequestScope scope)
        {
            var repo = scope.ServiceProvider.GetService<IStatusesRepository>();

            if (repo == null)
                throw new Exception("IStatusesRepository nem hozhato letre. Cannot instantiate IStatusesRepository.");

            return repo;
        }
    }
}
