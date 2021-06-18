/*──────────────────────────────────────────────────────────────
 * FileName     : LFDenseMatrixStorage.cs
 * Created      : 2021-06-10 16:35:19
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;
using System.Linq;

namespace LF.Mathematics.LinearAlgebra.Storage
{
    /// <summary>
    /// 稠密矩阵内存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class LFDenseMatrixStorage<T> : LFMatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields
        /// <summary>
        /// 矩阵数据
        /// </summary>
        [DataMember(Order = 1)]
        public readonly T[] Data;
        #endregion

        #region Properties
        public override bool IsDense => true;

        #endregion

        #region Constructors
        internal LFDenseMatrixStorage(int rows,int columns)
            :base(rows,columns)
        {
            Data = new T[rows * columns];
        }

        internal LFDenseMatrixStorage(int rows, int columns, T[] data)
           : base(rows, columns)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length != rows * columns)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {rows * columns}.");
            }

            Data = data;
        }


        #endregion

        #region Methods

        #region Common Methods
        /// <summary>
        /// Retrieves the requested element without range checking.
        /// </summary>
        public override T Get(int row, int column)
        {
            return Data[(column * RowCount) + row];
        }

        /// <summary>
        /// Sets the element without range checking.
        /// </summary>
        public override void Set(int row, int column, T value)
        {
            Data[(column * RowCount) + row] = value;
        }

        /// <summary>
        /// Evaluate the row and column at a specific data index.
        /// </summary>
        /// <remarks>计算特定数据索引处的行和列。</remarks>
        void RowColumnAtIndex(int index, out int row, out int column)
        {
            column = Math.DivRem(index, RowCount, out row);
        }

        #endregion

        #region Clearing Methods
        // CLEARING
        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        internal override void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            if (rowIndex == 0 && columnIndex == 0 && rowCount == RowCount && columnCount == ColumnCount)
            {
                Array.Clear(Data, 0, Data.Length);
                return;
            }

            for (int j = columnIndex; j < columnIndex + columnCount; j++)
            {
                Array.Clear(Data, j * RowCount + rowIndex, rowCount);
            }
        }

        internal override void ClearRowsUnchecked(int[] rowIndices)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                int offset = j * RowCount;
                for (var k = 0; k < rowIndices.Length; k++)
                {
                    Data[offset + rowIndices[k]] = Zero;
                }
            }
        }

        internal override void ClearColumnsUnchecked(int[] columnIndices)
        {
            for (int k = 0; k < columnIndices.Length; k++)
            {
                Array.Clear(Data, columnIndices[k] * RowCount, RowCount);
            }
        }
        #endregion


        #region Initialization Methods

        /// <summary>
        /// 从矩阵内存构建稠密矩阵内存
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromMatrix(LFMatrixStorage<T> matrix)
        {
            var storage = new LFDenseMatrixStorage<T>(matrix.RowCount, matrix.RowCount);
            matrix.CopyToUnchecked(storage, DataClearOption.Skip);
            return storage;
        }

        public static LFDenseMatrixStorage<T> FromValue(int rows, int columns, T value)
        {
            var storage = new LFDenseMatrixStorage<T>(rows, columns);
            var data = storage.Data;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = value;
            }
            return storage;
        }

        /// <summary>
        /// 根据索引位置构造稠密矩阵内存
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="indexfun">索引函数</param>
        /// <returns>稠密矩阵内存</returns>
        public static LFDenseMatrixStorage<T> FromIndex(int rows, int columns, Func<int, int, T> indexfun)
        {
            var storage = new LFDenseMatrixStorage<T>(rows, columns);
            int index = 0;
            for (var j = 0; j < columns; j++)
            {
                for (var i = 0; i < rows; i++)
                {
                    storage.Data[index++] = indexfun(i, j);
                }
            }
            return storage;
        }

        /// <summary>
        /// 根据对角索引构造
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="indexfun"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromDiagonalIndex(int rows, int columns, Func<int, T> indexfun)
        {
            var storage = new LFDenseMatrixStorage<T>(rows, columns);
            int index = 0;
            int stride = rows + 1;
            for (var i = 0; i < Math.Min(rows, columns); i++)
            {
                storage.Data[index] = indexfun(i);
                index += stride;
            }
            return storage;
        }

        /// <summary>
        /// 根据二维数组构造稠密矩阵内存
        /// </summary>
        /// <param name="array">二维数组</param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromArray(T[,] array)
        {
            var storage = new LFDenseMatrixStorage<T>(array.GetLength(0), array.GetLength(1));
            int index = 0;
            for (var j = 0; j < storage.ColumnCount; j++)
            {
                for (var i = 0; i < storage.RowCount; i++)
                {
                    storage.Data[index++] = array[i, j];
                }
            }
            return storage;
        }

        /// <summary>
        /// 根据列数组构造稠密矩阵内存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromColumnArrays(T[][] data)
        {
            if (data.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Matrices can not be empty and must have at least one row and column.");
            }

            int columns = data.Length;
            int rows = data[0].Length;
            var array = new T[rows * columns];
            for (int j = 0; j < data.Length; j++)
            {
                Array.Copy(data[j], 0, array, j * rows, rows);
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据行数组构造稠密矩阵内存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromRowArrays(T[][] data)
        {
            if (data.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Matrices can not be empty and must have at least one row and column.");
            }

            int rows = data.Length;
            int columns = data[0].Length;
            var array = new T[rows * columns];
            for (int j = 0; j < columns; j++)
            {
                int offset = j * rows;
                for (int i = 0; i < rows; i++)
                {
                    array[offset + i] = data[i][j];
                }
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据列主数组构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromColumnMajorArray(int rows, int columns, T[] data)
        {
            T[] ret = new T[rows * columns];
            Array.Copy(data, 0, ret, 0, Math.Min(ret.Length, data.Length));
            return new LFDenseMatrixStorage<T>(rows, columns, ret);
        }

        /// <summary>
        /// 根据行主数组构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromRowMajorArray(int rows, int columns, T[] data)
        {
            T[] ret = new T[rows * columns];
            for (int i = 0; i < rows; i++)
            {
                int offset = i * columns;
                for (int j = 0; j < columns; j++)
                {
                    ret[(j * rows) + i] = data[offset + j];
                }
            }
            return new LFDenseMatrixStorage<T>(rows, columns, ret);
        }

        /// <summary>
        /// 根据列向量数组构造稠密矩阵内存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromColumnVectors(LFVectorStorage<T>[] data)
        {
            if (data.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Matrices can not be empty and must have at least one row and column.");
            }

            int columns = data.Length;
            int rows = data[0].Length;
            var array = new T[rows * columns];
            for (int j = 0; j < data.Length; j++)
            {
                var column = data[j];
                if (column is LFDenseVectorStorage<T> denseColumn)
                {
                    Array.Copy(denseColumn.Data, 0, array, j * rows, rows);
                }
                else
                {
                    // FALL BACK
                    int offset = j * rows;
                    for (int i = 0; i < rows; i++)
                    {
                        array[offset + i] = column.Get(i);
                    }
                }
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据行向量数组构造稠密矩阵内存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromRowVectors(LFVectorStorage<T>[] data)
        {
            if (data.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Matrices can not be empty and must have at least one row and column.");
            }

            int rows = data.Length;
            int columns = data[0].Length;
            var array = new T[rows * columns];
            for (int j = 0; j < columns; j++)
            {
                int offset = j * rows;
                for (int i = 0; i < rows; i++)
                {
                    array[offset + i] = data[i].Get(j);
                }
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据枚举表构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromIndexedEnumerable(int rows, int columns, IEnumerable<Tuple<int, int, T>> data)
        {
            var array = new T[rows * columns];
            foreach (var item in data)
            {
                array[(item.Item2 * rows) + item.Item1] = item.Item3;
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据列主枚举表构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromColumnMajorEnumerable(int rows, int columns, IEnumerable<T> data)
        {
            if (data is T[] arrayData)
            {
                return FromColumnMajorArray(rows, columns, arrayData);
            }

            return new LFDenseMatrixStorage<T>(rows, columns, data.ToArray());
        }

        /// <summary>
        /// 根据行主枚举表构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromRowMajorEnumerable(int rows, int columns, IEnumerable<T> data)
        {
            return FromRowMajorArray(rows, columns, data as T[] ?? data.ToArray());
        }

        /// <summary>
        /// 根据列枚举表构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromColumnEnumerables(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            var array = new T[rows * columns];
            using (var columnIterator = data.GetEnumerator())
            {
                for (int column = 0; column < columns; column++)
                {
                    if (!columnIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {columns}.");
                    if (columnIterator.Current is T[] arrayColumn)
                    {
                        Array.Copy(arrayColumn, 0, array, column * rows, rows);
                    }
                    else
                    {
                        using (var rowIterator = columnIterator.Current.GetEnumerator())
                        {
                            var end = (column + 1) * rows;
                            for (int index = column * rows; index < end; index++)
                            {
                                if (!rowIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {rows}.");
                                array[index] = rowIterator.Current;
                            }
                            if (rowIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {rows}.");
                        }
                    }
                }
                if (columnIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {columns}.");
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }

        /// <summary>
        /// 根据行枚举表构造稠密矩阵内存
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LFDenseMatrixStorage<T> FromRowEnumerables(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            var array = new T[rows * columns];
            using (var rowIterator = data.GetEnumerator())
            {
                for (int row = 0; row < rows; row++)
                {
                    if (!rowIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {rows}.");
                    using (var columnIterator = rowIterator.Current.GetEnumerator())
                    {
                        for (int index = row; index < array.Length; index += rows)
                        {
                            if (!columnIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {columns}.");
                            array[index] = columnIterator.Current;
                        }
                        if (columnIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {columns}.");
                    }
                }
                if (rowIterator.MoveNext()) throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {rows}.");
            }
            return new LFDenseMatrixStorage<T>(rows, columns, array);
        }
        #endregion

        #region Copy
        // MATRIX COPY

        internal override void CopyToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<T> denseTarget)
            {
                CopyToUnchecked(denseTarget);
                return;
            }

            // FALL BACK

            for (int j = 0, offset = 0; j < ColumnCount; j++, offset += RowCount)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(i, j, Data[i + offset]);
                }
            }
        }

        void CopyToUnchecked(LFDenseMatrixStorage<T> target)
        {
            //Buffer.BlockCopy(Data, 0, target.Data, 0, Data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
            Array.Copy(Data, 0, target.Data, 0, Data.Length);
        }

        internal override void CopySubMatrixToUnchecked(LFMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<T> denseTarget)
            {
                CopySubMatrixToUnchecked(denseTarget, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                int index = sourceRowIndex + j * RowCount;
                for (int ii = targetRowIndex; ii < targetRowIndex + rowCount; ii++)
                {
                    target.Set(ii, jj, Data[index++]);
                }
            }
        }

        void CopySubMatrixToUnchecked(LFDenseMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                //Buffer.BlockCopy(Data, j*RowCount + sourceRowIndex, target.Data, jj*target.RowCount + targetRowIndex, rowCount * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
                Array.Copy(Data, j * RowCount + sourceRowIndex, target.Data, jj * target.RowCount + targetRowIndex, rowCount);
            }
        }

        // ROW COPY

        internal override void CopySubRowToUnchecked(LFVectorStorage<T> target, int rowIndex, int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData)
        {
            if (target is LFDenseVectorStorage<T> targetDense)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    targetDense.Data[j + targetColumnIndex] = Data[(j + sourceColumnIndex) * RowCount + rowIndex];
                }
                return;
            }

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                target.Set(jj, Data[(j * RowCount) + rowIndex]);
            }
        }

        // COLUMN COPY

        internal override void CopySubColumnToUnchecked(LFVectorStorage<T> target, int columnIndex, int sourceRowIndex, int targetRowIndex, int rowCount,
            DataClearOption existingData)
        {
            if (target is LFDenseVectorStorage<T> targetDense)
            {
                Array.Copy(Data, columnIndex * RowCount + sourceRowIndex, targetDense.Data, targetRowIndex, rowCount);
                return;
            }

            // FALL BACK

            var offset = columnIndex * RowCount;
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
            {
                target.Set(ii, Data[offset + i]);
            }
        }
        #endregion

        #region Transpose
        // TRANSPOSE

        internal override void TransposeToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<T> denseTarget)
            {
                TransposeToUnchecked(denseTarget);
                return;
            }

            if (target is LFSparseMatrixStorage<T> sparseTarget)
            {
                TransposeToUnchecked(sparseTarget);
                return;
            }

            // FALL BACK

            for (int j = 0, offset = 0; j < ColumnCount; j++, offset += RowCount)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(j, i, Data[i + offset]);
                }
            }
        }

        void TransposeToUnchecked(LFDenseMatrixStorage<T> target)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                var index = j * RowCount;
                for (var i = 0; i < RowCount; i++)
                {
                    target.Data[(i * ColumnCount) + j] = Data[index + i];
                }
            }
        }

        void TransposeToUnchecked(LFSparseMatrixStorage<T> target)
        {
            var rowPointers = target.RowPointers;
            var columnIndices = new List<int>();
            var values = new List<T>();

            for (int j = 0; j < ColumnCount; j++)
            {
                rowPointers[j] = values.Count;
                var index = j * RowCount;
                for (int i = 0; i < RowCount; i++)
                {
                    if (!Zero.Equals(Data[index + i]))
                    {
                        values.Add(Data[index + i]);
                        columnIndices.Add(i);
                    }
                }
            }

            rowPointers[ColumnCount] = values.Count;
            target.ColumnIndices = columnIndices.ToArray();
            target.Values = values.ToArray();
        }

        internal override void TransposeSquareInplaceUnchecked()
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                var index = j * RowCount;
                for (var i = 0; i < j; i++)
                {
                    T swap = Data[index + i];
                    Data[index + i] = Data[i * ColumnCount + j];
                    Data[i * ColumnCount + j] = swap;
                }
            }
        }
        #endregion

        #region Extract
        // EXTRACT

        public override T[] ToRowMajorArray()
        {
            var ret = new T[Data.Length];
            for (int i = 0; i < RowCount; i++)
            {
                var offset = i * ColumnCount;
                for (int j = 0; j < ColumnCount; j++)
                {
                    ret[offset + j] = Data[(j * RowCount) + i];
                }
            }
            return ret;
        }

        public override T[] ToColumnMajorArray()
        {
            var ret = new T[Data.Length];
            Array.Copy(Data, 0, ret, 0, Data.Length);
            return ret;
        }

        public override T[][] ToRowArrays()
        {
            var ret = new T[RowCount][];
            for (int i = 0; i < RowCount; i++)
            {
                var row = new T[ColumnCount];
                for (int j = 0; j < ColumnCount; j++)
                {
                    row[j] = Data[j * RowCount + i];
                }
                ret[i] = row;
            }
            return ret;
        }

        public override T[][] ToColumnArrays()
        {
            var ret = new T[ColumnCount][];
            for (int j = 0; j < ColumnCount; j++)
            {
                var column = new T[RowCount];
                Array.Copy(Data, j * RowCount, column, 0, RowCount);
                ret[j] = column;
            }
            return ret;
        }

        public override T[,] ToArray()
        {
            var ret = new T[RowCount, ColumnCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    ret[i, j] = Data[(j * RowCount) + i];
                }
            }
            return ret;
        }

        public override T[] AsColumnMajorArray()
        {
            return Data;
        }
        #endregion

        #region Enumeration
        // ENUMERATION

        public override IEnumerable<T> Enumerate()
        {
            return Data;
        }

        public override IEnumerable<Tuple<int, int, T>> EnumerateIndexed()
        {
            int index = 0;
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    yield return new Tuple<int, int, T>(i, j, Data[index]);
                    index++;
                }
            }
        }

        public override IEnumerable<T> EnumerateNonZero()
        {
            return Data.Where(x => !Zero.Equals(x));
        }

        public override IEnumerable<Tuple<int, int, T>> EnumerateNonZeroIndexed()
        {
            int index = 0;
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    var x = Data[index];
                    if (!Zero.Equals(x))
                    {
                        yield return new Tuple<int, int, T>(i, j, x);
                    }
                    index++;
                }
            }
        }
        #endregion

        #region Find
        // FIND

        public override Tuple<int, int, T> Find(Func<T, bool> predicate, ZerosOption zeros)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (predicate(Data[i]))
                {
                    int row, column;
                    RowColumnAtIndex(i, out row, out column);
                    return new Tuple<int, int, T>(row, column, Data[i]);
                }
            }
            return null;
        }

        internal override Tuple<int, int, T, TOther> Find2Unchecked<TOther>(LFMatrixStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
        {
            if (other is LFDenseMatrixStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;
                for (int i = 0; i < Data.Length; i++)
                {
                    if (predicate(Data[i], otherData[i]))
                    {
                        int row, column;
                        RowColumnAtIndex(i, out row, out column);
                        return new Tuple<int, int, T, TOther>(row, column, Data[i], otherData[i]);

                    }
                }
                return null;
            }

            if (other is LFDiagonalMatrixStorage<TOther> diagonalOther)
            {
                TOther[] otherData = diagonalOther.Data;
                TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                int k = 0;
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        if (predicate(Data[k], i == j ? otherData[i] : otherZero))
                        {
                            return new Tuple<int, int, T, TOther>(i, j, Data[k], i == j ? otherData[i] : otherZero);
                        }
                        k++;
                    }
                }
                return null;
            }

            if (other is LFSparseMatrixStorage<TOther> sparseOther)
            {
                int[] otherRowPointers = sparseOther.RowPointers;
                int[] otherColumnIndices = sparseOther.ColumnIndices;
                TOther[] otherValues = sparseOther.Values;
                TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                int k = 0;
                for (int row = 0; row < RowCount; row++)
                {
                    for (int col = 0; col < ColumnCount; col++)
                    {
                        if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                        {
                            if (predicate(Data[col * RowCount + row], otherValues[k]))
                            {
                                return new Tuple<int, int, T, TOther>(row, col, Data[col * RowCount + row], otherValues[k]);
                            }
                            k++;
                        }
                        else
                        {
                            if (predicate(Data[col * RowCount + row], otherZero))
                            {
                                return new Tuple<int, int, T, TOther>(row, col, Data[col * RowCount + row], otherValues[k]);
                            }
                        }
                    }
                }
                return null;
            }

            // FALL BACK

            return base.Find2Unchecked(other, predicate, zeros);
        }
        #endregion

        #region Functional Combinators: Map
        // FUNCTIONAL COMBINATORS: MAP

        public override void MapInplace(Func<T, T> f, ZerosOption zeros)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = f(Data[i]);
            }
        }

        public override void MapIndexedInplace(Func<int, int, T, T> f, ZerosOption zeros)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                int index = j * RowCount;
                for (int i = 0; i < RowCount; i++)
                {
                    Data[index] = f(i, j, Data[index]);
                    index++;
                }
            }
        }

        internal override void MapToUnchecked<TU>(LFMatrixStorage<TU> target, Func<T, TU> f,
            ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<TU> denseTarget)
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    denseTarget.Data[i] = f(Data[i]);
                }
                return;
            }

            // FALL BACK

            int index = 0;
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(i, j, f(Data[index++]));
                }
            }
        }

        internal override void MapIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<TU> denseTarget)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    int index = j * RowCount;
                    for (int i = 0; i < RowCount; i++)
                    {
                        denseTarget.Data[index] = f(i, j, Data[index]);
                        index++;
                    }
                }
                return;
            }

            // FALL BACK

            int index2 = 0;
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    target.Set(i, j, f(i, j, Data[index2++]));
                }
            }
        }

        internal override void MapSubMatrixIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<TU> denseTarget)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    int sourceIndex = sourceRowIndex + (j + sourceColumnIndex) * RowCount;
                    int targetIndex = targetRowIndex + (j + targetColumnIndex) * target.RowCount;
                    for (int i = 0; i < rowCount; i++)
                    {
                        denseTarget.Data[targetIndex++] = f(targetRowIndex + i, targetColumnIndex + j, Data[sourceIndex++]);
                    }
                }
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                int index = sourceRowIndex + j * RowCount;
                for (int ii = targetRowIndex; ii < targetRowIndex + rowCount; ii++)
                {
                    target.Set(ii, jj, f(ii, jj, Data[index++]));
                }
            }
        }
        #endregion

        #region Functional Combinators: Fold
        // FUNCTIONAL COMBINATORS: FOLD

        internal override void FoldByRowUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            for (int i = 0; i < RowCount; i++)
            {
                TU s = state[i];
                for (int j = 0; j < ColumnCount; j++)
                {
                    s = f(s, Data[j * RowCount + i]);
                }
                target[i] = finalize(s, ColumnCount);
            }
        }

        internal override void FoldByColumnUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                int offset = j * RowCount;
                TU s = state[j];
                for (int i = 0; i < RowCount; i++)
                {
                    s = f(s, Data[offset + i]);
                }
                target[j] = finalize(s, RowCount);
            }
        }

        internal override TState Fold2Unchecked<TOther, TState>(LFMatrixStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
        {
            if (other is LFDenseMatrixStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;
                for (int i = 0; i < Data.Length; i++)
                {
                    state = f(state, Data[i], otherData[i]);
                }
                return state;
            }

            if (other is LFDiagonalMatrixStorage<TOther> diagonalOther)
            {
                TOther[] otherData = diagonalOther.Data;
                TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                int k = 0;
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        state = f(state, Data[k], i == j ? otherData[i] : otherZero);
                        k++;
                    }
                }
                return state;
            }

            if (other is LFSparseMatrixStorage<TOther> sparseOther)
            {
                int[] otherRowPointers = sparseOther.RowPointers;
                int[] otherColumnIndices = sparseOther.ColumnIndices;
                TOther[] otherValues = sparseOther.Values;
                TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                int k = 0;
                for (int row = 0; row < RowCount; row++)
                {
                    for (int col = 0; col < ColumnCount; col++)
                    {
                        if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                        {
                            state = f(state, Data[col * RowCount + row], otherValues[k++]);
                        }
                        else
                        {
                            state = f(state, Data[col * RowCount + row], otherZero);
                        }
                    }
                }
                return state;
            }

            // FALL BACK

            return base.Fold2Unchecked(other, f, state, zeros);
        }

        #endregion

        #endregion
    }
}