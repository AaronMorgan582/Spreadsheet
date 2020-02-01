///<summary>
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      2/1/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part
/// or in whole from another source, with the exception of the skeleton implementation (read below).
/// 
///Skeleton written by Joe Zachary for CS 3500, September 2013
/// Read the entire skeleton carefully and completely before you
/// do anything else!
///
/// Version 1.1 (9/22/13 11:45 a.m.)
///
/// Change log:
///  (Version 1.1) Repaired mistake in GetTokens
///  (Version 1.1) Changed specification of second constructor to
///                clarify description of how validation works
///
/// (Daniel Kopta) 
/// Version 1.2 (9/10/17) 
///
/// Change log:
///  (Version 1.2) Changed the definition of equality with regards
///                to numeric tokens
///
/// File Contents
/// 
/// This file contains the Formula class and its respective methods. The Formula
/// class takes a string, in proper infix notation format, to be stored or calculated.
/// 
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        public delegate string normalizeFormula(string formula);
        public delegate bool validateFormula(string formula);

        private string expression;
        private Stack<double> values;
        private Stack<string> operators;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity method, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            ParsingRules(formula);

            string[] tokens = GetTokens(formula).ToArray();

            for (int index = 0; index < tokens.Length; index++)
            {
                string token = normalize(tokens[index]);
                if (isValid(token) == true)
                {
                    expression += token;
                }
                else
                {
                    throw new FormulaFormatException("Invalid variable format.");
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            values = new Stack<double>();
            operators = new Stack<string>();

            string[] substrings = GetTokens(expression).ToArray();

            //Regex expression to capture any string that starts with upper/lower case letters, followed by any length of numbers 0-9.
            Regex reg = new Regex("^[a-zA-Z]+[0-9]+");

            for (int index = 0; index < substrings.Length; index++)
            {
                string token = substrings[index];

                //If the token is a number value:
                if (double.TryParse(token, out double number))
                {
                    if (operators.Count != 0 && operators.Peek() == "*")
                    {
                        MultOrDivide(values, operators, number);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "/")
                    {
                        if (number == 0)
                        {
                            return new FormulaError("Cannot divide by zero.");
                        }
                        else
                        {
                            MultOrDivide(values, operators, number);
                        }
                    }
                    else
                    {
                        values.Push(number);
                    }
                }

                //Otherwise, if it's a + or -:
                else if (token == "+" || token == "-")
                {
                    if (operators.Count != 0 && operators.Peek() == "+")
                    {
                        AddOrSubtract(values, operators);
                        operators.Push(token);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "-")
                    {
                        AddOrSubtract(values, operators);
                        operators.Push(token);
                    }
                    else
                    {
                        operators.Push(token);
                    }
                }

                //Otherwise, if it's a *, /, or (:
                else if (token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }

                //Otherwise, if it's a ):
                else if (token == ")")
                {
                    if (operators.Count != 0 && operators.Peek() == "+")
                    {
                        AddOrSubtract(values, operators);
                        CheckParentheses(operators);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "-")
                    {
                        AddOrSubtract(values, operators);
                        CheckParentheses(operators);
                    }

                    if (operators.Count != 0 && operators.Peek() == "*")
                    {
                        double firstStackNumber = values.Pop();
                        MultOrDivide(values, operators, firstStackNumber);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "/")
                    {
                        double firstStackNumber = values.Pop();
                        MultOrDivide(values, operators, firstStackNumber);
                    }
                }
                //If the token is anything else, it should be a variable that needs to be looked up via the delegate.
                else if (reg.Match(token).Success)
                {
                    double variableValue = lookup(token);
                    if (operators.Count != 0 && operators.Peek() == "*")
                    {
                        MultOrDivide(values, operators, variableValue);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "/")
                    {
                        if (variableValue == 0)
                        {
                            return new FormulaError("Cannot divide by zero.");
                        }
                        else
                        {
                            MultOrDivide(values, operators, variableValue);
                        }
                    }
                    else
                    {
                        values.Push(variableValue);
                    }
                }
                else { return new FormulaError("Invalid variable"); }
            }
            //If the Value Stack has more than 1 number in it, then the calculation is not finished.
            if (values.Count > 1)
            {
                double finalResult = 0;

                //It needs to loop over the Operator Stack, because there should be more values than operators. If you try to
                //loop over the Value Stack, it will error because there won't be enough operators to process.
                for (int i = 0; i < operators.Count; i++)
                {
                    double firstStackNumber = values.Pop();
                    if (operators.Peek() == "+")
                    {
                        double secondStackNumber = values.Pop();
                        double result = secondStackNumber + firstStackNumber;
                        operators.Pop();
                        finalResult += result;
                    }
                    else if (operators.Peek() == "-")
                    {
                        double secondStackNumber = values.Pop();
                        double result = secondStackNumber - firstStackNumber;
                        operators.Pop();
                        finalResult += result;
                    }
                }
                return finalResult;
            }
            return values.Pop();
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            string[] tokens = GetTokens(expression).ToArray();
            Regex variables = new Regex("^[a-zA-Z]+");

            //A HashSet should be used for potential duplicates (e.g. new Formula("x+X*z", N, s => true)).
            HashSet<string> variableSet = new HashSet<string>();

            foreach (string token in tokens)
            {
                if (variables.IsMatch(token))
                {
                    if (!variableSet.Contains(token)) { variableSet.Add(token); }
                }
            }
            return variableSet;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !typeof(Formula).IsInstanceOfType(obj)) { return false; }

            string objectExpression = obj.ToString();

            string[] expressionTokens = GetTokens(expression).ToArray();
            string[] objectTokens = GetTokens(objectExpression).ToArray();

            //After being split into tokens, if they differ in length, they can't be equal.
            if (expressionTokens.Length != objectTokens.Length) { return false; }

            for (int index = 0; index < expressionTokens.Length; index++)
            {
                string expToken = expressionTokens[index];
                string objToken = objectTokens[index];

                //Check to see if the expression token is a number.
                if (Double.TryParse(expToken, out double expressionNumber))
                {
                    //If it is, check to see if the passed object token is also a number.
                    if (Double.TryParse(objToken, out double objNumber))
                    {
                        //If it is, the ToString method will determine equality.
                        if (expressionNumber.ToString() != objNumber.ToString())
                        {
                            return false;
                        }
                    }
                    //If the passed object token isn't a number, the two objects are not equal.
                    else
                    {
                        return false;
                    }
                }
                //For all other inputs, each token should just be a string, and the string's Equals method can be used.
                else if (!expToken.Equals(objToken)) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1.Equals(f2)) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (f1.Equals(f2)) { return false; }
            else { return true; }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return expression.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
        /// <summary>
        /// To check if the given string is valid for a formula, it has to follow specific formatting rules.
        /// 
        /// To do this, this private helper method parses the string into specific tokens (see private method GetVariables). If
        /// the token array is empty, this method will throw a FormulaFormatException.
        /// 
        /// Otherwise, it utilizes two other private helper methods (CheckValidCharacters and CheckFormula) to ensure
        /// that the other formatting rules are being followed. Please refer to the documentation of those two
        /// aforementioned private helper methods if more information is required about the specific rules.
        /// </summary>
        /// <param name="formula">The formula that needs to be checked before a Formula object is created.</param>
        private void ParsingRules(string formula)
        {
            string[] tokens = GetTokens(formula).ToArray();
            if (tokens.Length == 0) { throw new FormulaFormatException("Formula is empty."); }

            foreach (string token in tokens)
            {
                CheckValidCharacters(token);
            }

            CheckFormula(tokens);
        }

        /// <summary>
        /// Checks the given string to see if it is in the proper format for a formula.
        /// 
        /// The first and last tokens of the given string are checked separately, via the private
        /// methods CheckFirstToken and CheckFinalToken. Each conditional statement throws
        /// a FormulaFormatException if the formatting rule isn't followed; refer to 
        /// the output message for the FormulaFormatException for more information.
        /// </summary>
        /// <param name="tokens">The token array that should have been established in the ParsingRules method.</param>
        private void CheckFormula(string[] tokens)
        {
            int leftParenthesisCount = CheckFirstToken(tokens);
            int rightParenthesisCount = 0;

            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            /// Since the first and last tokens are already checked, the index needs to be adjusted to account for it.
            for (int index = 1; index < tokens.Length - 1; index++)
            {
                string token = tokens[index];
                if (token.Equals("("))
                {
                    leftParenthesisCount += 1;
                    string nextToken = tokens[index + 1];

                    if (!numbers.IsMatch(nextToken) && !variables.IsMatch(nextToken) && !nextToken.Equals("("))
                    {
                        throw new FormulaFormatException("An opening parenthesis must be followed by a number, variable, or another opening parenthesis.");
                    }
                }
                else if (token.Equals("+") || token.Equals("-") || token.Equals("/") || token.Equals("*"))
                {
                    string nextToken = tokens[index + 1];

                    if (!numbers.IsMatch(nextToken) && !variables.IsMatch(nextToken) && !nextToken.Equals("("))
                    {
                        throw new FormulaFormatException("An operator must be followed by a number, variable, or another opening parenthesis.");
                    }
                }
                else if (numbers.IsMatch(token) || variables.IsMatch(token) || token.Equals(")"))
                {
                    if (token.Equals(")"))
                    {
                        rightParenthesisCount += 1;
                    }

                    string nextToken = tokens[index + 1];

                    if (CheckOperators(nextToken) && !nextToken.Equals(")"))
                    {
                        throw new FormulaFormatException("An operator or closing parenthesis must follow each number, variable, and closing parenthesis.");
                    }
                }
            }

            rightParenthesisCount += CheckFinalToken(tokens);

            if (leftParenthesisCount != rightParenthesisCount || rightParenthesisCount > leftParenthesisCount)
            {
                throw new FormulaFormatException("Formula is missing parentheses.");
            }
        }

        /// <summary>
        /// This private helper method checks the first token of the token array,
        /// to ensure it is following proper formatting rules.
        /// 
        /// If the rules are not properly followed, it will throw a FormulaFormatException.
        /// Please refer to the generated message for each FormulaFormatException for more
        /// information about what the specific rule is.
        /// </summary>
        /// <param name="tokens">The token array.</param>
        /// <returns>If the method found an opening parenthesis, it will return a count of 1.</returns>
        private int CheckFirstToken(string[] tokens)
        {
            string firstToken = tokens[0];
            int leftParenthesisCount = 0;

            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            if (numbers.IsMatch(firstToken) || variables.IsMatch(firstToken) || firstToken.Equals("("))
            {
                if (numbers.IsMatch(firstToken) || variables.IsMatch(firstToken))
                {
                    string secondToken = tokens[1];

                    if (CheckOperators(secondToken) && !secondToken.Equals(")"))
                    {
                        throw new FormulaFormatException("Any number or variable must be followed by an operator or closing parenthesis.");
                    }
                }
                else if (firstToken.Equals("(")){ leftParenthesisCount += 1; }
            }
            else { throw new FormulaFormatException("Formula must begin with a number, a variable, or an open parenthesis."); }

            return leftParenthesisCount;
        }

        /// <summary>
        /// This private helper method checks the last token of the array, to
        /// ensure it is following proper format rules.
        /// 
        /// If the rules are not properly followed, it will throw a FormulaFormatException.
        /// Please refer to the generated message for each FormulaFormatException for more
        /// information about what the specific rule is.
        /// </summary>
        /// <param name="tokens">The token array.</param>
        /// <returns>If the method found a closing parenthesis, it will return a count of 1.</returns>
        private int CheckFinalToken(string[] tokens)
        {
            string lastToken = tokens[tokens.Length - 1];
            int rightParenthesisCount = 0;

            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            if (numbers.IsMatch(lastToken) || variables.IsMatch(lastToken) || lastToken.Equals(")"))
            {
                if (lastToken.Equals(")")) { rightParenthesisCount += 1; }
            }
            else { throw new FormulaFormatException("Formula must end with a number, a variable, or a closing parenthesis."); }

            return rightParenthesisCount;
        }

        /// <summary>
        /// Private helper method to check for proper formatting of a given formula.
        /// 
        /// Valid tokens in a properly formatted formula are:
        /// (, ), +, -, /, *,
        /// 
        /// As well as numbers in integer or double format. Variables are also accepted, assuming they
        /// are stary with an upper or lower-case letter.
        /// </summary>
        /// <param name="token">The token (from the token array) that needs to be checked.</param>
        private void CheckValidCharacters(string token)
        {
            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            if (!numbers.IsMatch(token) && !variables.IsMatch(token) && CheckOperators(token) && !token.Equals("(") && !token.Equals(")"))
            {
                throw new FormulaFormatException("Invalid character found");
            }
        }

        /// <summary>
        /// This private helper method is used exclusively to condense the conditional statements
        /// found within the CheckFormula, CheckFirstToken, and CheckValidCharacters method.
        /// </summary>
        /// <param name="token">The current token (from the token array) being checked.</param>
        /// <returns>Returns true if the given token is NOT a +, -, /, or * (i.e an operator), returns false otherwise.</returns>
        private bool CheckOperators(string token)
        {
            if (!token.Equals("+") && !token.Equals("-") && !token.Equals("/") && !token.Equals("*")) { return true; }
            else { return false; }
        }

        /// <summary>
        /// This private helper method helps condense the Evaluate method by checking to see
        /// if there is a left parentheses (to remove) on the Operators Stack.
        /// </summary>
        /// <param name="operators">The Operators Stack.</param>
        private void CheckParentheses(Stack<string> operators)
        {
            if (operators.Count != 0 && operators.Peek() == "(") { operators.Pop(); }
        }

        /// <summary>
        /// This private helper method helps condense the Evaluate method by checking
        /// to see if there is a * (for multiplication) or / (for division) on the
        /// Operators Stack.
        /// </summary>
        /// <param name="values">The Values Stack</param>
        /// <param name="operators">The Operators Stack</param>
        /// <param name="number">The first number, which is given from the Values stack, the original formula, or from the Lookup delegate.</param>
        private void MultOrDivide(Stack<double> values, Stack<string> operators, double number)
        {
            if(operators.Peek() == "*")
            {
                double stackNumber = values.Pop();
                operators.Pop();
                double result = stackNumber * number;
                values.Push(result);
                CheckParentheses(operators);
            }
            else if(operators.Peek() == "/")
            {
                double stackNumber = values.Pop();
                operators.Pop();
                double result = stackNumber / number;
                values.Push(result);
                CheckParentheses(operators);
            }
        }

        /// <summary>
        /// This private helper method helps condense the Evaluate method by checking to see
        /// if there is a + (for addition) or a - (for subtraction) on the Operators Stack.
        /// </summary>
        /// <param name="values">The Values Stack.</param>
        /// <param name="operators">The Operators Stack.</param>
        private void AddOrSubtract(Stack<double> values, Stack<string> operators)
        {
            if(operators.Peek() == "+")
            {
                double firstStackNumber = values.Pop();
                double secondStackNumber = values.Pop();
                //Operator needs to be removed since it will be applied to the above stack numbers.
                operators.Pop();
                double result = firstStackNumber + secondStackNumber;
                values.Push(result);
            }
            else if(operators.Peek() == "-")
            {
                double firstStackNumber = values.Pop();
                double secondStackNumber = values.Pop();
                operators.Pop();
                double result = secondStackNumber - firstStackNumber;
                values.Push(result);
            }
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

