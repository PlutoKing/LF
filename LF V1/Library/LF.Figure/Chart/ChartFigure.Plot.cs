/*──────────────────────────────────────────────────────────────
 * FileName     : ChartFigure
 * Created      : 2020-10-16 14:57:04
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
using LF.Mathematics;

namespace LF.Figure
{
    public partial class ChartFigure
    {
        public ImageItem PlotImage(LFPoint2D point, Image image)
        {
            ImageItem item = new ImageItem();
            item.Image = image;
            Data d = new Data(this);
            d.X = point.X;
            d.Y = point.Y;
            item.Datas.Add(d);

            Data d2 = new Data(this);
            d2.X = point.X + image.Width;
            d2.Y = point.Y + image.Height;
            item.Datas.Add(d2);

            item.IsLineVisible = true;
            item.Symbol.Type = SymbolType.None;

            DefaultOverlay.Items.Add(item);

            return item;

        }

        #region 绘制散点图
        public LineItem PlotPoint(List<LFPoint2D> points)
        {
            LineItem item = new LineItem();

            for (int i = 0; i < points.Count; i++)
            {
                Data d = new Data(this);
                d.X = points[i].X;
                d.Y = points[i].Y;
                item.Datas.Add(d);
            }
            item.IsLineVisible = false;
            item.Symbol.Type = SymbolType.Circle;


            DefaultOverlay.Items.Add(item);

            return item;
        }

        public LineItem PlotPoint(List<LFPoint2D> points, Overlay overlay)
        {
            LineItem item = new LineItem();

            for (int i = 0; i < points.Count; i++)
            {
                Data d = new Data(this);
                d.X = points[i].X;
                d.Y = points[i].Y;
                item.Datas.Add(d);
            }
            item.IsLineVisible = false;
            item.Symbol.Type = SymbolType.Circle;


            overlay.Items.Add(item);

            return item;
        }
        #endregion

        #region 绘制折线图
        public LineItem PlotLine(double[] y)
        {
            LineItem line = new LineItem();

            for (int i = 0; i < y.Length; i++)
            {
                Data d = new Data(this);
                d.X = i;
                d.Y = y[i];
                line.Datas.Add(d);
            }

            DefaultOverlay.Items.Add(line);
            this.AxisChange();
            return line;
        }

        /// <summary>
        /// 绘制单个点
        /// </summary>
        /// <param name="x">点坐标X</param>
        /// <param name="y">点坐标Y</param>
        /// <returns></returns>
        public LineItem PlotLine(double x, double y)
        {
            LineItem line = new LineItem();

            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            line.Datas.Add(d);

            DefaultOverlay.Items.Add(line);
            return line;
        }
        public LineItem PlotLine(double x, double y, Overlay overlay)
        {
            LineItem line = new LineItem();

            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            line.Datas.Add(d);

            overlay.Items.Add(line);
            return line;
        }

        public LineItem PlotLine(double[] x, double[] y)
        {
            LineItem line = new LineItem();

            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }

            DefaultOverlay.Items.Add(line);
            return line;
        }

        public LineItem PlotLine(double[] x, double[] y, Overlay overlay)
        {
            LineItem line = new LineItem();

            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }

            overlay.Items.Add(line);
            return line;
        }

        public void PlotLine(double[] x, double[] y, Color color)
        {
            LineItem line = new LineItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }
            line.Line.Color = color;
            DefaultOverlay.Items.Add(line);
        }

        public void PlotLine(double[] x, double[] y, float lineWidth)
        {
            LineItem line = new LineItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }
            line.Line.Width = lineWidth;
            DefaultOverlay.Items.Add(line);
        }

        public void PlotLine(double[] x, double[] y, Color color, float lineWidth)
        {
            LineItem line = new LineItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }
            line.Line.Color = color;
            line.Line.Width = lineWidth;
            DefaultOverlay.Items.Add(line);
        }

        public LineItem PlotLine(double[] x, double[] y, Color color, float lineWidth, SymbolType type)
        {
            LineItem line = new LineItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }
            line.Line.Color = color;
            line.Line.Width = lineWidth;
            line.Symbol.Type = type;
            DefaultOverlay.Items.Add(line);
            return line;
        }

        public LineItem PlotLine(double[] x, double[] y, Color color, float lineWidth, SymbolType type, float symbolSize)
        {
            LineItem line = new LineItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                line.Datas.Add(d);
            }
            line.Line.Color = color;
            line.Line.Width = lineWidth;
            line.Symbol.Type = type;
            line.Symbol.Size = symbolSize;
            DefaultOverlay.Items.Add(line);
            return line;
        }

        public LineItem PlotLine(LFLine2D line)
        {
            LineItem item = new LineItem();

            item.Datas.Add(new Data(this, line.Start.X, line.Start.Y));
            item.Datas.Add(new Data(this, line.End.X, line.End.Y));

            DefaultOverlay.Items.Add(item);

            return item;
        }

        public LineItem PlotLine(List<LFPoint2D> points)
        {
            LineItem item = new LineItem();

            for (int i = 0; i < points.Count; i++)
            {
                Data d = new Data(this);
                d.X = points[i].X;
                d.Y = points[i].Y;
                item.Datas.Add(d);
            }

           
            DefaultOverlay.Items.Add(item);

            return item;
        }

        public LineItem PlotRoute(List<LFPoint2D> points)
        {
            LineItem line = new LineItem();

            for (int i = 0; i < points.Count; i++)
            {
                Data d = new Data(this);
                d.X = points[i].X;
                d.Y = points[i].Y;
                line.Datas.Add(d);
            }
            line.Line.Color = Color.FromArgb(255, 215, 0);
            line.Line.Width = 3;
            line.Symbol.Type = SymbolType.Circle;
            line.Symbol.Size = 8;
            line.Line.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            DefaultOverlay.Items.Add(line);

            return line;
        }

        public LineItem PlotLine(List<LFPoint2D> points, Overlay overlay)
        {
            LineItem item = new LineItem();

            for (int i = 0; i < points.Count; i++)
            {
                Data d = new Data(this);
                d.X = points[i].X;
                d.Y = points[i].Y;
                item.Datas.Add(d);
            }

            if (overlay != null)
            {
                overlay.Items.Add(item);
            }

            return item;
        }
        #endregion

        #region 绘制多边形

        public void PlotPolygon(double[] x, double[] y)
        {
            PolygonItem item = new PolygonItem();
            for (int i = 0; i < x.Length; i++)
            {
                Data d = new Data(this);
                d.X = x[i];
                d.Y = y[i];
                item.Datas.Add(d);
            }

            DefaultOverlay.Items.Add(item);
        }

        public void PlotPolygon(LFPolygon2D polygon)
        {
            PolygonItem item = new PolygonItem();
            for (int i = 0; i < polygon.Count; i++)
            {
                Data d = new Data(this);
                d.X = polygon.Points[i].X;
                d.Y = polygon.Points[i].Y;
                item.Datas.Add(d);
            }

            DefaultOverlay.Items.Add(item);
        }

        #endregion

        #region Drone
        /// <summary>
        /// 绘制无人机
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        public void PlotDrone(double x, double y, double theta)
        {
            DroneItem item = new DroneItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.Theta = theta;

            DefaultOverlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制无人机
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        public void PlotDrone(double x, double y, double theta, Overlay overlay)
        {
            DroneItem item = new DroneItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.Theta = 90-theta;

            overlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制无人机
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="lineWidth">线宽</param>
        /// <param name="fillColor">填充颜色</param>
        public void PlotDrone(double x, double y, double theta, Color lineColor, float lineWidth, Color fillColor)
        {
            DroneItem item = new DroneItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.Theta = theta;

            item.LinePen = new Pen(lineColor, lineWidth);
            item.FillBrush = new SolidBrush(fillColor);

            DefaultOverlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制无人机
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="lineWidth">线宽</param>
        /// <param name="fillColor">填充颜色</param>
        public void PlotDrone(double x, double y, double theta, Color lineColor, float lineWidth, Color fillColor, Overlay overlay)
        {
            DroneItem item = new DroneItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.Theta = theta;

            item.LinePen = new Pen(lineColor, lineWidth);
            item.FillBrush = new SolidBrush(fillColor);

            overlay.Items.Add(item);
        }

        #endregion

        #region Task
        /// <summary>
        /// 绘制任务点
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        public void PlotTask(double x, double y, int id)
        {
            TaskItem item = new TaskItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.ID = id;

            DefaultOverlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制任务点
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        public void PlotTask(double x, double y, int id, Overlay overlay)
        {
            TaskItem item = new TaskItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.ID = id;

            overlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制任务点
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="lineWidth">线宽</param>
        /// <param name="fillColor">填充颜色</param>
        public void PlotTask(double x, double y, int id, Color lineColor, float lineWidth, Color fillColor)
        {
            TaskItem item = new TaskItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);
            item.ID = id;

            item.LinePen = new Pen(lineColor, lineWidth);
            item.FillBrush = new SolidBrush(fillColor);

            DefaultOverlay.Items.Add(item);
        }

        /// <summary>
        /// 绘制任务点
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="theta">角度</param>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="lineWidth">线宽</param>
        /// <param name="fillColor">填充颜色</param>
        public void PlotTask(double x, double y, int id, Color lineColor, float lineWidth, Color fillColor, Overlay overlay)
        {
            TaskItem item = new TaskItem();
            Data d = new Data(this);
            d.X = x;
            d.Y = y;
            item.Datas.Add(d);

            item.ID = id;

            item.LinePen = new Pen(lineColor, lineWidth);
            item.FillBrush = new SolidBrush(fillColor);

            overlay.Items.Add(item);
        }

        #endregion
    }

}
