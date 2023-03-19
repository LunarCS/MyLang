using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLang.Runtime
{

    public interface IVariable
    {
        string AccessModifier { get; }
        string Name { get; }
        IRuntimeValue Value { get; set; }
    }

    public unsafe interface IRefVariable : IVariable
    {
        IRefVariable* Ptr { get; }
    }

    public static class RefnameDict
    {
        public static Dictionary<string, IntPtr> refnameDict = new Dictionary<string, IntPtr>();
    }

    public abstract class Var : IVariable
    {
        public string AccessModifier { get; protected set; }
        public IRuntimeValue Value { get; set; }
        public string Name { get; protected set; }
        public string Datatype { get; protected set; }
    }

    public class NormalVar : Var
    {
        public NormalVar(string access, string datatype, string name, IRuntimeValue value)
        {
            AccessModifier = access;
            Datatype = datatype;
            Name = name;
            Value = value;
        }
    }

    public unsafe class RefnameVariable : Var, IRefVariable
    {
        public IRefVariable* Ptr { get; protected set; }

        public RefnameVariable(string datatype, string name, IRuntimeValue value, RefnameVariable* ptr)
        {
            Datatype = datatype;
            Value = value;
            Name = name;
            Ptr = (IRefVariable*)ptr;
        }
    }


}
