using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLogicEngine
{
    /// <summary>
    /// This parse engine of in-fix notation for propositional logic produces
    /// a resulting parse tree or appropriate exception. 
    /// 
    /// The Syntax for Propositional logic
    /// 
    /// </summary>
    public class ParseEngine
    {

        public static bool TryParse(string Input, out ParseTree PTree, out string RPN, out Exception Error, out int ErrorStart, out int ErrorTokenLength)
        {
            ErrorStart = ErrorTokenLength = 0;
            List<ParseToken> set;
            List<ParseToken> rSet;
            try
            {
                set = Tokenize(Input);
            }
            catch (FormatException e)
            {
                //If we fail to parse, notify of the fail point.
                RPN = null;
                Error = e;
                PTree = null;
                return false;
            }

            rSet = Reduce(set, out Error);
            if(rSet == null)
            {
                RPN = null;
                PTree = null;
                return false;
            }

            ParseToken ErrorToken;
            string ErrorMessage;
            PTree = ParseTree.Parse(rSet, out ErrorToken, out ErrorMessage);
            if(PTree == null)
            {
                Error = new Exception(ErrorMessage);
                RPN = null;
                PTree = null;
                return false;
            }

            RPN = StringifyTokens(rSet);
            Error = null;
            return true;

        }

        /// <summary>
        /// Break down a string and return the list of parse tokens. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<ParseToken> Tokenize(string input)
        {
            List<ParseToken> result = new List<ParseToken>();
            string[] toks = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in toks)
            {
                try
                {
                    result.Add(ParseToken.Parse(t));
                }
                catch (ArgumentException)
                {
                    throw new FormatException(string.Format("{0} is not a valid operator or symbol.", t));
                }
            }
            return result;
        }

        /// <summary>
        /// Apply the shunting yard algorith to the infix notation
        /// to reduce the notation to Reverse Polish Notation. This will
        /// allow us to check that the parathesis and such are matched. 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static List<ParseToken> Reduce(List<ParseToken> Input, out Exception Error)
        {
            List<ParseToken> result = new List<ParseToken>();
            Stack<ParseToken> working_stack = new Stack<ParseToken>();
            ParseToken tok;
            foreach(ParseToken t in Input)
            {
                if(t.type == TokenType.SYMBOL)
                {
                    result.Add(t);
                    continue;
                }
                else if(t.op == Operation.LEFT_PARATHESIS)
                {
                    working_stack.Push(t);
                }
                else if(t.op == Operation.RIGHT_PARATHESIS)
                {
                    bool found_left = false;
                    while(working_stack.Count > 0)
                    {
                        tok = working_stack.Pop();
                        if(tok.op != Operation.LEFT_PARATHESIS)
                        {
                            result.Add(tok);
                        }
                        else
                        {
                            found_left = true;
                            break;
                        }
                    }
                    if(!found_left)
                    {
                        Error = new Exception("Mismatched Parathesis!");
                        return null;
                    }
                }
                else 
                {
                    while(working_stack.Count > 0 && t.op <= working_stack.Peek().op)
                    {
                        result.Add(working_stack.Pop());
                    }
                    working_stack.Push(t);
                }
                
            }
            while(working_stack.Count > 0)
            {
                tok = working_stack.Pop();
                if(tok.op == Operation.LEFT_PARATHESIS ||
                    tok.op == Operation.RIGHT_PARATHESIS)
                {
                    Error = new Exception("Mismatched Parathesis");
                    return null;
                }
                else
                {
                    result.Add(tok);
                }
            }

            Error = null;
            return result;
        }

        private static string StringifyTokens(List<ParseToken> Input)
        {
            string result = "";
            foreach(ParseToken pt in Input)
            {
                result += " " + pt.symbol;
            }
            return result.TrimStart(new char[] {' '});
        }

        private static bool FindToken(string Input, List<ParseToken> List, ParseToken Target, out int Start, out int Length)
        {
            int cur_ind = 0;
            foreach(ParseToken pt in List)
            {
                cur_ind = Input.IndexOf(pt.symbol, cur_ind);
                if (pt == Target) break;
            }
            Start = cur_ind;
            Length = Target.symbol.Length;
            return true;
        }


        
    }
}
