using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.MessageTypes {
    public class StartScenarioMessage {
        #region Constructors

        public StartScenarioMessage(Scenario scenario ) {
            Payload = scenario;
        }

        #endregion


        #region Properties

        public Scenario Payload { get; set; }

        #endregion


        #region Methods



        #endregion


        #region Fields



        #endregion
    }
}
