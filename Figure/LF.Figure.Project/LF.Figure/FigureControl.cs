/*──────────────────────────────────────────────────────────────
 * FileName     : FigureControl
 * Created      : 2020-11-09 11:44:10
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LF.Figure
{
    public partial class FigureControl : UserControl
    {
        #region Fields
        LFPane pane;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public FigureControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            pane = new LFPane(this.ClientRectangle);

            pane.Draw(e.Graphics);
        }
        #endregion
    }
}
