/*──────────────────────────────────────────────────────────────
 * FileName     : LFGa
 * Created      : 2020-10-20 19:54:12
 * Author       : Xu Zhe
 * Description  : 遗传算法 Genetic Algorithm
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Mathematics.IOA
{
    /// <summary>
    /// 遗传算法 Genetic Algorithm
    /// </summary>
    public class LFGa
    {
        #region Fields

        private int _iteration;             // 迭代次数
        private double _pc = 0.75;          // 交叉概率
        private double _pm = 0.01;          // 变异概率
        private int _popSize = 32;          // 种群大小
        private LFPopulation _population;   // 种群


        /// <summary>
        /// 适度值评估函数
        /// </summary>
        public event FitnessEvaluationHandler FitnessFunction;

        #endregion

        #region Properties
        /// <summary>
        /// 迭代次数
        /// </summary>
        public int Iteration { get => _iteration; set => _iteration = value; }
        /// <summary>
        /// 交叉概率
        /// </summary>
        public double Pc { get => _pc; set => _pc = value; }
        /// <summary>
        /// 变异概率
        /// </summary>
        public double Pm { get => _pm; set => _pm = value; }

        /// <summary>
        /// 种群规模
        /// </summary>
        public int PopSize { get => _popSize; set => _popSize = value; }

        /// <summary>
        /// 种群
        /// </summary>
        public LFPopulation Population { get => _population; set => _population = value; }

        #endregion

        #region Constructors
        public LFGa()
        {
            _population = new LFPopulation();
        }


        public LFGa(int iter, int popSize)
        {
            _iteration = iter;
            _popSize = popSize;
            _population = new LFPopulation();
        }

        public LFGa(int iter)
        {
            _iteration = iter;
            _population = new LFPopulation();
        }
        #endregion

        #region Methods

        #region 一般优化问题
        /// <summary>
        /// 一般优化问题
        /// </summary>
        public void Optimize(out LFChromosome chromosome, out List<double> history)
        {
            // 相关变量
            int popSize = _population.Size;                 // 种群大小
            double[] fitnessArray = new double[popSize];    // 记录适度值
            history = new List<double>();                   // 记录历史
            chromosome = _population[0].Clone();            // 输出编码
            double maxFitness = double.MinValue;            // 全局最大适度值
            Random random = new Random();                   // 随机数

            double max = 0;     // 一代种群中适应度最大值（区别于全局最大适度值）
            int index = 0;      // 一代种群中最大值索引

            double oldBest = 0;
            int holdCnt = 0;    // 优化结果保持不变 计数

            for (int i = 0; i < _iteration; i++)
            {
                // 评估种群中的每一个个体
                for (int p = 0; p < popSize; p++)
                {
                    fitnessArray[p] = _population[p].GetFitness(FitnessFunction);
                }
                // 获取最优个体的index和适应度
                LF.GetMax(fitnessArray, out index, out max);

                chromosome = _population[index].Clone();
                history.Add(max);

                // 终止条件检测
                oldBest = maxFitness;
                if (maxFitness < max)
                    maxFitness = max;

                if (oldBest == maxFitness)
                    holdCnt++;
                else
                    holdCnt = 0;

                if (holdCnt >= _iteration / 10)
                    break;

                // 进化
                int[] randomOrder = LF.RandArray(popSize);
                LFPopulation newPop = _population.Clone();    // 复制一份

                for (int p = 3; p < popSize; p += 4)
                {
                    // 随意选取4个个体和其对应的适度值
                    LFChromosome[] sunPop = new LFChromosome[] {
                        _population[randomOrder[p-3]],
                        _population[randomOrder[p-2]],
                        _population[randomOrder[p-1]],
                        _population[randomOrder[p]], };

                    double[] subFitness = new double[] {
                        fitnessArray[randomOrder[p - 3]],
                        fitnessArray[randomOrder[p - 2]],
                        fitnessArray[randomOrder[p - 1]],
                        fitnessArray[randomOrder[p]],};

                    LF.GetMax(subFitness, out index, out max);

                    LFChromosome best = sunPop[index]; // 4个当中最好的

                    newPop[p - 3] = best;   // 保留最优个体

                    int index1 = random.Next(0, best.Codes.Length);
                    int index2 = random.Next(0, best.Codes.Length);

                    newPop[p - 2].Codes = LF.ArrayFlip(best.Codes, index1, index2);    // 翻转
                    newPop[p - 1].Codes = LF.ArraySwap(best.Codes, index1, index2);    // 交换
                    newPop[p].Codes = LF.ArraySlide(best.Codes, index1, index2);       // 滑移
                }

                _population = newPop.Clone();
            }
        }



        #endregion

        #region 旅行商问题

        /// <summary>
        /// 遗传算法求解旅行商问题
        /// </summary>
        /// <param name="city">城市数目</param>
        /// <param name="chromosome">输出编码</param>
        /// <param name="fitness">输出适应度值</param>
        /// <param name="history">输出历史记录</param>
        public void SolveTsp(int city, out LFChromosome chromosome, out double fitness, out double[] history)
        {
            // 种群初始化
            InitializeTsp(1, city);

            double[] fitnessArray = new double[_popSize];       // 记录适度值
            List<double> record = new List<double>();           // 记录历史
            chromosome = _population[0].Clone();                // 输出编码
            double maxFitness = double.MinValue;                // 全局最大适度值
            Random random = new Random();                       // 随机数
            fitness = double.MinValue;
            double max = 0;     // 适应度最大值
            int index = 0;      // 最大值索引

            // 开始循环迭代

            double oldBest = 0;
            int holdCnt = 0;    // 优化结果保持不变 计数

            for (int i = 0; i < _iteration; i++)
            {
                // 评估种群中的每一个个体
                for (int p = 0; p < _popSize; p++)
                {
                    fitnessArray[p] = _population[p].GetFitness(FitnessFunction);
                }

                LF.GetMax(fitnessArray, out index, out max);
                chromosome = _population[index].Clone();

                record.Add(max);

                oldBest = maxFitness;
                if (maxFitness < max)
                    maxFitness = max;
                fitness = maxFitness;
                if (oldBest == maxFitness)
                    holdCnt++;
                else
                    holdCnt = 0;

                if (holdCnt >= _iteration / 10)
                    break;

                // 变异
                int[] randomOrder = LF.RandArray(_popSize);
                LFPopulation newPop = _population.Clone();    // 复制一份
                for (int p = 3; p < _popSize; p += 4)
                {
                    LFChromosome[] sunPop = new LFChromosome[] {
                        _population[randomOrder[p-3]],
                        _population[randomOrder[p-2]],
                        _population[randomOrder[p-1]],
                        _population[randomOrder[p]], };

                    double[] subFitness = new double[] {
                        fitnessArray[randomOrder[p - 3]],
                        fitnessArray[randomOrder[p - 2]],
                        fitnessArray[randomOrder[p - 1]],
                        fitnessArray[randomOrder[p]],};

                    LF.GetMax(subFitness, out index, out max);

                    LFChromosome best = sunPop[index]; // 4个当中最好的

                    newPop[p - 3] = best;   // 保留最优个体

                    int index1 = random.Next(0, best.Codes.Length);
                    int index2 = random.Next(0, best.Codes.Length);

                    newPop[p - 2].Codes = LF.ArrayFlip(best.Codes, index1, index2);
                    newPop[p - 1].Codes = LF.ArraySwap(best.Codes, index1, index2);
                    newPop[p].Codes = LF.ArraySlide(best.Codes, index1, index2);
                }
                _population = newPop.Clone();
            }

            history = record.ToArray();
        }

        /// <summary>
        /// 求解多旅行商问题
        /// </summary>
        /// <param name="salesmen">旅行商数目</param>
        /// <param name="city">城市数目</param>
        /// <param name="chromosome">输出编码</param>
        /// <param name="fitness">输出适应度值</param>
        /// <param name="history">输出历史记录</param>
        public void SolveMtsp(int salesmen, int city, out LFChromosome chromosome, out double fitness, out double[] history)
        {
            // 初始化
            InitializeTsp(salesmen, city);

            List<double> record = new List<double>();
            chromosome = _population[0];

            int popSize = _population.Size;                 // 种群大小
            double[] fitnessArray = new double[popSize];    // 记录适度值
            double maxFitness = double.MinValue;            // 全局最大适度值
            Random random = new Random();                   // 随机数
            fitness = double.MinValue;

            double max = 0;     // 适应度最大值
            int index = 0;      // 最大值索引

            int N = _population[0].Codes.Length;    // 主编码长度
            int M = salesmen - 1;                   // 副编码长度。分割数目（如4个旅行商，需要3个分割，将编码分为3份）

            int dof = N;    // 自由度

            int[] addto = new int[dof + 1];

            double oldBest = 0;
            int holdCnt = 0;    // 优化结果保持不变 计数

            /* 迭代进化 */
            for (int i = 0; i < _iteration; i++)
            {
                /* 对种群中每一个个体进行评估 */
                for (int p = 0; p < popSize; p++)
                {
                    fitnessArray[p] = _population[p].GetFitness(FitnessFunction);
                }

                LF.GetMax(fitnessArray, out index, out max);
                chromosome = _population[index];
                record.Add(max);

                oldBest = maxFitness;
                if (maxFitness < max)
                    maxFitness = max;
                fitness = maxFitness;

                if (oldBest == maxFitness)
                    holdCnt++;
                else
                    holdCnt = 0;

                if (holdCnt >= _iteration / 10)
                    break;

                /* 变异 */
                int[] randomOrder = LF.RandArray(popSize);
                LFPopulation newPop = _population.Clone();    // 复制一份

                for (int p = 7; p < popSize; p += 8)
                {
                    LFChromosome[] sunPop = new LFChromosome[] {
                        _population[randomOrder[p-7]],
                        _population[randomOrder[p-6]],
                        _population[randomOrder[p-5]],
                        _population[randomOrder[p-4]],
                        _population[randomOrder[p-3]],
                        _population[randomOrder[p-2]],
                        _population[randomOrder[p-1]],
                        _population[randomOrder[p]], };

                    double[] subFitness = new double[] {
                        fitnessArray[randomOrder[p - 7]],
                        fitnessArray[randomOrder[p - 6]],
                        fitnessArray[randomOrder[p - 5]],
                        fitnessArray[randomOrder[p - 4]],
                        fitnessArray[randomOrder[p - 3]],
                        fitnessArray[randomOrder[p - 2]],
                        fitnessArray[randomOrder[p - 1]],
                        fitnessArray[randomOrder[p]],};

                    LF.GetMax(subFitness, out index, out max);

                    LFChromosome best = sunPop[index]; // 8个当中最好的

                    newPop[p - 7] = best;   // 保留最优个体

                    int index1 = random.Next(0, best.Codes.Length);
                    int index2 = random.Next(0, best.Codes.Length);

                    newPop[p - 6].Codes = LF.ArrayFlip(best.Codes, index1, index2);
                    newPop[p - 5].Codes = LF.ArraySwap(best.Codes, index1, index2);
                    newPop[p - 4].Codes = LF.ArraySlide(best.Codes, index1, index2);

                    newPop[p - 3].SecondaryCodes = RandBreaks(N, M);
                    newPop[p - 2].Codes = LF.ArrayFlip(best.Codes, index1, index2);
                    newPop[p - 2].SecondaryCodes = RandBreaks(N, M);
                    newPop[p - 1].Codes = LF.ArraySwap(best.Codes, index1, index2);
                    newPop[p - 1].SecondaryCodes = RandBreaks(N, M);
                    newPop[p].Codes = LF.ArraySlide(best.Codes, index1, index2);
                    newPop[p].SecondaryCodes = RandBreaks(N, M);
                }
                _population = newPop.Clone();

            }
            history = record.ToArray();
        }

        /// <summary>
        /// 生成随机分割编码
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int[] RandBreaks(int n, int m)
        {
            List<int> brk = new List<int>();
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                brk.Add(random.Next(1, n - 1));
            }

            brk.Sort();
            return brk.ToArray();
        }

        /// <summary>
        /// 旅行商问题初始化
        /// </summary>
        /// <returns></returns>
        public void InitializeTsp(int salesmen, int city)
        {
            // 检查旅行商
            if (salesmen < 1)
            {
                salesmen = 1;
            }

            // 旅行商问题
            if (salesmen == 1)
            {
                if (_popSize < 4)
                {
                    _popSize = 4;
                }
                else
                {
                    if (_popSize % 4 != 0)
                    {
                        _popSize = ((_popSize / 4) + 1) * 4;
                    }
                }
            }
            else// 多旅行商问题
            {
                if (_popSize < 8)
                {
                    _popSize = 8;
                }
                else
                {
                    if (_popSize % 8 != 0)
                    {
                        _popSize = ((_popSize / 8) + 1) * 8;
                    }
                }
            }

            // 初始化种群
            _population.Clear();
            for (int i = 0; i < _popSize; i++)
            {
                int[] codes = LF.RandArray(city);
                LFChromosome ch = new LFChromosome(codes);  // 主编码
                if (salesmen != 1)
                    ch.SecondaryCodes = RandBreaks(city, salesmen - 1);    // 副编码
                _population.Add(ch);
            }
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
