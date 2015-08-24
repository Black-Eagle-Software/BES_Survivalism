using System;
using System.Reflection;

namespace SurvivalismRedux.Models {
    public class Singleton<T> where T : class {
        protected static T _instance;
        private static object initLock = new object();

        public static T Instance {
            get {
                if ( _instance == null ) {
                    CreateInstance();
                }
                return _instance;
            }
        }

        private static void CreateInstance() {
            lock ( initLock ) {
                if ( _instance == null ) {
                    Type t = typeof( T );
                    //Ensure no public constructors
                    ConstructorInfo[] ctors = t.GetConstructors();
                    if ( ctors.Length > 0 ) {
                        throw new InvalidOperationException( $"{t.Name} has at least one public or accessible constructor, making it impossible to enforce singleton behavior." );
                    }
                    _instance = ( T )Activator.CreateInstance( t, true );
                }
            }
        }
    }
}
