using adatvez.DAL;
using ahk.common;
using System.Threading.Tasks;

namespace adatvez
{
    public class Program
    {
        public static Task<int> Main(string[] args)
            => AhkExecutionHelper.Execute(
                new AhkEvaluationTask("Database init", DbInit.InitializeDatabase, isPreProcess: true),
                new AhkEvaluationTask(Feladat1.AhkExerciseName, Feladat1.Execute),
                new AhkEvaluationTask(Feladat2.AhkExerciseName, Feladat2.Execute));
    }
}
