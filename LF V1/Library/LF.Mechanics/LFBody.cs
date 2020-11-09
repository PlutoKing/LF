/*──────────────────────────────────────────────────────────────
 * FileName     : LFBody
 * Created      : 2020-10-20 19:26:00
 * Author       : Xu Zhe
 * Description  : 刚体
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
    /// <summary>
    /// 刚体
    /// </summary>
    public class LFBody
    {
        #region Fields

        private double _mass;                               // 质量
        private LFMatrix _inertia = new LFMatrix(3,3);      // 惯量

        private LFVector3 _force = new LFVector3();         // 受力
        private LFVector3 _moment = new LFVector3();        // 力矩

        private LFVector3 _pos = new LFVector3();           // 位置
        private LFVector3 _att = new LFVector3();           // 姿态
        private LFVector3 _vel = new LFVector3();           // 速度
        private LFVector3 _omg = new LFVector3();           // 角速度
        #endregion

        #region Properties

        /// <summary>
        /// 质量
        /// </summary>
        public double Mass { get => _mass; set => _mass = value; }

        /// <summary>
        /// 转动惯量
        /// </summary>
        public LFMatrix Inertia { get => _inertia; set => _inertia = value; }

        /// <summary>
        /// 外力
        /// </summary>
        public LFVector3 Force { get => _force; set => _force = value; }

        /// <summary>
        /// 力矩
        /// </summary>
        public LFVector3 Moment { get => _moment; set => _moment = value; }

        /// <summary>
        /// 位置：地面坐标系
        /// </summary>
        public LFVector3 Position { get => _pos; set => _pos = value; }
        
        /// <summary>
        /// 姿态：机体相对于地面
        /// </summary>
        public LFVector3 Attitude { get => _att; set => _att = value; }

        /// <summary>
        /// 速度：机体坐标系
        /// </summary>
        public LFVector3 Velocity { get => _vel; set => _vel = value; }

        /// <summary>
        /// 角速度
        /// </summary>
        public LFVector3 AngularVelocity { get => _omg; set => _omg = value; }

        #endregion

        #region Constructors
        public LFBody()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// 动力学方程：更新速度和角速度
        /// </summary>
        /// <param name="F">受力</param>
        /// <param name="M">力矩</param>
        public void Kinetics(double dt)
        {
            /* 计算速度 */
            LFVector3 acc = _force * (1 / _mass) - new LFVector3(LFVector.Cross(_omg, _vel));
            _vel += acc * dt;

            /* 计算角速度 */
            LFVector3 h = _inertia * _omg;

            LFVector3 hdot = _moment - new LFVector3(LFVector.Cross(_omg, h));

            LFVector3 omgAcc = new LFVector3(_inertia / hdot);

            _omg += omgAcc * dt;

            
        }

        /// <summary>
        /// 运动学方程：更新位置和姿态
        /// </summary>
        /// <param name="dt">时间步长</param>
        public void Kinematics(double dt)
        {
            double sinRoll = Math.Sin(Attitude.X * Math.PI / 180);
            double sinPitch = Math.Sin(Attitude.Y * Math.PI / 180);
            double sinYaw = Math.Sin(Attitude.Z * Math.PI / 180);
            double cosRoll = Math.Cos(Attitude.X * Math.PI / 180);
            double cosPitch = Math.Cos(Attitude.Y * Math.PI / 180);
            double cosYaw = Math.Cos(Attitude.Z * Math.PI / 180);
            double tanPitch = Math.Tan(Attitude.Y * Math.PI / 180);

            // 速度
            double Vx = cosPitch * cosYaw * Velocity.X + (sinPitch * sinRoll * cosYaw - cosRoll * sinYaw) * Velocity.Y + (sinPitch * cosRoll * cosYaw + sinRoll * sinYaw) * Velocity.Z;
            double Vy = cosPitch * sinYaw * Velocity.X + (sinPitch * sinRoll * sinYaw + cosRoll * cosYaw) * Velocity.Y + (sinPitch * cosRoll * sinYaw - sinRoll * cosYaw) * Velocity.Z;
            double Vz = -1 * sinPitch * Velocity.X + sinRoll * cosPitch * Velocity.Y + cosRoll * cosPitch * Velocity.Z;

            // 质心运动运动学方程
            Position.X += Vx * dt;
            Position.Y += Vy * dt;
            Position.Z += Vz * dt;

            // 绕质心转动运动学方程
            Attitude.X += (AngularVelocity.Y + tanPitch * (AngularVelocity.Y * sinRoll + AngularVelocity.Z * cosRoll)) * dt;
            Attitude.Y += (AngularVelocity.Y * cosRoll - AngularVelocity.Z * sinRoll) * dt;
            Attitude.Z += (1 / cosPitch * (AngularVelocity.Y * sinRoll + AngularVelocity.Z * cosRoll)) * dt;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
