using SurvivalismRedux.Models;
using SurvivalismRedux.Scripting.Attributes;
using SurvivalismRedux.Scripting.Interfaces;
using SurvivalismRedux.Scripting.Lua;
using SurvivalismRedux.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurvivalismRedux.Managers {
    public class ApplicationManager : Singleton<ApplicationManager> {
        #region Constructors

        protected ApplicationManager() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public void Startup() {
            var vm = new MainWindowVM();
            var mw = new MainWindow { DataContext = vm };
            mw.Show();

            //create folders we might need
            CheckForAndCreateAppDataFolders();
            CopyDefaultScenariosToDisk( new[] {
                "BlackEagle_AbandonedFarmhouse"
            } );

            //Zod: "Release the world(scripting) engine!"
            LuaScriptingEngine.Instance.Startup();
            ReadInScenarios();

            //start the game manager
            //on a new thread?!?
            _gm = GameManager.Instance;
            _gm.Initialize();
        }

        public void Shutdown() {
            _gm.EndGame();
        }

        private void CheckForAndCreateAppDataFolders() {
            //create application folder so we can store files
            // Check if folder exists and if not, create it
            if ( !Directory.Exists( _scenarioPath ) ) {
                Directory.CreateDirectory( _scenarioPath );
            }
        }
        private void CopyDefaultScenariosToDisk( string[] names ) {
            var baseResPath = "SurvivalismRedux.Resources.Scripts.Scenarios";
            foreach ( var str in names ) {
                var path = Path.Combine( _scenarioPath, str );
                if ( !Directory.Exists( path ) ) {
                    Directory.CreateDirectory( path );
                }
                //read in the .toc file and get its list of files
                //var lPath = string.Format( "{2}{1}{0}.lua", str, Path.DirectorySeparatorChar, path );
                var tPath = $"{path}{Path.DirectorySeparatorChar}{str}.toc";    //c#6 is sexy!
                //var lRes = string.Format( "SurvivalismRedux.Resources.Scripts.Scenarios.{1}.{0}.lua", str, resFldr );
                var tRes = $"{baseResPath}.{str}.{str}.toc";
                //OutputResourceToFile( lRes, lPath );
                var tocR = new LuaTocReader();
                var sResult = tocR.ReadTocFileFromStream( Assembly.GetExecutingAssembly().GetManifestResourceStream( tRes ), Path.GetFileName( tPath ), $"{_scenarioPath}{Path.DirectorySeparatorChar}{str}" );
                OutputResourceToFile( tRes, tPath );
                foreach ( var fp in sResult.FilePaths ) {
                    var fn = Path.GetFileName( fp );
                    OutputResourceToFile( $"{baseResPath}.{str}.{fn}", fp );
                }
            }
        }

        private void OutputResourceToFile( string resourcePath, string filePath ) {
            using ( var fs = new FileStream( filePath, FileMode.Create, FileAccess.Write ) ) {
                var s = Assembly.GetExecutingAssembly().GetManifestResourceStream( resourcePath );
                if ( s == null ) return;
                s.Seek( 0, SeekOrigin.Begin );
                s.CopyTo( fs );
            }
        }

        private void ReadInScenarios() {
            var scm = ScenarioManager.Instance;
            var ltr = new LuaTocReader();
            foreach ( var p in Directory.EnumerateDirectories( _scenarioPath ) ) {
                var pf = Directory.EnumerateFiles( p );
                var s = ltr.ReadTocFileFromFilePath( pf.First( f => f.EndsWith( "toc" ) ) );
                scm.AddScenario( s );
            }
        }

        #endregion


        #region Fields

        private GameManager _gm;
        private int _currentApiVersion = 100;
        private string _scenarioPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Black Eagle Software", "Survivalism", "Scripts", "Scenarios" );

        #endregion
    }
}
