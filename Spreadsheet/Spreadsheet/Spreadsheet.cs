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
        /// <summary>
        /// Private class to hold the contents/values of each cell in the spreadsheet.
        /// 
        /// Getters and Setters are implemented for both the value and contents of the Cell.
        /// </summary>
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
        //Static Regex to define variables: Starting with upper/lower case, or underscore, followed by any number of the digits 0-9.
        private static Regex variable = new Regex("^[a-zA-Z_]+[0-9]*$");

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
                List<string> effectedCellList = CreateCell(name, number);
                return effectedCellList;
            }
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else if(text == null) { throw new ArgumentNullException(); }
            else
            {
                List<string> effectedCellList = CreateCell(name, text);
                return effectedCellList;
            }
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else if (formula == null) { throw new ArgumentNullException(); }
            else
            {
  

                ///To check for circular dependencies, gather any variables found within the formula that was passed in.
                IEnumerable<string> variables = formula.GetVariables();
                foreach(string variable in variables)
                {
                    ///Each of them needs to be added to the dependency graph first, in order for GetCellsToRecalculate to run.
                    graph.AddDependency(variable, name);
                    try
                    {
                        GetCellsToRecalculate(variable);
                    }
                    //If it catches the exception, it needs to remove the dependency, then throw the exception again.
                    catch (CircularException)
                    {
                        graph.RemoveDependency(name, variable);
                        throw new CircularException();
                    }
                }
                //If there were no circular dependencies found, it can be created normally.
                List<string> effectedCellList = CreateCell(name, formula);
                return effectedCellList;
            }
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            IEnumerable<string> gatheredDependents = graph.GetDependents(name);
            HashSet<string> dependents = new HashSet<string>();
            foreach(string dependent in gatheredDependents) { dependents.Add(dependent); }

            return dependents;
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

        private List<string> CreateCell(string name, object input)
        {
            if (cellDictionary.TryGetValue(name, out Cell cell))
            {
                cell.Contents = input;
                if(input is string || input is double)
                {
                    graph.ReplaceDependees(name, new HashSet<string>());
                }
                else if(input is Formula)
                {
                    Formula formula = (Formula)input;
                    graph.ReplaceDependees(name, formula.GetVariables());
                }

                List<string> effectedCells = CreateEffectedCellsList(name);

                return effectedCells;
            }
            else
            {
                Cell newCell = new Cell(input);
                cellDictionary.Add(name, newCell);
                List<string> effectedCells = CreateEffectedCellsList(name);

                return effectedCells;
            }
        }
    }
}
