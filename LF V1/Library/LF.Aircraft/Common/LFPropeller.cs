/*──────────────────────────────────────────────────────────────
 * FileName     : LFPropeller
 * Created      : 2020-11-01 15:29:36
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
using LF.Mathematics;

namespace LF.Aircraft
{
    public class LFPropeller
    {
        #region Fields

        private LFVector3 _force = new LFVector3();
        private LFVector3 _moment = new LFVector3();

        private double _diameter;  // 螺旋桨面积
        private double _cT;  // 螺旋桨系数
        private double _cM;

        #endregion

        #region Properties

        /// <summary>
        /// 力
        /// </summary>
        public LFVector3 Force { get => _force; set => _force = value; }

        /// <summary>
        /// 力矩
        /// </summary>
        public LFVector3 Moment { get => _moment; set => _moment = value; }
        public double Diameter { get => _diameter; set => _diameter = value; }
        public double CT { get => _cT; set => _cT = value; }
        public double CM { get => _cM; set => _cM = value; }


        #endregion

        #region Constructors
        public LFPropeller()
        {
        }

        #endregion

        #region Methods


        /// <summary>
        /// 螺旋桨输出
        /// </summary>
        /// <param name="rho">大气密度</param>
        /// <param name="n">转速(RPM, revolutions per minute)</param>
        public void ForceAndMoment(double rho,double n)
        {
            _force.X = _cT * rho * Math.Pow(_diameter ,4) * Math.Pow((n / 60),2);

            _moment.X = _cM * rho * Math.Pow(_diameter, 5) * Math.Pow((n / 60), 2);
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
