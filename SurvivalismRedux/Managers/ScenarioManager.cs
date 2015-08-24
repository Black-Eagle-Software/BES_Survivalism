using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Managers {
    public class ScenarioManager : Singleton<ScenarioManager> {
        #region Constructors

        protected ScenarioManager() {
            _rand = new Random();
        }



        #endregion


        #region Properties

        public List<Scenario> ScenarioPool { get { return _scenarioPool; } }

        #endregion


        #region Methods

        internal Scenario GetScenario() {
            return _scenarioPool[_rand.Next( _scenarioPool.Count )];
        }

        internal void AddScenario( Scenario s ) {
            if ( s != null ) {
                _scenarioPool.Add( s );
            }
        }

        #endregion


        #region Fields

        private Random _rand;
        private List<Scenario> _scenarioPool = new List<Scenario>();

        

        #endregion
    }
}
