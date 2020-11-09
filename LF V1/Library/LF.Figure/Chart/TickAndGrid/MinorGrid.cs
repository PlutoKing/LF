/*──────────────────────────────────────────────────────────────
 * FileName     : MinorGrid
 * Created      : 2019-12-20 19:07:15
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    /// <summary>
    /// 副刻度网格线样式
    /// </summary>
    public class MinorGrid : ICloneable
    {
        #region Fields
        protected bool _isVisible;

        protected float _dashOn;
        protected float _dashOff;
        protected float _lineWidth;

        protected Color _color;
        #endregion

        #region Properties

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }

        /// <summary>
        /// 虚线样式
        /// </summary>
        public float DashOn { get => _dashOn; set => _dashOn = value; }

        /// <summary>
        /// 虚线样式
        /// </summary>
        public float DashOff { get => _dashOff; set => _dashOff = value; }

        /// <summary>
        /// 线宽
        /// </summary>
        public float LineWidth { get => _lineWidth; set => _lineWidth = value; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get => _color; set => _color = value; }

        #endregion

        #region Constructors
        public MinorGrid()
        {
            _dashOn = Default.DashOn;
            _dashOff = Default.DashOff;
            _lineWidth = Default.LineWidth;
            _isVisible = Default.IsVisible;
            _color = Default.Color;
        }

        public MinorGrid(MinorGrid rhs)
        {
            _dashOn = rhs._dashOn;
            _dashOff = rhs._dashOff;
            _lineWidth = rhs._lineWidth;

            _isVisible = rhs._isVisible;

            _color = rhs._color;
        }

        public MinorGrid Clone()
        {
            return new MinorGrid(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }


        #endregion

        #region Methods

        /// <summary>
        /// 绘制网格线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="pixVal"></param>
        /// <param name="topPix"></param>
        internal void Draw(Graphics g, Pen pen, float pixVal, float topPix)
        {
            if (_isVisible)
                g.DrawLine(pen, pixVal, 0.0F, pixVal, topPix);
        }

        internal Pen GetPen(ChartFigure graph, float scaleFactor)
        {
            Pen pen = new Pen(_color,
                        graph.ScaledLineWidth(_lineWidth, scaleFactor));

            if (_dashOff > 1e-10 && _dashOn > 1e-10)
            {
                pen.DashStyle = DashStyle.Custom;
                float[] pattern = new float[2];
                pattern[0] = _dashOn;
                pattern[1] = _dashOff;
                pen.DashPattern = pattern;
            }

            return pen;
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public struct Default
        {
            public static bool IsVisible = false;

            public static float DashOn = 1.0F;
            public static float DashOff = 10.0F;
            public static float LineWidth = 1.0F;

            public static Color Color = Color.Gray;
        }
        #endregion
    }
}
