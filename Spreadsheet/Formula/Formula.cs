// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FormulaEvaluator;

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
        /// The associated normalizer is the identity function, and the associated validator
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
            formula = normalize(formula);

            if (isValid(formula))
            {
                expression = formula;
            }
            else
            {
                throw new FormulaFormatException("Variable is not in the proper format.");
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
                        ProcessNumber(number, operators, values);
                    }

                    else if (token == "+" || token == "-")
                    {
                        if (operators.Count != 0 && operators.Peek() == "+")
                        {
                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            //Operator needs to be removed since it will be applied to the above stack numbers.
                            operators.Pop();
                            double result = firstStackNumber + secondStackNumber;
                            values.Push(result);
                            operators.Push(token);
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {
                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            operators.Pop();
                            double result = secondStackNumber - firstStackNumber;
                            values.Push(result);
                            operators.Push(token);
                        }
                        else
                        {
                            operators.Push(token);
                        }
                    }

                    else if (token == "*" || token == "/" || token == "(")
                    {
                        operators.Push(token);
                    }

                    else if (token == ")")
                    {
                        if (operators.Count != 0 && operators.Peek() == "+")
                        {
                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            operators.Pop();
                            double result = firstStackNumber + secondStackNumber;
                            values.Push(result);
                            operators.Pop(); // Assumes the first parenthesis ( is on the top of the Stack.
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {

                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            operators.Pop();
                            double result = secondStackNumber - firstStackNumber;
                            values.Push(result);
                            operators.Pop(); // Assumes the first parenthesis ( is on the top of the Stack.
                        }

                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            operators.Pop();
                            double result = firstStackNumber * secondStackNumber;
                            values.Push(result);
                            operators.Pop();
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            double firstStackNumber = values.Pop();
                            double secondStackNumber = values.Pop();
                            operators.Pop();
                            double result = secondStackNumber / firstStackNumber;
                            values.Push(result);
                            operators.Pop();
                        }
                    }
                    //If the token is anything else, it should be a variable that needs to be looked up via the delegate.
                    else if (reg.Match(token).Success)
                    {
                        double variableValue = lookup(token);
                        ProcessNumber(variableValue, operators, values);
                    }
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

            foreach(string token in tokens)
            {
                if (variables.IsMatch(token))
                {
                    variableSet.Add(token);
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
            expression = Regex.Replace(expression, @"\s", "");
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
            if(obj == null || !typeof(Formula).IsInstanceOfType(obj)) { return false; }

            string objectExpression = obj.ToString();

            string[] expressionTokens = GetTokens(expression).ToArray();
            string[] objectTokens = GetTokens(objectExpression).ToArray();

            //After being split into tokens, if they differ in length, they can't be equal.
            if(expressionTokens.Length != objectTokens.Length) { return false; }

            for (int index = 0; index < expressionTokens.Length; index++)
            {
                string expToken = expressionTokens[index];
                string objToken = objectTokens[index];

                //Check to see if the expression token is a number.
                if (Double.TryParse(expToken, out double expressionNumber)){
                    //If it is, check to see if the passed object token is also a number.
                    if (Double.TryParse(objToken, out double objNumber)) {
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
                //For all other inputs, each token should just be a string, and the string's Equals function can be used.
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
            if(f1 == null && f2 == null){ return true;}

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
            if(f1 == null && f2 == null) { return false; }
        
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

        private void ParsingRules(string formula)
        {
            string[] tokens = GetTokens(formula).ToArray();
            if (tokens.Length == 0) { throw new FormulaFormatException("Formula is empty.");}

            foreach(string token in tokens)
            {
                CheckValidCharacters(token);
            }

            CheckFormula(tokens);
        }

        private void CheckFormula(string[] tokens)
        {
            int leftParenthesisCount = CheckFirstToken(tokens);
            int rightParenthesisCount = 0;

            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

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
                else if (firstToken.Equals("("))
                {
                    leftParenthesisCount += 1;
                }
            }
            else
            {
                throw new FormulaFormatException("Formula must begin with a number, a variable, or an open parenthesis.");
            }
           
            return leftParenthesisCount;
        }

        private int CheckFinalToken (string[] tokens)
        {
            string lastToken = tokens[tokens.Length - 1];
            int rightParenthesisCount = 0;

            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            if (numbers.IsMatch(lastToken) || variables.IsMatch(lastToken) || lastToken.Equals(")"))
            {
                if (lastToken.Equals(")"))
                {
                    rightParenthesisCount += 1;
                }
            }
            else
            {
                throw new FormulaFormatException("Formula must end with a number, a variable, or a closing parenthesis.");
            }
            
            return rightParenthesisCount;
        }

        private void CheckValidCharacters(string token)
        {
            Regex numbers = new Regex("^[0-9]+");
            Regex variables = new Regex("^[a-zA-Z]+");

            if (!numbers.IsMatch(token) && !variables.IsMatch(token) && CheckOperators(token) && !token.Equals("(") && !token.Equals(")"))
            {
                throw new FormulaFormatException("Invalid character found");
            }
        }

        private bool CheckOperators(string token)
        {
            if(!token.Equals("+") && !token.Equals("-") && !token.Equals("/") && !token.Equals("*"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Private helper method to process the numbers found within the String array, to help
        /// determine their usage with the two Stacks; if the Operator Stack has a * or / on top,
        /// then the number gets processed immediately, and the new value gets pushed onto the Value Stack.
        /// Otherwise, the number is immediately put on the Value Stack.
        /// 
        /// Edge cases to be aware of: Each operation assumes that there are at least 2 numbers on the
        /// Value Stack.
        /// </summary>
        /// <param name="number">The number to be processed.</param>
        /// <param name="operators">The name of the Stack that holds the operators.</param>
        /// <param name="values">The name of the Stack that holds the numbers.</param>
        private static void ProcessNumber(double number, Stack<string> operators, Stack<double> values)
        {
            if (operators.Count != 0 && operators.Peek() == "*")
            {
                double stackNumber = values.Pop();
                operators.Pop();
                double result = stackNumber * number;
                values.Push(result);
                if (operators.Count != 0 && operators.Peek() == "(")
                {
                    operators.Pop();
                }
            }
            else if (operators.Count != 0 && operators.Peek() == "/")
            {
                if (number == 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    double stackNumber = values.Pop();
                    operators.Pop();
                    double result = stackNumber / number;
                    values.Push(result);
                }

                if (operators.Count != 0 && operators.Peek() == "(")
                {
                    operators.Pop();
                }
            }
            else
            {
                values.Push(number);
            }
        }

        private double ProcessOperator(Stack<double> values, Stack<string> operators)
        {

            if(operators.Peek() == "+")
            {
                double firstStackNumber = values.Pop();
                double secondStackNumber = values.Pop();
                //Operator needs to be removed since it will be applied to the above stack numbers.
                operators.Pop();
                double result = firstStackNumber + secondStackNumber;
                return result;
            }
            return 0;
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

