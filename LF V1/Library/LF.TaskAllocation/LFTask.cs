/*──────────────────────────────────────────────────────────────
 * FileName     : LFTask
 * Created      : 2020-10-20 19:17:12
 * Author       : Xu Zhe
 * Description  : 任务
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.TaskAllocation
{
    /// <summary>
    /// 任务
    /// </summary>
    public class LFTask
    {
        #region Fields
        private int _id = 0;
        private bool _isDone = false;

        private double _x;
        private double _y;

        #endregion

        #region Properties

        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get => _id; set => _id = value; }


        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get => _isDone; set => _isDone = value; }
        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        #endregion

        #region Constructors
        public LFTask()
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
