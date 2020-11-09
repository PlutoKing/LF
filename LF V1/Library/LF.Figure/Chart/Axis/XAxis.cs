/*──────────────────────────────────────────────────────────────
 * FileName     : XAxis
 * Created      : 2019-12-20 19:14:38
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
    public class XAxis : Axis, ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public XAxis()
            : this("X Axis")
        {
        }

        public XAxis(string title)
            : base(title)
        {
            _isVisible = Default.IsVisible;
            _majorGrid.IsZeroLine = Default.IsZeroLine;
            _scale._fontSpec.Angle = 0F;

        }

        public XAxis(XAxis rhs)
            : base(rhs)
        {
        }

        public XAxis Clone()
        {
            return new XAxis(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods
        override internal bool IsPrimary(ChartFigure graph)
        {
            return this == graph.XAxis;
        }


        override public void SetTransformMatrix(Graphics g, ChartFigure graph, float scaleFactor)
        {
            // Move the origin to the BottomLeft of the ChartRect, which is the left
            // side of the X axis (facing from the label side)
            g.TranslateTransform(graph.Chart.Area.Left, graph.Chart.Area.Bottom);
        }

        internal override float GetCrossShift(ChartFigure graph)
        {
            double effCross = EffectiveCrossValue(graph);

            if (!_crossAuto)
                return graph.YAxis.Scale.Transform(effCross) - graph.YAxis.Scale._maxPix;
            else
                return 0;
        }

        override public Axis GetCrossAxis(ChartFigure graph)
        {
            return graph.YAxis;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public new struct Default
        {
            public static bool IsVisible = true;
            public static bool IsZeroLine = false;
        }
        #endregion
    }
}
