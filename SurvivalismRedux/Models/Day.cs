using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Managers;
using SurvivalismRedux.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Models {
    public class Day : IComparer<Day> {
        #region Constructors

        public Day( int dayNumber, Player player ) {
            _messenger = Messenger.Default;
            _sm = ScenarioManager.Instance;

            Index = dayNumber;
            
            PlayerCharacter = player;

            _messenger.Register<EndScenarioMessage>( this, msg => {
                _messenger.Send( new DecisionMessage( new Decision( "Continue...", EndScenario ) ) );
            } );
        }

        #endregion


        #region Properties

        public int Index { get; } = -1;
        public TimeOfDay Time { get; private set; }
        public Player PlayerCharacter { get; }

        #endregion


        #region Methods

        public void StartDay() {
            Time = TimeOfDay.Morning;
            //get a new scenario from the scenario pool
            StartNewScenarioFromPool();
            //complete scenario
            //scenario may have one or more decisions (decision, then followup decision, then followup, etc.)
            //progress to next part of day
            //_messenger.Send( new PrintMessage( string.Format( "Starting {1} of day {0}", _index, TimeOfDay.Afternoon) ) );
            //repeat
            //_messenger.Send( new PrintMessage( string.Format( "Starting {1} of day {0}", _index, TimeOfDay.Evening ) ) );
        }

        public void EndDay() {
            OnDidEndDay();
        }

        private void StartNewScenarioFromPool() {
            _messenger.Send( new ClearOutputParagraphMessage() );
            ReportStartOfTimeOfDay();
            var nextScenario = _sm.GetScenario();
            nextScenario.LastRan = this;
            nextScenario.TimesRan++;
            _messenger.Send( new StartScenarioMessage( nextScenario ) );
        }

        private void ReportStartOfTimeOfDay() {
            _messenger.Send( new PrintMessage( $"Starting day {Index}, {Time}", PrintMessage.MessageType.MESSAGE_BOX ) );
        }

        public int Compare( Day x, Day y ) {
            return x.Index.CompareTo( y.Index );
        }

        protected void OnDidEndDay() {
            var handler = DidEndDay;
            if ( handler != null ) {
                handler( this );
            }
        }

        private void EndScenario() {
            //transition to the next time of day
            if ( Time != TimeOfDay.Evening && PlayerCharacter.ActionPoints > 0 ) {
                Time++;
                StartNewScenarioFromPool();
            } else {
                EndDay();
            }
        }

        #endregion


        #region Fields
        
        private IMessenger _messenger;
        private ScenarioManager _sm;

        public delegate void EndedDay( Day sender );
        public event EndedDay DidEndDay;
        public enum TimeOfDay {
            Morning,
            Afternoon,
            Evening
        }

        #endregion

    }
}
