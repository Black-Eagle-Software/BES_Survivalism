using GalaSoft.MvvmLight.Command;
using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SurvivalismRedux.ViewModels {
    public class DecisionButtonVM : ViewModelExtendedBase {
        #region Constructors

        public DecisionButtonVM( Decision decision ) {
            _decision = decision;
            ButtonText = _decision.CostAndDescription;
            Result = _decision.Result;
        }

        #endregion


        #region Properties

        public string ButtonText {
            get { return _text; }
            set { SetField( ref _text, value ); }
        }
        public Action Result {
            get { return _result; }
            set { SetField( ref _result, value ); }
        }
        public ICommand ResultCommand {
            get {
                return new RelayCommand( () => {
                    if ( ResultCommand == null ) return;
                    OnDidSelectButton();
                    _decision.OnDidSelectDecision();
                    Result();                    
                } );
            }
        }

        protected void OnDidSelectButton() {
            var handler = DidSelectButton;
            if ( handler != null ) {
                handler();
            }
        }

        #endregion


        #region Methods



        #endregion


        #region Fields

        private Decision _decision;
        private string _text;
        private Action _result;

        public delegate void SelectedButton();
        public event SelectedButton DidSelectButton;

        #endregion
    }
}
