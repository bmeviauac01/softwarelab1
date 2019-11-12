using System;
using System.Linq;

namespace adatvez
{
    public static class SysObjectsHelper
    {
        public static bool TriggerExistsWithName(string triggerName, ref AhkResult result)
            => SysObjectExistsWithName(triggerName, "TR", "trigger", result);

        private static bool SysObjectExistsWithName(string objectName, string objectTypeIdInMssql, string objectTypeDescription, AhkResult result)
        {
            var sysobjectsDbContextOptions = DbFactory.GetDbOptions<SysObjectsDbContext.SysObjectsDbContext>();
            using (var db = new SysObjectsDbContext.SysObjectsDbContext(sysobjectsDbContextOptions))
            {
                var trigger = db.ListSysObjects().FirstOrDefault(obj => obj.Type.Equals(objectTypeIdInMssql, StringComparison.OrdinalIgnoreCase) && obj.Name.Equals(objectName, StringComparison.OrdinalIgnoreCase));
                if (trigger == null)
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
