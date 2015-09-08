using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Models;

namespace SurvivalismRedux.Factory {
    public class StorylineFactory : Singleton<StorylineFactory> {
        #region Constructors

        protected StorylineFactory() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public Storyline GetStorylineFromString(string reqValues) {
            return this._storylines.FirstOrDefault(s => s.Name == reqValues);
        }

        #endregion


        #region Fields

        private readonly List<Storyline> _storylines = new List<Storyline>();

        #endregion


    }
}
