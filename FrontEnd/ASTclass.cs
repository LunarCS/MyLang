using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    public enum NodeType
    {
        //Statements
        Program,

        //Variables
        AccessModifier,
        Class,
        ClassDeclaration,
        Refname,
        Datatype,
        Identifier,
        VarDeclaration,

        //Expressions
        BinaryExpression,
        AssignmentExpression,

        //Literals
        IntLiteral,
        FloatLiteral,
        StringLiteral,
        NullLiteral,
        Object,
        Property
    }

    public interface IStatement
    {
        NodeType Type { get; }
        string TypeName { get; }
    }

    public class ProgramNode : IStatement
    {
        public NodeType Type { get; } = NodeType.Program;
        public string TypeName { get; } = "Program";
        public List<IStatement> body;
        public ProgramNode(List<IStatement> body)
        {
            this.body = body;
        }
    }

    public class Datatype : IStatement
    {
        public NodeType Type { get; } = NodeType.Datatype;

        public string TypeName { get; } = "Datatype";
        public string DataTypeProperty { get; set; }
            
        public Datatype(string dataType)
        {
            DataTypeProperty = dataType;
        }
    }

    public class VarDeclaration : IStatement
    {
        public NodeType Type { get; } = NodeType.VarDeclaration;
        public string TypeName { get; } = "VarDeclaration";
        public AccessModifierLiteral AccessModifier { get; set; }
        public Datatype Datatype { get; }
        public Identifier Identifier { get; }
        public Expression Value { get; set; } = null;

        public VarDeclaration(AccessModifierLiteral access, Datatype datatype, Identifier identifier, Expression value = null)
        {
            AccessModifier = access;
            Datatype = datatype;
            Identifier = identifier;
            Value = value;
        }
    }

    public class ClassDeclaration : IStatement
    {
        public NodeType Type { get; } = NodeType.ClassDeclaration;
        public string TypeName { get; } = "ClassDeclaration";
        public AccessModifierLiteral AccessModifier { get; set; }
        public Identifier Identifier { get; }
        public Dictionary<string, VarDeclaration> properties { get; }
        public ClassDeclaration(AccessModifierLiteral accessModifier, Identifier identifier, Dictionary<string, VarDeclaration> properties)
        {
            AccessModifier = accessModifier;
            Identifier = identifier;
            this.properties = properties;
        }
    }


    public class Expression : IStatement
    {
        public NodeType Type { get; protected set; }
        public string TypeName { get; protected set; }
    }

    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }
        public string Operator {get; }

        public BinaryExpression(Expression left, Expression right, string _operator)
        {
            Type = NodeType.BinaryExpression;
            TypeName = "BinaryExpression";
            Left = left;
            Right = right;
            Operator = _operator;
        }
    }

    public class Identifier : Expression
    {
        public string Symbol { get; }

        public Identifier(string symbol) 
        {
            Type = NodeType.Identifier;
            TypeName = "Identifier";
            Symbol = symbol;
        }
    }

    public class IntegerLiteral : Expression
    {
        public int Value { get; set; }
        public IntegerLiteral(int val)
        {
            Value = val;
            Type = NodeType.IntLiteral;
            TypeName = "NumericLiteral";
        }
    }
    public class FloatLiteral : Expression
    {
        public float Value { get; set; }
        public FloatLiteral(float val)
        {
            Value = val;
            Type = NodeType.FloatLiteral;
            TypeName = "NumericLiteral";
        }
    }
    public class NullLiteral : Expression
    {
        public float Value { get; }
        public NullLiteral()
        {
            Value = 0;
            Type = NodeType.NullLiteral;
            TypeName = "NullLiteral";
        }
    }

    public class StringLiteral : Expression
    {
        public string Value { get; set; }
        public StringLiteral(string val)
        {
            Value = val;
            Type = NodeType.StringLiteral;
            TypeName = "String";
        }
    }
    public class RefName : Expression
    {
        public bool Exists { get; set; }
        public RefName(bool exists)
        {
            Exists = exists;
            Type = NodeType.Refname;
            TypeName = "Refname";
        }
    }
    public class AccessModifierLiteral : Expression
    {
        public string Value { get; set; }
        public AccessModifierLiteral(string Value)
        {
            this.Value = Value;
            Type = NodeType.AccessModifier;
            TypeName = "AccessModifier";
        }

    }
    public class ClassLiteral : Expression
    {
        public List<PropertyLiteral> Properties { get; set; }
        public ClassLiteral(List<PropertyLiteral> properties = null)
        {
            Properties = properties;
            Type = NodeType.Class;
            TypeName = "Class";
        }
    }

    public class ObjectLiteral : Expression
    {
        public ClassDeclaration Value { get; set; }
        public ObjectLiteral(ClassDeclaration value)
        {
            Type = NodeType.Object;
            TypeName = "Object";
            Value = value;
        }
    }

    public class PropertyLiteral : Expression
    {
        public VarDeclaration Property { get; set; }
        public PropertyLiteral(VarDeclaration property)
        {
            Type = NodeType.Property;
            TypeName = "Property";
            Property = property;
        }
    }

    public class AssignmentExpression : Expression
    {
        public Expression Value { get; set; }
        public Expression Assignment { get; set; }
        public AssignmentExpression(Expression assignment, Expression val)
        {
            Type = NodeType.AssignmentExpression;
            TypeName = "Assignment";
            Value = val;
            Assignment = assignment;
        }
    }

}
