using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pLogicEngine
{
    /// <summary>
    /// A implementation of a Binary Expression Tree. This class is interactive, 
    /// allowing for the assignment of variables within its structure and evaluation. 
    /// </summary>
    public class ParseTree
    {

        private ParseNode _root;
        private Dictionary<string, List<ParseNode>> _symbolTable;

        private ParseTree(ParseNode root, Dictionary<string, List<ParseNode>> symbolTable)
        {
            this._root = root;
            this._symbolTable = symbolTable;
            
        }
    
        /// <summary>
        /// Attempt to parse an RPN sequence of Parse tokens. If successful return the reslting
        /// tree, else return null and populate the Error and ErrorMessage parameters. 
        /// </summary>
        /// <param name="Input">RPN sequence of Tokens. </param>
        /// <param name="Error">Container for Token of Error</param>
        /// <param name="ErrorMessage">Error message regarding violating token. </param>
        /// <returns>A binary expression tree if succesful, otherwise null. </returns>
        public static ParseTree Parse(List<ParseToken> Input, out ParseToken Error, out string ErrorMessage)
        {
            Dictionary<string, List<ParseNode>> symbolTable = new Dictionary<string, List<ParseNode>>();
            Stack<ParseNode> wSet = new Stack<ParseNode>(); //Stack for processing. 
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
                            //Right was placed on the stack last
                            //This order does not matter except for IF operators
                            c.Right = wSet.Pop();
                            c.Left = wSet.Pop();
                        }
                        //and push it on the stack (fall through)
                    }
                    else
                    {
                        //if its an operand we want to add it to our symbol table.
                        try
                        {
                            symbolTable.Add(c.Token.symbol, new List<ParseNode>() { c } );
                        }
                        catch (ArgumentException) 
                        {
                            symbolTable[c.Token.symbol].Add(c);
                        } //Thrown when the key is already present, so at it to the current list.
                    }
                    
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
               //We lost out root?
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
                ErrorMessage = string.Format(
                    "Unused operands starting w/ {0}.", wSet.Peek().Token.symbol);
                return null;
            }

            Error = null;
            ErrorMessage = null;
            return new ParseTree(wSet.Pop(), symbolTable);
        }
       
        /// <summary>
        /// Evaluate the tree using the currently assigned TruthValues of the nodes. 
        /// By default all nodes have a value of Unknown, use AssignValue to 
        /// change variable assignments using variable names in GetSymbols(). 
        /// </summary>
        /// <returns>A truth value of the tree</returns>
        public TruthValue Evaluate()
        {
            return _root.Evaluate();
        }

        /// <summary>
        /// Evaluate the tree using a provided assignment of values. 
        /// </summary>
        /// <param name="Assignment">A dictionary of values which must 
        /// contain at least the symbols present in the tree, additional
        /// sybols within the dicitonary will not be used. </param>
        /// <returns>Truth value of expression using provided assignment.</returns>
        public TruthValue Evaluate(Dictionary<string, TruthValue> Assignment)
        {
            TruthValue result = TruthValue.Unknown;
            Dictionary<string, TruthValue> Old = new Dictionary<string, TruthValue>();
            string s = "";
            try
            {
                foreach (string sym in _symbolTable.Keys)
                {
                    s = sym;
                    Old.Add(sym, _symbolTable[sym][0].Value.Value);
                    //We can make this assumption because no empty list is every
                    //added to the dictionary. 
                    AssignValue(sym, Assignment[sym]);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Assignment did not contain a definition for {0}.", s);
            }

            result = Evaluate();

            foreach (string sym in _symbolTable.Keys)
            {
                AssignValue(sym, Old[sym]);
            }

            return result;
        }

        /// <summary>
        /// Assign a value to a variable within the expression. 
        /// </summary>
        /// <param name="Symbol">Variable name. </param>
        /// <param name="Value">Desired truth value.</param>
        public void AssignValue(string Symbol, TruthValue Value)
        {
            Debug.Assert(_symbolTable.ContainsKey(Symbol), "Cannot assign value to non-symbol!");
            foreach(ParseNode n in _symbolTable[Symbol])
            {
                n.Value = Value;
            }
        }

        /// <summary>
        /// Get a list of the variable names within the expression. 
        /// </summary>
        /// <returns></returns>
        public string[] GetSymbols()
        {
            List<string> result = new List<string>();
            foreach(string key in _symbolTable.Keys)
            {
                result.Add(key);
            }
            result.Sort();
            return result.ToArray();
        }

        /// <summary>
        /// An internal class used by the parse tree to store the expression. 
        /// It is a standard binary tree node that contains a ParseToken structure. 
        /// </summary>
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

            /// <summary>
            /// Return true if the Node is an operator. 
            /// </summary>
            public bool IsOperator
            {
                get
                {
                    return Token.op.HasValue;
                }
            }

            /// <summary>
            /// Returns the Operation of the node. Will 
            /// throw a NullReferenceException if node is not an 
            /// opertor, use IsOperator to check. 
            /// </summary>
            public Operation Operation
            {
                get
                {
                    return Token.op.Value;
                }
            }

            /// <summary>
            /// Recursive evaluation method. 
            /// </summary>
            /// <returns>Thruth value of this node or its expression if
            /// it is an operator.</returns>
            public TruthValue Evaluate()
            {
                //If an operator, recursively call the evaluate while applying the
                //appropriate operator. 
                if (this.IsOperator)
                {
                    switch (this.Token.op)
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
                        default:
                            Debug.Assert(false, "Invalid Operator in Parse Tree!");
                            return TruthValue.Unknown;
                    }
                }
                else
                {
                    //Base case, we are a leaf, return our value.
                    return this.Value.Value;
                }
                
            }


        }



    }
}
