using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace pLogicEngine
{
    public class ParseToken
    {
        private static Regex r_symbol = new Regex("^[a-z_]+$");

        public static Dictionary<Operation, string> Keywords = new Dictionary<Operation, string>()
            {
                {Operation.NOT, "NOT" },
                {Operation.AND, "AND" },
                {Operation.OR, "OR" },
                {Operation.IF, "->" }, 
                {Operation.IFF, "<->"},
                {Operation.LEFT_PARATHESIS, "("},
                {Operation.RIGHT_PARATHESIS, ")"}
            };

        public static ParseToken Parse(string Input)
        {
            //Check if string is operator.
            foreach(Operation key in Keywords.Keys)
            {
                if(Keywords[key].Equals(Input))
                {
                    return new ParseToken(key);
                }
            }

            //if not an opeator, it must be a symbol. 
            if(r_symbol.IsMatch(Input))
            {
                return new ParseToken(Input);
            }

            //If we reach this point and have no returned something, 
            //then we are looking at a string which is not a valid token. 
            throw new ArgumentException();
        }

        public ParseToken(string SymbolName)
        {
            this.type = TokenType.SYMBOL;
            this.op = null;
            this.symbol = SymbolName;
        }

        public ParseToken(Operation Op)
        {
            this.type = TokenType.OPERATION;
            this.op = Op;
            this.symbol = Keywords[Op];
        }

        public TokenType type;
        public Operation? op;
        public string symbol;

    }
}
