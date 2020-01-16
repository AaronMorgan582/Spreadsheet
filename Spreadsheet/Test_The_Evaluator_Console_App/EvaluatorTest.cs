using System;

namespace Test_The_Evaluator_Console_App
{
    class EvaluatorTest
    {
        static void Main(string[] args)
        {
            //Testing simple multiplication.
            Console.WriteLine($"1*2 = {FormulaEvaluator.Evaluator.Evaluate("1*2", null)}");

            //Testing simple addition.
            Console.WriteLine($"1+1 = {FormulaEvaluator.Evaluator.Evaluate("1+1", null)}");

            //Testing simple division.
            Console.WriteLine($"18/6 = {FormulaEvaluator.Evaluator.Evaluate("18/6", null)}");

            //Testing simple subtraction.
            Console.WriteLine($"5-1 = {FormulaEvaluator.Evaluator.Evaluate("5-1", null)}");

            //Testing simple parentheses usage.
            Console.WriteLine($"(3+5) = {FormulaEvaluator.Evaluator.Evaluate("(3+5)", null)}");
        }
    }
}
