using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    enum TokenType
    {
        // Keywords
        Keyword,
        Datatype,
        Class,
        AccessModifier,
        UserDefined,
        Null,

        // Literals
        Identifier,
        Literal,
        IntLiteral,
        FloatLiteral,
        StringLiteral,

        // Grouping / Operators
        Operator,
        Delimiter,
        OpenParen,  // (
        CloseParen, // )
        SemiColon,
        Equal,
        Comma,
        Colon,
        OpenBrace,  // [
        CloseBrace, // ]
        OpenBracket,  // {
        CloseBracket, // }
        Dot,   // .

        EOF,
    }
    class Token
    {
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public Token(TokenType type, string value)
        {
            Value = value;
            Type = type;
        }
    }
    internal static class Lexer
    {

        // refname? ref? type var = (smth + smth);
        // [refNameToken, refToken, typeToken, identifierToken, equalsToken, openParenToken,
        // closeParenToken, BinaryOperatorToken, valueToken, endToken]

        public static readonly Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>
        {
            { "ref", TokenType.Keyword },
            { "null", TokenType.Null },
            { "refname", TokenType.Keyword },
            { "class", TokenType.Class },
            { "private", TokenType.AccessModifier },
            { "public", TokenType.AccessModifier },
            { "protected", TokenType.AccessModifier },
            { "if", TokenType.Keyword },
            { "else", TokenType.Keyword },
            { "while", TokenType.Keyword },
            { "for", TokenType.Keyword },
            { "int", TokenType.Datatype },
            { "string", TokenType.Datatype },
            { "bool", TokenType.Datatype },
            { "char", TokenType.Datatype },
            { "float", TokenType.Datatype },
            { "true", TokenType.Literal },
            { "false", TokenType.Literal }
        };

        public static Dictionary<string, TokenType> UserDefined = new Dictionary<string, TokenType>();
        private static readonly List<char> operators = new List<char>
        {
            '+', '-', '*', '/', '%', '>', '<', '!'
        };

        public static List<Token> Tokenize(string sourceCode)
        {
            List<Token> tokens = new List<Token>();
            string currentToken = "";
            
            foreach(var character in sourceCode)
            {
                HandleTokens(character, ref tokens, ref currentToken);
            }
            if (!string.IsNullOrEmpty(currentToken))
            {
                tokens.Add(CreateToken(currentToken));
            }

            tokens.Add(new Token(TokenType.EOF, "EndOfFile"));

            return tokens;
        }

        static void CheckIfEndOfToken(ref List<Token> tokens, ref string currentToken)
        {
            if (!string.IsNullOrEmpty(currentToken))
            {
                tokens.Add(CreateToken(currentToken));
                currentToken = "";
            }
        }
        static void HandleTokens(char character, ref List<Token> tokens, ref string currentToken)
        {
            if (char.IsWhiteSpace(character))
            {
                CheckIfEndOfToken(ref tokens, ref currentToken);
            }
            else if (character == '(' || character == ')' || character == '{' || character == '}' || character == ':' || character == ',' || character == '.')
            {
                CheckIfEndOfToken(ref tokens, ref currentToken);
                tokens.Add(CreateToken(character.ToString()));
            }
            else if (character == ';')
            {
                CheckIfEndOfToken(ref tokens, ref currentToken);
                tokens.Add(CreateToken(character.ToString()));
            }
            else
            {
                currentToken += character;
            }
        }
        private static Token CreateToken(string tokenStr)
        {
            if (int.TryParse(tokenStr, out _))
            {
                return new Token(TokenType.IntLiteral, tokenStr);
            }

            if (float.TryParse(tokenStr, out _))
            {
                return new Token(TokenType.FloatLiteral, tokenStr);
            }

            if (KEYWORDS.TryGetValue(tokenStr, out var keywordValue))
            {
                return new Token(keywordValue, tokenStr);
            }

            if (operators.Contains(tokenStr[0]))
            {
                return new Token(TokenType.Operator, tokenStr);
            }

            if (UserDefined.ContainsKey(tokenStr))
            {
                return new Token(UserDefined[tokenStr], tokenStr);
            }
            
            switch (tokenStr[0])
            {
                case '(':
                    return new Token(TokenType.OpenParen, tokenStr);
                case ')':
                    return new Token(TokenType.CloseParen, tokenStr);
                case ';':
                    return new Token(TokenType.SemiColon, tokenStr);
                case '=':
                    return new Token(TokenType.Equal, tokenStr);
                case '[':
                    return new Token(TokenType.OpenBrace, tokenStr);
                case ']':
                    return new Token(TokenType.CloseBrace, tokenStr);
                case ':':
                    return new Token(TokenType.Colon, tokenStr);
                case ',':
                    return new Token(TokenType.Comma, tokenStr);
                case '{':
                    return new Token(TokenType.OpenBracket, tokenStr);
                case '}':
                    return new Token(TokenType.CloseBracket, tokenStr);
                case '.':
                    return new Token(TokenType.Dot, tokenStr);
            }
            
            if (tokenStr[0] == '"')
            {
                if (tokenStr[tokenStr.Length - 1] == '"')
                {
                    return new Token(TokenType.StringLiteral, tokenStr);
                }
            }
            return new Token(TokenType.Identifier, tokenStr);
        }
    }
}
