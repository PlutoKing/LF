/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-06-30 21:39:50
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.CSharpCatia.Project.ViewModel
{
    public class MainWindowModel
    {
        #region Fields
        public LFFlywing01 flywing01 = new LFFlywing01();
        #endregion

        #region Properties
        public LFCommand DrawCmd { get; set; }
        #endregion

        #region Constructors
        public MainWindowModel()
        {
            /* 初始化命令 */
            DrawCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    flywing01.Catia.Startup();
                    flywing01.Draw();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        #endregion
    }
}