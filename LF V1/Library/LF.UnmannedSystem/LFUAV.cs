/*──────────────────────────────────────────────────────────────
 * FileName     : LFUav
 * Created      : 2020-11-01 12:11:43
 * Author       : Xu Zhe
 * Description  : 无人机系统
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.FlightControl;
using LF.Aircraft;
using LF.TaskAllocation;
using System.Timers;
using LF.Mechanics;

namespace LF.UnmannedSystem
{
    /// <summary>
    /// 无人机
    /// </summary>
    public class LFUav
    {
        #region Fields
        private int _id;

        /* 仿真变量 */
        public Timer SimTimer = new Timer();        // 仿真计时器
        private double _simStep = 0.01d;            // 仿真步长

        /* 系统组成 */
        private LFAutopilot _autopilot = new LFAutopilot(); // 自动驾驶仪
        private IAircraft _aircraft;                        // 飞行器平台
        private LFAgent _agent = new LFAgent();             // 任务分配代理
                                                            // 缺通信模块、载荷模块

        #endregion

        #region Properties

        #region 运动信息
       
        /// <summary>
        /// 位置坐标X
        /// </summary>
        public double X
        {
            get { return _aircraft.Body.Position.X; }
            set { _aircraft.Body.Position.X = value; }
        }

        /// <summary>
        /// 位置坐标Y
        /// </summary>
        public double Y
        {
            get { return _aircraft.Body.Position.Y; }
            set { _aircraft.Body.Position.Y = value; }
        }

        /// <summary>
        /// 位置坐标Z
        /// </summary>
        public double Z
        {
            get { return _aircraft.Body.Position.Z; }
            set { _aircraft.Body.Position.Z = value; }
        }

        /// <summary>
        /// 滚转角φ
        /// </summary>
        public double Roll
        {
            get { return _aircraft.Body.Attitude.X; }
            set { _aircraft.Body.Attitude.X = value; }
        }

        /// <summary>
        /// 俯仰角θ
        /// </summary>
        public double Pitch
        {
            get { return _aircraft.Body.Attitude.Y; }
            set { _aircraft.Body.Attitude.Y = value; }
        }

        /// <summary>
        /// 偏航角ψ
        /// </summary>
        public double Yaw
        {
            get { return _aircraft.Body.Attitude.Z; }
            set { _aircraft.Body.Attitude.Z = value; }
        }

        /// <summary>
        /// X轴速度U
        /// </summary>
        public double U
        {
            get { return _aircraft.Body.Velocity.X; }
            set { _aircraft.Body.Velocity.X = value; }
        }

        /// <summary>
        /// Y轴速度V
        /// </summary>
        public double V
        {
            get { return _aircraft.Body.Velocity.Y; }
            set { _aircraft.Body.Velocity.Y = value; }
        }


        /// <summary>
        /// Z轴速度W
        /// </summary>
        public double W
        {
            get { return _aircraft.Body.Velocity.Z; }
            set { _aircraft.Body.Velocity.Z = value; }
        }

        /// <summary>
        /// 绕X轴角速度P
        /// </summary>
        public double P
        {
            get { return _aircraft.Body.AngularVelocity.X; }
            set { _aircraft.Body.AngularVelocity.X = value; }
        }

        /// <summary>
        /// 绕Y轴角速度Q
        /// </summary>
        public double Q
        {
            get { return _aircraft.Body.AngularVelocity.Y; }
            set { _aircraft.Body.AngularVelocity.Y = value; }
        }

        /// <summary>
        /// 绕Z轴角速度R
        /// </summary>
        public double R
        {
            get { return _aircraft.Body.AngularVelocity.Z; }
            set { _aircraft.Body.AngularVelocity.Z = value; }
        }
        #endregion

        #endregion

        #region Constructors
        public LFUav()
        {

        }
        #endregion

        #region Methods

        #region 仿真运行
        /// <summary>
        /// 开机运行
        /// </summary>
        public void Run()
        {
            SimTimer.Enabled = true;
            SimTimer.Interval = 10;
            SimTimer.Elapsed += MainLoop;

        }

        /// <summary>
        /// 仿真循环
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLoop(object sender, ElapsedEventArgs e)
        {
            /* 检测是否进行任务规划 */

            /* 检测是否进行航迹规划 */

            /* 控制 */


            /* 飞行平台状态更新 */

        }

        #endregion


        #region 核心函数

        public void RoutePlanning()
        {

        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
