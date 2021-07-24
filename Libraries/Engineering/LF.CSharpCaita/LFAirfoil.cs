/*──────────────────────────────────────────────────────────────
 * FileName     : LFcs
 * Created      : 2021-06-30 21:25:03
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using INFITF;
using MECMOD;
using HybridShapeTypeLib;

namespace LF.CSharpCatia
{
    /// <summary>
    /// 翼型
    /// </summary>
    public class LFAirfoil
    {
        #region Fields
        /// <summary>
        /// 上下翼面控制参数
        /// </summary>
        public readonly double[][] A = new double[2][];
        /// <summary>
        /// 控制点数目
        /// </summary>
        public int Nc = 50;
        public readonly double[] Psi;
        public double Zt;
        public double S;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFAirfoil(string name)
        {
            switch (name)
            {

                case "S3010":
                    A = Default.AS3010;
                    break;
                case "S5010":
                    A = Default.AS5010;
                    break;
                case "NACA0009":
                    A = Default.ANACA0009;
                    break;
                case "Fuselage":
                    A = Default.F;
                    break;
                default:
                    A = Default.AS5010;
                    break;
            }
            Psi = new double[Nc];
            for (int i = 0; i < Nc; i++)
            {
                Psi[i] = (Math.Sin(i * 1.0 / (Nc - 1) * Math.PI - Math.PI / 2) + 1) / 2;
            }
            Zt = 0;
        }
        #endregion

        #region Methods

        public Reference[] Draw(LFCatia catia,double c, double x, double y, double z, double theta, double phi, double psi, double scale, bool isUp)
        {
            Reference[] r = new Reference[Nc];

            double x1;
            double y1;
            double z1;

            double x2;
            double y2;
            double z2;

            double ctheta = Math.Cos(theta * Math.PI / 180);
            double stheta = Math.Sin(theta * Math.PI / 180);
            double cphi = Math.Cos(phi * Math.PI / 180);
            double sphi = Math.Sin(phi * Math.PI / 180);
            double cpsi = Math.Cos(psi * Math.PI / 180);
            double spsi = Math.Sin(psi * Math.PI / 180);

            double a11 = ctheta * cpsi;
            double a12 = ctheta * spsi;
            double a13 = -stheta;
            double a21 = stheta * sphi * cpsi - cphi * spsi;
            double a22 = stheta * sphi * spsi + cphi * cpsi;
            double a23 = sphi * ctheta;
            double a31 = stheta * cphi * cpsi + sphi * spsi;
            double a32 = stheta * cphi * spsi - sphi * cpsi;
            double a33 = cphi * ctheta;

            if (isUp)
            {
                for (int i = 0; i < Nc; i++)
                {
                    S = 0;
                    if (A[0].Length == 1)
                    {
                        S = A[0][0];
                        x1 = c * Psi[i];
                        y1 = 0;
                        z1 = c * Math.Pow(Psi[i], 0.7) * Math.Pow((1 - Psi[i]), 1) * S * scale + Psi[i] * Zt;

                    }
                    else
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            S = S + A[0][j] * Math.Pow(Psi[i], j);
                        }
                        x1 = c * Psi[i];
                        y1 = 0;
                        z1 = c * Math.Pow(Psi[i], 0.5) * (1 - Psi[i]) * S * scale + Psi[i] * Zt;

                    }



                    x2 = a11 * x1 + a12 * y1 + a13 * z1;
                    y2 = a21 * x1 + a22 * y1 + a23 * z1;
                    z2 = a31 * x1 + a32 * y1 + a33 * z1;

                    r[i] = catia.DrawPoint(x + x2, y + y2, z + z2);
                }
            }
            else
            {
                for (int i = 0; i < Nc; i++)
                {
                    S = 0;
                    if (A[0].Length == 1)
                    {
                        S = A[1][0];
                        x1 = c * Psi[i];
                        y1 = 0;
                        z1 = c * Math.Pow(Psi[i], 0.7) * Math.Pow((1 - Psi[i]), 1) * S * scale - Psi[i] * Zt;
                    }
                    else
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            S = S + A[1][j] * Math.Pow(Psi[i], j);
                        }
                        x1 = c * Psi[i];
                        y1 = 0;
                        z1 = c * Math.Pow(Psi[i], 0.5) * (1 - Psi[i]) * S * scale - Psi[i] * Zt;
                    }



                    x2 = a11 * x1 + a12 * y1 + a13 * z1;
                    y2 = a21 * x1 + a22 * y1 + a23 * z1;
                    z2 = a31 * x1 + a32 * y1 + a33 * z1;

                    r[i] = catia.DrawPoint(x + x2, y + y2, z + z2);
                }
            }


            return r;
        }

        #endregion

        #region Defaults
        public struct Default
        {
            public static double[][] AS3010 = new double[][]
            {
                new double[]{ 0.1748,0.7124,-5.3408,19.4486,-38.8055,42.1055,-23.055,4.8424 },
                new double[]{ -0.1135, 0.4416,-2.4782 ,10.0096,-23.3451,30.8517,-21.4175,6.0904}
            };

            public static double[][] AS5010 = new double[][]
            {
                new double[]{ 0.1508,1.0305,-9.1979,40.1532,-96.9707,129.0178,-88.9312,24.7596 },
                new double[]{ -0.0953, 0.3206,-2.5837 ,12.7844,-35.4655,54.1816,-42.3824,13.1829}
            };

            public static double[][] ANACA0009 = new double[][]
            {
                new double[]{ 0.1313,-0.2014,1.5808,-6.361,12.2112,-10.4493,2.3374,0.9155 },
                new double[]{-0.1313, 0.2014,-1.5808 ,6.361,-12.2112,10.4493,-2.3374,-0.9155 }
            };

            public static double[][] F = new double[][]
            {
                new double[]{0.12 },
                new double[]{-0.12 }
            };
        }
        #endregion
    }
}