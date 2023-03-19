using LLang;
using LLang.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    internal class Interpreter
    {
        public static IRuntimeValue Eval(IStatement astNode, Environment environment)
        {
            switch (astNode.Type)
            {
                case NodeType.Object:
                    ObjectLiteral objectLiteral = (ObjectLiteral)astNode;
                    return new ObjectValue(objectLiteral);
                case NodeType.Identifier:
                    Identifier identifier = (Identifier)astNode;
                    return Expressions.EvalIdentifier(identifier, environment);

                case NodeType.IntLiteral:
                    IntegerLiteral integerLiteral = (IntegerLiteral)astNode;
                    return new IntValue(integerLiteral.Value);

                case NodeType.FloatLiteral:
                    FloatLiteral floatLiteral = (FloatLiteral)astNode;
                    return new FloatValue(floatLiteral.Value);

                case NodeType.StringLiteral:
                    StringLiteral stringLiteral = (StringLiteral)astNode;
                    return new StringValue(stringLiteral.Value);

                case NodeType.NullLiteral:
                    return new NullableValue();

                case NodeType.BinaryExpression:
                    BinaryExpression binaryExpression = (BinaryExpression)astNode;
                    return Expressions.EvalBinExpr(binaryExpression, environment);

                case NodeType.Program:
                    ProgramNode program = (ProgramNode)astNode;
                    return Expressions.EvalProgram(program, environment);

                case NodeType.VarDeclaration:
                    VarDeclaration varDeclaration = (VarDeclaration)astNode;
                    return Expressions.EvalVarDeclaration(varDeclaration, environment);

                case NodeType.ClassDeclaration:
                    ClassDeclaration classDeclaration = (ClassDeclaration)astNode;
                    return Expressions.EvalClassDeclaration(classDeclaration, environment);
                case NodeType.AssignmentExpression:
                    AssignmentExpression assignmentExpression = (AssignmentExpression)astNode;
                    return Expressions.EvalAssignment(assignmentExpression, environment);
                default:
                    Console.WriteLine($"AST node has not been setup yet for interpretation. {astNode}");
                    throw new NotImplementedException();

            }
        }
    }
}
