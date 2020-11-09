/*──────────────────────────────────────────────────────────────
 * FileName     : LFChromosome
 * Created      : 2020-10-20 19:55:14
 * Author       : Xu Zhe
 * Description  : 染色体
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
    /// 染色体
    /// </summary>
    public class LFChromosome:ICloneable
    {
        #region Fields
        private int _length;        // 染色体长度
        private int[] _codes;         // 染色体编码
        private int[] _secondaryCodes; // 第二编码
        #endregion

        #region Properties

        /// <summary>
        /// 编码长度
        /// </summary>
        public int Length { get => _length; set => _length = value; }

        /// <summary>
        /// 编码
        /// </summary>
        public int[] Codes { get => _codes; set => _codes = value; }

        /// <summary>
        /// 第二编码
        /// </summary>
        public int[] SecondaryCodes { get => _secondaryCodes; set => _secondaryCodes = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="length"></param>
        public LFChromosome(int length)
        {
            _length = length;
            _codes = new int[length];
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codes"></param>
        public LFChromosome(int[] codes)
        {
            _codes = codes;
            _length = codes.Length;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFChromosome(LFChromosome rhs)
        {
            _length = rhs._length;
            _codes = (int[])rhs._codes.Clone();
            if (rhs._secondaryCodes != null)
                _secondaryCodes = (int[])rhs._secondaryCodes.Clone();

        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFChromosome Clone()
        {
            return new LFChromosome(this);
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        /// <summary>
        /// 二进制编码
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void BinaryEncode(double x, double min, double max)
        {
            int y;
            y = (int)((x - min) / (max - min) * (Math.Pow(2, _length) - 1));
            int[] codes = new int[_length];
            for (int i = 0; i < _length; i++)
            {
                codes[_length - i - 1] = y % 2;
                y = y / 2;
            }
        }



        /// <summary>
        /// 二进制解码
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public double BinaryDecode(double min ,double max)
        {
            double x = 0;
            int n = _codes.Length - 1;
            for (int i = 0; i <= n; i++)
            {
                x += _codes[n - i] * Math.Pow(2, i);
            }

            x = x / (Math.Pow(2, n + 1) - 1) * (max - min) + min;

            return x;
        }

        /// <summary>
        /// 适度值评估
        /// </summary>
        /// <param name="FitnessFunction"></param>
        /// <returns></returns>
        public double GetFitness(FitnessEvaluationHandler FitnessFunction)
        {
            if (FitnessFunction != null)
            {
                return FitnessFunction(this);
            }
            else
            {
                throw new Exception("There is not any evaluation functions");
            }
        }

        /// <summary>
        /// 获取子编码片段
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int[] GetSubCodes(int start, int end)
        {
            int tmp;
            if (end < start)
            {
                tmp = start;
                start = end;
                end = tmp;
            }

            int l = end - start + 1;
            int[] r = new int[l];
            for (int i = 0; i < l; i++)
            {
                r[i] = _codes[start + i];
            }
            return r;
        }

        /// <summary>
        /// 交换子编码片段
        /// </summary>
        /// <param name="newCode"></param>
        /// <param name="start"></param>
        public void BinaryCrossover(int[] newCode, int start)
        {
            for (int i = 0; i < newCode.Length; i++)
            {
                if (i + start < _codes.Length)
                    _codes[i + start] = newCode[i];
            }
        }

        /// <summary>
        /// 二进制编码变化子编码变换
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void BinaryMutation(int start, int end)
        {
            int tmp;
            if (end < start)
            {
                tmp = start;
                start = end;
                end = tmp;
            }
            int l = end - start + 1;
            for (int i = 0; i < l; i++)
            {
                _codes[i + start] = 1 - _codes[i + start];
            }
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }

    /// <summary>
    /// 适应度评估处理程序
    /// </summary>
    /// <param name="ch">染色体</param>
    /// <returns></returns>
    public delegate double FitnessEvaluationHandler(LFChromosome ch);

}
