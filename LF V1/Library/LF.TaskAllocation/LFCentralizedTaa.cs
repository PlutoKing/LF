/*──────────────────────────────────────────────────────────────
 * FileName     : LFCentralizedTaa
 * Created      : 2020-10-21 15:15:41
 * Author       : Xu Zhe
 * Description  : 集中式任务分配算法
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
    /// 集中式任务分配算法
    /// </summary>
    public class LFCentralizedTaa
    {
        #region Fields
        private List<LFTask> _missions = new List<LFTask>();  // 要完成的任务
        private List<LFAgent> _agents = new List<LFAgent>();        // 执行任务的对象


        /* 全局交流信息 */
        private List<double> evaluationValue = new List<double>(); // 用于记录评估值
        public List<LFTask> RemoveList = new List<LFTask>();
        public List<int> RemoveIndex = new List<int>();
        public static List<int> AddIndex = new List<int>();


        public double newMax = -1;
        public double oldMax = 0;

        MissionEvaluationHandler EvaluationFunction;
        #endregion

        #region Properties
        public List<LFAgent> Agents { get => _agents; set => _agents = value; }

        #endregion

        #region Constructors
        public LFCentralizedTaa()
        {

        }
        #endregion

        #region Methods
        public void Calculate()
        {
            Initialization();

            Evaluation();

            int c = 0;
            for(int i = 0; i < 300; i++)
            {
                RemoveMission();
                AddMission();
                Evaluation();
                if(newMax >= oldMax)
                {
                    Undo();
                    newMax = oldMax;
                    c++;
                }
                else
                {
                    c = 0;
                }
                if (c >= 10)
                {
                    break;
                }
                evaluationValue.Add(newMax);
            }
            Evaluation();
        }

       

        public void Initialization()
        {
            int i = 0;
            evaluationValue.Clear();
            newMax = -1;
            oldMax = 0;

            foreach(LFAgent a in _agents)
            {
                a.Tasks.Clear();
            }
            foreach(LFTask m in _missions)
            {
                if (!m.IsDone)
                {
                    _agents[i].Tasks.Add(m);
                    i++;
                    if (i >= _agents.Count)
                    {
                        i = 0;
                    }
                }
            }
        }

        public static List<double> Costs = new List<double>();


        public void Evaluation()
        {
            oldMax = newMax;
            Costs.Clear();
            foreach (LFAgent a in _agents)
            {
                /* 路径规划计算距离 */
                List<LFTask> mtmp = new List<LFTask>();
                foreach(LFTask m1 in _missions)
                {
                    bool isAdd = false;
                    foreach(LFTask m2 in a.Tasks)
                    {
                        if(m1.ID == m2.ID)
                        {
                            isAdd = false;
                            break;
                        }
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        mtmp.Add(m1);
                    }
                }

                Costs.Add(EvaluationFunction(mtmp));
            }
            newMax = GetMax(Costs.ToArray()).value;
        }

        public void RemoveMission()
        {
            RemoveList.Clear();                 // 清空移除任务缓存列表
            RemoveIndex.Clear();                // 清空序号缓存列表

            foreach(LFAgent a in _agents)
            {
                int l = a.Tasks.Count;
                double[] Influence = new double[l];
                int removeIndex = 0;
                for (int i = 0; i < l; i++)
                {
                    Influence[i] = a.Cost - NewCost(a, i);
                }

                removeIndex = GetMax(Influence).index;
                if (a.Tasks.Count > 2 && removeIndex != -1)    // removeIndex为-1表示不移除，不为-1时移除
                {
                    RemoveList.Add(a.Tasks[removeIndex]);  // 
                    a.Tasks.RemoveAt(removeIndex);
                    RemoveIndex.Add(removeIndex);
                }
                else
                {
                    RemoveList.Add(null);       // 不移除时，零时序列中为空，序号为-1，-1代表空
                    RemoveIndex.Add(-1);
                }

            }

        }

        public double NewCost(LFAgent a,int c)
        {
            LFAgent atmp = a.Clone();

            atmp.Tasks.RemoveAt(c);  // 移除

            atmp.Cost = EvaluationFunction(atmp.Tasks);

            return atmp.Cost;

        }

        public void AddMission()
        {
            int[] addIndex = RandArray(RemoveList.Count);   // 临时列表中的随机序列
            AddIndex.Clear();
            for (int i = 0; i < RemoveList.Count; i++)
            {
                AddIndex.Add(RemoveIndex[addIndex[i]]);     // 添加的ID，
                if (RemoveList[addIndex[i]] != null)        // 不为空表示 RemoveIndex不为-1
                {
                    _agents[i].Tasks.Add(RemoveList[addIndex[i]]);
                }
            }
        }


        /// <summary>
        /// 恢复
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < RemoveList.Count; i++)
            {
                if (AddIndex[i] != -1)       // 不为-1表示添加，添加过就需要移除
                {
                    _agents[i].Tasks.RemoveAt(_agents[i].Tasks.Count - 1);
                }
                if (RemoveIndex[i] != -1)   // 不为-1表示移除过，移除过需要插入
                {
                    _agents[i].Tasks.Insert(RemoveIndex[i], RemoveList[i]);
                }
            }
        }

        #region Max and Min
        public struct MaxMin
        {
            public int index;
            public double value;
        }
        public static MaxMin GetMin(double[] arr)
        {
            int i = 0;
            double m = arr[i];

            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[j] < m)
                {
                    m = arr[j];
                    i = j;
                }
            }
            MaxMin min = new MaxMin();
            min.index = i;
            min.value = m;
            return min;
        }
        public static MaxMin GetMax(double[] arr)
        {
            int i = 0;
            double m = arr[i];

            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[j] > m)
                {
                    m = arr[j];
                    i = j;
                }
            }
            MaxMin max = new MaxMin();
            max.index = i;
            max.value = m;
            return max;
        }

        public static int[] RandArray(int n)
        {
            int[] arr = new int[n];

            for (int j = 0; j < n; j++)
            {
                arr[j] = j;
            }

            int[] newarr = new int[n];
            int k = 0;
            while (k < n)
            {
                int temp = new Random().Next(0, n);
                if (arr[temp] != -1)
                {
                    newarr[k] = arr[temp];
                    k++;
                    arr[temp] = -1;
                }
            }
            return newarr;
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
