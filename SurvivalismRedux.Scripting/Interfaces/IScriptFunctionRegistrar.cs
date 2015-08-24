using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Interfaces {
    public interface IScriptFunctionRegistrar {
        void RegisterFunction( object target );
        void ScanForAndRegisterFunctions();
    }
}
