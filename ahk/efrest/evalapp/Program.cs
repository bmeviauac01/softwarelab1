using ahk.common;
using System.Threading.Tasks;

namespace adatvez
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await AhkExecutionHelper.Execute(
                        new AhkEvaluationTask(WebAppInit.AhkExerciseName, WebAppInit.StartWebApp, isPreProcess: true),
                        new AhkEvaluationTask(Feladat1.AhkExerciseName, Feladat1.Execute),
                        new AhkEvaluationTask(Feladat2.AhkExerciseName, Feladat2.Execute),
                        new AhkEvaluationTask(Feladat3.AhkExerciseName, Feladat3.Execute),
                        new AhkEvaluationTask(Feladat4.AhkExerciseName, Feladat4.Execute));
        }
    }
}
