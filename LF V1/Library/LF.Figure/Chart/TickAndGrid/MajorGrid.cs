/*──────────────────────────────────────────────────────────────
 * FileName     : MajorGrid
 * Created      : 2019-12-20 19:08:36
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
    /// 主刻度网格线样式
    /// </summary>
    public class MajorGrid : MinorGrid, ICloneable
    {
        #region Fields
        private bool _isZeroLine;
        #endregion

        #region Properties

        /// <summary>
        /// 是否0线
        /// </summary>
        public bool IsZeroLine { get => _isZeroLine; set => _isZeroLine = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MajorGrid()
        {
            _dashOn = Default.DashOn;
            _dashOff = Default.DashOff;
            _lineWidth = Default.LineWidth;
            _isVisible = Default.IsVisible;
            _color = Default.Color;
            _isZeroLine = Default.IsZeroLine;
        }

        public MajorGrid(MajorGrid rhs)
            : base(rhs)
        {
            _isZeroLine = rhs._isZeroLine;
        }

        public new MajorGrid Clone()
        {
            return new MajorGrid(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public new struct Default
        {
            /// <summary>
            /// 是否显示，默认不显示
            /// </summary>
            public static bool IsVisible = true;

            /// <summary>
            /// 是否零线，默认不是
            /// </summary>
            public static bool IsZeroLine = false;

            /// <summary>
            /// 网格默认线宽 1.0f
            /// </summary>
            public static float LineWidth = 1.0F;
            public static float DashOn = 1F;
            public static float DashOff = 0F;

            public static Color Color = Color.LightGray;

        }
        #endregion
    }
}
