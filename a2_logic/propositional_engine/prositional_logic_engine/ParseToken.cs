using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prositional_logic_engine
{
    public class ParseToken
    {
        Dictionary<Operation, string> Keywords = new Dictionary<Operation, string>()
            {
                {Operation.NOT, "NOT" },
                {Operation.AND, "AND" },
                {Operation.OR, "OR" },
                {Operation.IF, "->" }, 
                {Operation.IFF, "<->"},
                {Operation.LEFT_PARATHESIS, "("},
                {Operation.RIGHT_PARATHESIS, ")"}
            };

        enum Type
        {
            OPERATION,
            SYMBOL
        }

        /// <summary>
        /// Operations are enumerated by precedence
        /// </summary>
        enum Operation : byte
        {
            NOT = 5,
            AND = 4,
            OR = 3,
            IF = 2,
            IFF = 1,
            LEFT_PARATHESIS = 0,
            RIGHT_PARATHESIS = 0
        }

        public ParseToken(string SymbolName)
        {
            this.type = Type.SYMBOL;
            this.symbol = SymbolName;
        }

        public ParseToken(Operation Op)
        {
            this.type = Type.OPERATION;
            this.symbol = Keywords[op];
        }

        public Type type;
        public Operation op;
        public string symbol;

    }
}
