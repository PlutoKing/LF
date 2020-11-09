/*──────────────────────────────────────────────────────────────
 * FileName     : Y2Axis
 * Created      : 2019-12-21 13:14:49
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
    public class Y2Axis : Axis, ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Y2Axis()
            : this("Y2 Axis")
        {
        }

        public Y2Axis(string title)
            : base(title)
        {
            _isVisible = Default.IsVisible;
            _majorGrid.IsZeroLine = Default.IsZeroLine;
            _scale._fontSpec.Angle = -90.0F;
        }

        public Y2Axis(Y2Axis rhs)
            : base(rhs)
        {
        }

        public Y2Axis Clone()
        {
            return new Y2Axis(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        override internal bool IsPrimary(ChartFigure graph)
        {
            return this == graph.Y2Axis;
        }

        override public void SetTransformMatrix(Graphics g, ChartFigure graph, float scaleFactor)
        {
            g.TranslateTransform(graph.Chart.Area.Right, graph.Chart.Area.Bottom);
            g.RotateTransform(-90);
        }

        internal override float GetCrossShift(ChartFigure graph)
        {
            double effCross = EffectiveCrossValue(graph);

            if (!_crossAuto)
                return graph.XAxis.Scale.Transform(effCross) - graph.XAxis.Scale._maxPix;
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
            public static bool IsVisible = false;
            public static bool IsZeroLine = true;
        }
        #endregion
    }
}
