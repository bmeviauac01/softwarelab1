using System;

namespace ahk.common
{
    public class AhkEvaluationTask
    {
        public AhkEvaluationTask(string exerciseName, Action<AhkResult> executeAction, bool isPreProcess = false)
        {
            this.ExerciseName = exerciseName;
            this.ExecuteAction = executeAction;
            this.IsPreProcess = isPreProcess;
        }

        public string ExerciseName { get; }
        public Action<AhkResult> ExecuteAction { get; }
        public bool IsPreProcess { get; } = false;
    }
}
