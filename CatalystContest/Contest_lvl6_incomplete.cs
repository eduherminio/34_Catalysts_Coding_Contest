using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl6
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            input.Tasks = input.Tasks.OrderBy(t => t.AllowedEndInterval - t.AllowedStartInterval).ToList();

            while (true)
            {
                ulong totalElectricityBill = ulong.MaxValue;
                var minuteConsumption = new Dictionary<int, int>(Enumerable.Range(0, input.Price.Count).Select(n => new KeyValuePair<int, int>(n, 0)));
                var minuteConcurrentTasks = new Dictionary<int, int>(Enumerable.Range(0, input.Price.Count).Select(n => new KeyValuePair<int, int>(n, 0)));

                var groupings = input.Tasks.SelectMany(t => Enumerable.Range(t.AllowedStartInterval, t.AllowedEndInterval + 1)).GroupBy(x => x).OrderBy(g => g.Count()).ToList();

                foreach (var task in input.Tasks)
                {
                    var powerToSatisfyLeft = task.PowerNeeded;

                    var minutesUsed = new HashSet<int>();

                    task.Completed = true;
                    while (powerToSatisfyLeft > 0)
                    {
                        var allowedMinutes = input.Price
                            .GetRange(task.AllowedStartInterval, task.AllowedEndInterval - task.AllowedStartInterval + 1)
                            .Select((n, index) => new { price = Convert.ToUInt64(n), index })
                            .Where(minute =>
                                minuteConsumption[task.AllowedStartInterval + minute.index] < input.MaxPowerPerMinute
                                && minuteConcurrentTasks[task.AllowedStartInterval + minute.index] < input.MaxConcurrentTasksPerMinute)
                            .ToList();

                        if (!allowedMinutes.Any())
                        {
                            task.Completed = false;
                            break;
                        }

                        var orderedAllowedMinutes = allowedMinutes
                            .OrderBy(m => groupings.FindIndex(g => g.Key == m.index + task.AllowedStartInterval));
                        //.ThenBy(m => m.price * Convert.ToUInt64(1 + (minuteConsumption[m.index + task.AllowedStartInterval]) / input.MaxPowerPerMinute));
                        //.OrderBy(m => m.price * Convert.ToUInt64(1 + (minuteConsumption[m.index + task.AllowedStartInterval]) / input.MaxPowerPerMinute));
                        //.OrderBy(m => Convert.ToUInt64(minuteConsumption[m.index + task.AllowedStartInterval]));
                        //.OrderBy(m => m.GetHashCode());

                        foreach (var minute in orderedAllowedMinutes)
                        {
                            var realMinuteIndex = task.AllowedStartInterval + minute.index;

                            minutesUsed.Add(realMinuteIndex); // Update minuteConcurrentTasks later

                            ++minuteConsumption[realMinuteIndex];

                            task.MinutePowerDrawn[realMinuteIndex] =
                                task.MinutePowerDrawn.TryGetValue(realMinuteIndex, out var value)
                                ? value + 1
                                : 1;

                            totalElectricityBill += Convert.ToUInt64(Math.Ceiling(minute.price * Convert.ToDouble(1 + (1 / input.MaxPowerPerMinute))));

                            if (--powerToSatisfyLeft == 0)
                            {
                                break;
                            }
                        }
                    }

                    foreach (var minute in minutesUsed)
                    {
                        ++minuteConcurrentTasks[minute];
                    }
                }

                if (totalElectricityBill < Convert.ToUInt64(input.MaxElectricityBill)
                    && input.Tasks.All(t => t.Completed))
                {
                    break;
                }

                input.Tasks.ForEach(t => t.MinutePowerDrawn.Clear());
                input.Tasks = input.Tasks.OrderBy(t => t.GetHashCode()).ToList();

                input.Tasks = input.Tasks.OrderBy(t => t.AllowedEndInterval - t.AllowedStartInterval).ToList();
                var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

                var one = rnd.Next(0, 30);
                var two = rnd.Next(0, 30);

                Swap(input.Tasks, one, two);
            }

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Tasks.Count}");
            foreach (var task in input.Tasks.OrderBy(t => t.Id))
            {
                var minutesConsumption = string.Join(' ', task.MinutePowerDrawn.Select(pair => $"{pair.Key} {pair.Value}"));
                sw.WriteLine($"{task.Id} {minutesConsumption}");
            }
        }

        public static void Swap<T>(List<T> array, int indexA, int indexB)
        {
            T temp = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = temp;
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
