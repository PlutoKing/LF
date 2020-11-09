/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindow
 * Created      : 2020-11-01 16:11:59
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System.Windows;
using LF.Figure;
using System.Drawing;
using LF.FlightControl;
using System.Windows.Forms;


namespace LF.FlightControl.Project
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

            map.Figure.Broder.IsVisible = true;
            map.Figure.Broder.Line.Color = Color.FromArgb(172, 172, 172);
            map.Figure.XAxis.Title.Text = "x/m";
            map.Figure.YAxis.Title.Text = "y/m";
            map.Figure.XAxis.Scale.Max = 1000;
            map.Figure.YAxis.Scale.Max = 1000;

            map.Figure.IsAxisEqual = true;
            map.Figure.XAxis.Scale.MajorStep = 100;
            map.Figure.XAxis.Scale.MajorStepAuto = false;
            map.Figure.YAxis.Scale.MajorStep = 100;
            map.Refresh();
        }
        #endregion

        #region Methods
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
