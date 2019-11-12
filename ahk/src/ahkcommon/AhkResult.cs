using System;
using System.Collections.Generic;

namespace adatvez
{
    public class AhkResult
    {
        private readonly List<string> log = new List<string>();

        public AhkResult(string exerciseName)
            => this.ExerciseName = exerciseName;

        public string ExerciseName { get; }
        public int Points { get; private set; } = 0;
        public IReadOnlyCollection<string> LogLines => log;

        public void AddProblem(string description)
        {
            Console.WriteLine(description);
            log.Add(description);
        }

        public void AddProblem(Exception ex, string description)
        {
            Log(ex);
            log.Add(description);
        }

        public void Log(Exception ex)
        {
            if (ex != null)
            {
                Log(ex.Message);
                if (ex.InnerException != null)
                    Log(ex.InnerException);
            }
        }

        public void Log(string description)
        {
            Console.WriteLine(description);
            log.Add(description);
        }

        public void AddPoints(int pointToAdd)
            => Points += pointToAdd;
    }
}
