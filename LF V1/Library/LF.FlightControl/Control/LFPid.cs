/*──────────────────────────────────────────────────────────────
 * FileName     : LFPid
 * Created      : 2020-10-31 15:39:00
 * Author       : Xu Zhe
 * Description  : PID控制器
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.FlightControl.Control
{
    /// <summary>
    /// PID控制器
    /// </summary>
    public class LFPid
    {
        #region Fields

        private double _kp;
        private double _ki;
        private double _kd;

        private double _errOld;
        private double _errSum;
        #endregion

        #region Properties
        /// <summary>
        /// 比例系数
        /// </summary>
        public double Kp { get => _kp; set => _kp = value; }

        /// <summary>
        /// 积分系数
        /// </summary>
        public double Ki { get => _ki; set => _ki = value; }

        /// <summary>
        /// 微分系数
        /// </summary>
        public double Kd { get => _kd; set => _kd = value; }


        #endregion

        #region Constructors
        public LFPid()
        {
        }

        public LFPid(double kp,double ki,double kd)
        {
            _kp = kp;
            _ki = ki;
            _kd = kd;
        }

        #endregion

        #region Methods

        /// <summary>
        /// PID控制器
        /// </summary>
        /// <param name="expectedVal"></param>
        /// <param name="measuredVal"></param>
        /// <returns>操纵量</returns>
        public double Run(double expectedVal,double measuredVal)
        {
            double err = expectedVal - measuredVal;
            double Pout = _kp * err;

            _errSum += err;                 // 误差值积分

            double Iout = _ki * _errSum;

            double derr = err - _errOld;    // 误差值微分

            double Dout = _kd * derr;

            double output = Pout + Iout + Dout;

            return output;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
