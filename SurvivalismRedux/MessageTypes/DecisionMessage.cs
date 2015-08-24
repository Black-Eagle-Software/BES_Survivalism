using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.MessageTypes {
    public class DecisionMessage {
        #region Constructors

        public DecisionMessage( Decision decision ) {
            _decisions.Add( decision );
        }
        public DecisionMessage( Decision[] decisions ) {
            for ( var i = 0; i < decisions.Length; i++ ) {
                _decisions.Add( decisions[ i ] );
            }
        }

        #endregion


        #region Properties

        public int DecisionCount { get { return _decisions.Count; } }
        public List<Decision> Decisions { get { return _decisions; } }

        #endregion


        #region Methods



        #endregion


        #region Fields

        private List<Decision> _decisions = new List<Decision>();

        #endregion
    }
}
