/*──────────────────────────────────────────────────────────────
 * FileName     : PointPairBase
 * Created      : 2020-01-09 11:01:12
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
    public class PointPairBase
    {
        #region Fields
        public double X;
        public double Y;
        #endregion

        #region Properties
        public bool IsInvalid
        {
            get
            {
                return this.X == double.MaxValue ||
                        this.Y == double.MaxValue ||
                        Double.IsInfinity(this.X) ||
                        Double.IsInfinity(this.Y) ||
                        Double.IsNaN(this.X) ||
                        Double.IsNaN(this.Y);
            }
        }

        public static bool IsValueInvalid(double value)
        {
            return (value == double.MaxValue ||
                    Double.IsInfinity(value) ||
                    Double.IsNaN(value));
        }
        #endregion

        #region Constructors
        public PointPairBase()
            : this(0, 0)
        {
        }

        public PointPairBase(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public PointPairBase(PointF pt)
            : this(pt.X, pt.Y)
        {
        }

        public PointPairBase(PointPairBase rhs)
        {
            this.X = rhs.X;
            this.Y = rhs.Y;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 强制转换
        /// </summary>
        /// <param name="pair"></param>
        public static implicit operator PointF(PointPairBase pair)
        {
            return new PointF((float)pair.X, (float)pair.Y);
        }

        public override bool Equals(object obj)
        {
            PointPairBase rhs = obj as PointPairBase;
            return this.X == rhs.X && this.Y == rhs.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.ToString("G");
        }

        public string ToString(string format)
        {
            return "( " + this.X.ToString(format) +
                    ", " + this.Y.ToString(format) +
                    " )";
        }

        public string ToString(string formatX, string formatY)
        {
            return "( " + this.X.ToString(formatX) +
                    ", " + this.Y.ToString(formatY) +
                    " )";
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
