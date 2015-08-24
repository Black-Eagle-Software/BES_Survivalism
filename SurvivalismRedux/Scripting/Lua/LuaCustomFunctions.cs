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
            if (decisionsToSend == null) {
                return;
            }
            if ( decisionsToSend.Values.Count > 1 ) {
                var decs=new Decision[decisionsToSend.Keys.Count];
                for ( var i = 0; i < decisionsToSend.Keys.Count; i++ ) {
                    decs[i] = (Decision)decisionsToSend[i];
                }
                Messenger.Default.Send( new DecisionMessage( decs ) );
            }else {
                var d = decisionsToSend.Values.Cast<Decision>().ToArray();
                Messenger.Default.Send( new DecisionMessage( d[0] ) );
            }
        }

        [RegisterScriptFunction( "BES_CreateDecision", "Create a decision", 
            "desc: Decsription of the decision", "func: Function to be called when this decision is chosen", 
            "cost: Action points cost of choosing this decision")]
        public Decision CreateDecision( string desc, LuaFunction func, int cost ) {
            var d = new Decision( desc, () => { func.Call(); }, cost );
            return d;
        }

        [RegisterScriptFunction("BES_ChangePlayerStats", "Change the value of a player's stat", 
            "player: Player to target", "stat: Stat value to change", "value: Amount to change the stat by")]
        public void ChangeStats(string pName, string sName, int amount ) {
            Messenger.Default.Send( new StatChangeMessage( pName, sName, amount ) );
        }
        [RegisterScriptFunction("BES_GetStatValue", "Gets the integer value of a particular player stat", "stat: The stat value to return")]
        public int GetStatValue(string sName) {
            return GameManager.Instance.CurrentGame.CheckStatValue( sName );
        }

        [RegisterScriptFunction("BES_EndScenario", "Ends the current scenario")]
        public void EndScenario() {
            Messenger.Default.Send( new EndScenarioMessage() );
        }

        [RegisterScriptFunction("BES_StartNewGame", "Start a new game")]
        public void StartNewGame() {
            GameManager.Instance.StartGame();
        }

        [RegisterScriptFunction("BES_ExitGame", "Exits the game")]
        public void ExitGame() {
            //GameManager.Instance.EndGame();
            GameManager.Instance.EndGameAskForConfirmation();
        }
        [RegisterScriptFunction("BES_EnterTestArea", "Enters test area of the game")]
        public void EnterTestArea() {
            TestManager.Instance.EnterTestArea();
        }

        #endregion


        #region Fields



        #endregion

    }
}
