/*──────────────────────────────────────────────────────────────
 * FileName     : X2Axis
 * Created      : 2019-12-21 13:00:41
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
    /// 第二X轴
    /// </summary>
    public class X2Axis : Axis,ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public X2Axis()
            : this("X2 Axis")
        {
        }

        public X2Axis(string title)
            : base(title)
        {
            _isVisible = Default.IsVisible;
            _majorGrid.IsZeroLine = Default.IsZeroLine;
            _scale._fontSpec.Angle = 180F;
            _title._fontSpec.Angle = 180F;
        }

        public X2Axis(X2Axis rhs)
            : base(rhs)
        {
        }
        public X2Axis Clone()
        {
            return new X2Axis(this);
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods
        internal override bool IsPrimary(ChartFigure graph)
        {
            return this == graph.X2Axis;
        }

        public override void SetTransformMatrix(Graphics g, ChartFigure graph, float scaleFactor)
        {
            g.TranslateTransform(graph.Chart.Area.Right, graph.Chart.Area.Top);
            g.RotateTransform(180);
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
            public static bool IsVisible = false;
            public static bool IsZeroLine = false;
        }
        #endregion
    }
}
