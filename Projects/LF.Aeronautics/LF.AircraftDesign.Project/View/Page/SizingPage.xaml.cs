/*──────────────────────────────────────────────────────────────
 * FileName     : SizingPage.cs
 * Created      : 2021-06-22 16:52:01
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using LF.AircraftDesign.Project.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LF.AircraftDesign.Project.View
{
    /// <summary>
    /// SizingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SizingPage : UserControl
    {
        #region Fields

        #endregion

        #region Constructors
        public SizingPage()
        {
            InitializeComponent();

            DataContext = new SizingPageModel();
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion
    }
}
