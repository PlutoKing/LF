/*──────────────────────────────────────────────────────────────
 * FileName     : LFAgent
 * Created      : 2020-10-21 09:00:07
 * Author       : Xu Zhe
 * Description  : 代理：用于接收任务的模型
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
    /// 任务分配代理
    /// </summary>
    public class LFAgent:ICloneable
    {
        #region Fields

        private int _id;
        private List<LFTask> _tasks = new List<LFTask>();  // 分配到的任务列表
        private double _cost;

        public event MissionEvaluationHandler EvaluationFunction;   // 分配到的任务评估
        #endregion

        #region Properties
        public int ID { get => _id; set => _id = value; }

        /// <summary>
        /// 分配任务列表
        /// </summary>
        public double Cost { get => _cost; set => _cost = value; }
        public List<LFTask> Tasks { get => _tasks; set => _tasks = value; }
        #endregion

        #region Constructors
        public LFAgent()
        {

        }

        public LFAgent(LFAgent rhs)
        {
            this._id = rhs._id;
            foreach(LFTask m in rhs._tasks)
            {
                this._tasks.Add(m);
            }
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFAgent Clone()
        {
            return new LFAgent(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        public void AnalysisInfluence()
        {
            /* 分析思路：
             * 将某一个任务暂时剔除，分析剩下的任务的适度值，并进行比较
             * 输出想要剔除的任务*/
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }

    /// <summary>
    /// 任务评估
    /// </summary>
    /// <param name="missions">分配的任务</param>
    /// <returns></returns>
    public delegate double MissionEvaluationHandler(List<LFTask> missions);
}
