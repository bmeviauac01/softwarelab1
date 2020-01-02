using System;
using System.Threading.Tasks;

namespace ahk.common
{
    public class AhkEvaluationTask
    {
        public AhkEvaluationTask(string exerciseName, Action<AhkResult> execute, bool isPreProcess = false)
        {
            this.ExerciseName = exerciseName;
            this.Execute = ahkResult => Task.Run(() => execute(ahkResult));
            this.IsPreProcess = isPreProcess;
        }

        public AhkEvaluationTask(string exerciseName, Func<AhkResult, Task> execute, bool isPreProcess = false)
        {
            this.ExerciseName = exerciseName;
            this.Execute = execute;
            this.IsPreProcess = isPreProcess;
        }

        public string ExerciseName { get; }
        public Func<AhkResult, Task> Execute { get; }
        public bool IsPreProcess { get; } = false;
    }
}
