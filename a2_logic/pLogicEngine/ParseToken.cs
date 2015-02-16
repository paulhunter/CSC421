using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace pLogicEngine
{
    /// <summary>
    /// A ParseToken is a string within an input which represents a valid operator
    /// or variable. 
    /// </summary>
    public class ParseToken
    {
        //Members of the Parse Token.
        public TokenType type; //Token type. 
        public Operation? op; //The operation of the token, null if a Variable.
        public string symbol; //The string version of the variable or operator. 

        //Matching for variable names. 
        private static Regex r_symbol = new Regex("^[a-z0-9_]+$");

        //Keywords used for Operators. 
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

        /// <summary>
        /// Attempt to parse a string into a Token. 
        /// </summary>
        /// <param name="Input">Variable or opeator string</param>
        /// <returns>A ParseToken if the string was valid, otherwise
        /// throws an ArgumentException. </returns>
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

        /// <summary>
        /// Constructure of Variable ParseTokens. 
        /// </summary>
        /// <param name="SymbolName"></param>
        public ParseToken(string SymbolName)
        {
            this.type = TokenType.SYMBOL;
            this.op = null;
            this.symbol = SymbolName;
        }

        /// <summary>
        /// Constructor for Operator ParseTokens. 
        /// </summary>
        /// <param name="Op"></param>
        public ParseToken(Operation Op)
        {
            this.type = TokenType.OPERATION;
            this.op = Op;
            this.symbol = Keywords[Op];
        }

    }
}
