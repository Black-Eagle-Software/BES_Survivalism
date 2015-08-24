using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SurvivalismRedux.MessageTypes {
    public class PrintMessage {
        public PrintMessage( string msg, MessageType type = MessageType.PRINT ) {
            Message = msg;
            Type = type;
        }
        public PrintMessage(Paragraph para, MessageType type = MessageType.PRINT ) {
            ParaMessage = para;
            Type = type;
        }
        public PrintMessage(Table table, MessageType type = MessageType.PRINT ) {
            TableMessage = table;
            Type = type;
        }

        public string Message { get; set; }
        public Paragraph ParaMessage { get; set; }
        public Table TableMessage { get; set; }
        public MessageType Type { get; set; }

        public enum MessageType {
            DEBUG,
            MESSAGE_BOX,
            PRINT
        };
    }
}
