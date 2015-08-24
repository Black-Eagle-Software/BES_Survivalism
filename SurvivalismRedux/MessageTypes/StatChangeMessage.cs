using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.MessageTypes {
    public class StatChangeMessage {
        #region Constructors

        public StatChangeMessage(string player, string statName, int amount ) {
            PlayerName = player;
            StatName = statName;
            Amount = amount;
        }

        #endregion


        #region Properties

        public string PlayerName { get; set; }
        public string StatName { get; set; }
        public int Amount { get; set; }

        #endregion


        #region Methods



        #endregion


        #region Fields



        #endregion
    }
}
