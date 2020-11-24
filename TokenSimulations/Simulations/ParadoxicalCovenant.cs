using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TokenSimulations.Simulations
{
    public static class ParadoxicalCovenant
    {
        public static void SimplePull(long iterations)
        {
            int bless = 10;
            int curse = 10;
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

            System.Console.WriteLine("Paradoxical Covenant simply pulling a token");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        public static void JackiePull(long iterations)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless: 10, curse: 10);
                var tokenToResolve = ChooseJackieTokenToResolve(bag.Pull(3));

                var finalTokens = bag.Resolve(tokenToResolve);

                if (finalTokens.Any(t => t.Type == TokenType.Bless) && finalTokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }

            }

            System.Console.WriteLine("Paradoxical Covenant using Jacqueline Fine's ability");
            PrintResults(iterations, activations);
        }

        public static void JackiePullAfter(long iterations)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless: 9, curse: 10);
                var tokenToResolve = ChooseJackieTokenToResolve(bag.Pull(3));

                var finalTokens = bag.Resolve(tokenToResolve);

                if (finalTokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }

            }

            double multiplier = 20.0 / 36.0;

            System.Console.WriteLine("Paradoxical Covenant using Jacqueline Fine's ability after drawing 1 bless/curse token");
            PrintResults(iterations, activations, multiplier);
        }

        public static void OlivePull(long iterations)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless: 5, curse: 5);
                var tokensToResolve = ChooseOliveTokensToResolve(bag.Pull(3));

                var finalTokens = bag.Resolve(tokensToResolve);

                if (finalTokens.Any(t => t.Type == TokenType.Bless) && finalTokens.Any(t => t.Type == TokenType.Curse))
                {
                    activations++;
                }
            }

            System.Console.WriteLine("Paradoxical Covenant using Olive McBride's ability");
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

        private static IEnumerable<ChaosToken> ChooseJackieTokenToResolve(IEnumerable<ChaosToken> chaosTokens)
        {
            if(chaosTokens.Any(t => t.Type == TokenType.AutoFail))
            {
                return chaosTokens.Where(t => t.Type != TokenType.AutoFail);
            }

            var blessOrCurse = chaosTokens.FirstOrDefault(t => t.Type == TokenType.Curse);

            if(blessOrCurse != null)
            {
                return new ChaosToken[] { blessOrCurse };
            }
            else
            {
                blessOrCurse = chaosTokens.FirstOrDefault(t => t.Type == TokenType.Bless);

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
