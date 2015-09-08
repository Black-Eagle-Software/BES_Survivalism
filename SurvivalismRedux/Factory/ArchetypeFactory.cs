using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Models;

namespace SurvivalismRedux.Factory {
    public class ArchetypeFactory : Singleton<ArchetypeFactory> {
        #region Constructors

        protected ArchetypeFactory() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public Archetype GetArchetypeFromString(string value) {
            return this._archetypes.FirstOrDefault(a => a.Name == value);
        }

        #endregion


        #region Fields

        private readonly List<Archetype> _archetypes = new List<Archetype>();

        #endregion


    }
}
