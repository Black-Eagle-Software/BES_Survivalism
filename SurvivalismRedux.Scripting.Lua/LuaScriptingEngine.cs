using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Scripting.Attributes;
using SurvivalismRedux.Scripting.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace SurvivalismRedux.Scripting.Lua {
    [ScriptingEngine( "Lua" )]
    public class LuaScriptingEngine : IScriptEngine {
        #region Constructors



        #endregion


        #region Properties



        #endregion


        #region Methods

        private void Main( string initFilePath ) {
            CheckLuaIsReady();
            _scriptMgr.DoScriptFile( initFilePath );

            //var currentPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            //if ( currentPath == null ) return;
            //_scriptMgr.DoScriptFile( Path.Combine( currentPath, "Scripts", "BES_Test", "BES_Test.lua" ) );
            //_scriptMgr.DoScriptFile( Path.Combine( currentPath, "Scripts", "BES_Console", "BES_Console.lua" ) );
            //_scriptMgr.Main();
        }
        private void Main( Stream fileStream ) {
            CheckLuaIsReady();
            _scriptMgr.DoStream( fileStream );
            _scriptMgr.Main();
        }

        public void Startup() {
            InitializeLuaState();

            Messenger.Default.Register<LuaFunctionExecuteMessage>( this, msg => {
                _luaState.GetFunction( msg.Function.ToString() );
            } );
            Messenger.Default.Register<ExecuteScriptMessage>( this, msg => {
                if ( msg.FileStream != null ) {
                    Main( msg.FileStream );
                    return;
                }
                Main( msg.FilePath );
            } );
        }

        private void InitializeLuaState() {
            if ( _luaState == null ) {
                _luaState = new NLua.Lua();
            }

            //disable importing assemblies
            _luaState.DoString( @"import = function () end" );

            //start up the registrar and register the functions
            Messenger.Default.Register<RegisterScriptMethodMessage>( this, msg => this.RegisterFunction( msg.Name, msg.Target, msg.Function ) );
            _funcRegistrar = new LuaFunctionRegistrar();
            _funcRegistrar.ScanForAndRegisterFunctions();
        }

        private void RegisterFunction( string name, object target, MethodBase function ) {
            _luaState.RegisterFunction( name, target, function );
            Console.WriteLine( "Registered function for use by Lua scripts: {0}, {1}, {2}", name, target, function );
        }

        private void CheckLuaIsReady() {
            if ( _luaState == null ) {
                InitializeLuaState();
            }
            if ( _scriptMgr == null ) {
                _scriptMgr = new LuaScriptManager( _luaState );
            }
        }

        #endregion


        #region Fields

        private NLua.Lua _luaState;
        private IScriptFunctionRegistrar _funcRegistrar;
        private IScriptManager _scriptMgr;

        #endregion


    }
}
