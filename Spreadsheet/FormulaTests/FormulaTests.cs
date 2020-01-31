using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
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

    }
}
