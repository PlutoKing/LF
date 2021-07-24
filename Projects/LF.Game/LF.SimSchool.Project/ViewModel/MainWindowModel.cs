/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-07-01 17:18:18
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Xml;
using LF.SimSchool.Project.View;

namespace LF.SimSchool.Project
{
    /// <summary>
    /// 主窗口模型
    /// </summary>
    public class MainWindowModel:LFNotify
    {
        #region Fields
        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; Notify(); }
        }

        #endregion

        #region Properties



        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public LFCommand CloseWindowCmd { get; set; }

        public LFCommand SettingCmd { get; set; }

        public LFCommand NavigationCmd { get; set; }

        #endregion

        #region Constructors
        public MainWindowModel()
        {
            /* 计算按钮1 */
            CloseWindowCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    (sender as MainWindow).Close();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };

            /* 计算按钮1 */
            SettingCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    SetProject(sender);
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };

            NavigationCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    LoadPage(sender);
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };

            LoadPage("CampusPage");
        }
        #endregion

        #region Methods

        public void SetProject( object sender)
        {
            SettingDialog dialog = new SettingDialog();

            if(dialog.ShowDialog() == true)
            {
                (sender as MainWindow).Close();
            }

        }

        public void LoadPage(object sender)
        {
            Type type = Type.GetType("LF.SimSchool.Project.View." + sender.ToString());
            ConstructorInfo cit = type.GetConstructor(System.Type.EmptyTypes);
            MainContent = (FrameworkElement)cit.Invoke(null);
        }
        #endregion
    }
}