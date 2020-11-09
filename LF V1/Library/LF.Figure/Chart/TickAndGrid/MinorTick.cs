/*──────────────────────────────────────────────────────────────
 * FileName     : MinorTick
 * Created      : 2019-12-20 19:05:17
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
    public class MinorTick : ICloneable
    {
        #region Fields
        protected bool _isOutside;
        protected bool _isInside;
        protected bool _isOpposite;
        protected bool _isCrossOutside;
        protected bool _isCrossInside;

        protected float _lineWidth;
        protected float _size;

        protected Color _color;

        #endregion

        #region Properties

        /// <summary>
        /// 是否坐标区外侧显示
        /// </summary>
        public bool IsOutside { get => _isOutside; set => _isOutside = value; }

        /// <summary>
        /// 是否在坐标区内侧显示
        /// </summary>
        public bool IsInside { get => _isInside; set => _isInside = value; }

        /// <summary>
        /// 是否在边框上显示
        /// </summary>
        public bool IsOpposite { get => _isOpposite; set => _isOpposite = value; }

        /// <summary>
        /// 是否在原点处坐标区外侧显示
        /// </summary>
        public bool IsCrossOutside { get => _isCrossOutside; set => _isCrossOutside = value; }

        /// <summary>
        /// 是否在原点坐标区内侧显示
        /// </summary>
        public bool IsCrossInside { get => _isCrossInside; set => _isCrossInside = value; }

        /// <summary>
        /// 线宽
        /// </summary>
        public float LineWidth { get => _lineWidth; set => _lineWidth = value; }

        /// <summary>
        /// 长度
        /// </summary>
        public float Size { get => _size; set => _size = value; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get => _color; set => _color = value; }

        #endregion

        #region Constructors
        public MinorTick()
        {
            _isOutside = Default.IsOutside;
            _isInside = Default.IsInside;
            _isOpposite = Default.IsOpposite;
            _isCrossOutside = Default.IsCrossOutside;
            _isCrossInside = Default.IsCrossInside;

            _lineWidth = Default.LineWidth;
            _size = Default.Size;
            _color = Default.Color;
        }

        public MinorTick(MinorTick rhs)
        {
            _size = rhs._size;
            _color = rhs._color;
            _lineWidth = rhs._lineWidth;

            this.IsOutside = rhs.IsOutside;
            this.IsInside = rhs.IsInside;
            this.IsOpposite = rhs.IsOpposite;
            _isCrossOutside = rhs._isCrossOutside;
            _isCrossInside = rhs._isCrossInside;
        }

        public MinorTick Clone()
        {
            return new MinorTick(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 绘制刻度线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="pixVal"></param>
        /// <param name="topPix"></param>
        /// <param name="shift"></param>
		internal void Draw(Graphics g, ChartFigure graph, Pen pen, float pixVal, float topPix,
                    float shift, float scaledTic)
        {
            // draw the outside tick
            SmoothingMode smode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;
            if (this.IsOutside)
                g.DrawLine(pen, pixVal, shift, pixVal, shift + scaledTic);

            // draw the cross tick
            if (_isCrossOutside)
                g.DrawLine(pen, pixVal, 0.0f, pixVal, scaledTic);

            // draw the inside tick
            if (this.IsInside)
                g.DrawLine(pen, pixVal, shift, pixVal, shift - scaledTic);

            // draw the inside cross tick
            if (_isCrossInside)
                g.DrawLine(pen, pixVal, 0.0f, pixVal, -scaledTic);

            // draw the opposite tick
            if (this.IsOpposite)
                g.DrawLine(pen, pixVal, topPix, pixVal, topPix + scaledTic);
            g.SmoothingMode = smode;
        }


        public float GetScaledTickSize(float scaleFactor)
        {
            return (float)(_size * scaleFactor);
        }

        internal Pen GetPen(ChartFigure graph,float scaleFactor)
        {
            return new Pen(_color, graph.ScaledLineWidth(_lineWidth, scaleFactor));
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults

        public struct Default
        {
            public static bool IsOutside = false;
            public static bool IsInside = false;
            public static bool IsOpposite = false;
            public static bool IsCrossOutside = false;
            public static bool IsCrossInside = false;
            public static float LineWidth = 1.0f;
            public static float Size = 3.0f;
            public static Color Color = Color.Black;
        }

        #endregion
    }
}
