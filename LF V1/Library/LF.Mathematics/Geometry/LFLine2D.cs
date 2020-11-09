/*──────────────────────────────────────────────────────────────
 * FileName     : LFLine2D
 * Created      : 2020-10-13 15:39:32
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


namespace LF.Mathematics
{
    /// <summary>
    /// 2D线段
    /// </summary>
    public class LFLine2D:ICloneable
    {
        #region Fields
        private LFPoint2D _start;
        private LFPoint2D _end;

        #endregion

        #region Properties
        /// <summary>
        /// 起点
        /// </summary>
        public LFPoint2D Start { get => _start; set => _start = value; }
        /// <summary>
        /// 终点
        /// </summary>
        public LFPoint2D End { get => _end; set => _end = value; }

        /// <summary>
        /// 线段长度
        /// </summary>
        public double Length
        {
            get { return LFPoint2D.Distance(_start, _end); }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        public LFLine2D(LFPoint2D start, LFPoint2D end)
        {
            if (start == end)
            {
                throw new Exception("线段起点与终点不可以相同，将退化为点。");
            }
            _start = start;
            _end = end;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x1">起点x坐标</param>
        /// <param name="y1">起点y坐标</param>
        /// <param name="x2">终点x坐标</param>
        /// <param name="y2">终点y坐标</param>
        public LFLine2D(double x1, double y1, double x2, double y2)
            : this(new LFPoint2D(x1, y1), new LFPoint2D(x2, y2))
        {


        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFLine2D(LFLine2D rhs)
        {
            this._start = rhs._start.Clone();
            this._end = rhs._end.Clone();
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFLine2D Clone()
        {
            return new LFLine2D(this);
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods
        /// <summary>
        /// 判断是否与<paramref name="line"/>相交
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IntersectWith(LFLine2D line)
        {
            // 排斥检测
            if (Math.Max(line._start.X, line.End.X) < Math.Min(_start.X, _end.X)
                || Math.Min(line._start.X, line.End.X) > Math.Max(_start.X, _end.X)
                || Math.Max(line._start.Y, line.End.Y) < Math.Min(_start.Y, _end.Y)
                || Math.Min(line._start.Y, line.End.Y) > Math.Max(_start.Y, _end.Y))
                return false;

            // 叉乘检测

            double crossP1P2Q1 = LFPoint2D.Cross(_start, _end, line.Start);
            double crossP1Q2P2 = LFPoint2D.Cross(_start, line.End, _end);
            double crossQ1Q2P1 = LFPoint2D.Cross(line.Start, line.End, _start);
            double crossQ1P2Q2 = LFPoint2D.Cross(line.Start, _end, line.End);

            if ((crossP1P2Q1 * crossP1Q2P2 < 0) || (crossQ1Q2P1 * crossQ1P2Q2 < 0))
                return false;

            return true;
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
