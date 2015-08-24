using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Scripting.Interfaces;
using System.IO;

namespace SurvivalismRedux.Scripting.Lua {
    public class LuaScriptManager : IScriptManager {
        #region Constructors

        public LuaScriptManager( NLua.Lua luaState ) {
            _lua = luaState;
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public void DoScriptFile( string path ) {
            _lua.DoFile( path );
        }
        public void DoStream( Stream s ) {
            var str = ReadStreamContents( s );
            _lua.DoString( str );
        }

        public void LoadScriptFile( string path ) {
            _lua.LoadFile( path );
        }
        public void LoadStreamContents( Stream s ) {
            var str = ReadStreamContents( s );
            _lua.LoadString( str, "n/a" );
        }

        public void Main( string globalName ) {
            //start the scripts running
            var main = _lua.GetFunction( string.Format( "{0}.Main", globalName ) );
            if ( main == null ) return;
            main.Call();
        }

        public void Startup() {
            throw new NotImplementedException();
        }

        private string ReadStreamContents( Stream s ) {
            s.Seek( 0, SeekOrigin.Begin );
            string result;
            using ( var sr = new StreamReader( s ) ) {
                result = sr.ReadToEnd();
            }
            if ( string.IsNullOrEmpty( result ) ) {
                Console.WriteLine( "Stream was null or empty" );
                return null;
            }
            return result;
        }

        #endregion


        #region Fields

        private readonly NLua.Lua _lua;

        #endregion

    }
}
