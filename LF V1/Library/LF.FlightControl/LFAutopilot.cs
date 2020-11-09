/*──────────────────────────────────────────────────────────────
 * FileName     : LFAutopilot
 * Created      : 2020-10-31 15:35:17
 * Author       : Xu Zhe
 * Description  : 自动驾驶仪
 * 导航、制导与控制
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
using LF.Aircraft;
using LF.FlightControl.Control;
using LF.FlightControl.Guidance;

namespace LF.FlightControl
{
    /// <summary>
    /// 自动驾驶仪
    /// </summary>
    public class LFAutopilot
    {
        #region Fields

        /* 导航 */
        /* 制导 */
        private LFMap2D _map = new LFMap2D();       // 航迹规划地图
        int routeID = 0;                            // 当前目标路径点
        LFVector3 targetPosition = new LFVector3(); // 当前期望目标

        /* 控制 */
        private LFPid rollPID = new LFPid();        // 滚转角PID
        private LFPid pitchPID = new LFPid();       // 俯仰角PID
        private LFPid yawPID = new LFPid();         // 偏航角PID
        private LFPid sideOffsetPID = new LFPid();  // 侧偏距PID
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public LFAutopilot()
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="step">时间步长</param>
        public void Run(double step)
        {

        }


        /// <summary>
        /// 制导模块
        /// </summary>
        public void GuidanceModel()
        {
            _map.GetRoute();
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
