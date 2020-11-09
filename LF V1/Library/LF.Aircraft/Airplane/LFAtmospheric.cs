/*──────────────────────────────────────────────────────────────
 * FileName     : LFAtmospheric
 * Created      : 2020-10-31 16:33:53
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


namespace LF.Aircraft
{
    /// <summary>
    /// 大气模型
    /// </summary>
    public class LFAtmospheric
    {
        #region Fields

        private double _temperature;    // 温度
        private double _density;    // 大气密度
        private double _pressure;   // 大气压强
        private double _gravity;    // 重力加速度
        private double _acousticVelocity;   // 声速

        /// <summary>
        /// 地球半径km
        /// </summary>
        public readonly double EarthRadius = 6356.766;

        /// <summary>
        /// 海平面温度
        /// </summary>
        public readonly double Temperature0 = 288.15;
        public readonly double Pressure0 = 101325;
        public readonly double Density0 = 1.225;
        public readonly double Gravity0 = 9.80665;
        public readonly double AcousticVelocity0 = 340.294;
        #endregion

        #region Properties

        /// <summary>
        /// 大气温度 K
        /// </summary>
        public double Temperature { get => _temperature; set => _temperature = value; }

        /// <summary>
        /// 大气密度 kg/m3
        /// </summary>
        public double Density { get => _density; set => _density = value; }

        /// <summary>
        /// 大气压强 Pa
        /// </summary>
        public double Pressure { get => _pressure; set => _pressure = value; }

        /// <summary>
        /// 重力加速度 m/s2
        /// </summary>
        public double Gravity { get => _gravity; set => _gravity = value; }

        /// <summary>
        /// 声速 m/s
        /// </summary>
        public double AcousticVelocity { get => _acousticVelocity; set => _acousticVelocity = value; }


        #endregion

        #region Constructors
        public LFAtmospheric()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="z">z坐标（负数）</param>
        public void Refresh(double z)
        {
            // 重力位势高度
            double H = (-z) / (1 - z / EarthRadius);

            if (H > 0 && H <= 11)
            {
                double w = 1 - H / 44.3308;
                _temperature = 288.15 * w;
                _pressure = Pressure0 * Math.Pow(w, 5.2559);
                _density = Density0 * Math.Pow(w, 4.2559);
            }
            else if (H > 11 & H < 20)
            {
                double w = Math.Exp((14.9647 - H) / 6.3416);
                _temperature = 216.65;
                _pressure = Pressure0 * 0.11953 * w;
                _density = Density0 * 0.15898 * w;
            }

            _acousticVelocity = 20.0468 * Math.Pow(_temperature, 0.5);
            _gravity = Gravity0 / Math.Pow(1 - z / EarthRadius, 2);
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
