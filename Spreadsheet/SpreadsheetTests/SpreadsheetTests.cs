/// <summary> 
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      2/9/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source.
/// 
/// File Contents
/// 
/// This file contains the tests for the Spreadsheet class.
/// 
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SS;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace SpreadsheetTests
{
    /// <summary>
    /// This is the test class for the Spreadsheet class and is intended
    /// to contain all the SpreadsheetTest Unit Tests.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Error testing. Refer to the test name for what each test is validating.
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContentsInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("24");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContentsWithNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellWithInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("65x", "3433");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellWithNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "3433");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellWithNullStringEntry()
        {
            Spreadsheet s = new Spreadsheet();
            string test = null;
            s.SetContentsOfCell("x35", test);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellWithCircularDependencyFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("c4", "=x6 + 100");
            s.SetContentsOfCell("x6", "=c4+10");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestComplexCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1 + b2");
            s.SetContentsOfCell("b1","=c1+c2");
            s.SetContentsOfCell("b2", "10");
            s.SetContentsOfCell("c1", "5");
            s.SetContentsOfCell("c2", "5");

            s.SetContentsOfCell("c1", "=a1");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestReplaceWithCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1 + b2");
            s.SetContentsOfCell("b1", "=c1+c2");

            s.SetContentsOfCell("b1", "=a1");
        }

        /// <summary>
        /// Method testing. Refer to the test name for what each test is validating.
        /// </summary>

        [TestMethod]
        public void TestGetContentsEmpty()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("x1"));
        }

        [TestMethod]
        public void TestSetContentsWithNumber()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> testList = s.SetContentsOfCell("a1", "5");
            double value = 5;
            Assert.AreEqual(value, s.GetCellContents("a1"));
            Assert.AreEqual("a1", testList[0]);
        }

        [TestMethod]
        public void TestSetContentsWithString()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> testList = s.SetContentsOfCell("a1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("a1"));
            Assert.AreEqual("a1", testList[0]);
        }

        [TestMethod]
        public void TestSetContentsWithFormula()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> testList = s.SetContentsOfCell("b3", "=x1+5");
            Formula testFormula = new Formula("x1+5");
            Assert.AreEqual(testFormula, s.GetCellContents("b3"));
            Assert.AreEqual("b3", testList[0]);
        }

        [TestMethod]
        public void TestGetNamesOfNonEmptyCellsWithEmptySpreadSheet()
        {
            Spreadsheet s = new Spreadsheet();
            IEnumerator<string> test = s.GetNamesOfAllNonemptyCells().GetEnumerator();
        }

        [TestMethod]
        public void TestGetNamesOfNonEmptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            List<string> testList = new List<string>();
            s.SetContentsOfCell("a1", "1");
            s.SetContentsOfCell("a2", "hello");
            s.SetContentsOfCell("a3", "world");

            IEnumerable<string> test = s.GetNamesOfAllNonemptyCells();
            foreach (string contents in test)
            {
                testList.Add(contents);
            }

            Assert.AreEqual(3, testList.Count);
            Assert.AreEqual("a1", testList[0]);
            Assert.AreEqual("a2", testList[1]);
            Assert.AreEqual("a3", testList[2]);
        }

        [TestMethod]
        public void TestMultipleDependentCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("b1", "=a1*2");
            s.SetContentsOfCell("c1", "=b1+a1");

            IList<string> testList = s.SetContentsOfCell("a1", "5");
            Assert.AreEqual(3, testList.Count);
            double value = 10;
            Assert.AreEqual(value, s.GetCellValue("b1"));
            foreach (string dependent in testList)
            {
                Console.WriteLine(dependent);
            }
        }

        [TestMethod]
        public void TestIndirectDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=b1");
            s.SetContentsOfCell("b1","=c1");

            IList<string> testList = s.SetContentsOfCell("c1", "5");
            Assert.AreEqual(3, testList.Count);
            foreach (string dependent in testList)
            {
                Console.WriteLine(dependent);
            }
        }

        [TestMethod]
        public void TestReplaceFormulaWithDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("b1","=a1*2");
            IList<string> testList = s.SetContentsOfCell("a1", "5");
            Assert.AreEqual(2, testList.Count);

            //Current dependents of a1 are itself and b1.
            foreach (string dependent in testList)
            {
                Console.WriteLine(dependent);
            }
            s.SetContentsOfCell("b1", "10");
            IList<string> newA1List = s.SetContentsOfCell("a1", "10");

            //Setting b1 to 10 should have severed the dependency, thus only a1 should be in the list.
            Assert.AreEqual(1, newA1List.Count);
            foreach (string dependent in newA1List)
            {
                Console.WriteLine(dependent);
            }
        }

        [TestMethod]
        public void TestReplaceFormulaWithString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("b1", "=a1*2");
            IList<string> testList = s.SetContentsOfCell("a1", "5");

            //Current dependents of a1 are itself and b1.
            Assert.AreEqual(2, testList.Count);
            foreach (string dependent in testList)
            {
                Console.WriteLine(dependent);
            }

            s.SetContentsOfCell("b1", "hello");
            IList<string> newA1List = s.SetContentsOfCell("a1", "10");

            //Setting cell b1 to "hello" should have severed the dependency, thus only a1 should be in the list.
            Assert.AreEqual(1, newA1List.Count);
            foreach (string dependent in newA1List)
            {
                Console.WriteLine(dependent);
            }
        }

        [TestMethod]
        public void TestReplaceFormulaWithNewFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("b1", "=a1*2");
            s.SetContentsOfCell("c1", "=a1+5");
            IList<string> a1List = s.SetContentsOfCell("a1", "5");

            //Current dependents of a1 are b1 and c1.
            Assert.AreEqual(3, a1List.Count);
            foreach (string dependent in a1List)
            {
                Console.WriteLine(dependent);
            }

            s.SetContentsOfCell("b1", "=d1");
            IList<string> newA1List = s.SetContentsOfCell("a1", "5");

            //Setting b1 to a new Formula should sever the (a1, b1) dependency, but the c1 dependency should be intact.
            Assert.AreEqual(2, newA1List.Count);
            foreach (string dependent in newA1List)
            {
                Console.WriteLine(dependent);
            }
        }

        [TestMethod]
        public void TestSave()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "hello");
            s.SetContentsOfCell("c1", "world");
            s.SetContentsOfCell("d1", "=d4+a5");
            s.Save("test.xml");
        }

        [TestMethod]
        public void TestSimpleEvaluate()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "=a1+5");
            double value = 10;
            Assert.AreEqual(value, s.GetCellValue("b1"));
        }

        [TestMethod]
        public void TestGetSavedVersion()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "hello");
            s.SetContentsOfCell("c1", "world");
            s.SetContentsOfCell("d1", "=d4+a5");
            s.Save("test.xml");

            Assert.AreEqual("default", s.GetSavedVersion("test.xml"));
        }


    }
}
