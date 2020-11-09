/*──────────────────────────────────────────────────────────────
 * FileName     : LFAirplane
 * Created      : 2020-10-31 15:58:08
 * Author       : Xu Zhe
 * Description  : 固定翼飞机
 * ──────────────────────────────────────────────────────────────*/

using System;
using LF.Mathematics;
using LF.Mechanics;

namespace LF.Aircraft
{
    /// <summary>
    /// 固定翼飞机
    /// </summary>
    public class LFAirplane:IAircraft
    {
        #region Fields

        private int _id;

        private double _wingArea;       // 机翼面积
        private double _wingSpan;       // 机翼展长
        private double _wingMac;        // 机翼平均气动弦长

        private LFAtmospheric _air;     // 大气参数
        private LFAerodynamics _aero;   // 气动参数

        private LFBody _body;           // 刚体运动模型

        private LFPropeller _propeller; // 螺旋桨
        #endregion

        #region Properties

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get => _id; set => _id = value; }


        /// <summary>
        /// 机翼面积
        /// </summary>
        public double WingArea { get => _wingArea; set => _wingArea = value; }

        /// <summary>
        /// 翼展
        /// </summary>
        public double WingSpan { get => _wingSpan; set => _wingSpan = value; }

        /// <summary>
        /// 机翼平均气动弦长
        /// </summary>
        public double WingMac { get => _wingMac; set => _wingMac = value; }

        /// <summary>
        /// 气动参数
        /// </summary>
        public LFAerodynamics Aerodynamics { get => _aero; set => _aero = value; }

        /// <summary>
        /// 大气参数
        /// </summary>
        public LFAtmospheric Air { get => _air; set => _air = value; }

        /// <summary>
        /// 刚体模型
        /// </summary>
        public LFBody Body { get => _body; set => _body = value; }

        /// <summary>
        /// 螺旋桨
        /// </summary>
        public LFPropeller Propeller { get => _propeller; set => _propeller = value; }

        #endregion

        #region Constructors
        public LFAirplane()
        {

        }

        #endregion

        #region Methods

        #region 运动模型

        /// <summary>
        /// 操纵函数
        /// </summary>
        /// <param name="u">操纵量 4维
        /// ch1通道 副翼
        /// ch2通道 升降舵
        /// ch3通道 油门
        /// ch4通道 方向舵</param>
        public void Operate(LFVector u, LFVector3 Vw, double dt)
        {
            /* 预处理 */
            WindModel(Vw);         // 风速模型：迎角、侧滑角、空速计算
            _air.Refresh(_body.Position.Z);     // 大气模型：更新大气密度、重力加速度

            /* 计算气动力 */
            _aero.ForceAndMoment(_air.Density, _wingArea, _wingSpan, _wingMac, _body.AngularVelocity, u);
            LFMatrix Lba = Coordinate.Lab(_aero.Alpha, _aero.Beta).Transpose();
            LFVector3 Fa_b = Lba * _aero.Force;

            /* 计算气动力 */
            LFVector3 Fg_g = new LFVector3(0, 0, -_body.Mass * _air.Gravity);
            LFMatrix Lbg = Coordinate.Lbg(_body.Attitude);
            LFVector3 Fg_b = Lbg * Fg_g;

            /* 计算螺旋力 */
            _propeller.ForceAndMoment(_air.Density, u[3]);

            /* 合力计算 */
            _body.Force = Fa_b + Fg_b + _propeller.Force;
            _body.Moment = _aero.Moment + _propeller.Moment;

            /* 运动方程 */
            _body.Kinetics(dt);     // 动力学方程
            _body.Kinematics(dt);   // 运动学方程
        }

        /// <summary>
        /// 风速模型
        /// </summary>
        /// <param name="Vw">风速</param>
        public void WindModel(LFVector3 Vw)
        {
            LFMatrix lba = Coordinate.Lab(_aero.Alpha, _aero.Beta).Transpose();
            LFVector3 tmp = lba * Vw;

            double du = _body.Velocity.X - tmp.X;
            double dv = _body.Velocity.Y - tmp.Y;
            double dw = _body.Velocity.Z - tmp.Z;


            if(du == 0)
            {
                _aero.Alpha = 0;
                
            }
            else
            {
                _aero.Alpha = Math.Atan2(dw, du);
            }

            if (dw == 0 && du == 0)
            {
                _aero.Beta = 0;
            }
            else
            {
                _aero.Beta = Math.Atan2(dv, Math.Pow(du * du + dw * dw, 0.5));

            }

            _aero.Va = Math.Pow(du * du + dv * dv + dw * dw, 0.5);
        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
