/*──────────────────────────────────────────────────────────────
 * FileName     : ChartFigure
 * Created      : 2020-10-16 11:58:02
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
using System.Collections.ObjectModel;

namespace LF.Figure
{
    public partial class ChartFigure:Figure
    {
        #region Fields
        private Chart _chart;
        private XAxis _xAxis;
        private X2Axis _x2Axis;
        private YAxisList _yAxisList;
        private Y2AxisList _y2AxisList;

        private ZoomStateStack _zoomStack;

        private bool _isIgnoreInitial;
        private bool _isIgnoreMissing;
        private bool _isBoundedRanges;
        private bool _isAlignGrids;
        private bool _isAxisEqual;

        public delegate void AxisChangeEventHandler(ChartFigure graph);
        public event AxisChangeEventHandler AxisChangeEvent;

        public readonly ObservableCollection<Overlay> Overlays = new ObservableCollection<Overlay>();
        public readonly Overlay DefaultOverlay;

        #endregion

        #region Properties

        #region Class Instance Properties

        /// <summary>
        /// X轴
        /// </summary>
        public XAxis XAxis
        {
            get { return _xAxis; }
        }

        /// <summary>
        /// 副X轴
        /// </summary>
        public X2Axis X2Axis
        {
            get { return _x2Axis; }
        }

        /// <summary>
        /// Y轴
        /// </summary>
        public YAxis YAxis
        {
            get { return _yAxisList[0] as YAxis; }
        }

        /// <summary>
        /// 副Y轴
        /// </summary>
        public Y2Axis Y2Axis
        {
            get { return _y2AxisList[0] as Y2Axis; }
        }

        /// <summary>
        /// Y轴列表
        /// </summary>
        public YAxisList YAxisList
        {
            get { return _yAxisList; }
        }

        /// <summary>
        /// 副Y轴列表
        /// </summary>
        public Y2AxisList Y2AxisList
        {
            get { return _y2AxisList; }
        }

        /// <summary>
        /// 绘图区
        /// </summary>
        public Chart Chart
        {
            get { return _chart; }
        }

        #endregion

        #region General Properties

        public bool IsIgnoreInitial
        {
            get { return _isIgnoreInitial; }
            set { _isIgnoreInitial = value; }
        }

        public bool IsBoundedRanges
        {
            get { return _isBoundedRanges; }
            set { _isBoundedRanges = value; }
        }

        public bool IsAxisEqual
        {
            get { return _isAxisEqual; }
            set { _isAxisEqual = value; }
        }

        public bool IsIgnoreMissing
        {
            get { return _isIgnoreMissing; }
            set { _isIgnoreMissing = value; }
        }

        public bool IsAlignGrids
        {
            get { return _isAlignGrids; }
            set { _isAlignGrids = value; }
        }

        public bool IsZoomed
        {
            get { return !_zoomStack.IsEmpty; }
        }

        public ZoomStateStack ZoomStack
        {
            get { return _zoomStack; }
        }
        #endregion

        #endregion


        #region Constructors
        public ChartFigure(RectangleF area, string title, string xTitle, string yTitle)
                    : base(title, area)
        {
            _xAxis = new XAxis(xTitle);
            _x2Axis = new X2Axis("");

            _yAxisList = new YAxisList();
            _y2AxisList = new Y2AxisList();

            _yAxisList.Add(new YAxis(yTitle));
            _y2AxisList.Add(new Y2Axis(string.Empty));

            _zoomStack = new ZoomStateStack();

            _isIgnoreInitial = Default.IsIgnoreInitial;
            _isBoundedRanges = Default.IsBoundedRanges;
            _isAlignGrids = false;
            _isAxisEqual = Default.IsAxisEqual;

            _chart = new Chart();

            DefaultOverlay = new Overlay(this);
            Overlays.CollectionChanged += Overlays_CollectionChanged;
            Overlays.Add(DefaultOverlay);
        }

        public ChartFigure()
            : this(new RectangleF(0, 0, 500, 375), "", "", "")
        {
        }

        #endregion

        #region Methods

        #region Basic Methods

        public void AxisChange()
        {
            using (var img = new Bitmap((int)this._area.Width, (int)this._area.Height))
            using (Graphics g = Graphics.FromImage(img))
                AxisChange(g);
        }

        public void AxisChange(Graphics g)
        {

            float scaleFactor = this.GetScaleFactor();


            PickScale(g, scaleFactor);

            if (this.AxisChangeEvent != null)
                this.AxisChangeEvent(this);
        }

        public void ResetRange()
        {
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int i = 0;
            foreach (Overlay o in Overlays)
            {
                foreach (IItem item in o.Items)
                {
                    double xmin;
                    double xmax;
                    double ymin;
                    double ymax;

                    if (i == 0)
                    {

                        item.Datas.GetRange(out xmin, out xmax, out ymin, out ymax);

                        double y = ymax - ymin;

                        minX = xmin;
                        maxX = xmax;
                        minY = ymin - y / 10; ;
                        maxY = ymax + y / 10;
                    }
                    else
                    {

                        item.Datas.GetRange(out xmin, out xmax, out ymin, out ymax);

                        if (xmin < minX)
                            minX = xmin;
                        if (xmax > maxX)
                            maxX = xmax;
                        if (ymin < minY)
                            minY = ymin;
                        if (ymax > maxY)
                            maxY = ymax;
                    }

                }
                i++;
            }

            _xAxis.Scale.Max = maxX;
            _x2Axis.Scale.Max = maxX;
            _xAxis.Scale.Min = minX;
            _x2Axis.Scale.Min = minX;

            foreach (YAxis yAxis in _yAxisList)
            {
                yAxis.Scale.Max = maxY;
                yAxis.Scale.Min = minY;
            }
            foreach (Y2Axis y2Axis in _y2AxisList)
            {
                y2Axis.Scale.Max = maxY;
                y2Axis.Scale.Min = minY;
            }

            AxisChange();
        }

        private void PickScale(Graphics g, float scaleFactor)
        {
            int maxTicks = 0;

            _xAxis._scale.MajorStepAuto = true;
            _x2Axis._scale.MajorStepAuto = true;


            _xAxis._scale.PickScale(this, g, scaleFactor);
            _x2Axis._scale.PickScale(this, g, scaleFactor);

            foreach (Axis axis in _yAxisList)
            {
                axis._scale.MajorStepAuto = true;
                axis._scale.PickScale(this, g, scaleFactor);
                if (axis._scale.MaxAuto)
                {
                    int nTicks = axis._scale.GetNumTicks();
                    maxTicks = nTicks > maxTicks ? nTicks : maxTicks;
                }
            }
            foreach (Axis axis in _y2AxisList)
            {
                axis._scale.MajorStepAuto = true;
                axis._scale.PickScale(this, g, scaleFactor);
                if (axis._scale.MaxAuto)
                {
                    int nTicks = axis._scale.GetNumTicks();
                    maxTicks = nTicks > maxTicks ? nTicks : maxTicks;
                }
            }

            if (_isAlignGrids)
            {
                foreach (Axis axis in _yAxisList)
                    ForceNumTicks(axis, maxTicks);

                foreach (Axis axis in _y2AxisList)
                    ForceNumTicks(axis, maxTicks);
            }

        }

        private void ForceNumTicks(Axis axis, int numTicks)
        {
            if (axis._scale.MaxAuto)
            {
                int nTicks = axis._scale.GetNumTicks();
                if (nTicks < numTicks)
                    axis._scale._maxLinearized += axis._scale.MajorStep * (numTicks - nTicks);
            }
        }

        #endregion

        #region Rendering Methods
        public override void Draw(Graphics g)
        {
            base.Draw(g);

            float scaleFactor = this.GetScaleFactor();

            // 计算核心绘图区的区域
            _chart.Area = GetChartArea(g, scaleFactor);

            // 尺寸检测
            if (_chart.Area.Width < 1 || _chart.Area.Height < 1)
                return;


            bool showGraf = AxisRangesValid();

            if (_chart.Border.IsVisible)
            {
                Axis.Default.IsAxisSegmentVisible = false;
            }
            else
            {
                MinorTick.Default.IsOpposite = false;
                MajorTick.Default.IsOpposite = false;
            }

            // 设置坐标轴参数
            _xAxis.Scale.SetupScaleData(this, _xAxis);
            _x2Axis.Scale.SetupScaleData(this, _x2Axis);
            foreach (Axis axis in _yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in _y2AxisList)
                axis.Scale.SetupScaleData(this, axis);

            // 填充Chart区域
            g.FillRectangle(_chart.Fill, _chart.Area);

            if (showGraf)
            {
                // 绘制网格和副刻度
                DrawGridAndMinorTicks(g, scaleFactor);

                // 绘制坐标轴
                _xAxis.Draw(g, this, scaleFactor, 0.0f);
                _x2Axis.Draw(g, this, scaleFactor, 0.0f);
                float yPos = 0;
                foreach (Axis axis in _yAxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis._tmpSpace;
                }

                yPos = 0;
                foreach (Axis axis in _y2AxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis._tmpSpace;
                }

                // 绘制对象
                g.SetClip(_chart.Area);

                #region 绘制对象
                foreach (Overlay o in Overlays)
                {
                    o.Draw(g);
                }
                #endregion

                g.ResetClip();
            }

            _chart.Border.Draw(g, _chart.Area, scaleFactor);

        }

        /// <summary>
        /// 绘制网格和副刻度线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        internal void DrawGridAndMinorTicks(Graphics g, float scaleFactor)
        {
            _xAxis.DrawGridAndMinorTicks(g, this, scaleFactor, 0.0f);
            _x2Axis.DrawGridAndMinorTicks(g, this, scaleFactor, 0.0f);

            float shiftPos = 0.0f;
            foreach (YAxis yAxis in _yAxisList)
            {
                yAxis.DrawGridAndMinorTicks(g, this, scaleFactor, shiftPos);
                shiftPos += yAxis._tmpSpace;
            }

            shiftPos = 0.0f;
            foreach (Y2Axis y2Axis in _y2AxisList)
            {
                y2Axis.DrawGridAndMinorTicks(g, this, scaleFactor, shiftPos);
                shiftPos += y2Axis._tmpSpace;
            }
        }

        #endregion


        #region Chart Methods

        /// <summary>
        /// 计算Chart区域
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public RectangleF GetChartArea(Graphics g, float scaleFactor)
        {
            // chart rect starts out at the full pane rect less the margins
            //   and less space for the Pane title
            RectangleF clientRect = this.GetClientArea(g, scaleFactor);

            //float minSpaceX = 0;
            //float minSpaceY = 0;
            //float minSpaceY2 = 0;
            float totSpaceY = 0;
            //float spaceY2 = 0;

            // actual minimum axis space for the left side of the chart rect
            float minSpaceL = 0;
            // actual minimum axis space for the right side of the chart rect
            float minSpaceR = 0;
            // actual minimum axis space for the bottom side of the chart rect
            float minSpaceB = 0;
            // actual minimum axis space for the top side of the chart rect
            float minSpaceT = 0;

            _xAxis.GetSpace(g, this, scaleFactor, out minSpaceB);
            _x2Axis.GetSpace(g, this, scaleFactor, out minSpaceT);

            //minSpaceB = _xAxis.tmpMinSpace;

            foreach (Axis axis in _yAxisList)
            {
                float fixedSpace;
                float tmp = axis.GetSpace(g, this, scaleFactor, out fixedSpace);
                //if ( !axis.CrossAuto || axis.Cross < _xAxis.Min )
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;

                minSpaceL += fixedSpace;
            }
            foreach (Axis axis in _y2AxisList)
            {
                float fixedSpace;
                float tmp = axis.GetSpace(g, this, scaleFactor, out fixedSpace);
                //if ( !axis.CrossAuto || axis.Cross < _xAxis.Min )
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;

                minSpaceR += fixedSpace;
            }

            float spaceB = 0, spaceT = 0, spaceL = 0, spaceR = 0;

            SetSpace(_xAxis, clientRect.Height - _xAxis._tmpSpace, ref spaceB, ref spaceT);
            //			minSpaceT = Math.Max( minSpaceT, spaceT );
            SetSpace(_x2Axis, clientRect.Height - _x2Axis._tmpSpace, ref spaceT, ref spaceB);
            _xAxis._tmpSpace = spaceB;
            _x2Axis._tmpSpace = spaceT;

            float totSpaceL = 0;
            float totSpaceR = 0;

            foreach (Axis axis in _yAxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceL, ref spaceR);
                minSpaceR = Math.Max(minSpaceR, spaceR);
                totSpaceL += spaceL;
                axis._tmpSpace = spaceL;
            }
            foreach (Axis axis in _y2AxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceR, ref spaceL);
                minSpaceL = Math.Max(minSpaceL, spaceL);
                totSpaceR += spaceR;
                axis._tmpSpace = spaceR;
            }

            RectangleF tmpRect = clientRect;

            totSpaceL = Math.Max(totSpaceL, minSpaceL);
            totSpaceR = Math.Max(totSpaceR, minSpaceR);
            spaceB = Math.Max(spaceB, minSpaceB);
            spaceT = Math.Max(spaceT, minSpaceT);

            tmpRect.X += totSpaceL;
            tmpRect.Width -= totSpaceL + totSpaceR;
            tmpRect.Height -= spaceT + spaceB;
            tmpRect.Y += spaceT;

            //_legend.GetRect(g, this, scaleFactor, ref tmpRect);

            // 设置坐标轴比例一直
            if (_isAxisEqual)
            {
                double xLen = _xAxis.Scale.Max - _xAxis.Scale.Min;
                double yLen = YAxis.Scale.Max - YAxis.Scale.Min;
                double r = yLen / xLen;

                double htmp = tmpRect.Width * r;
                double wtmp = tmpRect.Height / r;

                if (tmpRect.Height > htmp)
                {
                    tmpRect.Y += (tmpRect.Height - (float)htmp) / 2;
                    tmpRect.Height = (float)htmp;
                }
                else
                {
                    tmpRect.X += (tmpRect.Width - (float)wtmp) / 2;
                    tmpRect.Width = (float)wtmp;
                }

            }

            return tmpRect;
        }

        private void SetSpace(Axis axis, float clientSize, ref float spaceNorm, ref float spaceAlt)
        {
            float crossFrac = axis.GetCrossFraction(this);
            float crossPix = crossFrac * (1 + crossFrac) * (1 + crossFrac * crossFrac) * clientSize;

            if (!axis.IsPrimary(this) && axis.IsCrossShifted(this))
                axis._tmpSpace = 0;

            if (axis._tmpSpace < crossPix)
                axis._tmpSpace = 0;
            else if (crossPix > 0)
                axis._tmpSpace -= crossPix;

            if (axis._scale.IsLabelsInside && (axis.IsPrimary(this) || (crossFrac != 0.0 && crossFrac != 1.0)))
                spaceAlt = axis._tmpSpace;
            else
                spaceNorm = axis._tmpSpace;
        }

        public void SetMinSpaceBuffer(Graphics g, float bufferFraction, bool isGrowOnly)
        {
            _xAxis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            _x2Axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in _yAxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in _y2AxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
        }

        private bool AxisRangesValid()
        {
            bool showGraf = _xAxis._scale.Min < _xAxis._scale.Max &&
                    _x2Axis._scale.Min < _x2Axis._scale.Max;
            foreach (Axis axis in _yAxisList)
                if (axis._scale.Min >= axis._scale.Max)
                    showGraf = false;
            foreach (Axis axis in _y2AxisList)
                if (axis._scale.Min >= axis._scale.Max)
                    showGraf = false;

            return showGraf;
        }
        #endregion

        #endregion

        #region Serializations
        private void Overlays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
        #endregion

        #region Defaults
        /// <summary>
        /// Default parameters of <see cref="PaneBase"/>.
        /// </summary>
        public new struct Default
        {


            public static bool IsIgnoreInitial = false;
            public static bool IsBoundedRanges = false;
            public static bool IsAxisEqual = false;

            public static double ClusterScaleWidth = 1.0;
            public static double NearestTol = 7.0;
        }
        #endregion

    }
}
