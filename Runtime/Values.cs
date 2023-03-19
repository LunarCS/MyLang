
using LLang.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang
{
    public enum ValueTypes
    {
        Null,
        Int,
        Float,
        String,
        Object
    }

    public interface IRuntimeValue
    {
        ValueTypes Type { get; set; }
        string Name { get; set; }
        void DisplayResult();
    }


    class IntValue : IRuntimeValue
    {
        public ValueTypes Type { get; set; } = ValueTypes.Int;
        public string Name { get; set; } = "NumberValue";
        public int Value { get; set; }
        public IntValue(int num)
        {
            Type = ValueTypes.Int;
            Name = "Number";
            Value = num;
        }

        public void DisplayResult()
        {
            Console.WriteLine(Value);
        }
    }

    class FloatValue : IRuntimeValue
    {
        public ValueTypes Type { get; set; } = ValueTypes.Float;
        public string Name { get; set; } = "NumberValue";
        public float Value { get; set; }
        public FloatValue(float num)
        {
            Type = ValueTypes.Float;
            Name = "Number";
            Value = num;
        }
        public void DisplayResult()
        {
            Console.WriteLine(Value);
        }
    }
    class StringValue : IRuntimeValue
    {
        public ValueTypes Type { get; set; } = ValueTypes.String;
        public string Name { get; set; } = "StringValue";
        public string Value { get; set; }
        public StringValue(string str)
        {
            Type = ValueTypes.String;
            Name = "Number";
            Value = str;
        }
        public void DisplayResult()
        {
            Console.WriteLine(Value);
        }
    }
    class NullableValue : IRuntimeValue
    {
        public ValueTypes Type { get; set; } = ValueTypes.Null;
        public string Name { get; set; }
        public NullableValue()
        {
            Type = ValueTypes.Null;
            Name = "null";
        }
        public void DisplayResult()
        {
            Console.WriteLine(Name);
        }
    }

    class ObjectValue : IRuntimeValue
    {
        public ValueTypes Type { get; set; } = ValueTypes.Object;
        public string Name { get; set; }
        public ObjectLiteral Value { get; set; }
        public ObjectValue(ObjectLiteral val)
        {
            Type = ValueTypes.Null;
            Name = "null";
            Value = val;
        }
        public void DisplayResult()
        {
            Console.WriteLine(Name);
        }
    }
}
