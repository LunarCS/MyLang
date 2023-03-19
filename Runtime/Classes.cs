using LLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang.Runtime
{
    public interface IImplementations
    {
        string Access { get; }
        string Datatype { get; }
        string Name { get; }
    }
    //public class Property : IImplementations
    //{
    //    public string Access { get; set; }
    //    public string Name { get; }
    //    public string Datatype { get; }
    //    public IRuntimeValue Value { get; set; }

    //    public Property(string access, string name, string datatype, IRuntimeValue value)
    //    {
    //        Access = access;
    //        Name = name;
    //        Datatype = datatype;
    //        Value = value;
    //    }
    //}

    public class Method : IImplementations
    {
        public string Access { get; set; }
        public string Name { get; set; }
        public string Datatype { get; set; }
        public IRuntimeValue ReturnValue { get; }
        public List<IVariable> Parameters { get; set; }

        public Method(string access, string name, string datatype, List<IVariable> _params)
        {
            Access = access;
            Name = name;
            Datatype = datatype;
            Parameters = _params;
        }
    }
    public class Class : Environment
    {
        public string AccessModifier { get; }
        public string Name { get; }

        public Class(string access, string className)
        {
            AccessModifier = access;
            Name = className;
        }
    }

    public class ObjectClass : IVariable
    {
        public string AccessModifier { get; }
        public Class Class { get; }
        public string Name { get; }
        public IRuntimeValue Value { get; set; }
        public ObjectClass(Class _class, string name, IRuntimeValue value, string access = "private")
        {
            AccessModifier = access;
            Class = _class;
            Name = name;
            Value = value;
        }

        public void AssignProperty(string varName, IRuntimeValue val)
        {
            Class.AssignObjectProperty(varName, Name, val);
        }
    }
}
