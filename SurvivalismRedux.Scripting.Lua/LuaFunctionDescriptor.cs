using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Lua {
    public class LuaFunctionDescriptor {
        #region Constructors

        public LuaFunctionDescriptor(string name, string description, ArrayList parameters, ArrayList parameterDescriptions) {
            _name = name;
            _description = description;
            _parameters = parameters;
            _paramDescriptions = parameterDescriptions;

            var header = name + "(%params%) - " + description;
            var body = "\n\n";
            var parms = "";

            var bFirst = true;

            for ( var i = 0; i < parameters.Count; i++ ) {
                if ( !bFirst ) {
                    parms += ", ";
                }
                parms += parameters[ i ];
                body += "\t" + parameters[ i ] + "\t\t" + parameterDescriptions[ i ] + "\n";
                bFirst = false;
            }
            body = body.Substring( 0, body.Length - 1 );
            if ( bFirst ) {
                body = body.Substring( 0, body.Length - 1 );
            }
            _documentationString = header.Replace( "%params%", parms ) + body;
        }

        #endregion


        #region Properties

        public string FunctionName { get { return _name; } }
        public string FunctionDescription { get { return _description; } }
        public ArrayList FunctionParameters { get { return _parameters; } }
        public ArrayList FunctionParameterDescriptions { get { return _paramDescriptions; } }
        public string FunctionDocumentationString { get { return _documentationString; } }

        public string FunctionHeader {
            get {
                if ( FunctionDocumentationString.IndexOf( "\n" ) == -1 ) {
                    return FunctionDocumentationString;
                }
                return FunctionDocumentationString.Substring( 0, FunctionDocumentationString.IndexOf( "\n" ) );
            }
        }

        #endregion


        #region Methods



        #endregion


        #region Fields

        private string _name;
        private string _description;
        private ArrayList _parameters;
        private ArrayList _paramDescriptions;
        private string _documentationString;

        #endregion
    }
}
