using System;
using ahk.adatvez.mssqldb;
using ahk.common;

namespace adatvez
{
    class Program
    {
        static int Main(string[] args)
        {
            return AhkExecutionHelper.Execute(
                        Tuple.Create<string, Action>(Feladat1.AhkExerciseName, DbInit.InitializeDatabase),
                        Tuple.Create<string, Action>(Feladat1.AhkExerciseName, Feladat1.Execute),
                        Tuple.Create<string, Action>(Feladat2.AhkExerciseName, Feladat2.Execute),
                        Tuple.Create<string, Action>(Feladat3.AhkExerciseName, Feladat3.Execute),
                        Tuple.Create<string, Action>(Feladat3.AhkExerciseName, Feladat4.Execute));
        }
    }
}
