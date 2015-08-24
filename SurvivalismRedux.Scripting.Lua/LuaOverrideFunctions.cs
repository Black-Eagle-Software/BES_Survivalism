using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Scripting.Attributes;
using GalaSoft.MvvmLight.Messaging;

namespace SurvivalismRedux.Scripting.Lua {
    [ScriptFunctionClass]
    public class LuaOverrideFunctions {
        #region Constructors



        #endregion


        #region Properties



        #endregion


        #region Methods

        [RegisterScriptFunction( "message", "Print strings to the UI as special messages.", "S: string to send as body of message" )]
        public void RenderMessage( string s ) {
            Messenger.Default.Send( new PrintMessage( s, PrintMessage.MessageType.MESSAGE_BOX ) );
            //Console.WriteLine( "Received Lua message: '{0}'", s );
        }

        [RegisterScriptFunction( "print", "Print strings to the UI.", "S: string to print" )]
        public void RenderPrint( string s ) {
            Messenger.Default.Send( new PrintMessage( s, PrintMessage.MessageType.PRINT ) );
            //Console.WriteLine( "Printing from Lua: '{0}'", s );
        }

        [RegisterScriptFunction( "debug", "Print strings to the debug console UI.", "S: string to print" )]
        public void RenderDebug( string s ) {
            Messenger.Default.Send( new PrintMessage( s, PrintMessage.MessageType.DEBUG ) );
        }

        #endregion


        #region Fields



        #endregion
    }
}
