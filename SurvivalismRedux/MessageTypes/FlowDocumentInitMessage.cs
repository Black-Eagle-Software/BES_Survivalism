using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SurvivalismRedux.MessageTypes {
    public class FlowDocumentInitMessage {
        public FlowDocumentInitMessage( object view, Guid id, FlowDocument output ) {
            View = view;
            Id = id;
            Output = output;
        }

        public object View { get; set; }
        public Guid Id { get; set; }
        public FlowDocument Output { get; set; }
    }
}
