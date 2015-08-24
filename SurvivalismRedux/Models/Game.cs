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
            _messenger = Messenger.Default;
            _psof = PlayerStatOutputFactory.Instance;
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        internal void Initialize() {
            _timeline = new Timeline();
            _player = new Player( "Player", 10 );

            _messenger.Register<DecisionMessage>( this, msg => {
                for ( var i = 0; i < msg.DecisionCount; i++ ) {
                    msg.Decisions[ i ].DidSelectDecision += Dec_DidSelectDecision;
                }
            } );
            _messenger.Register<StatChangeMessage>( this, msg => {
                if ( _player.Name.Equals( msg.PlayerName, StringComparison.OrdinalIgnoreCase ) ) {
                    _player.ChangeStatValue( msg.StatName, msg.Amount );
                }
            } );
        }

        internal void StartGame() {
            StartNewDay();
            //should save at the start of each day
            //should have 3 rolling saves going
            //...newest, prior, next oldest
            //and allow for loading any of them
            //though progress will be lost for not newest save
            //also need to coordinate between the game and the days
            //so that we know when a day has passed
        }

        internal int CheckStatValue( string statName ) {
            return _player.CheckStatValue( statName );
        }

        private void D_DidEndDay( Day sender ) {
            sender.DidEndDay -= D_DidEndDay;
            _messenger.Send( new ClearOutputParagraphMessage() );
            _messenger.Send( new PrintMessage( $"Current player stats summary:" ) );
            //_messenger.Send( new PrintMessage( $"{_psof.CreateStringFromPlayerStats( _player )}" ) );
            _messenger.Send( new PrintMessage( _psof.CreateTableFromPlayerStats( _player ) ) );
            _messenger.Send( new DecisionMessage( new Decision( "Continue...", StartNewDay ) ) );
            //StartNewDay();
        }

        private void StartNewDay() {
            _player.PrepareForNewDay();
            var d = new Day( _timeline.Count + 1, _player );
            d.DidEndDay += D_DidEndDay;
            _timeline.Add( d );
            d.StartDay();
        }

        private void Dec_DidSelectDecision( Decision sender ) {
            if ( sender.ActionPointsCost > 0 ) {
                _player.DeductActionPoints( sender.ActionPointsCost );
                //should check if out of action points for the day now  
                _messenger.Send( new PrintMessage( $"Player action points remaining: {_player.ActionPoints}", PrintMessage.MessageType.DEBUG ) );
            }
        }

        #endregion


        #region Fields

        private IMessenger _messenger;
        private Player _player;
        private PlayerStatOutputFactory _psof;
        private Timeline _timeline;

        public bool HasSaveData;

        #endregion
    }
}
