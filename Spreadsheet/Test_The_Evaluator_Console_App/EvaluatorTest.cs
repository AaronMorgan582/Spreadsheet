using System;
using FormulaEvaluator;

namespace Test_The_Evaluator_Console_App
{
    class EvaluatorTest
    {
        public static int variableLookup(string token)
        {
            if (token == "x1") return 5;
            if (token == "x2") return 10;
            else throw new ArgumentException();
        }
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

            //Testing all operators with multiple parentheses, and with multiple white spaces.
            Console.Write($"(2 * 3) / 2 * 10 + (10 - 1) = {Evaluator.Evaluate("(2 * 3) / 2 * 10 + (10 - 1)", null)}");
            if (Evaluator.Evaluate("(2 * 3) / 2 * 10 + (10 - 1)", null) == 39) Console.WriteLine(" Success");
            else Console.WriteLine(" Failed");


            //Testing simple expression with delegate usage.
            Console.WriteLine($"x1+x2 = {Evaluator.Evaluate("x1+x2", variableLookup)}");

            //Testing simple expression with lambda usage.
            Console.WriteLine($"a7+a7 = {Evaluator.Evaluate("a7+a7", (a) => 10)}");
        }
    }
}
