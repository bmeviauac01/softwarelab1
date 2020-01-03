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

        public static DbSet<DbStatus> GetStatusesDbSet(this TasksDbContext dbContext) => dbContext.Set<DbStatus>();

        public static DbSet<DbTask> GetTasksDbSet(this TasksDbContext dbContext) => dbContext.Set<DbTask>();

        public static TryResult<int> TryAddStatusRecord(this TasksDbContext dbContext, string name, System.Reflection.PropertyInfo idProperty, AhkResult ahkResult)
        {
            var nameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (nameProperty == null)
            {
                ahkResult.AddProblem("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");
                return TryResult<int>.Failed();
            }

            var newStatus = new DbStatus();
            try
            {
                nameProperty.SetValue(newStatus, name);

                dbContext.GetStatusesDbSet().Add(newStatus);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "DbStatus rekord beszurasa DbContext-be sikertelen. Failed to add new DbStatus record.");
                return TryResult<int>.Failed();
            }

            return TryResult<int>.Ok((int)idProperty.GetValue(newStatus));
        }

        public static TryResult<int> TryAddTaskRecord(this TasksDbContext dbContext, string name, System.Reflection.PropertyInfo idProperty, System.Reflection.PropertyInfo statusNavigationProperty, AhkResult ahkResult)
        {
            var possibleTitleProperties = typeof(DbTask).GetProperties().Where(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string)).ToList();
            if (possibleTitleProperties.Count == 0)
            {
                ahkResult.AddProblem("DbTask-ban nincs string Title property. DbTask has no string Title property.");
                return TryResult<int>.Failed();
            }
            if (possibleTitleProperties.Count > 1)
            {
                ahkResult.AddProblem("DbTask-ban tobb string property van, de csak egy Title kell (statusz nevet szovegkent tarolod?). DbTask has more than one string property, but only need a single Title (are you storing the status name too?).");
                return TryResult<int>.Failed();
            }
            var titleProperty = possibleTitleProperties.Single();

            var statusNameProperty = typeof(DbStatus).GetProperties().FirstOrDefault(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string) && p.Name.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (statusNameProperty == null)
            {
                ahkResult.AddProblem("DbStatus.Name property nem talalhato. DbStatus.Name property does not exist.");
                return TryResult<int>.Failed();
            }

            var newTask = new DbTask();
            var newAttachedStatus = new DbStatus();
            try
            {
                titleProperty.SetValue(newTask, name);
                statusNameProperty.SetValue(newAttachedStatus, name);
                statusNavigationProperty.SetValue(newTask, newAttachedStatus);

                dbContext.GetTasksDbSet().Add(newTask);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, "DbTask rekord beszurasa DbContext-be sikertelen. Failed to add new DbTask record.");
                return TryResult<int>.Failed();
            }

            return TryResult<int>.Ok((int)idProperty.GetValue(newTask));
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
