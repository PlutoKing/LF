/*──────────────────────────────────────────────────────────────
 * FileName     : Border
 * Created      : 2020-10-16 14:34:40
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
    /// <summary>
    /// 边框
    /// </summary>
    public class Border : ICloneable
    {
        #region Fields
        private bool _isVisible;
        private Pen _linePen;
        private float _inflateFactor;
        #endregion

        #region Properties

        /// <summary>
        /// 边框线条
        /// </summary>
        public Pen Line { get => _linePen; set => _linePen = value; }

        /// <summary>
        /// 膨胀系数
        /// </summary>
        public float InflateFactor { get => _inflateFactor; set => _inflateFactor = value; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }

        #endregion

        #region Constructors
        public Border()
        {
            _isVisible = Default.IsVisible;
            _inflateFactor = Default.InflateFactor;
            _linePen = new Pen(Default.LineColor, Default.LineWidth);
        }

        public Border(bool isVisible, Color color, float width)
        {
            _isVisible = isVisible;
            _inflateFactor = Default.InflateFactor;
            _linePen = new Pen(color, width);
        }

        public Border(Border rhs)
        {
            _isVisible = rhs._isVisible;
            _linePen = (Pen)rhs._linePen.Clone();
            _inflateFactor = rhs._inflateFactor;
        }

        public Border Clone()
        {
            return new Border(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        /// <summary>
        /// 绘制<see cref="Border"/>
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="pane">窗格</param>
        /// <param name="scaleFactor">缩放比例</param>
        /// <param name="area">绘制区域</param>
        public void Draw(Graphics g, RectangleF area, float scaleFactor)
        {
            if (_isVisible)
            {
                RectangleF tmpArea = area;
                float scaledInflate = _inflateFactor * scaleFactor;
                tmpArea.Inflate(scaledInflate, scaledInflate);
                g.DrawRectangle(_linePen, tmpArea.X, tmpArea.Y, tmpArea.Width, tmpArea.Height);
            }
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public struct Default
        {
            public static float InflateFactor = 0.0F;
            public static Color LineColor = Color.Black;
            public static float LineWidth = 1.0f;
            public static bool IsVisible = true;

        }
        #endregion
    }
}
