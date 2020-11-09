/*──────────────────────────────────────────────────────────────
 * FileName     : LFQuadrotor
 * Created      : 2020-10-31 21:07:24
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
using LF.Mechanics;
using LF.Mathematics;

namespace LF.Aircraft
{
    /// <summary>
    /// 四旋翼
    /// </summary>
    public class LFQuadrotor:IAircraft
    {
        #region Fields
        private LFBody _body;
        #endregion

        #region Properties

        /// <summary>
        /// 运动模型
        /// </summary>
        public LFBody Body { get => _body; set => _body = value; }

        #endregion

        #region Constructors
        public LFQuadrotor()
        {

        }

        #endregion

        #region Methods
        
        /// <summary>
        /// 操纵函数
        /// </summary>
        /// <param name="u"></param>
        /// <param name="Vw"></param>
        /// <param name="dt"></param>
        public void Operate(LFVector u,LFVector3 Vw,double dt)
        {
            LFVector dT = OperateTransform(u);
        }


        /// <summary>
        /// 操纵模块：将四通道输入转化为四个油门量
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public LFVector OperateTransform(LFVector u)
        {
            return u;
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
