/*──────────────────────────────────────────────────────────────
 * FileName     : LFLayer
 * Created      : 2020-11-09 09:38:24
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    /// <summary>
    /// 图层
    /// </summary>
    public class LFLayer
    {
        #region Fields

        private LFPane _parent;     // 所属窗格
        #endregion

        #region Properties

        /// <summary>
        /// 所属窗格
        /// </summary>
        public LFPane Parent { get => _parent; set => _parent = value; }

        #endregion

        #region Constructors
        public LFLayer()
        {
        }

        #endregion

        #region Methods
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
