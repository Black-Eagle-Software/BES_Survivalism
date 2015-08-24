using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Factory;
using SurvivalismRedux.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Models {
    public class Game {
        #region Constructors

        public Game() {
            this._messenger = Messenger.Default;
            this._psof = PlayerStatOutputFactory.Instance;
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        internal void Initialize() {
            this._timeline = new Timeline();
            this._player = new Player( "Player", 10 );

            this._messenger.Register<DecisionMessage>( this, msg => {
                for ( var i = 0; i < msg.DecisionCount; i++ ) {
                    msg.Decisions[ i ].DidSelectDecision += this.Dec_DidSelectDecision;
                }
            } );
            this._messenger.Register<StatChangeMessage>( this, msg => {
                if (this._player.Name.Equals( msg.PlayerName, StringComparison.OrdinalIgnoreCase ) ) {
                    this._player.ChangeStatValue( msg.StatName, msg.Amount );
                }
            } );
        }

        internal void StartGame() {
            this.StartNewDay();
            //should save at the start of each day
            //should have 3 rolling saves going
            //...newest, prior, next oldest
            //and allow for loading any of them
            //though progress will be lost for not newest save
            //also need to coordinate between the game and the days
            //so that we know when a day has passed
        }

        internal int CheckStatValue( string statName ) {
            return this._player.CheckStatValue( statName );
        }

        private void D_DidEndDay( Day sender ) {
            sender.DidEndDay -= this.D_DidEndDay;
            this._messenger.Send( new ClearOutputParagraphMessage() );
            this._messenger.Send( new PrintMessage( $"Current player stats summary:" ) );
            //_messenger.Send( new PrintMessage( $"{_psof.CreateStringFromPlayerStats( _player )}" ) );
            this._messenger.Send( new PrintMessage(this._psof.CreateTableFromPlayerStats(this._player ) ) );
            this._messenger.Send( new DecisionMessage( new Decision( "Continue...", this.StartNewDay ) ) );
            //StartNewDay();
        }

        private void StartNewDay() {
            this._player.PrepareForNewDay();
            var d = new Day(this._timeline.Count + 1, this._player );
            d.DidEndDay += this.D_DidEndDay;
            this._timeline.Add( d );
            d.StartDay();
        }

        private void Dec_DidSelectDecision( Decision sender ) {
            if (sender.ActionPointsCost <= 0) {
                return;
            }
            this._player.DeductActionPoints( sender.ActionPointsCost );
            //should check if out of action points for the day now  
            this._messenger.Send( new PrintMessage( $"Player action points remaining: {this._player.ActionPoints}", PrintMessage.MessageType.DEBUG ) );
        }

        #endregion


        #region Fields

        private readonly IMessenger _messenger;
        private Player _player;
        private readonly PlayerStatOutputFactory _psof;
        private Timeline _timeline;

        public bool HasSaveData;

        #endregion
    }
}
