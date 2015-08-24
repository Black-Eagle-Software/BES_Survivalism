using SurvivalismRedux.Models;
using SurvivalismRedux.Scripting.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalismRedux.Scripting.Lua {
    public class LuaTocReader : IScriptTocReader {
        #region Constructors

        public LuaTocReader() {
            _tags = Enum.GetNames( typeof( Tags ) );
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public Scenario ReadTocFileFromStream(Stream file, string tocFilename, string expectedPath ) {
            var lines = new List<string>();
            using ( var sr = new StreamReader( file ) ) {
                while ( !sr.EndOfStream ) {
                    lines.Add( sr.ReadLine() );
                }
            }
            var result = new Scenario();
            result.TocFilename = tocFilename;
            //this will get messy
            var files = new List<string>();
            foreach ( var line in lines ) {
                if ( line.Length == 0 ) continue;
                if ( line.StartsWith( "##" ) ) {
                    //found a header line
                    CheckHeaderTag( line, result );
                } else if ( line.StartsWith( "#" ) ) {
                    //should have found a comment line, ignore it
                    break;
                } else {
                    //found a filePath line
                    files.Add( $"{expectedPath}{Path.DirectorySeparatorChar}{line}" );
                }
            }
            result.FilePaths = files.ToArray();
            return result;
        }

        public Scenario ReadTocFileFromFilePath( string filePath ) {
            if ( string.IsNullOrEmpty( filePath ) ) return null;
            var lines = File.ReadAllLines( filePath );
            var result = new Scenario();
            result.TocFilename = Path.GetFileName( filePath );
            //this will get messy
            var files = new List<string>();
            foreach ( var line in lines ) {
                if ( line.Length == 0 ) continue;
                if ( line.StartsWith( "##" ) ) {
                    //found a header line
                    CheckHeaderTag( line, result );
                } else if ( line.StartsWith( "#" ) ) {
                    //should have found a comment line, ignore it
                    break;
                } else {
                    //found a filePath line
                    var lPath = Directory.GetParent( filePath );
                    files.Add( $"{lPath}{Path.DirectorySeparatorChar}{line}" );
                }
            }
            result.FilePaths = files.ToArray();
            return result;
        }

        private void CheckHeaderTag( string line, Scenario scenario ) {
            //first, clean away the cruft
            var ss = line.TrimStart( '#' );
            ss = ss.Trim();
            //now, find out what the tag is
            var colonIndex = ss.IndexOf( ':' );
            var tag = ss.Substring( 0, colonIndex );
            var value = ss.Substring( colonIndex + 1 );
            if ( _tags.Any( t => t.Equals( tag, StringComparison.OrdinalIgnoreCase ) ) ) {
                //found a tag, so pull out the value
                var tagTag = ( Tags )Enum.Parse( typeof( Tags ), tag );
                switch ( tagTag ) {
                    case Tags.API_Version:
                        scenario.ApiVersion = int.Parse( value );
                        break;
                    case Tags.Author:
                        scenario.Author = value;
                        break;
                    case Tags.Dependencies:
                        //NYI
                        break;
                    case Tags.MinimumHeadCount:
                        //NYI
                        break;
                    case Tags.Description:
                        scenario.Description = value;
                        break;
                    case Tags.SavedVariables:
                        //NYI
                        break;
                    case Tags.Storylines:
                        //NYI
                        break;
                    case Tags.Title:
                        scenario.Title = value;
                        break;
                    case Tags.Version:
                        scenario.Version = value;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion


        #region Fields

        private string[] _tags;

        #endregion
    }
}
