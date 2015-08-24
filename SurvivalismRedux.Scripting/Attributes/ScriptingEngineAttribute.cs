using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptingEngineAttribute:Attribute {
        public ScriptingEngineAttribute( string name ) {
            EngineName = name;
        }

        public string EngineName { get; set; }
    }
}
