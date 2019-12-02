using System;

namespace ahk.common
{
    public class AhkEvaluationTask
    {
        public AhkEvaluationTask(string exerciseName, Action<AhkResult> executeAction, bool required = false)
        {
            this.ExerciseName = exerciseName;
            this.ExecuteAction = executeAction;
            this.Required = required;
        }

        public string ExerciseName { get; }
        public Action<AhkResult> ExecuteAction { get; }
        public bool Required { get; } = false;
    }
}
