using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for (int index = 0; index < substrings.Length; index++)
            {
                String token = substrings[index];

                //If the String is a number value:
                if (int.TryParse(token, out int number))
                {
                    if (operators.Count != 0 && operators.Peek() == "*")
                    {
                        int stackNumber = int.Parse(values.Pop());
                        //Storing the operator into a variable isn't necessary, but it still needs to be removed from the Stack.
                        operators.Pop();
                        int result = stackNumber * number;
                        values.Push(result.ToString());
                    }
                    else if (operators.Count != 0 && operators.Peek() == "/")
                    {
                        int stackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = stackNumber / number;
                        values.Push(result.ToString());
                    }
                    else
                    {
                        values.Push(token);
                    }
                }
                else if (token == "+" || token == "-")
                {
                    if (operators.Count != 0 && operators.Peek() == "+")
                    {
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = firstStackNumber + secondStackNumber;
                        values.Push(result.ToString());
                        operators.Push(token);
                    }
                    else if (operators.Count != 0 && operators.Peek() == "-")
                    {
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = secondStackNumber - firstStackNumber;
                        values.Push(result.ToString());
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
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = firstStackNumber + secondStackNumber;
                        values.Push(result.ToString());
                        operators.Pop(); // Assumes the first parenthesis ( is on the top of the Stack.
                    }
                    else if (operators.Count != 0 && operators.Peek() == "-")
                    {
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = secondStackNumber - firstStackNumber;
                        values.Push(result.ToString());
                        operators.Pop(); //Assumes the first parenthesis ( is on the top of the Stack.
                    }
                    if (operators.Count != 0 && operators.Peek() == "*")
                    {
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = firstStackNumber * secondStackNumber;
                        values.Push(result.ToString());
                    }
                    else if (operators.Count != 0 && operators.Peek() == "/")
                    {
                        int firstStackNumber = int.Parse(values.Pop());
                        int secondStackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = secondStackNumber / firstStackNumber;
                        values.Push(result.ToString());
                    }
                }
            }

            if (values.Count > 1)
            {
                int finalResult = 0;
                for (int i = 0; i < operators.Count; i++)
                {
                    int firstNumber = int.Parse(values.Pop());
                    if (operators.Peek() == "+")
                    {
                        int secondNumber = int.Parse(values.Pop());
                        int result = secondNumber + firstNumber;
                        finalResult += result;
                    }
                    else if (operators.Peek() == "-")
                    {
                        int secondNumber = int.Parse(values.Pop());
                        int result = secondNumber - firstNumber;
                        finalResult += result;
                    }
                    else if (operators.Peek() == "*")
                    {
                        int secondNumber = int.Parse(values.Pop());
                        int result = secondNumber * firstNumber;
                        finalResult += result;
                    }
                    else if (operators.Peek() == "/")
                    {
                        int secondNumber = int.Parse(values.Pop());
                        int result = secondNumber / firstNumber;
                        finalResult += result;
                    }
                }
                return finalResult;
            }
            else
            {
                return int.Parse(values.Pop());
            }
        }
    }
}

            
