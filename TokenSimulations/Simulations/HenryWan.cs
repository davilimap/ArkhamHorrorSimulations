using System;
using System.Linq;

namespace TokenSimulations.Simulations
{
    public static class HenryWan
    {
        private const int MAXPULLS = 100;

        public static void PrintMany(int stoppingPoint = 5, long iterations = 100000)
        {
            for (int i = 0; i < stoppingPoint; i++)
            {
                SimulatePrint(i + 1, iterations, 10, 0);
            }
        }

        public static long Simulate(int pulls, long iterations, int bless = 0, int curse = 0)
        {
            long successes = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);
                var busted = false;
                for(int j = 0; j < pulls && !busted; j++)
                {
                    var token = bag.Pull().First();
                    busted = IsBust(token.Type);
                }

                if(!busted)
                {
                    successes++;
                }
            }

            return successes;
        }

        public static double EvUntilBust(long iterations, int bless = 0, int curse = 0)
        {
            double totalValues = 0;
            for (int i = 0; i < iterations; i++)
            {
                totalValues += PullUntilBust(bless, curse);
            }

            return totalValues / iterations;
        }

        public static void SimulatePrint(int pulls, long iterations, int bless = 0, int curse = 0)
        {
            var activations = Simulate(pulls, iterations, bless, curse);

            System.Console.WriteLine($"Henry Wan trying for {pulls} pulls");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        private static int PullUntilBust(int bless = 0, int curse = 0)
        {
            var bag = SetupBag(bless, curse);
            int i = 1;
            while (i <= MAXPULLS)
            {
                var token = bag.Pull().First();
                if(IsBust(token.Type))
                {
                    return i;
                }

                i++;
            }

            return i;
        }

        private static bool IsBust(TokenType type)
        {
            return type == TokenType.Skull ||
                    type == TokenType.Cultist ||
                    type == TokenType.Tablet ||
                    type == TokenType.ElderThing ||
                    type == TokenType.AutoFail;
        }

        private static ChaosBag SetupBag(int bless, int curse)
        {
            var bag = new ChaosBag(Campaign.NightOfTheZealot, Difficulty.Standard);
            if (!bag.AddBless(bless) || !bag.AddCurse(curse))
            {
                throw new InvalidOperationException("Added too many bless/curse tokens to the bag");
            }
            return bag;
        }

        private static void PrintResults(long iterations, long activations, double multiplier = 1)
        {
            double chanceOfActivating = 100 * multiplier * (double)activations / ((double)iterations);

            System.Console.WriteLine($"Iterations: {iterations}");
            System.Console.WriteLine($"Chance of success: {chanceOfActivating:F2}");
            System.Console.WriteLine();
        }
    }
}
