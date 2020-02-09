using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTests
{
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
        public void TestSetWithInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("65x", 3433);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetWithNullStringEntry()
        {
            Spreadsheet s = new Spreadsheet();
            string test = null;
            s.SetCellContents("x35", test);
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
            IList<string> testList = s.SetCellContents("a1", 5);
            double value = 5;
            Assert.AreEqual(value, s.GetCellContents("a1"));
            Assert.AreEqual("a1", testList[0]);
        }

        [TestMethod]
        public void TestSetContentsWithString()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> testList = s.SetCellContents("a1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("a1"));
            Assert.AreEqual("a1", testList[0]);
        }

        [TestMethod]
        public void TestGetNamesOfNonEmptyCellsWithEmptySpreadSheet()
        {
            Spreadsheet s = new Spreadsheet();
            IEnumerator<string> test = s.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(test.MoveNext());
        }

        [TestMethod]
        public void TestGetNamesOfNonEmptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            List<string> testList = new List<string>();
            s.SetCellContents("a1", 1);
            s.SetCellContents("a2", "hello");
            s.SetCellContents("a3", "world");

            IEnumerable<string> test = s.GetNamesOfAllNonemptyCells();
            foreach(string contents in test)
            {
                testList.Add(contents);
            }

            Assert.AreEqual(3, testList.Count);
            Assert.AreEqual("a1", testList[0]);
            Assert.AreEqual("a2", testList[1]);
            Assert.AreEqual("a3", testList[2]);

        }

    }
}
