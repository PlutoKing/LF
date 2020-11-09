/*──────────────────────────────────────────────────────────────
 * FileName     : LFFontStyle
 * Created      : 2020-11-09 11:11:41
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
    /// 字体样式
    /// </summary>
    public class LFFontStyle
    {
        #region Fields

        /// <summary>
        /// 字体
        /// </summary>
        private string _family;

        /// <summary>
        /// 字号
        /// </summary>
        private float _size;

        /// <summary>
        /// 颜色
        /// </summary>
        private Color _color;

        private bool _isBold;

        private bool _isItalic;

        private bool _isUnderline;

        private StringAlignment _stringAlignment;

        private Font _font;

        private bool _isAntiAlias;

        private bool _isDropShadow;
        private Color _dropShadowColor;
        private float _dropShadowAngle;
        private float _dropShadowOffset;

        private Font _superScriptFont;
        private Font _subScriptFont;

        private float _scaledSize;


        #endregion

        #region Properties

        /// <summary>
        /// 字体
        /// </summary>
        public string Family { get => _family; set => _family = value; }
        
        /// <summary>
        /// 字号
        /// </summary>
        public float Size
        {
            get { return _size; }
            set
            {
                if (value != _size)
                {
                    Remake(_scaledSize / _size * value, _size,
                        ref _scaledSize, ref _font);
                    _size = value;
                }
            }
        }

        public float Size1 { get => _size; set => _size = value; }
        public Color Color { get => _color; set => _color = value; }
        public bool IsBold { get => _isBold; set => _isBold = value; }
        public bool IsItalic { get => _isItalic; set => _isItalic = value; }
        public bool IsUnderline { get => _isUnderline; set => _isUnderline = value; }
        public StringAlignment StringAlignment { get => _stringAlignment; set => _stringAlignment = value; }
        public Font Font { get => _font; set => _font = value; }
        public bool IsAntiAlias { get => _isAntiAlias; set => _isAntiAlias = value; }
        public bool IsDropShadow { get => _isDropShadow; set => _isDropShadow = value; }
        public Color DropShadowColor { get => _dropShadowColor; set => _dropShadowColor = value; }
        public float DropShadowAngle { get => _dropShadowAngle; set => _dropShadowAngle = value; }
        public float DropShadowOffset { get => _dropShadowOffset; set => _dropShadowOffset = value; }
        public Font SuperScriptFont { get => _superScriptFont; set => _superScriptFont = value; }
        public Font SubScriptFont { get => _subScriptFont; set => _subScriptFont = value; }
        public float ScaledSize { get => _scaledSize; set => _scaledSize = value; }
        #endregion

        #region Constructors
        public LFFontStyle()
        {
            _family = "微软雅黑";
            _size = 16;
            _color = Color.Black;
        }
        #endregion

        #region Methods

        #region Constructor Methods

        /// <summary>
        /// 重新生成字体
        /// </summary>
        /// <param name="scaleFactor">缩放因子</param>
        /// <param name="size">字号</param>
        /// <param name="scaledSize">输出：缩放字号</param>
        /// <param name="font">字体</param>
        private void Remake(float scaleFactor,float size, ref float scaledSize, ref Font font)
        {
            float newSize = size * scaleFactor;
            float oldSize = (font == null) ? 0.0f : font.Size;

            if (font == null ||
                    Math.Abs(newSize - oldSize) > 0.1 ||
                    font.Name != this._family ||
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
        /// 生成字体
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public Font GetFont(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            return _font;
        }
        #endregion


        #region Sizing Methods

        /// <summary>
        /// 计算高度
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public float GetHeight(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            float height = _font.Height;
            if (_isDropShadow)
                height += (float)(Math.Sin(_dropShadowAngle) * _dropShadowOffset * _font.Height);
            return height;
        }

        /// <summary>
        /// 计算默认宽度
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public float GetWidth(Graphics g, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            return g.MeasureString("x", _font).Width;
        }

        /// <summary>
        /// 计算文本宽度
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text">文本</param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public float GetWidth(Graphics g, string text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            float width = g.MeasureString(text, _font).Width;
            if (_isDropShadow)
                width += (float)(Math.Cos(_dropShadowAngle) * _dropShadowOffset * _font.Height);
            return width;
        }

        /// <summary>
        /// 计算文本尺寸
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public SizeF MeasureString(Graphics g, string text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            SizeF size = g.MeasureString(text, _font);
            if (_isDropShadow)
            {
                size.Width += (float)(Math.Cos(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
                size.Height += (float)(Math.Sin(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
            }
            return size;
        }

        /// <summary>
        /// 计算文本尺寸
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="layoutArea"></param>
        /// <returns></returns>
        public SizeF MeasureString(Graphics g, string text, float scaleFactor, SizeF layoutArea)
        {
            Remake(scaleFactor, this.Size, ref _scaledSize, ref _font);
            SizeF size = g.MeasureString(text, _font, layoutArea);
            if (_isDropShadow)
            {
                size.Width += (float)(Math.Cos(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
                size.Height += (float)(Math.Sin(_dropShadowAngle) *
                                _dropShadowOffset * _font.Height);
            }
            return size;
        }
   

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
