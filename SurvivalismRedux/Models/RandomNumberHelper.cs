using System;

namespace SurvivalismRedux.Models {
    public class RandomNumberHelper : Singleton<RandomNumberHelper> {
        #region Constructors

        protected RandomNumberHelper() {
            _randomNumber = new Random();
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public int GetRandomInt() {
            return _randomNumber.Next();
        }
        public int GetRandomInt(int maxValue ) {
            return _randomNumber.Next( maxValue );
        }
        public int GetRandomInt(int minValue, int maxValue ) {
            return _randomNumber.Next( minValue, maxValue );
        }

        public float GetRandomDistributionNumber( float mean, float stdDev ) {
            var number1 = _randomNumber.NextDouble();
            var number2 = _randomNumber.NextDouble();
            var randStdNormal = Math.Sqrt( -2.0 * Math.Log( number1 ) ) * Math.Sin( 2.0 * Math.PI * number2 );

            var returnNumber = mean + stdDev * randStdNormal;
            return ( float )returnNumber;
        }

        #endregion


        #region Fields

        Random _randomNumber;

        #endregion
    }
}
