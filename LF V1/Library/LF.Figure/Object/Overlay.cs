/*──────────────────────────────────────────────────────────────
 * FileName     : Overlay
 * Created      : 2020-01-05 13:02:09
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System.Drawing;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace LF.Figure
{
    /// <summary>
    /// 图层
    /// 用于管理绘制对象
    /// </summary>
    public class Overlay
    {
        #region Fields
        /// <summary>
        /// Items集合
        /// </summary>
        public readonly ObservableCollection<IItem> Items = new ObservableCollection<IItem>();

        public ChartFigure _graph;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Overlay(ChartFigure graph)
        {
            _graph = graph;
            Items.CollectionChanged += Items_CollectionChanged;
        }

        
        #endregion

        #region Methods
        public void Draw(Graphics g)
        {
            if (_graph != null)
            {
                foreach(IItem item in Items)
                {
                    if (item.IsVisible)
                    {
                        UpdateLocalPosition(item);
                        item.Draw(g);
                    }
                }
            }
        }

        public void UpdateLocalPosition(IItem item)
        {
            item.LocalPoints.Clear();
            for (int i = 0; i < item.Datas.Count; i++)
            {
                item.LocalPoints.Add(item.Datas[i].FromDataToLocal());
            }
            item.UpdateGraphicsPath();
        }

        //private int maxPts;
        //public void GetRange(bool bIgnoreInitial, bool isBoundedRanges, ChartFigure graph)
        //{
        //    double tXMinVal,
        //                tXMaxVal,
        //                tYMinVal,
        //                tYMaxVal;

        //    InitScale(graph.XAxis.Scale, isBoundedRanges);
        //    InitScale(graph.X2Axis.Scale, isBoundedRanges);

        //    foreach (YAxis axis in graph.YAxisList)
        //        InitScale(axis.Scale, isBoundedRanges);

        //    foreach (Y2Axis axis in graph.Y2AxisList)
        //        InitScale(axis.Scale, isBoundedRanges);

        //    maxPts = 1;

        //    foreach (IItem curve in this.Items)
        //    {
        //        if (curve.IsVisible)
        //        {
        //            // Call the GetRange() member function for the current
        //            // curve to get the min and max values
        //            curve.GetRange(out tXMinVal, out tXMaxVal,
        //                            out tYMinVal, out tYMaxVal, bIgnoreInitial, true, pane);


        //            // isYOrd is true if the Y axis is an ordinal type
        //            Scale yScale = curve.GetYAxis(pane).Scale;

        //            Scale xScale = curve.GetXAxis(pane).Scale;
        //            bool isYOrd = yScale.IsAnyOrdinal;
        //            // isXOrd is true if the X axis is an ordinal type
        //            bool isXOrd = xScale.IsAnyOrdinal;

        //            // For ordinal Axes, the data range is just 1 to Npts
        //            if (isYOrd && !curve.IsOverrideOrdinal)
        //            {
        //                tYMinVal = 1.0;
        //                tYMaxVal = curve.NPts;
        //            }
        //            if (isXOrd && !curve.IsOverrideOrdinal)
        //            {
        //                tXMinVal = 1.0;
        //                tXMaxVal = curve.NPts;
        //            }

        //            // Bar types always include the Y=0 value
        //            if (curve.IsBar)
        //            {
        //                if (pane._barSettings.Base == BarBase.X ||
        //                        pane._barSettings.Base == BarBase.X2)
        //                {
        //                    // Only force z=0 for BarItems, not HiLowBarItems
        //                    if (!(curve is HiLowBarItem))
        //                    {
        //                        if (tYMinVal > 0)
        //                            tYMinVal = 0;
        //                        else if (tYMaxVal < 0)
        //                            tYMaxVal = 0;
        //                    }

        //                    // for non-ordinal axes, expand the data range slightly for bar charts to
        //                    // account for the fact that the bar clusters have a width
        //                    if (!isXOrd)
        //                    {
        //                        tXMinVal -= pane._barSettings._clusterScaleWidth / 2.0;
        //                        tXMaxVal += pane._barSettings._clusterScaleWidth / 2.0;
        //                    }
        //                }
        //                else
        //                {
        //                    // Only force z=0 for BarItems, not HiLowBarItems
        //                    if (!(curve is HiLowBarItem))
        //                    {
        //                        if (tXMinVal > 0)
        //                            tXMinVal = 0;
        //                        else if (tXMaxVal < 0)
        //                            tXMaxVal = 0;
        //                    }

        //                    // for non-ordinal axes, expand the data range slightly for bar charts to
        //                    // account for the fact that the bar clusters have a width
        //                    if (!isYOrd)
        //                    {
        //                        tYMinVal -= pane._barSettings._clusterScaleWidth / 2.0;
        //                        tYMaxVal += pane._barSettings._clusterScaleWidth / 2.0;
        //                    }
        //                }
        //            }

        //            // determine which curve has the maximum number of points
        //            if (curve.NPts > maxPts)
        //                maxPts = curve.NPts;

        //            // If the min and/or max values from the current curve
        //            // are the absolute min and/or max, then save the values
        //            // Also, differentiate between Y and Y2 values

        //            if (tYMinVal < yScale._rangeMin)
        //                yScale._rangeMin = tYMinVal;
        //            if (tYMaxVal > yScale._rangeMax)
        //                yScale._rangeMax = tYMaxVal;


        //            if (tXMinVal < xScale._rangeMin)
        //                xScale._rangeMin = tXMinVal;
        //            if (tXMaxVal > xScale._rangeMax)
        //                xScale._rangeMax = tXMaxVal;
        //        }
        //    }

        //}

        private void InitScale(Scale scale,bool isBoundedRanges)
        {
            scale.RangeMin = double.MaxValue;
            scale.RangeMax = double.MinValue;
            scale.LBound = (isBoundedRanges && !scale.MinAuto) ?
                scale.Min : double.MinValue;
            scale.UBound = (isBoundedRanges && !scale.MaxAuto) ?
                scale.Max : double.MaxValue;

        }
        #endregion

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                double minX = double.MaxValue;
                double maxX = double.MinValue;
                double minY = double.MaxValue;
                double maxY = double.MinValue;               

                foreach (IItem item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.Overlay = this;
                        UpdateLocalPosition(item);

                        double xmin;
                        double xmax;
                        double ymin;
                        double ymax;

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
                if (_graph != null)
                {
                    if (_graph.XAxis.Scale.Max < maxX)
                    {
                        _graph.XAxis.Scale.Max = maxX;
                        _graph.X2Axis.Scale.Max = maxX;
                    }

                    if (_graph.XAxis.Scale.Min > minX)
                    {
                        _graph.XAxis.Scale.Min = minX;
                        _graph.X2Axis.Scale.Min = minX;
                    }

                    if (_graph.YAxis.Scale.Max < maxY)
                    {
                        foreach (YAxis yAxis in _graph.YAxisList)
                        {
                            yAxis.Scale.Max = maxY;
                        }
                        foreach (Y2Axis y2Axis in _graph.Y2AxisList)
                        {
                            y2Axis.Scale.Max = maxY;
                        }
                    }

                    if (_graph.YAxis.Scale.Min > minY)
                    {
                        foreach (YAxis yAxis in _graph.YAxisList)
                        {
                            yAxis.Scale.Min = minY;
                        }
                        foreach (Y2Axis y2Axis in _graph.Y2AxisList)
                        {
                            y2Axis.Scale.Min = minY;
                        }
                    }

                    _graph.AxisChange();
                }
            }
        }

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
