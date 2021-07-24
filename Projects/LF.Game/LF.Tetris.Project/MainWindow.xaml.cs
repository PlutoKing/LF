/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindow.cs
 * Created      : 2021-06-26 17:13:20
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using LF.Tetris.Project.Model;
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
using System.Windows.Shapes;

namespace LF.Tetris.Project
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            LFContainer.grid = Grid1;
            LFContainer.waitingGrid = grid3;
            DataContext = LFResult.GetInstance();
            LFContainer.OnGameover += OnGameover;

        }
        #endregion

        #region Methods

        #endregion

        #region Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    LFContainer.ActivityBox.ChangeShape();
                    break;
                case Key.Left:
                    LFContainer.ActivityBox.MoveLeft();
                    break;
                case Key.Right:
                    LFContainer.ActivityBox.MoveRight();
                    break;
                case Key.Down:
                    LFContainer.ActivityBox.MoveDown();
                    break;
                case Key.Space:
                    LFContainer.ActivityBox.FastDown();
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (LFContainer.ActivityBox == null) LFContainer.NewBoxReadyToDown();
            else
            {
                LFContainer.Pause();

                if (MessageBox.Show("当前游戏还在进行中，您是否重新开始新游戏?", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LFContainer.Stop();
                    button2.Content = "暂停";
                    LFContainer.NewBoxReadyToDown();
                }
                else
                {
                    if (button2.Content.ToString() == "暂停") LFContainer.UnPause();
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "暂停")
            {
                LFContainer.Pause();
                (sender as Button).Content = "取消暂停";
            }
            else
            {
                LFContainer.UnPause();
                (sender as Button).Content = "暂停";
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            LFContainer.Pause();
            if (MessageBox.Show("您是否结束游戏?", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LFContainer.Stop();
                button2.Content = "暂停";
            }
            else
            {
                if (button2.Content.ToString() == "暂停") LFContainer.UnPause();
            }
        }

        static private void OnGameover(object sender, EventArgs e)
        {
            MessageBox.Show("游戏结束！");
        }

        #endregion
    }
}
