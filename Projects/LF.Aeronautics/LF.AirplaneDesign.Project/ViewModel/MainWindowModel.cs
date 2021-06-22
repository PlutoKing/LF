/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-06-21 22:24:29
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using LF.AirplaneDesign.Project.Common;
using LF.AirplaneDesign.Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.AirplaneDesign.Project.ViewModel
{
    /// <summary>
    /// “主窗口”<seealso cref="MainWindow"/>模型
    /// </summary>
    public class MainWindowModel
    {
        #region Command
        public LFCommand CreateProjectCmd { get; set; }
        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// “主窗口”<seealso cref="MainWindow"/>模型默认构造函数
        /// </summary>
        public MainWindowModel()
        {
            /* 初始化新建项目命令 */
            CreateProjectCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    CreateProject(sender);
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods
        /// <summary>
        /// 新建项目
        /// </summary>
        public void CreateProject(object sender)
        {
            CreateProjectDialog cpd = new CreateProjectDialog();

            if (cpd.ShowDialog() == true)
            {
                
            }
        }

        #endregion
    }
}