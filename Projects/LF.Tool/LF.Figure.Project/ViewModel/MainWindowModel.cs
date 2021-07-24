/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-06-23 10:19:02
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.Figure.Project.ViewModel
{
    public class MainWindowModel:LFNotify
    {
        #region Fields

        #endregion

        #region Properties
        private string _figureTitle;

        public string FigureTitle
        {
            get { return _figureTitle; }
            set 
            { 
                _figureTitle = value;
                Notify();

            }
        }

        #endregion

        #region Constructors
        public MainWindowModel()
        {
        }
        #endregion

        #region Methods
        public void ConfigFigure()
        {

        }
        #endregion
    }
}