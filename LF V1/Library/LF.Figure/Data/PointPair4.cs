/*──────────────────────────────────────────────────────────────
 * FileName     : PointPair4
 * Created      : 2020-01-09 11:09:04
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
    public class PointPair4 : PointPair
    {
        #region Fields
        public double T;
        #endregion

        #region Properties
        public bool IsInvalid4D
        {
            get
            {
                return this.X == double.MaxValue ||
                        this.Y == double.MaxValue ||
                        this.Z == double.MaxValue ||
                        this.T == double.MaxValue ||
                        Double.IsInfinity(this.X) ||
                        Double.IsInfinity(this.Y) ||
                        Double.IsInfinity(this.Z) ||
                        Double.IsInfinity(this.T) ||
                        Double.IsNaN(this.X) ||
                        Double.IsNaN(this.Y) ||
                        Double.IsNaN(this.Z) ||
                        Double.IsNaN(this.T);
            }
        }
        #endregion

        #region Constructors
        public PointPair4() : base()
        {
            this.T = 0;
        }
        public PointPair4(double x, double y, double z, double t) : base(x, y, z)
        {
            this.T = t;
        }
        public PointPair4(double x, double y, double z, double t, string label) :
                    base(x, y, z, label)
        {
            this.T = t;
        }
        public PointPair4(PointPair4 rhs) : base(rhs)
        {
            this.T = rhs.T;
        }
        #endregion

        #region Methods
        public new string ToString(bool isShowZT)
        {
            return this.ToString("g", isShowZT);
        }

        public new string ToString(string format, bool isShowZT)
        {
            return "( " + this.X.ToString(format) +
                    ", " + this.Y.ToString(format) +
                    (isShowZT ? (", " + this.Z.ToString(format) +
                            ", " + this.T.ToString(format)) : "") + " )";
        }

        public string ToString(string formatX, string formatY, string formatZ, string formatT)
        {
            return "( " + this.X.ToString(formatX) +
                    ", " + this.Y.ToString(formatY) +
                    ", " + this.Z.ToString(formatZ) +
                    ", " + this.T.ToString(formatT) +
                    " )";
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
