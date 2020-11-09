/*──────────────────────────────────────────────────────────────
 * FileName     : LFAerodynamics
 * Created      : 2020-10-31 16:02:46
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

namespace LF.Aircraft
{
    /// <summary>
    /// 空气动力学
    /// </summary>
    public class LFAerodynamics
    {
        #region Fields

        private LngCoefficient _cL;
        private LngCoefficient _cD;
        private LngCoefficient _cm;

        private LatCoefficient _cC;
        private LatCoefficient _cl;
        private LatCoefficient _cn;

        private double _alpha;
        private double _beta;
        private double _va;

        private LFVector3 _force;
        private LFVector3 _moment;
        
        #endregion

        #region Properties
        /// <summary>
        /// 升力系数
        /// </summary>
        public LngCoefficient CL { get => _cL; set => _cL = value; }

        /// <summary>
        /// 阻力系数
        /// </summary>
        public LngCoefficient CD { get => _cD; set => _cD = value; }

        /// <summary>
        /// 俯仰力矩系数
        /// </summary>
        public LngCoefficient Cm { get => _cm; set => _cm = value; }

        /// <summary>
        /// 侧滑力系数
        /// </summary>
        public LatCoefficient CC { get => _cC; set => _cC = value; }

        /// <summary>
        /// 滚转力矩系数
        /// </summary>
        public LatCoefficient Cl { get => _cl; set => _cl = value; }

        /// <summary>
        /// 偏航力矩系数
        /// /// </summary>
        public LatCoefficient Cn { get => _cn; set => _cn = value; }
        
        /// <summary>
        /// 迎角
        /// </summary>
        public double Alpha { get => _alpha; set => _alpha = value; }
        
        /// <summary>
        /// 侧滑角
        /// </summary>
        public double Beta { get => _beta; set => _beta = value; }
        
        /// <summary>
        /// 空速
        /// </summary>
        public double Va { get => _va; set => _va = value; }

        /// <summary>
        /// 气动力
        /// </summary>
        public LFVector3 Force { get => _force; set => _force = value; }
        
        /// <summary>
        /// 气动力矩
        /// </summary>
        public LFVector3 Moment { get => _moment; set => _moment = value; }



        #endregion

        #region Constructors
        public LFAerodynamics()
        {

        }
        #endregion

        #region Methods

        #region 气动导数计算



        #endregion

        #region 气动力计算
        public void ForceAndMoment(double rho, double S, double b, double c, LFVector3 omg, LFVector u)
        {
            // 角速度
            double p = omg.X;
            double q = omg.Y;
            double r = omg.Z;

            // 操纵量
            double da = u[0];
            double de = u[1];
            double dr = u[3];

            // 无量纲化
            double pbar = p * b / (2 * Va);
            double qbar = q * c / (2 * Va);
            double rbar = r * b / (2 * Va);

            // 动压
            double pre = Pressure(rho, Va);

            // 阻力
            _force.X = -pre * S * _cD.GetValue(_alpha, qbar, de);
            // 侧力
            _force.Y = pre * S * _cC.GetValue(_beta, pbar, rbar, da, dr);
            // 升力
            _force.Z = -pre * S * _cL.GetValue(_alpha, qbar, de);
            // 滚转力矩
            _moment.X = pre * S * b * _cl.GetValue(_beta, pbar, rbar, da, dr);
            // 俯仰力矩
            _moment.Y = pre * S * c * _cm.GetValue(_alpha, qbar, de);
            // 偏航力矩
            _moment.Z = pre * S * b * _cn.GetValue(_beta, pbar, rbar, da, dr);
        }



        /// <summary>
        /// 伯努利原理：计算动压
        /// </summary>
        /// <param name="rho">密度</param>
        /// <param name="V">速度</param>
        /// <returns></returns>
        public static double Pressure(double rho, double V)
        {
            return 0.5 * rho * V * V;
        }

        #endregion
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }

    /// <summary>
    /// 纵向气动系数
    /// </summary>
    public struct LngCoefficient
    {
        /// <summary>
        /// 零升力系数
        /// </summary>
        public double Zero;
        /// <summary>
        /// 迎角导数
        /// </summary>
        public double Alpha;
        /// <summary>
        /// 俯仰角速度导数
        /// </summary>
        public double Q;
        /// <summary>
        /// 升降舵偏角导数
        /// </summary>
        public double Elevator;

        /// <summary>
        /// 气动系数
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="de"></param>
        /// <returns></returns>
        public double GetValue(double alpha, double q, double de)
        {
            return Zero + Alpha * alpha + Q * q + Elevator * de;
        }
    }

    /// <summary>
    /// 横航向气动导数
    /// </summary>
    public struct LatCoefficient
    {
        /// <summary>
        /// 侧滑角导数
        /// </summary>
        public double Beta;
        /// <summary>
        /// 滚转角速度导数
        /// </summary>
        public double P;
        /// <summary>
        /// 偏航角速度导数
        /// </summary>
        public double R;
        /// <summary>
        /// 副翼偏角导数
        /// </summary>
        public double Aileron;
        /// <summary>
        /// 方向舵偏角导数
        /// </summary>
        public double Rudder;

        /// <summary>
        /// 气动系数
        /// </summary>
        /// <param name="beta">侧滑角</param>
        /// <param name="p">滚转角速度</param>
        /// <param name="r">偏航角速度</param>
        /// <param name="da">副翼偏角</param>
        /// <param name="dr">方向舵偏角</param>
        /// <returns></returns>
        public double GetValue(double beta, double p,double r,double da, double dr)
        {
            return beta * Beta + p * P + r * R + da * Aileron + dr * Rudder;
        }
    }
}
