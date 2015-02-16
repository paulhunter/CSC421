using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pLogicEngine;

namespace pLogicEngineTests
{
    [TestClass]
    public class ParseTreeTests
    {

        const string A = "( ( p1 -> ( p2 AND p3 ) ) AND ( ( NOT p1 ) -> ( p3 aND p4 ) ) )";
        const string B = "( ( p3 -> ( NOT p6 ) ) AND ( ( NOT p3 ) -> ( p4 -> p1 ) ) )";
        const string C = "( ( NOT ( p2 AND p5 ) ) AND ( p2 -> p5 ) )";
        const string D = "( NOT ( p3 -> p6 ) )";
        const string X = string.Format("( ( {0} AND ( {1} AND {2} ) ) -> {3} )", A, B, C, D);

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
            
        }
    }
}
