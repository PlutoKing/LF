/*──────────────────────────────────────────────────────────────
 * FileName     : LFLineStyle
 * Created      : 2020-11-09 10:06:31
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
using System.Drawing.Drawing2D;

namespace LF.Figure
{
    /// <summary>
    /// 线条样式
    /// </summary>
    public class LFLineStyle:ICloneable
    {
        #region Fields
        /// <summary>
        /// 是否可见
        /// </summary>
        protected bool _isVisible;

        /// <summary>
        /// 颜色
        /// </summary>
        protected Color _color;

        /// <summary>
        /// 线宽
        /// </summary>
        protected float _width;

        /// <summary>
        /// 样式
        /// </summary>
        protected DashStyle _style;

        /// <summary>
        /// 短划线样式参数
        /// </summary>
        protected float _dashOn;

        /// <summary>
        /// 短划线样式参数
        /// </summary>
        protected float _dashOff;

        /// <summary>
        /// 是否抗锯齿
        /// </summary>
        protected bool _isAntiAlias;

        #endregion

        #region Properties

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get => _color; set => _color = value; }
        
        /// <summary>
        /// 线宽
        /// </summary>
        public float Width { get => _width; set => _width = value; }
        
        /// <summary>
        /// 样式
        /// </summary>
        public DashStyle Style { get => _style; set => _style = value; }
        
        /// <summary>
        /// 短划线参数
        /// </summary>
        public float DashOn { get => _dashOn; set => _dashOn = value; }

        /// <summary>
        /// 短划线参数
        /// </summary>
        public float DashOff { get => _dashOff; set => _dashOff = value; }

        /// <summary>
        /// 是否抗锯齿
        /// </summary>
        public bool IsAntiAlias { get => _isAntiAlias; set => _isAntiAlias = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LFLineStyle() : this(Color.Empty)
        {
        }
        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="color"></param>
        public LFLineStyle(Color color)
        {
            _isVisible = Default.IsVisible;
            _color = color.IsEmpty ? Default.Color : color;

            _width = Default.Width;

            _style = Default.Style;
            _dashOn = Default.DashOn;
            _dashOff = Default.DashOff;

            _isAntiAlias = Default.IsAntiAlias;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFLineStyle(LFLineStyle rhs)
        {
            _isVisible = rhs._isVisible;
            _color = rhs._color;

            _width = rhs._width;
            _style = rhs._style;
            _dashOn = rhs._dashOn;
            _dashOff = rhs._dashOff;

            _isAntiAlias = rhs._isAntiAlias;
        }

        /// <summary>
        /// 新建拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFLineStyle Clone()
        {
            return new LFLineStyle(this);
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return  this.Clone();
        }
        #endregion

        #region Methods

        /// <summary>
        /// 根据样式获取画笔
        /// </summary>
        /// <param name="pane">窗格</param>
        /// <param name="scaleFactor">缩放因子</param>
        /// <returns></returns>
        public Pen GetPen(LFPane pane,float scaleFactor)
        {
            // 新建画笔
            Pen pen = new Pen(_color, pane.ScaledLineWidth(_width, scaleFactor));

            // 配置画笔样式
            pen.DashStyle = _style;

            if(_style == DashStyle.Custom)
            {
                if (_dashOff > 1e-10 && _dashOn > 1e-10)
                {
                    pen.DashStyle = DashStyle.Custom;
                    float[] pattern = new float[2];
                    pattern[0] = _dashOn;
                    pattern[1] = _dashOff;
                    pen.DashPattern = pattern;
                }
                else
                    pen.DashStyle = DashStyle.Solid;
            }

            return pen;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        public struct Default
        {
            /// <summary>
            /// 默认是否显示【是】
            /// </summary>
            public static bool IsVisible = true;

            /// <summary>
            /// 默认线条颜色【黑色】
            /// </summary>
            public static Color Color = Color.Black;

            /// <summary>
            /// 默认线宽【1.0】
            /// </summary>
            public static float Width = 1.0F;

            /// <summary>
            /// 默认样式【实线】
            /// </summary>
            public static DashStyle Style = DashStyle.Solid;

            /// <summary>
            /// 默认短划线参数【1.0】
            /// </summary>
            public static float DashOn = 1.0F;

            /// <summary>
            /// 默认短划线参数【1.0】
            /// </summary>
            public static float DashOff = 1.0F;

            /// <summary>
            /// 默认是否抗锯齿【否】
            /// </summary>
            public static bool IsAntiAlias = false;
        }

        #endregion
    }
}
