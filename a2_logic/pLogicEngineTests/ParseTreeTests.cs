using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pLogicEngine;

namespace pLogicEngineTests
{
    [TestClass]
    public class ParseTreeTests
    {
        /* ParseTreeTests
         * 
         * A set of tests using the provided expressions and assignments,
         * cross-checked against wolfram alpha. 
         * 
         */

        const string A = "( ( p1 -> ( p2 AND p3 ) ) AND ( ( NOT p1 ) -> " + 
            "( p3 AND p4 ) ) )"; // A1 = False, A2 = False
        //http://www.wolframalpha.com/input/?i=%28+%28+A+implies+%28+B+AND+C+%29+%29+AND+%28+%28+NOT+A+%29+implies+%28+C+AND+D+%29+%29+%29

        const string B = "( ( p3 -> ( NOT p6 ) ) AND ( ( NOT p3 ) -> " +
            "( p4 -> p1 ) ) )"; // A1 = True, A2 = False
        //http://www.wolframalpha.com/input/?i=%28+%28+C+implies+%28+!+F+%29+%29+AND+%28+%28+!+C+%29+implies+%28+D+implies+A+%29+%29+%29

        const string C = "( ( NOT ( p2 AND p5 ) ) AND ( p2 -> p5 ) )"; // A1 = True, A2 = False
        //http://www.wolframalpha.com/input/?i=%28+%28+!+%28+B+AND+G+%29+%29+AND+%28+B+implies+G+%29+%29

        const string D = "( NOT ( p3 -> p6 ) )";  //A1 = True, A2 = False
        //http://www.wolframalpha.com/input/?i=%28+!+%28+C+implies+F+%29+%29

        string X = string.Format("( ( {0} AND ( {1} AND {2} ) ) " 
            + "-> {3} )", A, B, C, D); // A1 = True, A2 = True (Left side clause is always false)

        Dictionary<string, TruthValue> A1 = new Dictionary<string, TruthValue>()
        {
            { "p1", TruthValue.True }, 
            { "p2", TruthValue.False }, 
            { "p3", TruthValue.True }, 
            { "p4", TruthValue.False }, 
            { "p5", TruthValue.True }, 
            { "p6", TruthValue.False }
        };

        Dictionary<string, TruthValue> A2 = new Dictionary<string, TruthValue>()
        {
            { "p1", TruthValue.False }, 
            { "p2", TruthValue.True }, 
            { "p3", TruthValue.False }, 
            { "p4", TruthValue.True }, 
            { "p5", TruthValue.False }, 
            { "p6", TruthValue.True }
        };

        [TestMethod]
        public void ParseTree_ParseAndEval_A_A1()
        {
            Assert.AreEqual(TruthValue.False, GeneratePT(A).Evaluate(A1));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_A_A2()
        {
            Assert.AreEqual(TruthValue.False, GeneratePT(A).Evaluate(A2));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_B_A1()
        {
            Assert.AreEqual(TruthValue.True, GeneratePT(B).Evaluate(A1));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_B_A2()
        {
            Assert.AreEqual(TruthValue.False, GeneratePT(B).Evaluate(A2));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_C_A1()
        {
            Assert.AreEqual(TruthValue.True, GeneratePT(C).Evaluate(A1));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_C_A2()
        {
            Assert.AreEqual(TruthValue.False, GeneratePT(C).Evaluate(A2));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_D_A1()
        {
            Assert.AreEqual(TruthValue.True, GeneratePT(D).Evaluate(A1));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_D_A2()
        {
            Assert.AreEqual(TruthValue.False, GeneratePT(D).Evaluate(A2));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_X_A1()
        {
            Assert.AreEqual(TruthValue.True, GeneratePT(X).Evaluate(A1));
        }

        [TestMethod]
        public void ParseTree_ParseAndEval_X_A2()
        {
            Assert.AreEqual(TruthValue.True, GeneratePT(X).Evaluate(A2));
        }

        private ParseTree GeneratePT(string Input)
        {
            ParseTree r;
            string rpn;
            Exception e;
            int x, y;
            Assert.AreEqual(true, ParseEngine.TryParse(Input, out r, out rpn, out e, out x, out y), "Invalid infix notation");
            return r;
        }
    }
}
