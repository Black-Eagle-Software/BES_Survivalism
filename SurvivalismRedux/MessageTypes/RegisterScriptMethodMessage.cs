using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.MessageTypes {
    public class RegisterScriptMethodMessage {
        public RegisterScriptMethodMessage( string name, object target, MethodBase function ) {
            Name = name;
            Target = target;
            Function = function;
        }

        public string Name { get; set; }
        public object Target { get; set; }
        public MethodBase Function { get; set; }
    }
}
