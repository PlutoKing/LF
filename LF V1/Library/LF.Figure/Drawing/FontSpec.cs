/*──────────────────────────────────────────────────────────────
 * FileName     : FontSpec
 * Created      : 2020-10-16 14:39:21
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace LF.Figure
{
    /// <summary>
    /// 字体规格
    /// </summary>
    public class FontSpec : ICloneable
    {
        #region Fields
        private Color _fontColor;
        private string _family;
        private bool _isBold;
        private bool _isItalic;
        private bool _isUnderline;
        private Brush _fillBrush;
        private Border _border;
        private float _angle;
        private StringAlignment _stringAlignment;
        private float _size;
        private Font _font;
        private bool _isAntiAlias;
        private bool _isDropShadow;
        private Color _dropShadowColor;
        private float _dropShadowAngle;
        private float _dropShadowOffset;
        private Font _superScriptFont;
        private float _scaledSize;

        #endregion

        #region Properties

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// 字体类型
        /// </summary>
        public string Family
        {
            get { return _family; }
            set
            {
                if (value != _family)
                {
                    _family = value;
                    Remake(_scaledSize / _size, _size, ref _scaledSize, ref _font);
                }
            }
        }

        public bool IsBold
        {
            get { return _isBold; }
            set
            {
                if (value != _isBold)
                {
                    _isBold = value;
                    Remake(_scaledSize / _size, this.Size, ref _scaledSize, ref _font);
                }
            }
        }

        public bool IsItalic
        {
            get { return _isItalic; }
            set
            {
                if (value != _isItalic)
                {
                    _isItalic = value;
                    Remake(_scaledSize / _size, this.Size, ref _scaledSize, ref _font);
                }
            }
        }

        public bool IsUnderline
        {
            get { return _isUnderline; }
            set
            {
                if (value != _isUnderline)
                {
                    _isUnderline = value;
                    Remake(_scaledSize / _size, this.Size, ref _scaledSize, ref _font);
                }
            }
        }

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        public StringAlignment StringAlignment
        {
            get { return _stringAlignment; }
            set { _stringAlignment = value; }
        }
        public float Size
        {
            get { return _size; }
            set
            {
                if (value != _size)
                {
                    Remake(_scaledSize / _size * value, _size, ref _scaledSize,
                                ref _font);
                    _size = value;
                }
            }
        }
        public Border Border
        {
            get { return _border; }
            set { _border = value; }
        }

        public Brush FillBrush
        {
            get { return _fillBrush; }
            set { _fillBrush = value; }
        }

        public bool IsAntiAlias
        {
            get { return _isAntiAlias; }
            set { _isAntiAlias = value; }
        }

        public bool IsDropShadow
        {
            get { return _isDropShadow; }
            set { _isDropShadow = value; }
        }

        public Color DropShadowColor
        {
            get { return _dropShadowColor; }
            set { _dropShadowColor = value; }
        }

        public float DropShadowAngle
        {
            get { return _dropShadowAngle; }
            set { _dropShadowAngle = value; }
        }

        public float DropShadowOffset
        {
            get { return _dropShadowOffset; }
            set { _dropShadowOffset = value; }
        }

        #endregion

        #region Constructors
        public FontSpec()
            : this("Arial", 12, Color.Black, false, false, false)
        {
        }

        public FontSpec(string family, float size, Color color, bool isBold,
    bool isItalic, bool isUnderline)
        {
            Init(family, size, color, isBold, isItalic, isUnderline,
                    Default.FillColor);
        }
        public FontSpec(string family, float size, Color color, bool isBold,
                    bool isItalic, bool isUnderline, Color fillColor)
        {
            Init(family, size, color, isBold, isItalic, isUnderline,
                    fillColor);
        }

        private void Init(string family, float size, Color color, bool isBold, bool isItalic, bool isUnderline, Color fillColor)
        {
            _fontColor = color;
            _family = family;
            _isBold = isBold;
            _isItalic = isItalic;
            _isUnderline = isUnderline;
            _size = size;
            _angle = 0F;

            _isAntiAlias = Default.IsAntiAlias;
            _stringAlignment = Default.StringAlignment;
            _isDropShadow = Default.IsDropShadow;
            _dropShadowColor = Default.DropShadowColor;
            _dropShadowAngle = Default.DropShadowAngle;
            _dropShadowOffset = Default.DropShadowOffset;

            _fillBrush = new SolidBrush(fillColor);
            _border = new Border(true, Color.Black, 1.0F);

            _scaledSize = -1;
            Remake(1.0F, _size, ref _scaledSize, ref _font);
        }

        public FontSpec(FontSpec rhs)
        {
            _fontColor = rhs._fontColor;
            _family = rhs._family;
            _isBold = rhs._isBold;
            _isItalic = rhs._isItalic;
            _isUnderline = rhs._isUnderline;
            _fillBrush = (Brush)rhs._fillBrush.Clone();
            _border = rhs._border.Clone();
            _isAntiAlias = rhs._isAntiAlias;

            _stringAlignment = rhs._stringAlignment;
            _angle = rhs._angle;
            _size = rhs._size;

            _isDropShadow = rhs._isDropShadow;
            _dropShadowColor = rhs._dropShadowColor;
            _dropShadowAngle = rhs._dropShadowAngle;
            _dropShadowOffset = rhs._dropShadowOffset;

            _scaledSize = rhs._scaledSize;
            Remake(1.0F, _size, ref _scaledSize, ref _font);
        }

        public FontSpec Clone()
        {
            return new FontSpec(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods

        #region Font Construction Methods
        /// <summary>
        /// Recreate the font based on a new scaled size.  The font
        /// will only be recreated if the scaled size has changed by
        /// at least 0.1 points.
        /// </summary>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="size">The unscaled size of the font, in points</param>
        /// <param name="scaledSize">The scaled size of the font, in points</param>
        /// <param name="font">A reference to the <see cref="Font"/> object</param>
        private void Remake(float scaleFactor, float size, ref float scaledSize, ref Font font)
        {
            float newSize = size * scaleFactor;

            float oldSize = (font == null) ? 0.0f : font.Size;

            // Regenerate the font only if the size has changed significantly
            if (font == null ||
                    Math.Abs(newSize - oldSize) > 0.1 ||
                    font.Name != this.Family ||
                    font.Bold != _isBold ||
                    font.Italic != _isItalic ||
                    font.Underline != _isUnderline)
            {
                FontStyle style = FontStyle.Regular;
                style = (_isBold ? FontStyle.Bold : style) |
                            (_isItalic ? FontStyle.Italic : style) |
                             (_isUnderline ? FontStyle.Underline : style);

                scaledSize = size * (float)scaleFactor;
                font = new Font(_family, scaledSize, style, GraphicsUnit.World);
            }
        }

        /// <summary>
        /// Get the <see cref="Font"/> class for the current scaled font.
        /// </summary>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>Returns a reference to a <see cref="Font"/> object
        /// with a size of <see cref="_scaledSize"/>, and font <see cref="Family"/>.
        /// </returns>
        public Font GetFont(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            return _font;
        }
        #endregion

        #region Rendering Methods
        /// <summary>
        /// Render the specified <paramref name="text"/> to the specifed
        /// <see cref="Graphics"/> device.  The text, border, and fill options
        /// will be rendered as required.
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="pane">
        /// A reference to the <see cref="PaneBase"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        public void Draw(Graphics g, string text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            this.Draw(g, text, x, y, alignH, alignV,
                        scaleFactor, new SizeF());
        }

        /// <summary>
        /// Render the specified <paramref name="text"/> to the specifed
        /// <see cref="Graphics"/> device.  The text, border, and fill options
        /// will be rendered as required.
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="pane">
        /// A reference to the <see cref="PaneBase"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="layoutArea">The limiting area (<see cref="SizeF"/>) into which the text
        /// must fit.  The actual rectangle may be smaller than this, but the text will be wrapped
        /// to accomodate the area.</param>
        public void Draw(Graphics g, string text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor, SizeF layoutArea)
        {
            // make sure the font size is properly scaled
            //Remake( scaleFactor, this.Size, ref this.scaledSize, ref this.font );

            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (_isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }

            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = MeasureString(g, text, scaleFactor);
            else
                sizeF = MeasureString(g, text, scaleFactor, layoutArea);

            // Save the old transform matrix for later restoration
            Matrix saveMatrix = g.Transform;
            g.Transform = SetupMatrix(g.Transform, x, y, sizeF, alignH, alignV, _angle);

            // Create a rectangle representing the border around the
            // text.  Note that, while the text is drawn based on the
            // TopCenter position, the rectangle is drawn based on
            // the TopLeft position.  Therefore, move the rectangle
            // width/2 to the left to align it properly
            RectangleF rectF = new RectangleF(-sizeF.Width / 2.0F, 0.0F,
                                sizeF.Width, sizeF.Height);

            // If the background is to be filled, fill it
            g.FillRectangle(_fillBrush, rectF);

            // Draw the border around the text if required
            _border.Draw(g, rectF, scaleFactor);

            // make a center justified StringFormat alignment
            // for drawing the text
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = _stringAlignment;
            //			if ( this.stringAlignment == StringAlignment.Far )
            //				g.TranslateTransform( sizeF.Width / 2.0F, 0F, MatrixOrder.Prepend );
            //			else if ( this.stringAlignment == StringAlignment.Near )
            //				g.TranslateTransform( -sizeF.Width / 2.0F, 0F, MatrixOrder.Prepend )

            // Draw the drop shadow text.  Note that the coordinate system
            // is set up such that 0,0 is at the location where the
            // CenterTop of the text needs to be.
            if (_isDropShadow)
            {
                float xShift = (float)(Math.Cos(_dropShadowAngle) *
                            _dropShadowOffset * _font.Height);
                float yShift = (float)(Math.Sin(_dropShadowAngle) *
                            _dropShadowOffset * _font.Height);
                RectangleF rectD = rectF;
                rectD.Offset(xShift, yShift);
                // make a solid brush for rendering the font itself
                using (SolidBrush brushD = new SolidBrush(_dropShadowColor))
                    g.DrawString(text, _font, brushD, rectD, strFormat);
            }

            // make a solid brush for rendering the font itself
            using (SolidBrush brush = new SolidBrush(_fontColor))
            {
                // Draw the actual text.  Note that the coordinate system
                // is set up such that 0,0 is at the location where the
                // CenterTop of the text needs to be.
                //RectangleF layoutArea = new RectangleF( 0.0F, 0.0F, sizeF.Width, sizeF.Height );
                g.DrawString(text, _font, brush, rectF, strFormat);
            }

            // Restore the transform matrix back to original
            g.Transform = saveMatrix;

            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }

        /// <summary>
        /// Render the specified <paramref name="text"/> to the specifed
        /// <see cref="Graphics"/> device.  The text, border, and fill options
        /// will be rendered as required.  This special case method will show the
        /// specified text as a power of 10, using the <see cref="Default.SuperSize"/>
        /// and <see cref="Default.SuperShift"/>.
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="pane">
        /// A reference to the <see cref="LF.Figure.ChartFigure"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        public void DrawTenPower(Graphics g, string text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (_isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }

            // make sure the font size is properly scaled
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            float scaledSuperSize = _scaledSize * Default.SuperSize;
            Remake(scaleFactor, this.Size * Default.SuperSize, ref scaledSuperSize,
                ref _superScriptFont);

            // Get the width and height of the text
            SizeF size10 = g.MeasureString("10", _font);
            SizeF sizeText = g.MeasureString(text, _superScriptFont);
            SizeF totSize = new SizeF(size10.Width + sizeText.Width,
                                    size10.Height + sizeText.Height * Default.SuperShift);
            float charWidth = g.MeasureString("x", _superScriptFont).Width;

            // Save the old transform matrix for later restoration
            Matrix saveMatrix = g.Transform;

            g.Transform = SetupMatrix(g.Transform, x, y, totSize, alignH, alignV, _angle);

            // make a center justified StringFormat alignment
            // for drawing the text
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = _stringAlignment;

            // Create a rectangle representing the border around the
            // text.  Note that, while the text is drawn based on the
            // TopCenter position, the rectangle is drawn based on
            // the TopLeft position.  Therefore, move the rectangle
            // width/2 to the left to align it properly
            RectangleF rectF = new RectangleF(-totSize.Width / 2.0F, 0.0F,
                totSize.Width, totSize.Height);

            // If the background is to be filled, fill it
            g.FillRectangle(_fillBrush, rectF);

            // Draw the border around the text if required
            _border.Draw(g, rectF, scaleFactor);

            // make a solid brush for rendering the font itself
            using (SolidBrush brush = new SolidBrush(_fontColor))
            {
                // Draw the actual text.  Note that the coordinate system
                // is set up such that 0,0 is at the location where the
                // CenterTop of the text needs to be.
                g.DrawString("10", _font, brush,
                                (-totSize.Width + size10.Width) / 2.0F,
                                sizeText.Height * Default.SuperShift, strFormat);
                g.DrawString(text, _superScriptFont, brush,
                                (totSize.Width - sizeText.Width - charWidth) / 2.0F,
                                0.0F,
                                strFormat);
            }

            // Restore the transform matrix back to original
            g.Transform = saveMatrix;

            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }
        #endregion

        #region Sizing Methods
        /// <summary>
        /// Get the height of the scaled font
        /// </summary>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled font height, in pixels</returns>
        public float GetHeight(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            float height = _font.Height;
            if (_isDropShadow)
                height += (float)(Math.Sin(_dropShadowAngle) * _dropShadowOffset * _font.Height);
            return height * 1.1f;
        }
        /// <summary>
        /// Get the average character width of the scaled font.  The average width is
        /// based on the character 'x'
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled font width, in pixels</returns>
        public float GetWidth(Graphics g, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            return g.MeasureString("x", _font).Width;
        }

        /// <summary>
        /// Get the total width of the specified text string
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled text width, in pixels</returns>
        public float GetWidth(Graphics g, string text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            float width = g.MeasureString(text, _font).Width;
            if (_isDropShadow)
                width += (float)(Math.Cos(_dropShadowAngle) * _dropShadowOffset * _font.Height);
            return width;
        }
        /// <summary>
        /// Get a <see cref="SizeF"/> struct representing the width and height
        /// of the specified text string, based on the scaled font size
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled text dimensions, in pixels, in the form of
        /// a <see cref="SizeF"/> struct</returns>
        public SizeF MeasureString(Graphics g, string text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            SizeF size = g.MeasureString(text, _font);
            if (_isDropShadow)
            {
                size.Width += (float)(Math.Cos(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
                size.Height += (float)(Math.Sin(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height) * 1.1f;
            }
            return size;
        }

        /// <summary>
        /// Get a <see cref="SizeF"/> struct representing the width and height
        /// of the specified text string, based on the scaled font size, and using
        /// the specified <see cref="SizeF"/> as an outer limit.
        /// </summary>
        /// <remarks>
        /// This method will allow the text to wrap as necessary to fit the 
        /// <see paramref="layoutArea"/>.
        /// </remarks>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="layoutArea">The limiting area (<see cref="SizeF"/>) into which the text
        /// must fit.  The actual rectangle may be smaller than this, but the text will be wrapped
        /// to accomodate the area.</param>
        /// <returns>The scaled text dimensions, in pixels, in the form of
        /// a <see cref="SizeF"/> struct</returns>
        public SizeF MeasureString(Graphics g, string text, float scaleFactor, SizeF layoutArea)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            SizeF size = g.MeasureString(text, _font, layoutArea);
            if (_isDropShadow)
            {
                size.Width += (float)(Math.Cos(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
                size.Height += (float)(Math.Sin(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height) * 1.1f;
            }
            return size;
        }

        /// <summary>
        /// Get a <see cref="SizeF"/> struct representing the width and height
        /// of the bounding box for the specified text string, based on the scaled font size.
        /// </summary>
        /// <remarks>
        /// This routine differs from <see cref="MeasureString(Graphics,string,float)"/> in that it takes into
        /// account the rotation angle of the font, and gives the dimensions of the
        /// bounding box that encloses the text at the specified angle.
        /// </remarks>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled text dimensions, in pixels, in the form of
        /// a <see cref="SizeF"/> struct</returns>
        public SizeF BoundingBox(Graphics g, string text, float scaleFactor)
        {
            return BoundingBox(g, text, scaleFactor, new SizeF());
        }

        /// <summary>
        /// Get a <see cref="SizeF"/> struct representing the width and height
        /// of the bounding box for the specified text string, based on the scaled font size.
        /// </summary>
        /// <remarks>
        /// This routine differs from <see cref="MeasureString(Graphics,string,float)"/> in that it takes into
        /// account the rotation angle of the font, and gives the dimensions of the
        /// bounding box that encloses the text at the specified angle.
        /// </remarks>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="layoutArea">The limiting area (<see cref="SizeF"/>) into which the text
        /// must fit.  The actual rectangle may be smaller than this, but the text will be wrapped
        /// to accomodate the area.</param>
        /// <returns>The scaled text dimensions, in pixels, in the form of
        /// a <see cref="SizeF"/> struct</returns>
        public SizeF BoundingBox(Graphics g, string text, float scaleFactor, SizeF layoutArea)
        {
            //Remake( scaleFactor, this.Size, ref this.scaledSize, ref this.font );
            SizeF s;
            if (layoutArea.IsEmpty)
                s = MeasureString(g, text, scaleFactor);
            else
                s = MeasureString(g, text, scaleFactor, layoutArea);

            float cs = (float)Math.Abs(Math.Cos(_angle * Math.PI / 180.0));
            float sn = (float)Math.Abs(Math.Sin(_angle * Math.PI / 180.0));

            SizeF s2 = new SizeF(s.Width * cs + s.Height * sn,
                (s.Width * sn + s.Height * cs) * 1.1f);

            return s2;
        }

        /// <summary>
        /// Get a <see cref="SizeF"/> struct representing the width and height
        /// of the bounding box for the specified text string, based on the scaled font size.
        /// </summary>
        /// <remarks>
        /// This special case method will show the specified string as a power of 10,
        /// superscripted and downsized according to the
        /// <see cref="Default.SuperSize"/> and <see cref="Default.SuperShift"/>.
        /// This routine differs from <see cref="MeasureString(Graphics,string,float)"/> in that it takes into
        /// account the rotation angle of the font, and gives the dimensions of the
        /// bounding box that encloses the text at the specified angle.
        /// </remarks>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">The text string for which the width is to be calculated
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>The scaled text dimensions, in pixels, in the form of
        /// a <see cref="SizeF"/> struct</returns>
        public SizeF BoundingBoxTenPower(Graphics g, string text, float scaleFactor)
        {
            //Remake( scaleFactor, this.Size, ref this.scaledSize, ref this.font );
            float scaledSuperSize = _scaledSize * Default.SuperSize;
            Remake(scaleFactor, this.Size * Default.SuperSize, ref scaledSuperSize,
                ref _superScriptFont);

            // Get the width and height of the text
            SizeF size10 = MeasureString(g, "10", scaleFactor);
            SizeF sizeText = g.MeasureString(text, _superScriptFont);

            if (_isDropShadow)
            {
                sizeText.Width += (float)(Math.Cos(_dropShadowAngle) *
                            _dropShadowOffset * _superScriptFont.Height);
                sizeText.Height += (float)(Math.Sin(_dropShadowAngle) *
                            _dropShadowOffset * _superScriptFont.Height);
            }

            SizeF totSize = new SizeF(size10.Width + sizeText.Width,
                size10.Height + sizeText.Height * Default.SuperShift);


            float cs = (float)Math.Abs(Math.Cos(_angle * Math.PI / 180.0));
            float sn = (float)Math.Abs(Math.Sin(_angle * Math.PI / 180.0));

            SizeF s2 = new SizeF(totSize.Width * cs + totSize.Height * sn,
                (totSize.Width * sn + totSize.Height * cs) * 1.1f);

            return s2;
        }


        /// <summary>
        /// Determines if the specified screen point lies within the bounding box of
        /// the text, taking into account alignment and rotation parameters.
        /// </summary>
        /// <param name="pt">The screen point, in pixel units</param>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>true if the point lies within the bounding box, false otherwise</returns>
        public bool PointInBox(PointF pt, Graphics g, string text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            return PointInBox(pt, g, text, x, y, alignH, alignV, scaleFactor, new SizeF());
        }

        /// <summary>
        /// Determines if the specified screen point lies within the bounding box of
        /// the text, taking into account alignment and rotation parameters.
        /// </summary>
        /// <param name="pt">The screen point, in pixel units</param>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="layoutArea">The limiting area (<see cref="SizeF"/>) into which the text
        /// must fit.  The actual rectangle may be smaller than this, but the text will be wrapped
        /// to accomodate the area.</param>
        /// <returns>true if the point lies within the bounding box, false otherwise</returns>
        public bool PointInBox(PointF pt, Graphics g, string text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor, SizeF layoutArea)
        {
            // make sure the font size is properly scaled
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);

            // Get the width and height of the text
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = g.MeasureString(text, _font);
            else
                sizeF = g.MeasureString(text, _font, layoutArea);

            // Create a bounding box rectangle for the text
            RectangleF rect = new RectangleF(new PointF(-sizeF.Width / 2.0F, 0.0F), sizeF);

            // Build a transform matrix that inverts that drawing transform
            // in this manner, the point is brought back to the box, rather
            // than vice-versa.  This allows the container check to be a simple
            // RectangleF.Contains, since the rectangle won't be rotated.
            Matrix matrix = GetMatrix(x, y, sizeF, alignH, alignV, _angle);

            PointF[] pts = new PointF[1];
            pts[0] = pt;
            matrix.TransformPoints(pts);

            return rect.Contains(pts[0]);
        }

        private Matrix SetupMatrix(Matrix matrix, float x, float y, SizeF sizeF, AlignH alignH,
                AlignV alignV, float angle)
        {
            // Move the coordinate system to local coordinates
            // of this text object (that is, at the specified
            // x,y location)
            matrix.Translate(x, y, MatrixOrder.Prepend);

            // Rotate the coordinate system according to the 
            // specified angle of the FontSpec
            if (_angle != 0.0F)
                matrix.Rotate(-angle, MatrixOrder.Prepend);

            // Since the text will be drawn by g.DrawString()
            // assuming the location is the TopCenter
            // (the Font is aligned using StringFormat to the
            // center so multi-line text is center justified),
            // shift the coordinate system so that we are
            // actually aligned per the caller specified position
            float xa, ya;
            if (alignH == AlignH.Left)
                xa = sizeF.Width / 2.0F;
            else if (alignH == AlignH.Right)
                xa = -sizeF.Width / 2.0F;
            else
                xa = 0.0F;

            if (alignV == AlignV.Center)
                ya = -sizeF.Height / 2.0F;
            else if (alignV == AlignV.Bottom)
                ya = -sizeF.Height;
            else
                ya = 0.0F;

            // Shift the coordinates to accomodate the alignment
            // parameters
            matrix.Translate(xa, ya, MatrixOrder.Prepend);

            return matrix;
        }

        private Matrix GetMatrix(float x, float y, SizeF sizeF, AlignH alignH, AlignV alignV,
                            float angle)
        {
            // Build a transform matrix that inverts that drawing transform
            // in this manner, the point is brought back to the box, rather
            // than vice-versa.  This allows the container check to be a simple
            // RectangleF.Contains, since the rectangle won't be rotated.
            Matrix matrix = new Matrix();

            // In this case, the bounding box is anchored to the
            // top-left of the text box.  Handle the alignment
            // as needed.
            float xa, ya;
            if (alignH == AlignH.Left)
                xa = sizeF.Width / 2.0F;
            else if (alignH == AlignH.Right)
                xa = -sizeF.Width / 2.0F;
            else
                xa = 0.0F;

            if (alignV == AlignV.Center)
                ya = -sizeF.Height / 2.0F;
            else if (alignV == AlignV.Bottom)
                ya = -sizeF.Height;
            else
                ya = 0.0F;

            // Shift the coordinates to accomodate the alignment
            // parameters
            matrix.Translate(-xa, -ya, MatrixOrder.Prepend);

            // Rotate the coordinate system according to the 
            // specified angle of the FontSpec
            if (angle != 0.0F)
                matrix.Rotate(angle, MatrixOrder.Prepend);

            // Move the coordinate system to local coordinates
            // of this text object (that is, at the specified
            // x,y location)
            matrix.Translate(-x, -y, MatrixOrder.Prepend);

            return matrix;
        }

        /// <summary>
        /// Returns a polygon that defines the bounding box of
        /// the text, taking into account alignment and rotation parameters.
        /// </summary>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="text">A string value containing the text to be
        /// displayed.  This can be multiple lines, separated by newline ('\n')
        /// characters</param>
        /// <param name="x">The X location to display the text, in screen
        /// coordinates, relative to the horizontal (<see cref="AlignH"/>)
        /// alignment parameter <paramref name="alignH"/></param>
        /// <param name="y">The Y location to display the text, in screen
        /// coordinates, relative to the vertical (<see cref="AlignV"/>
        /// alignment parameter <paramref name="alignV"/></param>
        /// <param name="alignH">A horizontal alignment parameter specified
        /// using the <see cref="AlignH"/> enum type</param>
        /// <param name="alignV">A vertical alignment parameter specified
        /// using the <see cref="AlignV"/> enum type</param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="ChartFigure"/> object using the
        /// <see cref="PaneBase.GetScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <param name="layoutArea">The limiting area (<see cref="SizeF"/>) into which the text
        /// must fit.  The actual rectangle may be smaller than this, but the text will be wrapped
        /// to accomodate the area.</param>
        /// <returns>A polygon of 4 points defining the area of this text</returns>
        public PointF[] GetBox(Graphics g, string text, float x,
                float y, AlignH alignH, AlignV alignV,
                float scaleFactor, SizeF layoutArea)
        {
            // make sure the font size is properly scaled
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);

            // Get the width and height of the text
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = g.MeasureString(text, _font);
            else
                sizeF = g.MeasureString(text, _font, layoutArea);

            // Create a bounding box rectangle for the text
            RectangleF rect = new RectangleF(new PointF(-sizeF.Width / 2.0F, 0.0F), sizeF);

            Matrix matrix = new Matrix();
            SetupMatrix(matrix, x, y, sizeF, alignH, alignV, _angle);

            PointF[] pts = new PointF[4];
            pts[0] = new PointF(rect.Left, rect.Top);
            pts[1] = new PointF(rect.Right, rect.Top);
            pts[2] = new PointF(rect.Right, rect.Bottom);
            pts[3] = new PointF(rect.Left, rect.Bottom);
            matrix.TransformPoints(pts);

            return pts;
        }

        #endregion
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        /// <summary>
        /// A simple struct that defines the
        /// default property values for the <see cref="FontSpec"/> class.
        /// </summary>
        public struct Default
        {
            /// <summary>
            /// The default size fraction of the superscript font, expressed as a fraction
            /// of the size of the main font.
            /// </summary>
            public static float SuperSize = 0.85F;
            /// <summary>
            /// The default shift fraction of the superscript, expressed as a
            /// fraction of the superscripted character height.  This is the height
            /// above the main font (a zero shift means the main font and the superscript
            /// font have the tops aligned).
            /// </summary>
            public static float SuperShift = 0.4F;
            /// <summary>
            /// The default color for filling in the background of the text block
            /// (<see cref="LF.Figure.Fill.Color"/> property).
            /// </summary>
            public static Color FillColor = Color.White;
            /// <summary>
            /// The default custom brush for filling in this <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.Fill.Brush"/> property).
            /// </summary>
            public static Brush FillBrush = null;
            /// <summary>
            /// The default fill mode for this <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.Fill.Type"/> property).
            /// </summary>
            //public static brus FillType = FillType.Solid;
            /// <summary>
            /// Default value for the alignment with which this
            /// <see cref="FontSpec"/> object is drawn.  This alignment really only
            /// affects multi-line strings.
            /// </summary>
            /// <value>A <see cref="StringAlignment"/> enumeration.</value>
            public static StringAlignment StringAlignment = StringAlignment.Center;

            /// <summary>
            /// Default value for <see cref="FontSpec.IsDropShadow"/>, which determines
            /// if the drop shadow is displayed for this <see cref="FontSpec" />.
            /// </summary>
            public static bool IsDropShadow = false;
            /// <summary>
            /// Default value for <see cref="FontSpec.IsAntiAlias"/>, which determines
            /// if anti-aliasing logic is used for this <see cref="FontSpec" />.
            /// </summary>
            public static bool IsAntiAlias = true;
            /// <summary>
            /// Default value for <see cref="FontSpec.DropShadowColor"/>, which determines
            /// the color of the drop shadow for this <see cref="FontSpec" />.
            /// </summary>
            public static Color DropShadowColor = Color.Black;
            /// <summary>
            /// Default value for <see cref="FontSpec.DropShadowAngle"/>, which determines
            /// the offset angle of the drop shadow for this <see cref="FontSpec" />.
            /// </summary>
            public static float DropShadowAngle = 45f;
            /// <summary>
            /// Default value for <see cref="FontSpec.DropShadowOffset"/>, which determines
            /// the offset distance of the drop shadow for this <see cref="FontSpec" />.
            /// </summary>
            public static float DropShadowOffset = 0.05f;

        }
        #endregion
    }


    /// <summary>
    /// 不同水平文本对齐选项的枚举类型
    /// </summary>
    /// <seealso cref="FontSpec"/>
    public enum AlignH
    {
        /// <summary>
        /// Position the text so that its left edge is aligned with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Left,
        /// <summary>
        /// Position the text so that its center is aligned (horizontally) with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Center,
        /// <summary>
        /// Position the text so that its right edge is aligned with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Right
    }

    /// <summary>
    /// 不同的近端对齐选项的枚举类型
    /// </summary>
    /// <seealso cref="FontSpec"/>
    /// <seealso cref="Scale.Align"/>
    public enum AlignP
    {
        /// <summary>
        /// Position the text so that its "inside" edge (the edge that is
        /// nearest to the alignment reference point or object) is aligned.
        /// Used by the <see cref="Scale.Align"/> method to align text
        /// to the axis.
        /// </summary>
        Inside,
        /// <summary>
        /// Position the text so that its center is aligned with the
        /// reference object or point.
        /// Used by the <see cref="Scale.Align"/> method to align text
        /// to the axis.
        /// </summary>
        Center,
        /// <summary>
        /// Position the text so that its right edge (the edge that is
        /// farthest from the alignment reference point or object) is aligned.
        /// Used by the <see cref="Scale.Align"/> method to align text
        /// to the axis.
        /// </summary>
        Outside
    }

    /// <summary>
    /// 不同垂直文本对齐选项的枚举类型
    /// </summary>
    /// specified X,Y location.  Used by the
    /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
    public enum AlignV
    {
        /// <summary>
        /// Position the text so that its top edge is aligned with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Top,
        /// <summary>
        /// Position the text so that its center is aligned (vertically) with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Center,
        /// <summary>
        /// Position the text so that its bottom edge is aligned with the
        /// specified X,Y location.  Used by the
        /// <see cref="FontSpec.Draw(Graphics,PaneBase,string,float,float,AlignH,AlignV,float)"/> method.
        /// </summary>
        Bottom
    }
}
