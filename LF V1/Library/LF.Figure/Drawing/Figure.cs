/*──────────────────────────────────────────────────────────────
 * FileName     : Figure
 * Created      : 2020-10-16 11:58:18
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;

namespace LF.Figure
{
    /// <summary>
    /// 图：基类
    /// </summary>
    public class Figure:ICloneable
    {
        #region Fields
        protected object _tag;
        protected RectangleF _area;

        protected Margin _margin;
        protected Brush _fillbrush;
        protected Border _border;
        protected Label _title;

        protected bool _isFontScaled;         // 是否字体缩放
        protected bool _isLineWidthScaled;    // 是否线宽缩放
        protected float _baseDimension;       // 
        protected float _titleGap;            // 
        #endregion

        #region Properties
        public object Tag { get => _tag; set => _tag = value; }
        public RectangleF Area { get => _area; set => _area = value; }
        public Margin Margin { get => _margin; set => _margin = value; }
        public Brush Fillbrush { get => _fillbrush; set => _fillbrush = value; }
        public Border Broder { get => _border; set => _border = value; }
        public Label Title { get => _title; set => _title = value; }
        public bool IsFontScaled { get => _isFontScaled; set => _isFontScaled = value; }
        public bool IsLineWidthScaled { get => _isLineWidthScaled; set => _isLineWidthScaled = value; }
        public float BaseDimension { get => _baseDimension; set => _baseDimension = value; }
        public float TitleGap { get => _titleGap; set => _titleGap = value; }

        #endregion

        #region Constructors
        public Figure(string title, RectangleF area)
        {
            _area = area;

            _margin = new Margin();
            _fillbrush = new SolidBrush(Default.BackColor);
            _border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderLineWidth);

            _isFontScaled = Default.IsFontScaled;
            _isLineWidthScaled = Default.IsLineWidthScaled;
            _baseDimension = Default.BaseDimension;

            _tag = null;

            _titleGap = Default.TitleGap;
            _title = new Label(title, Default.FontFamily,
                Default.FontSize, Default.FontColor, Default.FontBold,
                Default.FontItalic, Default.FontUnderline);
            _title._fontSpec.Border.IsVisible = false;
            _title._fontSpec.FillBrush = new SolidBrush(Default.BackColor);
        }

        public Figure()
            : this("", new RectangleF(0, 0, 0, 0))
        {
        }

        public Figure(Figure rhs)
        {
            _area = rhs.Area;

            _margin = rhs._margin.Clone();
            _fillbrush = (Brush)rhs._fillbrush.Clone();
            _border = rhs._border.Clone();
            _isFontScaled = rhs._isFontScaled;
            _isLineWidthScaled = rhs._isLineWidthScaled;
            _baseDimension = rhs._baseDimension;

            if (rhs._tag is ICloneable)
                _tag = ((ICloneable)rhs._tag).Clone();
            else
                _tag = rhs._tag;

            _titleGap = rhs._titleGap;

            _title = rhs._title.Clone();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException("Can't clone an abstract base type -- child types must implement ICloneable");
        }

        public Figure ShallowClone()
        {
            // return a shallow copy
            return this.MemberwiseClone() as Figure;
        }
        #endregion

        #region Methods

        public virtual void Draw(Graphics g)
        {
            // 尺寸检测
            if (_area.Width <= 1 || _area.Height <= 1)
                return;
            float scaleFactor = this.GetScaleFactor();

            // 绘制底纹和边框
            DrawGraphFrame(g, scaleFactor);

            //// 设定绘制区域
            g.SetClip(_area);

            DrawTitle(g, scaleFactor);

            g.ResetClip();

        }

        /// <summary>
        /// 绘制图框架：底纹和边框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        public void DrawGraphFrame(Graphics g, float scaleFactor)
        {
            g.FillRectangle(_fillbrush, _area);
            RectangleF rect = new RectangleF(_area.X, _area.Y, _area.Width - 1, _area.Height - 1);

            _border.Draw(g, rect, scaleFactor);
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        public void DrawTitle(Graphics g, float scaleFactor)
        {
            // 是否显示
            if (_title._isVisible)
            {
                SizeF size = _title._fontSpec.BoundingBox(g, _title._text, scaleFactor);

                _title._fontSpec.Draw(g, _title._text,
                    (_area.Left + _area.Right) / 2,
                    _area.Top + _margin.Top * (float)scaleFactor + size.Height / 2.0F,
                    AlignH.Center, AlignV.Center, scaleFactor);
            }
        }

        /// <summary>
        /// 尺寸改变
        /// </summary>
        /// <param name="area"></param>
        public virtual void ReSize(RectangleF area)
        {
            _area = area;
        }

        /// <summary>
        /// 计算缩放因数
        /// </summary>
        /// <returns></returns>
        public float GetScaleFactor()
        {
            float scaleFactor;
            const float ASPECTLIMIT = 1.5F;

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
        /// 计算线宽
        /// </summary>
        /// <param name="penWidth"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public float ScaledLineWidth(float penWidth, float scaleFactor)
        {
            if (_isLineWidthScaled)
                return (float)(penWidth * scaleFactor);
            else
                return penWidth;
        }

        /// <summary>
        /// 获取用户区域
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public RectangleF GetClientArea(Graphics g, float scaleFactor)
        {
            // get scaled values for the paneGap and character height
            //float scaledOuterGap = (float) ( Default.OuterPaneGap * scaleFactor );
            float charHeight = _title._fontSpec.GetHeight(scaleFactor);

            // chart rect starts out at the full pane rect.  It gets reduced to make room for the legend,
            // scales, titles, etc.
            RectangleF innerRect = new RectangleF(
                            _area.Left + _margin.Left * scaleFactor,
                            _area.Top + _margin.Top * scaleFactor,
                            _area.Width - scaleFactor * (_margin.Left + _margin.Right),
                            _area.Height - scaleFactor * (_margin.Top + _margin.Bottom));

            // Leave room for the title
            if (_title._isVisible && _title._text != string.Empty)
            {
                SizeF titleSize = _title._fontSpec.BoundingBox(g, _title._text, scaleFactor);
                // Leave room for the title height, plus a line spacing of charHeight * _titleGap
                innerRect.Y += titleSize.Height + charHeight * _titleGap;
                innerRect.Height -= titleSize.Height + charHeight * _titleGap;
            }

            // Getulate the legend rect, and back it out of the current ChartRect
            //this.legend.GetRect( g, this, scaleFactor, ref innerRect );

            return innerRect;
        }

        #region Image Methods
        public Bitmap GetImage()
        {
            return GetImage(false);
        }

        public Bitmap GetImage(bool isAntiAlias)
        {
            Bitmap bitmap = new Bitmap((int)_area.Width, (int)_area.Height);
            using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
            {
                bitmapGraphics.TranslateTransform(-_area.Left, -_area.Top);
                this.Draw(bitmapGraphics);
            }

            return bitmap;
        }

        public Bitmap GetImage(int width, int height, float dpi, bool isAntiAlias)
        {
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(dpi, dpi);
            using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
            {
                MakeImage(bitmapGraphics, width, height, isAntiAlias);
            }

            return bitmap;
        }

        public Bitmap GetImage(int width, int height, float dpi)
        {
            return GetImage(width, height, dpi, false);
        }

        internal void SetAntiAliasMode(Graphics g, bool isAntiAlias)
        {
            if (isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
        }

        private void MakeImage(Graphics g, int width, int height, bool antiAlias)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            SetAntiAliasMode(g, antiAlias);

            // This is actually a shallow clone, so we don't duplicate all the data, curveLists, etc.
            Figure tempPane = this.ShallowClone();

            // Clone the Chart object for ChartPanes so we don't mess up the minPix and maxPix values or
            // the rect/ChartRect calculations of the original
            //RectangleF saveRect = new RectangleF();
            //if ( this is ChartFigure )
            //	saveRect = ( this as ChartFigure ).Chart.Rect;

            tempPane.ReSize(new RectangleF(0, 0, width, height));

            tempPane.Draw(g);

            //if ( this is ChartFigure )
            //{
            //	ChartFigure gPane = this as ChartFigure;
            //	gPane.Chart.Rect = saveRect;
            //	gPane.XAxis.Scale.SetupScaleData( gPane, gPane.XAxis );
            //	foreach ( Axis axis in gPane.YAxisList )
            //		axis.Scale.SetupScaleData( gPane, axis );
            //	foreach ( Axis axis in gPane.Y2AxisList )
            //		axis.Scale.SetupScaleData( gPane, axis );
            //}

            // To restore all the various state variables, you must redraw the graph in it's
            // original form.  For this we create a 1x1 bitmap (it doesn't matter what size you use,
            // since we're only mimicing the draw.  If you use the 'bitmapGraphics' already created,
            // then you will be drawing back into the bitmap that will be returned.

            Bitmap bm = new Bitmap(1, 1);
            using (Graphics bmg = Graphics.FromImage(bm))
            {
                this.ReSize(this.Area);
                SetAntiAliasMode(bmg, antiAlias);
                this.Draw(bmg);
            }
        }

        public Metafile GetMetafile(int width, int height, bool isAntiAlias)
        {
            Bitmap bm = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bm))
            {
                IntPtr hdc = g.GetHdc();
                Stream stream = new MemoryStream();
                Metafile metafile = new Metafile(stream, hdc, _area,
                            MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
                g.ReleaseHdc(hdc);

                using (Graphics metafileGraphics = Graphics.FromImage(metafile))
                {
                    //metafileGraphics.TranslateTransform( -_area.Left, -_area.Top );
                    metafileGraphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                    PointF P = new PointF(width, height);
                    PointF[] PA = new PointF[] { P };
                    metafileGraphics.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, PA);
                    //metafileGraphics.PageScale = 1f;

                    // output
                    MakeImage(metafileGraphics, width, height, isAntiAlias);
                    //this.Draw( metafileGraphics );

                    return metafile;
                }
            }
        }

        public Metafile GetMetafile(int width, int height)
        {
            return GetMetafile(width, height, false);
        }

        public Metafile GetMetafile()
        {
            Bitmap bm = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bm))
            {
                IntPtr hdc = g.GetHdc();
                Stream stream = new MemoryStream();
                Metafile metafile = new Metafile(stream, hdc, _area,
                            MetafileFrameUnit.Pixel, EmfType.EmfOnly);

                using (Graphics metafileGraphics = Graphics.FromImage(metafile))
                {
                    metafileGraphics.TranslateTransform(-_area.Left, -_area.Top);
                    metafileGraphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                    PointF P = new PointF(_area.Width, _area.Height);
                    PointF[] PA = new PointF[] { P };
                    metafileGraphics.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, PA);
                    //metafileGraphics.PageScale = 1f;

                    // output
                    this.Draw(metafileGraphics);

                    g.ReleaseHdc(hdc);
                    return metafile;
                }
            }
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        /// <summary>
        /// 默认值
        /// </summary>
        public struct Default
        {
            public static bool IsShowTitle = true;
            public static string FontFamily = "宋体";
            public static float FontSize = 20;
            public static Color FontColor = Color.Black;
            public static bool FontBold = false;
            public static bool FontItalic = false;
            public static bool FontUnderline = false;

            public static StringAlignment TitleAlignV = StringAlignment.Center;
            public static StringAlignment TitleAlignH = StringAlignment.Center;

            public static bool IsBorderVisible = false;
            public static Color BorderColor = Color.Black;
            public static Color BackColor = Color.White;
            public static float BorderLineWidth = 1.0f;
            public static float BaseDimension = 8.0f;

            public static bool IsLineWidthScaled = false;
            public static bool IsFontScaled = false;

            public static float TitleGap = 0.5f;
        }

        #endregion

    }
}
