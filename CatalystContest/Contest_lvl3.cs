using CatalystContest.Model;
using FileParser;
using System;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl3
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            foreach (var task in input.Tasks)
            {
                var allowedMinutes = input.Price.GetRange(task.AllowedStartInterval, task.AllowedEndInterval - task.AllowedStartInterval + 1);

                task.MinuteDrawingPower = task.AllowedStartInterval + allowedMinutes.FindIndex(m => m == allowedMinutes.Min());
            }

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Tasks.Count}");
            foreach (var task in input.Tasks)
            {
                sw.WriteLine($"{task.Id} {task.MinuteDrawingPower} {task.PowerNeeded}");
            }
        }

        private Input ParseInput(string level)
        {
            var input = new Input();
            var file = new ParsedFile($"Inputs/{level}");
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
                throw new ParsingException("Error parsing file");
            }

            return input;
        }
    }
}
