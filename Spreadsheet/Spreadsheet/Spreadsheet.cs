///<summary>
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      2/16/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source, with the exception of:
/// 
/// <list type="bullet">
///     <item>The header comments for public and protected methods found within this file.</item>
///     <item>The code for reading and writing XML is mostly an adaptation of code provided by Professor Jim de St. Germain.</item>
/// </list>
/// 
/// The header comments of the public and protected methods were written by Professor Jim de St. Germain,
/// for the University of Utah's School of Computing's CS 3500 class, during the Spring 2020 term.
/// 
/// File Contents
/// 
/// This file contains the Spreadsheet class and its respective methods.
///</summary>

using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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

        private DependencyGraph graph;
        private Dictionary<string, Cell> cellMap;
        private static Regex cellName = new Regex("^[a-zA-Z]+[0-9]*$"); //Static Regex to define Cell names: Starting with upper/lower case, or underscore, followed by any number of the digits 0-9.

        /// <summary>
        /// Private class to hold the contents/values of each cell in the Spreadsheet.
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

        /// <summary>
        /// Empty argument constructor.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            graph = new DependencyGraph();
            cellMap = new Dictionary<string, Cell>();
        }


        /// <summary>
        /// Constructor that takes a delegate to define valid variables,
        /// a delegate that normalizes the cell names and variables,
        /// and a string that defines the version.
        /// 
        /// </summary>
        /// <param name="isValid">The delegate that validates any variables used in Formulas.</param>
        /// <param name="normalize">The delegate that normalizes cell names and variables.</param>
        /// <param name="version">The version of the spreadsheet.</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            graph = new DependencyGraph();
            cellMap = new Dictionary<string, Cell>();
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
        }

        /// <summary>
        /// Constructor that takes a string to define where an existing Spreadsheet is,
        /// a delegate to define valid variables,
        /// a delegate that normalizes the cell names and variables,
        /// and a string that defines the version.
        /// 
        /// <exception cref="SpreadsheetReadWriteException"> 
        ///   If constructor version does not match the version retrieved from the saved Spreadsheet, throw a SpreadsheetReadWriteException.
        /// </exception>
        /// 
        /// </summary>
        /// <param name="filepath">The filepath to locate a previously saved Spreadsheet.</param>
        /// <param name="isValid">The delegate that validates any variables used in Formulas.</param>
        /// <param name="normalize">The delegate that normalizes cell names and variables.</param>
        /// <param name="version">The version of the spreadsheet.</param>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            graph = new DependencyGraph();
            cellMap = new Dictionary<string, Cell>();
            this.IsValid = IsValid;
            this.Normalize = Normalize;

            string readVersion = GetSavedVersion(filepath);
            if (!readVersion.Equals(version)) { throw new SpreadsheetReadWriteException("Versions are not equivalent"); }
            else { this.Version = version; }

            string cellname = "";

            //If the Constructor with a filepath is used, this means that a Spreadsheet object (should) already have been written
            //once. This means that the the file needs to be read and the Cell information retrieved needs to be added to the newly
            //constructed Spreadsheet.
            using (XmlReader reader = XmlReader.Create(filepath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        { 
                            case "name":
                                reader.Read();
                                cellname = reader.Value;
                                break;

                            case "contents":
                                reader.Read();
                                SetContentsOfCell(cellname, reader.Value);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

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
            if (name is null || !cellName.IsMatch(name)) { throw new InvalidNameException(); }
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
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"> 
        ///   If the content parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        ///
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluated in the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (name is null || !cellName.IsMatch(name)) { throw new InvalidNameException(); }
            else if (content == null) { throw new ArgumentNullException(); }
            else
            {
                //Normalize the name of the cell with the normalize delegate.
                string normalizedCell = this.Normalize(name);

                //Setting a cell's contents means the file has changed.
                this.Changed = true;

                //Determine whether the passed content parameter is a double, string, or Formula.
                if (double.TryParse(content, out double number))
                {
                    IList<string> cellsToEvaluate = SetCellContents(normalizedCell, number);

                    //If the passed in cell had dependents, then each of those dependents need to be recalculated.
                    foreach (string cell in cellsToEvaluate)
                    {
                        Recalculate(cell);
                    }
                    return cellsToEvaluate;
                }

                else if (content[0] == '=')
                {
                    //Normalize the formula with the normalize delegate.
                    string formulaString = content.Substring(1);
                    string normalizedFormulaString = this.Normalize(formulaString);

                    //The formula has to pass the given validation delegate.
                    if (this.IsValid(normalizedFormulaString) == true)
                    {
                        Formula formula = new Formula(normalizedFormulaString);
                        IList<string> cellsToEvaluate = SetCellContents(normalizedCell, formula);

                        //To recalculate any dependents, as the above comment mentioned (with doubles).
                        foreach (string cell in cellsToEvaluate)
                        {
                            Recalculate(cell);
                        }
                        return cellsToEvaluate;
                    }
                    else
                    {
                        throw new FormulaFormatException("Invalid formula.");
                    }
                }

                else
                {
                    //Recalculation is not necessary for strings.
                    return SetCellContents(normalizedCell, content);
                }
            }
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// 
        /// <exception cref="SpreadsheetReadWriteException"> 
        ///   If the filename doesn't exist, or if the version wasn't found,
        ///   throw a SpreadsheetReadWriteException.
        /// </exception>
        /// 
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    return reader.GetAttribute("version");
                            }
                        }
                    }
                }
                throw new SpreadsheetReadWriteException("Version not found.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Invalid filepath.");
            }

        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        ///<exception cref="SpreadsheetReadWriteException">
        /// If the filename is not a valid path/name, a SpreadSheetReadWriteException is thrown.
        ///</exception>
        /// </summary>
        public override void Save(string filename)
        {
            this.Changed = false;

            //Format settings for readability.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", this.Version);

                    //Writes the contents of each cell found within the Spreadsheet. WriteCellContents is found within Spreadsheet, not XmlWriter.
                    this.WriteCellContents(writer);

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Invalid filepath.");
            }
        }

        ///<summary>
        /// <para>Retrieves the Value of a Cell (as opposed to the Contents).</para>
        /// <para>
        ///     For Cells containing doubles and strings, the Contents and Value are the same.
        ///     For Cells containing Formulas, the Value is the calculated Formula.
        /// </para>
        /// 
        /// <para>
        ///     In the case where a Cell contains a Formula that is dependent on another Cell
        ///     (such as A1*2), but A1 has nothing in it, then the Value is a SpreadsheetUtilities.FormulaError.
        /// </para>
        ///</summary>
        /// 
        /// <param name="name">The name of the Cell within the Spreadsheet.</param>
        /// <returns>The Value of the Cell, which is a double, string, or a SpreadsheetUtilities.FormulaError.</returns>
        public override object GetCellValue(string name)
        {
            if (name is null || !cellName.IsMatch(name)) { throw new InvalidNameException(); }
            else
            {
                Cell cell = cellMap[name];
                return cell.Value;
            }
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
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
            List<string> effectedCellList = CreateCell(name, text);
            return effectedCellList;
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
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
            if (name is null || !cellName.IsMatch(name)) { throw new InvalidNameException(); }

            HashSet<string> dependents = new HashSet<string>(graph.GetDependents(name));

            return dependents;
        }

        ///<summary>
        /// <para>
        ///     A private helper method utilized when setting the contents of a cell.
        ///     This is used in the private helper method CreateCell (which is called when
        ///     calling SetCellContents), mostly used to condense code.
        /// </para>
        /// 
        /// <para>
        ///     GetCellsToRecalculate (which is found in AbstractSpreadSheet) is used here,
        ///     because it is effectively recursive. Each SetCellContents returns a list
        ///     of dependents, which includes indirect dependents. For example:
        /// </para>
        ///
        /// <list type="bullet">
        ///     <item>A1 contains the formula B1+5</item>
        ///     <item>B1 contains the formula C1+10</item>
        ///     <item>C1 contains the number 100</item>
        /// </list>
        /// 
        /// <para>
        ///     In that example, C1 has a direct dependent of B1, and since B1 has a dependent
        ///     of A1, this indirectly means that C1 also has a dependent of A1.
        /// </para>
        /// 
        /// <para>
        ///     This means that each cell needs to be visited in order to establish all of the
        ///     dependencies that relate to the given cell (the parameter passed in).
        /// </para>
        ///</summary>
        ///
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

        ///<summary>
        /// This is a private helper method used in SetCellContents. Each SetCellContents
        /// method is mostly doing the same thing, so this is primarily used to condense code.
        ///</summary>
        ///
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

        ///<summary>
        /// A private helper method that is used by the Save method to
        /// write the information of each Cell found in the Spreadsheet.
        ///</summary>
        /// <param name="writer">The XML Writer object created in the Save method.</param>
        private void WriteCellContents(XmlWriter writer)
        {
            IEnumerable<string> usedCells = this.GetNamesOfAllNonemptyCells();
            foreach(string cell in usedCells)
            {
                writer.WriteStartElement("cell");
                writer.WriteElementString("name", cell);
                if(GetCellContents(cell) is Formula)
                {
                    //Formulas need to have the "=" sign prepended.
                    string formula = "=" + GetCellContents(cell).ToString();
                    writer.WriteElementString("contents", formula);
                }
                else
                {
                    writer.WriteElementString("contents", GetCellContents(cell).ToString());
                }
                writer.WriteEndElement();
            }

        }

        ///<summary>
        /// <para>The delegate that is used by Evaluate method for Formulas.</para>
        /// <para>
        ///     In a Spreadsheet, the variables in a Formula should be the given Cells
        ///     found within the Spreadsheet.
        /// </para>
        /// 
        /// <exception cref="ArgumentException"> 
        ///   If the given Cell does not a have a value currently associated 
        ///   with it, or if the value is not a double, throw an ArgumentException.
        /// </exception>
        /// </summary>
        /// 
        /// <param name="cellName">The name of the Cell within the Spreadsheet.</param>
        /// <returns>The value (which should be a double) associated with the Cell.</returns>
        private double LookUp(string cellName)
        {
            if (cellMap.ContainsKey(cellName))
            {
                if (this.GetCellValue(cellName) is double)
                {
                    return (double)this.GetCellValue(cellName);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// This private helper method evaluates a given Cell
        /// with the Formula's Evaluate method, then sets the Value
        /// of that Cell to what it was evaluated to.
        /// </summary>
        /// <param name="name">The name of the Cell within the Spreadsheet.</param>
        private void Recalculate(string name)
        {
            if(cellMap.TryGetValue(name, out Cell cell))
            {
                if(cell.Contents is Formula)
                {
                    Formula formula = (Formula)cell.Contents;
                    cell.Value = formula.Evaluate(LookUp);
                }
            }
        }
    }
}
