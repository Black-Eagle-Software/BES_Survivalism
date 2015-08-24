using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace SurvivalismRedux.Factory {
    public class OutputParagraphFactory : Singleton<OutputParagraphFactory> {
        #region Constructors

        protected OutputParagraphFactory() {
            Messenger.Default.Register<PrintMessage>( this, msg => ReceivePrintMessage( msg ) );
            Messenger.Default.Register<ClearOutputParagraphMessage>( this, msg => { _output.Blocks.Clear(); } );
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        internal void RegisterOutputDocument( FlowDocument _content ) {
            if ( _content == null ) return;
            _output = _content;
        }

        private void ReceivePrintMessage( PrintMessage msg ) {
            var p = new Paragraph();
            if ( msg.ParaMessage != null ) {
                p = msg.ParaMessage;
                _output.Blocks.Add( p );
                return;
            }
            if ( msg.TableMessage != null ) {
                _output.Blocks.Add( msg.TableMessage );
                return;
            }
            var r = new Run( msg.Message );
            switch ( msg.Type ) {
                case PrintMessage.MessageType.DEBUG:
                    r = new Run( $"[{DateTime.Now.ToShortTimeString()}] {msg.Message}" );
                    r.Foreground = new SolidColorBrush( Colors.Orange );
                    break;
                case PrintMessage.MessageType.MESSAGE_BOX:
                    r.Foreground = new SolidColorBrush( Colors.Red );
                    break;
                case PrintMessage.MessageType.PRINT:
                    r.Foreground = new SolidColorBrush( Colors.Lime );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            p.Inlines.Add( r );
            if ( _output != null ) {
                _output.Blocks.Add( p );                
            }
            _outputHistory.Add( p );
        }

        #endregion


        #region Fields

        private FlowDocument _output;
        private List<Paragraph> _outputHistory = new List<Paragraph>();

        #endregion

    }
}
