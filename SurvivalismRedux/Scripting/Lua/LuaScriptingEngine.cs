using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Scripting.Attributes;
using SurvivalismRedux.Scripting.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Models;
using SurvivalismRedux.Managers;

namespace SurvivalismRedux.Scripting.Lua {
    [ScriptingEngine( "Lua" )]
    public class LuaScriptingEngine : Singleton<LuaScriptingEngine>, IScriptEngine {
        #region Constructors

        protected LuaScriptingEngine() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        private void Main( string initFilePath, string globalName ) {
            CheckLuaIsReady();
            _scriptMgr.DoScriptFile( initFilePath );
            _scriptMgr.Main(globalName);
        }

        private void Main( Stream fileStream, string globalName ) {
            CheckLuaIsReady();
            _scriptMgr.DoStream( fileStream );
            _scriptMgr.Main(globalName);
        }

        public void Startup() {
            InitializeLuaState();

            Messenger.Default.Register<LuaFunctionExecuteMessage>( this, msg => {
                _luaState.GetFunction( msg.Function.ToString() );
            } );
            Messenger.Default.Register<ExecuteScriptMessage>( this, msg => {
                if ( msg.FileStream != null ) {
                    _luaState.NewTable( msg.GlobalName );
                    Main( msg.FileStream, msg.GlobalName );
                    return;
                }
                Main( msg.FilePath, msg.GlobalName );
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
            Messenger.Default.Register<StartScenarioMessage>( this, msg => {
                var tableName = msg.Payload.TocFilename.Remove( msg.Payload.TocFilename.IndexOf( '.' ) );
                //give every scenario a global table to play with
                _luaState.NewTable( tableName );
                foreach ( var fp in msg.Payload.FilePaths ) {
                    Main( fp, tableName );
                }
            } );

            _funcRegistrar = new LuaFunctionRegistrar();
            _funcRegistrar.ScanForAndRegisterFunctions(new object[] {
                new LuaOverrideFunctions(),
                new LuaCustomFunctions()
            } );
            
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
