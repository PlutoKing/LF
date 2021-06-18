/*──────────────────────────────────────────────────────────────
 * FileName     : LFMatrixStorage.cs
 * Created      : 2021-06-10 15:57:07
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.Mathematics.LinearAlgebra.Storage
{
    /// <summary>
    /// 矩阵内存
    /// </summary>
    public abstract partial class LFMatrixStorage<T> : IEquatable<LFMatrixStorage<T>>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields

        /// <summary>
        /// 0元素
        /// </summary>
        protected static readonly T Zero = LFBuilder<T>.Matrix.Zero;

        /// <summary>
        /// 矩阵行数
        /// </summary>
        public readonly int RowCount;

        /// <summary>
        /// 矩阵列数
        /// </summary>
        public readonly int ColumnCount;

        #endregion

        #region Properties

        /// <summary>
        /// 是否为稠密矩阵内存
        /// </summary>
        public abstract bool IsDense { get; }


        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <returns>第<paramref name="row"/>行，第<paramref name="column"/>列的矩阵元素</returns>
        public T this[int row,int column]
        {
            get
            {
                ValidateRange(row, column);

                return Get(row, column);
            }
            set
            {
                ValidateRange(row, column);

                Set(row, column, value);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 构造<paramref name="rows"/>行<paramref name="columns"/>列的矩阵内存
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        public LFMatrixStorage(int rows,int columns)
        {
            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), "矩阵的行数必须是非负的！");
            }

            if (columns < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), "矩阵的列数必须是非负的！");
            }
            RowCount = rows;
            ColumnCount = columns;
        }

        #endregion

        #region Methods

        #region Common Methods

        /// <summary>
        /// 按行索引<paramref name="row"/>和列索引<paramref name="column"/>获取矩阵元素值
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        /// <returns>元素值</returns>
        public abstract T Get(int row, int column);


        /// <summary>
        /// 按行索引<paramref name="row"/>和列索引<paramref name="column"/>给矩阵元素赋值
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        /// <param name="value">元素值</param>
        public abstract void Set(int row, int column, T value);

        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="other">另一个对象</param>
        /// <returns>相等，返回<c>true</c>；否则，返回<c>false</c></returns>
        public bool Equals(LFMatrixStorage<T> other)
        {
            if (other == null)
                return false;

            if (ColumnCount != other.ColumnCount || RowCount != other.RowCount)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            // 如果找不到不相等的元素，则返回true
            return Find2Unchecked(other, (a, b) => !a.Equals(b), ZerosOption.AllowSkip) == null;
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as LFMatrixStorage<T>);
        }


        /// <summary>
        /// 重写Equals后，必须重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashNum = Math.Min(RowCount * ColumnCount, 25);
            int hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++)
                {
                    int col;
                    int row = Math.DivRem(i, ColumnCount, out col);
                    hash = hash * 31 + Get(row, col).GetHashCode();
                }
            }
            return hash;
        }
        #endregion

        #region Clearing
        
        /// <summary>
        /// 将矩阵元素全部清零
        /// </summary>
        /// <remarks>该过程无需检测</remarks>
        public virtual void Clear()
        {
            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    Set(i, j, Zero);
                }
            }
        }

        /// <summary>
        /// 将子矩阵元素清零
        /// </summary>
        /// <remarks>该过程需要检测</remarks>
        /// <param name="rowIndex"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnIndex"></param>
        /// <param name="columnCount"></param>
        public void Clear(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            if (rowCount < 1 || columnCount < 1)
            {
                return;
            }

            if (rowIndex + rowCount > RowCount || rowIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }

            if (columnIndex + columnCount > ColumnCount || columnIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            ClearUnchecked(rowIndex, rowCount, columnIndex, columnCount);
        }

        /// <summary>
        /// 将子矩阵元素清零，但并未进行检测
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnIndex"></param>
        /// <param name="columnCount"></param>
        internal virtual void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            for (var i = rowIndex; i < rowIndex + rowCount; i++)
            {
                for (var j = columnIndex; j < columnIndex + columnCount; j++)
                {
                    Set(i, j, Zero);
                }
            }
        }

        /// <summary>
        /// 将某几列元素清零
        /// </summary>
        /// <param name="rowIndices"></param>
        public void ClearRows(int[] rowIndices)
        {
            if (rowIndices.Length == 0)
            {
                return;
            }

            for (int k = 0; k < rowIndices.Length; k++)
            {
                if (rowIndices[k] < 0 || rowIndices[k] >= RowCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(rowIndices));
                }
            }

            ClearRowsUnchecked(rowIndices);
        }

        public void ClearColumns(int[] columnIndices)
        {
            if (columnIndices.Length == 0)
            {
                return;
            }

            for (int k = 0; k < columnIndices.Length; k++)
            {
                if ((uint)columnIndices[k] >= (uint)ColumnCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndices));
                }
            }

            ClearColumnsUnchecked(columnIndices);
        }

        internal virtual void ClearRowsUnchecked(int[] rowIndices)
        {
            for (var k = 0; k < rowIndices.Length; k++)
            {
                int row = rowIndices[k];
                for (var j = 0; j < ColumnCount; j++)
                {
                    Set(row, j, Zero);
                }
            }
        }

        internal virtual void ClearColumnsUnchecked(int[] columnIndices)
        {
            for (var k = 0; k < columnIndices.Length; k++)
            {
                int column = columnIndices[k];
                for (var i = 0; i < RowCount; i++)
                {
                    Set(i, column, Zero);
                }
            }
        }
        #endregion

        #region Matrix Copy

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="target"></param>
        /// <param name="existingData"></param>
        public void CopyTo(LFMatrixStorage<T> target, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (ReferenceEquals(this, target))
            {
                return;
            }

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {target.RowCount}x{target.ColumnCount}.";
                throw new ArgumentException(message, nameof(target));
            }

            CopyToUnchecked(target, existingData);
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="target"></param>
        /// <param name="existingData"></param>
        internal virtual void CopyToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(i, j, Get(i, j));
                }
            }
        }

        public void CopySubMatrixTo(LFMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (rowCount == 0 || columnCount == 0)
            {
                return;
            }

            if (sourceRowIndex == 0 && targetRowIndex == 0 && rowCount == RowCount && rowCount == target.RowCount
                && sourceColumnIndex == 0 && targetColumnIndex == 0 && columnCount == ColumnCount && columnCount == target.ColumnCount)
            {
                CopyTo(target);
                return;
            }

            if (ReferenceEquals(this, target))
            {
                throw new NotSupportedException();
            }

            ValidateSubMatrixRange(target,
                sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount);

            CopySubMatrixToUnchecked(target, sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount, existingData);
        }

        internal virtual void CopySubMatrixToUnchecked(LFMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                {
                    target.Set(ii, jj, Get(i, j));
                }
            }
        }
        #endregion

        #region Row Copy and Column Copy
        // ROW COPY

        public void CopyRowTo(LFVectorStorage<T> target, int rowIndex, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            ValidateRowRange(target, rowIndex);
            CopySubRowToUnchecked(target, rowIndex, 0, 0, ColumnCount, existingData);
        }

        public void CopySubRowTo(LFVectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (columnCount == 0)
            {
                return;
            }

            ValidateSubRowRange(target, rowIndex, sourceColumnIndex, targetColumnIndex, columnCount);
            CopySubRowToUnchecked(target, rowIndex, sourceColumnIndex, targetColumnIndex, columnCount, existingData);
        }

        internal virtual void CopySubRowToUnchecked(LFVectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, DataClearOption existingData)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                target.Set(jj, Get(rowIndex, j));
            }
        }

        // COLUMN COPY

        public void CopyColumnTo(LFVectorStorage<T> target, int columnIndex, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            ValidateColumnRange(target, columnIndex);
            CopySubColumnToUnchecked(target, columnIndex, 0, 0, RowCount, existingData);
        }

        public void CopySubColumnTo(LFVectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (rowCount == 0)
            {
                return;
            }

            ValidateSubColumnRange(target, columnIndex, sourceRowIndex, targetRowIndex, rowCount);
            CopySubColumnToUnchecked(target, columnIndex, sourceRowIndex, targetRowIndex, rowCount, existingData);
        }

        internal virtual void CopySubColumnToUnchecked(LFVectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount, DataClearOption existingData)
        {
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
            {
                target.Set(ii, Get(i, columnIndex));
            }
        }
        #endregion

        #region Transpose
        // TRANSPOSE

        public void TransposeTo(LFMatrixStorage<T> target, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (RowCount != target.ColumnCount || ColumnCount != target.RowCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {target.RowCount}x{target.ColumnCount}.";
                throw new ArgumentException(message, nameof(target));
            }

            if (ReferenceEquals(this, target))
            {
                TransposeSquareInplaceUnchecked();
                return;
            }

            TransposeToUnchecked(target, existingData);
        }

        internal virtual void TransposeToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(j, i, Get(i, j));
                }
            }
        }

        internal virtual void TransposeSquareInplaceUnchecked()
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    T swap = Get(i, j);
                    Set(i, j, Get(j, i));
                    Set(j, i, swap);
                }
            }
        }
        #endregion

        #region Extract 提取
        // EXTRACT

        public virtual T[] ToRowMajorArray()
        {
            var ret = new T[RowCount * ColumnCount];
            for (int i = 0; i < RowCount; i++)
            {
                var offset = i * ColumnCount;
                for (int j = 0; j < ColumnCount; j++)
                {
                    ret[offset + j] = Get(i, j);
                }
            }
            return ret;
        }

        public virtual T[] ToColumnMajorArray()
        {
            var ret = new T[RowCount * ColumnCount];
            for (int j = 0; j < ColumnCount; j++)
            {
                var offset = j * RowCount;
                for (int i = 0; i < RowCount; i++)
                {
                    ret[offset + i] = Get(i, j);
                }
            }
            return ret;
        }

        public virtual T[][] ToRowArrays()
        {
            var ret = new T[RowCount][];
            for (int i = 0; i < RowCount; i++)
            {
                var row = new T[ColumnCount];
                for (int j = 0; j < ColumnCount; j++)
                {
                    row[j] = Get(i, j);
                }
                ret[i] = row;
            }
            return ret;
        }

        public virtual T[][] ToColumnArrays()
        {
            var ret = new T[ColumnCount][];
            for (int j = 0; j < ColumnCount; j++)
            {
                var column = new T[RowCount];
                for (int i = 0; i < RowCount; i++)
                {
                    column[i] = Get(i, j);
                }
                ret[j] = column;
            }
            return ret;
        }

        public virtual T[,] ToArray()
        {
            var ret = new T[RowCount, ColumnCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    ret[i, j] = Get(i, j);
                }
            }
            return ret;
        }

        public virtual T[] AsRowMajorArray()
        {
            return null;
        }

        public virtual T[] AsColumnMajorArray()
        {
            return null;
        }

        public virtual T[][] AsRowArrays()
        {
            return null;
        }

        public virtual T[][] AsColumnArrays()
        {
            return null;
        }

        public virtual T[,] AsArray()
        {
            return null;
        }
        #endregion

        #region Enumertion
        // ENUMERATION

        public virtual IEnumerable<T> Enumerate()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    yield return Get(i, j);
                }
            }
        }

        public virtual IEnumerable<Tuple<int, int, T>> EnumerateIndexed()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    yield return new Tuple<int, int, T>(i, j, Get(i, j));
                }
            }
        }

        public virtual IEnumerable<T> EnumerateNonZero()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var x = Get(i, j);
                    if (!Zero.Equals(x))
                    {
                        yield return x;
                    }
                }
            }
        }

        public virtual IEnumerable<Tuple<int, int, T>> EnumerateNonZeroIndexed()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var x = Get(i, j);
                    if (!Zero.Equals(x))
                    {
                        yield return new Tuple<int, int, T>(i, j, x);
                    }
                }
            }
        }
        #endregion

        #region Find
        // FIND

        public virtual Tuple<int, int, T> Find(Func<T, bool> predicate, ZerosOption zeros)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var item = Get(i, j);
                    if (predicate(item))
                    {
                        return new Tuple<int, int, T>(i, j, item);
                    }
                }
            }
            return null;
        }

        public Tuple<int, int, T, TOther> Find2<TOther>(LFMatrixStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {other.RowCount}x{other.ColumnCount}.";
                throw new ArgumentException(message, nameof(other));
            }

            return Find2Unchecked(other, predicate, zeros);
        }

        internal virtual Tuple<int, int, T, TOther> Find2Unchecked<TOther>(LFMatrixStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var item = Get(i, j);
                    var otherItem = other.Get(i, j);
                    if (predicate(item, otherItem))
                    {
                        return new Tuple<int, int, T, TOther>(i, j, item, otherItem);
                    }
                }
            }
            return null;
        }
        #endregion

        #region Functional Combinators: Map
        // FUNCTIONAL COMBINATORS: MAP

        public virtual void MapInplace(Func<T, T> f, ZerosOption zeros)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Set(i, j, f(Get(i, j)));
                }
            }
        }

        public virtual void MapIndexedInplace(Func<int, int, T, T> f, ZerosOption zeros)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Set(i, j, f(i, j, Get(i, j)));
                }
            }
        }

        public void MapTo<TU>(LFMatrixStorage<TU> target, Func<T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {target.RowCount}x{target.ColumnCount}.";
                throw new ArgumentException(message, nameof(target));
            }

            MapToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapToUnchecked<TU>(LFMatrixStorage<TU> target, Func<T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    target.Set(i, j, f(Get(i, j)));
                }
            }
        }

        public void MapIndexedTo<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {target.RowCount}x{target.ColumnCount}.";
                throw new ArgumentException(message, nameof(target));
            }

            MapIndexedToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(i, j, f(i, j, Get(i, j)));
                }
            }
        }

        public void MapSubMatrixIndexedTo<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (rowCount == 0 || columnCount == 0)
            {
                return;
            }

            if (ReferenceEquals(this, target))
            {
                throw new NotSupportedException();
            }

            ValidateSubMatrixRange(target,
                sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount);

            MapSubMatrixIndexedToUnchecked(target, f, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount, zeros, existingData);
        }

        internal virtual void MapSubMatrixIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                {
                    target.Set(ii, jj, f(ii, jj, Get(i, j)));
                }
            }
        }

        public void Map2To(LFMatrixStorage<T> target, LFMatrixStorage<T> other, Func<T, T, T> f, ZerosOption zeros, DataClearOption existingData)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {target.RowCount}x{target.ColumnCount}.";
                throw new ArgumentException(message, nameof(target));
            }

            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {other.RowCount}x{other.ColumnCount}.";
                throw new ArgumentException(message, nameof(other));
            }

            Map2ToUnchecked(target, other, f, zeros, existingData);
        }

        internal virtual void Map2ToUnchecked(LFMatrixStorage<T> target, LFMatrixStorage<T> other, Func<T, T, T> f, ZerosOption zeros, DataClearOption existingData)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    target.Set(i, j, f(Get(i, j), other.Get(i, j)));
                }
            }
        }

        #endregion

        #region Functional Combinators: Fold
        // FUNCTIONAL COMBINATORS: FOLD

        /// <remarks>The state array will not be modified, unless it is the same instance as the target array (which is allowed).</remarks>
        public void FoldByRow<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (target.Length != RowCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            if (state.Length != RowCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(state));
            }

            FoldByRowUnchecked(target, f, finalize, state, zeros);
        }

        /// <remarks>The state array will not be modified, unless it is the same instance as the target array (which is allowed).</remarks>
        internal virtual void FoldByRowUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            for (int i = 0; i < RowCount; i++)
            {
                TU s = state[i];
                for (int j = 0; j < ColumnCount; j++)
                {
                    s = f(s, Get(i, j));
                }
                target[i] = finalize(s, ColumnCount);
            }
        }

        /// <remarks>The state array will not be modified, unless it is the same instance as the target array (which is allowed).</remarks>
        public void FoldByColumn<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (target.Length != ColumnCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            if (state.Length != ColumnCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(state));
            }

            FoldByColumnUnchecked(target, f, finalize, state, zeros);
        }

        /// <remarks>The state array will not be modified, unless it is the same instance as the target array (which is allowed).</remarks>
        internal virtual void FoldByColumnUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                TU s = state[j];
                for (int i = 0; i < RowCount; i++)
                {
                    s = f(s, Get(i, j));
                }
                target[j] = finalize(s, RowCount);
            }
        }

        public TState Fold2<TOther, TState>(LFMatrixStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                var message = $"Matrix dimensions must agree: op1 is {RowCount}x{ColumnCount}, op2 is {other.RowCount}x{other.ColumnCount}.";
                throw new ArgumentException(message, nameof(other));
            }

            return Fold2Unchecked(other, f, state, zeros);
        }

        internal virtual TState Fold2Unchecked<TOther, TState>(LFMatrixStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    state = f(state, Get(i, j), other.Get(i, j));
                }
            }

            return state;
        }
        #endregion

        #endregion
    }

    public enum ZerosOption
    {
        /// <summary>
        /// 允许跳过0元素，可以加快处理稀疏矩阵的效率
        /// </summary>
        AllowSkip = 0,

        /// <summary>
        /// 禁止跳过0元素
        /// </summary>
        NoSkip = 1,
    }

    public enum DataClearOption
    {
        /// <summary>
        /// 清除所有数据
        /// </summary>
        Clear = 0,

        /// <summary>
        /// 跳过清除数据
        /// </summary>
        Skip = 1,
    }
}