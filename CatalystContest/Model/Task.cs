using System.Collections.Generic;

namespace CatalystContest.Model
{
    public class Task
    {
        #region Inputs

        public int Id { get; set; }

        public int CompletionTime { get; set; }

        public int PowerNeeded { get; set; }

        public int AllowedStartInterval { get; set; }

        public int AllowedEndInterval { get; set; }

        #endregion

        #region Outputs & helpers

        public int MinuteDrawingPower { get; set; }

        public Dictionary<int, int> MinutePowerDrawn { get; set; } = new Dictionary<int, int>();

        public bool Completed { get; set; }

        #endregion

        public Task() { }

        public Task(int id, int powerNeeded, int allowedStartInterval, int allowedEndInterval)
        {
            Id = id;
            PowerNeeded = powerNeeded;
            AllowedStartInterval = allowedStartInterval;
            AllowedEndInterval = allowedEndInterval;
        }
    }
}
