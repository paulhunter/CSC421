using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using pLogicEngine;

namespace pLogicEngineTests
{
    [TestClass]
    public class OpTest
    {
        [TestMethod]
        public void Test_OR_AllCases()
        {
            //Test all nine combos of inputs. 
            Assert.AreEqual(TruthValue.Unknown, Op.OR(TruthValue.Unknown, TruthValue.Unknown), "OR(U,U) != U");
            Assert.AreEqual(TruthValue.True, Op.OR(TruthValue.Unknown, TruthValue.True), "OR(U,T) != T");
            Assert.AreEqual(TruthValue.Unknown, Op.OR(TruthValue.Unknown, TruthValue.False), "OR(U,F) != U");
            Assert.AreEqual(TruthValue.True, Op.OR(TruthValue.True, TruthValue.Unknown), "OR(T,U) != T");
            Assert.AreEqual(TruthValue.True, Op.OR(TruthValue.True, TruthValue.True), "OR(T,T) != T");
            Assert.AreEqual(TruthValue.True, Op.OR(TruthValue.True, TruthValue.False), "OR(T,F) != T");
            Assert.AreEqual(TruthValue.Unknown, Op.OR(TruthValue.False, TruthValue.Unknown), "OR(F,U) != U");
            Assert.AreEqual(TruthValue.True, Op.OR(TruthValue.False, TruthValue.True), "OR(F,T) != T");
            Assert.AreEqual(TruthValue.False, Op.OR(TruthValue.False, TruthValue.False), "OR(F,F) != F");
        }

        [TestMethod]
        public void AND_Tests()
        {
            //Test all nine combos of inputs.
            Assert.AreEqual(TruthValue.Unknown, Op.AND(TruthValue.Unknown, TruthValue.Unknown), "AND(U,U) != U");
            Assert.AreEqual(TruthValue.Unknown, Op.AND(TruthValue.Unknown, TruthValue.True), "AND(U,T) != U");
            Assert.AreEqual(TruthValue.False, Op.AND(TruthValue.Unknown, TruthValue.False), "AND(U,F) != F");
            Assert.AreEqual(TruthValue.Unknown, Op.AND(TruthValue.True, TruthValue.Unknown), "AND(T,U) != U");
            Assert.AreEqual(TruthValue.True, Op.AND(TruthValue.True, TruthValue.True), "AND(T,T) != T");
            Assert.AreEqual(TruthValue.False, Op.AND(TruthValue.True, TruthValue.False), "AND(T,F) != F");
            Assert.AreEqual(TruthValue.False, Op.AND(TruthValue.False, TruthValue.Unknown), "AND(F,U) != F");
            Assert.AreEqual(TruthValue.False, Op.AND(TruthValue.False, TruthValue.True), "AND(F,T) != F");
            Assert.AreEqual(TruthValue.False, Op.AND(TruthValue.False, TruthValue.False), "AND(F,F) != F");
        }

        [TestMethod]
        public void NOT_Tests()
        {
            //Test all three inputs. 
            Assert.AreEqual(TruthValue.Unknown, Op.NOT(TruthValue.Unknown), "NOT(U) != U");
            Assert.AreEqual(TruthValue.False, Op.NOT(TruthValue.True), "NOT(T) != F");
            Assert.AreEqual(TruthValue.True, Op.NOT(TruthValue.False), "NOT(F) != T");
        }
    }
}
