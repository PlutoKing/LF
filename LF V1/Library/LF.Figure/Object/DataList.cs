/*──────────────────────────────────────────────────────────────
 * FileName     : DataList
 * Created      : 2020-01-05 13:34:08
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
    public class DataList : List<Data>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public DataList()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// 获取范围
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        public void GetRange(out double xmin,out double xmax,out double ymin,out double ymax)
        {
            xmin = double.MaxValue;
            xmax = double.MinValue;
            ymin = double.MaxValue;
            ymax = double.MinValue;
            for (int i = 0; i < this.Count; i++)
            {
                if (xmin > this[i].X)
                    xmin = this[i].X;
                if (xmax < this[i].X)
                    xmax = this[i].X;
                if (ymin > this[i].Y)
                    ymin = this[i].Y;
                if (ymax < this[i].Y)
                    ymax = this[i].Y;
            }
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
