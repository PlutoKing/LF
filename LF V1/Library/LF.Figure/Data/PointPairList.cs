/*──────────────────────────────────────────────────────────────
 * FileName     : PointPairList
 * Created      : 2020-01-09 11:10:30
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
    public class PointPairList : List<PointPair>, IPointList, IPointListEdit
    {
        #region Fields
        protected bool _sorted = true;
        #endregion

        #region Properties
        public bool Sorted
        {
            get { return _sorted; }
        }
        #endregion

        #region Constructors
        public PointPairList()
        {
            _sorted = false;
        }

        public PointPairList(double[] x, double[] y)
        {
            Add(x, y);

            _sorted = false;
        }

        public PointPairList(IPointList list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
                Add(list[i]);

            _sorted = false;
        }

        public PointPairList(double[] x, double[] y, double[] baseVal)
        {
            Add(x, y, baseVal);

            _sorted = false;
        }

        public PointPairList(PointPairList rhs)
        {
            Add(rhs);

            _sorted = false;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public PointPairList Clone()
        {
            return new PointPairList(this);
        }
        #endregion

        #region Methods
        public new void Add(PointPair point)
        {
            _sorted = false;
            //base.Add( new PointPair( point ) );
            base.Add(point.Clone());
        }

        public void Add(PointPairList pointList)
        {
            foreach (PointPair point in pointList)
                Add(point);

            _sorted = false;
        }

        public void Add(double[] x, double[] y)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair(0, 0, 0);
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = double.MaxValue;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = double.MaxValue;

                base.Add(point);
            }

            _sorted = false;
        }

        public void Add(double[] x, double[] y, double[] z)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;
            if (z != null && z.Length > len)
                len = z.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair();

                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = double.MaxValue;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = double.MaxValue;

                if (z == null)
                    point.Z = (double)i + 1.0;
                else if (i < z.Length)
                    point.Z = z[i];
                else
                    point.Z = double.MaxValue;

                base.Add(point);
            }

            _sorted = false;
        }

        public void Add(double x, double y)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y);
            base.Add(point);
        }
        public void Add(double x, double y, string tag)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, tag);
            base.Add(point);
        }
        public void Add(double x, double y, double z)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, z);
            base.Add(point);
        }
        public void Add(double x, double y, double z, string tag)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, z, tag);
            base.Add(point);
        }
        public new void Insert(int index, PointPair point)
        {
            _sorted = false;
            base.Insert(index, point);
        }
        public void Insert(int index, double x, double y)
        {
            _sorted = false;
            base.Insert(index, new PointPair(x, y));
        }
        public void Insert(int index, double x, double y, double z)
        {
            _sorted = false;
            Insert(index, new PointPair(x, y, z));
        }
        public int IndexOfTag(string label)
        {
            int iPt = 0;
            foreach (PointPair p in this)
            {
                if (p.Tag is string && String.Compare((string)p.Tag, label, true) == 0)
                    return iPt;
                iPt++;
            }

            return -1;
        }
        public override bool Equals(object obj)
        {
            PointPairList rhs = obj as PointPairList;
            if (this.Count != rhs.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(rhs[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public new bool Sort()
        {
            // if it is already sorted we don't have to sort again
            if (_sorted)
                return true;

            Sort(new PointPair.PointPairComparer(SortType.XValues));
            return false;
        }

        public bool Sort(SortType type)
        {
            // if it is already sorted we don't have to sort again
            if (_sorted)
                return true;

            this.Sort(new PointPair.PointPairComparer(type));

            return false;
        }

        public void SetX(double[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (i < this.Count)
                    this[i].X = x[i];
            }

            _sorted = false;
        }
        public void SetY(double[] y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (i < this.Count)
                    this[i].Y = y[i];
            }

            _sorted = false;
        }
        public void SetZ(double[] z)
        {
            for (int i = 0; i < z.Length; i++)
            {
                if (i < this.Count)
                    this[i].Z = z[i];
            }

            _sorted = false;
        }
        public void SumY(PointPairList sumList)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (i < sumList.Count)
                    this[i].Y += sumList[i].Y;
            }

            //sorted = false;
        }
        public void SumX(PointPairList sumList)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (i < sumList.Count)
                    this[i].X += sumList[i].X;
            }

            _sorted = false;
        }
        public double InterpolateX(double xTarget)
        {
            int lo, mid, hi;
            if (this.Count < 2)
                throw new Exception("Error: Not enough points in curve to interpolate");

            if (xTarget <= this[0].X)
            {
                lo = 0;
                hi = 1;
            }
            else if (xTarget >= this[this.Count - 1].X)
            {
                lo = this.Count - 2;
                hi = this.Count - 1;
            }
            else
            {
                // if x is within the bounds of the x table, then do a binary search
                // in the x table to find table entries that bound the x value
                lo = 0;
                hi = this.Count - 1;

                // limit to 1000 loops to avoid an infinite loop problem
                int j;
                for (j = 0; j < 1000 && hi > lo + 1; j++)
                {
                    mid = (hi + lo) / 2;
                    if (xTarget > this[mid].X)
                        lo = mid;
                    else
                        hi = mid;
                }

                if (j >= 1000)
                    throw new Exception("Error: Infinite loop in interpolation");
            }

            return (xTarget - this[lo].X) / (this[hi].X - this[lo].X) *
                    (this[hi].Y - this[lo].Y) + this[lo].Y;

        }
        public double SplineInterpolateX(double xTarget, double tension)
        {
            // Scale the tension value to be compatible with the GDI+ values
            tension /= 3.0;

            int lo, mid, hi;
            if (this.Count < 2)
                throw new Exception("Error: Not enough points in curve to interpolate");

            // Extrapolation not allowed
            if (xTarget <= this[0].X || xTarget >= this[this.Count - 1].X)
                return double.MaxValue;
            else
            {
                // if x is within the bounds of the x table, then do a binary search
                // in the x table to find table entries that bound the x value
                lo = 0;
                hi = this.Count - 1;

                // limit to 1000 loops to avoid an infinite loop problem
                int j;
                for (j = 0; j < 1000 && hi > lo + 1; j++)
                {
                    mid = (hi + lo) / 2;
                    if (xTarget > this[mid].X)
                        lo = mid;
                    else
                        hi = mid;
                }

                if (j >= 1000)
                    throw new Exception("Error: Infinite loop in interpolation");
            }

            // At this point, we know the two bounding points around our point of interest
            // We need the four points that surround our point

            double X0, X1, X2, X3;
            double Y0, Y1, Y2, Y3;
            double B0, B1, B2, B3;

            X1 = this[lo].X;
            X2 = this[hi].X;
            Y1 = this[lo].Y;
            Y2 = this[hi].Y;

            // if we are at either the beginning of the table or the end, then make up a before
            // and/or after point to fill in the four points
            if (lo == 0)
            {
                X0 = X1 - (X2 - X1) / 3;
                Y0 = Y1 - (Y2 - Y1) / 3;
            }
            else
            {
                X0 = this[lo - 1].X;
                Y0 = this[lo - 1].Y;
            }

            if (hi == this.Count - 1)
            {
                X3 = X2 + (X2 - X1) / 3;
                Y3 = Y2 + (Y2 - Y1) / 3;
            }
            else
            {
                X3 = this[hi + 1].X;
                Y3 = this[hi + 1].Y;
            }

            double newX, newY,
                        lastX = X1,
                        lastY = Y1;

            // Do 100 steps to find the result
            for (double t = 0.01; t <= 1; t += 0.01)
            {
                B0 = (1 - t) * (1 - t) * (1 - t);
                B1 = 3.0 * t * (1 - t) * (1 - t);
                B2 = 3.0 * t * t * (1 - t);
                B3 = t * t * t;

                newX = X1 * B0 + (X1 + (X2 - X0) * tension) * B1 +
                        (X2 - (X3 - X1) * tension) * B2 + X2 * B3;
                newY = Y1 * B0 + (Y1 + (Y2 - Y0) * tension) * B1 +
                        (Y2 - (Y3 - Y1) * tension) * B2 + Y2 * B3;

                // We are looking for the first X that exceeds the target
                if (newX >= xTarget)
                {
                    // We now have two bounding X values around our target
                    // use linear interpolation to minimize the discretization
                    // error.
                    return (xTarget - lastX) / (newX - lastX) *
                            (newY - lastY) + lastY;
                }

                lastX = newX;
                lastY = newY;
            }

            // This should never happen
            return Y2;
        }
        public double InterpolateY(double yTarget)
        {
            int lo, mid, hi;
            if (this.Count < 2)
                throw new Exception("Error: Not enough points in curve to interpolate");

            if (yTarget <= this[0].Y)
            {
                lo = 0;
                hi = 1;
            }
            else if (yTarget >= this[this.Count - 1].Y)
            {
                lo = this.Count - 2;
                hi = this.Count - 1;
            }
            else
            {
                // if y is within the bounds of the y table, then do a binary search
                // in the y table to find table entries that bound the y value
                lo = 0;
                hi = this.Count - 1;

                // limit to 1000 loops to avoid an infinite loop problem
                int j;
                for (j = 0; j < 1000 && hi > lo + 1; j++)
                {
                    mid = (hi + lo) / 2;
                    if (yTarget > this[mid].Y)
                        lo = mid;
                    else
                        hi = mid;
                }

                if (j >= 1000)
                    throw new Exception("Error: Infinite loop in interpolation");
            }

            return (yTarget - this[lo].Y) / (this[hi].Y - this[lo].Y) *
                    (this[hi].X - this[lo].X) + this[lo].X;

        }
        public PointPairList LinearRegression(IPointList points, int pointCount)
        {
            double minX = double.MaxValue;
            double maxX = double.MinValue;

            for (int i = 0; i < points.Count; i++)
            {
                PointPair pt = points[i];

                if (!pt.IsInvalid)
                {
                    minX = pt.X < minX ? pt.X : minX;
                    maxX = pt.X > maxX ? pt.X : maxX;
                }
            }

            return LinearRegression(points, pointCount, minX, maxX);
        }
        public PointPairList LinearRegression(IPointList points, int pointCount,
    double minX, double maxX)
        {
            double x = 0, y = 0, xx = 0, xy = 0, count = 0;
            for (int i = 0; i < points.Count; i++)
            {
                PointPair pt = points[i];
                if (!pt.IsInvalid)
                {
                    x += points[i].X;
                    y += points[i].Y;
                    xx += points[i].X * points[i].X;
                    xy += points[i].X * points[i].Y;
                    count++;
                }
            }

            if (count < 2 || maxX - minX < 1e-20)
                return null;

            double slope = (count * xy - x * y) / (count * xx - x * x);
            double intercept = (y - slope * x) / count;

            PointPairList newPoints = new PointPairList();
            double stepSize = (maxX - minX) / pointCount;
            double value = minX;
            for (int i = 0; i < pointCount; i++)
            {
                newPoints.Add(new PointPair(value, value * slope + intercept));
                value += stepSize;
            }

            return newPoints;
        }


        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
