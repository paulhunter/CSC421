using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using pLogicEngine;

namespace pLogicEngineTests
{
    //Test class for the Three-value logic operators 
    // http://en.wikipedia.org/wiki/Three-valued_logic
    [TestClass]
    public class OpTest
    {
        [TestMethod]
        public void OR_Tests()
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

        [TestMethod]
        public void IF_Tests()
        {
            //Test all nine inputs.
            Assert.AreEqual(TruthValue.Unknown, Op.IF(TruthValue.Unknown, TruthValue.Unknown), "IF(U,U) != U");
            Assert.AreEqual(TruthValue.True, Op.IF(TruthValue.Unknown, TruthValue.True), "IF(U,T) != T");
            Assert.AreEqual(TruthValue.Unknown, Op.IF(TruthValue.Unknown, TruthValue.False), "IF(U,F) != U");
            Assert.AreEqual(TruthValue.Unknown, Op.IF(TruthValue.True, TruthValue.Unknown), "IF(T,U) != U");
            Assert.AreEqual(TruthValue.True, Op.IF(TruthValue.True, TruthValue.True), "IF(T,T) != T");
            Assert.AreEqual(TruthValue.False, Op.IF(TruthValue.True, TruthValue.False), "IF(T,F) != F");
            Assert.AreEqual(TruthValue.True, Op.IF(TruthValue.False, TruthValue.Unknown), "IF(F,U) != T");
            Assert.AreEqual(TruthValue.True, Op.IF(TruthValue.False, TruthValue.True), "IF(F,T) != T");
            Assert.AreEqual(TruthValue.True, Op.IF(TruthValue.False, TruthValue.False), "IF(F,F) != T");
        }

        [TestMethod]
        public void IFF_Tests()
        {
            //Test all nine inputs
            Assert.AreEqual(TruthValue.Unknown, Op.IFF(TruthValue.Unknown, TruthValue.Unknown), "IFF(U,U) != U");
            Assert.AreEqual(TruthValue.Unknown, Op.IFF(TruthValue.Unknown, TruthValue.True), "IFF(U,T) != U");
            Assert.AreEqual(TruthValue.Unknown, Op.IFF(TruthValue.Unknown, TruthValue.False), "IFF(U,F) != U");
            Assert.AreEqual(TruthValue.Unknown, Op.IFF(TruthValue.True, TruthValue.Unknown), "IFF(T,U) != U");
            Assert.AreEqual(TruthValue.True, Op.IFF(TruthValue.True, TruthValue.True), "IFF(T,T) != T");
            Assert.AreEqual(TruthValue.False, Op.IFF(TruthValue.True, TruthValue.False), "IFF(T,F) != F");
            Assert.AreEqual(TruthValue.Unknown, Op.IFF(TruthValue.False, TruthValue.Unknown), "IFF(F,U) != U");
            Assert.AreEqual(TruthValue.False, Op.IFF(TruthValue.False, TruthValue.True), "IFF(F,T) != F");
            Assert.AreEqual(TruthValue.True, Op.IFF(TruthValue.False, TruthValue.False), "IFF(F,F) != T");
        }
    }
}
