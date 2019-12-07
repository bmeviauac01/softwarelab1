using ahk.adatvez.mssqldb;
using ahk.common;

namespace adatvez
{
    class Program
    {
        static int Main(string[] args)
        {
            return AhkExecutionHelper.Execute(
                        new AhkEvaluationTask(Feladat1.AhkExerciseName, DbInit.InitializeDatabase, isPreProcess: true),
                        new AhkEvaluationTask(Feladat1.AhkExerciseName, Feladat1.Execute),
                        new AhkEvaluationTask(Feladat2.AhkExerciseName, Feladat2.Execute),
                        new AhkEvaluationTask(Feladat3.AhkExerciseName, Feladat3.Execute),
                        new AhkEvaluationTask(Feladat4.AhkExerciseName, Feladat4.Execute));
        }
    }
}
