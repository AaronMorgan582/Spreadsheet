/// <summary> 
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      2/1/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source.
/// 
/// File Contents
/// 
/// This file contains the tests for the Formula class.
/// 
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTests
{
    /// <summary>
    /// This is the test class for the Formula class and is intended
    /// to contain all the FormulaTest Unit Tests.
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        /// <summary>
        /// This is the delegate that can be used when creating a Formula object.
        /// 
        /// The given string should be part of the total formula. This method will check to see if it is a letter, 
        /// and if it is, it will change it to upper-case.
        /// </summary>
        /// <param name="token">The part of the formula that needs to be checked.</param>
        /// <returns>The same token, modified to be upper-case.</returns>
        public static string Normalize(string token)
        {
            Regex variable = new Regex("^[a-zA-Z]+");
            if (variable.IsMatch(token)){ token = token.ToUpper();}

            return token;
        }

        /// <summary>
        /// This is the second delegate that can be used when creating a Formula object.
        /// 
        /// The given string should be part of the total formula. This method checks to see if it passes the formatting requirement
        /// (in this case, if the variable is a letter followed by a number).
        /// </summary>
        /// <param name="token">The part of the formula that needs to be checked.</param>
        /// <returns>Returns true if the token is in the proper format, false otherwise.</returns>
        public static bool IsValid(string token)
        {
            Regex variableStart = new Regex("^[a-zA-Z]+");
            Regex validVariable = new Regex("^[a-zA-Z]+[0-9]+");

            if (variableStart.IsMatch(token))
            {
                if (!validVariable.IsMatch(token)) { return false; }
            }

            return true;
        }
        /// <summary>
        /// This is the delegate for a Formula object's Evaluate method, which will
        /// look up the passed in variable.
        /// 
        /// This throws an ArgumentException if the variable is not found.
        /// </summary>
        /// <param name="variable">The variable to be looked up.</param>
        /// <returns>The value (a double) associated with the variable</returns>
        public static double LookUp(string variable)
        {
            if(variable == "X1") { return 5; }
            if(variable == "X2") { return 1; }
            if(variable == "X3") { return 0; }
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
        public void TestParenthesesFollowingRule()
        {
            Formula test = new Formula("(5+(+4))");
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
        public void TestEmptyFormula()
        {
            Formula test = new Formula(" ");
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
        public void TestExtraFollowingRule()
        {
            Formula test = new Formula("2x+1");
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
            Formula test = new Formula("(5/0)");
            FormulaError testObject = new FormulaError("Cannot divide by zero.");
            Assert.AreEqual(testObject, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestDivideByZeroWithVariable()
        {
            Formula test = new Formula("5/x3", Normalize, IsValid);
            FormulaError testObject = new FormulaError("Cannot divide by zero.");
            Assert.AreEqual(testObject, test.Evaluate(LookUp));
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
        public void TestGetVariablesWithNormalize()
        {
            Formula test = new Formula("3+x+X*z-5", Normalize, s => true);
            IEnumerable<string> variables = test.GetVariables();
            foreach (string variable in variables)
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
        public void TestSimpleMultiplyWithVariable()
        {
            Formula test = new Formula("(x1*x2)", Normalize, IsValid);
            double value = 5;
            Assert.AreEqual(value, test.Evaluate(LookUp));
        }

        [TestMethod()]
        public void TestSimpleParentheses()
        {
            Formula test = new Formula("(5+3)");
            double value = 8;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestSubtractionWithParentheses()
        {
            Formula test = new Formula("(5-3)");
            double value = 2;
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
        public void TestSimpleDivisionWithVariable()
        {
            Formula test = new Formula("(x1/x2)", Normalize, IsValid);
            double value = 5;
            Assert.AreEqual(value, test.Evaluate(LookUp));
        }

        [TestMethod()]
        public void TestComplexFormula()
        {
            Formula test = new Formula("(30*2) - (4*(4+1))/2*2");
            double value = 40;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestDivisionFollowedByParentheses()
        {
            Formula test = new Formula("5/(1*5)");
            double value = 1;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestAdditionFollowedBySubtraction()
        {
            Formula test = new Formula("5+1-5");
            double value = 1;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestSubtractionFollowedByAddition()
        {
            Formula test = new Formula("5-1+5");
            double value = 9;
            Assert.AreEqual(value, test.Evaluate(null));
        }

        [TestMethod()]
        public void TestHashCodeEqual()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x+y", Normalize, s => true);
            Assert.AreEqual(test.GetHashCode(), equivTest.GetHashCode());
        }

        [TestMethod()]
        public void TestHashCodeNotEqual()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x1+y", Normalize, s => true);
            Assert.IsFalse(test.GetHashCode() == equivTest.GetHashCode());
        }

        [TestMethod()]
        public void TestEqualsTrue()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x+y", Normalize, s => true);
            Assert.IsTrue(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsFail()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x1+y", Normalize, s => true);
            Assert.IsFalse(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsDifferentLength()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x1+y+5+4", Normalize, s => true);
            Assert.IsFalse(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsTrueWithNumber()
        {
            Formula test = new Formula("2+Y");
            Formula equivTest = new Formula("2.000+y", Normalize, s => true);
            Assert.IsTrue(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsWithDifferentNumbers()
        {
            Formula test = new Formula("25+Y");
            Formula equivTest = new Formula("2.000+y", Normalize, s => true);
            Assert.IsFalse(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsFalseWithNumber()
        {
            Formula test = new Formula("25+Y");
            Formula equivTest = new Formula("x+y", Normalize, s => true);
            Assert.IsFalse(test.Equals(equivTest));
        }

        [TestMethod()]
        public void TestEqualsWithDifferentObject()
        {
            Formula test = new Formula("25+Y");
            int[] arr = new int[5];
            Assert.IsFalse(test.Equals(arr));
        }

        [TestMethod()]
        public void TestEqualOperatorTrue()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x+y", Normalize, s => true);
            Assert.IsTrue(test == equivTest);
        }

        [TestMethod()]
        public void TestEqualOperatorFalse()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("4+y", Normalize, s => true);
            Assert.IsFalse(test == equivTest);
        }

        [TestMethod()]
        public void TestNotEqualOperatorTrue()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("4+y", Normalize, s => true);
            Assert.IsTrue(test != equivTest);
        }

        [TestMethod()]
        public void TestNotEqualOperatorFalse()
        {
            Formula test = new Formula("X+Y");
            Formula equivTest = new Formula("x+y", Normalize, s => true);
            Assert.IsFalse(test != equivTest);
        }
    }
}
