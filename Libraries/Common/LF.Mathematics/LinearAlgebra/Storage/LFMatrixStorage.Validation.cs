/*──────────────────────────────────────────────────────────────
 * FileName     : LFMatrixStorage.cs
 * Created      : 2021-06-10 19:05:14
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
    public partial class LFMatrixStorage<T>
    {
        void ValidateRange(int row, int column)
        {
            if ((uint)row >= (uint)RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            if ((uint)column >= (uint)ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }
        }

        void ValidateSubMatrixRange<TU>(LFMatrixStorage<TU> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "Value must be positive.");
            }

            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "Value must be positive.");
            }

            // Verify Source

            if ((uint)sourceRowIndex >= (uint)RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRowIndex));
            }

            if ((uint)sourceColumnIndex >= (uint)ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceColumnIndex));
            }

            var sourceRowMax = sourceRowIndex + rowCount;
            var sourceColumnMax = sourceColumnIndex + columnCount;

            if (sourceRowMax > RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            if (sourceColumnMax > ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }

            // Verify Target

            if ((uint)targetRowIndex >= (uint)target.RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(targetRowIndex));
            }

            if ((uint)targetColumnIndex >= (uint)target.ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(targetColumnIndex));
            }

            var targetRowMax = targetRowIndex + rowCount;
            var targetColumnMax = targetColumnIndex + columnCount;

            if (targetRowMax > target.RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            if (targetColumnMax > target.ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }
        }

        void ValidateRowRange<TU>(LFVectorStorage<TU> target, int rowIndex)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if ((uint)rowIndex >= (uint)RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }

            if (ColumnCount != target.Length)
            {
                throw new ArgumentException("Matrix row dimensions must agree.", nameof(target));
            }
        }

        void ValidateColumnRange<TU>(LFVectorStorage<TU> target, int columnIndex)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if ((uint)columnIndex >= (uint)ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            if (RowCount != target.Length)
            {
                throw new ArgumentException("Matrix column dimensions must agree.", nameof(target));
            }
        }

        void ValidateSubRowRange<TU>(LFVectorStorage<TU> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "Value must be positive.");
            }

            // Verify Source

            if ((uint)rowIndex >= (uint)RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }

            if ((uint)sourceColumnIndex >= (uint)ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceColumnIndex));
            }

            if (sourceColumnIndex + columnCount > ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }

            // Verify Target

            if ((uint)targetColumnIndex >= (uint)target.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(targetColumnIndex));
            }

            if (targetColumnIndex + columnCount > target.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }
        }

        void ValidateSubColumnRange<TU>(LFVectorStorage<TU> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "Value must be positive.");
            }

            // Verify Source

            if ((uint)columnIndex >= (uint)ColumnCount)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            if ((uint)sourceRowIndex >= (uint)RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRowIndex));
            }

            if (sourceRowIndex + rowCount > RowCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            // Verify Target

            if ((uint)targetRowIndex >= (uint)target.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(targetRowIndex));
            }

            if (targetRowIndex + rowCount > target.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }
        }
    }
}