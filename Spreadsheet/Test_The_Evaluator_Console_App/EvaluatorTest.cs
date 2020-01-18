/// <summary> 
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
/// This is the class to test the Formula Evaluator class.
/// </summary>
using System;
using FormulaEvaluator;

namespace Test_The_Evaluator_Console_App
{
    class EvaluatorTest
    {
        /// <summary>
        /// This is the delgate that will look up the passed in variable.
        /// </summary>
        /// <param name="token">The String (variable) to be looked up.</param>
        /// <returns>An int. The value that is associated with the given variable.</returns>
        public static int variableLookup(string token)
        {
            if (token == "x1") return 5;
            if (token == "x2") return 10;
            else throw new ArgumentException();
        }
        
        /// <summary>
        /// Main function that is used for testing purposes.
        /// </summary>
        /// <param name="args">To be ignored.</param>
        static void Main(string[] args)
        {
            //Testing simple multiplication.
            Console.WriteLine($"1*2 = {Evaluator.Evaluate("1*2", null)}");

            //Testing simple addition.
            Console.WriteLine($"1+1 = {Evaluator.Evaluate("1+1", null)}");

            //Testing simple division.
            Console.WriteLine($"18/6 = {Evaluator.Evaluate("18/6", null)}");

            //Testing simple subtraction.
            Console.WriteLine($"5-1 = {Evaluator.Evaluate("5-1", null)}");

            //Testing simple parentheses usage.
            Console.WriteLine($"(3+5) = {Evaluator.Evaluate("(3+5)", null)}");

            //Testing multiplication, parentheses, and addition.
            Console.Write($"2 + 5*(2 + 3) = {Evaluator.Evaluate("2 + 5*(2 + 3)", null)}");
            if (Evaluator.Evaluate("2 + 5*(2 + 3)", null) == 27) Console.WriteLine(" Success");
            else Console.WriteLine("Failed");

            //Testing all operators with multiple parentheses.
            Console.Write($"(30*2) - (4*(4+1))/2*2 = {Evaluator.Evaluate("(30*2) - (4*(4+1))/2*2", null)}");
            if (Evaluator.Evaluate("(30*2) - (4*(4+1))/2*2", null) == 40) Console.WriteLine(" Success");
            else Console.WriteLine(" Failed");

            //Testing order of operations.
            Console.Write($"(2 * 3) / 2 * 10 + (10 - 1) = {Evaluator.Evaluate("(2 * 3) / 2 * 10 + (10 - 1)", null)}");
            if (Evaluator.Evaluate("(2 * 3) / 2 * 10 + (10 - 1)", null) == 39) Console.WriteLine(" Success");
            else Console.WriteLine(" Failed");

            //Testing simple expression with delegate usage.
            Console.WriteLine($"x1+x2 = {Evaluator.Evaluate("x1+x2", variableLookup)}");

            //Testing simple expression with lambda usage.
            Console.WriteLine($"a7+a7 = {Evaluator.Evaluate("a7+a7", (a) => 10)}");

            //Testing dividing by zero.
            try
            {
                Console.WriteLine($"4/0 = {Evaluator.Evaluate("4/0", null)}");
            } catch (DivideByZeroException)
            {
                Console.WriteLine("Cannot divide by zero.");
            }

            //Testing a variable that isn't defined.
            try
            {
                Console.WriteLine($"X222+5 = {Evaluator.Evaluate("X222+5", variableLookup)}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Variable not found.");
            }

            //Testing an incomplete expression.
            try
            {
                Console.WriteLine($"4+ = {Evaluator.Evaluate("4+", null)}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid expression.");
            }

            //Testing the input of an invalid character.
            try
            {
                Console.WriteLine($"-A- = {Evaluator.Evaluate("-A-", null)}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Variable is not in the proper format.");
            }

            //Testing improper parentheses usage.
            try
            {
                Console.WriteLine($"3+3) = {Evaluator.Evaluate("3+3)", null)}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Expression is not in the proper format.");
            }

            //Testing negative numbers.
            try
            {
                Console.WriteLine($"-4 = {Evaluator.Evaluate("-4", null)}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Negative numbers are not supported.");
            }
        }
    }
}
