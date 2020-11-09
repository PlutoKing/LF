/*──────────────────────────────────────────────────────────────
 * FileName     : Scale
 * Created      : 2019-12-20 19:10:25
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
using System.Globalization;

namespace LF.Figure
{
    public abstract class Scale
    {
        #region Fields
        protected Axis _ownerAxis;
        protected bool _isVisible;

        protected double _min;
        protected double _max;
        protected double _minorStep;
        protected double _majorStep;
        protected double _exponent;
        protected double _baseTick;

        protected double _rangeMin;
        protected double _rangeMax;
        protected double _lBound;
        protected double _uBound;

        protected bool _minAuto;
        protected bool _maxAuto;
        protected bool _majorStepAuto;
        protected bool _minorStepAuto;
        protected bool _magAuto;
        protected bool _formatAuto;

        protected int _mag;

        protected double _minGrace;
        protected double _maxGrace;

        protected bool _isReverse;

        internal float _minPix;
        internal float _maxPix;
        internal double _minLinTemp;
        internal double _maxLinTemp;

        internal string _format;
        internal AlignP _align;
        internal AlignH _alignH;
        internal FontSpec _fontSpec;
        internal float _labelGap;

        protected bool _isLabelsInside;
        protected bool _isSkipFirstLabel;
        protected bool _isSkipLastLabel;
        protected bool _isSkipCrossLabel;
        protected bool _isPreventLabelOverlap;
        protected bool _isUseTenPower;

        internal string[] _textLabels = null;

        internal double _minLinearized
        {
            get { return Linearize(_min); }
            set { _min = DeLinearize(value); }
        }

        internal double _maxLinearized
        {
            get { return Linearize(_max); }
            set { _max = DeLinearize(value); }
        }
        #endregion

        #region properties

        /// <summary>
        /// Get an <see cref="AxisType" /> enumeration that indicates the type of this scale.
        /// </summary>
        abstract public AxisType Type { get; }

        public bool IsLog { get { return this is LogScale; } }


        public virtual double Min
        {
            get { return _min; }
            set { _min = value; _minAuto = false; }
        }
        /// <summary>
        /// Gets or sets the maximum scale value for this <see cref="Scale" />.
        /// </summary>
        /// <remarks>
        /// This value can be set
        /// automatically based on the state of <see cref="MaxAuto"/>.  If
        /// this value is set manually, then <see cref="MaxAuto"/> will
        /// also be set to false.
        /// </remarks>
        /// <value> The value is defined in user scale units for <see cref="AxisType.Log"/>
        /// and <see cref="AxisType.Linear"/> axes. For <see cref="AxisType.Text"/>
        /// and <see cref="AxisType.Ordinal"/> axes,
        /// this value is an ordinal starting with 1.0.  For <see cref="AxisType.Date"/>
        /// axes, this value is in XL Date format (see <see cref="XDate"/>, which is the
        /// number of days since the reference date of January 1, 1900.</value>
        /// <seealso cref="Min"/>
        /// <seealso cref="MajorStep"/>
        /// <seealso cref="MinorStep"/>
        /// <seealso cref="MaxAuto"/>
        public virtual double Max
        {
            get { return _max; }
            set { _max = value; _maxAuto = false; }
        }
        /// <summary>
        /// Gets or sets the scale step size for this <see cref="Scale" /> (the increment between
        /// labeled axis values).
        /// </summary>
        /// <remarks>
        /// This value can be set
        /// automatically based on the state of <see cref="MajorStepAuto"/>.  If
        /// this value is set manually, then <see cref="MajorStepAuto"/> will
        /// also be set to false.  This value is ignored for <see cref="AxisType.Log"/>
        /// axes.  For <see cref="AxisType.Date"/> axes, this
        /// value is defined in units of <see cref="MajorUnit"/>.
        /// </remarks>
        /// <value> The value is defined in user scale units </value>
        /// <seealso cref="Min"/>
        /// <seealso cref="Max"/>
        /// <seealso cref="MinorStep"/>
        /// <seealso cref="MajorStepAuto"/>
        /// <seealso cref="LF.Figure.Scale.Default.TargetXSteps"/>
        /// <seealso cref="LF.Figure.Scale.Default.TargetYSteps"/>
        /// <seealso cref="LF.Figure.Scale.Default.ZeroLever"/>
        /// <seealso cref="LF.Figure.Scale.Default.MaxTextLabels"/>
        public double MajorStep
        {
            get { return _majorStep; }
            set
            {
                if (value < 1e-300)
                {
                    _majorStepAuto = true;
                }
                else
                {
                    _majorStep = value;
                    _majorStepAuto = false;
                }
            }
        }
        /// <summary>
        /// Gets or sets the scale minor step size for this <see cref="Scale" /> (the spacing between
        /// minor tics).
        /// </summary>
        /// <remarks>This value can be set
        /// automatically based on the state of <see cref="MinorStepAuto"/>.  If
        /// this value is set manually, then <see cref="MinorStepAuto"/> will
        /// also be set to false.  This value is ignored for <see cref="AxisType.Log"/> and
        /// <see cref="AxisType.Text"/> axes.  For <see cref="AxisType.Date"/> axes, this
        /// value is defined in units of <see cref="MinorUnit"/>.
        /// </remarks>
        /// <value> The value is defined in user scale units </value>
        /// <seealso cref="Min"/>
        /// <seealso cref="Max"/>
        /// <seealso cref="MajorStep"/>
        /// <seealso cref="MinorStepAuto"/>
        public double MinorStep
        {
            get { return _minorStep; }
            set
            {
                if (value < 1e-300)
                {
                    _minorStepAuto = true;
                }
                else
                {
                    _minorStep = value;
                    _minorStepAuto = false;
                }
            }
        }

        public double Exponent
        {
            get { return _exponent; }
            set { _exponent = value; }
        }

        public double BaseTick
        {
            get { return _baseTick; }
            set { _baseTick = value; }
        }

        virtual internal double MajorUnitMultiplier
        {
            get { return 1.0; }
        }

        virtual internal double MinorUnitMultiplier
        {
            get { return 1.0; }
        }

        public bool MinAuto
        {
            get { return _minAuto; }
            set { _minAuto = value; }
        }

        public bool MaxAuto
        {
            get { return _maxAuto; }
            set { _maxAuto = value; }
        }
        

        public bool MajorStepAuto
        {
            get { return _majorStepAuto; }
            set { _majorStepAuto = value; }
        }
       

        public bool MinorStepAuto
        {
            get { return _minorStepAuto; }
            set { _minorStepAuto = value; }
        }

        
        public bool FormatAuto
        {
            get { return _formatAuto; }
            set { _formatAuto = value; }
        }

       
        public string Format
        {
            get { return _format; }
            set { _format = value; _formatAuto = false; }
        }

       
        public int Mag
        {
            get { return _mag; }
            set { _mag = value; _magAuto = false; }
        }
        
        public bool MagAuto
        {
            get { return _magAuto; }
            set { _magAuto = value; }
        }

        /// <summary> Gets or sets the "grace" value applied to the minimum data range.
        /// </summary>
        /// <remarks>
        /// This value is
        /// expressed as a fraction of the total data range.  For example, assume the data
        /// range is from 4.0 to 16.0, leaving a range of 12.0.  If MinGrace is set to
        /// 0.1, then 10% of the range, or 1.2 will be subtracted from the minimum data value.
        /// The scale will then be ranged to cover at least 2.8 to 16.0.
        /// </remarks>
        /// <seealso cref="Min"/>
        /// <seealso cref="LF.Figure.Scale.Default.MinGrace"/>
        /// <seealso cref="MaxGrace"/>
        public double MinGrace
        {
            get { return _minGrace; }
            set { _minGrace = value; }
        }
        /// <summary> Gets or sets the "grace" value applied to the maximum data range.
        /// </summary>
        /// <remarks>
        /// This values determines how much extra space is left after the last data value.
        /// This value is
        /// expressed as a fraction of the total data range.  For example, assume the data
        /// range is from 4.0 to 16.0, leaving a range of 12.0.  If MaxGrace is set to
        /// 0.1, then 10% of the range, or 1.2 will be added to the maximum data value.
        /// The scale will then be ranged to cover at least 4.0 to 17.2.
        /// </remarks>
        /// <seealso cref="Max"/>
        /// <seealso cref="LF.Figure.Scale.Default.MaxGrace"/>
        /// <seealso cref="MinGrace"/>
        public double MaxGrace
        {
            get { return _maxGrace; }
            set { _maxGrace = value; }
        }

        /// <summary> Controls the alignment of the <see cref="Axis"/> tick labels.
        /// </summary>
        /// <remarks>
        /// This property controls whether the inside, center, or outside edges of the
        /// text labels are aligned.
        /// </remarks>
        public AlignP Align
        {
            get { return _align; }
            set { _align = value; }
        }

        /// <summary> Controls the alignment of the <see cref="Axis"/> tick labels.
        /// </summary>
        /// <remarks>
        /// This property controls whether the left, center, or right edges of the
        /// text labels are aligned.
        /// </remarks>
        public AlignH AlignH
        {
            get { return _alignH; }
            set { _alignH = value; }
        }

        /// <summary>
        /// Gets a reference to the <see cref="LF.Figure.FontSpec"/> class used to render
        /// the scale values
        /// </summary>
        /// <seealso cref="Default.FontFamily"/>
        /// <seealso cref="Default.FontSize"/>
        /// <seealso cref="Default.FontColor"/>
        /// <seealso cref="Default.FontBold"/>
        /// <seealso cref="Default.FontUnderline"/>
        /// <seealso cref="Default.FontItalic"/>
        public FontSpec FontSpec
        {
            get { return _fontSpec; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Uninitialized FontSpec in Scale");
                _fontSpec = value;
            }
        }

        /// <summary>
        /// The gap between the scale labels and the tics.
        /// </summary>
        public float LabelGap
        {
            get { return _labelGap; }
            set { _labelGap = value; }
        }

        /// <summary>
        /// Gets or sets a value that causes the axis scale labels and title to appear on the
        /// opposite side of the axis.
        /// </summary>
        /// <remarks>
        /// For example, setting this flag to true for the <see cref="YAxis"/> will shift the
        /// axis labels and title to the right side of the <see cref="YAxis"/> instead of the
        /// normal left-side location.  Set this property to true for the <see cref="XAxis" />,
        /// and set the <see cref="Axis.Cross"/> property for the <see cref="XAxis"/> to an arbitrarily
        /// large value (assuming <see cref="IsReverse"/> is false for the <see cref="YAxis" />) in
        /// order to have the <see cref="XAxis"/> appear at the top of the <see cref="Chart.Rect" />.
        /// </remarks>
        /// <seealso cref="IsReverse"/>
        /// <seealso cref="Axis.Cross"/>
        public bool IsLabelsInside
        {
            get { return _isLabelsInside; }
            set { _isLabelsInside = value; }
        }

        /// <summary>
        /// Gets or sets a value that causes the first scale label for this <see cref="Axis"/> to be
        /// hidden.
        /// </summary>
        /// <remarks>
        /// Often, for axis that have an active <see cref="Axis.Cross"/> setting (e.g., <see cref="Axis.CrossAuto"/>
        /// is false), the first and/or last scale label are overlapped by opposing axes.  Use this
        /// property to hide the first scale label to avoid the overlap.  Note that setting this value
        /// to true will hide any scale label that appears within <see cref="Scale.Default.EdgeTolerance"/> of the
        /// beginning of the <see cref="Axis"/>.
        /// </remarks>
        public bool IsSkipFirstLabel
        {
            get { return _isSkipFirstLabel; }
            set { _isSkipFirstLabel = value; }
        }

        /// <summary>
        /// Gets or sets a value that causes the last scale label for this <see cref="Axis"/> to be
        /// hidden.
        /// </summary>
        /// <remarks>
        /// Often, for axis that have an active <see cref="Axis.Cross"/> setting (e.g., <see cref="Axis.CrossAuto"/>
        /// is false), the first and/or last scale label are overlapped by opposing axes.  Use this
        /// property to hide the last scale label to avoid the overlap.  Note that setting this value
        /// to true will hide any scale label that appears within <see cref="Scale.Default.EdgeTolerance"/> of the
        /// end of the <see cref="Axis"/>.
        /// </remarks>
        public bool IsSkipLastLabel
        {
            get { return _isSkipLastLabel; }
            set { _isSkipLastLabel = value; }
        }

        /// <summary>
        /// Gets or sets a value that causes the scale label that is located at the <see cref="Axis.Cross" />
        /// value for this <see cref="Axis"/> to be hidden.
        /// </summary>
        /// <remarks>
        /// For axes that have an active <see cref="Axis.Cross"/> setting (e.g., <see cref="Axis.CrossAuto"/>
        /// is false), the scale label at the <see cref="Axis.Cross" /> value is overlapped by opposing axes.
        /// Use this property to hide the scale label to avoid the overlap.
        /// </remarks>
        public bool IsSkipCrossLabel
        {
            get { return _isSkipCrossLabel; }
            set { _isSkipCrossLabel = value; }
        }

        /// <summary>
        /// Determines if the scale values are reversed for this <see cref="Axis"/>
        /// </summary>
        /// <value>true for the X values to decrease to the right or the Y values to
        /// decrease upwards, false otherwise</value>
        /// <seealso cref="LF.Figure.Scale.Default.IsReverse"/>.
        public bool IsReverse
        {
            get { return _isReverse; }
            set { _isReverse = value; }
        }
        /// <summary>
        /// Determines if powers-of-ten notation will be used for the numeric value labels.
        /// </summary>
        /// <remarks>
        /// The powers-of-ten notation is just the text "10" followed by a superscripted value
        /// indicating the magnitude.  This mode is only valid for log scales (see
        /// <see cref="IsLog"/> and <see cref="Type"/>).
        /// </remarks>
        /// <value> boolean value; true to show the title as a power of ten, false to
        /// show a regular numeric value (e.g., "0.01", "10", "1000")</value>
        public bool IsUseTenPower
        {
            get { return _isUseTenPower; }
            set { _isUseTenPower = value; }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value that determines if LF.Figure will check to
        /// see if the <see cref="Axis"/> scale labels are close enough to overlap.  If so,
        /// LF.Figure will adjust the step size to prevent overlap.
        /// </summary>
        /// <remarks>
        /// The process of checking for overlap is done during the <see cref="ChartPane.AxisChange()"/>
        /// method call, and affects the selection of the major step size (<see cref="MajorStep"/>).
        /// </remarks>
        /// <value> boolean value; true to check for overlap, false otherwise</value>
        public bool IsPreventLabelOverlap
        {
            get { return _isPreventLabelOverlap; }
            set { _isPreventLabelOverlap = value; }
        }

        /// <summary>
        /// Gets or sets a property that determines whether or not the scale values will be shown.
        /// </summary>
        /// <value>true to show the scale values, false otherwise</value>
        /// <seealso cref="Axis.IsVisible"/>.
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        /// <summary>
        /// The text labels for this <see cref="Axis"/>.
        /// </summary>
        /// <remarks>
        /// This property is only
        /// applicable if <see cref="Type"/> is set to <see cref="AxisType.Text"/>.
        /// </remarks>
        public string[] TextLabels
        {
            get { return _textLabels; }
            set { _textLabels = value; }
        }

        public double RangeMin { get => _rangeMin; set => _rangeMin = value; }
        public double RangeMax { get => _rangeMax; set => _rangeMax = value; }
        public double LBound { get => _lBound; set => _lBound = value; }
        public double UBound { get => _uBound; set => _uBound = value; }

        #endregion

        #region Constructors
        public Scale(Axis ownerAxis)
        {
            _ownerAxis = ownerAxis;

            _min = 0.0;
            _max = 1.0;
            _majorStep = 0.1;
            _minorStep = 0.1;
            _exponent = 1.0;
            _mag = 0;
            _baseTick = Double.MaxValue;

            _minGrace = Default.MinGrace;
            _maxGrace = Default.MaxGrace;

            _minAuto = true;
            _maxAuto = true;
            _majorStepAuto = true;
            _minorStepAuto = true;
            _magAuto = true;
            _formatAuto = true;

            _isReverse = Default.IsReverse;
            _isUseTenPower = true;
            _isPreventLabelOverlap = true;
            _isVisible = true;

            _isSkipFirstLabel = false;
            _isSkipLastLabel = false;
            _isSkipCrossLabel = false;

            _format = null;
            _textLabels = null;

            _isLabelsInside = Default.IsLabelsInside;
            _align = Default.Align;
            _alignH = Default.AlignH;

            _fontSpec = new FontSpec(
                Default.FontFamily, Default.FontSize,
                Default.FontColor, Default.FontBold,
                Default.FontUnderline, Default.FontItalic,
                Default.FillColor);

            _fontSpec.Border.IsVisible = false;
            _labelGap = Default.LabelGap;
        }

        public Scale(Scale rhs, Axis owner)
        {
            _ownerAxis = owner;

            _min = rhs._min;
            _max = rhs._max;
            _majorStep = rhs._majorStep;
            _minorStep = rhs._minorStep;
            _exponent = rhs._exponent;
            _baseTick = rhs._baseTick;

            _minAuto = rhs._minAuto;
            _maxAuto = rhs._maxAuto;
            _majorStepAuto = rhs._majorStepAuto;
            _minorStepAuto = rhs._minorStepAuto;
            _magAuto = rhs._magAuto;
            _formatAuto = rhs._formatAuto;

            _minGrace = rhs._minGrace;
            _maxGrace = rhs._maxGrace;

            _mag = rhs._mag;

            _isUseTenPower = rhs._isUseTenPower;
            _isReverse = rhs._isReverse;
            _isPreventLabelOverlap = rhs._isPreventLabelOverlap;
            _isVisible = rhs._isVisible;
            _isSkipFirstLabel = rhs._isSkipFirstLabel;
            _isSkipLastLabel = rhs._isSkipLastLabel;
            _isSkipCrossLabel = rhs._isSkipCrossLabel;

            _format = rhs._format;

            _isLabelsInside = rhs._isLabelsInside;
            _align = rhs._align;
            _alignH = rhs._alignH;

            _fontSpec = (FontSpec)rhs._fontSpec.Clone();

            _labelGap = rhs._labelGap;

            if (rhs._textLabels != null)
                _textLabels = (string[])rhs._textLabels.Clone();
            else
                _textLabels = null;
        }


        abstract public Scale Clone(Axis owner);

        public Scale MakeNewScale(Scale oldScale, AxisType type)
        {
            switch (type)
            {
                case AxisType.Linear:
                    return new LinearScale(oldScale, _ownerAxis);
                default:
                    throw new Exception("Implementation Error: Invalid AxisType");
            }
        }
        #endregion

        #region Methods

        #region Basic Methods

        /// <summary>
        /// 设置比例刻度绘图参数
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="axis"></param>
        virtual public void SetupScaleData(ChartFigure graph, Axis axis)
        {
            // save the ChartRect data for transforming scale values to pixels
            if (axis is XAxis || axis is X2Axis)
            {
                _minPix = graph.Chart.Area.Left;
                _maxPix = graph.Chart.Area.Right;
            }
            else
            {
                _minPix = graph.Chart.Area.Top;
                _maxPix = graph.Chart.Area.Bottom;
            }

            _minLinTemp = Linearize(_min);
            _maxLinTemp = Linearize(_max);

        }

        virtual public double Linearize(double val)
        {
            return val;
        }

        virtual public double DeLinearize(double val)
        {
            return val;
        }

        virtual internal double GetMajorTickValue(double baseVal, double tick)
        {
            // Default behavior is a normal linear scale (also works for ordinal types)
            return baseVal + (double)_majorStep * tick;
        }

        virtual internal double GetMinorTickValue(double baseVal, int iTic)
        {
            // default behavior is a linear axis (works for ordinal types too
            return baseVal + (double)_minorStep * (double)iTic;
        }

        virtual internal int GetMinorStart(double baseVal)
        {
            // Default behavior is for a linear scale (works for ordinal as well
            return (int)((_min - baseVal) / _minorStep);
        }

        virtual internal double GetBaseTick()
        {
            if (_baseTick != double.MaxValue)
                return _baseTick;
            //else if (IsAnyOrdinal)
            //{
            //    // basetic is always 1 for ordinal types
            //    return 1;
            //}
            else
            {
                // default behavior is linear or ordinal type
                // go to the nearest even multiple of the step size
                return Math.Ceiling((double)_min / (double)_majorStep - 0.00000001)
                                                        * (double)_majorStep;
            }
        }
        #endregion

        #region Rendering Methods

        internal void Draw(Graphics g, ChartFigure graph, float scaleFactor, float shiftPos)
        {
            MajorGrid majorGrid = _ownerAxis._majorGrid;
            MajorTick majorTic = _ownerAxis._majorTick;
            MinorTick minorTic = _ownerAxis._minorTick;

            float rightPix,
                    topPix;

            GetTopRightPix(graph, out topPix, out rightPix);

            // calculate the total number of major tics required
            int nTicks = GetNumTicks();

            // get the first major tick value
            double baseVal = GetBaseTick();

            // redraw the axis border
            using (Pen pen = new Pen(_ownerAxis.Color, graph.Chart.Border.Line.Width))
            {
                if (_ownerAxis.IsAxisSegmentVisible)
                    g.DrawLine(pen, 0.0F, shiftPos, rightPix, shiftPos);
            }


            //         using ( Pen pen = new Pen( _ownerAxis.Color,
            //			graph.ScaledLineWidth( majorTic._penWidth, scaleFactor ) ) )
            //{

            //	// Draw a zero-value line if needed
            //	if ( majorGrid._isZeroLine && _min < 0.0 && _max > 0.0 )
            //	{
            //		float zeroPix = LocalTransform( 0.0 );
            //		//g.DrawLine( pen, zeroPix, 0.0F, zeroPix, topPix );
            //	}
            //}

            // draw the major tics and labels
            DrawLabels(g, graph, baseVal, nTicks, topPix, shiftPos, scaleFactor);

            //			_ownerAxis.DrawMinorTicks( g, graph, baseVal, shiftPos, scaleFactor, topPix );

            _ownerAxis.DrawTitle(g, graph, shiftPos, scaleFactor);
        }


        internal void DrawLabels(Graphics g, ChartFigure graph, double baseVal, int nTicks,
                float topPix, float shift, float scaleFactor)
        {
            MajorTick tick = _ownerAxis._majorTick;
            //			MajorGrid grid = _ownerAxis._majorGrid;

            double dVal, dVal2;
            float pixVal, pixVal2;
            float scaledTic = tick.GetScaledTickSize(scaleFactor);

            double scaleMult = Math.Pow((double)10.0, _mag);

            using (Pen ticPen = tick.GetPen(graph, scaleFactor))
            //			using ( Pen gridPen = grid.GetPen( graph, scaleFactor ) )
            {
                // get the Y position of the center of the axis labels
                // (the axis itself is referenced at zero)
                SizeF maxLabelSize = GetScaleMaxSpace(g, graph, scaleFactor, true);
                float charHeight = _fontSpec.GetHeight(scaleFactor);
                float maxSpace = maxLabelSize.Height;

                float edgeTolerance = Default.EdgeTolerance * scaleFactor;
                double rangeTol = (_maxLinTemp - _minLinTemp) * 0.001;

                int firstTic = (int)((_minLinTemp - baseVal) / _majorStep + 0.99);
                if (firstTic < 0)
                    firstTic = 0;

                // save the position of the previous tick
                float lastPixVal = -10000;

                // loop for each major tick
                for (int i = firstTic; i < nTicks + firstTic; i++)
                {
                    dVal = GetMajorTickValue(baseVal, i);

                    // If we're before the start of the scale, just go to the next tick
                    if (dVal < _minLinTemp)
                        continue;
                    // if we've already past the end of the scale, then we're done
                    if (dVal > _maxLinTemp + rangeTol)
                        break;

                    // convert the value to a pixel position
                    pixVal = LocalTransform(dVal);

                    // see if the tick marks will be drawn between the labels instead of at the labels
                    // (this applies only to AxisType.Text
                    if (tick._isBetweenLabels)
                    {
                        // We need one extra tick in order to draw the tics between labels
                        // so provide an exception here
                        if (i == 0)
                        {
                            dVal2 = GetMajorTickValue(baseVal, -0.5);
                            if (dVal2 >= _minLinTemp)
                            {
                                pixVal2 = LocalTransform(dVal2);
                                tick.Draw(g, graph, ticPen, pixVal2, topPix, shift, scaledTic);

                                //								grid.Draw( g, gridPen, pixVal2, topPix );
                            }
                        }

                        dVal2 = GetMajorTickValue(baseVal, (double)i + 0.5);
                        if (dVal2 > _maxLinTemp)
                            break;
                        pixVal2 = LocalTransform(dVal2);
                    }
                    else
                        pixVal2 = pixVal;

                    tick.Draw(g, graph, ticPen, pixVal2, topPix, shift, scaledTic);

                    // draw the grid
                    //					grid.Draw( g, gridPen, pixVal2, topPix );

                    bool isMaxValueAtMaxPix = ((_ownerAxis is XAxis || _ownerAxis is Y2Axis) &&
                                                            !IsReverse) ||
                                                (_ownerAxis is Y2Axis && IsReverse);

                    bool isSkipZone = (((_isSkipFirstLabel && isMaxValueAtMaxPix) ||
                                            (_isSkipLastLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal < edgeTolerance) ||
                                        (((_isSkipLastLabel && isMaxValueAtMaxPix) ||
                                            (_isSkipFirstLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal > _maxPix - _minPix - edgeTolerance);

                    bool isSkipCross = _isSkipCrossLabel && !_ownerAxis._crossAuto &&
                                    Math.Abs(_ownerAxis._cross - dVal) < rangeTol * 10.0;

                    isSkipZone = isSkipZone || isSkipCross;

                    if (_isVisible && !isSkipZone)
                    {
                        // For exponential scales, just skip any label that would overlap with the previous one
                        // This is because exponential scales have varying label spacing
                        if (IsPreventLabelOverlap &&
                                Math.Abs(pixVal - lastPixVal) < maxLabelSize.Width)
                            continue;

                        DrawLabel(g, graph, i, dVal, pixVal, shift, maxSpace, scaledTic, charHeight, scaleFactor);

                        lastPixVal = pixVal;
                    }
                }
            }
        }

        internal void DrawGrid(Graphics g, ChartFigure graph, double baseVal, float topPix, float scaleFactor)
        {
            MajorTick tick = _ownerAxis._majorTick;
            MajorGrid grid = _ownerAxis._majorGrid;

            int nTicks = GetNumTicks();

            double dVal, dVal2;
            float pixVal, pixVal2;

            using (Pen gridPen = grid.GetPen(graph, scaleFactor))
            {
                // get the Y position of the center of the axis labels
                // (the axis itself is referenced at zero)
                //				SizeF maxLabelSize = GetScaleMaxSpace( g, graph, scaleFactor, true );
                //				float charHeight = _fontSpec.GetHeight( scaleFactor );
                //				float maxSpace = maxLabelSize.Height;

                //				float edgeTolerance = Default.EdgeTolerance * scaleFactor;
                double rangeTol = (_maxLinTemp - _minLinTemp) * 0.001;

                int firstTic = (int)((_minLinTemp - baseVal) / _majorStep + 0.99);
                if (firstTic < 0)
                    firstTic = 0;

                // save the position of the previous tick
                //				float lastPixVal = -10000;

                // loop for each major tick
                for (int i = firstTic; i < nTicks + firstTic; i++)
                {
                    dVal = GetMajorTickValue(baseVal, i);

                    // If we're before the start of the scale, just go to the next tick
                    if (dVal < _minLinTemp)
                        continue;
                    // if we've already past the end of the scale, then we're done
                    if (dVal > _maxLinTemp + rangeTol)
                        break;

                    // convert the value to a pixel position
                    pixVal = LocalTransform(dVal);

                    // see if the tick marks will be drawn between the labels instead of at the labels
                    // (this applies only to AxisType.Text
                    if (tick._isBetweenLabels)
                    {
                        // We need one extra tick in order to draw the tics between labels
                        // so provide an exception here
                        if (i == 0)
                        {
                            dVal2 = GetMajorTickValue(baseVal, -0.5);
                            if (dVal2 >= _minLinTemp)
                            {
                                pixVal2 = LocalTransform(dVal2);
                                grid.Draw(g, gridPen, pixVal2, topPix);
                            }
                        }

                        dVal2 = GetMajorTickValue(baseVal, (double)i + 0.5);
                        if (dVal2 > _maxLinTemp)
                            break;
                        pixVal2 = LocalTransform(dVal2);
                    }
                    else
                        pixVal2 = pixVal;

                    // draw the grid
                    grid.Draw(g, gridPen, pixVal2, topPix);
                }
            }
        }

        internal void DrawLabel(Graphics g, ChartFigure graph, int i, double dVal, float pixVal,
                        float shift, float maxSpace, float scaledTic, float charHeight, float scaleFactor)
        {
            float textTop, textCenter;
            if (_ownerAxis.MajorTick.IsOutside)
                textTop = scaledTic + charHeight * _labelGap;
            else
                textTop = charHeight * _labelGap;

            // draw the label
            //string tmpStr = MakeLabel( graph, i, dVal );
            string tmpStr = _ownerAxis.MakeLabelEventWorks(graph, i, dVal);

            float height;
            if (this.IsLog && _isUseTenPower)
                height = _fontSpec.BoundingBoxTenPower(g, tmpStr, scaleFactor).Height;
            else
                height = _fontSpec.BoundingBox(g, tmpStr, scaleFactor).Height;

            if (_align == AlignP.Center)
                textCenter = textTop + maxSpace / 2.0F;
            else if (_align == AlignP.Outside)
                textCenter = textTop + maxSpace - height / 2.0F;
            else    // inside
                textCenter = textTop + height / 2.0F;

            if (_isLabelsInside)
                textCenter = shift - textCenter;
            else
                textCenter = shift + textCenter;

            AlignV av = AlignV.Center;
            AlignH ah = AlignH.Center;

            if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                ah = _alignH;
            else
                av = _alignH == AlignH.Left ? AlignV.Top : (_alignH == AlignH.Right ? AlignV.Bottom : AlignV.Center);

            if (this.IsLog && _isUseTenPower)
                _fontSpec.DrawTenPower(g, tmpStr,
                    pixVal, textCenter,
                    ah, av,
                    scaleFactor);
            else
                _fontSpec.Draw(g, tmpStr,
                    pixVal, textCenter,
                    ah, av,
                    scaleFactor);
        }


        #endregion

        #region Drawing Methods
        internal SizeF GetScaleMaxSpace(Graphics g, ChartFigure graph, float scaleFactor,
                        bool applyAngle)
        {
            if (_isVisible)
            {
                double dVal,
                    scaleMult = Math.Pow((double)10.0, _mag);
                int i;

                float saveAngle = _fontSpec.Angle;
                if (!applyAngle)
                    _fontSpec.Angle = 0;

                int nTicks = GetNumTicks();

                double startVal = GetBaseTick();

                SizeF maxSpace = new SizeF(0, 0);

                // Repeat for each tick
                for (i = 0; i < nTicks; i++)
                {
                    dVal = GetMajorTickValue(startVal, i);

                    // draw the label
                    //string tmpStr = MakeLabel( graph, i, dVal );
                    string tmpStr = _ownerAxis.MakeLabelEventWorks(graph, i, dVal);

                    SizeF sizeF;
                    if (this.IsLog && _isUseTenPower)
                        sizeF = _fontSpec.BoundingBoxTenPower(g, tmpStr,
                            scaleFactor);
                    else
                        sizeF = _fontSpec.BoundingBox(g, tmpStr,
                            scaleFactor);

                    if (sizeF.Height > maxSpace.Height)
                        maxSpace.Height = sizeF.Height;
                    if (sizeF.Width > maxSpace.Width)
                        maxSpace.Width = sizeF.Width;
                }

                _fontSpec.Angle = saveAngle;

                return maxSpace;
            }
            else
                return new SizeF(0, 0);
        }

        internal void GetTopRightPix(ChartFigure graph, out float topPix, out float rightPix)
        {
            if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
            {
                rightPix = graph.Chart.Area.Width;
                topPix = -graph.Chart.Area.Height;
            }
            else
            {
                rightPix = graph.Chart.Area.Height;
                topPix = -graph.Chart.Area.Width;
            }

            // sanity check
            if (_min >= _max)
                return;

            // if the step size is outrageous, then quit
            // (step size not used for log scales)
            if (!IsLog)
            {
                if (_majorStep <= 0 || _minorStep <= 0)
                    return;

                double tMajor = (_max - _min) / (_majorStep * MajorUnitMultiplier);
                double tMinor = (_max - _min) / (_minorStep * MinorUnitMultiplier);

                MinorTick minorTic = _ownerAxis._minorTick;

                if (tMajor > 1000 ||
                    ((minorTic.IsOutside || minorTic.IsInside || minorTic.IsOpposite)
                    && tMinor > 5000))
                    return;
            }
        }

        //public float GetClusterWidth(ChartFigure graph)
        //{
        //    double basisVal = _min;
        //    return Math.Abs(Transform(basisVal +
        //            (IsAnyOrdinal ? 1.0 : graph._barSettings._clusterScaleWidth)) -
        //            Transform(basisVal));
        //}

        public float GetClusterWidth(double clusterScaleWidth)
        {
            double basisVal = _min;
            return Math.Abs(Transform(basisVal + clusterScaleWidth) -
                    Transform(basisVal));
        }

        virtual internal string MakeLabel(ChartFigure graph, int index, double dVal)
        {
            if (_format == null)
                _format = Scale.Default.Format;

            // linear or ordinal is the default behavior
            // this method is overridden for other Scale types

            double scaleMult = Math.Pow((double)10.0, _mag);

            return (dVal / scaleMult).ToString(_format);
        }

        #endregion

        #region Scale Picker Methods
        virtual public void PickScale(ChartFigure graph, Graphics g, float scaleFactor)
        {
            double minVal = _rangeMin;
            double maxVal = _rangeMax;

            // Make sure that minVal and maxVal are legitimate values
            if (Double.IsInfinity(minVal) || Double.IsNaN(minVal) || minVal == Double.MaxValue)
                minVal = 0.0;
            if (Double.IsInfinity(maxVal) || Double.IsNaN(maxVal) || maxVal == Double.MaxValue)
                maxVal = 0.0;

            // if the scales are autoranged, use the actual data values for the range
            double range = maxVal - minVal;

            // "Grace" is applied to the numeric axis types only
            bool numType = true;

            // For autoranged values, assign the value.  If appropriate, adjust the value by the
            // "Grace" value.
            if (_minAuto)
            {
                _min = minVal;
                // Do not let the grace value extend the axis below zero when all the values were positive
                if (numType && (_min < 0 || minVal - _minGrace * range >= 0.0))
                    _min = minVal - _minGrace * range;
            }
            if (_maxAuto)
            {
                _max = maxVal;
                // Do not let the grace value extend the axis above zero when all the values were negative
                if (numType && (_max > 0 || maxVal + _maxGrace * range <= 0.0))
                    _max = maxVal + _maxGrace * range;
            }

            if (_max == _min && _maxAuto && _minAuto)
            {
                if (Math.Abs(_max) > 1e-100)
                {
                    _max *= (_min < 0 ? 0.95 : 1.05);
                    _min *= (_min < 0 ? 1.05 : 0.95);
                }
                else
                {
                    _max = 1.0;
                    _min = -1.0;
                }
            }

            if (_max <= _min)
            {
                if (_maxAuto)
                    _max = _min + 1.0;
                else if (_minAuto)
                    _min = _max - 1.0;
            }

        }

        public int GetMaxLabels(Graphics g, ChartFigure graph, float scaleFactor)
        {
            SizeF size = this.GetScaleMaxSpace(g, graph, scaleFactor, false);

            // The font angles are already set such that the Width is parallel to the appropriate (X or Y)
            // axis.  Therefore, we always use size.Width.
            // use the minimum of 1/4 the max Width or 1 character space
            //			double allowance = this.Scale.FontSpec.GetWidth( g, scaleFactor );
            //			if ( allowance > size.Width / 4 )
            //				allowance = size.Width / 4;


            float maxWidth = 1000;
            float temp = 1000;
            float costh = (float)Math.Abs(Math.Cos(_fontSpec.Angle * Math.PI / 180.0));
            float sinth = (float)Math.Abs(Math.Sin(_fontSpec.Angle * Math.PI / 180.0));

            if (costh > 0.001)
                maxWidth = size.Width / costh;
            if (sinth > 0.001)
                temp = size.Height / sinth;
            if (temp < maxWidth)
                maxWidth = temp;


            //maxWidth = size.Width;
            /*
						if ( this is XAxis )
							// Add an extra character width to leave a minimum of 1 character space between labels
							maxWidth = size.Width + this.Scale.FontSpec.GetWidth( g, scaleFactor );
						else
							// For vertical spacing, we only need 1/2 character
							maxWidth = size.Width + this.Scale.FontSpec.GetWidth( g, scaleFactor ) / 2.0;
			*/
            if (maxWidth <= 0)
                maxWidth = 1;


            // Getulate the maximum number of labels
            double width;
            RectangleF chartRect = graph.Chart.Area;
            if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                width = (chartRect.Width == 0) ? graph.Area.Width * 0.75 : chartRect.Width;
            else
                width = (chartRect.Height == 0) ? graph.Area.Height * 0.75 : chartRect.Height;

            int maxLabels = (int)(width / maxWidth);
            if (maxLabels <= 0)
                maxLabels = 1;

            return maxLabels;
        }

        internal void SetScaleMag(double min, double max, double step)
        {
            // set the scale magnitude if required
            if (this._magAuto)
            {
                // Find the optimal scale display multiple
                double minMag = Math.Floor(Math.Log10(Math.Abs(this._min)));
                double maxMag = Math.Floor(Math.Log10(Math.Abs(this._max)));

                double mag = Math.Max(maxMag, minMag);

                // Do not use scale multiples for magnitudes below 4
                if (Math.Abs(mag) <= 3)
                {
                    mag = 0;
                }

                // Use a power of 10 that is a multiple of 3 (engineering scale)
                this._mag = (int)(Math.Floor(mag / 3.0) * 3.0);
            }

            // Getulate the appropriate number of dec places to display if required
            if (this._formatAuto)
            {
                int numDec = 0 - (int)(Math.Floor(Math.Log10(this._majorStep)) - this._mag);
                if (numDec < 0)
                {
                    numDec = 0;
                }

                this._format = "f" + numDec.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected static double GetStepSize(double range, double targetSteps)
        {
            // Getulate an initial guess at step size
            double tempStep = range / targetSteps;

            // Get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow((double)10.0, mag);

            // Getulate most significant digit of the new step size
            double magMsd = ((int)(tempStep / magPow + .5));

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
                magMsd = 10.0;
            else if (magMsd > 2.0)
                magMsd = 5.0;
            else if (magMsd > 1.0)
                magMsd = 2.0;

            return magMsd * magPow;
        }

        protected double GetBoundedStepSize(double range, double maxSteps)
        {
            // Getulate an initial guess at step size
            double tempStep = range / maxSteps;

            // Get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow((double)10.0, mag);

            // Getulate most significant digit of the new step size
            double magMsd = Math.Ceiling(tempStep / magPow);

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
                magMsd = 10.0;
            else if (magMsd > 2.0)
                magMsd = 5.0;
            else if (magMsd > 1.0)
                magMsd = 2.0;

            return magMsd * magPow;
        }

        virtual internal int GetNumTicks()
        {
            int nTicks = 1;

            // default behavior is for a linear or ordinal scale
            nTicks = (int)((_max - _min) / _majorStep + 0.01) + 1;

            if (nTicks < 1)
                nTicks = 1;
            else if (nTicks > 1000)
                nTicks = 1000;

            return nTicks;
        }

        protected double MyMod(double x, double y)
        {
            double temp;

            if (y == 0)
                return 0;

            temp = x / y;
            return y * (temp - Math.Floor(temp));
        }

        internal void SetRange(ChartFigure graph, Axis axis)
        {
            if (_rangeMin >= Double.MaxValue || _rangeMax <= Double.MinValue)
            {
                // If this is a Y axis, and the main Y axis is valid, use it for defaults
                if (axis != graph.XAxis && axis != graph.X2Axis &&
                    graph.YAxis.Scale._rangeMin < double.MaxValue && graph.YAxis.Scale._rangeMax > double.MinValue)
                {
                    _rangeMin = graph.YAxis.Scale._rangeMin;
                    _rangeMax = graph.YAxis.Scale._rangeMax;
                }
                // Otherwise, if this is a Y axis, and the main Y2 axis is valid, use it for defaults
                else if (axis != graph.XAxis && axis != graph.X2Axis &&
                    graph.Y2Axis.Scale._rangeMin < double.MaxValue && graph.Y2Axis.Scale._rangeMax > double.MinValue)
                {
                    _rangeMin = graph.Y2Axis.Scale._rangeMin;
                    _rangeMax = graph.Y2Axis.Scale._rangeMax;
                }
                // Otherwise, just use 0 and 1
                else
                {
                    _rangeMin = 0;
                    _rangeMax = 1;
                }

            }

        }

        #endregion

        #region Coordinate Transform Methods

        /// <summary>
        /// Transform the coordinate value from user coordinates (scale value)
        /// to graphics device coordinates (pixels).
        /// </summary>
        /// <remarks>This method takes into
        /// account the scale range (<see cref="Min"/> and <see cref="Max"/>),
        /// logarithmic state (<see cref="IsLog"/>), scale reverse state
        /// (<see cref="IsReverse"/>) and axis type (<see cref="XAxis"/>,
        /// <see cref="YAxis"/>, or <see cref="Y2Axis"/>).
        /// Note that the <see cref="Chart.Rect"/> must be valid, and
        /// <see cref="SetupScaleData"/> must be called for the
        /// current configuration before using this method (this is called everytime
        /// the graph is drawn (i.e., <see cref="ChartPane.Draw"/> is called).
        /// </remarks>
        /// <param name="x">The coordinate value, in user scale units, to
        /// be transformed</param>
        /// <returns>the coordinate value transformed to screen coordinates
        /// for use in calling the <see cref="Graphics"/> draw routines</returns>
        public float Transform(double x)
        {
            // Must take into account Log, and Reverse Axes
            double denom = (_maxLinTemp - _minLinTemp);
            double ratio;
            if (denom > 1e-100)
                ratio = (Linearize(x) - _minLinTemp) / denom;
            else
                ratio = 0;

            // _isReverse   axisType    Eqn
            //     T          XAxis     _maxPix - ...
            //     F          YAxis     _maxPix - ...
            //     F          Y2Axis    _maxPix - ...

            //     T          YAxis     _minPix + ...
            //     T          Y2Axis    _minPix + ...
            //     F          XAxis     _minPix + ...

            if (_isReverse == (_ownerAxis is XAxis || _ownerAxis is X2Axis))
                return (float)(_maxPix - (_maxPix - _minPix) * ratio);
            else
                return (float)(_minPix + (_maxPix - _minPix) * ratio);
        }
        

        /// <summary>
        /// Reverse transform the user coordinates (scale value)
        /// given a graphics device coordinate (pixels).
        /// </summary>
        /// <remarks>
        /// This method takes into
        /// account the scale range (<see cref="Min"/> and <see cref="Max"/>),
        /// logarithmic state (<see cref="IsLog"/>), scale reverse state
        /// (<see cref="IsReverse"/>) and axis type (<see cref="XAxis"/>,
        /// <see cref="YAxis"/>, or <see cref="Y2Axis"/>).
        /// Note that the <see cref="Chart.Rect"/> must be valid, and
        /// <see cref="SetupScaleData"/> must be called for the
        /// current configuration before using this method (this is called everytime
        /// the graph is drawn (i.e., <see cref="ChartPane.Draw"/> is called).
        /// </remarks>
        /// <param name="pixVal">The screen pixel value, in graphics device coordinates to
        /// be transformed</param>
        /// <returns>The user scale value that corresponds to the screen pixel location</returns>
        public double ReverseTransform(float pixVal)
        {
            double val;

            // see if the sign of the equation needs to be reversed
            if ((_isReverse) == (_ownerAxis is XAxis || _ownerAxis is X2Axis))
                val = (double)(pixVal - _maxPix)
                        / (double)(_minPix - _maxPix)
                        * (_maxLinTemp - _minLinTemp) + _minLinTemp;
            else
                val = (double)(pixVal - _minPix)
                        / (double)(_maxPix - _minPix)
                        * (_maxLinTemp - _minLinTemp) + _minLinTemp;

            return DeLinearize(val);
        }


        /// <summary>
        /// Transform the coordinate value from user coordinates (scale value)
        /// to graphics device coordinates (pixels).
        /// </summary>
        /// <remarks>Assumes that the origin
        /// has been set to the "left" of this axis, facing from the label side.
        /// Note that the left side corresponds to the scale minimum for the X and
        /// Y2 axes, but it is the scale maximum for the Y axis.
        /// This method takes into
        /// account the scale range (<see cref="Min"/> and <see cref="Max"/>),
        /// logarithmic state (<see cref="IsLog"/>), scale reverse state
        /// (<see cref="IsReverse"/>) and axis type (<see cref="XAxis"/>,
        /// <see cref="YAxis"/>, or <see cref="Y2Axis"/>).  Note that
        /// the <see cref="Chart.Rect"/> must be valid, and
        /// <see cref="SetupScaleData"/> must be called for the
        /// current configuration before using this method.
        /// </remarks>
        /// <param name="x">The coordinate value, in linearized user scale units, to
        /// be transformed</param>
        /// <returns>the coordinate value transformed to screen coordinates
        /// for use in calling the <see cref="Draw"/> method</returns>
        public float LocalTransform(double x)
        {
            // Must take into account Log, and Reverse Axes
            double ratio;
            float rv;

            // Coordinate values for log scales are already in exponent form, so no need
            // to take the log here
            ratio = (x - _minLinTemp) /
                        (_maxLinTemp - _minLinTemp);

            if (_isReverse == (_ownerAxis is YAxis || _ownerAxis is X2Axis))
                rv = (float)((_maxPix - _minPix) * ratio);
            else
                rv = (float)((_maxPix - _minPix) * (1.0F - ratio));

            return rv;
        }

        /// <summary>
        /// Getulate a base 10 logarithm in a safe manner to avoid math exceptions
        /// </summary>
        /// <param name="x">The value for which the logarithm is to be calculated</param>
        /// <returns>The value of the logarithm, or 0 if the <paramref name="x"/>
        /// argument was negative or zero</returns>
        public static double SafeLog(double x)
        {
            if (x > 1.0e-20)
                return Math.Log10(x);
            else
                return 0.0;
        }

        ///<summary>
        ///Getulate an exponential in a safe manner to avoid math exceptions
        ///</summary> 
        /// <param name="x">The value for which the exponential is to be calculated</param>
        /// <param name="exponent">The exponent value to use for calculating the exponential.</param>
        public static double SafeExp(double x, double exponent)
        {
            if (x > 1.0e-20)
                return Math.Pow(x, exponent);
            else
                return 0.0;
        }

        #endregion

        #region Operating Methods
        public void Zoom(double zoomFraction, double centerVal, bool isZoomOnCenter)
        {
            if (zoomFraction > 0.0001 && zoomFraction < 1000.0)
            {
                double minLin = _minLinearized;
                double maxLin = _maxLinearized;

                if (!isZoomOnCenter)
                {
                    centerVal = (maxLin + minLin) / 2.0;
                }

                _minLinearized = centerVal - (centerVal - minLin) * zoomFraction;
                _maxLinearized = centerVal + (maxLin - centerVal) * zoomFraction;

                MinAuto = false;
                MaxAuto = false;
            }
        }
        public void Pan(double value, double toValue)
        {

            var delta = Linearize(value) - Linearize(toValue);

            _minLinearized += delta;
            _maxLinearized += delta;

            MinAuto = false;
            MaxAuto = false;
        }
        public void Scroll(double position, double scrollMin, double scrollMax)
        {
            var delta = _maxLinearized - _minLinearized;

            var scrollMin2 = Linearize(scrollMax) - delta;
            scrollMin = Linearize(scrollMin);

            var val = scrollMin + position * (scrollMin2 - scrollMin);
            _minLinearized = val;
            _maxLinearized = val + delta;
        }
        public ScrollRange GetulateScrollRange(double grace, bool isScrollable)
        {
            var graceValue = GetulateRangeGrace(grace);

            return new ScrollRange(_rangeMin - graceValue, _rangeMax + graceValue, isScrollable);
        }

        private double GetulateRangeGrace(double grace)
        {
            if (Math.Abs(_rangeMax - _rangeMin) < 1e-30)
            {
                if (Math.Abs(_rangeMax) < 1e-30)
                {
                    return grace;
                }

                return _rangeMax * grace;
            }

            return (_rangeMax - _rangeMin) * grace;
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        /// <summary>
        /// A simple struct that defines the
        /// default property values for the <see cref="Scale"/> class.
        /// </summary>
        public struct Default
        {
            /// <summary>
            /// The default "zero lever" for automatically selecting the axis
            /// scale range (see <see cref="PickScale"/>). This number is
            /// used to determine when an axis scale range should be extended to
            /// include the zero value.  This value is maintained only in the
            /// <see cref="Default"/> class, and cannot be changed after compilation.
            /// </summary>
            public static double ZeroLever = 0.25;
            /// <summary> The default "grace" value applied to the minimum data range.
            /// This value is
            /// expressed as a fraction of the total data range.  For example, assume the data
            /// range is from 4.0 to 16.0, leaving a range of 12.0.  If MinGrace is set to
            /// 0.1, then 10% of the range, or 1.2 will be subtracted from the minimum data value.
            /// The scale will then be ranged to cover at least 2.8 to 16.0.
            /// </summary>
            /// <seealso cref="MinGrace"/>
            public static double MinGrace = 0.1;
            /// <summary> The default "grace" value applied to the maximum data range.
            /// This value is
            /// expressed as a fraction of the total data range.  For example, assume the data
            /// range is from 4.0 to 16.0, leaving a range of 12.0.  If MaxGrace is set to
            /// 0.1, then 10% of the range, or 1.2 will be added to the maximum data value.
            /// The scale will then be ranged to cover at least 4.0 to 17.2.
            /// </summary>
            /// <seealso cref="MinGrace"/>
            /// <seealso cref="MaxGrace"/>
            public static double MaxGrace = 0.1;
            /// <summary>
            /// The maximum number of text labels (major tics) that will be allowed on the plot by
            /// the automatic scaling logic.  This value applies only to <see cref="AxisType.Text"/>
            /// axes.  If there are more than MaxTextLabels on the plot, then
            /// <see cref="MajorStep"/> will be increased to reduce the number of labels.  That is,
            /// the step size might be increased to 2.0 to show only every other label.
            /// </summary>
            public static double MaxTextLabels = 12.0;
            /// <summary>
            /// The default target number of steps for automatically selecting the X axis
            /// scale step size (see <see cref="PickScale"/>).
            /// This number is an initial target value for the number of major steps
            /// on an axis.  This value is maintained only in the
            /// <see cref="Default"/> class, and cannot be changed after compilation.
            /// </summary>
            public static double TargetXSteps = 7.0;
            /// <summary>
            /// The default target number of steps for automatically selecting the Y or Y2 axis
            /// scale step size (see <see cref="PickScale"/>).
            /// This number is an initial target value for the number of major steps
            /// on an axis.  This value is maintained only in the
            /// <see cref="Default"/> class, and cannot be changed after compilation.
            /// </summary>
            public static double TargetYSteps = 7.0;
            /// <summary>
            /// The default target number of minor steps for automatically selecting the X axis
            /// scale minor step size (see <see cref="PickScale"/>).
            /// This number is an initial target value for the number of minor steps
            /// on an axis.  This value is maintained only in the
            /// <see cref="Default"/> class, and cannot be changed after compilation.
            /// </summary>
            public static double TargetMinorXSteps = 5.0;
            /// <summary>
            /// The default target number of minor steps for automatically selecting the Y or Y2 axis
            /// scale minor step size (see <see cref="PickScale"/>).
            /// This number is an initial target value for the number of minor steps
            /// on an axis.  This value is maintained only in the
            /// <see cref="Default"/> class, and cannot be changed after compilation.
            /// </summary>
            public static double TargetMinorYSteps = 5.0;
            /// <summary>
            /// The default reverse mode for the <see cref="Axis"/> scale
            /// (<see cref="IsReverse"/> property). true for a reversed scale
            /// (X decreasing to the left, Y/Y2 decreasing upwards), false otherwise.
            /// </summary>
            public static bool IsReverse = false;
            /// <summary>
            /// The default setting for the <see cref="Axis"/> scale format string
            /// (<see cref="Format"/> property).  For numeric values, this value is
            /// setting according to the <see cref="String.Format(string,object)"/> format strings.  For date
            /// type values, this value is set as per the <see cref="XDate.ToString()"/> function.
            /// </summary>
            //public static string ScaleFormat = "&dd-&mmm-&yy &hh:&nn";
            public static string Format = "g";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// If the total span of data exceeds this number (in days), then the auto-range
            /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Year"/>
            /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Year"/>.
            /// This value normally defaults to 1825 days (5 years).
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            public static double RangeYearYear = 1825;  // 5 years
                                                        /// <summary>
                                                        /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                        /// This values applies only to Date-Time type axes.
                                                        /// If the total span of data exceeds this number (in days), then the auto-range
                                                        /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Year"/>
                                                        /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Month"/>.
                                                        /// This value normally defaults to 730 days (2 years).
                                                        /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                        /// </summary>
            public static double RangeYearMonth = 730;  // 2 years
                                                        /// <summary>
                                                        /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                        /// This values applies only to Date-Time type axes.
                                                        /// If the total span of data exceeds this number (in days), then the auto-range
                                                        /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Month"/>
                                                        /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Month"/>.
                                                        /// This value normally defaults to 300 days (10 months).
                                                        /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                        /// </summary>
            public static double RangeMonthMonth = 300;  // 10 months
                                                         /// <summary>
                                                         /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                         /// This values applies only to Date-Time type axes.
                                                         /// If the total span of data exceeds this number (in days), then the auto-range
                                                         /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Day"/>
                                                         /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Day"/>.
                                                         /// This value normally defaults to 10 days.
                                                         /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                         /// </summary>
            public static double RangeDayDay = 10;  // 10 days
                                                    /// <summary>
                                                    /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                    /// This values applies only to Date-Time type axes.
                                                    /// If the total span of data exceeds this number (in days), then the auto-range
                                                    /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Day"/>
                                                    /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Hour"/>.
                                                    /// This value normally defaults to 3 days.
                                                    /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                    /// </summary>
            public static double RangeDayHour = 3;  // 3 days
                                                    /// <summary>
                                                    /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                    /// This values applies only to Date-Time type axes.
                                                    /// If the total span of data exceeds this number (in days), then the auto-range
                                                    /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Hour"/>
                                                    /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Hour"/>.
                                                    /// This value normally defaults to 0.4167 days (10 hours).
                                                    /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                    /// </summary>
            public static double RangeHourHour = 0.4167;  // 10 hours
                                                          /// <summary>
                                                          /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                          /// This values applies only to Date-Time type axes.
                                                          /// If the total span of data exceeds this number (in days), then the auto-range
                                                          /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Hour"/>
                                                          /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Minute"/>.
                                                          /// This value normally defaults to 0.125 days (3 hours).
                                                          /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                          /// </summary>
            public static double RangeHourMinute = 0.125;  // 3 hours
                                                           /// <summary>
                                                           /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                           /// This values applies only to Date-Time type axes.
                                                           /// If the total span of data exceeds this number (in days), then the auto-range
                                                           /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Minute"/>
                                                           /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Minute"/>.
                                                           /// This value normally defaults to 6.94e-3 days (10 minutes).
                                                           /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                           /// </summary>
            public static double RangeMinuteMinute = 6.94e-3;  // 10 Minutes
                                                               /// <summary>
                                                               /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                               /// This values applies only to Date-Time type axes.
                                                               /// If the total span of data exceeds this number (in days), then the auto-range
                                                               /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Minute"/>
                                                               /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Second"/>.
                                                               /// This value normally defaults to 2.083e-3 days (3 minutes).
                                                               /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                               /// </summary>
            public static double RangeMinuteSecond = 2.083e-3;  // 3 Minutes
                                                                /// <summary>
                                                                /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
                                                                /// This values applies only to Date-Time type axes.
                                                                /// If the total span of data exceeds this number (in days), then the auto-range
                                                                /// code will select <see cref="MajorUnit"/> = <see cref="DateUnit.Second"/>
                                                                /// and <see cref="MinorUnit"/> = <see cref="DateUnit.Second"/>.
                                                                /// This value normally defaults to 3.472e-5 days (3 seconds).
                                                                /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
                                                                /// </summary>
            public static double RangeSecondSecond = 3.472e-5;  // 3 Seconds

            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Year"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Year"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatYearYear = "yyyy";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Year"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Month"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatYearMonth = "MMM-yyyy";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Month"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Month"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatMonthMonth = "MMM-yyyy";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Day"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Day"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatDayDay = "d-MMM";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Day"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Hour"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatDayHour = "d-MMM HH:mm";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Hour"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Hour"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatHourHour = "HH:mm";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Hour"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Minute"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatHourMinute = "HH:mm";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Minute"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Minute"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatMinuteMinute = "HH:mm";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Minute"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Second"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatMinuteSecond = "mm:ss";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Second"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Second"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatSecondSecond = "mm:ss";
            /// <summary>
            /// A default setting for the <see cref="AxisType.Date"/> auto-ranging code.
            /// This values applies only to Date-Time type axes.
            /// This is the format used for the scale values when auto-ranging code
            /// selects a <see cref="Format"/> of <see cref="DateUnit.Millisecond"/>
            /// for <see cref="MajorUnit"/> and <see cref="DateUnit.Millisecond"/> for 
            /// for <see cref="MinorUnit"/>.
            /// This value is used by the <see cref="DateScale.GetDateStepSize"/> method.
            /// </summary>
            /// <seealso cref="System.Globalization.DateTimeFormatInfo"/>
            public static string FormatMillisecond = "ss.fff";

            /*  Prior format assignments using original XDate.ToString()
					this.scaleFormat = "&yyyy";
					this.scaleFormat = "&mmm-&yy";
					this.scaleFormat = "&mmm-&yy";
					scaleFormat = "&d-&mmm";
					this.scaleFormat = "&d-&mmm &hh:&nn";
					scaleFormat = "&hh:&nn";
					scaleFormat = "&hh:&nn";
					scaleFormat = "&hh:&nn";
					scaleFormat = "&nn:&ss";
					scaleFormat = "&nn:&ss";
			*/
            /// <summary> The default alignment of the <see cref="Axis"/> tick labels.
            /// This value controls whether the inside, center, or outside edges of the text labels are aligned.
            /// </summary>
            /// <seealso cref="AlignP"/>
            public static AlignP Align = AlignP.Center;
            /// <summary> The default alignment of the <see cref="Axis"/> tick labels.
            /// This value controls whether the left, center, or right edges of the text labels are aligned.
            /// </summary>
            /// <seealso cref="AlignH"/>
            public static AlignH AlignH = AlignH.Center;
            /// <summary>
            /// The default font family for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.Family"/> property).
            /// </summary>
            public static string FontFamily = "Times New Roman";
            /// <summary>
            /// The default font size for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.Size"/> property).  Units are
            /// in points (1/72 inch).
            /// </summary>
            public static float FontSize = 20;
            /// <summary>
            /// The default font color for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.FontColor"/> property).
            /// </summary>
            public static Color FontColor = Color.Black;
            /// <summary>
            /// The default font bold mode for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.IsBold"/> property). true
            /// for a bold typeface, false otherwise.
            /// </summary>
            public static bool FontBold = false;
            /// <summary>
            /// The default font italic mode for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.IsItalic"/> property). true
            /// for an italic typeface, false otherwise.
            /// </summary>
            public static bool FontItalic = false;
            /// <summary>
            /// The default font underline mode for the <see cref="Axis"/> scale values
            /// font specification <see cref="FontSpec"/>
            /// (<see cref="LF.Figure.FontSpec.IsUnderline"/> property). true
            /// for an underlined typeface, false otherwise.
            /// </summary>
            public static bool FontUnderline = false;
            /// <summary>
            /// The default color for filling in the scale text background
            /// (see <see cref="LF.Figure.Fill.Color"/> property).
            /// </summary>
            public static Color FillColor = Color.White;
  

            /// <summary>
            /// The default value for <see cref="IsVisible"/>, which determines
            /// whether or not the scale values are displayed.
            /// </summary>
            public static bool IsVisible = true;
            /// <summary>
            /// The default value for <see cref="IsLabelsInside"/>, which determines
            /// whether or not the scale labels and title for the <see cref="Axis"/> will appear
            /// on the opposite side of the <see cref="Axis"/> that it normally appears.
            /// </summary>
            public static bool IsLabelsInside = false;
            /// <summary>
            /// Determines the size of the band at the beginning and end of the axis that will have labels
            /// omitted if the axis is shifted due to a non-default location using the <see cref="Axis.Cross"/>
            /// property.
            /// </summary>
            /// <remarks>
            /// This parameter applies only when <see cref="Axis.CrossAuto"/> is false.  It is scaled according
            /// to the size of the graph based on <see cref="PaneBase.BaseDimension"/>.  When a non-default
            /// axis location is selected, the first and last labels on that axis will overlap the opposing
            /// axis frame.  This parameter allows those labels to be omitted to avoid the overlap.  Set this
            /// parameter to zero to turn off the effect.
            /// </remarks>
            public static float EdgeTolerance = 6;

            /// <summary>
            /// The default setting for the gap between the outside tics (or the axis edge
            /// if there are no outside tics) and the scale labels, expressed as a fraction of
            /// the major tick size.
            /// </summary>
            public static float LabelGap = 0.3f;
        }

        #endregion

    }

    public enum AxisType
    {
        /// <summary> An ordinary, cartesian axis </summary>
        Linear,
        /// <summary> A base 10 log axis </summary>
        Log,
        /// <summary> A cartesian axis with calendar dates or times </summary>
        Date,
        /// <summary> An ordinal axis with user-defined text labels.  An ordinal axis means that
        /// all data points are evenly spaced at integral values, and the actual coordinate values
        /// for points corresponding to that axis are ignored.  That is, if the X axis is an
        /// ordinal type, then all X values associated with the curves are ignored.</summary>
        /// <seealso cref="AxisType.Ordinal"/>
        /// <seealso cref="Scale.IsText"/>
        /// <seealso cref="LF.Figure.Scale.Default.MaxTextLabels"/>
        Text,
        /// <summary> An ordinal axis with regular numeric labels.  An ordinal axis means that
        /// all data points are evenly spaced at integral values, and the actual coordinate values
        /// for points corresponding to that axis are ignored.  That is, if the X axis is an
        /// ordinal type, then all X values associated with the curves are ignored. </summary>
        /// <seealso cref="AxisType.Text"/>
        /// <seealso cref="Scale.IsOrdinal"/>
        Ordinal,
        /// <summary> An ordinal axis that will have labels formatted with ordinal values corresponding
        /// to the number of values in each <see cref="ChartItem" />.
        /// </summary>
        /// <remarks>
        /// The <see cref="ChartItem" /> data points will be evenly-spaced at ordinal locations, and the
        /// actual data values are ignored. </remarks>
        /// <seealso cref="AxisType.Text"/>
        /// <seealso cref="Scale.IsOrdinal"/>
        DateAsOrdinal,
        /// <summary> An ordinal axis that will have labels formatted with values from the actual data
        /// values of the first <see cref="ChartItem" /> in the <see cref="ItemList" />.
        /// </summary>
        /// <remarks>
        /// Although the tics are labeled with real data values, the actual points will be
        /// evenly-spaced in spite of the data values.  For example, if the X values of the first curve
        /// are 1, 5, and 100, then the tick labels will show 1, 5, and 100, but they will be equal
        /// distance from each other. </remarks>
        /// <seealso cref="AxisType.Text"/>
        /// <seealso cref="Scale.IsOrdinal"/>
        LinearAsOrdinal,
        /// <summary> An exponential axis </summary>
        Exponent
    }
}
