/*──────────────────────────────────────────────────────────────
 * FileName     : LFDrone
 * Created      : 2020-10-20 19:20:37
 * Author       : Xu Zhe
 * Description  : 无人机
 * ──────────────────────────────────────────────────────────────*/

using LF.FlightControl.Guidance;
using LF.Mechanics;
using LF.TaskAllocation;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LF.Mathematics;

namespace LF.FlightControl
{
    /// <summary>
    /// 无人机
    /// </summary>
    public class LFDrone
    {
        #region Fields
        private int _id;
        /* 仿真变量 */
        public Timer SimTimer = new Timer();        // 仿真计时器
        private double _simStep = 0.01d;            // 仿真步长

        /* 运动状态 */
        private LFBody _body = new LFBody();        // 刚体运动模型

        /* 处理模块 */
        private LFAgent _agent = new LFAgent();     // 任务分配代理
        private LFMap2D _map = new LFMap2D();       // 路径规划地图
        private LFRoute2D _route = new LFRoute2D(); // 路径

        /* 导航、制导与控制 */
        int routeID = 0;                            // 当前目标路径点
        LFVector3 targetPosition = new LFVector3(); // 当前期望目标

        #endregion

        #region Properties
        public int ID { get => _id; set => _id = value; }

        /// <summary>
        /// 仿真步长
        /// </summary>
        public double SimStep { get => _simStep; set => _simStep = value; }

        #region Body
        /// <summary>
        /// 刚体运动信息
        /// </summary>
        public LFBody Body { get => _body; set => _body = value; }

        /// <summary>
        /// 位置坐标X
        /// </summary>
        public double X
        {
            get { return _body.Position.X; }
            set { _body.Position.X = value; }
        }

        /// <summary>
        /// 位置坐标Y
        /// </summary>
        public double Y
        {
            get { return _body.Position.Y; }
            set { _body.Position.Y = value; }
        }

        /// <summary>
        /// 位置坐标Z
        /// </summary>
        public double Z
        {
            get { return _body.Position.Z; }
            set { _body.Position.Z = value; }
        }

        /// <summary>
        /// 滚转角φ
        /// </summary>
        public double Roll
        {
            get { return _body.Attitude.X; }
            set { _body.Attitude.X = value; }
        }

        /// <summary>
        /// 俯仰角θ
        /// </summary>
        public double Pitch
        {
            get { return _body.Attitude.Y; }
            set { _body.Attitude.Y = value; }
        }

        /// <summary>
        /// 偏航角ψ
        /// </summary>
        public double Yaw
        {
            get { return _body.Attitude.Z; }
            set { _body.Attitude.Z = value; }
        }

        /// <summary>
        /// X轴速度U
        /// </summary>
        public double U
        {
            get { return _body.Velocity.X; }
            set { _body.Velocity.X = value; }
        }

        /// <summary>
        /// Y轴速度V
        /// </summary>
        public double V
        {
            get { return _body.Velocity.Y; }
            set { _body.Velocity.Y = value; }
        }


        /// <summary>
        /// Z轴速度W
        /// </summary>
        public double W
        {
            get { return _body.Velocity.Z; }
            set { _body.Velocity.Z = value; }
        }

        /// <summary>
        /// 绕X轴角速度P
        /// </summary>
        public double P
        {
            get { return _body.AngularVelocity.X; }
            set { _body.AngularVelocity.X = value; }
        }

        /// <summary>
        /// 绕Y轴角速度Q
        /// </summary>
        public double Q
        {
            get { return _body.AngularVelocity.Y; }
            set { _body.AngularVelocity.Y = value; }
        }

        /// <summary>
        /// 绕Z轴角速度R
        /// </summary>
        public double R
        {
            get { return _body.AngularVelocity.Z; }
            set { _body.AngularVelocity.Z = value; }
        } 
        #endregion

        public LFAgent Agent { get => _agent; set => _agent = value; }
        public LFMap2D Map { get => _map; set => _map = value; }
        public LFRoute2D Route { get => _route; set => _route = value; }


        #endregion

        #region Constructors
        public LFDrone()
        {
            _agent.EvaluationFunction += Agent_EvaluationFunction;
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
            SimTimer.Tick += new EventHandler(MainLoop);

        }

        /// <summary>
        /// 仿真循环
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainLoop(object sender, EventArgs e)
        {
            /* 判断是否进行路径规划 */


            /* 计算期望姿态 */
            U = 30;

            targetPosition = new LFVector3(_agent.Tasks[0].X, _agent.Tasks[0].Y, 0);

            Cal_R(targetPosition);

            _body.Kinematics(_simStep);
            double d = (Body.Position - targetPosition).Norm();

            if ( d< 10d)
            {
                _agent.Tasks[0].IsDone = true;
            }

        }


        public void Cal_R(LFVector3 Target)
        {
            #region 偏航角速率
            /* 计算位置偏差 */
            double dy = Target.X - X;
            double dx = Target.Y - Y;
            double dyaw = 0; // 角度偏差
            if (dy == 0)
            {
                if (dx >= 0)
                {
                    dyaw = 90;
                }
                else
                {
                    dyaw = -90;
                }
            }
            else if (dy > 0)
            {
                dyaw = Math.Atan(dx / dy) * 180 / Math.PI;
            }
            else
            {
                if (dx >= 0)
                {
                    dyaw = Math.Atan(dx / dy) * 180 / Math.PI + 180;
                }
                else
                {
                    dyaw = Math.Atan(dx / dy) * 180 / Math.PI - 180;
                }
            }
            dyaw -= Yaw;     // 偏差角
            /* 限幅 */
            while (dyaw > 180)
            {
                dyaw -= 360;
            }
            while (dyaw <= -180)
            {
                dyaw += 360;
            }

            double rMax = (U / 10 * 2);
            double r = dyaw * 2;        // 计算偏航角速率
                                        /* 限幅 */
            if (r > rMax)
            {
                r = rMax;
            }
            else if (r < -1 * rMax)
            {
                r = -1 * rMax;
            }
            R = r;
            #endregion
        }

        #endregion

        #region 任务分配
        /// <summary>
        /// 任务分配评估函数
        /// </summary>
        /// <param name="missions"></param>
        /// <returns></returns>
        private double Agent_EvaluationFunction(List<LFTask> missions)
        {
            _route = _map.GetRoute_GA(100, out double length, out _);
            return length;
        } 
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
