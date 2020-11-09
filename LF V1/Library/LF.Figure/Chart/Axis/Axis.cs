/*──────────────────────────────────────────────────────────────
 * FileName     : Axis
 * Created      : 2020-10-16 14:44:05
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
    /// 轴：抽象类
    /// </summary>
    public abstract class Axis : ICloneable
    {
        #region Fields
        /* 1. 刻度 */
        internal Scale _scale;
        internal MinorTick _minorTick;
        internal MajorTick _majorTick;
        internal MajorGrid _majorGrid;
        internal MinorGrid _minorGrid;

        internal double _cross;
        internal bool _crossAuto;

        /* 2. Type */
        protected bool _isVisible;
        protected bool _isAxisSegmentVisible;
        protected AxisLabel _title;
        public object Tag;
        private float _axisGap;
        private float _minSpace;
        private Color _color;

        internal float _tmpSpace;   // 非properties

        #region Delegate and Event
        public delegate string ScaleFormatHandler(ChartFigure graph, Axis axis, double val, int index);
        public event ScaleFormatHandler ScaleFormatEvent;

        public delegate string ScaleTitleEventHandler(Axis axis);
        public event ScaleTitleEventHandler ScaleTitleEvent;

        #endregion

        #endregion

        #region Properties

        #region Scale Properties

        /// <summary>
        /// 比例刻度
        /// </summary>
        public Scale Scale
        {
            get { return _scale; }
        }

        /// <summary>
        /// 原点
        /// </summary>
        public double Cross
        {
            get { return _cross; }
            set { _cross = value; _crossAuto = false; }
        }

        /// <summary>
        /// 原点自动
        /// </summary>
        public bool CrossAuto
        {
            get { return _crossAuto; }
            set { _crossAuto = value; }
        }

        /// <summary>
        /// 坐标轴最小空间
        /// </summary>
        public float MinSpace
        {
            get { return _minSpace; }
            set { _minSpace = value; }
        }

        #endregion

        #region Tick Properties

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 主刻度
        /// </summary>
        public MajorTick MajorTick
        {
            get { return _majorTick; }
        }

        /// <summary>
        /// 副刻度
        /// </summary>
        public MinorTick MinorTick
        {
            get { return _minorTick; }
        }

        #endregion

        #region Grid Properties

        /// <summary>
        /// 主网格
        /// </summary>
        public MajorGrid MajorGrid
        {
            get { return _majorGrid; }
        }

        /// <summary>
        /// 副网格
        /// </summary>
        public MinorGrid MinorGrid
        {
            get { return _minorGrid; }
        }

        #endregion

        #region Type Properties

        /// <summary>
        /// 显示
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        /// <summary>
        /// 轴线段是否显示
        /// </summary>
        public bool IsAxisSegmentVisible
        {
            get { return _isAxisSegmentVisible; }
            set { _isAxisSegmentVisible = value; }
        }

        /// <summary>
        /// 坐标轴类型
        /// </summary>
        public AxisType Type
        {
            get { return _scale.Type; }
            set { _scale = Scale.MakeNewScale(_scale, value); }
        }

        #endregion

        #region Label Properties

        /// <summary>
        /// 轴标签
        /// </summary>
        public AxisLabel Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 轴标签间距
        /// </summary>
        public float AxisGap
        {
            get { return _axisGap; }
            set { _axisGap = value; }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Axis()
        {
            _scale = new LinearScale(this);

            _cross = 0.0;

            _crossAuto = true;

            _majorTick = new MajorTick();
            _minorTick = new MinorTick();

            _majorGrid = new MajorGrid();
            _minorGrid = new MinorGrid();

            _axisGap = Default.AxisGap;

            _minSpace = Default.MinSpace;
            _isVisible = true;

            _isAxisSegmentVisible = Default.IsAxisSegmentVisible;

            _title = new AxisLabel("", Default.TitleFontFamily, Default.TitleFontSize,
                    Default.TitleFontColor, Default.TitleFontBold,
                    Default.TitleFontUnderline, Default.TitleFontItalic);
            _title.FontSpec.FillBrush = new SolidBrush(Default.TitleFillColor);

            _title.FontSpec.Border.IsVisible = false;


            _color = Default.Color;

        }

        /// <summary>
        /// 构造函数：设置标题
        /// </summary>
        /// <param name="title"></param>
        public Axis(string title)
            : this()
        {
            _title._text = title;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public Axis(Axis rhs)
        {
            _scale = rhs._scale.Clone(this);

            _cross = rhs._cross;

            _crossAuto = rhs._crossAuto;

            _majorTick = rhs._majorTick.Clone();
            _minorTick = rhs._minorTick.Clone();

            _majorGrid = rhs._majorGrid.Clone();
            _minorGrid = rhs._minorGrid.Clone();

            _isVisible = rhs.IsVisible;

            _isAxisSegmentVisible = rhs._isAxisSegmentVisible;

            _title = (AxisLabel)rhs.Title.Clone();

            _axisGap = rhs._axisGap;

            _minSpace = rhs.MinSpace;

            _color = rhs.Color;
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            throw new NotImplementedException("Can't clone an abstract base type -- child types must implement ICloneable");
        }

        #endregion

        #region Methods

        #region Basic Methods 基础

        /// <summary>
        /// 是否当前主轴
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal abstract bool IsPrimary(ChartFigure graph);

        /// <summary>
        /// 设置变换矩阵，正对Y轴、X2轴、Y2轴
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        public abstract void SetTransformMatrix(Graphics g, ChartFigure graph, float scaleFactor);

        /// <summary>
        /// 交叉点位置
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal abstract float GetCrossShift(ChartFigure graph);

        /// <summary>
        /// 获取相交轴
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public abstract Axis GetCrossAxis(ChartFigure graph);

        /// <summary>
        /// 有效的相交值
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal double EffectiveCrossValue(ChartFigure graph)
        {
            Axis crossAxis = GetCrossAxis(graph);

            double min = crossAxis.Scale.Min;
            double max = crossAxis.Scale.Max;

            if (_crossAuto)
            {
                return min;
            }
            else
            {
                if (_cross < min)
                    return min;
                else if (_cross > max)
                    return max;
                else
                    return _cross;
            }

        }

        #endregion

        #region Rendering 渲染方法

        /// <summary>
        /// 将与该<see cref="Axis"/>关联的所有绘制都绘制到指定的<see cref="Graphics"/> 设备。
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="shiftPos"></param>
        public void Draw(Graphics g, ChartFigure graph, float scaleFactor, float shiftPos)
        {


            /* 1. 配置比例刻度参数 */
            _scale.SetupScaleData(graph, this);

            /* 2. 若显示，开始绘制 */
            if (_isVisible)
            {
                //var smode = g.SmoothingMode;
                //g.SmoothingMode = SmoothingMode.None;

                /* 3.1 记录画布原始信息 */
                Matrix saveMatrix = g.Transform;

                /* 3.2 设置转换矩阵，主要针对Y轴 */
                SetTransformMatrix(g, graph, scaleFactor);

                /* 3.3 计算偏移量 */
                shiftPos = GetTotalShift(graph, scaleFactor, shiftPos);

                /* 3.4 绘制坐标轴 */
                _scale.Draw(g, graph, scaleFactor, shiftPos);

                /* 3.5 恢复画布原始设置 */
                g.Transform = saveMatrix;

                //g.SmoothingMode = smode;
            }
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="shiftPos"></param>
        /// <param name="scaleFactor"></param>
        public void DrawTitle(Graphics g, ChartFigure graph, float shiftPos, float scaleFactor)
        {
            string str = MakeTitle();

            if (_isVisible && _title._isVisible && !string.IsNullOrEmpty(str))
            {
                bool hasTic = (_scale.IsLabelsInside ?
                        (this.MajorTick.IsInside || this.MajorTick.IsCrossInside ||
                                this.MinorTick.IsInside || this.MinorTick.IsCrossInside) :
                        (this.MajorTick.IsOutside || this.MajorTick.IsCrossOutside || this.MinorTick.IsOutside || this.MinorTick.IsCrossOutside));

                float x = (_scale._maxPix - _scale._minPix) / 2;

                float scaledTic = MajorTick.GetScaledTickSize(scaleFactor);
                float scaledLabelGap = _scale._fontSpec.GetHeight(scaleFactor) * _scale._labelGap;
                float scaledTitleGap = _title.GetScaledGap(scaleFactor);

                float gap = scaledTic * (hasTic ? 1.0f : 0.0f) +
                            this.Title.FontSpec.BoundingBox(g, str, scaleFactor).Height / 2.0F;
                float y = (_scale.IsVisible ? _scale.GetScaleMaxSpace(g, graph, scaleFactor, true).Height
                            + scaledLabelGap : 0);

                if (_scale.IsLabelsInside)
                    y = shiftPos - y - gap;
                else
                    y = shiftPos + y + gap;

                if (!_crossAuto && !_title._isTitleAtCross)
                    y = Math.Max(y, gap);

                AlignV alignV = AlignV.Center;

                y += scaledTitleGap;

                this.Title.FontSpec.Draw(g, str, x, y,
                            AlignH.Center, alignV, scaleFactor);
            }
        }

        /// <summary>
        /// 绘制副刻度
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="baseVal"></param>
        /// <param name="shift"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="topPix"></param>
        public void DrawMinorTicks(Graphics g, ChartFigure graph, double baseVal, float shift, float scaleFactor, float topPix)
        {
            if ((this.MinorTick.IsOutside || this.MinorTick.IsOpposite || this.MinorTick.IsInside ||
                    this.MinorTick.IsCrossOutside || this.MinorTick.IsCrossInside || _minorGrid.IsVisible)
                    && _isVisible)
            {
                double tMajor = _scale.MajorStep * _scale.MajorUnitMultiplier,
                            tMinor = _scale.MinorStep * _scale.MinorUnitMultiplier;

                if (_scale.IsLog || tMinor < tMajor)
                {
                    float minorScaledTic = this.MinorTick.GetScaledTickSize(scaleFactor);

                    double first = _scale._minLinTemp,
                                last = _scale._maxLinTemp;

                    double dVal = first;
                    float pixVal;

                    int iTic = _scale.GetMinorStart(baseVal);
                    int MajorTick = 0;
                    double majorVal = _scale.GetMajorTickValue(baseVal, MajorTick);

                    using (Pen pen = new Pen(_minorTick.Color,
                                        graph.ScaledLineWidth(MinorTick.LineWidth, scaleFactor)))
                    using (Pen minorGridPen = _minorGrid.GetPen(graph, scaleFactor))
                    {

                        // Draw the minor tick marks
                        while (dVal < last && iTic < 5000)
                        {
                            // Getulate the scale value for the current tick
                            dVal = _scale.GetMinorTickValue(baseVal, iTic);
                            // Maintain a value for the current major tick
                            if (dVal > majorVal)
                                majorVal = _scale.GetMajorTickValue(baseVal, ++MajorTick);

                            // Make sure that the current value does not match up with a major tick
                            if (((Math.Abs(dVal) < 1e-20 && Math.Abs(dVal - majorVal) > 1e-20) ||
                                (Math.Abs(dVal) > 1e-20 && Math.Abs((dVal - majorVal) / dVal) > 1e-10)) &&
                                (dVal >= first && dVal <= last))
                            {
                                pixVal = _scale.LocalTransform(dVal);

                                _minorGrid.Draw(g, minorGridPen, pixVal, topPix);

                                _minorTick.Draw(g, graph, pen, pixVal, topPix, shift, minorScaledTic);
                            }

                            iTic++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="shiftPos"></param>
        internal void DrawGridAndMinorTicks(Graphics g, ChartFigure graph, float scaleFactor, float shiftPos)
        {
            /* 若显示，开始绘制 */
            if (_isVisible)
            {
                /* 1. 记录画布原始信息 */
                Matrix saveMatrix = g.Transform;

                /* 2. 设置转换矩阵，主要针对Y轴 */
                SetTransformMatrix(g, graph, scaleFactor);

                /* 3. 计算基础值 */
                double baseVal = _scale.GetBaseTick();

                /* 4. 计算像素坐标 */
                float topPix, rightPix;
                _scale.GetTopRightPix(graph, out topPix, out rightPix);

                /* 5. 计算偏移量 */
                shiftPos = GetTotalShift(graph, scaleFactor, shiftPos);

                /* 6. 绘制网格 */
                _scale.DrawGrid(g, graph, baseVal, topPix, scaleFactor);

                /* 7. 绘制副刻度线 */
                DrawMinorTicks(g, graph, baseVal, shiftPos, scaleFactor, topPix);

                /* 8. 恢复画布原始设置 */
                g.Transform = saveMatrix;
            }
        }
        #endregion

        #region Drawing Methods 绘图计算方法

        /// <summary>
        /// 计算坐标轴空间
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="fixedSpace"></param>
        /// <returns></returns>
        public float GetSpace(Graphics g, ChartFigure graph, float scaleFactor, out float fixedSpace)
        {
            //fixedSpace = 0;

            //Typical character height for the scale font
            float charHeight = _scale._fontSpec.GetHeight(scaleFactor);
            // Scaled size (pixels) of a tick
            float ticSize = _majorTick.GetScaledTickSize(scaleFactor);
            // Scaled size (pixels) of the axis gap
            float axisGap = _axisGap * scaleFactor;
            float scaledLabelGap = _scale._labelGap * charHeight;
            float scaledTitleGap = _title.GetScaledGap(scaleFactor);

            fixedSpace = 0;

            // The actual space needed for this axis (ignoring the setting of Axis.Cross)
            _tmpSpace = 0;

            // Account for the Axis
            if (_isVisible)
            {
                bool hasTic = this.MajorTick.IsOutside || this.MajorTick.IsCrossOutside ||
                                    this.MinorTick.IsOutside || this.MinorTick.IsCrossOutside;

                // account for the tick space.  Leave the tick space for any type of outside tick (Outside Tic Space)
                if (hasTic)
                    _tmpSpace += ticSize;

                // if this is not the primary axis
                if (!IsPrimary(graph))
                {
                    // always leave an extra tick space for the space between the multi-axes (Axis Gap)
                    _tmpSpace += axisGap;

                    // if it has inside tics, leave another tick space (Inside Tic Space)
                    if (this.MajorTick.IsInside || this.MajorTick.IsCrossInside ||
                            this.MinorTick.IsInside || this.MinorTick.IsCrossInside)
                        _tmpSpace += ticSize;
                }

                // tick takes up 1x tick
                // space between tick and scale label is 0.5 tick
                // scale label is GetScaleMaxSpace()
                // space between scale label and axis label is 0.5 tick

                // account for the tick labels + 'LabelGap' tick gap between the tick and the label
                _tmpSpace += _scale.GetScaleMaxSpace(g, graph, scaleFactor, true).Height +
                        scaledLabelGap;

                string str = MakeTitle();

                // Only add space for the title if there is one
                // Axis Title gets actual height
                // if ( str.Length > 0 && _title._isVisible )
                if (!string.IsNullOrEmpty(str) && _title._isVisible)
                {
                    //tmpSpace += this.TitleFontSpec.BoundingBox( g, str, scaleFactor ).Height;
                    fixedSpace = this.Title.FontSpec.BoundingBox(g, str, scaleFactor).Height +
                            scaledTitleGap;
                    _tmpSpace += fixedSpace;

                    fixedSpace += scaledTitleGap;
                }

                if (hasTic)
                    fixedSpace += ticSize;
            }

            // for the Y axes, make sure that enough space is left to fit the first
            // and last X axis scale label
            if (this.IsPrimary(graph) && ((
                    (this is YAxis && (
                        (!graph.XAxis._scale.IsSkipFirstLabel && !graph.XAxis._scale.IsReverse) ||
                        (!graph.XAxis._scale.IsSkipLastLabel && graph.XAxis._scale.IsReverse))) ||
                    (this is Y2Axis && (
                        (!graph.XAxis._scale.IsSkipFirstLabel && graph.XAxis._scale.IsReverse) ||
                        (!graph.XAxis._scale.IsSkipLastLabel && !graph.XAxis._scale.IsReverse)))) &&
                    graph.XAxis.IsVisible && graph.XAxis._scale.IsVisible))
            {
                // half the width of the widest item, plus a gap of 1/2 the charheight
                float tmp = graph.XAxis._scale.GetScaleMaxSpace(g, graph, scaleFactor, true).Width / 2.0F;
                //+ charHeight / 2.0F;
                //if ( tmp > tmpSpace )
                //	tmpSpace = tmp;

                fixedSpace = Math.Max(tmp, fixedSpace);
            }

            // Verify that the minSpace property was satisfied
            _tmpSpace = Math.Max(_tmpSpace, _minSpace * (float)scaleFactor);

            fixedSpace = Math.Max(fixedSpace, _minSpace * (float)scaleFactor);

            return _tmpSpace;
        }

        /// <summary>
        /// 计算坐标轴偏移量
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="shiftPos"></param>
        /// <returns></returns>
        private float GetTotalShift(ChartFigure graph, float scaleFactor, float shiftPos)
        {
            if (!IsPrimary(graph))
            {
                // if ( GetCrossFraction( graph ) != 0.0 )
                if (IsCrossShifted(graph))
                {
                    shiftPos = 0;
                }
                else
                {
                    // Scaled size (pixels) of a tick
                    float ticSize = _majorTick.GetScaledTickSize(scaleFactor);

                    // if the scalelabels are on the inside, shift everything so the axis is drawn,
                    // for example, to the left side of the available space for a YAxis type
                    if (_scale.IsLabelsInside)
                    {
                        shiftPos += _tmpSpace;

                        // shift the axis to leave room for the outside tics
                        if (_majorTick.IsOutside || _majorTick.IsCrossOutside ||
                                        _minorTick.IsOutside || _minorTick.IsCrossOutside)
                            shiftPos -= ticSize;
                    }
                    else
                    {
                        // if it's not the primary axis, add a tick space for the spacing between axes
                        shiftPos += _axisGap * scaleFactor;

                        // if it has inside tics, leave another tick space
                        if (_majorTick.IsInside || _majorTick.IsCrossInside ||
                                _minorTick.IsInside || _minorTick.IsCrossInside)
                            shiftPos += ticSize;
                    }
                }
            }

            // shift is the position of the actual axis line itself
            // everything else is based on that position.
            float crossShift = GetCrossShift(graph);
            shiftPos += crossShift;

            return shiftPos;
        }

        /// <summary>
        /// 生成轴标题
        /// </summary>
        /// <returns></returns>
        private string MakeTitle()
        {
            if (_title._text == null)
                _title._text = "";

            // Revision: JCarpenter 10/06
            // Allow customization of the modified title when the scale is very large
            // The event handler can edit the full label.  If the handler returns
            // null, then the title will be the default.
            if (ScaleTitleEvent != null)
            {
                string label = ScaleTitleEvent(this);
                if (label != null)
                    return label;
            }

            // If the Mag is non-zero and IsOmitMag == false, and IsLog == false,
            // then add the mag indicator to the title.
            if (_scale.Mag != 0 && !_title._isOmitMag && !_scale.IsLog)
                return _title._text + String.Format(" (10^{0})", _scale.Mag);
            else
                return _title._text;

        }

        /// <summary>
        /// 轴标题标签
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="index"></param>
        /// <param name="dVal"></param>
        /// <returns></returns>
        internal string MakeLabelEventWorks(ChartFigure graph, int index, double dVal)
        {
            // if there is a valid ScaleFormatEvent, then try to use it to create the label
            // the label will be non-null if it's to be used
            if (this.ScaleFormatEvent != null)
            {
                string label;

                label = this.ScaleFormatEvent(graph, this, dVal, index);
                if (label != null)
                    return label;
            }

            // second try.  If there's no custom ScaleFormatEvent, then just call
            // _scale.MakeLabel according to the type of scale
            if (this.Scale != null)
                return _scale.MakeLabel(graph, index, dVal);
            else
                return "?";
        }

        /// <summary>
        /// 此方法将为当前的所需空间乘以一个分数bufferFraction，为该<see cref="Axis"/>设置 <see cref="MinSpace"/> 属性。
        /// </summary>
        public void SetMinSpaceBuffer(Graphics g, ChartFigure graph, float bufferFraction, bool isGrowOnly)
        {

            /* 1. 保存最小空间原始值 */
            float oldSpace = this.MinSpace;

            /* 将minspace设置为零，因为我们不希望它影响GetSpace（）结果 */
            this.MinSpace = 0;
            // Getulate the space required for the current graph assuming scalefactor = 1.0
            // and apply the bufferFraction
            float fixedSpace;
            float space = this.GetSpace(g, graph, 1.0F, out fixedSpace) * bufferFraction;
            // isGrowOnly indicates the minSpace can grow but not shrink
            if (isGrowOnly)
                space = Math.Max(oldSpace, space);
            // Set the minSpace
            this.MinSpace = space;
        }

        #endregion

        #region Scale Methods 刻度方法

        /// <summary>
        /// 恢复到自动模式的刻度，并重新计算<see cref="Axis"/> 刻度范围。
        /// </summary>
        public void ResetAutoScale(ChartFigure graph, Graphics g)
        {
            _scale.MinAuto = true;
            _scale.MaxAuto = true;
            _scale.MajorStepAuto = true;
            _scale.MinAuto = true;
            _crossAuto = true;
            _scale.MagAuto = true;
            _scale.FormatAuto = true;
            graph.AxisChange(g);
        }

        /// <summary>
        /// 交叉点是否偏移
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal bool IsCrossShifted(ChartFigure graph)
        {
            if (_crossAuto)
                return false;
            else
            {
                Axis crossAxis = GetCrossAxis(graph);
                if (((this is XAxis || this is YAxis) && !crossAxis._scale.IsReverse) ||
                    ((this is X2Axis || this is Y2Axis) && crossAxis._scale.IsReverse))
                {
                    if (_cross <= crossAxis._scale.Min)
                        return false;
                }
                else
                {
                    if (_cross >= crossAxis._scale.Max)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 固定零线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        internal void FixZeroLine(Graphics g, ChartFigure graph, float scaleFactor, float left, float right)
        {
            // restore the zero line if needed (since the fill tends to cover it up)
            if (_isVisible && _majorGrid.IsZeroLine &&
                    _scale.Min < 0.0 && _scale.Max > 0.0)
            {
                float zeroPix = _scale.Transform(0.0);

                using (Pen zeroPen = new Pen(_color,
                        graph.ScaledLineWidth(_majorGrid.LineWidth, scaleFactor)))
                {
                    g.DrawLine(zeroPen, left, zeroPix, right, zeroPix);
                    //zeroPen.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取交叉系数
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        internal float GetCrossFraction(ChartFigure graph)
        {
            // if this axis is not shifted due to the Cross value
            if (!this.IsCrossShifted(graph))
            {
                // if it's the primary axis and the scale labels are on the inside, then we
                // don't need to save any room for the axis labels (they will be inside the chart rect)
                if (IsPrimary(graph) && _scale.IsLabelsInside)
                    return 1.0f;
                // otherwise, it's a secondary (outboard) axis and we always save room for the axis and labels.
                else
                    return 0.0f;
            }

            double effCross = EffectiveCrossValue(graph);
            Axis crossAxis = GetCrossAxis(graph);

            // Use Linearize here instead of _minLinTemp because this method is called
            // as part of GetRect() before scale is fully setup
            //			double max = crossAxis._scale.MaxLinTemp;
            //			double min = crossAxis._scale.MinLinTemp;
            double max = crossAxis._scale.Linearize(crossAxis._scale.Min);
            double min = crossAxis._scale.Linearize(crossAxis._scale.Max);
            float frac;

            if (((this is XAxis || this is YAxis) && _scale.IsLabelsInside == crossAxis._scale.IsReverse) ||
                 ((this is X2Axis || this is Y2Axis) && _scale.IsLabelsInside != crossAxis._scale.IsReverse))
                frac = (float)((effCross - min) / (max - min));
            else
                frac = (float)((max - effCross) / (max - min));

            if (frac < 0.0f)
                frac = 0.0f;
            if (frac > 1.0f)
                frac = 1.0f;

            return frac;
        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults

        /// <summary>
        /// 默认值
        /// </summary>
        public struct Default
        {
            public static float AxisGap = 5;
            public static float TitleGap = 0.0f;
            public static string TitleFontFamily = "Times new Roman";
            public static float TitleFontSize = 20;
            public static Color TitleFontColor = Color.Black;
            public static bool TitleFontBold = false;
            public static bool TitleFontItalic = false;
            public static bool TitleFontUnderline = false;
            public static Color TitleFillColor = Color.White;
            public static Brush TitleFillBrush = null;
            public static Color BorderColor = Color.Black;
            public static bool IsAxisSegmentVisible = true;
            public static AxisType Type = AxisType.Linear;
            public static Color Color = Color.Black;
            public static float MinSpace = 0f;
        }

        #endregion
    }
}
