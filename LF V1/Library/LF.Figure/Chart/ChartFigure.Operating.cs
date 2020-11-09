/*──────────────────────────────────────────────────────────────
 * FileName     : ChartFigure
 * Created      : 2020-10-16 14:56:30
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
    public partial class ChartFigure
    {
        #region Transform

        public void Transform(double x, double y, out float pixX, out float pixY)
        {
            pixX = _xAxis.Scale.Transform(x);
            pixY = _yAxisList[0].Scale.Transform(y);
        }
        /// <summary>
        /// 坐标<paramref name="ptF"/>转化为图表坐标系
        /// </summary>
        /// <param name="ptF"></param>
        /// <param name="x"></param>
        /// <param name="x2"></param>
        /// <param name="y"></param>
        /// <param name="y2"></param>
        public void ReverseTransform(PointF ptF, out double x, out double x2, out double[] y,
            out double[] y2)
        {
            _xAxis.Scale.SetupScaleData(this, _xAxis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            _x2Axis.Scale.SetupScaleData(this, _x2Axis);
            x2 = this.X2Axis.Scale.ReverseTransform(ptF.X);

            y = new double[_yAxisList.Count];
            y2 = new double[_y2AxisList.Count];

            for (int i = 0; i < _yAxisList.Count; i++)
            {
                Axis axis = _yAxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
            for (int i = 0; i < _y2AxisList.Count; i++)
            {
                Axis axis = _y2AxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y2[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
        }

        public void ReverseTransform(PointF ptF, bool isX2Axis, bool isY2Axis, int yAxisIndex,
           out double x, out double y)
        {
            // Setup the scaling data based on the chart rect
            Axis xAxis = _xAxis;
            if (isX2Axis)
                xAxis = _x2Axis;

            xAxis.Scale.SetupScaleData(this, xAxis);
            x = xAxis.Scale.ReverseTransform(ptF.X);

            Axis yAxis = null;
            if (isY2Axis && Y2AxisList.Count > yAxisIndex)
                yAxis = Y2AxisList[yAxisIndex];
            else if (!isY2Axis && YAxisList.Count > yAxisIndex)
                yAxis = YAxisList[yAxisIndex];

            if (yAxis != null)
            {
                yAxis.Scale.SetupScaleData(this, yAxis);
                y = yAxis.Scale.ReverseTransform(ptF.Y);
            }
            else
                y = double.MaxValue;
        }

        #endregion

        #region Zoom Methods
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="zoomFraction">缩放因数</param>
        /// <param name="centerPt">中心点</param>
        /// <param name="isZoomOnCenter"></param>
        /// <param name="isRefresh"></param>
        public void Zoom(double zoomFraction, PointF centerPt, bool isZoomOnCenter, bool isRefresh)
        {
            double x;
            double x2;
            double[] y;
            double[] y2;

            ReverseTransform(centerPt, out x, out x2, out y, out y2);
            ZoomScale(XAxis, zoomFraction, x, isZoomOnCenter);
            ZoomScale(X2Axis, zoomFraction, x2, isZoomOnCenter);
            for (int i = 0; i < YAxisList.Count; i++)
                ZoomScale(YAxisList[i], zoomFraction, y[i], isZoomOnCenter);
            for (int i = 0; i < Y2AxisList.Count; i++)
                ZoomScale(Y2AxisList[i], zoomFraction, y2[i], isZoomOnCenter);

            AxisChange();
        }

        protected void ZoomScale(Axis axis, double zoomFraction, double centerVal, bool isZoomOnCenter)
        {
            axis?.Scale.Zoom(zoomFraction, centerVal, isZoomOnCenter);
        }



        #endregion

        #region Pan Methods
        public Point Pan(Point mousePt, Point _dragStartPt)
        {
            if (_chart.Area.Contains(mousePt))
            {
                double x1, x2, xx1, xx2;
                double[] y1, y2, yy1, yy2;
                //PointF endPoint = mousePt;
                //PointF startPoint = ( (Control)sender ).PointToClient( this.dragRect.Location );

                ReverseTransform(_dragStartPt, out x1, out xx1, out y1, out yy1);   // 起点
                ReverseTransform(mousePt, out x2, out xx2, out y2, out yy2);    // 终点

                PanScale(XAxis, x1, x2);
                PanScale(X2Axis, xx1, xx2);
                for (int i = 0; i < y1.Length; i++)
                    PanScale(YAxisList[i], y1[i], y2[i]);
                for (int i = 0; i < yy1.Length; i++)
                    PanScale(Y2AxisList[i], yy1[i], yy2[i]);

                AxisChange();
            }
            return mousePt;
        }

        protected void PanScale(Axis axis, double startVal, double endVal)
        {

            axis?.Scale.Pan(startVal, endVal);
        }
        #endregion


    }
}
