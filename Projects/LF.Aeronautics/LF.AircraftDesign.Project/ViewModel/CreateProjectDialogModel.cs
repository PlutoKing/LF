/*──────────────────────────────────────────────────────────────
 * FileName     : CreateProjectDialogModel.cs
 * Created      : 2021-06-22 15:07:14
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace LF.AircraftDesign.Project.ViewModel
{
    /// <summary>
    /// “新建项目对话框”<seealso cref="View.CreateProjectDialog"/>模型
    /// </summary>
    public class CreateProjectDialogModel : LFNotify
    {
        #region Fields

        /// <summary>
        /// 项目路径
        /// </summary>
        private string _projectPath;

        /// <summary>
        /// 项目名称
        /// </summary>

        private string _projectName;


        #endregion


        #region Properties

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; Notify(); }
        }


        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath
        {
            get { return _projectPath; }
            set { _projectPath = value; Notify(); }
        }

        
        #region Command
        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public LFCommand CloseWindowCmd { get; set; }
        /// <summary>
        /// 新建项目命令
        /// </summary>
        public LFCommand CreateProjectCmd { get; set; }

        /// <summary>
        /// 浏览路径命令
        /// </summary>
        public LFCommand ChoosePathCmd { get; set; }
        #endregion

        #endregion

        #region Constructors
        public CreateProjectDialogModel()
        {
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

            /* 初始化新建项目命令 */
            ChoosePathCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    ChoosePath();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };

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
        /// <param name="sender"></param>
        public void CreateProject(object sender)
        {


            // 对话框结果为True，并关闭窗口
            (sender as Window).DialogResult = true;
            (sender as Window).Close();
        }

        public void ChoosePath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ProjectPath = fbd.SelectedPath;
            }

        }
        #endregion
    }
}