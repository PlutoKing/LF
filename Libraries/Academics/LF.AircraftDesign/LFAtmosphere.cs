/*──────────────────────────────────────────────────────────────
 * FileName     : Atmosphere.cs
 * Created      : 2021-06-22 15:27:15
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.AircraftDesign
{
    public class LFAtmosphere : LFNotify
    {
        #region Fields

        private double _altitude;

        private double _temperature = LFConstant.T0;

        private double _pressure = LFConstant.P0;

        private double _density = LFConstant.AirDensity0;

        private double _g = LFConstant.g0;

        private double _acousticSpeed = LFConstant.a0;

        private double _viscosity = 1.82497151817652E-06;

        #endregion

        #region Properties

        /// <summary>
        /// 高度 km
        /// </summary>
        public double Altitude
        {
            get { return _altitude; }
            set { _altitude = value; Notify(); }
        }

        /// <summary>
        /// 温度
        /// </summary>
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; Notify(); }
        }

        /// <summary>
        /// 压强 Pa
        /// </summary>
        public double Pressure
        {
            get { return _pressure; }
            set { _pressure = value; Notify(); }
        }

        /// <summary>
        /// 密度 kg/m^3
        /// </summary>
        public double Density
        {
            get { return _density; }
            set { _density = value; Notify(); }
        }

        /// <summary>
        /// 重力加速度 m/s^2
        /// </summary>
        public double G
        {
            get { return _g; }
            set { _g = value; Notify(); }
        }

        /// <summary>
        /// 声速 m/s
        /// </summary>
        public double AcousticSpeed
        {
            get { return _acousticSpeed; }
            set { _acousticSpeed = value; Notify(); }
        }

        /// <summary>
        /// 粘度 kg/(m·s)
        /// </summary>
        public double Viscosity
        {
            get { return _viscosity; }
            set { _viscosity = value; Notify(); }
        }

        #endregion

        #region Constructors
        public LFAtmosphere()
        {
        }
        #endregion

        #region Methods

        public void Calculate()
        {
            // 计算重力位势高度
            double h = _altitude / (1 + _altitude / LFConstant.EarthRadius);
            double w;
            if (_altitude >= 0 && _altitude <= 11.0191)
            {
                w = 1 - h / 44.3308;
                Temperature = LFConstant.T0 * w;
                Pressure = LFConstant.P0 * Math.Pow(w, 5.2559);
                Density = LFConstant.AirDensity0 * Math.Pow(w, 4.2559);
            }
            else if (_altitude > 11.0191 && _altitude <= 20.0631)
            {
                w = 1 + (h - 24.9021) / 221.552;
                Temperature = 216.650;
                Pressure = 0.11953 * LFConstant.P0 * w;
                Density = 0.15898 * LFConstant.AirDensity0 * w;
            }
            else
            {
                throw new Exception("请输入高度范围在0~20km以内！");
            }

            AcousticSpeed = 20.0468 * Math.Sqrt(Temperature);

            G = LFConstant.g0 / Math.Pow(1 + _altitude / LFConstant.EarthRadius, 2);

            Viscosity = 0.0000001487 * Math.Pow(Temperature, 1.5) / (Temperature + 110.4);
        }
        #endregion
    }
}