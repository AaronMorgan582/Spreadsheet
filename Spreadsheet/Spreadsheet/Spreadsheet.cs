using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private class Cell
        {
            private object cellValue;
            private object cellContents = "";

            public Cell(object input)
            {
                cellContents = input;
            }

            public object Contents{ 
                get { return cellContents; }
                set { this.cellContents = value; }
            }

            public object Value { 
                get { return cellValue; }
                set { this.cellValue = value; }
            }
        }

        private DependencyGraph graph;
        private Dictionary<string, Cell> cellDictionary;
        private static Regex variable = new Regex("^[a-zA-Z_]+");

        public Spreadsheet()
        {
            graph = new DependencyGraph();
            cellDictionary = new Dictionary<string, Cell>();
        }

        public override object GetCellContents(string name)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else
            {
                if(cellDictionary.TryGetValue(name, out Cell cell))
                {
                    return cell.Contents;
                }
                else
                {
                    return "";
                }
            }
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            Dictionary<string, Cell>.KeyCollection nonEmptyCells = cellDictionary.Keys;
            return nonEmptyCells;
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }
}
