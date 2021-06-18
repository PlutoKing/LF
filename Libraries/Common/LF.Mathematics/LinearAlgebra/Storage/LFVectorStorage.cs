/*──────────────────────────────────────────────────────────────
 * FileName     : LFVectorStorage.cs
 * Created      : 2021-06-10 18:33:06
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;

namespace LF.Mathematics.LinearAlgebra.Storage
{
    /// <summary>
    /// 向量内存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class LFVectorStorage<T> : IEquatable<LFVectorStorage<T>>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields
        protected static readonly T Zero = LFBuilder<T>.Vector.Zero;

        [DataMember(Order = 1)]
        public readonly int Length;
        #endregion

        #region Properties
        public abstract bool IsDense { get; }

        public T this[int index]
        {
            get
            {
                ValidateRange(index);
                return Get(index);
            }

            set
            {
                ValidateRange(index);
                Set(index, value);
            }
        }

        #endregion

        #region Constructors
        public LFVectorStorage(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must not be negative (zero is ok).");
            }

            Length = length;
        }
        #endregion

        #region Methods

        #region Common Methods
        public abstract T Get(int index);

        public abstract void Set(int index, T value);

        public virtual bool Equals(LFVectorStorage<T> other)
        {
            // Reject equality when the argument is null or has a different shape.
            if (other == null)
            {
                return false;
            }
            if (Length != other.Length)
            {
                return false;
            }

            // Accept if the argument is the same object as this.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If all else fails, perform element wise comparison.
            for (var index = 0; index < Length; index++)
            {
                if (!Get(index).Equals(other.Get(index)))
                {
                    return false;
                }
            }

            return true;
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as LFVectorStorage<T>);
        }

        public override int GetHashCode()
        {
            var hashNum = Math.Min(Length, 25);
            int hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++)
                {
                    hash = hash * 31 + Get(i).GetHashCode();
                }
            }
            return hash;
        }

        #endregion

        #region Clear

        public virtual void Clear()
        {
            for (var i = 0; i < Length; i++)
            {
                Set(i, Zero);
            }
        }

        public virtual void Clear(int index, int count)
        {
            for (var i = index; i < index + count; i++)
            {
                Set(i, Zero);
            }
        }

        #endregion

        #region Copy
        public void CopyTo(LFVectorStorage<T> target, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (ReferenceEquals(this, target))
            {
                return;
            }

            if (Length != target.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            CopyToUnchecked(target, existingData);
        }

        internal virtual void CopyToUnchecked(LFVectorStorage<T> target, DataClearOption existingData)
        {
            for (int i = 0; i < Length; i++)
            {
                target.Set(i, Get(i));
            }
        }

        public void CopyToRow(LFMatrixStorage<T> target, int rowIndex, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (Length != target.ColumnCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            ValidateRowRange(target, rowIndex);
            CopyToRowUnchecked(target, rowIndex, existingData);
        }

        internal virtual void CopyToRowUnchecked(LFMatrixStorage<T> target, int rowIndex, DataClearOption existingData)
        {
            for (int j = 0; j < Length; j++)
            {
                target.Set(rowIndex, j, Get(j));
            }
        }

        public void CopyToColumn(LFMatrixStorage<T> target, int columnIndex, DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (Length != target.RowCount)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            ValidateColumnRange(target, columnIndex);
            CopyToColumnUnchecked(target, columnIndex, existingData);
        }

        internal virtual void CopyToColumnUnchecked(LFMatrixStorage<T> target, int columnIndex, DataClearOption existingData)
        {
            for (int i = 0; i < Length; i++)
            {
                target.Set(i, columnIndex, Get(i));
            }
        }


        public void CopySubVectorTo(LFVectorStorage<T> target,
    int sourceIndex, int targetIndex, int count,
    DataClearOption existingData = DataClearOption.Clear)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (count == 0)
            {
                return;
            }

            ValidateSubVectorRange(target, sourceIndex, targetIndex, count);
            CopySubVectorToUnchecked(target, sourceIndex, targetIndex, count, existingData);
        }

        internal virtual void CopySubVectorToUnchecked(LFVectorStorage<T> target,
            int sourceIndex, int targetIndex, int count, DataClearOption existingData)
        {
            if (ReferenceEquals(this, target))
            {
                var tmp = new T[count];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = Get(i + sourceIndex);
                }
                for (int i = 0; i < tmp.Length; i++)
                {
                    Set(i + targetIndex, tmp[i]);
                }

                return;
            }

            for (int i = sourceIndex, ii = targetIndex; i < sourceIndex + count; i++, ii++)
            {
                target.Set(ii, Get(i));
            }
        }


        public void CopyToSubRow(LFMatrixStorage<T> target, int rowIndex,
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
            CopyToSubRowUnchecked(target, rowIndex, sourceColumnIndex, targetColumnIndex, columnCount, existingData);
        }

        internal virtual void CopyToSubRowUnchecked(LFMatrixStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, DataClearOption existingData)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                target.Set(rowIndex, jj, Get(j));
            }
        }

        public void CopyToSubColumn(LFMatrixStorage<T> target, int columnIndex,
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
            CopyToSubColumnUnchecked(target, columnIndex, sourceRowIndex, targetRowIndex, rowCount, existingData);
        }

        internal virtual void CopyToSubColumnUnchecked(LFMatrixStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount, DataClearOption existingData)
        {
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
            {
                target.Set(ii, columnIndex, Get(i));
            }
        }

        #endregion

        #region Extract
        public virtual T[] ToArray()
        {
            var ret = new T[Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Get(i);
            }
            return ret;
        }

        public virtual T[] AsArray()
        {
            return null;
        }


        #endregion

        #region Enumeration
        public virtual IEnumerable<T> Enumerate()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return Get(i);
            }
        }

        public virtual IEnumerable<Tuple<int, T>> EnumerateIndexed()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return new Tuple<int, T>(i, Get(i));
            }
        }

        public virtual IEnumerable<T> EnumerateNonZero()
        {
            for (var i = 0; i < Length; i++)
            {
                var x = Get(i);
                if (!Zero.Equals(x))
                {
                    yield return x;
                }
            }
        }

        public virtual IEnumerable<Tuple<int, T>> EnumerateNonZeroIndexed()
        {
            for (var i = 0; i < Length; i++)
            {
                var x = Get(i);
                if (!Zero.Equals(x))
                {
                    yield return new Tuple<int, T>(i, x);
                }
            }
        }


        #endregion

        #region Find
        public virtual Tuple<int, T> Find(Func<T, bool> predicate, ZerosOption zeros)
        {
            for (int i = 0; i < Length; i++)
            {
                var item = Get(i);
                if (predicate(item))
                {
                    return new Tuple<int, T>(i, item);
                }
            }
            return null;
        }

        public Tuple<int, T, TOther> Find2<TOther>(LFVectorStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Length != other.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(other));
            }

            return Find2Unchecked(other, predicate, zeros);
        }

        internal virtual Tuple<int, T, TOther> Find2Unchecked<TOther>(LFVectorStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (int i = 0; i < Length; i++)
            {
                var item = Get(i);
                var otherItem = other.Get(i);
                if (predicate(item, otherItem))
                {
                    return new Tuple<int, T, TOther>(i, item, otherItem);
                }
            }
            return null;
        }

        #endregion

        #region Functional Combinators: Map
        public virtual void MapInplace(Func<T, T> f, ZerosOption zeros)
        {
            for (int i = 0; i < Length; i++)
            {
                Set(i, f(Get(i)));
            }
        }

        public virtual void MapIndexedInplace(Func<int, T, T> f, ZerosOption zeros)
        {
            for (int i = 0; i < Length; i++)
            {
                Set(i, f(i, Get(i)));
            }
        }

        public void MapTo<TU>(LFVectorStorage<TU> target, Func<T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (Length != target.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            MapToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapToUnchecked<TU>(LFVectorStorage<TU> target, Func<T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int i = 0; i < Length; i++)
            {
                target.Set(i, f(Get(i)));
            }
        }

        public void MapIndexedTo<TU>(LFVectorStorage<TU> target, Func<int, T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (Length != target.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            MapIndexedToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapIndexedToUnchecked<TU>(LFVectorStorage<TU> target, Func<int, T, TU> f, ZerosOption zeros, DataClearOption existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int i = 0; i < Length; i++)
            {
                target.Set(i, f(i, Get(i)));
            }
        }

        public void Map2To(LFVectorStorage<T> target, LFVectorStorage<T> other, Func<T, T, T> f, ZerosOption zeros, DataClearOption existingData)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Length != target.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(target));
            }

            if (Length != other.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(other));
            }

            Map2ToUnchecked(target, other, f, zeros, existingData);
        }

        internal virtual void Map2ToUnchecked(LFVectorStorage<T> target, LFVectorStorage<T> other, Func<T, T, T> f, ZerosOption zeros, DataClearOption existingData)
        {
            for (int i = 0; i < Length; i++)
            {
                target.Set(i, f(Get(i), other.Get(i)));
            }
        }

        #endregion

        #region Functional Combinators: Fold
        public TState Fold2<TOther, TState>(LFVectorStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Length != other.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.", nameof(other));
            }

            return Fold2Unchecked(other, f, state, zeros);
        }

        internal virtual TState Fold2Unchecked<TOther, TState>(LFVectorStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (int i = 0; i < Length; i++)
            {
                state = f(state, Get(i), other.Get(i));
            }

            return state;
        }

        #endregion

        #endregion
    }
}