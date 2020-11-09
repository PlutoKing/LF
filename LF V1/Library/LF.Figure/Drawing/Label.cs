/*──────────────────────────────────────────────────────────────
 * FileName     : Label
 * Created      : 2020-10-16 14:39:02
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;


namespace LF.Figure
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Label : ICloneable
    {
        #region Fields
        internal string _text;
        internal FontSpec _fontSpec;
        internal bool _isVisible;
        internal float _gap;
        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="String" /> text to be displayed
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// A <see cref="LF.Figure.FontSpec" /> instance representing the font properties
        /// for the displayed text.
        /// </summary>
        public FontSpec FontSpec
        {
            get { return _fontSpec; }
            set { _fontSpec = value; }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines whether or not this label will be displayed.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public float Gap
        {
            get { return _gap; }
            set { _gap = value; }
        }
        #endregion


        #region Constructors
        public Label(string text, string fontFamily, float fontSize, Color color, bool isBold,
            bool isItalic, bool isUnderline)
        {
            _text = (text == null) ? string.Empty : text;

            _fontSpec = new FontSpec(fontFamily, fontSize, color, isBold, isItalic, isUnderline);
            _isVisible = true;
            _gap = Default.Gap;
        }

        public Label(string text, FontSpec fontSpec)
        {
            _text = (text == null) ? string.Empty : text;

            _fontSpec = fontSpec;
            _isVisible = true;
            _gap = Default.Gap;
        }

        public Label(Label rhs)
        {
            if (rhs._text != null)
                _text = (string)rhs._text.Clone();
            else
                _text = string.Empty;

            _isVisible = rhs._isVisible;
            if (rhs._fontSpec != null)
                _fontSpec = rhs._fontSpec.Clone();
            else
                _fontSpec = null;

            _gap = rhs._gap;
        }

        public Label Clone()
        {
            return new Label(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods

        public float GetScaledGap(float scaleFactor)
        {
            return _fontSpec.GetHeight(scaleFactor) * _gap;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public struct Default
        {
            /// <summary>
            /// The default <see cref="GapLabel.Gap" /> setting.
            /// </summary>
            public static float Gap = 0.3f;
        }
        #endregion
    }
}
