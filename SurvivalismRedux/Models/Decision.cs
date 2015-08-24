using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Models {
    public class Decision {
        #region Constructors

        public Decision( string description, Action result, int actionPointsCost = 0 ) {
            Description = description;
            Result = result;
            ActionPointsCost = actionPointsCost;
        }

        #endregion


        #region Properties

        public string Description { get;  }
        public int ActionPointsCost { get;  }
        public Action Result { get;  }

        public string CostAndDescription { get { return ActionPointsCost > 0 ? $"{Description} ({ActionPointsCost} AP)" : Description; } }

        #endregion


        #region Methods

        internal void OnDidSelectDecision() {
            var handler = DidSelectDecision;
            if ( handler != null ) {
                handler( this );
            }
        }

        #endregion


        #region Fields

        public delegate void SelectedDecision( Decision sender );
        public event SelectedDecision DidSelectDecision;

        #endregion
    }
}
