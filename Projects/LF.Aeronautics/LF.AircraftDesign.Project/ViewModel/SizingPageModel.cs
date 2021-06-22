/*──────────────────────────────────────────────────────────────
 * FileName     : SizingPageModel.cs
 * Created      : 2021-06-22 18:13:56
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using LF.AircraftDesign.Project.View;

namespace LF.AircraftDesign.Project.ViewModel
{
    /// <summary>
    /// 初步设计页面模型
    /// </summary>
    public class SizingPageModel
    {
        #region Fields

        #endregion

        #region Properties
        public LFCommand GetFuelFractionInCruiseCmd { get; set; }

        #endregion

        #region Constructors
        public SizingPageModel()
        {
            /* 大气参数工具 */
            GetFuelFractionInCruiseCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    FuelFractionInCruiseWindow ffcw = new FuelFractionInCruiseWindow();
                    ffcw.ShowDialog();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        #endregion
    }
}