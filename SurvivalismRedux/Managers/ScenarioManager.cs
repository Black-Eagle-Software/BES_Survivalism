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
            this._rand = new Random();
        }



        #endregion


        #region Properties

        public List<Scenario> ScenarioPool { get; } = new List<Scenario>();

        #endregion


        #region Methods

        internal Scenario GetScenario() {
            return this.ScenarioPool[this._rand.Next(this.ScenarioPool.Count )];
        }

        internal void AddScenario( Scenario s ) {
            if ( s != null ) {
                this.ScenarioPool.Add( s );
            }
        }

        #endregion


        #region Fields

        private readonly Random _rand;

        #endregion
    }
}
