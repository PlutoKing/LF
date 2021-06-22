/*──────────────────────────────────────────────────────────────
 * FileName     : FuelFractionInCruiseWindow.cs
 * Created      : 2021-06-22 18:12:28
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

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

namespace LF.AircraftDesign.Project.View
{
    /// <summary>
    /// FuelFractionInCruiseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FuelFractionInCruiseWindow : Window
    {
        #region Fields

        #endregion

        #region Constructors
        public FuelFractionInCruiseWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        private void Top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}
