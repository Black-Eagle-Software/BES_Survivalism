using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterScriptFunctionAttribute:Attribute {
        #region Constructors

        public RegisterScriptFunctionAttribute(string name, string description, params string[] parameterDocumentation) {
            this._name = name;
            this._description = description;
            this._params = parameterDocumentation;
        }
        public RegisterScriptFunctionAttribute( string name, string description ) {
            this._name = name;
            this._description = description;
        }

        #endregion


        #region Properties

        public string FunctionName { get { return this._name; } }
        public string FunctionDescription { get { return this._description; } }
        public string[] FunctionParameters { get { return this._params; } }

        #endregion


        #region Methods



        #endregion


        #region Fields

        private readonly string _name;
        private readonly string _description;
        private readonly string[] _params;

        #endregion
    }
}
