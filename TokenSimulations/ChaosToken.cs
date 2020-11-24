using System;
using System.Collections.Generic;
using System.Text;

namespace TokenSimulations
{
    class ChaosToken
    {
        public ChaosToken(TokenType type)
        {
            this.Type = type;
        }

        public TokenType Type { get; }
    }
}
