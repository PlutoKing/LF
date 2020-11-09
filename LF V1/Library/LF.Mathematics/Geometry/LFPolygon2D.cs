/*──────────────────────────────────────────────────────────────
 * FileName     : LFPolygon2D
 * Created      : 2020-10-13 10:49:01
 * Author       : Xu Zhe
 * Description  : 多边形
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Mathematics
{
    /// <summary>
    /// 多边形
    /// </summary>
    public class LFPolygon2D:ICloneable
    {
        #region Fields
        private List<LFPoint2D> _points = new List<LFPoint2D>();    // 点列
        #endregion

        #region Properties

        /// <summary>
        /// 顶点数
        /// </summary>
        public int Count
        {
            get { return _points.Count; }
        }

        public List<LFPoint2D> Points { get => _points; set => _points = value; }

        /// <summary>
        /// 边
        /// </summary>
        public List<LFLine2D> Edges
        {
            get
            {
                List<LFLine2D> lines = new List<LFLine2D>();
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    LFLine2D line = new LFLine2D(_points[i], _points[i + 1]);
                    lines.Add(line);
                }
                lines.Add(new LFLine2D(_points.Last<LFPoint2D>(), _points[0]));
                return lines;
            }
        }

        #endregion

        #region Constructors
        public LFPolygon2D()
        {
            _points = new List<LFPoint2D>();
        }
        public LFPolygon2D(List<LFPoint2D> points)
        {
            _points = points;
        }

        public LFPolygon2D(LFPolygon2D rhs)
        {
            _points = new List<LFPoint2D>();
            foreach (LFPoint2D point in rhs._points)
            {
                this._points.Add(point);
            }
        }


        object ICloneable.Clone()
        {
            return new LFPolygon2D(this);
        }
        #endregion

        #region Methods
        #region Basic Methods

        /// <summary>
        /// x坐标列表
        /// </summary>
        /// <returns></returns>
        internal List<double> XList()
        {
            List<double> list = new List<double>();
            foreach (LFPoint2D p in _points)
            {
                list.Add(p.X);
            }
            return list;
        }

        /// <summary>
        /// y坐标列表
        /// </summary>
        /// <returns></returns>
        internal List<double> YList()
        {
            List<double> list = new List<double>();
            foreach (LFPoint2D p in _points)
            {
                list.Add(p.Y);
            }

            return list;
        }

        /// <summary>
        /// x坐标范围
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        internal void XRange(out double xmin, out double xmax)
        {
            List<double> xs = XList();
            xs.Sort();
            xmin = xs[0];
            xmax = xs.Last<double>();
        }

        /// <summary>
        /// y坐标范围
        /// </summary>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        internal void YRange(out double ymin, out double ymax)
        {
            List<double> ys = YList();
            ys.Sort();
            ymin = ys[0];
            ymax = ys.Last<double>();
        }

        #endregion

        #region 几何关系
        /// <summary>
        /// 判断多边形<see cref="LFPolygon2D"/>是否包含点<paramref name="point"/>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsContain(LFPoint2D point)
        {
            double x = point.X;
            double y = point.Y;

            // preliminary check
            double minX;
            double maxX;
            double minY;
            double maxY;
            XRange(out minX, out maxX);
            YRange(out minY, out maxY);
            if (x < minX || x > maxX || y < minY || y > maxY)
                return false;

            // Core algorithm (PNPoly)
            int crossCount = 0;
            int n = Count;
            for (int i = 0; i < n; i++)
            {
                double vx1 = _points[i].X;
                double vy1 = _points[i].Y;

                double vx2 = _points[(i + 1) % n].X;
                double vy2 = _points[(i + 1) % n].Y;


                if (((vy1 > y) != (vy2 > y)) && (x < (vx2 - vx1) * (y - vy1) / (vy2 - vy1) + vx1))
                    crossCount++;
            }

            if (crossCount % 2 == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断多边形是否与直线相交
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsIntersectWith(LFLine2D line)
        {
            bool isContainStart = IsContain(line.Start);
            bool isContainEnd = IsContain(line.End);

            if (isContainStart || isContainEnd)
                return true;
            else
            {
                List<LFLine2D> lines = Edges;
                foreach (LFLine2D l in lines)
                {
                    if (line.IntersectWith(l))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 多边形偏移
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public LFPolygon2D Expand(double offset)
        {
            int n = _points.Count;
            LFPoint2D[] points = new LFPoint2D[n];

            for (int i = 0; i < n; i++)
            {

                LFPoint2D p = _points[i];
                LFPoint2D p1 = _points[i == 0 ? n - 1 : i - 1];
                LFPoint2D p2 = _points[i == n - 1 ? 0 : i + 1];

                double v1x = p1.X - p.X;
                double v1y = p1.Y - p.Y;
                double n1 = Math.Sqrt(v1x * v1x + v1y * v1y);

                v1x /= n1;
                v1y /= n1;

                double v2x = p2.X - p.X;
                double v2y = p2.Y - p.Y;
                double n2 = Math.Sqrt(v2x * v2x + v2y * v2y);

                v2x /= n2;
                v2y /= n2;
                double l = -offset / Math.Sqrt((1 - (v1x * v2x + v1y * v2y)) / 2);

                double vx = v1x + v2x;
                double vy = v1y + v2y;
                double norm = l / Math.Sqrt(vx * vx + vy * vy);

                vx *= norm;
                vy *= norm;

                points[i] = new LFPoint2D(vx + p.X, vy + p.Y);
            }
            return new LFPolygon2D(points.ToList<LFPoint2D>());
        }
        #endregion


        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
