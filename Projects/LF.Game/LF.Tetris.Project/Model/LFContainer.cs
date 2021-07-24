/*──────────────────────────────────────────────────────────────
 * FileName     : LFContainer.cs
 * Created      : 2021-06-27 20:18:10
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LF.Tetris.Project.Model
{
    static class LFContainer
    {
        #region Fields
        static public Grid grid;
        static public Grid waitingGrid;
        static public LFBlock WatingBox { get; set; }
        static public LFBlock ActivityBox { get; set; }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods
        /// <summary>
        /// gameover事件
        /// </summary>
        static public event EventHandler OnGameover;

        static public void NewBoxReadyToDown()
        {
            if (grid == null) new Exception("缺少活动区域，必须为容器指定grid值。");
            if (waitingGrid == null) new Exception("缺少等候区域，必须为容器指定waitingGrid值。");

            //if (WatingBox == null) WatingBox = BoxFactory.GetRandomBox(ref grid);

            waitingGrid.Children.Clear();

            if (WatingBox == null) ActivityBox = LFBlockFactory.GetRandomBox(ref grid);
            else ActivityBox = WatingBox;
            ActivityBox.OnBottom += ActivityBox_OnBottom;
            ActivityBox.Ready();
            ActivityBox.AtuoDown();

            WatingBox = LFBlockFactory.GetRandomBox(ref grid);
            waitingGrid.Children.Clear();
            WatingBox.ShowWating(ref waitingGrid);

            if (ActivityBox.ISOverlapping())
            {
                ActivityBox.Pause();
                if (OnGameover != null) OnGameover(null, null);
            }
        }


        /// <summary>
        /// 活动方块到达底部时触发
        /// </summary>
        static public void ActivityBox_OnBottom(object sender, EventArgs e)
        {
            LFResult.GetInstance().CalculateScore(RemoveLine());
            NewBoxReadyToDown();
        }


        static public void Stop()
        {
            if (ActivityBox != null) ActivityBox.StopAction();
            ActivityBox = null;
            WatingBox = null;
            grid.Children.Clear();
            waitingGrid.Children.Clear();
            LFResult.GetInstance().Level = 1;
            LFResult.GetInstance().Score = 0;
        }

        static public void Pause()
        {
            if (ActivityBox != null) ActivityBox.Pause();
        }
        static public void UnPause()
        {
            if (ActivityBox != null) ActivityBox.UnPause();
        }


        /// <summary>
        /// 消层，并返回消除的层数
        /// </summary>
        static int RemoveLine()
        {
            if (grid == null) new Exception("缺少活动区域，必须为容器指定grid值。");

            int[] lineCount = new int[24];
            for (int i = 0; i < 24; i++) lineCount[i] = 0;

            int RemoveLineCount = 0;

            //计算每一行方块总数
            foreach (var r in grid.Children)
            {
                if (r is Rectangle)
                {
                    int x = Convert.ToInt32((r as Rectangle).GetValue(Grid.RowProperty));
                    lineCount[x]++;
                }
            }
            for (int i = 23; i >= 0; i--)
            {
                if (lineCount[i] >= 12)
                {
                    //移除一行小方格
                    for (int j = 0; j < grid.Children.Count; j++)// (var r in mygrid.Children)
                    {
                        if (grid.Children[j] is Rectangle)
                        {
                            if (Convert.ToInt32((grid.Children[j] as Rectangle).GetValue(Grid.RowProperty)) == i + RemoveLineCount)
                            {
                                grid.Children.Remove((grid.Children[j] as Rectangle));
                                j--;
                            }
                        }
                    }

                    //将上面的所有小方格下降一行
                    foreach (var r in grid.Children)
                    {
                        if (r is Rectangle)
                        {
                            if (Convert.ToInt32((r as Rectangle).GetValue(Grid.RowProperty)) < i + RemoveLineCount)
                            {
                                (r as Rectangle).SetValue(Grid.RowProperty, Convert.ToInt32((r as Rectangle).GetValue(Grid.RowProperty)) + 1);
                            }
                        }
                    }

                    //被移除行数加1
                    RemoveLineCount++;
                }
            }
            return RemoveLineCount;
        }
        #endregion
    }
}