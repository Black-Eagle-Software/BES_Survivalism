using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SurvivalismRedux.ViewModels;
using SurvivalismRedux.Managers;
using System.Threading;

namespace SurvivalismRedux {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        #region Constructors



        #endregion


        #region Properties



        #endregion


        #region Methods

        private void Application_Startup( object sender, StartupEventArgs e ) {
            _appMgr = ApplicationManager.Instance;
            _appMgr.Startup();
        }

        private void Application_Exit( object sender, ExitEventArgs e ) {
            _appMgr.Shutdown();
        }

        #endregion


        #region Fields

        private ApplicationManager _appMgr;

        #endregion

        
    }
}
