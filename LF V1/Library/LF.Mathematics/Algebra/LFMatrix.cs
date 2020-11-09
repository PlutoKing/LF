/*──────────────────────────────────────────────────────────────
 * FileName     : LFMatrix
 * Created      : 2020-10-13 11:01:05
 * Author       : Xu Zhe
 * Description  : 矩阵及矩阵运算
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Text;

namespace LF.Mathematics
{
    /// <summary>
    /// 矩阵
    /// </summary>
    public class LFMatrix : ICloneable
    {
        #region Fields
        protected double[,] _elements;  // 矩阵元素
        #endregion

        #region Properties

        /// <summary>
        /// <see cref="LFMatrix"/>的元素
        /// </summary>
        public double[,] Elements { get => _elements; set => _elements = value; }

        /// <summary>
        /// <see cref="LFMatrix"/>的行数
        /// </summary>
        public int RowCount
        {
            get { return _elements.GetLength(0); }
        }

        /// <summary>
        /// <see cref="LFMatrix"/>的列数
        /// </summary>
        public int ColCount
        {
            get { return _elements.GetLength(1); }
        }

        /// <summary>
        /// <see cref="LFMatrix"/>的元素个数
        /// </summary>
        public int Count
        {
            get { return _elements.Length; }
        }

        /// <summary>
        /// <see cref="LFMatrix"/>的索引器
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public double this[int m, int n]
        {
            get { return _elements[m, n]; }
            set { _elements[m, n] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 以行数<paramref name="m"/>和列数<paramref name="n"/>构造矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        public LFMatrix(int m, int n)
        {
            _elements = new double[m, n];
        }

        /// <summary>
        /// 以行数构造矩阵
        /// </summary>
        /// <param name="element"></param>
        public LFMatrix(double[,] element)
        {
            _elements = element;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFMatrix(LFMatrix rhs)
        {
            int m = rhs.RowCount;
            int n = rhs.ColCount;
            _elements = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    _elements[i, j] = rhs[i, j];
                }
            }
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFMatrix Clone()
        {
            return new LFMatrix(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods

        #region 1. 常见矩阵

        /// <summary>
        /// <paramref name="m"/>阶恒等矩阵
        /// </summary>
        /// <param name="m">阶数</param>
        /// <returns></returns>
        public static LFMatrix IdentityMatrix(int m)
        {
            LFMatrix r = new LFMatrix(m, m);
            for (int i = 0; i < m; i++)
            {
                r[i, i] = 1;
            }
            return r;
        }

        /// <summary>
        /// 初等交换矩阵
        /// </summary>
        /// <param name="m">阶数</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static LFMatrix ElementaryMatrix(int m, int i, int j)
        {
            LFMatrix r = LFMatrix.IdentityMatrix(m);
            r[i, i] = 0;
            r[i, j] = 1;
            r[j, j] = 0;
            r[j, i] = 1;
            return r;
        }

        /// <summary>
        /// 初等数乘矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static LFMatrix ElementaryMatrix(int m, int i, double k)
        {
            LFMatrix r = LFMatrix.IdentityMatrix(m);
            if (k != 0)
            {
                r[i, i] = k;
            }
            return r;
        }

        /// <summary>
        /// 初等倍加矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static LFMatrix ElementaryMatrix(int m, int i, int j, double k)
        {
            LFMatrix r = LFMatrix.IdentityMatrix(m);
            if (k != 0)
            {
                r[i, j] = k;
            }
            return r;
        }
        #endregion

        #region 2. 矩阵结构

        /// <summary>
        /// 是否方阵
        /// </summary>
        /// <returns></returns>
        public bool IsSqure()
        {
            return (RowCount == ColCount);
        }

        /// <summary>
        /// 提取第<paramref name="index"/>列的列向量
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LFVector ColVector(int index)
        {
            LFVector r = new LFVector(VectorType.ColVector, RowCount);

            if (index < ColCount)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    r[i] = _elements[i, index];
                }
            }

            return r;
        }

        /// <summary>
        /// 行向量
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LFVector RowVector(int index)
        {
            LFVector r = new LFVector(VectorType.RowVector, ColCount);

            if (index < RowCount)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    r[i] = _elements[index, i];
                }
            }
            else
            {
                throw new Exception("索引超出矩阵列数！");
            }

            return r;
        }


        /// <summary>
        /// 列子矩阵
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LFMatrix ColSubMatrix(int[] index)
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);
            int n2 = index.Length;
            LFMatrix r = new LFMatrix(m, n2);
            if (n2 <= n)
            {
                for (int j = 0; j < n2; j++)
                {
                    for (int i = 0; i < m; i++)
                    {
                        if (index[j] < n)
                            r[i, j] = _elements[i, index[j]];
                    }
                }
            }

            return r;
        }

        /// <summary>
        /// 行子矩阵
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LFMatrix RowSubMatrix(int[] index)
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);
            int m2 = index.Length;

            LFMatrix r = new LFMatrix(m2, n);

            if (m2 <= m)
            {
                for (int i = 0; i < m2; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (index[i] < m)
                            r[i, j] = _elements[index[i], j];
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// 子矩阵
        /// </summary>
        /// <param name="mIndex">行索引</param>
        /// <param name="nIndex">列索引</param>
        /// <returns></returns>
        public LFMatrix SubMatrix(int[] mIndex, int[] nIndex)
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);
            int m2 = mIndex.Length;
            int n2 = nIndex.Length;
            LFMatrix r = new LFMatrix(m2, n2);

            if (m2 <= m && n2 <= n)
            {
                for (int i = 0; i < m2; i++)
                {
                    for (int j = 0; j < n2; j++)
                    {
                        if (mIndex[i] < m && nIndex[j] < n)
                            r[i, j] = _elements[mIndex[m2], nIndex[j]];
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// 余子式矩阵
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public LFMatrix MinorMatrix(int p, int q)
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);

            if (m > 1 && n > 1)
            {
                LFMatrix r = new LFMatrix(m - 1, n - 1);
                int a = 0;
                int b = 0;
                if (p >= 0 && p < m && q >= 0 && q < n)
                {
                    for (int i = 0; i < m; i++)
                    {
                        if (i != p)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                if (j != q)
                                {
                                    r[a, b] = _elements[i, j];
                                    b++;
                                }
                            }
                            b = 0;
                            a++;
                        }
                    }
                    a = 0;
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <returns></returns>
        public LFMatrix Transpose()
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);

            LFMatrix r = new LFMatrix(n, m);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    r[i, j] = _elements[j, i];
                }
            }
            return r;
        }

        /// <summary>
        /// 增广矩阵
        /// </summary>
        /// <param name="M"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static LFMatrix AugmentedMatrix(LFMatrix M, LFMatrix N)
        {
            int m1 = M.RowCount;
            int n1 = N.ColCount;

            int m2 = M.RowCount;
            int n2 = N.ColCount;

            if (m1 == m2)
            {
                LFMatrix r = new LFMatrix(m1, n1 + n2);
                for (int i = 0; i < m1; i++)
                {
                    for (int j = 0; j < n1; j++)
                    {
                        r[i, j] = M[i, j];
                    }
                    for (int k = 0; k < n2; k++)
                    {
                        r[i, k + n1] = N[i, k];
                    }
                }
                return r;
            }

            return null;

        }

        #endregion

        #region 2. Computation Methods

        /// <summary>
        /// 原矩阵
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static LFMatrix operator +(LFMatrix M)
        {
            return new LFMatrix(M._elements);
        }

        /// <summary>
        /// 负矩阵
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static LFMatrix operator -(LFMatrix M)
        {
            int m = M.RowCount;
            int n = M.ColCount;

            LFMatrix r = new LFMatrix(m, n);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    r._elements[m, n] = M._elements[m, n];
                }
            }

            return r;
        }

        /// <summary>
        /// 矩阵<see cref="LFMatrix"/>的加法
        /// </summary>
        /// <param name="M"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static LFMatrix operator +(LFMatrix M, LFMatrix N)
        {
            int m1 = M.RowCount;
            int n1 = M.ColCount;
            int m2 = N.RowCount;
            int n2 = N.ColCount;

            if (m1 == m2 && n1 == n2)
            {
                LFMatrix r = new LFMatrix(m1, n1);
                for (int i = 0; i < m1; i++)
                {
                    for (int j = 0; j < n1; j++)
                    {
                        r._elements[m1, n1] = M._elements[m1, n1] + N._elements[m2, n2];
                    }
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// 矩阵<see cref="LFMatrix"/>的减法
        /// </summary>
        /// <param name="M"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static LFMatrix operator -(LFMatrix M, LFMatrix N)
        {
            int m1 = M.RowCount;
            int n1 = M.ColCount;
            int m2 = N.RowCount;
            int n2 = N.ColCount;

            if (m1 == m2 && n1 == n2)
            {
                LFMatrix r = new LFMatrix(m1, n1);
                for (int i = 0; i < m1; i++)
                {
                    for (int j = 0; j < n1; j++)
                    {
                        r._elements[m1, n1] = M._elements[m1, n1] - N._elements[m2, n2];
                    }
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// 矩阵<see cref="LFMatrix"/>的乘法
        /// </summary>
        /// <param name="M"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static LFMatrix operator *(LFMatrix M, LFMatrix N)
        {
            int m1 = M.RowCount;
            int n1 = M.ColCount;
            int m2 = N.RowCount;
            int n2 = N.ColCount;

            if (n1 == m2)   // 第一矩阵的列数 = 第二矩阵的行数
            {
                LFMatrix r = new LFMatrix(m1, n2);
                for (int i = 0; i < m1; i++)
                {
                    for (int j = 0; j < n2; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < n1; k++)
                        {
                            sum += M[i, k] * N[k, j];
                        }
                        r[i, j] = sum;
                    }
                }
                return r;
            }

            return null;
        }

        /// <summary>
        /// 解方程组
        /// </summary>
        /// <param name="M"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static LFVector operator /(LFMatrix M, LFVector b)
        {
            int m = M.RowCount;
            int n = b.Length;

            if (m == n)
            {
                double D = M.Determinant();

                LFVector x = new LFVector(VectorType.ColVector, m);
                if (D != 0)
                {
                    // 计算Dj，把D的j列换成b
                    for (int j = 0; j < m; j++)
                    {
                        LFMatrix A = M.Clone();

                        for (int i = 0; i < m; i++)
                        {
                            A[i, j] = b[i];

                        }

                        double d = A.Determinant();
                        x[j] = d / D;

                    }
                }
                return x;
            }
            return null;
        }

        /// <summary>
        /// 矩阵<see cref="LFMatrix"/>与向量<see cref="LFVector"/>的乘法
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static LFVector operator *(LFMatrix M, LFVector v)
        {
            int m = M.RowCount;
            int n = M.ColCount;
            if (n == v.Length && v.Type == VectorType.ColVector)
            {
                LFVector r = new LFVector(VectorType.ColVector, m);
                for (int i = 0; i < m; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sum += M[i, j] * v[j];
                    }
                    r[i] = sum;
                }

                return r;
            }
            return null;

        }
        #endregion

        #region 矩阵计算

        /// <summary>
        /// 矩阵化简
        /// </summary>
        /// <returns></returns>
        public LFMatrix Simplify()
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);

            // 复制原矩阵
            LFMatrix r = this.Clone();

            int startRow = 0;
            for (int j = 0; j < n; j++) // 一列一列遍历
            {
                // 若第startRow行，第j列的元素不为0
                if (r[startRow, j] != 0)
                {
                    double k = 1 / r[startRow, j];
                    r = ElementaryMatrix(m, startRow, k) * r;  // 第startRow行，第j列的元素化为1
                    // Console.WriteLine("初等数乘第" + startRow.ToString() + "行" + k.ToString() + "：\n" + r.ToString());
                    // 把这个元素下面的元素化为0
                    if (startRow != m - 1)
                    {
                        for (int i = startRow + 1; i < m; i++)
                        {
                            if (r[i, j] != 0)
                            {
                                double k2 = -r[i, j] / r[startRow, j];
                                r = ElementaryMatrix(m, i, startRow, k2) * r;
                                // Console.WriteLine("初等倍加,第" + i.ToString() + "行+第" + startRow.ToString() + "行" + k2.ToString() + "：\n" + r.ToString());
                            }
                        }
                    }

                    // 这个元素上面的元素化为0
                    if (startRow != 0)
                    {
                        for (int i = 0; i < startRow; i++)
                        {
                            if (r[i, j] != 0)
                            {
                                double k2 = -r[i, j] / r[startRow, j];
                                r = ElementaryMatrix(m, i, startRow, k2) * r;
                                // Console.WriteLine("初等倍加,第" + i.ToString() + "行+第" + startRow.ToString() + "行" + k2.ToString() + "：\n" + r.ToString());
                            }
                        }
                    }

                    startRow++;
                    if (startRow >= m)
                    {
                        break;
                    }
                }
                else
                {
                    bool isDone = false;
                    for (int i = startRow; i < m; i++)
                    {

                        if (r[i, j] != 0)
                        {
                            r = ElementaryMatrix(m, startRow, i) * r;    // 找到第一个元素不为0的行，进行交换
                            isDone = true;
                            break;
                        }
                    }
                    if (isDone)
                        j--;
                }
            }
            return r;
        }

        /// <summary>
        /// <see cref="LFMatrix"/>行列式
        /// </summary>
        /// <returns></returns>
        public double Determinant()
        {
            //行数等于列数
            if (RowCount == ColCount)
            {
                if (RowCount == 1)
                {
                    return _elements[0, 0];
                }
                else
                {
                    double r = 0;
                    // 按照行展开
                    for (int j = 0; j < ColCount; j++)
                    {
                        LFMatrix subMatrix = MinorMatrix(0, j);
                        r += _elements[0, j] * Math.Pow(-1, j + 1) * subMatrix.Determinant();
                    }
                    return r;
                }
            }
            return 0;
        }

        /// <summary>
        /// 矩阵的逆
        /// </summary>
        /// <returns></returns>
        public LFMatrix Inverse()
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1);

            if (m == n)
            {
                LFMatrix r = new LFMatrix(m, n);

                LFMatrix A = LFMatrix.AugmentedMatrix(this, LFMatrix.IdentityMatrix(m));   // 增广矩阵，一同进行初等行变换

                LFMatrix A1 = A.Simplify();

                int[] colIndex = new int[m];
                for (int i = 0; i < m; i++)
                {
                    colIndex[i] = m + i;
                }

                r = A1.ColSubMatrix(colIndex);

                return r;
            }
            return null;
        }

        #endregion

        #region 通用方法 General Methods

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int m = _elements.GetLength(0);
            int n = _elements.GetLength(1) - 1;

            var s = new StringBuilder();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    s.Append(_elements[i, j].ToString("g"));
                    s.Append("\t");
                }
                s.AppendLine(_elements[i, n].ToString("g"));
            }

            return s.ToString();
        }

        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
