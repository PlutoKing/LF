/*──────────────────────────────────────────────────────────────
 * FileName     : LFMatrix3D
 * Created      : 2020-11-01 21:14:04
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
    public class LFMatrix3D:LFMatrix
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LFMatrix3D()
            :base(4,4)
        {
        }

        public LFMatrix3D(LFMatrix m)
            :base(m)
        {

        }
        #endregion

        #region Methods

        public static LFMatrix3D IdentityMatrix()
        {
            LFMatrix3D m = new LFMatrix3D();
            for (int i = 0; i < 4; i++)
            {
                m[i, i] = 1;
            }
            return m;
        }

        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        private static LFMatrix3D Scale(double sx, double sy, double sz)
        {
            LFMatrix3D m = IdentityMatrix();
            m[0, 0] = sx;
            m[1, 1] = sy;
            m[2, 2] = sz;
            return m;
        }

        /// <summary>
        /// 平移矩阵
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dz"></param>
        /// <returns></returns>
        public static LFMatrix3D Translate(double dx, double dy, double dz)
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[0, 3] = dx;
            m[1, 3] = dy;
            m[2, 3] = dz;
            return m;
        }

        /// <summary>
        /// 绕X轴旋转矩阵
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static LFMatrix3D RotateX(double theta)
        {
            theta *= Math.PI / 180d;
            double sn = Math.Sin(theta);
            double cn = Math.Cos(theta);

            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[1, 1] = cn;
            m[1, 2] = -sn;
            m[2, 1] = sn;
            m[2, 2] = cn;
            return m;
        }

        /// <summary>
        /// 绕Y轴旋转矩阵
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static LFMatrix3D RotateY(double theta)
        {
            theta *= Math.PI / 180d;
            double sn = Math.Sin(theta);
            double cn = Math.Cos(theta);

            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[0, 0] = cn;
            m[0, 2] = sn;
            m[2, 0] = -sn;
            m[2, 2] = cn;
            return m;
        }

        /// <summary>
        /// 绕Z轴旋转矩阵
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static LFMatrix3D RotateZ(double theta)
        {
            theta *= Math.PI / 180d;
            double sn = Math.Sin(theta);
            double cn = Math.Cos(theta);

            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[0, 0] = cn;
            m[0, 1] = -sn;
            m[1, 0] = sn;
            m[1, 1] = cn;
            return m;
        }

        /// <summary>
        /// 前视图矩阵
        /// </summary>
        /// <returns></returns>
        public static LFMatrix3D FrontView()
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[2, 2] = 0;
            return m;
        }

        /// <summary>
        /// 侧视图矩阵
        /// </summary>
        /// <returns></returns>
        public static LFMatrix3D SideView()
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[0, 0] = 0; 
            m[2, 2] = 0;
            m[0, 2] = -1;
            return m;
        }

        /// <summary>
        /// 俯视图矩阵
        /// </summary>
        /// <returns></returns>
        public static LFMatrix3D TopView()
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            m[1, 1] = 0;
            m[2, 2] = 0;
            m[1, 2] = -1;
            return m;
        }

        /// <summary>
        /// 轴侧投影
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static LFMatrix3D Axonometric(double alpha,double beta)
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            double sna = Math.Sin(alpha * Math.PI / 180d);
            double cna = Math.Cos(alpha * Math.PI / 180d);
            double snb = Math.Sin(beta * Math.PI / 180d);
            double cnb = Math.Cos(beta * Math.PI / 180d);

            m[0, 0] = cnb;
            m[0, 2] = snb;
            m[1, 0] = sna * snb;
            m[1, 1] = cna;
            m[1, 2] = -sna * snb;
            m[2, 2] = 0;
            return m;

        }

        /// <summary>
        /// 斜投影
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static LFMatrix3D Oblique(double alpha, double theta)
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);
            double ta = Math.Tan(alpha * Math.PI / 180d);
            double snt = Math.Sin(theta * Math.PI / 180d);
            double cnt = Math.Cos(theta * Math.PI / 180d);

            m[0, 2] = -cnt / ta;
            m[1, 2] = -snt / ta;
            m[2, 2] = 0;
            return m;

        }

        /// <summary>
        /// 透视投影
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static LFMatrix3D Perspective(float d)
        {
            LFMatrix3D m = (LFMatrix3D)IdentityMatrix(4);

            m[3, 2] = -1 / d;

            return m;

        }

        /// <summary>
        /// 视角矩阵
        /// </summary>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        /// <returns></returns>
        public static LFMatrix3D AzimuthElevation(double azimuth, double elevation)
        {
            LFMatrix3D m = IdentityMatrix();

            /* 确保视角范围 */
            if (elevation > 90)
                elevation = 90;
            else if (elevation < -90)
                elevation = -90;

            /* 确保方位角方位 */
            if (azimuth > 180)
                azimuth = 180;
            else if (azimuth < -180)
                azimuth = -180;

            elevation*= Math.PI / 180d;
            azimuth *= Math.PI / 180d;
            double sne = Math.Sin(elevation);
            double cne = Math.Cos(elevation);
            double sna = Math.Sin(azimuth);
            double cna = Math.Cos(azimuth);

            m[0, 0] = cna;
            m[0, 1] = sna;
            m[0, 2] = 0;
            m[1, 0] = -sne * sna;
            m[1, 1] = sne * sna;
            m[1, 2] = cne;
            m[2, 0] = cne*sna;
            m[2, 1] = -cne*cna;
            m[2, 2] = sne;

            return m;
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
