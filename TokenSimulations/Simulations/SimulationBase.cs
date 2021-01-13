using System;

namespace TokenSimulations.Simulations
{
    public abstract class SimulationBase
    {
        protected string name;

        protected SimulationBase(string name)
        {
            this.name = name;
        }

        public abstract long SimplePull(long iterations, int bless, int curse);

        public void SimplePullPrint(long iterations, int bless = 10, int curse = 10)
        {
            long activations = SimplePull(iterations, bless, curse);

            System.Console.WriteLine($"{name} simply pulling a token");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        public abstract long OlivePull(long iterations, int bless, int curse);

        public void OlivePullPrint(long iterations, int bless = 10, int curse = 10)
        {
            long activations = OlivePull(iterations, bless, curse);

            System.Console.WriteLine($"{name} using Olive McBride's ability");
            System.Console.WriteLine($"Bless tokens: {bless}, Curse tokens: {curse}");
            PrintResults(iterations, activations);
        }

        protected static ChaosBag SetupBag(int bless, int curse)
        {
            var bag = new ChaosBag(Campaign.NightOfTheZealot, Difficulty.Standard);
            if (!bag.AddBless(bless) || !bag.AddCurse(curse))
            {
                throw new InvalidOperationException("Added too many bless/curse tokens to the bag");
            }
            return bag;
        }

        protected static void PrintResults(long iterations, long activations, double multiplier = 1)
        {
            double chanceOfActivating = 100 * multiplier * (double)activations / ((double)iterations);

            System.Console.WriteLine($"Iterations: {iterations}");
            System.Console.WriteLine($"Chance of activating: {chanceOfActivating:F2}");
            System.Console.WriteLine();
        }
    }
}
