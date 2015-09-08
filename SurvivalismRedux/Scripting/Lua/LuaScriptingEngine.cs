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
    [ScriptingEngine("Lua")]
    public class LuaScriptingEngine : Singleton<LuaScriptingEngine>, IScriptEngine {
        #region Constructors

        protected LuaScriptingEngine() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        private void Main(string initFilePath, string globalName) {
            this.CheckLuaIsReady();
            this._scriptMgr.DoScriptFile(initFilePath);
            this._scriptMgr.Main(globalName);
        }

        private void Main(Stream fileStream, string globalName) {
            this.CheckLuaIsReady();
            this._scriptMgr.DoStream(fileStream);
            this._scriptMgr.Main(globalName);
        }

        public void Startup() {
            this.InitializeLuaState();

            Messenger.Default.Register<LuaFunctionExecuteMessage>(this, msg => {
                this._luaState.GetFunction(msg.Function.ToString());
            });
            Messenger.Default.Register<ExecuteScriptMessage>(this, msg => {
                if (msg.FileStream != null) {
                    this._luaState.NewTable(msg.GlobalName);
                    this.Main(msg.FileStream, msg.GlobalName);
                    return;
                }
                this.Main(msg.FilePath, msg.GlobalName);
            });
        }

        private void InitializeLuaState() {
            if (this._luaState == null) {
                this._luaState = new NLua.Lua();
            }

            //disable importing assemblies
            this._luaState.DoString(@"import = function () end");

            //start up the registrar and register the functions
            Messenger.Default.Register<RegisterScriptMethodMessage>(this, msg => this.RegisterFunction(msg.Name, msg.Target, msg.Function));
            Messenger.Default.Register<StartScenarioMessage>(this, msg => {
                var tableName = msg.Payload.TocFilename.Remove(msg.Payload.TocFilename.IndexOf('.'));
                //give every scenario a global table to play with
                this._luaState.NewTable(tableName);
                foreach (var fp in msg.Payload.FilePaths) {
                    this.Main(fp, tableName);
                }
            });

            this._funcRegistrar = new LuaFunctionRegistrar();
            this._funcRegistrar.ScanForAndRegisterFunctions(new object[] {
                new LuaOverrideFunctions(),
                new LuaCustomFunctions()
            });

        }

        private void RegisterFunction(string name, object target, MethodBase function) {
            this._luaState.RegisterFunction(name, target, function);
            Console.WriteLine($"Registered function for use by Lua scripts: {name}, {target}, {function}");
        }

        private void CheckLuaIsReady() {
            if (this._luaState == null) {
                this.InitializeLuaState();
            }
            if (this._scriptMgr == null) {
                this._scriptMgr = new LuaScriptManager(this._luaState);
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
