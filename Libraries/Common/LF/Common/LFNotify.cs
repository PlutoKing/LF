/*──────────────────────────────────────────────────────────────
 * FileName     : LFNotify.cs
 * Created      : 2021-06-22 11:17:06
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace LF
{
    /// <summary>
    /// 属性改变通知类
    /// </summary>
    public class LFNotify : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFNotify()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通知属性发生改变
        /// </summary>
        /// <param name="propName"></param>
        public void Notify([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}