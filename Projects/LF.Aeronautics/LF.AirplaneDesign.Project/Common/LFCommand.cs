/*──────────────────────────────────────────────────────────────
 * FileName     : LFCommand.cs
 * Created      : 2021-06-21 22:25:57
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;

namespace LF.AirplaneDesign.Project.Common
{
    /// <summary>
    /// 命令
    /// </summary>
    public class LFCommand:ICommand
    {
        #region Fields
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Properties
        public Action<object> DoExecute { get; set; }

        public Func<object, bool> DoCanExecute { get; set; }
        #endregion

        #region Constructors
        public LFCommand()
        {
        }

        #endregion

        #region Methods

        public bool CanExecute(object sender)
        {
            return DoCanExecute?.Invoke(sender) == true;
        }

        public void Execute(object parameter)
        {
            DoExecute?.Invoke(parameter);
        }

        #endregion
    }
}