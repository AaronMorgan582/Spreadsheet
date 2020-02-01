using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTests
{

    [TestClass]
    public class FormulaTests
    {
        public static string Normalize(string formula)
        {
            Regex variable = new Regex("^[a-zA-Z]+");
            if (variable.IsMatch(formula)){ formula = formula.ToUpper();}

            return formula;
        }

        public static bool IsValid(string formula)
        {
            Regex variableStart = new Regex("^[a-zA-Z]+");
            Regex validVariable = new Regex("^[a-zA-Z]+[0-9]+");

            if (variableStart.IsMatch(formula))
            {
                if (!validVariable.IsMatch(formula)) { return false; }
            }

            return true;
        }

        public static double LookUp(string variable)
        {
            if(variable == "X1") { return 5; }
            if(variable == "X2") { return 1; }
            else throw new ArgumentException();
        }


        ///Error Tests
        
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorIsValidError()
        {
            Formula test = new Formula("x+y3", Normalize, IsValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorInvalidFormat()
        {
            Formula test = new Formula("2x+y3", Normalize, IsValid);
        }

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

        [TestMethod()]
        public void TestDivideByZero()
        {
            Formula test = new Formula("5/0");
            FormulaError testObject = new FormulaError("Cannot divide by zero.");
            Assert.AreEqual(testObject, test.Evaluate(null));
        }


        /// Method tests

        [TestMethod()]
        public void TestSimpleVariableFormula()
        {
            Formula test = new Formula("x1+x2", Normalize, IsValid);
            double value = 6;
            Assert.AreEqual(value, test.Evaluate(LookUp));
        }

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

        [TestMethod()]
        public void TestSimpleAddition()
        {
            Formula test = new Formula("1+1");
            double value = 2;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestSimpleSubtract()
        {
            Formula test = new Formula("4-1");
            double value = 3;
            Assert.AreEqual(value, test.Evaluate(null));
        }
        [TestMethod()]
        public void TestSimpleMultiply()
        {
            Formula test = new Formula("5*3");
            double value = 15;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestSimpleParentheses()
        {
            Formula test = new Formula("(5+3)");
            double value = 8;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestSimpleDivision()
        {
            Formula test = new Formula("(6/2)");
            double value = 3;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestComplexFormula()
        {
            Formula test = new Formula("(30*2) - (4*(4+1))/2*2");
            double value = 40;
            Assert.AreEqual(value, test.Evaluate(null));
        }
    }
}
