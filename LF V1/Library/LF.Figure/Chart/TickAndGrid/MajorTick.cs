/*──────────────────────────────────────────────────────────────
 * FileName     : MajorTick
 * Created      : 2019-12-20 19:06:51
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
    public class MajorTick : MinorTick, ICloneable
    {
        #region Fields
        internal bool _isBetweenLabels;
        #endregion

        #region Properties
        public bool IsBetweenLabels
        {
            get { return _isBetweenLabels; }
            set { _isBetweenLabels = value; }
        }

        #endregion

        #region Constructors
        public MajorTick()
            : base()
        {
            _size = Default.Size;

            _color = Default.Color;
            _lineWidth = Default.LineWidth;

            this.IsOutside = Default.IsOutside;
            this.IsInside = Default.IsInside;
            this.IsOpposite = Default.IsOpposite;
            _isCrossOutside = Default.IsCrossOutside;
            _isCrossInside = Default.IsCrossInside;

            _isBetweenLabels = false;
        }

        public MajorTick(MajorTick rhs)
            : base(rhs)
        {
            _isBetweenLabels = rhs._isBetweenLabels;
        }

        public new MajorTick Clone()
        {
            return new MajorTick(this);
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
        public new struct Default
        {
            public static float Size = 5;
            public static float LineWidth = 1.0F;
            public static bool IsOutside = false;
            public static bool IsInside = true;
            public static bool IsOpposite = true;
            public static bool IsCrossOutside = false;
            public static bool IsCrossInside = false;
            public static Color Color = Color.Black;


        }
        #endregion
    }
}
