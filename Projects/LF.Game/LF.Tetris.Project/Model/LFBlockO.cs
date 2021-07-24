/*──────────────────────────────────────────────────────────────
 * FileName     : LFBlockO.cs
 * Created      : 2021-06-27 20:13:03
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System.Windows.Controls;
using System.Windows.Media;

namespace LF.Tetris.Project.Model
{
    public class LFBlockO:LFBlock
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFBlockO(ref Grid grid)
        {
            this.grid = grid;
            for (int i = 0; i < 4; i++) rectangles[i].Fill = new SolidColorBrush(Colors.DarkOrange);

        }
        #endregion

        #region Methods
        private void ShowAt(Position P, ref Grid grid)
        {
            rectangles[0].SetValue(Grid.ColumnProperty, P.x + 0);
            rectangles[0].SetValue(Grid.RowProperty, P.y + 0);

            rectangles[1].SetValue(Grid.ColumnProperty, P.x + 1);
            rectangles[1].SetValue(Grid.RowProperty, P.y + 0);

            rectangles[2].SetValue(Grid.ColumnProperty, P.x + 0);
            rectangles[2].SetValue(Grid.RowProperty, P.y + 1);

            rectangles[3].SetValue(Grid.ColumnProperty, P.x + 1);
            rectangles[3].SetValue(Grid.RowProperty, P.y + 1);

            for (int i = 0; i < 4; i++) grid.Children.Add(rectangles[i]);
        }

        public override void ShowWating(ref Grid WaingGrid)
        {
            ShowAt(new Position(1, 1), ref WaingGrid);
        }

        public override void Ready()
        {
            ShowAt(new Position(5, 0), ref grid);


            ActivityStatus = new Status();
            ActivityStatus.NextRelativeposition.Add(new Position(0, 0));
            ActivityStatus.NextRelativeposition.Add(new Position(0, 0));
            ActivityStatus.NextRelativeposition.Add(new Position(0, 0));
            ActivityStatus.NextRelativeposition.Add(new Position(0, 0));
            ActivityStatus.NeedCheck.Add(false);
            ActivityStatus.NeedCheck.Add(false);
            ActivityStatus.NeedCheck.Add(false);
            ActivityStatus.NeedCheck.Add(false);
            ActivityStatus.Next = ActivityStatus;

        }
        #endregion
    }
}