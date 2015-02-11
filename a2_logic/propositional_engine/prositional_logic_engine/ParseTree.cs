using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prositional_logic_engine
{
    public class ParseTree
    {



        /*
        public static ParseTree Parse(List<ParseToken> Input)
        {
            


        }
         */


        class ParseNode
        {
            public TruthValue? Value;
            public ParseToken Token;

            public ParseNode(ParseToken Token)
            {
                if (Token.type == TokenType.SYMBOL)
                {
                    Value = TruthValue.Unknown;
                }
                else
                {
                    Value = null;
                }
                this.Token = Token;
            }

            public bool IsOperator
            {
                get
                {
                    return Token.op.HasValue;
                }
            }

            public Operation Operation
            {
                get
                {
                    return Token.op.Value;
                }
            }


        }



    }
}
