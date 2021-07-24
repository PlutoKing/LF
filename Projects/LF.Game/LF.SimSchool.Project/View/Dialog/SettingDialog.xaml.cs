/*──────────────────────────────────────────────────────────────
 * FileName     : SettingDialog.cs
 * Created      : 2021-07-01 17:22:05
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

namespace LF.SimSchool.Project.View
{
    /// <summary>
    /// SettingDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SettingDialog : Window
    {
        #region Fields

        #endregion

        #region Constructors
        public SettingDialog()
        {
            InitializeComponent();

            DataContext = new SettingDialogModel();
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
