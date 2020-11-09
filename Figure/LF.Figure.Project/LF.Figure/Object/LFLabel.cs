/*──────────────────────────────────────────────────────────────
 * FileName     : LFLabel
 * Created      : 2020-11-09 11:23:06
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
using System.Drawing.Text;

namespace LF.Figure
{
    /// <summary>
    /// 文本标签
    /// </summary>
    public class LFLabel
    {
        #region Fields

        /// <summary>
        /// 文本
        /// </summary>
        private string _text;

        /// <summary>
        /// 字体
        /// </summary>
        private LFFontStyle _font;

        private AlignH _alignH;
        private AlignV _alignV;

        /// <summary>
        /// 是否显示
        /// </summary>
        private bool _isVisible = true;

        private PointF _position;
        private SizeF _size;

        private float _angle;

        private LFFillStyle _background;
        private LFBorder _border;

        public string Text { get => _text; set => _text = value; }
        public LFFontStyle Font { get => _font; set => _font = value; }
        public AlignH AlignH { get => _alignH; set => _alignH = value; }
        public AlignV AlignV { get => _alignV; set => _alignV = value; }
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        public PointF Position { get => _position; set => _position = value; }
        public SizeF Size { get => _size; set => _size = value; }
        public float Angle { get => _angle; set => _angle = value; }
        public LFFillStyle Background { get => _background; set => _background = value; }
        public LFBorder Border { get => _border; set => _border = value; }
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LFLabel()
        {
            _text = "测试";
            _font = new LFFontStyle();
            _border = new LFBorder();
            _background = new LFFillStyle();
        }
        #endregion

        #region Methods

        #region Constructor Methods

        #endregion

        #region Sizing Methods

        public void BoundingBox(Graphics g, float scaleFactor)
        {
            BoundingBox(g, scaleFactor, new SizeF());
        }

        public void BoundingBox(Graphics g, float scaleFactor, SizeF layoutArea)
        {
            //Remake( scaleFactor, this.Size, ref this.scaledSize, ref this.font );
            SizeF s;
            if (layoutArea.IsEmpty)
                s = _font.MeasureString(g, _text, scaleFactor);
            else
                s = _font.MeasureString(g, _text, scaleFactor, layoutArea);

            float cs = (float)Math.Abs(Math.Cos(_angle * Math.PI / 180.0));
            float sn = (float)Math.Abs(Math.Sin(_angle * Math.PI / 180.0));

            _size = new SizeF(s.Width * cs + s.Height * sn,
                s.Width * sn + s.Height * cs);
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
        #endregion

        #region Rendering Methods

        public void Draw(Graphics g,LFPane pane,float scaleFactor)
        {

            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (_font.IsAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }

            SizeF sizeF = _font.MeasureString(g, _text, scaleFactor);

            Matrix saveMatrix = g.Transform;
            g.Transform = SetupMatrix(g.Transform, _position.X, _position.Y, sizeF, _alignH, _alignV, _angle);

            RectangleF rectF = new RectangleF(-sizeF.Width / 2.0F, 0.0F,
                                sizeF.Width, sizeF.Height);

            using(Brush brush = _background.MakeBrush(rectF))
            {
                g.FillRectangle(brush, rectF);
            }
            _border.Draw(g, pane, scaleFactor, rectF);

            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = _font.StringAlignment;

            if (_font.IsDropShadow)
            {
                float xShift = (float)(Math.Cos(_font.DropShadowAngle) *
                            _font.DropShadowOffset * _font.Font.Height);
                float yShift = (float)(Math.Sin(_font.DropShadowAngle) *
                            _font.DropShadowOffset * _font.Font.Height);
                RectangleF rectD = rectF;
                rectD.Offset(xShift, yShift);
                // make a solid brush for rendering the font itself
                using (SolidBrush brushD = new SolidBrush(_font.DropShadowColor))
                    g.DrawString(_text, _font.Font, brushD, rectD, strFormat);
            }

            using (SolidBrush brush = new SolidBrush(_font.Color))
            {
                // Draw the actual text.  Note that the coordinate system
                // is set up such that 0,0 is at the location where the
                // CenterTop of the text needs to be.
                //RectangleF layoutArea = new RectangleF( 0.0F, 0.0F, sizeF.Width, sizeF.Height );
                g.DrawString(_text, _font.Font, brush, rectF, strFormat);
            }

            g.Transform = saveMatrix;

            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
