using System;

namespace adatvez
{
    class Program
    {
        static int Main(string[] args)
        {
            return AhkExecutionHelper.Execute(initializeDatabase: true,
                        Tuple.Create<string, Action>(Feladat1.AhkExerciseName, Feladat1.Execute),
                        Tuple.Create<string, Action>(Feladat2.AhkExerciseName, Feladat2.Execute));
        }
    }
}
