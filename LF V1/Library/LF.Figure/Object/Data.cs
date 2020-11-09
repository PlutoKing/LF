/*──────────────────────────────────────────────────────────────
 * FileName     : Data
 * Created      : 2020-01-05 13:32:51
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
    public class Data
    {
        #region Fields
        private ChartFigure _graph;
        private double _x;
        private double _y;
        private double _z;
        #endregion

        #region Properties
        /// <summary>
        /// 所属图
        /// </summary>
        public ChartFigure Graph { get => _graph; set => _graph = value; }

        /// <summary>
        /// 坐标X
        /// </summary>
        public double X { get => _x; set => _x = value; }

        /// <summary>
        /// 坐标X
        /// </summary>
        public double Y { get => _y; set => _y = value; }

        /// <summary>
        /// 坐标X
        /// </summary>
        public double Z { get => _z; set => _z = value; }
        #endregion

        #region Constructors
        public Data(ChartFigure graph)
        {
            _graph = graph;
        }

        public Data(ChartFigure graph,double x,double y)
        {
            _graph = graph;
            _x = x;
            _y = y;
        }
        #endregion

        #region Methods
        public PointF FromDataToLocal()
        {
            float pixX, pixY;
            _graph.Transform(_x, _y, out pixX, out pixY);
            return new PointF(pixX, pixY);
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
