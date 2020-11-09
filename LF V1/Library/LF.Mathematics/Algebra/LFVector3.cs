/*──────────────────────────────────────────────────────────────
 * FileName     : LFVector3
 * Created      : 2020-10-20 19:27:01
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
    public class LFVector3:LFVector
    {
        #region Fields
        #endregion

        #region Properties

        /// <summary>
        /// 第一个变量
        /// </summary>
        public double X
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        /// <summary>
        /// 第二个变量
        /// </summary>
        public double Y
        {
            get { return this[1]; }
            set { this[1] = value; }
        }

        /// <summary>
        /// 第三个变量
        /// </summary>
        public double Z
        {
            get { return this[2]; }
            set { this[2] = value; }
        }
        #endregion

        #region Constructors
        public LFVector3()
            :base(VectorType.ColVector,3)
        {
        }

        public LFVector3(LFVector v)
            :base(v)
        {

        }

        public LFVector3(double [] elements)
            :base(VectorType.ColVector,elements)
        {

        }

        public LFVector3(double x,double y,double z)
            :base(VectorType.ColVector,new double[] { x,y,z})
        {

        }

        public LFVector3(LFVector3 rhs)
            :base(VectorType.ColVector,rhs.Elements)
        {

        }

        #endregion

        #region Methods

        #region Computation Methods

        /// <summary>
        /// +<see cref="LFVector"/>，原向量
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static LFVector3 operator +(LFVector3 v)
        {
            return new LFVector3(v.Elements);
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static LFVector3 operator -(LFVector3 v)
        {
            LFVector3 r = new LFVector3();
            for (int i = 1; i < v.Length; i++)
            {
                r[i] = -v[i];
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector3 operator +(LFVector3 v1, LFVector3 v2)
        {

            LFVector3 r = new LFVector3();
            for (int i = 0; i < 3; i++)
            {
                r[i] = v1[i] + v2[i];
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>减法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector3 operator -(LFVector3 v1, LFVector3 v2)
        {
            LFVector3 r = new LFVector3();
            for (int i = 0; i < 3; i++)
            {
                r[i] = v1[i] - v2[i];
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator *(LFVector3 v1, LFVector3 v2)
        {
            LFVector3 r = new LFVector3();
            for (int i = 0; i < 3; i++)
            {
                r[i] = v1[i] * v2[i];
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector3 operator *(double k, LFVector3 v)
        {
            LFVector3 r = new LFVector3();
            for (int i = 0; i < 3; i++)
            {
                r[i] = v[i] * k;
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector3 operator *(LFVector3 v, double k)
        {
            
            LFVector3 r = new LFVector3();
            for (int i = 0; i < 3; i++)
            {
                r[i] = v[i] * k;
            }
            return r;
        }


        public static LFVector3 operator*(LFMatrix m,LFVector3 v)
        {
            LFVector3 r = new LFVector3();
            
            for(int i = 0; i < 3; i++)
            {
                r[i] = 0;
                for (int j = 0; j < 3; j++)
                {
                    r[i] += m[i, j] * v[j];
                }
            }

            return r;
        }

        #endregion


        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
