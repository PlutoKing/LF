/*──────────────────────────────────────────────────────────────
 * FileName     : Chart
 * Created      : 2019-12-20 19:11:55
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    public class Chart
    {
        #region Fields
        private RectangleF _area;
        private Brush _fill;
        private Border _border;
        private bool _isAreaAuto;
        #endregion

        #region Properties

        /// <summary>
        /// 区域
        /// </summary>
        public RectangleF Area { get => _area; set => _area = value; }

        /// <summary>
        /// 填充
        /// </summary>
        public Brush Fill { get => _fill; set => _fill = value; }

        /// <summary>
        /// 边框
        /// </summary>
        public Border Border { get => _border; set => _border = value; }
        public bool IsAreaAuto { get => _isAreaAuto; set => _isAreaAuto = value; }

        #endregion

        #region Constructors
        public Chart()
        {
            _border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderLineWidth);
            _fill = new SolidBrush(Default.BackColor);
        }
        #endregion

        #region Methods

        public void Draw(Graphics g,float scaleFactor)
        {
            g.FillRectangle(_fill, _area);
            _border.Draw(g,_area,scaleFactor);
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public struct Default
        {
            public static Color BorderColor = Color.Black;
            public static Color BackColor = Color.White;
            public static float BorderLineWidth = 1.5f;
            public static bool IsBorderVisible = true;
        }
        #endregion
    }
}
