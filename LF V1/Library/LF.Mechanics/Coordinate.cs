/*──────────────────────────────────────────────────────────────
 * FileName     : Coordinate
 * Created      : 2020-10-20 19:25:19
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

namespace LF.Mechanics
{
    public class Coordinate
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Coordinate()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// 转换矩阵：地面坐标系向机体坐标系
        /// </summary>
        /// <param name="attitude"></param>
        /// <returns></returns>
        public static LFMatrix Lbg(LFVector3 attitude)
        {
            double cphi = Math.Cos(attitude.X * Math.PI / 180);
            double sphi = Math.Sin(attitude.X * Math.PI / 180);
            double ctht = Math.Cos(attitude.Y * Math.PI / 180);
            double stht = Math.Sin(attitude.Y * Math.PI / 180);
            double cpsi = Math.Cos(attitude.Z * Math.PI / 180);
            double spsi = Math.Sin(attitude.Z * Math.PI / 180);

            double[,] lgb = new double[,]
            {
                {cpsi*ctht,                   ctht*spsi,      -stht },
                {     cpsi*sphi*stht - cphi*spsi,  cphi*cpsi + sphi*spsi*stht,  ctht*sphi },
                {sphi*spsi + cphi*cpsi*stht,  cphi*spsi*stht - cpsi*sphi,  cphi*ctht },
            };

            return new LFMatrix(lgb);
        }

        /// <summary>
        /// 转换矩阵：机体坐标系向气流坐标系
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static LFMatrix Lab(double alpha,double beta)
        {
            double calpha = Math.Cos(alpha * Math.PI / 180);
            double salpha = Math.Sin(alpha * Math.PI / 180);
            double cbeta = Math.Cos(beta * Math.PI / 180);
            double sbeta = Math.Sin(beta * Math.PI / 180);

            double[,] lab = new double[,]
            {
                {calpha*cbeta,   sbeta,     salpha*cbeta },
                {-calpha*sbeta,   cbeta,    -salpha*sbeta },
                {-salpha,       0,          calpha },
            };

            return new LFMatrix(lab);
        }
        #endregion

        #region Defaults
        #endregion
    }
}
