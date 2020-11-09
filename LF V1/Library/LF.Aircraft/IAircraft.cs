/*──────────────────────────────────────────────────────────────
 * FileName     : IAircraft
 * Created      : 2020-10-31 20:57:39
 * Author       : Xu Zhe
 * Description  : 飞行器接口
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
    /// 飞行器接口
    /// </summary>
    public interface IAircraft
    {
        /// <summary>
        /// 运动模型
        /// </summary>
        LFBody Body { get; set; }

        /// <summary>
        /// 操纵函数
        /// </summary>
        /// <param name="u">操纵量</param>
        /// <param name="Vw">风速</param>
        /// <param name="dt">时间步长</param>
        void Operate(LFVector u,LFVector3 Vw,double dt);

    }
}
