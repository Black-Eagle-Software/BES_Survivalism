using SurvivalismRedux.Factory;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SurvivalismRedux.ViewModels {
    public class MainWindowVM : ViewModelExtendedBase {
        #region Constructors

        public MainWindowVM() {
            MessengerInstance.Register<FlowDocumentInitMessage>( this, msg => {
                _viewGuid = msg.Id;
                _view = ( MainWindow )msg.View;
                _content = msg.Output;
                Initialize();
            } );
            MessengerInstance.Register<DecisionMessage>( this, msg => {
                ButtonsList.Clear();
                for ( var i = 0; i < msg.DecisionCount; i++ ) {
                    var dbvm = new DecisionButtonVM( msg.Decisions[i] );
                    dbvm.DidSelectButton += Dbvm_DidSelectButton;
                    ButtonsList.Add( dbvm );
                }
                RaisePropertyChanged( () => ButtonsList );
            } );
        }        

        #endregion


        #region Properties

        public ObservableCollection<DecisionButtonVM> ButtonsList {
            get {
                if ( _buttonsList != null ) return _buttonsList;
                _buttonsList = new ObservableCollection<DecisionButtonVM>();
                return _buttonsList;
            }
        }

        #endregion


        #region Methods

        public void Initialize() {
            _opf = OutputParagraphFactory.Instance;
            _opf.RegisterOutputDocument( _content );
        }

        private void Dbvm_DidSelectButton() {
            //one of the buttons in the list was selected, so clear the list();
            ButtonsList.Clear();
            RaisePropertyChanged( () => ButtonsList );
        }

        #endregion


        #region Fields
        
        private Guid _viewGuid;
        private MainWindow _view;
        private FlowDocument _content;
        private OutputParagraphFactory _opf;
        private ObservableCollection<DecisionButtonVM> _buttonsList;

        #endregion
    }
}
