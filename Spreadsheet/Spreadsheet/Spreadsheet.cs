///<summary>
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      2/9/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source, with the exception of the header comments for the public and
/// protected methods found within this file. 
/// 
/// The header comments of the public and protected methods were written by Professor Jim de St. Germain,
/// for the University of Utah's School of Computing's CS 3500 class, during the Spring 2020 term.
/// 
/// File Contents
/// 
/// This file contains the Spreadsheet class and its respective methods.
/// 
///</summary>

using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    /// <summary>
    /// <para>
    ///     This Spreadsheet class represents a simple spreadsheet that
    ///     consists of an infinite number of cells.
    /// </para>
    /// <para>
    ///     A string is a valid cell name if and only if:
    /// </para>
    /// <list type="number">
    ///      <item> its first character is an underscore or a letter</item>
    ///      <item> its remaining characters (if any) are underscores and/or letters and/or digits</item>
    /// </list>   
    /// <para>
    ///     Note that this is the same as the definition of valid variable from the Formula class.
    /// </para>
    /// 
    /// <para>
    ///     For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    ///     "25", "2x", and "&amp;" are not.  Cell names are case sensitive, so "x" and "X" are
    ///     different cell names.
    /// </para>
    /// 
    /// <para>
    ///     A spreadsheet contains a cell corresponding to every possible cell name. 
    ///     In addition to a name, each cell has a contents and a value.
    /// </para>
    /// 
    /// <para>
    ///     The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    ///     contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    ///     of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// </para>
    /// 
    /// <para>
    ///     In a new spreadsheet, the contents of every cell is the empty string. Note: 
    ///     this is by definition (it is IMPLIED, not stored).
    /// </para>
    /// 
    /// <para>
    ///     The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    ///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
    ///     in the grid.)
    /// </para>
    /// 
    /// <list type="number">
    ///   <item>If a cell's contents is a string, its value is that string.</item>
    /// 
    ///   <item>If a cell's contents is a double, its value is that double.</item>
    /// 
    ///   <item>
    ///      If a cell's contents is a Formula, its value is either a double or a FormulaError,
    ///      as reported by the Evaluate method of the Formula class.  The value of a Formula,
    ///      of course, can depend on the values of variables.  The value of a variable is the 
    ///      value of the spreadsheet cell it names (if that cell's value is a double) or 
    ///      is undefined (otherwise).
    ///   </item>
    /// 
    /// </list>
    /// 
    /// <para>
    ///     Spreadsheets are never allowed to contain a combination of Formulas that establish
    ///     a circular dependency.  A circular dependency exists when a cell depends on itself.
    ///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    ///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    ///     dependency.
    /// </para>
    /// </summary>
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

        private bool fileChange;
        private DependencyGraph graph;
        private Dictionary<string, Cell> cellMap;
        private static Regex variable = new Regex("^[a-zA-Z_]+[0-9]*$"); //Static Regex to define variables: Starting with upper/lower case, or underscore, followed by any number of the digits 0-9.

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Empty argument constructor.
        /// </summary>
        public Spreadsheet():base()
        {
            graph = new DependencyGraph();
            cellMap = new Dictionary<string, Cell>();
        }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
            fileChange = false;
        }

        public Spreadsheet(string filepath, Func<string, string> normalize, Func<string, bool> isValid, string version) :base(isValid, normalize, version)
        {
            this.IsValid = IsValid;
            this.Normalize = Normalize;
            this.Version = version;
            fileChange = false;
        }

        /// <summary>
        ///   Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is null or invalid
        /// </exception>
        /// 
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// 
        /// <returns>
        ///   The return value should be either a string, a double, or a Formula.
        ///   See the class header summary 
        /// </returns>
        public override object GetCellContents(string name)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else
            {
                if(cellMap.TryGetValue(name, out Cell cell))
                {
                    return cell.Contents;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerates 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            Dictionary<string, Cell>.KeyCollection nonEmptyCells = cellMap.Keys;
            return nonEmptyCells;
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///      The method returns a set consisting of name plus the names of all other cells whose value depends, 
        ///      directly or indirectly, on the named cell.
        ///   </para>
        /// 
        ///   <para>
        ///      For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///      set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            List<string> effectedCellList = CreateCell(name, number);
            return effectedCellList;
        }

        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   The method returns a set consisting of name plus the names of all 
        ///   other cells whose value depends, directly or indirectly, on the 
        ///   named cell.
        /// 
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            if(text == null) { throw new ArgumentNullException(); }
            else
            {
                List<string> effectedCellList = CreateCell(name, text);
                return effectedCellList;
            }
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If formula parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name</param>
        /// <param name="formula"> The content of the cell</param>
        /// 
        /// <returns>
        ///   <para>
        ///     The method returns a Set consisting of name plus the names of all other 
        ///     cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///   <para> 
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// 
        /// </returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {   
            if (formula == null) { throw new ArgumentNullException(); }
            else
            {
                ///To check for circular dependencies, gather any variables found within the formula that was passed in.
                IEnumerable<string> variables = formula.GetVariables();
                foreach (string variable in variables)
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
                        graph.RemoveDependency(variable, name);
                        throw new CircularException();
                    }
                }
                //If there were no circular dependencies found, it can be created normally.
                List<string> effectedCells = CreateCell(name, formula);

                return effectedCells;
            }
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. 
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// 
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }

            HashSet<string> dependents = new HashSet<string>(graph.GetDependents(name));

            return dependents;
        }

        /// <summary>
        /// <para>
        /// A private helper method utilized when setting the contents of a cell.
        /// This is used in the private helper method CreateCell (which is called when
        /// calling SetCellContents), mostly used to condense code.
        /// </para>
        /// 
        /// <para>
        /// GetCellsToRecalculate (which is found in AbstractSpreadSheet) is used here,
        /// because it is effectively recursive. Each SetCellContents returns a list
        /// of dependents, which includes indirect dependents. For example:
        /// </para>
        ///
        /// <list type="bullet">
        ///     <item>A1 contains the formula B1+5</item>
        ///     <item>B1 contains the formula C1+10</item>
        ///     <item>C1 contains the number 100</item>
        /// </list>
        /// 
        /// <para>
        /// In that example, C1 has a direct dependent of B1, and since B1 has a dependent
        /// of A1, this indirectly means that C1 also has a dependent of A1.
        /// </para>
        /// 
        /// <para>
        /// This means that each cell needs to be visited in order to establish all of the
        /// dependencies that relate to the given cell (the parameter passed in).
        /// </para>
        /// </summary>
        /// <param name="name">The name of the cell in the spreadsheet.</param>
        /// <returns>A List of strings, which are the direct and indirect dependents of the given cell.</returns>
        private List<string> CreateEffectedCellsList(string name)
        {
            
            IEnumerable<string> dependents = GetCellsToRecalculate(name);
            List<string> effectedCells = new List<string>();

            foreach (string dependent in dependents)
            {
                effectedCells.Add(dependent);
            }
            return effectedCells;
        }

        /// <summary>
        /// This is a private helper method used in SetCellContents. Each SetCellContents
        /// function is mostly doing the same thing, so this is primarily used to condense code.
        /// </summary>
        /// <param name="name">The name of the cell in the spreadsheet.</param>
        /// <param name="input">The object being entered. This should be a Formula, a double, or a string.</param>
        /// <returns>A List of strings, which are the direct and indirect dependents of the given cell. </returns>
        private List<string> CreateCell(string name, object input)
        {
            if (cellMap.TryGetValue(name, out Cell cell))
            {
                //If the passed in name is already established as a Cell, just alter the contents directly.
                cell.Contents = input;

                //However, this also indicates that the cell's contents is being replaced with new contents.
                //If the new contents are a string or a double, then any prior dependencies need to be cleared,
                //in the case where the old contents were a Formula.
                if (input is string || input is double)
                {
                    graph.ReplaceDependees(name, new HashSet<string>());
                }

                //If the replacement is a Formula, then new dependencies in the Dependency Graph need to be
                //established.
                else if (input is Formula)
                {
                    Formula formula = (Formula)input;
                    graph.ReplaceDependees(name, formula.GetVariables());
                }

                List<string> effectedCells = CreateEffectedCellsList(name);

                return effectedCells;
            }

            //If the name is not in the spreadsheet, then it can be added as a new addition.
            else
            {
                Cell newCell = new Cell(input);
                cellMap.Add(name, newCell);
                List<string> effectedCells = CreateEffectedCellsList(name);

                return effectedCells;
            }
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (name is null || !variable.IsMatch(name)) { throw new InvalidNameException(); }
            else
            {
                if(double.TryParse(content, out double number))
                {
                    return SetCellContents(name, number);
                }
                else if(content[0] == '=')
                {
                    string formulaString = content.Substring(1);
                    Formula formula = new Formula(formulaString);
                    return SetCellContents(name, formula);
                }
                else
                {
                    return SetCellContents(name, content);
                }
            }
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }
    }
}
