using ahk.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace adatvez.Helpers
{
    public static class EntityFrameworkModelHelper
    {
        public static TryResult<System.Reflection.PropertyInfo> TryGetPrimaryKey<T>(this DbContext dbContext, AhkResult ahkResult)
        {
            var typeName = typeof(T).Name;

            var entityPrimaryKeys = dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey();
            if (entityPrimaryKeys == null)
            {
                ahkResult.AddProblem($"{typeName} nem tartalmaz elsodleges kulcsot. {typeName} has no primary key.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }
            if (entityPrimaryKeys.Properties.Count != 1)
            {
                ahkResult.AddProblem($"{typeName} osszetett elsodleges kulcsot tartalmaz. {typeName} has composite primary key.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            var idProperty = entityPrimaryKeys.Properties.First().PropertyInfo;
            if (idProperty == null)
            {
                ahkResult.AddProblem($"{typeName} elsodleges kulcs property nem talalhato. {typeName} primary key property does not exist.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            ahkResult.Log($"{typeName} PK: {idProperty.Name}");
            return TryResult<System.Reflection.PropertyInfo>.Ok(idProperty);
        }

        public static TryResult<System.Reflection.PropertyInfo> TryGetForeignKey<TSource, TTarget>(this DbContext dbContext, AhkResult ahkResult)
        {
            var typeName = $"{typeof(TSource).Name} -> {typeof(TTarget).Name}";

            var entityForeignKeys = dbContext.Model.FindEntityType(typeof(TSource)).GetForeignKeys().Where(fk => fk.PrincipalKey?.DeclaringEntityType?.ClrType == typeof(TTarget)).ToList();
            if (entityForeignKeys.Count == 0)
            {
                ahkResult.AddProblem($"{typeName} kapcsolat hianyzik. {typeName} connection missing.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }
            if (entityForeignKeys.Count > 1)
            {
                ahkResult.AddProblem($"{typeName} kapcsolatbol van. {typeName} has multiple connections.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            if (entityForeignKeys.Single().Properties.Count != 1)
            {
                ahkResult.AddProblem($"{typeName} kulso kulcs property osszetett. {typeName} foreign key is composite.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            var fkProperty = entityForeignKeys.Single().Properties.First().PropertyInfo;
            if (fkProperty == null)
            {
                ahkResult.AddProblem($"{typeName} kulso kulcs property nem talalhato. {typeName} foreign key property missing.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            ahkResult.Log($"{typeName} FK: {fkProperty.Name}");
            return TryResult<System.Reflection.PropertyInfo>.Ok(fkProperty);
        }

        public static TryResult<System.Reflection.PropertyInfo> TryGetNavigationPropery<TSource, TTarget>(this DbContext dbContext, AhkResult ahkResult)
        {
            var typeName = $"{typeof(TSource).Name} -> {typeof(TTarget).Name}";

            var entityForeignKeys = dbContext.Model.FindEntityType(typeof(TSource)).GetForeignKeys().Where(fk => fk.PrincipalKey?.DeclaringEntityType?.ClrType == typeof(TTarget)).ToList();
            if (entityForeignKeys.Count == 0)
            {
                ahkResult.AddProblem($"{typeName} kapcsolat hianyzik. {typeName} connection missing.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }
            if (entityForeignKeys.Count > 1)
            {
                ahkResult.AddProblem($"{typeName} kapcsolatbol van. {typeName} has multiple connections.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            var navigationProperty = entityForeignKeys.Single().DependentToPrincipal?.PropertyInfo;
            if (navigationProperty == null)
            {
                ahkResult.AddProblem($"{typeName} navigation property nem talalhato. {typeName} navigation property missing.");
                return TryResult<System.Reflection.PropertyInfo>.Failed();
            }

            ahkResult.Log($"{typeName} navigation propery: {navigationProperty.Name}");
            return TryResult<System.Reflection.PropertyInfo>.Ok(navigationProperty);
        }
    }
}
