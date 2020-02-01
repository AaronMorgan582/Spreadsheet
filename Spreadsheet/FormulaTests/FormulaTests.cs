using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        ///Error Tests

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidStartingToken()
        {
            Formula test = new Formula("+5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidEndingToken()
        {
            Formula test = new Formula("5+");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyOpeningParenthesis()
        {
            Formula test = new Formula("(5+4");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyClosingParenthesis()
        {
            Formula test = new Formula("5+4)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMismatchedParentheses()
        {
            Formula test = new Formula("(5+4))");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestImpliedMultiplication()
        {
            Formula test = new Formula("4(5+3)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorFollowingRule()
        {
            Formula test = new Formula("5++10");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidCharacter()
        {
            Formula test = new Formula("5@20");
        }
        /// Function tests.

        [TestMethod()]
        public void TestSimpleFormulaToString()
        {
            Formula test = new Formula("1+1");
            Assert.AreEqual("1+1", test.ToString());
        }

        [TestMethod()]
        public void TestSimpleFormulaWithWhiteSpaceToString()
        {
            Formula test = new Formula("1 + 1");
            Assert.AreEqual("1+1", test.ToString());
        }

        [TestMethod()]
        public void TestSimpleFormulaWithVariables()
        {
            Formula test = new Formula("x + y");
            Assert.AreEqual("x+y", test.ToString());
        }

        [TestMethod()]
        public void TestEqualsFalse()
        {
            Formula test = new Formula("x + y");
            Formula testUpper = new Formula("X+Y");
            Assert.IsFalse(test.Equals(testUpper));
        }

        [TestMethod()]
        public void TestEqualsTrueWithIntAndDecimal()
        {
            Formula test = new Formula("2 + x7");
            Formula testDecimal = new Formula("2.000 + x7");
            Assert.IsTrue(test.Equals(testDecimal));
        }

        [TestMethod()]
        public void TestEqualsTrueWithOnlyDecimal()
        {
            Formula test = new Formula("2.0 + x7");
            Formula testDecimal = new Formula("2.000 + x7");
            Assert.IsTrue(test.Equals(testDecimal));
        }

        [TestMethod()]
        public void TestGetVariables()
        {
            Formula test = new Formula("3+x+X*z-5");
            IEnumerable<string> variables = test.GetVariables();
            foreach(string variable in variables)
            {
                Console.Write(variable + " ");
            }

        }
    }
}
