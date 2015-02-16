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
    /// Syntax - all atoms are space delimited (1+ spaces)
    /// expression ::= para_expression | op_expression | variable
    /// para_expression ::= ‘(‘ expression ‘)’
    /// op_expression ::= expression operator expression
    /// operator ::= ‘OR’ | ‘AND’ | ‘NOT’ | ‘->’ | ‘<->’ 
    /// variable ::= [‘a’-‘z’’0’-‘9’]+ 
    /// </summary>
    public class ParseEngine
    {
        /// <summary>
        /// Given an input of space delimited words, attempt to 
        /// parse the infix notation into a parse tree. If the attempt is 
        /// successful the PTree parameter will be populated, otherwise it
        /// will be null. 
        /// If a failure does occur, use the Message of Error and ErrorStart
        /// and ErrorToken length to find the problem in the input. 
        /// </summary>
        /// <param name="Input">Infix Notation</param>
        /// <param name="PTree">Resulting Parse Tree, if successful</param>
        /// <param name="RPN">The RPN of the expression, if successful</param>
        /// <param name="Error">A Parse Error</param>
        /// <param name="ErrorStart">The index of the start of the error.</param>
        /// <param name="ErrorTokenLength">The length of the token causing failure.</param>
        /// <returns>True if the parse was successful, in which case PTree and 
        /// RPN will not be null, otherwise, if the prase failed, Error, ErrorStart
        /// and ErrorTokenLength will be populated.</returns>
        public static bool TryParse(string Input, out ParseTree PTree, 
            out string RPN, out Exception Error, out int ErrorStart, out int ErrorTokenLength)
        {
            ErrorStart = ErrorTokenLength = 0;
            List<ParseToken> set; //Our infix notation token sequence
            List<ParseToken> rSet; //out RPN notation token sequence
            try
            {
                set = Tokenize(Input);
            }
            catch (FormatException e)
            {
                //One of the tokens did not match an operator
                //or fall into the Variables language. 
                RPN = null;
                Error = e;
                PTree = null;
                return false;
            }

            rSet = Reduce(set, out Error);
            if(rSet == null)
            {
                //Mismatched Paratheses.
                RPN = null;
                PTree = null;
                return false;
            }

            ParseToken ErrorToken;
            string ErrorMessage;
            PTree = ParseTree.Parse(rSet, out ErrorToken, out ErrorMessage);
            if(PTree == null)
            {
                //Operator/Operand mismatch error. 
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
        /// Break down a string of space delmited input into parse tokens. If a token
        /// does not follow syntax, a FormatException will be thrown. 
        /// </summary>
        /// <param name="input">Space delimited input</param>
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
        /// allow us to check that the parathesis are matched. 
        /// </summary>
        /// <param name="Input">Infix notation sequence of tokens. </param>
        /// <returns>RPN notation sequence of tokens with paratheses removed.</returns>
        private static List<ParseToken> Reduce(List<ParseToken> Input, out Exception Error)
        {
            //http://en.wikipedia.org/wiki/Shunting-yard_algorithm
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

        /// <summary>
        /// Given a list of tokens, produce a space delimited string. 
        /// </summary>
        /// <param name="Input">Tokens</param>
        /// <returns>Spaced delimited string of Tokens.</returns>
        private static string StringifyTokens(List<ParseToken> Input)
        {
            string result = "";
            foreach(ParseToken pt in Input)
            {
                result += " " + pt.symbol;
            }
            return result.TrimStart(new char[] {' '});
        }

        /// <summary>
        /// Given the string of original input, a list of Parse Tokens, and the 
        /// target offender, find the index the token begins and the length of the 
        /// token. 
        /// </summary>
        /// <param name="Input">Original infix input. </param>
        /// <param name="List">Sequence of infix tokens. </param>
        /// <param name="Target">Token to find the start index and length of</param>
        /// <param name="Start">0 based index in Input were Target begins.</param>
        /// <param name="Length">Length of target in characters. </param>
        /// <returns></returns>
        private static bool FindToken(string Input, List<ParseToken> List, ParseToken Target, 
            out int Start, out int Length)
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
