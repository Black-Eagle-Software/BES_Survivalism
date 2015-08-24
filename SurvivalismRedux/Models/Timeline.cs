using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Models {
    public class Timeline : IList<Day> {
        #region Constructors



        #endregion


        #region Properties

        public Day this[int index] {
            get { return _backer[index]; }
            set { _backer[index] = value; }
        }
        public int Count { get { return _backer.Count(); } }

        public bool IsReadOnly { get { return ( _backer as IList ).IsReadOnly; } }

        #endregion


        #region Methods

        public void Add( Day item ) {
            //only add if not duplicate
            if ( _backer.Any( d => d.Equals( item ) ) ) return;
            _backer.Add( item );
            _backer.Sort( ( a, b ) => a.Compare( a, b ) );
        }

        public void Clear() {
            _backer.Clear();
        }

        public bool Contains( Day item ) {
            return _backer.Contains( item );
        }

        public void CopyTo( Day[] array, int arrayIndex ) {
            _backer.CopyTo( array, arrayIndex );
        }

        public IEnumerator<Day> GetEnumerator() {
            return _backer.GetEnumerator();
        }

        public int IndexOf( Day item ) {
            return _backer.IndexOf( item );
        }

        public void Insert( int index, Day item ) {
            _backer.Insert( index, item );
        }

        public bool Remove( Day item ) {
            return _backer.Remove( item );
        }

        public void RemoveAt( int index ) {
            _backer.RemoveAt( index );
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion


        #region Fields

        protected List<Day> _backer = new List<Day>();

        #endregion





    }
}
