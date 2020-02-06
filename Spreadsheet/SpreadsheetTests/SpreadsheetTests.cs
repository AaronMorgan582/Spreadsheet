using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SS;

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
    }
}
