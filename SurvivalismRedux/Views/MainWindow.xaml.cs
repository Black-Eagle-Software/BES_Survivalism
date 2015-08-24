using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurvivalismRedux {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var output = new FlowDocument { LineHeight = 1 };
            rtbOutput.Document = output;
            Messenger.Default.Send( new FlowDocumentInitMessage( this, Guid.NewGuid(), output ) );
        }
    }
}
