using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.MessageTypes;
using System;
using System.Collections.Generic;

namespace SurvivalismRedux.Models {
    public class Player {
        #region Constructors

        public Player( string name, int ap ) {
            Name = name;
            SetGender();

            ActionPoints = ap;
            _originalAp = ActionPoints;

            _stats.Add( Stats.AGILITY, 6 );
            _stats.Add( Stats.HEALTH, 10 );
            _stats.Add( Stats.SANITY, 10 );
            _stats.Add( Stats.DAMAGE, 10 );
        }

        #endregion


        #region Properties

        public Dictionary<Stats, int> PlayerStats { get { return _stats; } }
        public List<Tuple<Stats, int>> ModifiedStats { get { return _modifiedStats; } }
        public int ActionPoints { get; private set; }
        public string Name { get; private set; }
        public Gender PlayerGender { get; private set; }
        public Archetype PlayerArchetype { get; private set; }

        public int Agility { get { return _stats[ Stats.AGILITY ]; } }
        public int Health { get { return _stats[ Stats.HEALTH ]; } }
        public int Sanity { get { return _stats[ Stats.SANITY ]; } }



        #endregion


        #region Methods

        public void PrepareForNewDay() {
            ResetActionPoints();
            _modifiedStats.Clear();
        }
        public void ResetActionPoints() {
            ActionPoints = _originalAp;
        }
        public void DeductActionPoints( int actionPointsCost ) {
            ActionPoints -= actionPointsCost;
        }

        internal void ChangeStatValue( string statName, int amount ) {
            var sk = ( Stats )Enum.Parse( typeof( Stats ), statName.ToUpper() );
            if ( !_stats.ContainsKey( sk ) ) return;
            var ov = _stats[ sk ];
            _stats[ sk ] += amount;
            _modifiedStats.Add( new Tuple<Stats, int>( sk, amount ) );
            Messenger.Default.Send( new PrintMessage( $"Changed {statName} by {amount}, old value was {ov}, new value is {_stats[ sk ]}", PrintMessage.MessageType.DEBUG ) );
        }

        internal int CheckStatValue( string statName ) {
            var sk = ( Stats )Enum.Parse( typeof( Stats ), statName.ToUpper() );
            if ( !_stats.ContainsKey( sk ) ) return int.MinValue;
            return _stats[ sk ];
        }

        private void SetGender() {
            var rn = RandomNumberHelper.Instance.GetRandomInt( 1000 );
            PlayerGender = rn >= 485 ? Gender.MALE : Gender.FEMALE;
        }

        #endregion


        #region Fields

        private int _originalAp;

        private Dictionary<Stats, int> _stats = new Dictionary<Stats, int>();
        private List<Tuple<Stats, int>> _modifiedStats = new List<Tuple<Stats, int>>();

        #endregion
    }
}
