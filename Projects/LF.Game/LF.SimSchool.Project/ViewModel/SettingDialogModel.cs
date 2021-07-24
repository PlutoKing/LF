/*──────────────────────────────────────────────────────────────
 * FileName     : SettingDialogModel.cs
 * Created      : 2021-07-01 17:29:20
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml;

namespace LF.SimSchool.Project
{
    public class SettingDialogModel
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public LFCommand CloseWindowCmd { get; set; }

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public LFCommand CloseMainWindowCmd { get; set; }
        #endregion

        #region Constructors
        public SettingDialogModel()
        {
            /* 计算按钮1 */
            CloseWindowCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    (sender as Window).Close();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };

            CloseMainWindowCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    
                    (sender as Window).DialogResult = true;
                    (sender as Window).Close();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        #endregion
    }
}