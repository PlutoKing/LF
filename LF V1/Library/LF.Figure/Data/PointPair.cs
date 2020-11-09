/*──────────────────────────────────────────────────────────────
 * FileName     : PointPair
 * Created      : 2020-01-09 11:00:53
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
    public class PointPair : PointPairBase, ICloneable
    {
        #region Fields
        public double Z;
        public object Tag;
        #endregion

        #region Properties
        public bool IsInvalid3D
        {
            get
            {
                return this.X == double.MaxValue ||
                          this.Y == double.MaxValue ||
                          this.Z == double.MaxValue ||
                          Double.IsInfinity(this.X) ||
                          Double.IsInfinity(this.Y) ||
                          Double.IsInfinity(this.Z) ||
                          Double.IsNaN(this.X) ||
                          Double.IsNaN(this.Y) ||
                          Double.IsNaN(this.Z);
            }
        }

        public double LowValue
        {
            get { return this.Z; }
            set { this.Z = value; }
        }

        virtual public double ColorValue
        {
            get { return Z; }
            set { Z = value; }
        }
        #endregion

        #region Constructors
        public PointPair() : this(0, 0, 0, null)
        {
        }

        public PointPair(double x, double y)
            : this(x, y, 0, null)
        {
        }

        public PointPair(double x, double y, string label)
            : this(x, y, 0, label as object)
        {
        }

        public PointPair(double x, double y, double z)
            : this(x, y, z, null)
        {
        }

        public PointPair(double x, double y, double z, string label)
            : this(x, y, z, label as object)
        {
        }

        public PointPair(double x, double y, double z, object tag)
            : base(x, y)
        {
            this.Z = z;
            this.Tag = tag;
        }

        public PointPair(PointF pt) : this(pt.X, pt.Y, 0, null)
        {
        }

        public PointPair(PointPair rhs) : base(rhs)
        {
            this.Z = rhs.Z;

            if (rhs.Tag is ICloneable)
                this.Tag = ((ICloneable)rhs.Tag).Clone();
            else
                this.Tag = rhs.Tag;
        }


        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public PointPair Clone()
        {
            return new PointPair(this);
        }
        #endregion

        #region Methods

        public class PointPairComparerY : IComparer<PointPair>
        {

            /// <summary>
            /// Compares two <see cref="PointPair"/>s.
            /// </summary>
            /// <param name="l">Point to the left.</param>
            /// <param name="r">Point to the right.</param>
            /// <returns>-1, 0, or 1 depending on l.Y's relation to r.Y</returns>
            public int Compare(PointPair l, PointPair r)
            {
                if (l == null && r == null)
                {
                    return 0;
                }
                else if (l == null && r != null)
                {
                    return -1;
                }
                else if (l != null && r == null)
                {
                    return 1;
                }

                double lY = l.Y;
                double rY = r.Y;

                if (System.Math.Abs(lY - rY) < .000000001)
                    return 0;

                return lY < rY ? -1 : 1;
            }
        }

        public class PointPairComparer : IComparer<PointPair>
        {
            private SortType sortType;

            /// <summary>
            /// Constructor for PointPairComparer.
            /// </summary>
            /// <param name="type">The axis type on which to sort.</param>
            public PointPairComparer(SortType type)
            {
                this.sortType = type;
            }

            /// <summary>
            /// Compares two <see cref="PointPair"/>s.
            /// </summary>
            /// <param name="l">Point to the left.</param>
            /// <param name="r">Point to the right.</param>
            /// <returns>-1, 0, or 1 depending on l.X's relation to r.X</returns>
            public int Compare(PointPair l, PointPair r)
            {
                if (l == null && r == null)
                    return 0;
                else if (l == null && r != null)
                    return -1;
                else if (l != null && r == null)
                    return 1;

                double lVal, rVal;

                if (sortType == SortType.XValues)
                {
                    lVal = l.X;
                    rVal = r.X;
                }
                else
                {
                    lVal = l.Y;
                    rVal = r.Y;
                }

                if (lVal == double.MaxValue || Double.IsInfinity(lVal) || Double.IsNaN(lVal))
                    l = null;
                if (rVal == double.MaxValue || Double.IsInfinity(rVal) || Double.IsNaN(rVal))
                    r = null;

                if ((l == null && r == null) || (System.Math.Abs(lVal - rVal) < 1e-100))
                    return 0;
                else if (l == null && r != null)
                    return -1;
                else if (l != null && r == null)
                    return 1;
                else
                    return lVal < rVal ? -1 : 1;
            }
        }

        public override bool Equals(object obj)
        {
            PointPair rhs = obj as PointPair;
            return this.X == rhs.X && this.Y == rhs.Y && this.Z == rhs.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        virtual public string ToString(bool isShowZ)
        {
            return this.ToString("g", isShowZ);
        }

        virtual public string ToString(string format, bool isShowZ)
        {
            return "( " + this.X.ToString(format) +
                    ", " + this.Y.ToString(format) +
                    (isShowZ ? (", " + this.Z.ToString(format)) : "")
                    + " )";
        }

        public string ToString(string formatX, string formatY, string formatZ)
        {
            return "( " + this.X.ToString(formatX) +
                    ", " + this.Y.ToString(formatY) +
                    ", " + this.Z.ToString(formatZ) +
                    " )";
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }

    public enum SortType
    {
        /// <summary>
        /// Use the Y values to sort the list.
        /// </summary>
        YValues,
        /// <summary>
        /// Use the X values to sort the list.
        /// </summary>
        XValues
    };
}
