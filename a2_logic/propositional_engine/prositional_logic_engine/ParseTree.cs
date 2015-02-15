using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prositional_logic_engine
{
    public class ParseTree
    {

        private ParseNode _root;

        private ParseTree(ParseNode root)
        {
            this._root = root;
        }

        
        public static ParseTree Parse(List<ParseToken> Input, out ParseToken Error, out string ErrorMessage)
        {
            Stack<ParseNode> wSet = new Stack<ParseNode>();
            ParseNode c;


            foreach (ParseToken pt in Input)
            {
                try
                {
                    c = new ParseNode(pt);
                    if (c.IsOperator)
                    {
                        //NOT is the only operator that requires
                        //a single operand.
                        if (c.Operation == Operation.NOT)
                        {
                            c.Left = wSet.Pop();
                        }
                        else
                        {
                            //Right was placed on the stack last. 
                            c.Right = wSet.Pop();
                            c.Left = wSet.Pop();
                        }
                    }
                    //if its an operand we just pop it onto the stack.
                    wSet.Push(c);
                }
                catch (InvalidOperationException)
                {
                    //An operator was not able to pop its needed
                    //operands from the working set.
                    Error = pt;
                    ErrorMessage = string.Format("Too few operands for {0}.", pt.symbol);
                    return null;
                }
            }

            if(wSet.Count == 0)
            {
               throw new Exception();
            }

            if(wSet.Count > 1)
            {
                //If there are still things in the wSet more than
                //one thing left in the stack there was too many 
                //operands for the operators. 
                while(wSet.Peek().IsOperator && wSet.Count > 1)
                {
                    wSet.Pop();
                }
                Error = wSet.Peek().Token;
                ErrorMessage = string.Format("Unused operands starting w/ {0}.", wSet.Peek().Token.symbol);
                return null;
            }

            Error = null;
            ErrorMessage = null;
            return new ParseTree(wSet.Pop());
        }
        


        class ParseNode
        {
            public TruthValue? Value;
            public ParseToken Token;
            public ParseNode Left;
            public ParseNode Right;

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
                Left = null;
                Right = null;
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

            public TruthValue Evaluate()
            {
                if(this.IsOperator)
                {
                    switch(this.Token.op)
                    {
                        case Operation.OR:
                            return Op.OR(Left.Evaluate(), Right.Evaluate());
                        case Operation.AND:
                            return Op.AND(Left.Evaluate(), Right.Evaluate());
                        case Operation.NOT:
                            return Op.NOT(Left.Evaluate());
                        case Operation.IF:
                            return Op.IF(Left.Evaluate(), Right.Evaluate());
                        case Operation.IFF:
                            return Op.IFF(Left.Evaluate(), Right.Evaluate());
                    }
                }
                else
                {
                    return this.Value.Value;
                }
            }


        }



    }
}
