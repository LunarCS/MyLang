using LLang;
using LLang.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    internal class Expressions
    {
        public static IRuntimeValue EvalProgram(ProgramNode program, Environment environment)
        {
            IRuntimeValue lastEval = null;

            foreach (IStatement statement in program.body)
            {
                lastEval = Interpreter.Eval(statement, environment);
            }

            return lastEval;
        }

        public static IRuntimeValue EvalBinExpr(BinaryExpression binexpr, Environment environment)
        {
            IRuntimeValue leftHandSide = Interpreter.Eval(binexpr.Left, environment);
            IRuntimeValue rightHandSide = Interpreter.Eval(binexpr.Right, environment);


            if (leftHandSide.Type == ValueTypes.Int && rightHandSide.Type == ValueTypes.Int)
            {
                return EvalIntBinExpr((IntValue)rightHandSide, (IntValue)leftHandSide, binexpr.Operator);
            }

            if (leftHandSide.Type == ValueTypes.Float && rightHandSide.Type == ValueTypes.Float)
            {
                return EvalFloatBinExpr((FloatValue)rightHandSide, (FloatValue)leftHandSide, binexpr.Operator);
            }

            if (leftHandSide.Type == ValueTypes.String && rightHandSide.Type == ValueTypes.String)
            {
                return EvalStrBinExpr((StringValue)rightHandSide, (StringValue)leftHandSide, binexpr.Operator);
            }



            return new NullableValue();
        }

        public static IRuntimeValue EvalIntBinExpr(IntValue rightHandSide, IntValue leftHandSide, string op)
        {
            int result;

            switch (op)
            {
                case "+":
                    result = leftHandSide.Value + rightHandSide.Value;
                    break;
                case "-":
                    result = leftHandSide.Value - rightHandSide.Value;
                    break;
                case "*":
                    result = leftHandSide.Value * rightHandSide.Value;
                    break;
                case "/":
                    if (rightHandSide.Type == 0)
                    {
                        Console.Error.WriteLine("Can not divide by zero!");
                        throw new DivideByZeroException("Math error, tried dividing by zero ");
                    }
                    result = leftHandSide.Value / rightHandSide.Value;
                    break;
                case "%":
                    result = leftHandSide.Value % rightHandSide.Value;
                    break;
                default:
                    Console.Error.WriteLine($"Invalid operator! {op}");

                    throw new NotImplementedException("Invalid operator");
            }
            return new IntValue(result);
        }

        public static FloatValue EvalFloatBinExpr(FloatValue rightHandSide, FloatValue leftHandSide, string op)
        {
            float result;

            switch (op)
            {
                case "+":
                    result = leftHandSide.Value + rightHandSide.Value;
                    break;
                case "-":
                    result = leftHandSide.Value - rightHandSide.Value;
                    break;
                case "*":
                    result = leftHandSide.Value * rightHandSide.Value;
                    break;
                case "/":
                    if (rightHandSide.Type == 0)
                    {
                        Console.Error.WriteLine("Can not divide by zero!");
                        throw new DivideByZeroException("Math error, tried dividing by zero ");
                    }
                    result = leftHandSide.Value / rightHandSide.Value;
                    break;
                case "%":
                    result = leftHandSide.Value % rightHandSide.Value;
                    break;
                default:
                    Console.Error.WriteLine($"Invalid operator! {op}");

                    throw new NotImplementedException("Invalid operator");
            }
            return new FloatValue(result);
        }

        public static StringValue EvalStrBinExpr(StringValue rightHandSide, StringValue leftHandSide, string op)
        {
            string result;

            switch (op)
            {
                case "+":
                    result = leftHandSide.Value + rightHandSide.Value;
                    break;
                default:
                    Console.Error.WriteLine($"Invalid operator! {op}");

                    throw new NotImplementedException("Invalid operator");
            }
            return new StringValue(result);
        }
        public static IRuntimeValue EvalIdentifier(Identifier identifier, Environment environment)
        {
            IRuntimeValue val = environment.LookupVariable(identifier.Symbol);
            return val;
        }

        public static IRuntimeValue EvalVarDeclaration(VarDeclaration varDeclaration, Environment environment)
        {
            environment.DeclareVar(varDeclaration.Datatype.DataTypeProperty, varDeclaration.Identifier.Symbol, varDeclaration.AccessModifier.Value);
            IRuntimeValue value = Interpreter.Eval(varDeclaration.Value, environment);
            return environment.AssignVariable(varDeclaration.Identifier.Symbol, Interpreter.Eval(varDeclaration.Value, environment));
        }
        public static IRuntimeValue EvalClassDeclaration(ClassDeclaration classDeclaration, Environment environment)
        {
            environment.CreateClass(classDeclaration.AccessModifier.Value, classDeclaration.Identifier.Symbol, classDeclaration.properties);

            return new StringValue($"Class of type {classDeclaration.Identifier.Symbol}");
        }

        public static IRuntimeValue EvalAssignment(AssignmentExpression assignment, Environment environment)
        {
            if (assignment.Assignment.Type != NodeType.Identifier) 
            {
                Console.WriteLine($"Cannot assign as you didnt specify a variable to assign to!");
                throw new ArgumentException();
            }

            Identifier var = (Identifier)assignment.Assignment;

            return environment.AssignVariable(var.Symbol, Interpreter.Eval(assignment.Value, environment));
        }



    }
}
