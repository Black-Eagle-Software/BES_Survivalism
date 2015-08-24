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
            this._messenger = Messenger.Default;
        }

        #endregion


        #region Properties

        public Game CurrentGame => this._games[0];

        #endregion


        #region Methods

        internal void Initialize() {
            //start our scripting engine
            this._messenger.Send( new ExecuteScriptMessage( Assembly.GetExecutingAssembly().GetManifestResourceStream( "SurvivalismRedux.Resources.Scripts.GameManager.lua" ), "GameManager" ) );

            this._saves = SaveGameManager.Instance;

            this._games = new List<Game>();
        }

        internal void GameIsShuttingDown() {
            //should save here before quitting
        }

        public void StartGame() {
            this._messenger.Send( new PrintMessage( "Starting new game in GameManager...", PrintMessage.MessageType.DEBUG ) );

            if (this._games.Count > 0 ) this._games.Clear();
            this._games.Add( new Game() );

            foreach (var g in this._games) {
                if ( !g.HasSaveData ) g.Initialize(); //not sure about this
                g.StartGame();
            }
        }

        public void EndGame() {
            this._messenger.Send( new PrintMessage( "Exiting the game in GameManager...", PrintMessage.MessageType.DEBUG ) );
            this.GameIsShuttingDown();
            Application.Current.Shutdown();
        }

        public void EndGameAskForConfirmation() {
            this._messenger.Send( new PrintMessage( "Are you sure you want to exit?" ) );
            this._messenger.Send( new DecisionMessage( new[] {
                new Decision( "Yes", this.EndGame ),
                new Decision( "No", this.EndGame )
            } ) );
        }

        #endregion


        #region Fields

        private readonly IMessenger _messenger;
        private SaveGameManager _saves;
        private List<Game> _games;        

        #endregion
    }
}
