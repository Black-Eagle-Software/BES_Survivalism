using GalaSoft.MvvmLight.Messaging;
using NLua;
using SurvivalismRedux.Managers;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models;
using SurvivalismRedux.Scripting.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Lua {
    [ScriptFunctionClass]
    public class LuaCustomFunctions {
        #region Constructors



        #endregion


        #region Properties



        #endregion


        #region Methods

        [RegisterScriptFunction( "BES_SendDecisionMessage", "Send a table of decisions to the UI", "decisions: Table of decisions to send" )]
        public void SendDecisionMessage( LuaTable decisionsToSend ) {
            if ( decisionsToSend != null ) {
                if ( decisionsToSend.Values.Count > 1 ) {
                    //seems to be pulling things in the order they are on the stack, not how they were put there
                    var d = decisionsToSend.Values.Cast<Decision>().Reverse().ToArray();
                    Messenger.Default.Send( new DecisionMessage( d ) );
                }else {
                    var d = decisionsToSend.Values.Cast<Decision>().ToArray();
                    Messenger.Default.Send( new DecisionMessage( d[0] ) );
                }
            }
        }
        [RegisterScriptFunction( "BES_CreateDecision", "Create a decision", new[] {
            "desc: Decsription of the decision",
            "func: Function to be called when this decision is chosen",
            "cost: Action points cost of choosing this decision"
        } )]
        public Decision CreateDecision( string desc, LuaFunction func, int cost ) {
            var d = new Decision( desc, () => { func.Call(); }, cost );
            return d != null ? d : null;
        }

        [RegisterScriptFunction("BES_StartNewGame", "Start a new game")]
        public void StartNewGame() {
            GameManager.Instance.StartGame();
        }

        [RegisterScriptFunction("BES_ExitGame", "Exits the game")]
        public void ExitGame() {
            GameManager.Instance.EndGame();
        }

        #endregion


        #region Fields



        #endregion

    }
}
