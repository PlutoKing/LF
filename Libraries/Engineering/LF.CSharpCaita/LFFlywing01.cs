/*──────────────────────────────────────────────────────────────
 * FileName     : LFFlywing01.cs
 * Created      : 2021-06-30 21:23:47
 * Author       : Xu Zhe
 * Description  : 飞翼布局01构型：由飞翼主题和两个垂尾构成。
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using INFITF;
using System.Linq;

namespace LF.CSharpCatia
{
    /// <summary>
    /// 飞翼布局01构型
    /// </summary>
    public class LFFlywing01
    {
        #region Fields
        public LFCatia Catia = new LFCatia();

        #region 参考面
        private Reference refXY;
        private Reference refYZ;
        private Reference refZX;

        private Reference PlaneU;
        private Reference PlaneD; 
        #endregion

        #region 机翼控制参数
        /// <summary>
        /// 机翼翼型
        /// </summary>
        public LFAirfoil airfoilw;

        /// <summary>
        /// 机翼各段宽度
        /// </summary>
        public double[] bw;

        /// <summary>
        /// 机翼截面弦长
        /// </summary>
        public double[] cw;

        /// <summary>
        /// 机翼各段后掠角
        /// </summary>
        public double[] Asw;

        /// <summary>
        /// 机翼各段上反角
        /// </summary>
        public double[] Ahw;

        /// <summary>
        /// 扭转角
        /// </summary>
        public double[] thetaw;

        /// <summary>
        /// 外倾角
        /// </summary>
        public double[] phiw;

        /// <summary>
        /// 偏航角
        /// </summary>
        public double[] psiw;
        #endregion

        #region 垂尾控制参数
        /// <summary>
        /// 垂尾翼型
        /// </summary>
        public LFAirfoil airfoilv;
        /// <summary>
        /// 垂尾各段高度
        /// </summary>
        public double[] bv;

        /// <summary>
        /// 垂尾各截面弦长
        /// </summary>
        public double[] cv;

        /// <summary>
        /// 垂尾各段后掠角
        /// </summary>
        public double[] Asv;

        /// <summary>
        /// 垂尾各段外倾角
        /// </summary>
        public double[] Ahv;
        #endregion

        #region 其他控制参数
        public double zt = 0.1;

        /// <summary>
        /// 电机半径
        /// </summary>
        public double RE = 15;
        #endregion

        #region 控制点定位参数

        /// <summary>
        /// 机翼各截面前缘x坐标
        /// </summary>
        public double[] xw;

        /// <summary>
        /// 机翼各截面前缘y坐标
        /// </summary>
        public double[] yw;

        /// <summary>
        /// 机翼各截面前缘z坐标
        /// </summary>
        public double[] zw;

        /// <summary>
        /// 垂尾各截面前缘x坐标
        /// </summary>
        public double[] xv;

        /// <summary>
        /// 垂尾各截面前缘y坐标
        /// </summary>
        public double[] yv;

        /// <summary>
        /// 垂尾各截面前缘z坐标
        /// </summary>
        public double[] zv;


        public int[] LinePointX = new int[] { 18, 25, 31, 49 };
        #endregion

        #region 控制点

        Reference[][] PointWU = new Reference[6][];
        Reference[][] PointWD = new Reference[6][];

        Reference[] PointWLeL = new Reference[2];
        Reference[][] PointWUL = new Reference[2][];
        Reference[][] PointWDL = new Reference[2][];

        /// <summary>
        /// 发动机中心点
        /// </summary>
        Reference PointE;

        /// <summary>
        /// 发动机右侧点
        /// </summary>
        Reference PointERU;
        /// <summary>
        /// 发动机右侧点
        /// </summary>
        Reference PointERD;

        Reference[][] PointVU = new Reference[4][];
        Reference[][] PointVD = new Reference[4][];

        #endregion

        #region 控制线

        Reference[] AirfoilWU = new Reference[6];
        Reference[] AirfoilWD = new Reference[6];
        Reference[] AirfoilWLe = new Reference[6];
        Reference[] SectionW = new Reference[6];

        Reference[] AirfoilWUL = new Reference[2];
        Reference[] AirfoilWDL = new Reference[2];
        Reference[] SectionWL = new Reference[2];

        Reference LineW23Le;
        Reference[] LineW23U = new Reference[4];
        Reference[] LineW23D = new Reference[4];

        Reference LineW32Le;
        Reference[] LineW32U = new Reference[4];
        Reference[] LineW32D = new Reference[4];

        Reference LineW34Le;
        Reference[] LineW34U = new Reference[4];
        Reference[] LineW34D = new Reference[4];

        Reference LineW45Le;
        Reference[] LineW45U = new Reference[4];
        Reference[] LineW45D = new Reference[4];


        Reference LineW11Le;
        Reference[] LineW11U = new Reference[4];
        Reference[] LineW11D = new Reference[4];

        /// <summary>
        /// 发动机座上下半圆
        /// </summary>
        Reference CircleEU;
        /// <summary>
        /// 发动机座上下半圆
        /// </summary>
        Reference CircleED;
        /// <summary>
        /// 机身后缘控制线
        /// </summary>
        Reference LineW01U;
        /// <summary>
        /// 机身后缘控制线
        /// </summary>
        Reference LineW01D;

        /// <summary>
        /// 中翼段后缘控制线
        /// </summary>
        Reference LineW12U;
        /// <summary>
        /// 中翼段后缘控制线
        /// </summary>
        Reference LineW12D;

        Reference CornerRU;
        Reference CornerRD;

        /// <summary>
        /// 机身后缘右侧直线
        /// </summary>
        Reference LineFRLeU;
        Reference LineFRLeD;

        Reference[] AirfoilVU = new Reference[4];
        Reference[] AirfoilVD = new Reference[4];
        Reference[] AirfoilVLe = new Reference[4];
        Reference[] SectionV = new Reference[4];

        Reference[] LineVLe = new Reference[3];
        Reference[][] LineVU = new Reference[3][];
        Reference[][] LineVD = new Reference[3][];

        #endregion

        #region 翼面
        Reference SurfaceFU;
        Reference SurfaceFD;
        Reference SurfaceFR;
        Reference SurfaceFL;
        Reference SurfaceWR;
        Reference SurfaceWL;
        Reference SurfaceTipR;
        Reference SurfaceTipL;

        Reference SurfaceE;
        Reference SurfaceFLeR;
        Reference SurfaceFRLeR;
        Reference SurfaceWLeR;
        Reference SurfaceTipLeR;
        Reference SurfaceFLeL;
        Reference SurfaceFLLeL;
        Reference SurfaceWLeL;
        Reference SurfaceTipLeL;
        Reference SufraceTipFillR;
        Reference SufraceTipFillL;

        Reference SurfaceV01;
        Reference SurfaceV12;
        Reference SurfaceV23;

        Reference SurfaceV00;
        Reference SurfaceV33;

        Reference SurfaceVR;
        Reference SurfaceVL;
        #endregion
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFFlywing01()
        {
        }
        #endregion

        #region Methods

        public void Draw()
        {
            Init();

            /* 参考面 */
            refXY = Catia.SetPlaneXY();
            refYZ = Catia.SetPlaneYZ();
            refZX = Catia.SetPlaneZX();
            PlaneU = Catia.SetPlane(refXY, zt);
            PlaneD = Catia.SetPlane(refXY, -zt);

            airfoilw.Zt = RE;
            PointWU[0] = airfoilw.Draw(Catia, cw[0], xw[0], yw[0], zw[0], thetaw[0], phiw[0], psiw[0], 1.5, true);
            AirfoilWU[0] = Catia.DrawSpline(PointWU[0]);
            PointWD[0] = airfoilw.Draw(Catia, cw[0], xw[0], yw[0], zw[0], thetaw[0], phiw[0], psiw[0], 2.5, false);
            AirfoilWD[0] = Catia.DrawSpline(PointWD[0]);
            //AirfoilWLe[0] = Catia.DrawLine(PointWU[0].Last<Reference>(), PointWD[0].Last<Reference>());

            //SectionW[0] = Catia.Join(new Reference[] { AirfoilWU[0], AirfoilWD[0]});

            airfoilw.Zt = zt;

            PointWU[1] = airfoilw.Draw(Catia, cw[1], xw[1], yw[1], zw[1], thetaw[1], phiw[1], psiw[1], 1.2, true);
            AirfoilWU[1] = Catia.DrawSpline(PointWU[1]);
            PointWD[1] = airfoilw.Draw(Catia, cw[1], xw[1], yw[1], zw[1], thetaw[1], phiw[1], psiw[1], 1.2, false);
            AirfoilWD[1] = Catia.DrawSpline(PointWD[1]);
            AirfoilWLe[1] = Catia.DrawLine(PointWU[1].Last<Reference>(), PointWD[1].Last<Reference>());
            SectionW[1] = Catia.Join(new Reference[] { AirfoilWU[1], AirfoilWD[1] });

            for (int i = 2; i < 6; i++)
            {
                PointWU[i] = airfoilw.Draw(Catia, cw[i], xw[i], yw[i], zw[i], thetaw[i], phiw[i], psiw[i], 1, true);
                AirfoilWU[i] = Catia.DrawSpline(PointWU[i]);
                PointWD[i] = airfoilw.Draw(Catia, cw[i], xw[i], yw[i], zw[i], thetaw[i], phiw[i], psiw[i], 1, false);
                AirfoilWD[i] = Catia.DrawSpline(PointWD[i]);
                AirfoilWLe[i] = Catia.DrawLine(PointWU[i].Last<Reference>(), PointWD[i].Last<Reference>());

                SectionW[i] = Catia.Join(new Reference[] { AirfoilWU[i], AirfoilWD[i] });
            }

            for (int i = 0; i < 4; i++)
            {
                PointVU[i] = airfoilv.Draw(Catia, cv[i], xv[i], yv[i], zv[i], 0, -90, 0, 1, true);
                PointVD[i] = airfoilv.Draw(Catia, cv[i], xv[i], yv[i], zv[i], 0, -90, 0, 1, false);
                AirfoilVU[i] = Catia.DrawSpline(PointVU[i]);
                AirfoilVD[i] = Catia.DrawSpline(PointVD[i]);
                AirfoilVLe[i] = Catia.DrawLine(PointVU[i].Last<Reference>(), PointVD[i].Last<Reference>());
                SectionV[i] = Catia.Join(new Reference[] { AirfoilVU[i], AirfoilVD[i], AirfoilVLe[i] });
            }

            AirfoilWUL[0] = Catia.GetSymmetry(AirfoilWU[1], refZX);
            AirfoilWDL[0] = Catia.GetSymmetry(AirfoilWD[1], refZX);
            for (int i = 0; i < 2; i++)
            {
                //AirfoilWUL[i] = Catia.GetSymmetry(AirfoilWU[i + 1], refZX);
                //AirfoilWDL[i] = Catia.GetSymmetry(AirfoilWD[i + 1], refZX);
                //SectionWL[i] = Catia.GetSymmetry(SectionW[i + 1], refZX);
                PointWLeL[i] = Catia.GetSymmetry(PointWU[i + 1][0], refZX);
                PointWUL[i] = new Reference[4];
                PointWDL[i] = new Reference[4];

                for (int j = 0; j < 3; j++)
                {
                    PointWUL[i][j] = Catia.GetSymmetry(PointWU[i + 1][LinePointX[j]], refZX);
                    PointWDL[i][j] = Catia.GetSymmetry(PointWD[i + 1][LinePointX[j]], refZX);
                }
            }

            LineW23Le = Catia.DrawLine(PointWU[2][0], PointWU[3][0]);
            LineW45Le = Catia.DrawLine(PointWU[4][0], PointWU[5][0]);
            LineW32Le = Catia.GetSymmetry(LineW23Le, refZX);
            LineW11Le = Catia.DrawSpline(new Reference[] { PointWU[2][0], PointWU[1][0], PointWU[0][0], PointWLeL[0], PointWLeL[1] }, LineW23Le, -1, LineW32Le, 1);
            LineW34Le = Catia.DrawSpline(new Reference[] { PointWU[3][0], PointWU[4][0] }, LineW23Le, 1, LineW45Le, 1);
            for (int i = 0; i < 4; i++)
            {
                LineW23U[i] = Catia.DrawLine(PointWU[2][LinePointX[i]], PointWU[3][LinePointX[i]]);
                LineW45U[i] = Catia.DrawLine(PointWU[4][LinePointX[i]], PointWU[5][LinePointX[i]]);
                LineW23D[i] = Catia.DrawLine(PointWD[2][LinePointX[i]], PointWD[3][LinePointX[i]]);
                LineW45D[i] = Catia.DrawLine(PointWD[4][LinePointX[i]], PointWD[5][LinePointX[i]]);
                LineW32U[i] = Catia.GetSymmetry(LineW23U[i], refZX);
                LineW32D[i] = Catia.GetSymmetry(LineW23D[i], refZX);
                LineW34U[i] = Catia.DrawSpline(new Reference[] { PointWU[3][LinePointX[i]], PointWU[4][LinePointX[i]] }, LineW23U[i], 1, LineW45U[i], 1);
                LineW34D[i] = Catia.DrawSpline(new Reference[] { PointWD[3][LinePointX[i]], PointWD[4][LinePointX[i]] }, LineW23D[i], 1, LineW45D[i], 1);

            }
            Catia.Shapes.GSMVisibility(LineW32U[3], 0);
            Catia.Shapes.GSMVisibility(LineW32D[3], 0);
            Catia.Shapes.GSMVisibility(SectionW[5], 0);
            Catia.Shapes.GSMVisibility(AirfoilWLe[5], 0);
            //Catia.Shapes.GSMVisibility(AirfoilWLe[5], 0);

            for (int i = 0; i < 3; i++)
            {
                LineW11U[i] = Catia.DrawSpline(new Reference[] { PointWU[2][LinePointX[i]], PointWU[1][LinePointX[i]], PointWU[0][LinePointX[i]], PointWUL[0][i], PointWUL[1][i] }, LineW23U[i], -1, LineW32U[i], 1);
                LineW11D[i] = Catia.DrawSpline(new Reference[] { PointWD[2][LinePointX[i]], PointWD[1][LinePointX[i]], PointWD[0][LinePointX[i]], PointWDL[0][i], PointWDL[1][i] }, LineW23D[i], -1, LineW32D[i], 1);
            }

            PointE = Catia.DrawCenterPoint(PointWU[0].Last<Reference>(), PointWD[0].Last<Reference>());

            //PointER = Catia.DrawPoint(PointE, 0, RE, 0);

            CircleEU = Catia.DrawCircle(refYZ, PointE, RE, 0, 180);
            CircleED = Catia.DrawCircle(refYZ, PointE, RE, 180, 360);

            PointERU = Catia.GetIntersection(CircleEU, PlaneU, PointWU[1].Last<Reference>());
            PointERD = Catia.GetIntersection(CircleED, PlaneD, PointWD[1].Last<Reference>());

            LineW01U = Catia.DrawLine(PointERU, PointWU[1].Last<Reference>());
            LineW01D = Catia.DrawLine(PointERD, PointWD[1].Last<Reference>());

            LineW12U = Catia.DrawSpline(new Reference[] { PointWU[1].Last<Reference>(), PointWU[2].Last<Reference>() }, LineW01U, 1, LineW23U[3], 1);
            LineW12D = Catia.DrawSpline(new Reference[] { PointWD[1].Last<Reference>(), PointWD[2].Last<Reference>() }, LineW01D, 1, LineW23D[3], 1);

            CornerRU = Catia.GetCorner(LineW01U, 1, CircleEU, -1, 10, 2, 1, -1);
            CornerRD = Catia.GetCorner(LineW01D, -1, CircleED, -1, 10, 1, -1, -1);

            LineW01U = Catia.Cut(LineW01U, CornerRU, -1);
            LineW01D = Catia.Cut(LineW01D, CornerRD, -1);

            LineFRLeU = Catia.Cut(CircleEU, CornerRU, 1);
            LineFRLeD = Catia.Cut(CircleED, CornerRD, -1);

            LineW11U[3] = Catia.Cut(CircleEU, CornerRU, -1);
            LineW11U[3] = Catia.Cut(LineW11U[3], refZX, -1);
            LineW11U[3] = Catia.Join(new Reference[] { LineW11U[3], CornerRU, LineW01U, LineW12U });
            LineW11U[3] = Catia.Join(new Reference[] { LineW11U[3], Catia.GetSymmetry(LineW11U[3], refZX) });

            LineW11D[3] = Catia.Cut(CircleED, CornerRD, 1);
            LineW11D[3] = Catia.Cut(LineW11D[3], refZX, -1);
            LineW11D[3] = Catia.Join(new Reference[] { LineW11D[3], CornerRD, LineW01D, LineW12D });
            LineW11D[3] = Catia.Join(new Reference[] { LineW11D[3], Catia.GetSymmetry(LineW11D[3], refZX) });

            for (int i = 0; i < 2; i++)
            {
                LineVLe[i] = Catia.DrawLine(PointVU[i][0], PointVU[i + 1][0]);
                LineVU[i] = new Reference[4];
                LineVD[i] = new Reference[4];
                for (int j = 0; j < 4; j++)
                {
                    LineVU[i][j] = Catia.DrawLine(PointVU[i][LinePointX[j]], PointVU[i + 1][LinePointX[j]]);
                    LineVD[i][j] = Catia.DrawLine(PointVD[i][LinePointX[j]], PointVD[i + 1][LinePointX[j]]);
                }
            }

            LineVLe[2] = Catia.DrawSpline(new Reference[] { PointVU[2][0], PointVU[2 + 1][0] }, LineVLe[1], 1);
            LineVU[2] = new Reference[4];
            LineVD[2] = new Reference[4];
            for (int j = 0; j < 4; j++)
            {
                LineVU[2][j] = Catia.DrawSpline(new Reference[] { PointVU[2][LinePointX[j]], PointVU[2 + 1][LinePointX[j]] }, LineVU[1][j], 1);
                LineVD[2][j] = Catia.DrawSpline(new Reference[] { PointVD[2][LinePointX[j]], PointVD[2 + 1][LinePointX[j]] }, LineVD[1][j], 1);
            }

            SurfaceFU = Catia.DrawLoft(new Reference[] { AirfoilWU[1], AirfoilWU[0], AirfoilWUL[0] }, LineW11Le, LineW11U);
            SurfaceFD = Catia.DrawLoft(new Reference[] { AirfoilWD[1], AirfoilWD[0], AirfoilWDL[0] }, LineW11Le, LineW11D);
            SurfaceFR = Catia.DrawLoft(new Reference[] { SectionW[1], SectionW[2] }, LineW11Le, LineW11U, LineW11D);
            SurfaceWR = Catia.DrawLoft(new Reference[] { SectionW[2], SectionW[3] }, LineW23Le, LineW23U, LineW23D);
            SurfaceTipR = Catia.DrawLoft(new Reference[] { SectionW[3], SectionW[4] }, LineW34Le, LineW34U, LineW34D);
            SurfaceFL = Catia.GetSymmetry(SurfaceFR, refZX);
            SurfaceWL = Catia.GetSymmetry(SurfaceWR, refZX);
            SurfaceTipL = Catia.GetSymmetry(SurfaceTipR, refZX);

            SurfaceE = Catia.Fill(new Reference[] { CircleEU, CircleED });
            SurfaceFLeR = Catia.Fill(new Reference[] { LineFRLeU, CornerRU, LineW01U, AirfoilWLe[1], LineW01D, CornerRD, LineFRLeD });
            SurfaceFRLeR = Catia.Fill(new Reference[] { AirfoilWLe[1], LineW12U, AirfoilWLe[2], LineW12D });
            SurfaceWLeR = Catia.Fill(new Reference[] { AirfoilWLe[2], LineW23U[3], AirfoilWLe[3], LineW23D[3] });
            SurfaceTipLeR = Catia.Fill(new Reference[] { AirfoilWLe[3], LineW34U[3], AirfoilWLe[4], LineW34D[3] });
            // SufraceTipFillR = Catia.Blend(SurfaceTipR, AirfoilWU[4], -1, SurfaceTipR, AirfoilWD[4], 1) ;  
            SufraceTipFillR = Catia.Fill(new Reference[] { AirfoilWU[4], AirfoilWLe[4], AirfoilWD[4] });

            SurfaceFLeL = Catia.GetSymmetry(SurfaceFLeR, refZX);
            SurfaceFLLeL = Catia.GetSymmetry(SurfaceFRLeR, refZX);
            SurfaceWLeL = Catia.GetSymmetry(SurfaceWLeR, refZX);
            SurfaceTipLeL = Catia.GetSymmetry(SurfaceTipLeR, refZX);
            SufraceTipFillL = Catia.GetSymmetry(SufraceTipFillR, refZX);

            SurfaceV01 = Catia.DrawLoft(new Reference[] { SectionV[0], SectionV[1] }, LineVLe[0], LineVU[0], LineVD[0]);
            SurfaceV12 = Catia.DrawLoft(new Reference[] { SectionV[1], SectionV[2] }, LineVLe[1], LineVU[1], LineVD[1]);
            SurfaceV23 = Catia.DrawLoft(new Reference[] { SectionV[2], SectionV[3] }, LineVLe[2], LineVU[2], LineVD[2]);

            //SurfaceV00 = Catia.Blend(SurfaceV01, AirfoilVU[0], 1, SurfaceV01, AirfoilVD[0], -1);
            //SurfaceV33 = Catia.Blend(SurfaceV23, AirfoilVU[3], -1, SurfaceV23, AirfoilVD[3], 1);
            SurfaceV00 = Catia.Fill(SectionV[0]);
            SurfaceV33 = Catia.Fill(SectionV[3]);
            SurfaceVR = Catia.Join(new Reference[] { SurfaceV00, SurfaceV01, SurfaceV12, SurfaceV23, SurfaceV33 });
            SurfaceVL = Catia.GetSymmetry(SurfaceVR, refZX);

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 翼型初始化
            airfoilw = new LFAirfoil("S3010");
            airfoilv = new LFAirfoil("NACA0009");
            airfoilw.Zt = 0.1;
            airfoilv.Zt = 0.1;

            /* 机翼 */
            bw = new double[5] { 50, 50, 200, 0, 0 };
            cw = new double[6] { 360, 280, 200, 100, 30, 20 };
            Asw = new double[7] { 50, 50, 35, 60, 80, 24, 38 };
            Ahw = new double[5] { 0, 0, 2, 15, 30 };

            bw[3] = (cw[3] - cw[4]) / ((Math.Tan(Asw[3] * Math.PI / 180)) - (Math.Tan(Asw[5] * Math.PI / 180)));
            bw[4] = (cw[4] - cw[5]) / ((Math.Tan(Asw[4] * Math.PI / 180)) - (Math.Tan(Asw[6] * Math.PI / 180)));

            xw = new double[6];
            xw[0] = 0; //-cw[0] * 0.5
            for (int i = 1; i < 6; i++)
            {
                xw[i] = xw[i - 1] + bw[i - 1] * Math.Tan(Asw[i - 1] * Math.PI / 180);
            }

            yw = new double[6];
            yw[0] = 0;
            for (int i = 1; i < 6; i++)
            {
                yw[i] = yw[i - 1] + bw[i - 1];
            }

            zw = new double[6];
            zw[0] = 0;
            for (int i = 1; i < 6; i++)
            {
                zw[i] = zw[i - 1] + bw[i - 1] * Math.Tan(Ahw[i - 1] * Math.PI / 180);
            }

            thetaw = new double[6];
            phiw = new double[6];
            psiw = new double[6];

            phiw[4] = -30;
            phiw[5] = -30;

            /* 垂尾 */
            bv = new double[3] { 10, 80, 12 };
            cv = new double[4] { 100, 100, 60, 48.101 };
            Asv = new double[3] { -35, 35, 50 };
            Ahv = new double[3] { 0, 0, 0 };

            xv = new double[4];
            xv[0] = xw[2] + cv[0] + cv[0] / 3;
            for (int i = 1; i < 4; i++)
            {
                xv[i] = xv[i - 1] + bv[i - 1] * Math.Tan(Asv[i - 1] * Math.PI / 180);
            }

            zv = new double[4];
            zv[0] = -bv[0];
            for (int i = 1; i < 4; i++)
            {
                zv[i] = zv[i - 1] + bv[i - 1];
            }

            yv = new double[4];
            yv[0] = yw[2];
            for (int i = 1; i < 4; i++)
            {
                yv[i] = yv[i - 1] + bv[i - 1] * Math.Tan(Ahv[i - 1] * Math.PI / 180);
            }
        }
        #endregion
    }
}