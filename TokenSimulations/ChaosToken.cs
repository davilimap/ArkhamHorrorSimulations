using System;
using System.Collections.Generic;
using System.Text;

namespace TokenSimulations
{
    public class ChaosToken
    {
        public ChaosToken(TokenType type)
        {
            this.Type = type;
        }

        public TokenType Type { get; }
    }
}
