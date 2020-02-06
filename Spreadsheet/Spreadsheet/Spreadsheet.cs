using SpreadsheetUtilities;
using System;
using System.Collections;
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
                if(input is double || input is string)
                {
                    cellValue = input;
                }
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
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else
            {
                if(cellDictionary.TryGetValue(name, out Cell cell))
                {
                    cell.Contents = number;
                    List<string> effectedCells = CreateEffectedCellsList(name);

                    return effectedCells;
                }
                else
                {
                    Cell newCell = new Cell(number);
                    cellDictionary.Add(name, newCell);
                    graph.ReplaceDependents(name, new HashSet<string>());
                    List<string> effectedCells = CreateEffectedCellsList(name);

                    return effectedCells;
                }
            }
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

        private List<string> CreateEffectedCellsList(string name)
        {
            IEnumerable<string> dependents = graph.GetDependents(name);
            List<string> effectedCells = new List<string>();
            effectedCells.Add(name);

            foreach (string dependent in dependents)
            {
                effectedCells.Add(dependent);
            }
            return effectedCells;
        }
    }
}
