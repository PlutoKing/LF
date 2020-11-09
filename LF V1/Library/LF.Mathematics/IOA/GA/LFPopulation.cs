/*──────────────────────────────────────────────────────────────
 * FileName     : LFPopulation
 * Created      : 2020-10-20 20:05:20
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


namespace LF.Mathematics.IOA
{
    public class LFPopulation : List<LFChromosome>, ICloneable
    {
        #region Fields
        // public double[] FitnessArray;
        Random random = new Random();
        #endregion

        #region Properties
        public int Size
        {
            get { return Count; }

        }
        #endregion

        #region Constructors
        public LFPopulation()
        {
        }

        public LFPopulation(LFPopulation rhs)
        {
            foreach (LFChromosome ch in rhs)
            {
                this.Add(ch.Clone());
            }
        }

        public LFPopulation Clone()
        {
            return new LFPopulation(this);
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        #region 1. 选择
        /// <summary>
        /// 选择操作
        /// </summary>
        /// <returns></returns>
        public void Selection(double[] fitnessArray)
        {
            LFPopulation pop = this.Clone();
            int[] index = this.Roulette(fitnessArray);
            for (int i = 0; i < Count; i++)
            {
                this[i] = pop[index[i]].Clone();
            }
        }

        /// <summary>
        /// 轮盘赌
        /// </summary>
        /// <returns></returns>
        public int[] Roulette(double[] fitnessArray)
        {
            /* 计算和 */
            double sum = 0;
            for (int i = 0; i < Count; i++)
            {
                sum += fitnessArray[i];
            }

            /* 计算概率 */
            double[] pArray = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                pArray[i] = fitnessArray[i] / sum;
            }

            /* 计算概率和 */
            double[] pSums = new double[Count];
            pSums[0] = pArray[0];
            for (int i = 1; i < Count; i++)
            {
                pSums[i] = pSums[i - 1] + pArray[i];
            }

            // 随机数
            Random random = new Random();
            double r;

            int[] index = new int[Count];
            for (int k = 0; k < Count; k++)
            {
                for (int i = 0; i < Count; i++)
                {
                    r = random.NextDouble();
                    if (r < pSums[i])
                    {
                        index[k] = i;
                        break;
                    }
                }
            }

            return index;
        }

        #endregion

        #region 2. 交叉

        /// <summary>
        /// 交叉
        /// 正对二进制编码
        /// </summary>
        /// <param name="pc"></param>
        public void Cross(double pc)
        {
            double r;
            int bn = this[0].Codes.Length;
            for (int i = 0; i < Count - 1; i += 2)
            {
                r = random.NextDouble();
                // 交叉概率
                if (r < pc)
                {
                    // 选出两个父辈
                    LFChromosome ch1 = this[i];
                    LFChromosome ch2 = this[i + 1];


                    // 随机选取两个数
                    int index1 = random.Next(0, bn - 1);
                    int index2 = random.Next(0, bn - 1);

                    int tmp;
                    if (index1 > index2)
                    {
                        tmp = index1;
                        index1 = index2;
                        index2 = tmp;
                    }

                    int[] crossCodes1 = ch1.GetSubCodes(index1, index2);
                    int[] crossCodes2 = ch2.GetSubCodes(index1, index2);


                    ch1.BinaryCrossover(crossCodes2, index1);
                    ch2.BinaryCrossover(crossCodes1, index1);
                }
            }
        }
        #endregion

        #region 3. 变异
        public void Mutantion(double pc)
        {
            double r;
            int bn = this[0].Codes.Length;
            for (int i = 0; i < Count; i++)
            {
                r = random.NextDouble();
                if (r < pc)
                {
                    LFChromosome ch = this[i];

                    // 随机选取两个数
                    int index1 = random.Next(0, bn - 1);

                    ch.Codes[index1] = 1 - ch.Codes[index1];
                }
            }
        }
        #endregion

        #region 4. 精英

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
