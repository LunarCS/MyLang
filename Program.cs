using LLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    internal class Program
    {

        // Classes in order:
        // Lexer -> Parser -> ASTclass -> Interpreter

        static void Main(string[] args)
        {
            REPL();
        }

        static async void REPL()
        {
            Parser parser = new Parser();
            Environment environment = new Environment();
            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine();
                if (input == null || input.Contains("exit"))
                    return;
                ProgramNode program = parser.ProduceAST(input);
                IRuntimeValue result = Interpreter.Eval(program, environment);
                result.DisplayResult();
            }
        }
    }
}
