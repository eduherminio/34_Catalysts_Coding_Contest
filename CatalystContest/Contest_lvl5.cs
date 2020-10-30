using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl5
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            while (true)
            {
                ulong totalElectricityBill = ulong.MaxValue;
                var minuteConsumption = new Dictionary<int, int>(Enumerable.Range(0, input.Price.Count).Select(n => new KeyValuePair<int, int>(n, 0)));
                var minuteConcurrentTasks = new Dictionary<int, int>(Enumerable.Range(0, input.Price.Count).Select(n => new KeyValuePair<int, int>(n, 0)));

                foreach (var task in input.Tasks)
                {
                    var allowedMinutes = input.Price
                        .GetRange(task.AllowedStartInterval, task.AllowedEndInterval - task.AllowedStartInterval + 1)
                        .Select((n, index) => new { price = n, index })
                        .Where(minute =>
                            minuteConsumption[task.AllowedStartInterval + minute.index] < input.MaxPowerPerMinute
                            && minuteConcurrentTasks[task.AllowedStartInterval + minute.index] < input.MaxConcurrentTasksPerMinute)
                        .ToList();

                    var orderedAllowedMinutes = allowedMinutes.OrderBy(p => p.price).ToList();

                    var powerToSatisfyLeft = task.PowerNeeded;

                    foreach (var minute in orderedAllowedMinutes)
                    {
                        var realMinuteIndex = task.AllowedStartInterval + minute.index;
                        var powerLeftForMinuteX = input.MaxPowerPerMinute - minuteConsumption[realMinuteIndex];

                        ++minuteConcurrentTasks[realMinuteIndex];
                        if (powerLeftForMinuteX >= powerToSatisfyLeft)
                        {
                            minuteConsumption[realMinuteIndex] += powerToSatisfyLeft;
                            task.MinutePowerDrawn.Add(realMinuteIndex, powerToSatisfyLeft);
                            totalElectricityBill += Convert.ToUInt64(minute.price) * Convert.ToUInt64(powerToSatisfyLeft);
                            task.Completed = true;
                            break;
                        }
                        else
                        {
                            minuteConsumption[realMinuteIndex] = input.MaxPowerPerMinute;
                            powerToSatisfyLeft -= powerLeftForMinuteX;
                            task.MinutePowerDrawn.Add(realMinuteIndex, powerLeftForMinuteX);
                            totalElectricityBill += Convert.ToUInt64(minute.price) * Convert.ToUInt64(powerLeftForMinuteX);
                        }

                    }
                }

                if (totalElectricityBill < Convert.ToUInt64(input.MaxElectricityBill)
                    && input.Tasks.All(t => t.Completed))
                {
                    break;
                }

                input.Tasks.ForEach(t => t.MinutePowerDrawn.Clear());
                input.Tasks = input.Tasks.OrderBy(t => t.AllowedEndInterval - t.AllowedStartInterval).ToList();
            }

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Tasks.Count}");
            foreach (var task in input.Tasks.OrderBy(t => t.Id))
            {
                var minutesConsumption = string.Join(' ', task.MinutePowerDrawn.Select(pair => $"{pair.Key} {pair.Value}"));
                sw.WriteLine($"{task.Id} {minutesConsumption}");
            }
        }

        private Input ParseInput(string level)
        {
            var input = new Input();
            var file = new ParsedFile($"Inputs/{level}");

            input.MaxPowerPerMinute = file.NextLine().NextElement<int>();
            input.MaxElectricityBill = file.NextLine().NextElement<long>();
            input.MaxConcurrentTasksPerMinute = file.NextLine().NextElement<int>();

            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine();

                input.Price.Add(line.NextElement<int>());
            }
            numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine();

                input.Tasks.Add(new Task(line.NextElement<int>(), line.NextElement<int>(), line.NextElement<int>(), line.NextElement<int>()));
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }

            return input;
        }
    }
}
