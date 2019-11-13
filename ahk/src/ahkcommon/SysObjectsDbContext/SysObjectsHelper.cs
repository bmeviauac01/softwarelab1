using System;
using System.Linq;

namespace adatvez
{
    public static class SysObjectsHelper
    {
        public static bool TriggerExistsWithName(string triggerName, ref AhkResult result)
            => SysObjectExistsWithName(triggerName, "TR", "trigger", result);

        public static bool StoredProcedureExistsWithName(string precedureName, ref AhkResult result)
            => SysObjectExistsWithName(precedureName, "P", "tarolt eljaras", result);

        private static bool SysObjectExistsWithName(string objectName, string objectTypeIdInMssql, string objectTypeDescription, AhkResult result)
        {
            var sysobjectsDbContextOptions = DbFactory.GetDbOptions<SysObjectsDbContext.SysObjectsDbContext>();
            using (var db = new SysObjectsDbContext.SysObjectsDbContext(sysobjectsDbContextOptions))
            {
                var itemmmmsss = db.ListSysObjects().Where(obj => obj.Type.Equals(objectTypeIdInMssql, StringComparison.OrdinalIgnoreCase)).ToList();

                var item = db.ListSysObjects().FirstOrDefault(obj => obj.Type.Trim().Equals(objectTypeIdInMssql, StringComparison.OrdinalIgnoreCase) && obj.Name.Trim().Equals(objectName, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                {
                    result.AddProblem($"Nem letezik a {objectName} {objectTypeDescription}");
                    return false;
                }
                else
                {
                    result.Log($"{objectName} {objectTypeDescription} letezik");
                    return true;
                }
            }
        }
    }
}
