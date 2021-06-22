/*──────────────────────────────────────────────────────────────
 * FileName     : AtmosphereToolWindowModel.cs
 * Created      : 2021-06-22 15:25:18
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml;

namespace LF.AircraftDesign.Project.ViewModel
{
    /// <summary>
    /// 大气参数工具<seealso cref="View.AtmosphereToolWindow"/>模型
    /// </summary>
    public class AtmosphereToolWindowModel:LFNotify
    {
        #region Fields
        private LFAtmosphere _atmosphere;

        private double _t = 15;

        
        private double _viscosity = 1.8250;

       
        private string _errorMessage = "";



        #endregion

        #region Properties

        /// <summary>
        /// 大气参数
        /// </summary>
        public LFAtmosphere Atmosphere
        {
            get { return _atmosphere; }
            set { _atmosphere = value; Notify(); }
        }

        /// <summary>
        /// 温度 ℃
        /// </summary>
        public double T
        {
            get { return _t; }
            set { _t = value; Notify(); }
        }

        /// <summary>
        /// 粘度 10^-6
        /// </summary>
        public double Viscosity
        {
            get { return _viscosity; }
            set { _viscosity = value; Notify(); }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; Notify(); }
        }
        #region Command
        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public LFCommand CloseWindowCmd { get; set; }
        /// <summary>
        /// 计算命令
        /// </summary>
        public LFCommand CalculateCmd { get; set; }
        #endregion
        #endregion

        #region Constructors
        public AtmosphereToolWindowModel()
        {
            _atmosphere = new LFAtmosphere();
            /* 初始化关闭窗口命令 */
            CloseWindowCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    // 将传进来的对象作为窗口关闭
                    (sender as Window).Close();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
            /* 初始化计算命令 */
            CalculateCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    try
                    {
                        _atmosphere.Calculate();
                        T = Atmosphere.Temperature - 273.15;
                        Viscosity = Atmosphere.Viscosity * 1000000;
                        ErrorMessage = "";
                    }
                    catch(Exception ex)
                    {
                        ErrorMessage = ex.Message.ToString();
                    }
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        #endregion
    }
}