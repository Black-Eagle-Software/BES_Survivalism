using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Models;

namespace SurvivalismRedux.Managers {
    public class StorySubjectManager : Singleton<StorySubjectManager> {
        #region Constructors

        protected StorySubjectManager() {
            this._rand = new Random();
        }

        #endregion


        #region Properties

        public List<StorySubject> StorySubjectPool { get; } = new List<StorySubject>();

        #endregion


        #region Methods

        internal StorySubject GetStorySubject() {
            return this.StorySubjectPool[this._rand.Next(this.StorySubjectPool.Count)];
        }

        internal StorySubject GetStorySubject(string name) {
            return this.StorySubjectPool.FirstOrDefault(s => s.Name == name);
        }

        internal void Add(StorySubject ss) {
            if (ss != null) {
                this.StorySubjectPool.Add(ss);
            }
        }

        #endregion


        #region Fields

        private Random _rand;

        #endregion
    }
}
