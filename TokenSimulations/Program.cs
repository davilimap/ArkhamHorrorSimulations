using System;
using System.Collections.Generic;
using TokenSimulations.Simulations;

namespace TokenSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            HenryWan.PrintMany();

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

                var jackiePull = ParadoxicalCovenant.JackiePull(iterations, bless, curse);
                float jackiePullChance = 100 * (float)jackiePull / (float)iterations;

                var olivePull = ParadoxicalCovenant.OlivePull(iterations, bless, curse);
                float olivePullChance = 100 * (float)olivePull / (float)iterations;

                outputLines.Add($"{bless}, {curse}, {simplePullChance:F2}, {jackiePullChance:F2}, {olivePullChance:F2}");

                bless++;
                curse++;
            }

            var outputPath = @"C:\ArkhamHorrorSimulations\ParadoxicalCovenant.csv";

            System.Console.WriteLine($"Paradoxical Dovenant. Output: {outputPath}");

            System.IO.File.WriteAllLines(outputPath, outputLines);
        }
    }
}
