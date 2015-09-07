using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Models.Interfaces;

namespace SurvivalismRedux.Models {
    public class Scenario {
        #region Constructors



        #endregion


        #region Properties

        public int ApiVersion { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string[] FilePaths { get; set; }
        public string TocFilename { get; set; }

        public Day LastRan { get; set; }
        public int TimesRan { get; set; }

        public List<IScenarioRequirement> Requirements
            => this._requirements ?? (this._requirements = new List<IScenarioRequirement>());

        #endregion


        #region Methods



        #endregion


        #region Fields

        private List<IScenarioRequirement> _requirements;

        #endregion
    }
}
