/*──────────────────────────────────────────────────────────────
 * FileName     : LFPoint3D
 * Created      : 2020-11-01 21:12:39
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
    public class LFPoint3D:LFVector
    {
        #region Fields


        #endregion

        #region Properties

        public double X
        {
            get { return this[0]; }
            set { this[0] = value; }
        }
        public double Y
        {
            get { return this[1]; }
            set { this[1] = value; }
        }

        public double Z
        {
            get { return this[2]; }
            set { this[2] = value; }
        }

        public double W
        {
            get { return this[3]; }
            set { this[3] = value; }
        }
        #endregion

        #region Constructors
        public LFPoint3D()
            :base(VectorType.ColVector,4)
        {
        }

        public LFPoint3D(double x,double y,double z)
            :base(VectorType.ColVector,new double[] { x,y,z,1})
        {

        }

        public LFPoint3D(LFVector v)
            : base(v)
        {

        }
        #endregion

        #region Methods
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
