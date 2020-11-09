/*──────────────────────────────────────────────────────────────
 * FileName     : YAxis
 * Created      : 2019-12-20 19:15:55
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
    public class YAxis : Axis, ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public YAxis()
            : this("Y Axis")
        {
        }

        public YAxis(string title)
            : base(title)
        {
            _isVisible = Default.IsVisible;
            _majorGrid.IsZeroLine = Default.IsZeroLine;
            _scale._fontSpec.Angle = 90.0F;
            _title._fontSpec.Angle = -180F;
        }

        public YAxis(YAxis rhs)
            : base(rhs)
        {
        }

        public YAxis Clone()
        {
            return new YAxis(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods
        override internal bool IsPrimary(ChartFigure graph)
        {
            return this == graph.YAxis;
        }


        override public void SetTransformMatrix(Graphics g, ChartFigure graph, float scaleFactor)
        {
            // Move the origin to the TopLeft of the ChartRect, which is the left
            // side of the axis (facing from the label side)
            g.TranslateTransform(graph.Chart.Area.Left, graph.Chart.Area.Top);
            // rotate so this axis is in the left-right direction
            g.RotateTransform(90);
        }

        internal override float GetCrossShift(ChartFigure graph)
        {
            double effCross = EffectiveCrossValue(graph);

            if (!_crossAuto)
                return graph.XAxis.Scale._minPix - graph.XAxis.Scale.Transform(effCross);
            else
                return 0;
        }
        override public Axis GetCrossAxis(ChartFigure graph)
        {
            return graph.XAxis;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public new struct Default
        {
            public static bool IsVisible = true;
            public static bool IsZeroLine = true;
        }
        #endregion
    }
}
