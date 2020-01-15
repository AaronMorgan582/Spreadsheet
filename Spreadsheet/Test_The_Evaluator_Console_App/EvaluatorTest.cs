using System;

namespace Test_The_Evaluator_Console_App
{
    class EvaluatorTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"1*2 = {FormulaEvaluator.Evaluator.Evaluate("1*2", null)}");
        }
    }
}
