using SurvivalismRedux.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SurvivalismRedux.Factory {
    public class PlayerStatOutputFactory : Singleton<PlayerStatOutputFactory> {
        #region Constructors

        protected PlayerStatOutputFactory() { }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public string CreateStringFromPlayerStats( Player player ) {
            //need to summarize what stats changed and by how much            
            var s = $"Name: {player.Name}\tHealth: {OutputStat( player, Stats.HEALTH )}\tSanity: {OutputStat( player, Stats.SANITY )}\tAgility: {OutputStat( player, Stats.AGILITY )}";
            return s;
        }
        public Table CreateTableFromPlayerStats( Player player ) {
            return SummarizeStatsAsTable( player );
        }

        internal Table SummarizeStatsAsTable( Player player ) {
            var t = new Table();
            t.CellSpacing = 0;            
            
            for ( int i = 0; i < columnsCount; i++ ) {
                t.Columns.Add( new TableColumn() );
                t.Columns[ i ].Width = new GridLength( columnWidth );
            }

            var rowGroups = t.RowGroups;
            rowGroups.Add( new TableRowGroup() );
            rowGroups[ 0 ].Rows.Add( new TableRow() );
            var currentRow = rowGroups[ 0 ].Rows[ 0 ];
            //output name with gender color
            currentRow.Cells.Add( new TableCell( OutputPlayerNameWithGenderColor( player ) ) );            
            //output stats list
            var statNames = Enum.GetNames( typeof( Stats ) );
            var statValues = ( Stats[] )Enum.GetValues( typeof( Stats ) );
            var rowCount = statNames.Count() / 2;
            var rowCounter = 1;
            for ( var i = 0; i < statNames.Count(); i++ ) {
                if ( i > 0 && i % columnsCount == 0 ) {
                    rowCounter++;
                }
                if ( rowGroups[ 0 ].Rows.Count() <= rowCounter ) {
                    rowGroups[ 0 ].Rows.Add( new TableRow() );
                }
                currentRow = rowGroups[ 0 ].Rows[ rowCounter ];
                currentRow.Cells.Add( CreateCellFromStatParagraph( player, statValues[ i ] ) );
            }
            return t;
        }

        private TableCell CreateCellFromStatParagraph( Player player, Stats target ) {
            var tc = new TableCell( OutputStatAsParagraph( player, target ) );            
            return tc;
        }

        private Paragraph OutputPlayerNameWithGenderColor(Player player ) {
            var p = new Paragraph();
            var nR = new Run( player.Name );
            nR.Foreground = player.PlayerGender == Gender.MALE ? new SolidColorBrush( Colors.CornflowerBlue ) : new SolidColorBrush( Colors.Pink );
            p.Inlines.Add( "Name: " );
            p.Inlines.Add( nR );
            return p;
        }

        private Paragraph OutputStatAsParagraph( Player player, Stats target ) {
            var p = new Paragraph();

            var pt = player.PlayerStats[ target ];
            var ptR = new Run( pt.ToString() );
            if ( !( pt > 0 ) ) {
                ptR.Foreground = new SolidColorBrush( Colors.Red );
            }

            var tT = Total( player.ModifiedStats, target );
            var tTr = new Run( tT.ToString() );
            if ( tT < 0 ) {
                tTr.Foreground = new SolidColorBrush( Colors.Red );
            }
            p.Inlines.Add( $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase( target.ToString().ToLower() )}: " );
            p.Inlines.Add( ptR );
            p.Inlines.Add( " (" );
            p.Inlines.Add( tTr );
            p.Inlines.Add( ")" );
            return p;
        }

        private string OutputStat( Player player, Stats target ) {
            var tT = Total( player.ModifiedStats, target );
            return $"{player.PlayerStats[ target ]} ({tT})";
        }

        private int Total( IEnumerable<Tuple<Stats, int>> values, Stats target ) {
            return values.Where( t => t.Item1 == target ).Sum( t => t.Item2 );
        }

        #endregion


        #region Fields

        const int columnsCount = 2;
        const int columnWidth = 200;

        #endregion
    }
}
