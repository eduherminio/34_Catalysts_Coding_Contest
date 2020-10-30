using CatalystContest.Model;
using FileParser;
using System;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl1
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Price.FindIndex(i => i == input.Price.Min())}");
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
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }

            return input;
        }
    }
}
