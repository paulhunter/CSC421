using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLogicEngine
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

    public class Op
    {

        /// <summary>
        /// Propositional Logic Operator
        /// Inclusive Or
        /// A OR B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>See summary</returns>
        public static TruthValue OR(TruthValue A, TruthValue B)
        {
            if (A == TruthValue.True || B == TruthValue.True)
                return TruthValue.True;
            else if (A == TruthValue.Unknown || B == TruthValue.Unknown)
                return TruthValue.Unknown;
            else
                return TruthValue.False;
        }

        /// <summary>
        /// Propositional Logic Operator
        /// And
        /// A AND B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>See summary</returns>
        public static TruthValue AND(TruthValue A, TruthValue B)
        {
            if (A == TruthValue.False || B == TruthValue.False)
                return TruthValue.False;
            else if (A == TruthValue.Unknown || B == TruthValue.Unknown)
                return TruthValue.Unknown;
            else
                return TruthValue.True;
        }

        /// <summary>
        /// Propositional Logic Operator
        /// Not, Negate
        /// NOT A
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static TruthValue NOT(TruthValue A)
        {
            if (A == TruthValue.Unknown)
                return TruthValue.Unknown;
            else if (A == TruthValue.True)
                return TruthValue.False;
            else
                return TruthValue.True;
        }

        /// <summary>
        /// Propositional Logic Operator
        /// Implies, If/Then, IF
        /// A -> B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>See summary</returns>
        public static TruthValue IF(TruthValue A, TruthValue B)
        {
            return OR(NOT(A), B);
        }

        /// <summary>
        /// Propositional Logic Operator
        /// Doubly Implies, If and only If, IFF
        /// A <-> B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>See summary</returns>
        public static TruthValue IFF(TruthValue A, TruthValue B)
        {
            return AND(IF(A, B), IF(B, A));
        }
    }
}
