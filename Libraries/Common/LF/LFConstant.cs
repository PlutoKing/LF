/*──────────────────────────────────────────────────────────────
 * FileName     : LFConstant.cs
 * Created      : 2021-06-22 15:32:33
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF
{
    public static class LFConstant
    {
        /// <summary>
        /// 地球半径 6356.766km
        /// </summary>
        public const double EarthRadius = 6356.766;

        /// <summary>
        /// 海平面标准大气压下温度 288.15K
        /// </summary>
        public const double T0 = 288.15;

        /// <summary>
        /// 海平面标准大气压 101325Pa
        /// </summary>
        public const double P0 = 101325;

        /// <summary>
        /// 海平面标准大气压下大气密度 1.225kg/m^3
        /// </summary>
        public const double AirDensity0 = 1.225;

        /// <summary>
        /// 海平面重力加速度 9.80665m/s^2
        /// </summary>
        public const double g0 = 9.80665;

        /// <summary>
        /// 海平面标准大气压下大声速 340.294m/s
        /// </summary>
        public const double a0 = 340.2941;
    }
}