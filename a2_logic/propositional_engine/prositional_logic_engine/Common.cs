using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prositional_logic_engine
{
    public enum TruthValue
    {
        True,
        False,
        Unknown
    }

    public enum TokenType
    {
        OPERATION,
        SYMBOL
    }

    /// <summary>
    /// Operations are enumerated by precedence
    /// </summary>
    public enum Operation : byte
    {
        NOT = 6,
        AND = 5,
        OR = 4,
        IF = 3,
        IFF = 2,
        LEFT_PARATHESIS = 1,
        RIGHT_PARATHESIS = 0,
    }
}
