/*──────────────────────────────────────────────────────────────
 * FileName     : LFGlobal.cs
 * Created      : 2021-06-16 22:00:10
 * Author       : Xu Zhe
 * Description  : 全局静态信息
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.AircraftDesign
{
    /// <summary>
    /// 全局信息
    /// </summary>
    public class LFGlobal : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 实现接口：属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private LFTypeList _airplaneTypes = new LFTypeList();

        private LFTypeList _specifications = new LFTypeList();
        
        #endregion

        #region Properties
        /// <summary>
        /// 飞机类型
        /// </summary>
        public LFTypeList AirplaneTypes
        {
            get => _airplaneTypes;
            set
            {
                _airplaneTypes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AirplaneTypes"));
            }
        }

        /// <summary>
        /// 规范类型
        /// </summary>
        public LFTypeList Specifications
        {
            get => _specifications;
            set
            {
                _specifications = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Specifications"));
            }
        }
        #endregion

        #region Constructors
        public LFGlobal()
        {
        }
        #endregion

        #region Methods

        #endregion
    }
}