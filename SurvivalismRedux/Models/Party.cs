using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Models {
    public class Party : IList<Player> {
        #region Constructors



        #endregion


        #region Properties

        public int Count => this._backingList.Count;
        public bool IsReadOnly => (this._backingList as IList).IsReadOnly;
        public Player this[int index] {
            get { return this._backingList[index]; }
            set { this._backingList[index] = value; }
        }

        #endregion


        #region Methods

        public IEnumerator<Player> GetEnumerator() {
            return this._backingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Add(Player item) {
            if (this._backingList.Any(p => p.Equals(item))) return; //everyone is special!
            this._backingList.Add(item);
        }

        public void Clear() {
            this._backingList.Clear();
        }

        public bool Contains(Player item) {
            return this._backingList.Contains(item);
        }

        public void CopyTo(Player[] array, int arrayIndex) {
            this._backingList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Player item) {
            return this._backingList.Remove(item);
        }
        public int IndexOf(Player item) {
            return this._backingList.IndexOf(item);
        }

        public void Insert(int index, Player item) {
            this._backingList.Insert(index, item);
        }

        public void RemoveAt(int index) {
            this._backingList.RemoveAt(index);
        }

        #endregion


        #region Fields

        private readonly List<Player> _backingList = new List<Player>();

        #endregion







    }
}
