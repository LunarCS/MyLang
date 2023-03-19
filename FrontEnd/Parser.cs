using LLang;
using LLang.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    internal class Parser
    {
        private List<Token> tokens;
        private Dictionary<string, ClassDeclaration> classes = new Dictionary<string, ClassDeclaration>();
        private Dictionary<string, VarDeclaration> objects = new Dictionary<string, VarDeclaration>();
        public ProgramNode ProduceAST(string sourceCode)
        {
            tokens = Lexer.Tokenize(sourceCode);
            List<IStatement> body = new List<IStatement>();
            ProgramNode program = new ProgramNode(body);

            while (NotEof())
            {
                program.body.Add(ParseStatement());
            }

            return program;
        }
        private Token Shift()
        {
            Token prev = tokens[0];
            tokens.RemoveAt(0);
            return prev;
        }

        private Token At()
        {
            return tokens[0];
        }

        private Token Advance()
        {
            Token prev = Shift();
            return prev;
        }

        private Token Expect(TokenType expected, string errorMessage) 
        {
            Token prev = Shift();
            if(prev == null || prev.Type != expected) 
            {
                Console.Error.WriteLine($"Parser Error: {errorMessage} \n {prev}, Expected: {expected} ");
                throw new Exception();
            }
            return prev;
        }

        private IStatement ParseStatement()
        {
            switch (At().Type)
            {
                case TokenType.AccessModifier:
                    return HandleClassOrVarDeclare();
                case TokenType.Class:
                    return ParseClassDeclaration();
                case TokenType.Datatype:
                case TokenType.UserDefined:
                    return ParseVarDeclaration();
                case TokenType.EOF:
                    return null;
                default:
                    return ParseExpression();

            }
        }

        private IStatement HandleClassOrVarDeclare()
        {
            if (tokens[1].Type == TokenType.Class)
                return ParseClassDeclaration();
            else if (tokens[1].Type == TokenType.Datatype || tokens[1].Type == TokenType.UserDefined)
                return ParseVarDeclaration();
            Console.Error.WriteLine("Expected keyword class or datatype after Access Modifier");
            throw new Exception("Expected keyword class or datatype after Access Modifier");
        }

        private IStatement ParseClassDeclaration()      // accessModifier class Classname { Properties[]; Methods;  }
        {
            Token accessModToken = new Token(TokenType.AccessModifier, "private");
            Token classToken = new Token(TokenType.Datatype, null);
            switch (At().Type)
            {
                case TokenType.AccessModifier:
                    accessModToken = Advance();
                    classToken = Advance();
                    break;
                case TokenType.Class:
                    classToken = Advance();
                    break;
            }
            AccessModifierLiteral accessModifier = new AccessModifierLiteral(accessModToken.Value);
            Token identifierToken = Expect(TokenType.Identifier, $"Expected a variable name after {classToken.Value}");
            Identifier identifier = new Identifier(identifierToken.Value);
            Dictionary<string, VarDeclaration> properties = new Dictionary<string, VarDeclaration>();
            if (Advance().Type == TokenType.OpenBracket)
            {
                while(At().Type != TokenType.CloseBracket && At().Type != TokenType.EOF)
                properties.Add(At().Value, (VarDeclaration)ParseStatement());
            }
            ClassDeclaration classDeclaration = new ClassDeclaration(accessModifier, identifier, properties);
            classes.Add(identifier.Symbol, classDeclaration);
            Lexer.UserDefined.Add(identifier.Symbol, TokenType.UserDefined);
            return classDeclaration;
        }
        private IStatement ParseVarDeclaration()    // accessModifier? refname? datatype varname; OR refname? datatype varname = value;
        {
            Token accessMod = new Token(TokenType.AccessModifier, "private");
            Token datatypeToken = new Token(TokenType.Datatype, null);
            switch (At().Type)
            {
                case TokenType.AccessModifier:
                    accessMod = Advance();
                    datatypeToken = Advance();
                    break;
                case TokenType.Datatype:
                case TokenType.UserDefined:
                    datatypeToken = Advance();
                    break;
            }
            Datatype datatype = new Datatype(datatypeToken.Value);
            AccessModifierLiteral accessModifier = new AccessModifierLiteral(accessMod.Value);
            Identifier identifier = new Identifier(null);
            if (Lexer.KEYWORDS.ContainsKey(datatype.DataTypeProperty) || Lexer.UserDefined.ContainsKey(datatype.DataTypeProperty))
            {
                Token identifierToken = Expect(TokenType.Identifier, $"Expected a variable name after datatype {datatypeToken.Value}");
                identifier = new Identifier(identifierToken.Value);
            }
            Expression value = new Expression();
            if (At().Type == TokenType.SemiColon)
            {
                Token current = Advance();
                if (At().Type == TokenType.CloseBracket)
                {
                    Advance();
                }

                switch (datatype.DataTypeProperty)
                {
                    case "int":
                        value = new IntegerLiteral(0);
                        break;
                    case "float":
                        value = new FloatLiteral(0);
                        break;
                    case "string":
                        value = new StringLiteral(null); 
                        break;

                    default:
                        if (classes.TryGetValue(datatype.DataTypeProperty, out ClassDeclaration _class))
                        value = new ObjectLiteral(_class);
                        objects.Add(identifier.Symbol, new VarDeclaration(accessModifier, datatype, identifier, value));
                        break;
                }
                return new VarDeclaration(accessModifier, datatype, identifier, value);
            }
            //if (Lexer.UserDefined.ContainsKey(datatype.DataTypeProperty))
            //{
            //    Expect(TokenType.Dot, "Expected a dot operator following object or another object!");
            //    ObjectLiteral objectLiteral = new ObjectLiteral(classes[datatype.DataTypeProperty]);
            //    Token propertyExpected = Expect(TokenType.Identifier, "Expected an attribute / property to assign to!");
            //    Expect(TokenType.Equal, "Expected = sign following declaration or ;");
            //    objectLiteral.Value.properties[propertyExpected.Value].Value = ParseExpression();
            //}
            Expect(TokenType.Equal, "Expected = sign following declaration or ;");
            VarDeclaration var = new VarDeclaration(accessModifier, datatype, identifier, ParseExpression());
            Expect(TokenType.SemiColon, "; expected");
            return var;

        }

        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private Expression ParseAssignmentExpression()      // varname = values;
        {
            Expression left = new Expression();
            if (objects.ContainsKey(At().Value))
            {
                Advance();
                if (At().Type == TokenType.Dot)
                {
                    Advance();
                    left = ParseAdditiveExpression();
                    Advance();
                    Expression value = ParseAssignmentExpression();
                    return new AssignmentExpression(left, value);
                }
            }

            left = ParseAdditiveExpression();
            if (At().Type == TokenType.Equal)
            {
                Advance();
                Expression value = ParseAssignmentExpression();
                return new AssignmentExpression(left, value);
            }

            return left;
        }



        private Expression ParseAdditiveExpression()
        {
            Expression left = ParseMultiplicativeExpression();

            while (At().Value == "+" || At().Value == "-")
            {
                Token op = Advance();
                Expression right = ParseMultiplicativeExpression();
                left = new BinaryExpression(left, right, op.Value);
            }
            return left;
        }

        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParsePrimaryExpression();

            while (At().Value == "*" || At().Value == "/" || At().Value == "%")
            {
                string op = Advance().Value;
                Expression right = ParsePrimaryExpression();
                left = new BinaryExpression(left, right, op);
            }
            return left;
        }

        private Expression ParsePrimaryExpression()
        {
            TokenType token = At().Type;

            switch (token)
            {
                case TokenType.Identifier:
                    return new Identifier(Advance().Value);
                case TokenType.IntLiteral:
                    return new IntegerLiteral(int.Parse(Advance().Value));
                case TokenType.FloatLiteral:
                    return new FloatLiteral(float.Parse(Advance().Value));
                case TokenType.StringLiteral:
                    return new StringLiteral(Advance().Value);
                case TokenType.OpenParen:
                    Advance();
                    Expression value = ParseExpression();
                    Expect(TokenType.CloseParen, "Expected a closed parenthesis at the end of the expression!");
                    return value;
                case TokenType.Null:
                    Advance();
                    return new NullLiteral();
                case TokenType.OpenBracket:
                    Advance();
                    Expression val = ParseExpression();
                    Expect(TokenType.CloseParen, "Expected a closed parenthesis at the end of the expression!");
                    return val;
                case TokenType.Dot:
                    return null;


                default:
                    Console.Error.WriteLine($"Unexpected token found {At().Type}, {At().Value}");
                    throw new Exception();
            }
        }

        private bool NotEof()
        {
            return tokens[0].Type != TokenType.EOF;
        }
    }
}
