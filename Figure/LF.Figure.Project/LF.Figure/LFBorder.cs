/*──────────────────────────────────────────────────────────────
 * FileName     : LFBorder
 * Created      : 2020-11-09 10:22:48
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
    /// 边框
    /// </summary>
    public class LFBorder:LFLineStyle,ICloneable
    {
        #region Fields

        /// <summary>
        /// 膨胀因子
        /// </summary>
        private float _inflateFactor;

        //private float _leftWidth;

        //private float _rightWidth;

        //private float _topWidth;

        //private float _bottomWidth;
        #endregion

        #region Properties
        /// <summary>
        /// 膨胀系数
        /// </summary>
        public float InflateFactor { get => _inflateFactor; set => _inflateFactor = value; }
        
        ///// <summary>
        ///// 左边框线宽
        ///// </summary>
        //public float LeftWidth { get => _leftWidth; set => _leftWidth = value; }
        
        ///// <summary>
        ///// 右边框线宽
        ///// </summary>
        //public float RightWidth { get => _rightWidth; set => _rightWidth = value; }

        ///// <summary>
        ///// 上边框线宽
        ///// </summary>
        //public float TopWidth { get => _topWidth; set => _topWidth = value; }

        ///// <summary>
        ///// 下边框线宽
        ///// </summary>
        //public float BottomWidth { get => _bottomWidth; set => _bottomWidth = value; }

        ///// <summary>
        ///// 配置所有线宽
        ///// </summary>
        //public float All
        //{
        //    set
        //    {
        //        _leftWidth = value;
        //        _rightWidth = value;
        //        _topWidth = value;
        //        _bottomWidth = value;
        //    }
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LFBorder():base()
        {
            _inflateFactor = Default.InflateFactor;
            //_leftWidth = Default.LeftWidth;
            //_rightWidth = Default.RightWidth;
            //_topWidth = Default.TopWidth;
            //_bottomWidth = Default.BottomWidth;
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        public LFBorder(bool isVisible, Color color, float width) : base(color)
        {
            _width = width;
            _isVisible = isVisible;
            _inflateFactor = Default.InflateFactor;
            //_leftWidth = Default.LeftWidth;
            //_rightWidth = Default.RightWidth;
            //_topWidth = Default.TopWidth;
            //_bottomWidth = Default.BottomWidth;
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        public LFBorder(Color color, float width) : this(!color.IsEmpty, color, width)
        {
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFBorder(LFBorder rhs) : base(rhs)
        {
            _inflateFactor = rhs._inflateFactor;
            //_leftWidth = rhs._leftWidth;
            //_rightWidth = rhs._rightWidth;
            //_topWidth = rhs._topWidth;
            //_bottomWidth = rhs._bottomWidth;
        }

        /// <summary>
        /// 新建拷贝函数
        /// </summary>
        /// <returns></returns>
        public new LFBorder Clone()
        {
            return new LFBorder(this);
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }


        #endregion

        #region Methods

        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="pane">窗格</param>
        /// <param name="scaleFactor">缩放因子</param>
        /// <param name="area">边框区域</param>
        public void Draw(Graphics g,LFPane pane,float scaleFactor,RectangleF area)
        {
            if (_isVisible)
            {
                var smode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.None;

                RectangleF tmpArea = area;

                float scaledInflate = (float)(_inflateFactor * scaleFactor);
                tmpArea.Inflate(scaledInflate, scaledInflate);

                using (Pen pen = GetPen(pane, scaleFactor))
                    g.DrawRectangle(pen, tmpArea.X, tmpArea.Y, tmpArea.Width, tmpArea.Height);


                g.SmoothingMode = smode;
            }
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        public new struct Default
        {
            /// <summary>
            /// 默认膨胀系数【0】
            /// </summary>
            public static float InflateFactor = 0.0F;

            ///// <summary>
            ///// 默认左边框线宽【1.0】
            ///// </summary>
            //public static float LeftWidth = 1.0F;

            ///// <summary>
            ///// 默认右边框线宽【1.0】
            ///// </summary>
            //public static float RightWidth = 1.0F;

            ///// <summary>
            ///// 默认上边框线宽【1.0】
            ///// </summary>
            //public static float TopWidth = 1.0F;

            ///// <summary>
            ///// 默认下边框线宽【1.0】
            ///// </summary>
            //public static float BottomWidth = 1.0F;

        }

        #endregion
    }
}
