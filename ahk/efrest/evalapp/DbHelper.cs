using ahk.common;
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

        public static TryResult<int> TryAddStatusRecord(this TasksDbContext dbContext, string name, AhkResult ahkResult)
        {
            var nameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (nameProperty == null)
            {
                ahkResult.AddProblem("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");
                return TryResult<int>.Failed();
            }

            var idProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(int) && p.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (idProperty == null)
            {
                ahkResult.AddProblem("DbStatus.Id property nem talalhato. DbStatus.Id property does not exist.");
                return TryResult<int>.Failed();
            }

            var newInstance = new DbStatus();
            nameProperty.SetValue(newInstance, name);

            try
            {
                dbContext.GetStatusesDbSet().Add(newInstance);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "DbStatus rekord beszurasa DbContext-be sikertelen. Failed to add new DbStatus record.");
                return TryResult<int>.Failed();
            }

            return TryResult<int>.Ok((int)idProperty.GetValue(newInstance));
        }

        public static string ReadStatusRecordName(this DbStatus value, AhkResult ahkResult)
        {
            var nameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (nameProperty == null)
            {
                ahkResult.AddProblem("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");
                return string.Empty;
            }
            else
            {
                return (string)nameProperty.GetValue(value);
            }
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
