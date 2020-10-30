using CatalystContest.Model;
using FileParser;
using System;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl2
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            foreach (var task in input.Tasks)
            {
                var minCost = int.MaxValue;
                var minStartMinute = int.MaxValue;
                var currentCost = 0;
                var counter = 0;

                for (int t = 0; t < input.Price.Count; ++t)
                {
                    currentCost += input.Price[t];
                    counter++;

                    if (counter == task.CompletionTime)
                    {
                        if (currentCost < minCost)
                        {
                            minCost = currentCost;
                            minStartMinute = t - task.CompletionTime + 1;
                        }

                        counter = 0;
                        currentCost = 0;

                        t -= task.CompletionTime - 1;
                    }
                }

                task.MinuteDrawingPower = minStartMinute;
            }

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Tasks.Count}");
            foreach (var task in input.Tasks)
            {
                sw.WriteLine($"{task.Id} {task.MinuteDrawingPower}");
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

                input.Tasks.Add(new Task
                {
                    Id = line.NextElement<int>(),
                    CompletionTime = line.NextElement<int>()
                });
            }
            if (!file.Empty)
            {
                throw new ParsingException("Error parsing file");
            }

            return input;
        }
    }
}
