/*──────────────────────────────────────────────────────────────
 * FileName     : LFRoute
 * Created      : 2020-10-21 09:27:06
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

namespace LF.FlightControl.Guidance
{
    public class LFRoute2D : List<LFPoint2D>, ICloneable
    {
        #region Fields
        #endregion

        #region Properties



        #endregion

        #region Constructors
        public LFRoute2D()
        {

        }
        public LFRoute2D(LFPoint2D start)
        {
            this.Add(start);
        }

        public LFRoute2D(LFPoint2D start, LFPoint2D end)
        {
            this.Add(start);
            this.Add(end);
        }

        public LFRoute2D(List<LFPoint2D> points)
        {
            foreach (LFPoint2D p in points)
            {
                this.Add(p);
            }
        }

        public LFRoute2D(LFPoint2D[] points)
        {
            for (int i = 0; i < points.Count(); i++)
            {
                this.Add(points[i]);
            }
        }

        public LFRoute2D(LFRoute2D rhs)
        {
            foreach (LFPoint2D p in rhs)
            {
                this.Add(p.Clone());
            }
        }

        public LFRoute2D Clone()
        {
            return new LFRoute2D(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        /// <summary>
        /// 是否环路
        /// </summary>
        /// <returns></returns>
        public bool IsLoop()
        {
            return this[0] == this.Last();
        }

        /// <summary>
        /// 路线长度
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            double d = 0;
            for (int i = 0; i < this.Count - 1; i++)
            {
                d += LFPoint2D.Distance(this[i], this[i + 1]);
            }
            return d;
        }

        public void AddRoute(LFRoute2D r)
        {
            if (this.Count != 0 && r.Count != 0)
            {
                if (this.Last() == r[0])
                {
                    r.RemoveAt(0);
                }
            }
            foreach (LFPoint2D p in r)
            {
                this.Add(p);
            }
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
