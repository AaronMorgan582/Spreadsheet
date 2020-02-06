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
        public void TestSetContents()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> testList = s.SetCellContents("a1", 5);
            double value = 5;
            Assert.AreEqual(value, s.GetCellContents("a1"));
            Assert.AreEqual("a1", testList[0]);
        }
    }
}
