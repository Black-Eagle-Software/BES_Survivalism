using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Scripting.Attributes;
using SurvivalismRedux.Scripting.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace SurvivalismRedux.Scripting.Lua {
    //taken from http://www.gamedev.net/page/resources/_/technical/game-programming/using-lua-with-c-r2275
    public class LuaFunctionRegistrar : IScriptFunctionRegistrar {
        #region Constructors

        public LuaFunctionRegistrar() {
            _functions = new Hashtable();
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public void RegisterFunction( object target ) {
            var targetType = target.GetType();
            foreach ( var info in targetType.GetMethods( ) ) {
                if ( Attribute.GetCustomAttributes( info ).Any( a => a.GetType() == typeof( RegisterScriptFunctionAttribute ) ) ) {
                    try {
                        foreach ( var attribute in Attribute.GetCustomAttributes( info ) ) {
                            if ( attribute.GetType() == typeof ( RegisterScriptFunctionAttribute ) ) {
                                var attr = ( RegisterScriptFunctionAttribute ) attribute;
                                var parameters = new ArrayList();
                                var parameterDocs = new ArrayList();

                                // Now get the expected parameters from the MethodInfo object
                                var parameterInfo = info.GetParameters();

                                // If they don't match, someone forgot to add some documentation to the
                                // attribute, complain and go to the next method
                                if ( attr.FunctionParameters != null && ( parameterInfo.Length != attr.FunctionParameters.Length ) ) {
                                    //throw an error about missing information
                                    Console.WriteLine( "Function " + info.Name + " (exported as " + attr.FunctionName + ") argument number mismatch.  Declared " + attr.FunctionParameters.Length + " but requires " + parameterInfo.Length + "." );
                                    break;
                                }
                                //Build a parameter <-> parameter doc hashtable
                                for ( var i = 0; i < parameterInfo.Length; i++ ) {
                                    parameters.Add( parameterInfo[ i ].Name );
                                    parameterDocs.Add( attr.FunctionParameters[ i ] );
                                }

                                // Get a new function descriptor from this information
                                var descriptor = new LuaFunctionDescriptor( attr.FunctionName, attr.FunctionDescription, parameters, parameterDocs );

                                // Add it to the global hashtable
                                _functions.Add( attr.FunctionName, descriptor );

                                // And tell the VM to register it.
                                Messenger.Default.Send( new RegisterScriptMethodMessage( attr.FunctionName, target, info ) );
                            }
                        }
                    } catch ( Exception ex ) {
                        Console.WriteLine( "[LuaFunctionRegistrar.RegisterFunction({0})] Exception caught: {1}", target, ex.InnerException );
                    }
                }
            }
        }

        public void ScanForAndRegisterFunctions() {
            //taken from http://www.jkfill.com/2010/12/29/self-registering-factories-in-c-sharp/
            //this is nice, but probably not needed, should just load by hand
            var currentPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            if ( currentPath == null ) return;
            foreach ( var file in Directory.EnumerateFiles( currentPath, "*.dll" ) ) {
                try {
                    var asm = Assembly.LoadFrom( file );
                    var types = from t in asm.GetTypes()
                                where t.IsClass
                                select t;
                    foreach ( var type in types ) {
                        var attributes = type.GetCustomAttributes( typeof( ScriptFunctionClassAttribute ), false );
                        foreach ( ScriptFunctionClassAttribute attribute in attributes ) {
                            //_messenger.Send( new LogMessage( LogType.INFO, string.Format( "Loaded module: {0}, {1}", attribute.ModuleName, attribute.ModuleDescription ) ) );
                            var c = asm.CreateInstance( type.FullName, true );
                            if ( c != null ) {
                                //_renderMgr = c;
                                RegisterFunction( c );
                            }
                        }
                        //RegisterFunction( type );
                    }
                } catch ( Exception ex ) {
                    //_messenger.Send( new LogMessage( LogType.ERROR_EXCEPTION, ex  ) );
                    Console.WriteLine( ex.InnerException );
                }
            }
        }

        #endregion


        #region Fields

        private readonly Hashtable _functions;

        #endregion


    }
}
