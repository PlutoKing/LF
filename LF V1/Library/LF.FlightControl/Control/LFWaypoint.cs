/*──────────────────────────────────────────────────────────────
 * FileName     : LFWaypoint
 * Created      : 2020-10-21 14:50:59
 * Author       : Xu Zhe
 * Description  : 航点
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.FlightControl
{
    /// <summary>
    /// 航点指令
    /// </summary>
    public class LFWaypoint
    {
        #region Fields

        private double _x;
        private double _y;
        private double _z;

        #endregion

        #region Properties
        /// <summary>
        /// 航点位置X坐标
        /// </summary>
        public double X { get => _x; set => _x = value; }
        /// <summary>
        /// 航点位置Y坐标
        /// </summary>
        public double Y { get => _y; set => _y = value; }
        /// <summary>
        /// 航点位置Z坐标
        /// </summary>
        public double Z { get => _z; set => _z = value; }


        #endregion

        #region Constructors
        public LFWaypoint()
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
