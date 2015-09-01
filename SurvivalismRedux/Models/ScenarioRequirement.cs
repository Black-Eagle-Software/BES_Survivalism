using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Scripting;

namespace SurvivalismRedux.Models {
    public class ScenarioRequirement {
        #region Constructors

        public ScenarioRequirement(RequirementTags name, object[] values) {
            this.Name = name;
            this.Values = values;
        }

        #endregion


        #region Properties

        public RequirementTags Name { get; set; }
        public object[] Values { get; set; }

        #endregion


        #region Methods



        #endregion


        #region Fields



        #endregion
    }
}
