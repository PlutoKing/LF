/*──────────────────────────────────────────────────────────────
 * FileName     : AxisLabel
 * Created      : 2019-12-21 14:22:50
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
    public class AxisLabel : Label, ICloneable
    {
        #region Fields
        internal bool _isOmitMag;
        internal bool _isTitleAtCross;
        #endregion

        #region Properties
        public bool IsOmitMag
        {
            get { return _isOmitMag; }
            set { _isOmitMag = value; }
        }

        public bool IsTitleAtCross
        {
            get { return _isTitleAtCross; }
            set { _isTitleAtCross = value; }
        }
        #endregion

        #region Constructors
        public AxisLabel(string text, string fontFamily, float fontSize, Color color, bool isBold,
                                 bool isItalic, bool isUnderline) :
             base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            _isOmitMag = false;
            _isTitleAtCross = true;
        }

        public AxisLabel(AxisLabel rhs)
            : base(rhs)
        {
            _isOmitMag = rhs._isOmitMag;
            _isTitleAtCross = rhs._isTitleAtCross;
        }

        public new AxisLabel Clone()
        {
            return new AxisLabel(this);
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
        #endregion
    }
}
