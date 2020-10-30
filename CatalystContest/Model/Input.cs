using System.Collections.Generic;

namespace CatalystContest.Model
{
    public class Input
    {
        public int MaxPowerPerMinute { get; set; }
        public long MaxElectricityBill { get; set; }
        public long MaxConcurrentTasksPerMinute { get; set; }
        public List<int> Price { get; set; }
        public List<Task> Tasks { get; set; }

        public Input()
        {
            Price = new List<int>();
            Tasks = new List<Task>();
        }
    }
}
