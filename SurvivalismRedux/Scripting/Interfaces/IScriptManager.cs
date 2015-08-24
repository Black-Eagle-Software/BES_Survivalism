using System.IO;

namespace SurvivalismRedux.Scripting.Interfaces {
    public interface IScriptManager {
        void DoScriptFile( string path );
        void DoStream( Stream s );
        
        void LoadScriptFile( string path );
        void LoadStreamContents( Stream s );
        
        void Main(string globalName);
        void Startup();
    }
}
