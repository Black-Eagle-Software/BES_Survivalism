using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.MessageTypes {
    public class ExecuteScriptMessage {
        public ExecuteScriptMessage(string filePath, string globalName ) {
            FilePath = filePath;
            GlobalName = globalName;
        }
        public ExecuteScriptMessage(Stream initFileStream, string globalName ) {
            FileStream = initFileStream;
            GlobalName = globalName;
        }

        public string FilePath { get; set; }
        public Stream FileStream { get; set; }
        public string GlobalName { get; set; }
    }
}
