using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace SurvivalismRedux.ViewModels {
    public class ViewModelExtendedBase : ViewModelBase {
        #region Constructors



        #endregion

        #region Properties

        public ICommand CloseCommand {
            get {
                return new RelayCommand( () => {
                    if ( this.CloseCommand == null ) return;
                    //this.MessengerInstance.Send( new LogMessage( LogType.DEBUG, string.Format( "[ViewModelExtendedBase.CloseCommand] Closing view: {0}", this ) ) );
                    this.RequestClose( this, EventArgs.Empty );
                } );
            }
        }

        #endregion

        #region Methods

        //taken from http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string propertyName = "" ) {
            if ( EqualityComparer<T>.Default.Equals( field, value ) ) return false;
            field = value;
            if ( propertyName != null ) base.RaisePropertyChanged(propertyName );
            return true;
        }

        public void Close() {
            //this.MessengerInstance.Send( new LogMessage( LogType.DEBUG, string.Format( "[ViewModelExtendedBase.CloseCommand] Closing view: {0}", this ) ) );
            var onRequestClose = this.RequestClose;
            if (onRequestClose != null) {
                onRequestClose( this, EventArgs.Empty );
            }
        }

        #endregion

        #region Fields

        public event EventHandler RequestClose;

        #endregion
    }
}
