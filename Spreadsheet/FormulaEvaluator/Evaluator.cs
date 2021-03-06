﻿/// <summary> 
/// Author:    Aaron Morgan
/// Partner:   None
/// Date:      1/10/2020
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Aaron Morgan
/// 
/// I, Aaron Morgan, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
/// This is a library to evaluate certain expressions, with a certain format. Valid mathematical operators are +, -, *, /, (, ), as well 
/// as non-negative integers. Variables are accepted, but need to be in the format of letters followed by numbers, such as
/// x1, xY123, ABC55, and so on.
/// </summary>
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(string variable_name);

        /// <summary>
        /// This function calculates a given expression using infix notation. You should be aware of the following edge cases:
        /// - Multiplication by parentheses, such as 4(5) is not supported.
        /// - Improperly formatted equations, such as 4+ or 3-3).
        /// - Negative numbers are not supported.
        /// - Non-integers are not supported.
        /// </summary>
        /// <param name="expression">expression represents the input expression that will be calculated.</param> 
        /// <param name="variableEvaluator">variableEvaluator represents the delegate that determines the values for variables.</param>
        /// <returns>An int that represents the result of the calculation.</returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int> values = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            //Regex expression to capture any string that starts with upper/lower case letters, followed by any length of numbers 0-9.
            Regex reg = new Regex("^[a-zA-Z]+[0-9]+");

            for (int index = 0; index < substrings.Length; index++)
            {
                string token = substrings[index];

                //Ignores white spaces and empty characters found within the substring array.
                if (token != "" && token != " ")
                {
                    //If the token is a number value:
                    if (int.TryParse(token, out int number))
                    {
                        ProcessNumber(number, operators, values);
                    }

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

                    else if (token == "*" || token == "/" || token == "(")
                    {
                        operators.Push(token);
                    }

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
                        //Special case for multiple parentheses, such as (5*(3+2)). If a right parentheses is processed, but the top of the stack
                        //is a left parentheses, the left parentheses needs to be removed, because it could be preventing other operators from being
                        //processed.
                        else if (operators.Count != 0 && operators.Peek() == "(")
                        {
                            throw new ArgumentException(); ;
                        }

                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            int firstStackNumber = values.Pop();
                            MultOrDivide(values, operators, firstStackNumber);
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            int firstStackNumber = values.Pop();
                            MultOrDivide(values, operators, firstStackNumber);
                        }
                    }
                    //If the token is anything else, it should be a variable that needs to be looked up via the delegate.
                    else if (reg.Match(token).Success)
                    {
                        int variableValue = variableEvaluator(token);
                        ProcessNumber(variableValue, operators, values);
                    }
                    else
                    {
                        throw new ArgumentException();
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

            //To indicate improper formats: If the Operator Stack is not empty, and there is exactly one Value left; if Values is empty,
            //but there are Operators left; if both Stacks are empty, then an empty space was passed in originally.
            if (values.Count == 1 && operators.Count != 0 || values.Count == 0 && operators.Count != 0 || values.Count == 0 && operators.Count == 0)
            {
                throw new ArgumentException();
            }
            else
            {
                return values.Pop();
            }
        }

        private static void MultOrDivide(Stack<int> values, Stack<string> operators, int number)
        {
            if (operators.Peek() == "*")
            {
                int stackNumber = values.Pop();
                operators.Pop();
                int result = stackNumber * number;
                values.Push(result);
            }
            else if (operators.Peek() == "/")
            {
                int stackNumber = values.Pop();
                operators.Pop();
                int result = stackNumber / number;
                values.Push(result);
            }
        }

        /// <summary>
        /// This private helper method helps condense the Evaluate method by checking to see
        /// if there is a + (for addition) or a - (for subtraction) on the Operators Stack.
        /// </summary>
        /// <param name="values">The Values Stack.</param>
        /// <param name="operators">The Operators Stack.</param>
        private static void AddOrSubtract(Stack<int> values, Stack<string> operators)
        {
            if (operators.Peek() == "+")
            {
                int firstStackNumber = values.Pop();
                int secondStackNumber = values.Pop();
                //Operator needs to be removed since it will be applied to the above stack numbers.
                operators.Pop();
                int result = firstStackNumber + secondStackNumber;
                values.Push(result);
            }
            else if (operators.Peek() == "-")
            {
                int firstStackNumber = values.Pop();
                int secondStackNumber = values.Pop();
                operators.Pop();
                int result = secondStackNumber - firstStackNumber;
                values.Push(result);
            }
        }

        /// <summary>
        /// This private helper method helps condense the Evaluate method by checking to see
        /// if there is a left parentheses (to remove) on the Operators Stack.
        /// </summary>
        /// <param name="operators">The Operators Stack.</param>
        private static void CheckParentheses(Stack<string> operators)
        {
            if (operators.Count != 0 && operators.Peek() == "(") { operators.Pop(); }
            else throw new ArgumentException();
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
        private static void ProcessNumber(int number, Stack<string> operators, Stack<int> values)
        {
            if (operators.Count != 0 && operators.Peek() == "*")
            {
                int stackNumber = values.Pop();
                operators.Pop();
                int result = stackNumber * number;
                values.Push(result);
                if (operators.Count != 0 && operators.Peek() == "(")
                {
                    operators.Pop();
                }
            }
            else if (operators.Count != 0 && operators.Peek() == "/")
            {
                if(number == 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    int stackNumber = values.Pop();
                    operators.Pop();
                    int result = stackNumber / number;
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
    }
}


