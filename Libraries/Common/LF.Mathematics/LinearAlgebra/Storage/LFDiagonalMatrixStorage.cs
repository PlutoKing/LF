/*──────────────────────────────────────────────────────────────
 * FileName     : LFDiagonalMatrixStorage.cs
 * Created      : 2021-06-10 19:43:44
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Linq;
using System.Runtime.Serialization;

namespace LF.Mathematics.LinearAlgebra.Storage
{
    [Serializable]
    public class LFDiagonalMatrixStorage<T> : LFMatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields
        [DataMember(Order = 1)]
        public readonly T[] Data;

        #endregion

        #region Properties
        public override bool IsDense => false;

        #endregion

        #region Constructors
        internal LFDiagonalMatrixStorage(int rows, int columns)
            : base(rows, columns)
        {
            Data = new T[Math.Min(rows, columns)];
        }

        internal LFDiagonalMatrixStorage(int rows, int columns, T[] data)
            : base(rows, columns)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length != Math.Min(rows, columns))
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {Math.Min(rows, columns)}.");
            }

            Data = data;
        }
        #endregion

        #region Methods
        #region Common Methos
        /// <summary>
        /// Retrieves the requested element without range checking.
        /// </summary>
        public override T Get(int row, int column)
        {
            return row == column ? Data[row] : Zero;
        }

        /// <summary>
        /// Sets the element without range checking.
        /// </summary>
        public override void Set(int row, int column, T value)
        {
            if (row == column)
            {
                Data[row] = value;
            }
            else if (!Zero.Equals(value))
            {
                throw new IndexOutOfRangeException("Cannot set an off-diagonal element in a diagonal matrix.");
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var hashNum = Math.Min(Data.Length, 25);
            int hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++)
                {
                    hash = hash * 31 + Data[i].GetHashCode();
                }
            }
            return hash;
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
            var beginInclusive = Math.Max(rowIndex, columnIndex);
            var endExclusive = Math.Min(rowIndex + rowCount, columnIndex + columnCount);
            if (endExclusive > beginInclusive)
            {
                Array.Clear(Data, beginInclusive, endExclusive - beginInclusive);
            }
        }

        internal override void ClearRowsUnchecked(int[] rowIndices)
        {
            for (int i = 0; i < rowIndices.Length; i++)
            {
                Data[rowIndices[i]] = Zero;
            }
        }

        internal override void ClearColumnsUnchecked(int[] columnIndices)
        {
            for (int i = 0; i < columnIndices.Length; i++)
            {
                Data[columnIndices[i]] = Zero;
            }
        }
        #endregion

        #region Initialization

        // INITIALIZATION

        public static LFDiagonalMatrixStorage<T> OfMatrix(LFMatrixStorage<T> matrix)
        {
            var storage = new LFDiagonalMatrixStorage<T>(matrix.RowCount, matrix.ColumnCount);
            matrix.CopyToUnchecked(storage, DataClearOption.Skip);
            return storage;
        }

        public static LFDiagonalMatrixStorage<T> OfArray(T[,] array)
        {
            var storage = new LFDiagonalMatrixStorage<T>(array.GetLength(0), array.GetLength(1));
            for (var i = 0; i < storage.RowCount; i++)
            {
                for (var j = 0; j < storage.ColumnCount; j++)
                {
                    if (i == j)
                    {
                        storage.Data[i] = array[i, j];
                    }
                    else if (!Zero.Equals(array[i, j]))
                    {
                        throw new ArgumentException("Cannot set an off-diagonal element in a diagonal matrix.");
                    }
                }
            }
            return storage;
        }

        public static LFDiagonalMatrixStorage<T> OfValue(int rows, int columns, T diagonalValue)
        {
            var storage = new LFDiagonalMatrixStorage<T>(rows, columns);
            for (var i = 0; i < storage.Data.Length; i++)
            {
                storage.Data[i] = diagonalValue;
            }
            return storage;
        }

        public static LFDiagonalMatrixStorage<T> OfInit(int rows, int columns, Func<int, T> init)
        {
            var storage = new LFDiagonalMatrixStorage<T>(rows, columns);
            for (var i = 0; i < storage.Data.Length; i++)
            {
                storage.Data[i] = init(i);
            }
            return storage;
        }

        public static LFDiagonalMatrixStorage<T> OfEnumerable(int rows, int columns, IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data is T[] arrayData)
            {
                var copy = new T[arrayData.Length];
                Array.Copy(arrayData, 0, copy, 0, arrayData.Length);
                return new LFDiagonalMatrixStorage<T>(rows, columns, copy);
            }

            return new LFDiagonalMatrixStorage<T>(rows, columns, data.ToArray());
        }

        public static LFDiagonalMatrixStorage<T> OfIndexedEnumerable(int rows, int columns, IEnumerable<Tuple<int, T>> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var storage = new LFDiagonalMatrixStorage<T>(rows, columns);
            foreach (var item in data)
            {
                storage.Data[item.Item1] = item.Item2;
            }
            return storage;
        }
        #endregion

        #region Copy Methods
        // MATRIX COPY

        internal override void CopyToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            if (target is LFDiagonalMatrixStorage<T> diagonalTarget)
            {
                CopyToUnchecked(diagonalTarget);
                return;
            }

            if (target is LFDenseMatrixStorage<T> denseTarget)
            {
                CopyToUnchecked(denseTarget, existingData);
                return;
            }

            if (target is LFSparseMatrixStorage<T> sparseTarget)
            {
                CopyToUnchecked(sparseTarget, existingData);
                return;
            }

            // FALL BACK

            if (existingData == DataClearOption.Clear)
            {
                target.Clear();
            }

            for (int i = 0; i < Data.Length; i++)
            {
                target.Set(i, i, Data[i]);
            }
        }

        void CopyToUnchecked(LFDiagonalMatrixStorage<T> target)
        {
            //Buffer.BlockCopy(Data, 0, target.Data, 0, Data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
            Array.Copy(Data, 0, target.Data, 0, Data.Length);
        }

        void CopyToUnchecked(LFSparseMatrixStorage<T> target, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.Clear();
            }

            for (int i = 0; i < Data.Length; i++)
            {
                target.Set(i, i, Data[i]);
            }
        }

        void CopyToUnchecked(LFDenseMatrixStorage<T> target, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.Clear();
            }

            for (int i = 0; i < Data.Length; i++)
            {
                target.Data[i * (target.RowCount + 1)] = Data[i];
            }
        }

        internal override void CopySubMatrixToUnchecked(LFMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData)
        {
            if (target is LFDenseMatrixStorage<T> denseTarget)
            {
                CopySubMatrixToUnchecked(denseTarget, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount, existingData);
                return;
            }

            if (target is LFDiagonalMatrixStorage<T> diagonalTarget)
            {
                CopySubMatrixToUnchecked(diagonalTarget, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            if (existingData == DataClearOption.Clear)
            {
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
            }

            if (sourceRowIndex == sourceColumnIndex)
            {
                for (var i = 0; i < Math.Min(columnCount, rowCount); i++)
                {
                    target.Set(targetRowIndex + i, targetColumnIndex + i, Data[sourceRowIndex + i]);
                }
            }
            else if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning
                int columnInit = sourceRowIndex - sourceColumnIndex;
                for (var i = 0; i < Math.Min(columnCount - columnInit, rowCount); i++)
                {
                    target.Set(targetRowIndex + i, columnInit + targetColumnIndex + i, Data[sourceRowIndex + i]);
                }
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning
                int rowInit = sourceColumnIndex - sourceRowIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount - rowInit); i++)
                {
                    target.Set(rowInit + targetRowIndex + i, targetColumnIndex + i, Data[sourceColumnIndex + i]);
                }
            }
        }

        void CopySubMatrixToUnchecked(LFDiagonalMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
        {
            if (sourceRowIndex - sourceColumnIndex != targetRowIndex - targetColumnIndex)
            {
                if (Data.Any(x => !Zero.Equals(x)))
                {
                    throw new NotSupportedException();
                }

                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
                return;
            }

            var beginInclusive = Math.Max(sourceRowIndex, sourceColumnIndex);
            var endExclusive = Math.Min(sourceRowIndex + rowCount, sourceColumnIndex + columnCount);
            if (endExclusive > beginInclusive)
            {
                var beginTarget = Math.Max(targetRowIndex, targetColumnIndex);
                Array.Copy(Data, beginInclusive, target.Data, beginTarget, endExclusive - beginInclusive);
            }
        }

        void CopySubMatrixToUnchecked(LFDenseMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
            }

            if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning

                int columnInit = sourceRowIndex - sourceColumnIndex;
                int offset = (columnInit + targetColumnIndex) * target.RowCount + targetRowIndex;
                int step = target.RowCount + 1;
                int end = Math.Min(columnCount - columnInit, rowCount) + sourceRowIndex;

                for (int i = sourceRowIndex, j = offset; i < end; i++, j += step)
                {
                    target.Data[j] = Data[i];
                }
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning

                int rowInit = sourceColumnIndex - sourceRowIndex;
                int offset = targetColumnIndex * target.RowCount + rowInit + targetRowIndex;
                int step = target.RowCount + 1;
                int end = Math.Min(columnCount, rowCount - rowInit) + sourceColumnIndex;

                for (int i = sourceColumnIndex, j = offset; i < end; i++, j += step)
                {
                    target.Data[j] = Data[i];
                }
            }
            else
            {
                int offset = targetColumnIndex * target.RowCount + targetRowIndex;
                int step = target.RowCount + 1;
                var end = Math.Min(columnCount, rowCount) + sourceRowIndex;

                for (int i = sourceRowIndex, j = offset; i < end; i++, j += step)
                {
                    target.Data[j] = Data[i];
                }
            }
        }

        // ROW COPY

        internal override void CopySubRowToUnchecked(LFVectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.Clear(targetColumnIndex, columnCount);
            }

            if (rowIndex >= sourceColumnIndex && rowIndex < sourceColumnIndex + columnCount && rowIndex < Data.Length)
            {
                target.Set(rowIndex - sourceColumnIndex + targetColumnIndex, Data[rowIndex]);
            }
        }

        // COLUMN COPY

        internal override void CopySubColumnToUnchecked(LFVectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.Clear(targetRowIndex, rowCount);
            }

            if (columnIndex >= sourceRowIndex && columnIndex < sourceRowIndex + rowCount && columnIndex < Data.Length)
            {
                target.Set(columnIndex - sourceRowIndex + targetRowIndex, Data[columnIndex]);
            }
        }
        #endregion

        #region Transpose

        // TRANSPOSE

        internal override void TransposeToUnchecked(LFMatrixStorage<T> target, DataClearOption existingData)
        {
            CopyToUnchecked(target, existingData);
        }

        internal override void TransposeSquareInplaceUnchecked()
        {
            // nothing to do
        }
        #endregion

        #region Extract
        // EXTRACT

        public override T[] ToRowMajorArray()
        {
            var ret = new T[RowCount * ColumnCount];
            var stride = ColumnCount + 1;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i * stride] = Data[i];
            }
            return ret;
        }

        public override T[] ToColumnMajorArray()
        {
            var ret = new T[RowCount * ColumnCount];
            var stride = RowCount + 1;
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i * stride] = Data[i];
            }
            return ret;
        }

        public override T[][] ToRowArrays()
        {
            var ret = new T[RowCount][];
            for (int i = 0; i < RowCount; i++)
            {
                ret[i] = new T[ColumnCount];
            }
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i][i] = Data[i];
            }
            return ret;
        }

        public override T[][] ToColumnArrays()
        {
            var ret = new T[ColumnCount][];
            for (int j = 0; j < ColumnCount; j++)
            {
                ret[j] = new T[RowCount];
            }
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i][i] = Data[i];
            }
            return ret;
        }

        public override T[,] ToArray()
        {
            var ret = new T[RowCount, ColumnCount];
            for (int i = 0; i < Data.Length; i++)
            {
                ret[i, i] = Data[i];
            }
            return ret;
        }
        #endregion

        #region Enumeration
        // ENUMERATION

        public override IEnumerable<T> Enumerate()
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    // PERF: consider to break up loop to avoid branching
                    yield return i == j ? Data[i] : Zero;
                }
            }
        }

        public override IEnumerable<Tuple<int, int, T>> EnumerateIndexed()
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    // PERF: consider to break up loop to avoid branching
                    yield return i == j
                        ? new Tuple<int, int, T>(i, i, Data[i])
                        : new Tuple<int, int, T>(i, j, Zero);
                }
            }
        }

        public override IEnumerable<T> EnumerateNonZero()
        {
            return Data.Where(x => !Zero.Equals(x));
        }

        public override IEnumerable<Tuple<int, int, T>> EnumerateNonZeroIndexed()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (!Zero.Equals(Data[i]))
                {
                    yield return new Tuple<int, int, T>(i, i, Data[i]);
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
                    return new Tuple<int, int, T>(i, i, Data[i]);
                }
            }
            if (zeros == ZerosOption.NoSkip && (RowCount > 1 || ColumnCount > 1))
            {
                if (predicate(Zero))
                {
                    return new Tuple<int, int, T>(RowCount > 1 ? 1 : 0, RowCount > 1 ? 0 : 1, Zero);
                }
            }
            return null;
        }

        internal override Tuple<int, int, T, TOther> Find2Unchecked<TOther>(LFMatrixStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
        {
            if (other is LFDenseMatrixStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;
                int k = 0;
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        if (predicate(i == j ? Data[i] : Zero, otherData[k]))
                        {
                            return new Tuple<int, int, T, TOther>(i, j, i == j ? Data[i] : Zero, otherData[k]);
                        }
                        k++;
                    }
                }
                return null;
            }

            if (other is LFDiagonalMatrixStorage<TOther> diagonalOther)
            {
                TOther[] otherData = diagonalOther.Data;
                for (int i = 0; i < Data.Length; i++)
                {
                    if (predicate(Data[i], otherData[i]))
                    {
                        return new Tuple<int, int, T, TOther>(i, i, Data[i], otherData[i]);
                    }
                }
                if (zeros == ZerosOption.NoSkip && (RowCount > 1 || ColumnCount > 1))
                {
                    TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                    if (predicate(Zero, otherZero))
                    {
                        return new Tuple<int, int, T, TOther>(RowCount > 1 ? 1 : 0, RowCount > 1 ? 0 : 1, Zero, otherZero);
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
                for (int row = 0; row < RowCount; row++)
                {
                    bool diagonal = false;
                    var startIndex = otherRowPointers[row];
                    var endIndex = otherRowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                    {
                        if (otherColumnIndices[j] == row)
                        {
                            diagonal = true;
                            if (predicate(Data[row], otherValues[j]))
                            {
                                return new Tuple<int, int, T, TOther>(row, row, Data[row], otherValues[j]);
                            }
                        }
                        else
                        {
                            if (predicate(Zero, otherValues[j]))
                            {
                                return new Tuple<int, int, T, TOther>(row, otherColumnIndices[j], Zero, otherValues[j]);
                            }
                        }
                    }
                    if (!diagonal && row < ColumnCount)
                    {
                        if (predicate(Data[row], otherZero))
                        {
                            return new Tuple<int, int, T, TOther>(row, row, Data[row], otherZero);
                        }
                    }
                }
                if (zeros == ZerosOption.NoSkip && sparseOther.ValueCount < (RowCount * ColumnCount))
                {
                    if (predicate(Zero, otherZero))
                    {
                        int k = 0;
                        for (int row = 0; row < RowCount; row++)
                        {
                            for (int col = 0; col < ColumnCount; col++)
                            {
                                if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                                {
                                    k++;
                                }
                                else if (row != col)
                                {
                                    return new Tuple<int, int, T, TOther>(row, col, Zero, otherZero);
                                }
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

        #region Funcational Combinators: Map
        // FUNCTIONAL COMBINATORS: MAP

        public override void MapInplace(Func<T, T> f, ZerosOption zeros)
        {
            if (zeros == ZerosOption.NoSkip)
            {
                throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
            }

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = f(Data[i]);
            }
        }

        public override void MapIndexedInplace(Func<int, int, T, T> f, ZerosOption zeros)
        {
            if (zeros == ZerosOption.NoSkip)
            {
                throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
            }

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = f(i, i, Data[i]);
            }
        }

        internal override void MapToUnchecked<TU>(LFMatrixStorage<TU> target, Func<T, TU> f,
            ZerosOption zeros, DataClearOption existingData)
        {
            var processZeros = zeros == ZerosOption.NoSkip || !Zero.Equals(f(Zero));

            if (target is LFDiagonalMatrixStorage<TU> diagonalTarget)
            {
                if (processZeros)
                {
                    throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
                }

                for (int i = 0; i <Data.Length; i++)
                {
                    diagonalTarget.Data[i] = f(Data[i]);
                }
                return;
            }

            // FALL BACK

            if (existingData == DataClearOption.Clear && !processZeros)
            {
                target.Clear();
            }

            if (processZeros)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        target.Set(i, j, f(i == j ? Data[i] : Zero));
                    }
                }
            }
            else
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    target.Set(i, i, f(Data[i]));
                }
            }
        }

        internal override void MapIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            ZerosOption zeros, DataClearOption existingData)
        {
            var processZeros = zeros == ZerosOption.NoSkip || !Zero.Equals(f(0, 1, Zero));

            if (target is LFDiagonalMatrixStorage<TU> diagonalTarget)
            {
                if (processZeros)
                {
                    throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
                }

                for (int i = 0; i < Data.Length; i++)
                {
                    diagonalTarget.Data[i] = f(i, i, Data[i]);
                }
                return;
            }

            // FALL BACK

            if (existingData == DataClearOption.Clear && !processZeros)
            {
                target.Clear();
            }

            if (processZeros)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        target.Set(i, j, f(i, j, i == j ? Data[i] : Zero));
                    }
                }
            }
            else
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    target.Set(i, i, f(i, i, Data[i]));
                }
            }
        }

        internal override void MapSubMatrixIndexedToUnchecked<TU>(LFMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFDiagonalMatrixStorage<TU> diagonalTarget)
            {
                MapSubMatrixIndexedToUnchecked(diagonalTarget, f, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount, zeros);
                return;
            }

            if (target is LFDenseMatrixStorage<TU> denseTarget)
            {
                MapSubMatrixIndexedToUnchecked(denseTarget, f, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex, targetColumnIndex, columnCount, zeros, existingData);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            if (existingData == DataClearOption.Clear)
            {
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
            }

            if (sourceRowIndex == sourceColumnIndex)
            {
                int targetRow = targetRowIndex;
                int targetColumn = targetColumnIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount); i++)
                {
                    target.Set(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceRowIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
            else if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning
                int columnInit = sourceRowIndex - sourceColumnIndex;
                int targetRow = targetRowIndex;
                int targetColumn = targetColumnIndex + columnInit;
                for (var i = 0; i < Math.Min(columnCount - columnInit, rowCount); i++)
                {
                    target.Set(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceRowIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning
                int rowInit = sourceColumnIndex - sourceRowIndex;
                int targetRow = targetRowIndex + rowInit;
                int targetColumn = targetColumnIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount - rowInit); i++)
                {
                    target.Set(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceColumnIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
        }

        void MapSubMatrixIndexedToUnchecked<TU>(LFDiagonalMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var processZeros = zeros == ZerosOption.NoSkip || !Zero.Equals(f(0, 1, Zero));
            if (processZeros || sourceRowIndex - sourceColumnIndex != targetRowIndex - targetColumnIndex)
            {
                throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
            }

            var beginInclusive = Math.Max(sourceRowIndex, sourceColumnIndex);
            var count = Math.Min(sourceRowIndex + rowCount, sourceColumnIndex + columnCount) - beginInclusive;
            if (count > 0)
            {
                var beginTarget = Math.Max(targetRowIndex, targetColumnIndex);
                int targetIndex = beginTarget + 0;
                for (int i = 0; i < count; i++)
                {
                    target.Data[targetIndex] = f(targetIndex, targetIndex, Data[beginInclusive + i]);
                    targetIndex++;
                }
            }
        }

        void MapSubMatrixIndexedToUnchecked<TU>(LFDenseMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var processZeros = zeros == ZerosOption.NoSkip || !Zero.Equals(f(0, 1, Zero));
            if (existingData == DataClearOption.Clear && !processZeros)
            {
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
            }

            if (processZeros)
            {
                int sourceColumn = sourceColumnIndex + 0;
                int targetColumn = targetColumnIndex + 0;
                for (int j = 0; j < columnCount; j++)
                {
                    int targetIndex = targetRowIndex + (j + targetColumnIndex) * target.RowCount;
                    int sourceRow = sourceRowIndex;
                    int targetRow = targetRowIndex;
                    for (int i = 0; i < rowCount; i++)
                    {
                        target.Data[targetIndex++] = f(targetRow++, targetColumn, sourceRow++ == sourceColumn ? Data[sourceColumn] : Zero);
                    }
                    sourceColumn++;
                    targetColumn++;
                }
            }
            else
            {
                if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
                {
                    // column by column, but skip resulting zero columns at the beginning

                    int columnInit = sourceRowIndex - sourceColumnIndex;
                    int offset = (columnInit + targetColumnIndex) * target.RowCount + targetRowIndex;
                    int step = target.RowCount + 1;
                    int count = Math.Min(columnCount - columnInit, rowCount);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                    {
                        target.Data[j] = f(targetRowIndex + k, targetColumnIndex + columnInit + k, Data[sourceRowIndex + k]);
                    }
                }
                else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
                {
                    // row by row, but skip resulting zero rows at the beginning

                    int rowInit = sourceColumnIndex - sourceRowIndex;
                    int offset = targetColumnIndex * target.RowCount + rowInit + targetRowIndex;
                    int step = target.RowCount + 1;
                    int count = Math.Min(columnCount, rowCount - rowInit);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                    {
                        target.Data[j] = f(targetRowIndex + rowInit + k, targetColumnIndex + k, Data[sourceColumnIndex + k]);
                    }
                }
                else
                {
                    int offset = targetColumnIndex * target.RowCount + targetRowIndex;
                    int step = target.RowCount + 1;
                    var count = Math.Min(columnCount, rowCount);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                    {
                        target.Data[j] = f(targetRowIndex + k, targetColumnIndex + k, Data[sourceRowIndex + k]);
                    }
                }
            }
        }
        #endregion

        #region Funcational Combinators: Fold
        // FUNCTIONAL COMBINATORS: FOLD

        internal override void FoldByRowUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            if (zeros == ZerosOption.AllowSkip)
            {
                for (int k = 0; k < Data.Length; k++)
                {
                    target[k] = finalize(f(state[k], Data[k]), 1);
                }

                for (int k = Data.Length; k < RowCount; k++)
                {
                    target[k] = finalize(state[k], 0);
                }
            }
            else
            {
                for (int i = 0; i < RowCount; i++)
                {
                    TU s = state[i];
                    for (int j = 0; j < ColumnCount; j++)
                    {
                        s = f(s, i == j ? Data[i] : Zero);
                    }
                    target[i] = finalize(s, ColumnCount);
                }
            }
        }

        internal override void FoldByColumnUnchecked<TU>(TU[] target, Func<TU, T, TU> f, Func<TU, int, TU> finalize, TU[] state, ZerosOption zeros)
        {
            if (zeros == ZerosOption.AllowSkip)
            {
                for (int k = 0; k < Data.Length; k++)
                {
                    target[k] = finalize(f(state[k], Data[k]), 1);
                }

                for (int k = Data.Length; k < ColumnCount; k++)
                {
                    target[k] = finalize(state[k], 0);
                }
            }
            else
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    TU s = state[j];
                    for (int i = 0; i < RowCount; i++)
                    {
                        s = f(s, i == j ? Data[i] : Zero);
                    }
                    target[j] = finalize(s, RowCount);
                }
            }
        }

        internal override TState Fold2Unchecked<TOther, TState>(LFMatrixStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
        {
            if (other is LFDenseMatrixStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;
                int k = 0;
                for (int j = 0; j < ColumnCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        state = f(state, i == j ? Data[i] : Zero, otherData[k]);
                        k++;
                    }
                }
                return state;
            }

            if (other is LFDiagonalMatrixStorage<TOther> diagonalOther)
            {
                TOther[] otherData = diagonalOther.Data;
                for (int i = 0; i < Data.Length; i++)
                {
                    state = f(state, Data[i], otherData[i]);
                }

                // Do we really need to do this?
                if (zeros == ZerosOption.NoSkip)
                {
                    TOther otherZero = LFBuilder<TOther>.Matrix.Zero;
                    int count = RowCount * ColumnCount - Data.Length;
                    for (int i = 0; i < count; i++)
                    {
                        state = f(state, Zero, otherZero);
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

                if (zeros == ZerosOption.NoSkip)
                {
                    int k = 0;
                    for (int row = 0; row < RowCount; row++)
                    {
                        for (int col = 0; col < ColumnCount; col++)
                        {
                            if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                            {
                                state = f(state, row == col ? Data[row] : Zero, otherValues[k++]);
                            }
                            else
                            {
                                state = f(state, row == col ? Data[row] : Zero, otherZero);
                            }
                        }
                    }
                    return state;
                }

                for (int row = 0; row < RowCount; row++)
                {
                    bool diagonal = false;

                    var startIndex = otherRowPointers[row];
                    var endIndex = otherRowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                    {
                        if (otherColumnIndices[j] == row)
                        {
                            diagonal = true;
                            state = f(state, Data[row], otherValues[j]);
                        }
                        else
                        {
                            state = f(state, Zero, otherValues[j]);
                        }
                    }

                    if (!diagonal && row < ColumnCount)
                    {
                        state = f(state, Data[row], otherZero);
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