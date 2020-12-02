using System;
using System.Collections.Generic;
using TokenSimulations.Simulations;

namespace TokenSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            HenryWanGraph();

            Console.ReadLine();
        }

        static void ParadoxicalCovenantGraph(int iterations = 100000)
        {
            var outputLines = new List<string>();

            outputLines.Add("bless, curse, simpleChance, jackiepull, olivepull");

            int bless = 1;
            int curse = 1;
            for (int i = 0; i < 10; i++)
            {
                var simplePull = ParadoxicalCovenant.SimplePull(iterations, bless, curse);
                float simplePullChance = 100 * (float) simplePull / (float) iterations;

                var jackiePull = ParadoxicalCovenant.JackiePullAfter(iterations, bless, curse);
                float jackiePullChance = 100 * (float)jackiePull / (float)iterations;

                var olivePull = ParadoxicalCovenant.OlivePull(iterations, bless, curse);
                float olivePullChance = 100 * (float)olivePull / (float)iterations;

                outputLines.Add($"{bless}, {curse}, {simplePullChance:F2}, {jackiePullChance:F2}, {olivePullChance:F2}");

                bless++;
                curse++;
            }

            var outputPath = @"C:\ArkhamHorrorSimulations\ParadoxicalCovenant.csv";

            System.Console.WriteLine($"Paradoxical Covenant. Output: {outputPath}");

            System.IO.File.WriteAllLines(outputPath, outputLines);
        }

        static void HenryWanGraph(int iterations = 100000)
        {
            var outputLines = new List<string>();

            outputLines.Add("bless/curse, 1pull, 2pull, 3pull, 4 pull, 5pull, ev");

            int bless = 0;
            int curse = 0;
            for (int i = 0; i <= 20; i++)
            {
                var onePull = HenryWan.Simulate(pulls: 1, iterations, bless, curse);
                float onePullChance = 100 * (float)onePull / (float)iterations;

                var twoPull = HenryWan.Simulate(pulls: 2, iterations, bless, curse);
                float twoPullChance = 100 * (float)twoPull / (float)iterations;

                var threePull = HenryWan.Simulate(pulls: 3, iterations, bless, curse);
                float threePullChance = 100 * (float)threePull / (float)iterations;

                var fourPull = HenryWan.Simulate(pulls: 4, iterations, bless, curse);
                float fourPullChance = 100 * (float)fourPull / (float)iterations;

                var fivePull = HenryWan.Simulate(pulls: 5, iterations, bless, curse);
                float fivePullChance = 100 * (float)fivePull / (float)iterations;

                var ev = HenryWan.EvUntilBust(iterations, bless, curse);

                outputLines.Add($"{i}, {onePullChance:F2}, {twoPullChance:F2}, {threePullChance:F2}, {fourPullChance:F2}, {fivePullChance:F2}, {ev:F2}");

                if (i % 2 == 0)
                {
                    bless++;
                }
                else
                {
                    curse++;
                }                
            }

            var outputPath = @"C:\ArkhamHorrorSimulations\HenryWan.csv";

            System.Console.WriteLine($"Henry Wan. Output: {outputPath}");

            System.IO.File.WriteAllLines(outputPath, outputLines);
        }
    }
}
