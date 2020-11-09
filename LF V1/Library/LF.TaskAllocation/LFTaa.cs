/*──────────────────────────────────────────────────────────────
 * FileName     : LFTaa
 * Created      : 2020-10-21 14:55:52
 * Author       : Xu Zhe
 * Description  : 任务分配算法 Task Allocation Algorithm
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

namespace LF.TaskAllocation
{
    /// <summary>
    /// 任务分配算法 Task Allocation Algorithm
    /// </summary>
    public class LFTaa
    {
        #region Fields

        private List<LFTask> _tasks = new List<LFTask>();  // 要完成的任务
        private List<LFAgent> _agents = new List<LFAgent>();        // 执行任务的对象

        #endregion

        #region Properties

        /// <summary>
        /// 任务数目
        /// </summary>
        public int MissionCount
        {
            get { return _tasks.Count; }
        }

        /// <summary>
        /// 执行者数目
        /// </summary>
        public int AgentCount
        {
            get { return _agents.Count; }
        }

        public List<LFAgent> Agents { get => _agents; set => _agents = value; }
        public List<LFTask> Tasks { get => _tasks; set => _tasks = value; }
        #endregion

        #region Constructors
        public LFTaa()
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// 第一种分配情况，无协作
        /// </summary>
        public void Calculate()
        {
            // 任务少于执行者，则选择执行者
            if (MissionCount < AgentCount)
            {
                /* 添补虚拟无代价任务，使得数目相等，再调用匈牙利算法 */
            }
            else if(MissionCount == AgentCount) // 刚好相等
            {
                /* 匈牙利算法 */

                /**/
                for (int i = 0; i < MissionCount; i++)
                {
                    _agents[i].Tasks.Add(_tasks[i]);
                }
                
            }
            else // 任务多于执行者
            {

            }

        }

        /// <summary>
        /// 匈牙利算法
        /// </summary>
        public void Hungary()
        {

        }

        #region 集中分配算法

        public void CentralizedTaa()
        {

        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
