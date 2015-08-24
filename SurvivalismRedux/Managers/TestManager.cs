using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Factory;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Managers {
    public class TestManager : Singleton<TestManager> {
        #region Constructors

        protected TestManager() {
            _messenger = Messenger.Default;
            _psof = PlayerStatOutputFactory.Instance;
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        internal void EnterTestArea() {
            _messenger.Send( new ClearOutputParagraphMessage() );
            _messenger.Send( new PrintMessage( "Entering the test area in TestManager...", PrintMessage.MessageType.DEBUG ) );

            _testPlayer = new Player( "Test Player", 10 );

            SetupTestDecisions();
            OutputTestDecisions();
        }

        private void OutputTestDecisions() {
            _messenger.Send( new DecisionMessage( _testDecisions ) );
        }

        private void SetupTestDecisions() {
            this._testDecisions = new Decision[] {
                new Decision( "Output player stats as string", this.TestPlayerOutputAsString),
                new Decision( "Output player stats as table", this.TestPlayerOutputAsTable)
            };
        }

        private void TestPlayerOutputAsTable() {
            this._messenger.Send( new PrintMessage( $"Outputting stats for {_testPlayer.Name} as flow document table:", PrintMessage.MessageType.DEBUG ) );
            this._messenger.Send( new PrintMessage( this._psof.CreateTableFromPlayerStats( this._testPlayer ) ) );
            var dc = new Decision[] {
                new Decision( "Reduce player agility by 1 and refresh", ()=> {
                    _testPlayer.ChangeStatValue("agility", -1);
                    TestPlayerOutputAsTable();
                } ),
                new Decision( "Reduce player sanity to 0 and refresh", ()=> {
                    _testPlayer.ChangeStatValue("sanity", -_testPlayer.CheckStatValue("sanity"));
                    TestPlayerOutputAsTable();
                } ),
                new Decision("Return to main testing menu", this.EnterTestArea)
            };
            _messenger.Send( new DecisionMessage( dc ) );
            //OutputTestDecisions();
        }

        private void TestPlayerOutputAsString() {
            this._messenger.Send( new PrintMessage( $"Outputting stats for {_testPlayer.Name} as string:", PrintMessage.MessageType.DEBUG ) );
            _messenger.Send( new PrintMessage( _psof.CreateStringFromPlayerStats( _testPlayer ) ) );                        
            OutputTestDecisions();
        }

        #endregion


        #region Fields

        private IMessenger _messenger;
        private Player _testPlayer;
        private PlayerStatOutputFactory _psof;
        private Decision[] _testDecisions;

        #endregion

    }
}
