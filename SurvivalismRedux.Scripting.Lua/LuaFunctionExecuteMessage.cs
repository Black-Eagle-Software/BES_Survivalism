using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Lua {
    public class LuaFunctionExecuteMessage {
        public LuaFunctionExecuteMessage(LuaFunction func ) {
            Function = func;
        }

        public LuaFunction Function { get; set; }
    }
}
