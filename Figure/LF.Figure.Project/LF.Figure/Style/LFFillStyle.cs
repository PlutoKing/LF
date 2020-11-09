/*──────────────────────────────────────────────────────────────
 * FileName     : LFFillStyle
 * Created      : 2020-11-09 10:40:25
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
    /// 填充样式
    /// </summary>
    public class LFFillStyle
    {
        #region Fields

        /// <summary>
        /// 填充颜色
        /// </summary>
        private Color _color;

        /// <summary>
        /// 填充样式
        /// </summary>
        private FillType _type;

        /// <summary>
        /// 第二颜色
        /// </summary>
        private Color _secondaryValueGradientColor;

        /// <summary>
        /// 自定义画笔
        /// </summary>
        private Brush _brush;

        /// <summary>
        /// 是否缩放
        /// </summary>
        private bool _isScaled;

        /// <summary>
        /// 水平布局
        /// </summary>
        private AlignH _alignH;

        /// <summary>
        /// 垂直布局
        /// </summary>
        private AlignV _alignV;

        private double _rangeMin;
        private double _rangeMax;
        private double _rangeDefault;
        private Bitmap _gradientBM;

        private Image _image;

        /// <summary>
        /// 
        /// </summary>
        private WrapMode _wrapMode;

        /// <summary>
        /// 颜色数组
        /// 用于线性梯度
        /// </summary>
        private Color[] _colorList;

        /// <summary>
        /// 位置数组
        /// 用于线性梯度
        /// </summary>
        private float[] _positionList;

        /// <summary>
        /// 角度
        /// </summary>
        private float _angle;

        #endregion

        #region Properties

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get => _color; set => _color = value; }
        
        /// <summary>
        /// 样式
        /// </summary>
        public FillType Type { get => _type; set => _type = value; }
        public bool IsGradientValueType
        {
            get
            {
                return _type == FillType.GradientByX || _type == FillType.GradientByY ||
                  _type == FillType.GradientByZ || _type == FillType.GradientByColorValue;
            }
        }
        public bool IsVisible
        {
            get { return _type != FillType.None; }
            set { _type = value ? (_type == FillType.None ? FillType.Brush : _type) : FillType.None; }
        }
        public Color SecondaryValueGradientColor { get => _secondaryValueGradientColor; set => _secondaryValueGradientColor = value; }
        public Brush Brush { get => _brush; set => _brush = value; }
        public bool IsScaled { get => _isScaled; set => _isScaled = value; }
        public AlignH AlignH { get => _alignH; set => _alignH = value; }
        public AlignV AlignV { get => _alignV; set => _alignV = value; }
        public double RangeMin { get => _rangeMin; set => _rangeMin = value; }
        public double RangeMax { get => _rangeMax; set => _rangeMax = value; }
        public double RangeDefault { get => _rangeDefault; set => _rangeDefault = value; }
        public Bitmap GradientBM { get => _gradientBM; set => _gradientBM = value; }
        public Image Image { get => _image; set => _image = value; }
        public WrapMode WrapMode { get => _wrapMode; set => _wrapMode = value; }
        public Color[] ColorList { get => _colorList; set => _colorList = value; }
        public float[] PositionList { get => _positionList; set => _positionList = value; }
        public float Angle { get => _angle; set => _angle = value; }

        #endregion

        #region Constructors
        public LFFillStyle()
        {
            Init();
        }

        public LFFillStyle(Color color)
        {
            Init();
            _color = color;
            if (color != Color.Empty)
                _type = FillType.Solid;
        }

        public LFFillStyle(Color color, Brush brush, FillType type)
        {
            Init();
            _color = color;
            _brush = brush;
            _type = type;
        }

        public LFFillStyle(Color color1, Color color2, float angle)
        {
            Init();
            _color = color2;

            ColorBlend blend = new ColorBlend(2);
            blend.Colors[0] = color1;
            blend.Colors[1] = color2;
            blend.Positions[0] = 0.0f;
            blend.Positions[1] = 1.0f;
            _type = FillType.Brush;

            this.CreateBrushFromBlend(blend, angle);
        }
        #endregion

        #region Methods

        #region Constructor Methods

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            _color = Color.White;
            _secondaryValueGradientColor = Color.White;
            _brush = null;
            _type = FillType.None;
            _isScaled = Default.IsScaled;
            _alignH = Default.AlignH;
            _alignV = Default.AlignV;
            _rangeMin = 0.0;
            _rangeMax = 1.0;
            _rangeDefault = double.MaxValue;
            _gradientBM = null;

            _colorList = null;
            _positionList = null;
            _angle = 0;
            _image = null;
            _wrapMode = WrapMode.Tile;
        }

        private void CreateBrushFromBlend(ColorBlend blend, float angle)
        {
            _angle = angle;

            _colorList = (Color[])blend.Colors.Clone();
            _positionList = (float[])blend.Positions.Clone();

            _brush = new LinearGradientBrush(new Rectangle(0, 0, 100, 100),
                Color.Red, Color.White, angle);
            ((LinearGradientBrush)_brush).InterpolationColors = blend;
        }

        public Brush MakeBrush(RectangleF rect)
        {
            // just provide a default value for the valueFraction
            // return MakeBrush( rect, new PointPair( 0.5, 0.5, 0.5 ) );
            return MakeBrush(rect, Double.MaxValue);
        }

        public Brush MakeBrush(RectangleF rect,double dataValue)
        {
            if (this.IsVisible && (!_color.IsEmpty || _brush != null))
            {
                if (rect.Height < 1.0F)
                    rect.Height = 1.0F;
                if (rect.Width < 1.0F)
                    rect.Width = 1.0F;

                //Brush	brush;
                if (_type == FillType.Brush)
                {
                    return ScaleBrush(rect, _brush, _isScaled);
                }
                else if (IsGradientValueType)
                {
                    if (dataValue != Double.MaxValue)
                    {
                        if (!_secondaryValueGradientColor.IsEmpty)
                        {
                            // Go ahead and create a new Fill so we can do all the scaling, etc.,
                            // that is associated with a gradient
                            LFFillStyle tmpFill = new LFFillStyle(_secondaryValueGradientColor,
                                    GetGradientColor(dataValue), _angle);
                            return tmpFill.MakeBrush(rect);
                        }
                        else
                            return new SolidBrush(GetGradientColor(dataValue));
                    }
                    else if (_rangeDefault != double.MaxValue)
                    {
                        if (!_secondaryValueGradientColor.IsEmpty)
                        {
                            // Go ahead and create a new Fill so we can do all the scaling, etc.,
                            // that is associated with a gradient
                            LFFillStyle tmpFill = new LFFillStyle(_secondaryValueGradientColor,
                                    GetGradientColor(_rangeDefault), _angle);
                            return tmpFill.MakeBrush(rect);
                        }
                        else
                            return new SolidBrush(GetGradientColor(_rangeDefault));
                    }
                    else
                        return ScaleBrush(rect, _brush, true);
                }
                else
                    return new SolidBrush(_color);
            }

            // Always return a suitable default
            return new SolidBrush(Color.White);
        }

        internal Color GetGradientColor(double val)
        {
            double valueFraction;

            if (Double.IsInfinity(val) || double.IsNaN(val) || val == Double.MaxValue)
                val = _rangeDefault;

            if (_rangeMax - _rangeMin < 1e-20 || val == double.MaxValue)
                valueFraction = 0.5;
            else
                valueFraction = (val - _rangeMin) / (_rangeMax - _rangeMin);

            if (valueFraction < 0.0)
                valueFraction = 0.0;
            else if (valueFraction > 1.0)
                valueFraction = 1.0;

            if (_gradientBM == null)
            {
                RectangleF rect = new RectangleF(0, 0, 100, 1);
                _gradientBM = new Bitmap(100, 1);
                Graphics gBM = Graphics.FromImage(_gradientBM);

                Brush tmpBrush = ScaleBrush(rect, _brush, true);
                gBM.FillRectangle(tmpBrush, rect);
            }

            return _gradientBM.GetPixel((int)(99.9 * valueFraction), 0);
        }


        #endregion


        #region Sizing Methods
        private Brush ScaleBrush(RectangleF rect, Brush brush, bool isScaled)
        {
            if (brush != null)
            {
                if (brush is SolidBrush)
                {
                    return (Brush)brush.Clone();
                }
                else if (brush is LinearGradientBrush)
                {
                    LinearGradientBrush linBrush = (LinearGradientBrush)brush.Clone();

                    if (isScaled)
                    {
                        linBrush.ScaleTransform(rect.Width / linBrush.Rectangle.Width,
                            rect.Height / linBrush.Rectangle.Height, MatrixOrder.Append);
                        linBrush.TranslateTransform(rect.Left - linBrush.Rectangle.Left,
                            rect.Top - linBrush.Rectangle.Top, MatrixOrder.Append);
                    }
                    else
                    {
                        float dx = 0,
                                dy = 0;
                        switch (_alignH)
                        {
                            case AlignH.Left:
                                dx = rect.Left - linBrush.Rectangle.Left;
                                break;
                            case AlignH.Center:
                                dx = (rect.Left + rect.Width / 2.0F) - linBrush.Rectangle.Left;
                                break;
                            case AlignH.Right:
                                dx = (rect.Left + rect.Width) - linBrush.Rectangle.Left;
                                break;
                        }

                        switch (_alignV)
                        {
                            case AlignV.Top:
                                dy = rect.Top - linBrush.Rectangle.Top;
                                break;
                            case AlignV.Center:
                                dy = (rect.Top + rect.Height / 2.0F) - linBrush.Rectangle.Top;
                                break;
                            case AlignV.Bottom:
                                dy = (rect.Top + rect.Height) - linBrush.Rectangle.Top;
                                break;
                        }

                        linBrush.TranslateTransform(dx, dy, MatrixOrder.Append);
                    }

                    return linBrush;

                } // LinearGradientBrush
                else if (brush is TextureBrush)
                {
                    TextureBrush texBrush = (TextureBrush)brush.Clone();

                    if (isScaled)
                    {
                        texBrush.ScaleTransform(rect.Width / texBrush.Image.Width,
                            rect.Height / texBrush.Image.Height, MatrixOrder.Append);
                        texBrush.TranslateTransform(rect.Left, rect.Top, MatrixOrder.Append);
                    }
                    else
                    {
                        float dx = 0,
                                dy = 0;
                        switch (_alignH)
                        {
                            case AlignH.Left:
                                dx = rect.Left;
                                break;
                            case AlignH.Center:
                                dx = (rect.Left + rect.Width / 2.0F);
                                break;
                            case AlignH.Right:
                                dx = (rect.Left + rect.Width);
                                break;
                        }

                        switch (_alignV)
                        {
                            case AlignV.Top:
                                dy = rect.Top;
                                break;
                            case AlignV.Center:
                                dy = (rect.Top + rect.Height / 2.0F);
                                break;
                            case AlignV.Bottom:
                                dy = (rect.Top + rect.Height);
                                break;
                        }

                        texBrush.TranslateTransform(dx, dy, MatrixOrder.Append);
                    }

                    return texBrush;
                }
                else // other brush type
                {
                    return (Brush)brush.Clone();
                }
            }
            else
                // If they didn't provide a brush, make one using the fillcolor gradient to white
                return new LinearGradientBrush(rect, Color.White, _color, 0F);
        }

        #endregion

        #region Rendering Methods

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
            /// 是否缩放
            /// </summary>
            public static bool IsScaled = true;

            /// <summary>
            /// 水平布局
            /// </summary>
            public static AlignH AlignH = AlignH.Center;

            /// <summary>
            /// 垂直布局
            /// </summary>
            public static AlignV AlignV = AlignV.Center;
        }

        #endregion
    }

    /// <summary>
    /// 填充样式
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// 无填充
        /// </summary>
        None,

        /// <summary>
        /// 纯色填充
        /// </summary>
        Solid,

        /// <summary>
        /// 线性梯度填充或纹理填充
        /// </summary>
        Brush,

        GradientByX,
        GradientByY,
        GradientByZ,
        GradientByColorValue

    }

    /// <summary>
    /// 水平布局
    /// </summary>
    public enum AlignH
    {
        /// <summary>
        /// 水平居左
        /// </summary>
        Left,
        /// <summary>
        /// 水平居中
        /// </summary>
        Center,
        /// <summary>
        /// 水平居右
        /// </summary>
        Right
    }

    /// <summary>
    /// 垂直布局
    /// </summary>
    public enum AlignV
    {
        /// <summary>
        /// 垂直居上
        /// </summary>
        Top,
        /// <summary>
        /// 垂直居中
        /// </summary>
        Center,
        /// <summary>
        /// 垂直居下
        /// </summary>
        Bottom
    }

}
