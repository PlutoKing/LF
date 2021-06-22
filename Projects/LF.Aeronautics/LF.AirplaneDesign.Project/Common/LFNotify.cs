/*──────────────────────────────────────────────────────────────
 * FileName     : LFNotify.cs
 * Created      : 2021-06-21 22:53:05
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace LF.AirplaneDesign.Project.Common
{
    /// <summary>
    /// 属性改变统治
    /// </summary>
    public class LFNotify:INotifyPropertyChanged
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
        public void Notify([CallerMemberName] string propName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}