/*──────────────────────────────────────────────────────────────
 * FileName     : Margin
 * Created      : 2020-10-16 14:36:12
 * Author       : Xu Zhe
 * Description  : 边距
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
    /// 边缘间距
    /// </summary>
    public class Margin : ICloneable
    {
        #region Fields
        private float _left;
        private float _right;
        private float _top;
        private float _bottom;
        #endregion

        #region Properties

        /// <summary>
        /// <see cref="Margin"/>左侧侧边距
        /// </summary>
        public float Left { get => _left; set => _left = value; }

        /// <summary>
        /// <see cref="Margin"/>右侧侧边距
        /// </summary>
        public float Right { get => _right; set => _right = value; }

        /// <summary>
        /// <see cref="Margin"/>上侧侧边距
        /// </summary>
        public float Top { get => _top; set => _top = value; }

        /// <summary>
        /// <see cref="Margin"/>下侧侧边距
        /// </summary>
        public float Bottom { get => _bottom; set => _bottom = value; }

        /// <summary>
        /// 快速设定等侧边距
        /// </summary>
        public float All
        {
            set
            {
                _left = value;
                _right = value;
                _top = value;
                _bottom = value;
            }
        }
        #endregion

        #region Constructors
        public Margin()
        {
            _left = Default.Left;
            _right = Default.Right;
            _top = Default.Top;
            _bottom = Default.Bottom;
        }

        public Margin(float all)
        {
            All = all;
        }

        public Margin(float left, float right, float top, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public Margin(Margin rhs)
        {
            _left = rhs._left;
            _right = rhs._right;
            _top = rhs._top;
            _bottom = rhs._bottom;
        }

        public Margin Clone()
        {
            return new Margin(this);
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
        /// <summary>
        /// <see cref="Margin"/>默认参数结构体
        /// </summary>
        public struct Default
        {
            public static float Left = 20;
            public static float Right = 20;
            public static float Top = 20;
            public static float Bottom = 20;
        }
        #endregion
    }
}
