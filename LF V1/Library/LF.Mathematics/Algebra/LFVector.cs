/*──────────────────────────────────────────────────────────────
 * FileName     : LFVector
 * Created      : 2020-10-13 11:02:07
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


namespace LF.Mathematics
{
    /// <summary>
    /// 向量
    /// </summary>
    public class LFVector
    {
        #region Fields
        protected double[] _elements;   // 元素
        protected VectorType _type;     // 类型
        #endregion

        #region Properties

        /// <summary>
        /// <see cref="LFVector"/>的元素
        /// </summary>
        public double[] Elements { get => _elements; set => _elements = value; }

        /// <summary>
        /// <see cref="LFVector"/>的类型
        /// <seealso cref="VectorType"/>
        /// </summary>
        public VectorType Type { get => _type; set => _type = value; }

        /// <summary>
        /// <see cref="LFVector"/>的长度
        /// </summary>
        public int Length
        {
            get { return _elements.Length; }
        }

        /// <summary>
        /// <see cref="LFVector"/>的索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get { return _elements[index]; }
            set { _elements[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        public LFVector(VectorType type, int length)
        {
            _type = type;
            _elements = new double[length];
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="elements"></param>
        public LFVector(VectorType type, double[] elements)
        {
            _type = type;
            _elements = elements;
        }

        public LFVector(LFVector rhs)
        {
            this._elements = rhs._elements;
            this._type = rhs._type;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 是否零向量
        /// </summary>
        /// <returns></returns>
        public bool IsZeros()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i] != 0)
                {
                    return false;
                }
            }

            return true;
        }


        #region Computation Methods

        /// <summary>
        /// +<see cref="LFVector"/>，原向量
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static LFVector operator +(LFVector v)
        {
            return new LFVector(v.Type, v.Elements);
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static LFVector operator -(LFVector v)
        {
            LFVector r = new LFVector(v.Type, v.Length);
            for (int i = 1; i < v.Length; i++)
            {
                r[i] = -v[i];
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator +(LFVector v1, LFVector v2)
        {
            int l1 = v1.Length;
            int l2 = v2.Length;
            if (v1.Type == v2.Type && l1 == l2)
            {
                LFVector r = new LFVector(v1.Type, v1.Length);
                for (int i = 0; i < l2; i++)
                {
                    r[i] = v1[i] + v2[i];
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// <see cref="LFVector"/>减法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator -(LFVector v1, LFVector v2)
        {
            int l1 = v1.Length;
            int l2 = v2.Length;
            if (v1.Type == v2.Type && l1 == l2)
            {
                LFVector r = new LFVector(v1.Type, v1.Length);
                for (int i = 0; i < l2; i++)
                {
                    r[i] = v1[i] - v2[i];
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator *(LFVector v1, LFVector v2)
        {
            int l1 = v1.Length;
            int l2 = v2.Length;
            if (v1.Type == v2.Type && l1 == l2)
            {
                LFVector r = new LFVector(v1.Type, v1.Length);
                for (int i = 0; i < l2; i++)
                {
                    r[i] = v1[i] * v2[i];
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator *(double k, LFVector v)
        {
            int l = v.Length;
            LFVector r = new LFVector(v.Type, l);
            for (int i = 0; i < l; i++)
            {
                r[i] = v[i] * k;
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>乘法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static LFVector operator *(LFVector v, double k)
        {
            int l = v.Length;
            LFVector r = new LFVector(v.Type, l);
            for (int i = 0; i < l; i++)
            {
                r[i] = v[i] * k;
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>内积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double InnerProduct(LFVector v1, LFVector v2)
        {
            int l1 = v1.Length;
            int l2 = v2.Length;
            double r = 0;
            if (l1 == l2)
            {
                for (int i = 0; i < l2; i++)
                {
                    r += v1[i] * v2[i];
                }
                return r;
            }

            return r;
        }

        public static LFVector Cross(LFVector v1, LFVector v2)
        {

            int l1 = v1.Length;
            int l2 = v2.Length;
            if (v1.Type == v2.Type && l1 == l2)
            {
                LFVector r = new LFVector(v1.Type, v1.Length);
                for (int i = 0; i < l2; i++)
                {
                    r[i] = v1[i] * v2[i];
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="v"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static LFVector Mapping(LFVector v, Func<double, double> func)
        {
            int l = v.Length;
            LFVector r = new LFVector(v.Type, l);
            for (int i = 0; i < l; i++)
            {
                r[i] = func(v[i]);
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFVector"/>的模
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            int l = this.Length;
            double sum = 0;
            for (int i = 0; i < l; i++)
            {
                sum += Math.Pow(this[i], 2);
            }

            return Math.Sqrt(sum);
        }


        #endregion

        #region Operation  Methods

        /// <summary>
        /// 翻转
        /// </summary>
        public void Reserve()
        {
            double[] tmp = _elements;
            int l = _elements.Length;
            for (int i = 0; i < l; i++)
            {
                _elements[i] = tmp[l - i - 1];
            }
        }


        #endregion

        #region General Methods

        /// <summary>
        /// 转化为<see cref="string"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_type == VectorType.RowVector)
                return string.Join("  ", _elements);
            else
                return string.Join("\r\n", _elements);
        }

        /// <summary>
        /// 判断向量是否相等
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Equals(LFVector v)
        {
            const double eps = 1e-09;
            if (this._type != v._type)
                return false;
            int l = v.Length;
            if (l != this.Length)
                return false;

            for (int i = 0; i < l; i++)
            {
                if (Math.Abs(this[i] - v[i]) > eps)
                {
                    return false;
                }
            }

            return true;
        }


        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }

    /// <summary>
    /// 向量类型
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// 列向量
        /// </summary>
        ColVector,

        /// <summary>
        /// 行向量
        /// </summary>
        RowVector,
    }
}
