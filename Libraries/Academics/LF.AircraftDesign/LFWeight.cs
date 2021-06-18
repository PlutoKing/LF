/*──────────────────────────────────────────────────────────────
 * FileName     : LFWeight.cs
 * Created      : 2021-06-16 22:03:34
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
    /// <summary>
    /// 重量模块
    /// </summary>
    public class LFWeight : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 实现接口：属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private double _wto;        // 起飞重量
        private double _woe;        // 使用空重
        private double _wf;         // 任务燃油重量
        private double _wpl;        // 任务载荷重量
        private double _we;         // 空机重量
        private double _wtof;       // 废油重量
        private double _wcrew;      // 机组成员重量



        #endregion

        #region Properties

        /// <summary>
        /// 起飞重量
        /// </summary>
        public double Wto
        {
            get => _wto;
            set
            {
                _wto = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wto"));
            }
        }

        /// <summary>
        /// 使用空重
        /// </summary>
        public double Woe
        {
            get => _woe;
            set
            {
                _woe = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Woe"));
            }
        }

        /// <summary>
        /// 燃油重量
        /// </summary>
        public double Wf
        {
            get => _wf; set
            {
                _wf = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wf"));
            }
        }

        /// <summary>
        /// 任务载荷重量
        /// </summary>
        public double Wpl
        {
            get => _wpl;
            set
            {
                _wpl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wpl"));
            }
        }

        /// <summary>
        /// 空机重量
        /// </summary>
        public double We
        {
            get => _we;
            set
            {
                _we = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("We"));
            }
        }

        /// <summary>
        /// 废油重量
        /// </summary>
        public double Wtof
        {
            get => _wtof;
            set
            {
                _wtof = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wtof"));
            }
        }

        /// <summary>
        /// 机组成员重量
        /// </summary>
        public double Wcrew
        {
            get => _wcrew;
            set
            {
                _wcrew = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wcrew"));
            }
        }
        #endregion

        #region Constructors
        public LFWeight()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// 计算螺旋桨飞机巡航阶段燃油分数
        /// </summary>
        /// <param name="Rcr">航程(km)</param>
        /// <param name="etap"></param>
        /// <param name="cp"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        public double GetFcrProp(double Rcr, double etap, double cp, double K)
        {
            double tmp = -Rcr / (694.5 * (etap / cp) * (K));
            return Math.Exp(tmp);
        }

        #endregion
    }
}