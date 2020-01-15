using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public class Evaluator
    {
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();

            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for(int index = 0; index < substrings.Length; index++)
            {
                String foundCharacter = substrings[index];

                //If the String is a number value:
                if(int.TryParse(foundCharacter, out int number))
                {
                    if(operators.Count != 0 && operators.Peek() == "*")
                    {
                        int stackNumber = int.Parse(values.Pop());
                        //Storing the operator into a value isn't necessary, but it still needs to be removed from the Stack.
                        operators.Pop();
                        int result = stackNumber * number;
                        values.Push(result.ToString());
                    }
                    else if(operators.Count != 0 && operators.Peek() == "/")
                    {
                        int stackNumber = int.Parse(values.Pop());
                        operators.Pop();
                        int result = stackNumber / number;
                        values.Push(result.ToString());
                    }
                    else
                    {
                        values.Push(foundCharacter);
                    }
                }
                //Otherwise it is an operator, parentheses, or variable:
                else
                {
                    if(foundCharacter == "+" || foundCharacter == "-")
                    {
                        if(operators.Count != 0 && operators.Peek() == "+")
                        {
                            int firstStackNumber = int.Parse(values.Pop());
                            int secondStackNumber = int.Parse(values.Pop());
                            operators.Pop();
                            int result = firstStackNumber + secondStackNumber;
                            values.Push(result.ToString());
                            operators.Push(foundCharacter);
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {
                            int firstStackNumber = int.Parse(values.Pop());
                            int secondStackNumber = int.Parse(values.Pop());
                            operators.Pop();
                            int result = secondStackNumber - firstStackNumber;
                            values.Push(result.ToString());
                            operators.Push(foundCharacter);
                        }
                        else
                        {
                            operators.Push(foundCharacter);
                        }
                    }
                    else if(foundCharacter == "*" || foundCharacter == "/" || foundCharacter == "(")
                    {
                        operators.Push(foundCharacter);
                    }
                    else if(foundCharacter == ")")
                    {

                    }
                }

                
            }
            return int.Parse(values.Pop());
        }
    }
}
