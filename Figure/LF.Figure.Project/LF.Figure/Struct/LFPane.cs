/*──────────────────────────────────────────────────────────────
 * FileName     : LFPane
 * Created      : 2020-11-09 09:36:58
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
    /// 窗格
    /// 窗格使用图层<see cref="LFLayer"/>管理所有绘图对象<see cref="LFObject"/>
    /// </summary>
    public class LFPane:ICloneable
    {
        #region Fields

        /// <summary>
        /// 图层列表
        /// </summary>
        public readonly List<LFLayer> Layers = new List<LFLayer>();

        /// <summary>
        /// 默认图层
        /// </summary>
        public readonly LFLayer DefaultLayer = new LFLayer();


        /// <summary>
        /// 区域
        /// </summary>
        private RectangleF _area;

        /// <summary>
        /// 边框
        /// </summary>
        private LFBorder _border;

        private LFFillStyle _background;

        private LFLabel _title;

        /// <summary>
        /// 基础尺寸
        /// </summary>
        private float _baseDimension;

        /// <summary>
        /// 字体是否缩放
        /// </summary>
        private bool _isFontScaled;

        /// <summary>
        /// 线宽是否缩放
        /// </summary>
        private bool _isLineWidthScaled;
        #endregion

        #region Properties

        /// <summary>
        /// 区域
        /// </summary>
        public RectangleF Area { get => _area; set => _area = value; }
        
        /// <summary>
        /// 边框线条
        /// </summary>
        public LFBorder Border { get => _border; set => _border = value; }
        
        /// <summary>
        /// 背景填充
        /// </summary>
        public LFFillStyle Background { get => _background; set => _background = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="area"></param>
        public LFPane(RectangleF area)
        {
            _area = area;
            _title = new LFLabel();
            _background = new LFFillStyle();
            _border = new LFBorder();
            _baseDimension = Default.BaseDimension;
            _isFontScaled = Default.IsFontScaled;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFPane (LFPane rhs)
        {
            _area = rhs.Area;
        }

        /// <summary>
        /// 新建拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFPane Clone()
        {
            return new LFPane(this);
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

        #region Constructor Methods

        #endregion

        #region Sizing Methods

        /// <summary>
        /// 计算缩放因子
        /// </summary>
        /// <returns>缩放因子</returns>
        public float GetScaleFactor()
        {
            float scaleFactor;

            const float ASPECTLIMIT = 1.5F; // 长宽比1.5

            if (!_isFontScaled)
                return 1.0f;

            if (_area.Height <= 0)
                return 1.0F;

            float length = _area.Width;
            float aspect = _area.Width / _area.Height;

            if (aspect > ASPECTLIMIT)
                length = _area.Height * ASPECTLIMIT;
            if (aspect < 1.0F / ASPECTLIMIT)
                length = _area.Width * ASPECTLIMIT;

            scaleFactor = length / (_baseDimension * 72F);

            if (scaleFactor < 0.1F)
                scaleFactor = 0.1F;

            return scaleFactor;
        }

        /// <summary>
        /// 计算缩放后的线宽
        /// </summary>
        /// <param name="lineWidth"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public float ScaledLineWidth(float lineWidth,float scaleFactor)
        {
            if (_isLineWidthScaled)
                return lineWidth * scaleFactor;
            else
                return lineWidth;
        }
        #endregion

        #region Rendering Methods

        /// <summary>
        /// 绘制窗格
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
            // 尺寸检测
            if (_area.Width <= 1 || _area.Height <= 1)
                return;

            // 计算缩放因子
            float scaleFactor = GetScaleFactor();

            // 绘制背景和边框
            DrawPaneFrame(g, scaleFactor);

            // 绘制对象
            
        }


        /// <summary>
        /// 绘制窗格框架
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="scaleFactor">缩放因子</param>
        public void DrawPaneFrame(Graphics g,float scaleFactor)
        {
            // 绘制背景
            using (Brush brush = _background.MakeBrush(_area))
            {
                g.FillRectangle(brush, _area);
            }
            // 绘制边框
            RectangleF area = new RectangleF(_area.X, _area.Y, _area.Width - 1, _area.Height - 1);
            _border.Draw(g, this, scaleFactor, area);

            DrawTitle(g, scaleFactor);
        }

        public void DrawTitle(Graphics g,float scaleFactor)
        {
            if (_title.IsVisible)
            {
                _title.BoundingBox(g, scaleFactor);

                _title.Position = new PointF((_area.Left + _area.Right) / 2,
                    _area.Top  + _title.Size.Height / 2.0F);
                _title.Draw(g, this, scaleFactor);
            }
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        /// <summary>
        /// 默认参数
        /// </summary>
        public struct Default
        {
            /// <summary>
            /// 默认标题是否显示【是】
            /// </summary>
            public static bool IsTitleShow = true;

            /// <summary>
            /// 默认标题字体【微软雅黑】
            /// </summary>
            public static string TitleFamily = "微软雅黑";

            /// <summary>
            /// 默认标题字号【16】
            /// </summary>
            public static float TitleSize = 16;

            /// <summary>
            /// 默认标题颜色【黑色】
            /// </summary>
            public static Color TitleColor = Color.Black;

            /// <summary>
            /// 默认标题是否粗体【否】
            /// </summary>
            public static bool TitleBold = false;

            /// <summary>
            /// 默认标题是否斜体【否】
            /// </summary>
            public static bool TitleItalic = false;

            /// <summary>
            /// 默认标题是否下划线【否】
            /// </summary>
            public static bool TitleUnderLine = false;

            /// <summary>
            /// 默认标题行距【0.5倍】
            /// </summary>
            public static float TitleGap = 0.5F;

            /// <summary>
            /// 默认填充颜色【白色】
            /// </summary>
            public static Color FillColor = Color.White;

            /// <summary>
            /// 默认边框是否显示【是】
            /// </summary>
            public static bool IsBorderVisible = true;

            /// <summary>
            /// 默认边框颜色【黑色】
            /// </summary>
            public static Color BorderColor = Color.Black;

            /// <summary>
            /// 默认边框线宽【1】
            /// </summary>
            public static float BorderLineWidth = 1.0F;

            /// <summary>
            /// 默认基础尺寸【8】
            /// </summary>
            public static float BaseDimension = 8.0F;

            /// <summary>
            /// 默认是否缩放线宽【否】
            /// </summary>
            public static bool IsLineWidthScaled = false;

            /// <summary>
            /// 默认是否缩放字体【是】
            /// </summary>
            public static bool IsFontScaled = true;
        }

        #endregion
    }
}
