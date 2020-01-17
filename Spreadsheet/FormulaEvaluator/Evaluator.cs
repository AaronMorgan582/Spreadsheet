using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);
        /// <summary>
        /// Calculates a given expression. Valid mathematical operators are +, -, *, /, (, ), as well as non-negative
        /// integers. Variables are accepted, but need to be in the format of letters followed by numbers, such as
        /// x1, xy123, ABC55, and so on.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int> values = new Stack<int>();
            Stack<String> operators = new Stack<String>();

            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for (int index = 0; index < substrings.Length; index++)
            {
                String token = substrings[index];
                
                //Ignores white spaces and empty characters found within the substring array.
                if (token != "" && token != " ")
                {
                    //If the token is a number value:
                    if (int.TryParse(token, out int number))
                    {
                        processNumber(number, operators, values);
                    }

                    else if (token == "+" || token == "-")
                    {
                        if (operators.Count != 0 && operators.Peek() == "+")
                        {
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            //Operator needs to be removed since it will be applied to the above stack numbers.
                            operators.Pop();
                            int result = firstStackNumber + secondStackNumber;
                            values.Push(result);
                            operators.Push(token);
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            operators.Pop();
                            int result = secondStackNumber - firstStackNumber;
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
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            operators.Pop();
                            int result = firstStackNumber + secondStackNumber;
                            values.Push(result);
                            operators.Pop(); // Assumes the first parenthesis ( is on the top of the Stack.
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            operators.Pop();
                            int result = secondStackNumber - firstStackNumber;
                            values.Push(result);
                            operators.Pop(); //Assumes the first parenthesis ( is on the top of the Stack.
                        }
                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            operators.Pop();
                            int result = firstStackNumber * secondStackNumber;
                            values.Push(result);
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            int firstStackNumber = values.Pop();
                            int secondStackNumber = values.Pop();
                            operators.Pop();
                            int result = secondStackNumber / firstStackNumber;
                            values.Push(result);
                        }
                        //Special case for multiple parentheses, such as (5*(3+2)). If a right parentheses is processed, but the top of the stack
                        //is a left parentheses, the left parentheses needs to be removed, because it could be preventing other operators from being
                        //processed.
                        else if (operators.Count != 0 && operators.Peek() == "(")
                        {
                            operators.Pop();
                        }
                    }
                    //If the token is anything else, it's a variable that needs to be looked up via the delegate, or it is an invalid string.
                    else
                    {
                        token = variableEvaluator(token).ToString();
                        if (int.TryParse(token, out int variableNumber))
                        {
                            processNumber(variableNumber, operators, values);
                        }
                    }
                }
            }
            //If the Value Stack has more than 1 number in it, then the calculation is not finished.
            if (values.Count > 1)
            {
                int finalResult = 0;

                //It needs to loop over the Operator Stack, because there should be more values than operators. If you try to
                //loop over the Value Stack, it will error because there won't be enough operators to process.
                for (int i = 0; i < operators.Count; i++)
                {
                    int firstStackNumber = values.Pop();
                    if (operators.Peek() == "+")
                    {
                        int secondStackNumber = values.Pop();
                        int result = secondStackNumber + firstStackNumber;
                        operators.Pop();
                        finalResult += result;
                    }
                    else if (operators.Peek() == "-")
                    {
                        int secondStackNumber = values.Pop();
                        int result = secondStackNumber - firstStackNumber;
                        operators.Pop();
                        finalResult += result;
                    }
                }
                return finalResult;
            }
            else
            {
                return values.Pop();
            }
        }

        /// <summary>
        /// Private helper method to process the numbers found within the string array, to help
        /// determine their usage with the two Stacks; if the Operator Stack has a * or / on top,
        /// then the number gets processed immediately, and the new value gets pushed onto the Value Stack.
        /// Otherwise, the number is immediately put on the Value Stack.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="operators"></param>
        /// <param name="values"></param>
        private static void processNumber(int number, Stack<String> operators, Stack<int> values)
        {
            if (operators.Count != 0 && operators.Peek() == "*")
            {
                int stackNumber = values.Pop();
                operators.Pop();
                int result = stackNumber * number;
                values.Push(result);
            }
            else if (operators.Count != 0 && operators.Peek() == "/")
            {
                int stackNumber = values.Pop();
                operators.Pop();
                int result = stackNumber / number;
                values.Push(result);
            }
            else
            {
                values.Push(number);
            }
        }
    }
}

            
