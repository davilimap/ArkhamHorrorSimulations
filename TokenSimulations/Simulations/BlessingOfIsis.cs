using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokenSimulations.Simulations
{
    public class BlessingOfIsis : SimulationBase
    {
        public static BlessingOfIsis Instance = new BlessingOfIsis("Blessing of Isis");

        private BlessingOfIsis(string name): base(name)
        {
        }

        public override long SimplePull(long iterations, int bless = 10, int curse = 0)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);

                var finalTokens = bag.PullOneAndResolve();
                if (finalTokens.Count(t => t.Type == TokenType.Bless) >= 2)
                {
                    activations++;
                }
            }

            return activations;
        }

        public override long OlivePull(long iterations, int bless = 10, int curse = 0)
        {
            long activations = 0;

            for (int i = 0; i < iterations; i++)
            {
                var bag = SetupBag(bless, curse);

                var finalTokens = bag.Pull(3);
                if(finalTokens.Count(t => t.Type == TokenType.Bless) >= 2)
                {
                    activations++;
                }
                else if (finalTokens.Count(t => t.Type == TokenType.Bless) == 1)
                {
                    var otherTokens = bag.PullOneAndResolve();
                    if (otherTokens.Any(t => t.Type == TokenType.Bless))
                    {
                        activations++;
                    }
                }
            }

            return activations;
        }
    }
}
