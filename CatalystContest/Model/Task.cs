using System.Collections.Generic;

namespace CatalystContest.Model
{
    public class Task
    {
        public int Id { get; set; }
        // 2
        public int CompletionTime { get; set; }

        // 3
        public int PowerNeeded { get; set; }
        public int AllowedStartInterval { get; set; }
        public int AllowedEndInterval { get; set; }


        // 2
        public int MinuteStartDrawingPower { get; set; }

        // 3
        public int MinuteDrawingPower { get; set; }

        public bool Completed { get; set; }
        // 4
        public Dictionary<int, int> MinutePowerDrawn { get; set; } = new Dictionary<int, int>();

        public Task(int id, int completionTime)
        {
            Id = id;
            CompletionTime = completionTime;
        }

        public Task(int id, int powerNeeded, int allowedStartInterval, int allowedEndInterval)
        {
            Id = id;
            PowerNeeded = powerNeeded;
            AllowedStartInterval = allowedStartInterval;
            AllowedEndInterval = allowedEndInterval;
        }
    }
}
