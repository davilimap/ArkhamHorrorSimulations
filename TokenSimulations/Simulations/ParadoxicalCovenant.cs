using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TokenSimulations.Simulations
{
    public static class ParadoxicalCovenant
    {
        public static void PrintAll(long iterations = 100000)
        {
            SimplePullPrint(iterations);

            JackiePullPrint(iterations);

            JackiePullAfterPrint(iterations);

            OlivePullPrint(iterations);
        }

        static void OutputToCsv(int iterations = 100000)
        {
            var outputLines = new List<string>();

            outputLines.Add("bless, curse, simpleChance, jackiepull, olivepull");

            int bless = 1;
            int curse = 1;
            for (int i = 0; i < 10; i++)
            {
                var simplePull = SimplePull(iterations, bless, curse);
                float simplePullChance = 100 * (float)simplePull / (float)iterations;

                var jackiePull = JackiePull(iterations, bless, curse);
                float jackiePullChance = 100 * (float)jackiePull / (float)iterations;

                var olivePull = OlivePull(iterations, bless, curse);
                float olivePullChance = 100 * (float)olivePull / (float)iterations;

                outputLines.Add($"{bless}, {curse}, {simplePullChance:F2}, {jackiePullChance:F2}, {olivePullChance:F2}");

                bless++;
                curse++;
            }

            var outputPath = @"C:\ArkhamHorrorSimulations\ParadoxicalCovenant.csv";

            System.Console.WriteLine($"Paradoxical Dovenant. Output: {outputPath}");

            System.IO.File.WriteAllLines(outputPath, outputLines);
        }

        public static long SimplePull(long iterations, int bless = 10, int curse = 10)
        {
            long activations = 0;
            var bag = SetupBag(bless, curse);

            for (int i = 0; i < iterations; i++)
            {
                var tokens = bag.PullOneAndResolve(resetBag: true);

                if (tokens.Any(t => t.Type == TokenType.Bless) && tokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }
            }

            return activations;
        }

        public static void SimplePullPrint(long iterations, int bless = 10, int curse = 10)
        {
            var activations = SimplePull(iterations, bless, curse);

            System.Console.WriteLine("Paradoxical Covenant simply pulling a token");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        public static long JackiePull(long iterations, int bless = 10, int curse = 10)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);
                var tokenToResolve = ChooseJackieTokenToResolve(bag.Pull(3));

                var finalTokens = bag.Resolve(tokenToResolve);

                if (finalTokens.Any(t => t.Type == TokenType.Bless) && finalTokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }

            }

            return activations;
        }

        public static void JackiePullPrint(long iterations, int bless = 10, int curse = 10)
        {
            long activations = JackiePull(iterations, bless, curse);

            System.Console.WriteLine("Paradoxical Covenant using Jacqueline Fine's ability");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        public static long JackiePullAfter(long iterations, int bless = 10, int curse = 10)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);

                var firstToken = bag.Pull().First();

                if (firstToken.Type == TokenType.Bless || firstToken.Type == TokenType.Curse)
                {
                    var tokenToResolve = ChooseJackieTokenToResolve(bag.Pull(3), firstToken.Type);

                    var finalTokens = bag.Resolve(tokenToResolve).Append(firstToken);

                    if (finalTokens.Any(t => t.Type == TokenType.Bless) && finalTokens.Any(t => t.Type == TokenType.Curse))
                    {
                        activations++;
                    }
                }
            }

            return activations;
        }

        public static void JackiePullAfterPrint(long iterations, int bless = 10, int curse = 10)
        {
            long activations = JackiePullAfter(iterations, bless, curse);

            System.Console.WriteLine("Paradoxical Covenant using Jacqueline Fine's ability after drawing 1 bless/curse token");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        public static long OlivePull(long iterations, int bless = 10, int curse = 10)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);
                var tokensToResolve = ChooseOliveTokensToResolve(bag.Pull(3));

                var finalTokens = bag.Resolve(tokensToResolve);

                if (finalTokens.Any(t => t.Type == TokenType.Bless) && finalTokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }
            }

            return activations;
        }

        public static void OlivePullPrint(long iterations, int bless = 10, int curse = 10)
        {
            long activations = OlivePull(iterations, bless, curse);

            System.Console.WriteLine("Paradoxical Covenant using Olive McBride's ability");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        private static ChaosBag SetupBag(int bless, int curse)
        {
            var bag = new ChaosBag(Campaign.NightOfTheZealot, Difficulty.Standard);
            if(!bag.AddBless(bless) || !bag.AddCurse(curse))
            {
                throw new InvalidOperationException("Added too many bless/curse tokens to the bag");
            }
            return bag;
        }

        private static IEnumerable<ChaosToken> ChooseJackieTokenToResolve(IEnumerable<ChaosToken> chaosTokens, TokenType otherToLookFor = TokenType.Bless)
        {
            if (otherToLookFor != TokenType.Bless && otherToLookFor != TokenType.Curse)
            {
                throw new InvalidOperationException("Must be looking for bless or curse");
            }

            var tokenToLookFor = otherToLookFor == TokenType.Bless ? TokenType.Curse : TokenType.Bless;

            if(chaosTokens.Any(t => t.Type == TokenType.AutoFail))
            {
                return chaosTokens.Where(t => t.Type != TokenType.AutoFail);
            }

            var blessOrCurse = chaosTokens.FirstOrDefault(t => t.Type == tokenToLookFor);

            if(blessOrCurse != null)
            {
                return new ChaosToken[] { blessOrCurse };
            }
            else
            {
                blessOrCurse = chaosTokens.FirstOrDefault(t => t.Type == otherToLookFor);

                if (blessOrCurse != null)
                {
                    return new ChaosToken[] { blessOrCurse };
                }
                else
                {
                    return new ChaosToken[] { chaosTokens.First() };
                }
            }
        }

        private static IEnumerable<ChaosToken> ChooseOliveTokensToResolve(IEnumerable<ChaosToken> chaosTokens)
        {
            var bless = chaosTokens.FirstOrDefault(t => t.Type == TokenType.Bless);
            var curse = chaosTokens.FirstOrDefault(t => t.Type == TokenType.Curse);

            if (bless != null && curse != null)
            {
                return new ChaosToken[]
                {
                        bless,
                        curse,
                };
            }
            else
            {
                return chaosTokens.OrderBy(t => t.Type).Take(2);
            }
        }

        private static void PrintResults(long iterations, long activations, double multiplier = 1)
        {
            double chanceOfActivating = 100 * multiplier * (double)activations / ((double)iterations);

            System.Console.WriteLine($"Iterations: {iterations}");
            System.Console.WriteLine($"Chance of activating: {chanceOfActivating:F2}");
            System.Console.WriteLine();
        }
    }
}
