using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContentsInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("24");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContentsNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

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
    }
}
