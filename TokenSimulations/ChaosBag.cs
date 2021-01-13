using System;
using System.Collections.Generic;
using System.Linq;

namespace TokenSimulations
{
    public class ChaosBag
    {
        private readonly List<ChaosToken> bag;
        private readonly List<ChaosToken> sealedTokens;

        private readonly Random random;

        private int blessCount = 0;
        private int curseCount = 0;

        public ChaosBag(Campaign campaign, Difficulty difficulty)
        {
            this.random = new Random();
            this.sealedTokens = new List<ChaosToken>();

            this.bag = campaign switch
            {
                Campaign.NightOfTheZealot => GetNotzBag(difficulty),
                _ => new List<ChaosToken>(),
            };
        }

        public IEnumerable<ChaosToken> PullOneAndResolve(bool resetBag = false)
        {
            var pulledTokens = new List<ChaosToken>();
            var pullAgain = true;
            var pulledBless = 0;
            var pulledCurse = 0;

            int index;
            ChaosToken token;
            while (pullAgain)
            {
                index = this.random.Next(this.bag.Count);

                token = this.bag[index];
                this.bag.RemoveAt(index);

                if (token.Type == TokenType.Bless)
                {
                    pulledBless++;
                }
                else if (token.Type == TokenType.Curse)
                {
                    pulledCurse++;
                }
                else
                {
                    pullAgain = false;
                }

                pulledTokens.Add(token);
            }

            if (resetBag)
            {
                this.bag.AddRange(pulledTokens);
            }
            else
            {
                this.blessCount -= pulledBless;
                this.curseCount -= pulledCurse;
            }

            return pulledTokens;
        }

        public IEnumerable<ChaosToken> Pull(int quant = 1)
        {
            if (quant <= 0)
            {
                yield break;
            }

            int index;
            for (int i = 0; i < quant; i++)
            {
                index = this.random.Next(this.bag.Count);

                var token = this.bag[index];
                this.bag.RemoveAt(index);

                if (token.Type == TokenType.Bless)
                {
                    this.blessCount--;
                }
                else if (token.Type == TokenType.Curse)
                {
                    this.curseCount--;
                }

                yield return token;
            }
        }

        public IEnumerable<ChaosToken> Resolve(IEnumerable<ChaosToken> tokens)
        {
            var pulledTokens = new List<ChaosToken>(tokens);

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Bless || token.Type == TokenType.Curse)
                {
                    pulledTokens.AddRange(this.PullOneAndResolve());
                }
            }

            return pulledTokens;
        }

        public bool ReturnToBag(IEnumerable<ChaosToken> tokens)
        {
            var addedBless = tokens.Count(t => t.Type == TokenType.Bless);
            var addedCurse = tokens.Count(t => t.Type == TokenType.Curse);
            if (addedBless + this.blessCount > Constants.BlessLimit || addedCurse + this.curseCount > Constants.CurseLimit)
            {
                return false;
            }
            else
            {
                this.blessCount += addedBless;
                this.curseCount += addedCurse;
            }

            this.bag.AddRange(tokens);
            return true;
        }

        #region Sealing

        public bool SealTokens(IEnumerable<ChaosToken> tokens)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseTokens(IEnumerable<ChaosToken> tokens)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Bless and Curse

        public bool AddBless(int quant = 1)
        {
            if (quant <= 0)
            {
                return true;
            }

            if (this.blessCount + quant > Constants.BlessLimit)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < quant; i++)
                {
                    this.bag.Add(new ChaosToken(TokenType.Bless));
                }

                this.blessCount += quant;
                return true;
            }
        }

        public bool AddCurse(int quant = 1)
        {
            if (quant <= 0)
            {
                return true;
            }

            if (this.curseCount + quant > Constants.CurseLimit)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < quant; i++)
                {
                    this.bag.Add(new ChaosToken(TokenType.Curse));
                }

                this.blessCount += quant;
                return true;
            }
        }

        #endregion

        #region CampaignBags

        private static List<ChaosToken> GetNotzBag(Difficulty difficulty)
        {
            return new List<ChaosToken>
            {
                new ChaosToken(TokenType.AutoFail),
                new ChaosToken(TokenType.ElderSign),
                new ChaosToken(TokenType.PlusOne),
                new ChaosToken(TokenType.Zero),
                new ChaosToken(TokenType.Zero),
                new ChaosToken(TokenType.MinusOne),
                new ChaosToken(TokenType.MinusOne),
                new ChaosToken(TokenType.MinusOne),
                new ChaosToken(TokenType.MinusTwo),
                new ChaosToken(TokenType.MinusTwo),
                new ChaosToken(TokenType.MinusThree),
                new ChaosToken(TokenType.MinusFour),
                new ChaosToken(TokenType.Skull),
                new ChaosToken(TokenType.Skull),
                new ChaosToken(TokenType.Cultist),
                new ChaosToken(TokenType.Tablet),
            };
        }

        private static List<ChaosToken> GetBaseBag()
        {
            return new List<ChaosToken>
            {
                new ChaosToken(TokenType.AutoFail),
                new ChaosToken(TokenType.ElderSign),
            };
        }

        #endregion
    }
}
