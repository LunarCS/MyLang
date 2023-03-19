using LLang.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLang;
using System.Xml.Linq;

namespace LLang
{


    public class Environment
    {
        protected Environment parent;
        protected Dictionary<string, IVariable> Variables = new Dictionary<string, IVariable>();
        protected Dictionary<string, Class> Classes = new Dictionary<string, Class>();

        public Environment(Environment parent = null)
        {
            this.parent = parent;
        }

        public IRuntimeValue Datatype(string datatype)
        {
            switch (datatype)
            {
                case "string":
                    return new StringValue(null);
                case "int":
                    return new IntValue(0);
                case "float":
                    return new FloatValue(0);
                default:
                    Console.Error.WriteLine($"{datatype} is an invalid datatype!");
                    throw new NotImplementedException($"{datatype} is an invalid datatype!");
            }
        }

        public Class ClassType(string className)
        {
            if (Classes.ContainsKey(className))
            {
                return Classes[className];
            }
            else
            {
                Console.Error.WriteLine($"{className} is an invalid class or not implemented!");
                throw new NotImplementedException($"{className} is an invalid class!");
            }
        }

        public IRuntimeValue DeclareVar(string datatype, string name, string access = "private")
        {
            if (Variables.ContainsKey(name))
            {
                Console.Error.WriteLine($"Cannot declare new variable as it already exits! {name}");
                throw new Exception($"Cannot declare new variable as it already exits! {name}");
            }
            else
            {
                if (Classes.ContainsKey(datatype))
                {
                    ObjectClass obj = new ObjectClass(Classes[datatype], name, null, access);
                    Variables.Add(name, obj);
                    return new StringValue(null);
                }
                NormalVar var = new NormalVar(access, datatype, name, Datatype(datatype));
                Variables.Add(name, var);
                return var.Value;
            }

        }



        //public unsafe IRuntimeValue Declare(string refVar, string datatype, string name)
        //{
        //    IRuntimeValue variableVal = Datatype(datatype);
        //    RefnameVariable var = new RefnameVariable(datatype, name, variableVal, &var);
        //    if (Variables.ContainsKey(name))
        //    {
        //        Variables.Add(name, var);
        //    }
        //    else
        //    {
        //        Console.Error.WriteLine($"Cannot declare new variable as it already exits! {var.Name}");

        //        throw new Exception($"Cannot declare new variable as it already exits! {var.Name}");
        //    }
        //    return var.Value;
        //}

        public IRuntimeValue AssignVariable(string varName, IRuntimeValue value)
        {
            Environment environment = Resolve(varName);
            environment.Variables[varName].Value = value;
            return value;
        }

        public IRuntimeValue LookupVariable(string varName)
        {
            Environment environment = Resolve(varName);
            return environment.Variables[varName].Value;
        }

        public Environment Resolve(string varName)
        {
            if (Variables.ContainsKey(varName))
                return this;
            if (parent == null)
            {
                Console.WriteLine($"Can not resolve {varName}, either it doesn't exist or not in scope ");
                throw new Exception($"Can not resolve {varName}, either it doesn't exist or not in scope ");
            }
            return parent.Resolve(varName);
        }

        public void CreateClass(string access, string className, Dictionary<string, VarDeclaration> properties)
        {
            if (Classes.ContainsKey(className))
            {
                Console.Error.WriteLine($"Cannot define new class as it already exits! {className}");
                throw new Exception($"Cannot define new class as it already exits!  {className}");
            }
            else
            {
                Class @class = new Class(access, className);
                Classes.Add(className, @class);
                foreach (KeyValuePair<string, VarDeclaration> property in properties)
                {
                    @class.DeclareVar(property.Key, property.Value.Identifier.Symbol, property.Value.AccessModifier.Value);
                }
            }
        }
        //public ObjectClass DeclareObject(string access, string className, string name)
        //{
        //    if (Variables.ContainsKey(name))
        //    {
        //        Console.Error.WriteLine($"Cannot declare new object as it already exits! {name}");
        //        throw new Exception($"Cannot declare new object as it already exits! {name}");
        //    }
        //    else
        //    {
        //        ObjectClass obj = new ObjectClass(ClassType(className), name, name, access);
        //        Objects.Add(name, obj);
        //        return obj;
        //    }
        //public ObjectClass AssignObject(string varName, ObjectClass obj)
        //{
        //    Environment environment = Resolve(varName);
        //    environment.Objects[varName] = obj;
        //    return obj;
        //}
        public IRuntimeValue AssignObjectProperty(string varName, string propertyName, IRuntimeValue value)
        {
            Environment environment = Resolve(varName);
            ObjectClass obj = (ObjectClass)environment.Variables[varName];
            obj.AssignProperty(propertyName, value);
            return value;
        }
    } 
}
