using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Factory;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurvivalismRedux.Managers {
    public class GameManager : Singleton<GameManager> {
        #region Constructors

        protected GameManager() {
            _messenger = Messenger.Default;
        }

        #endregion


        #region Properties

        public Game CurrentGame { get { return _games[0]; } }

        #endregion


        #region Methods

        internal void Initialize() {
            //start our scripting engine
            _messenger.Send( new ExecuteScriptMessage( Assembly.GetExecutingAssembly().GetManifestResourceStream( "SurvivalismRedux.Resources.Scripts.GameManager.lua" ), "GameManager" ) );

            _saves = SaveGameManager.Instance;

            _games = new List<Game>();
        }

        internal void GameIsShuttingDown() {
            //should save here before quitting
        }

        public void StartGame() {
            _messenger.Send( new PrintMessage( "Starting new game in GameManager...", PrintMessage.MessageType.DEBUG ) );

            if ( _games.Count > 0 ) _games.Clear();
            _games.Add( new Game() );

            for ( var i = 0; i < _games.Count; i++ ) {
                if ( !_games[ i ].HasSaveData ) _games[ i ].Initialize(); //not sure about this
                _games[i].StartGame();
            }
        }

        public void EndGame() {
            _messenger.Send( new PrintMessage( "Exiting the game in GameManager...", PrintMessage.MessageType.DEBUG ) );
            GameIsShuttingDown();
            Application.Current.Shutdown();
        }

        public void EndGameAskForConfirmation() {
            _messenger.Send( new PrintMessage( "Are you sure you want to exit?" ) );
            _messenger.Send( new DecisionMessage( new Decision[] {
                new Decision( "Yes", EndGame ),
                new Decision( "No", EndGame )
            } ) );
        }

        #endregion


        #region Fields

        private IMessenger _messenger;
        private SaveGameManager _saves;
        private List<Game> _games;        

        #endregion
    }
}
