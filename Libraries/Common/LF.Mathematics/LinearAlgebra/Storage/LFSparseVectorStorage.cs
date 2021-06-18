﻿/*──────────────────────────────────────────────────────────────
 * FileName     : LFSparseVectorStorage.cs
 * Created      : 2021-06-10 18:55:37
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace LF.Mathematics.LinearAlgebra.Storage
{
    [Serializable]
    public class LFSparseVectorStorage<T> : LFVectorStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields
        [DataMember(Order = 1)]
        public int[] Indices;

        [DataMember(Order = 2)]
        public T[] Values;

        [DataMember(Order = 3)]
        public int ValueCount;
        #endregion

        #region Properties
        public override bool IsDense => false;

        #endregion

        #region Constructors
        internal LFSparseVectorStorage(int length)
            : base(length)
        {
            Indices = new int[0];
            Values = new T[0];
            ValueCount = 0;
        }
        #endregion

        #region Methods
        public override T Get(int index)
        {
            // Search if item index exists in NonZeroIndices array in range "0 - nonzero values count"
            var itemIndex = Array.BinarySearch(Indices, 0, ValueCount, index);
            return itemIndex >= 0 ? Values[itemIndex] : Zero;
        }

        public override void Set(int index, T value)
        {
            // Search if "index" already exists in range "0 - nonzero values count"
            var itemIndex = Array.BinarySearch(Indices, 0, ValueCount, index);
            if (itemIndex >= 0)
            {
                // Non-zero item found in matrix
                if (Zero.Equals(value))
                {
                    // Delete existing item
                    RemoveAtIndexUnchecked(itemIndex);
                }
                else
                {
                    // Update item
                    Values[itemIndex] = value;
                }
            }
            else
            {
                // Item not found. Add new value
                if (!Zero.Equals(value))
                {
                    InsertAtIndexUnchecked(~itemIndex, index, value);
                }
            }
        }

        internal void InsertAtIndexUnchecked(int itemIndex, int index, T value)
        {
            // Check if the storage needs to be increased
            if ((ValueCount == Values.Length) && (ValueCount < Length))
            {
                // Value and Indices arrays are completely full so we increase the size
                var size = Math.Min(Values.Length + GrowthSize(), Length);
                Array.Resize(ref Values, size);
                Array.Resize(ref Indices, size);
            }

            // Move all values (with a position larger than index) in the value array to the next position
            // Move all values (with a position larger than index) in the columIndices array to the next position
            Array.Copy(Values, itemIndex, Values, itemIndex + 1, ValueCount - itemIndex);
            Array.Copy(Indices, itemIndex, Indices, itemIndex + 1, ValueCount - itemIndex);

            // Add the value and the column index
            Values[itemIndex] = value;
            Indices[itemIndex] = index;

            // increase the number of non-zero numbers by one
            ValueCount += 1;
        }

        internal void RemoveAtIndexUnchecked(int itemIndex)
        {
            // Value is zero. Let's delete it from Values and Indices array
            Array.Copy(Values, itemIndex + 1, Values, itemIndex, ValueCount - itemIndex - 1);
            Array.Copy(Indices, itemIndex + 1, Indices, itemIndex, ValueCount - itemIndex - 1);

            ValueCount -= 1;

            // Check whether we need to shrink the arrays. This is reasonable to do if
            // there are a lot of non-zero elements and storage is two times bigger
            if ((ValueCount > 1024) && (ValueCount < Indices.Length / 2))
            {
                Array.Resize(ref Values, ValueCount);
                Array.Resize(ref Indices, ValueCount);
            }
        }

        /// <summary>
        /// Calculates the amount with which to grow the storage array's if they need to be
        /// increased in size.
        /// </summary>
        /// <returns>The amount grown.</returns>
        int GrowthSize()
        {
            int delta;
            if (Values.Length > 1024)
            {
                delta = Values.Length / 4;
            }
            else
            {
                if (Values.Length > 256)
                {
                    delta = 512;
                }
                else
                {
                    delta = Values.Length > 64 ? 128 : 32;
                }
            }

            return delta;
        }

        public override bool Equals(LFVectorStorage<T> other)
        {
            // Reject equality when the argument is null or has a different shape.
            if (other == null || Length != other.Length)
            {
                return false;
            }

            // Accept if the argument is the same object as this.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is LFSparseVectorStorage<T> otherSparse)
            {
                int i = 0, j = 0;
                while (i < ValueCount || j < otherSparse.ValueCount)
                {
                    if (j >= otherSparse.ValueCount || i < ValueCount && Indices[i] < otherSparse.Indices[j])
                    {
                        if (!Zero.Equals(Values[i++]))
                        {
                            return false;
                        }

                        continue;
                    }

                    if (i >= ValueCount || j < otherSparse.ValueCount && otherSparse.Indices[j] < Indices[i])
                    {
                        if (!Zero.Equals(otherSparse.Values[j++]))
                        {
                            return false;
                        }

                        continue;
                    }

                    if (!Values[i].Equals(otherSparse.Values[j]))
                    {
                        return false;
                    }

                    i++;
                    j++;
                }

                return true;
            }

            return base.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var values = Values;
            var hashNum = Math.Min(ValueCount, 25);
            int hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++)
                {
                    hash = hash * 31 + values[i].GetHashCode();
                }
            }
            return hash;
        }

        #region Clearing Methods
        public override void Clear()
        {
            ValueCount = 0;
        }

        public override void Clear(int index, int count)
        {
            if (index == 0 && count == Length)
            {
                Clear();
                return;
            }

            var first = Array.BinarySearch(Indices, 0, ValueCount, index);
            var last = Array.BinarySearch(Indices, 0, ValueCount, index + count - 1);
            if (first < 0) first = ~first;
            if (last < 0) last = ~last - 1;
            int itemCount = last - first + 1;

            if (itemCount > 0)
            {
                Array.Copy(Values, first + itemCount, Values, first, ValueCount - first - itemCount);
                Array.Copy(Indices, first + itemCount, Indices, first, ValueCount - first - itemCount);

                ValueCount -= itemCount;
            }

            // Check whether we need to shrink the arrays. This is reasonable to do if
            // there are a lot of non-zero elements and storage is two times bigger
            if ((ValueCount > 1024) && (ValueCount < Indices.Length / 2))
            {
                Array.Resize(ref Values, ValueCount);
                Array.Resize(ref Indices, ValueCount);
            }
        }
        #endregion

        #region Initialization Methods
        public static LFSparseVectorStorage<T> FromVector(LFVectorStorage<T> vector)
        {
            var storage = new LFSparseVectorStorage<T>(vector.Length);
            vector.CopyToUnchecked(storage, DataClearOption.Skip);
            return storage;
        }

        public static LFSparseVectorStorage<T> FromValue(int length, T value)
        {
            if (Zero.Equals(value))
            {
                return new LFSparseVectorStorage<T>(length);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must not be negative (zero is ok).");
            }

            var indices = new int[length];
            var values = new T[length];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
                values[i] = value;
            }

            return new LFSparseVectorStorage<T>(length)
            {
                Indices = indices,
                Values = values,
                ValueCount = length
            };
        }

        public static LFSparseVectorStorage<T> FromIndex(int length, Func<int, T> indexfun)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must not be negative (zero is ok).");
            }

            var indices = new List<int>();
            var values = new List<T>();
            for (int i = 0; i < length; i++)
            {
                var item = indexfun(i);
                if (!Zero.Equals(item))
                {
                    values.Add(item);
                    indices.Add(i);
                }
            }
            return new LFSparseVectorStorage<T>(length)
            {
                Indices = indices.ToArray(),
                Values = values.ToArray(),
                ValueCount = values.Count
            };
        }

        public static LFSparseVectorStorage<T> FromEnumerable(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var indices = new List<int>();
            var values = new List<T>();
            int index = 0;

            foreach (T item in data)
            {
                if (!Zero.Equals(item))
                {
                    values.Add(item);
                    indices.Add(index);
                }
                index++;
            }

            return new LFSparseVectorStorage<T>(index)
            {
                Indices = indices.ToArray(),
                Values = values.ToArray(),
                ValueCount = values.Count
            };
        }

        public static LFSparseVectorStorage<T> FromIndexedEnumerable(int length, IEnumerable<Tuple<int, T>> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var indices = new List<int>();
            var values = new List<T>();
            foreach (var item in data)
            {
                if (!Zero.Equals(item.Item2))
                {
                    values.Add(item.Item2);
                    indices.Add(item.Item1);
                }
            }

            var indicesArray = indices.ToArray();
            var valuesArray = values.ToArray();
            Sorting.Sort(indicesArray, valuesArray);

            return new LFSparseVectorStorage<T>(length)
            {
                Indices = indicesArray,
                Values = valuesArray,
                ValueCount = values.Count
            };
        }
        #endregion

        #region Copy Methods
        // VECTOR COPY

        internal override void CopyToUnchecked(LFVectorStorage<T> target, DataClearOption existingData)
        {
            if (target is LFSparseVectorStorage<T> sparseTarget)
            {
                CopyToUnchecked(sparseTarget);
                return;
            }

            // FALL BACK

            if (existingData == DataClearOption.Clear)
            {
                target.Clear();
            }

            if (ValueCount != 0)
            {
                for (int i = 0; i < ValueCount; i++)
                {
                    target.Set(Indices[i], Values[i]);
                }
            }
        }

        void CopyToUnchecked(LFSparseVectorStorage<T> target)
        {
            if (ReferenceEquals(this, target))
            {
                return;
            }

            if (Length != target.Length)
            {
                var message = $"Matrix dimensions must agree: op1 is {Length}, op2 is {target.Length}.";
                throw new ArgumentException(message, nameof(target));
            }

            target.ValueCount = ValueCount;
            target.Values = new T[ValueCount];
            target.Indices = new int[ValueCount];

            if (ValueCount != 0)
            {
                Array.Copy(Values, 0, target.Values, 0, ValueCount);
                Buffer.BlockCopy(Indices, 0, target.Indices, 0, ValueCount * Constant.SizeOfInt);
            }
        }

        // Row COPY

        internal override void CopyToRowUnchecked(LFMatrixStorage<T> target, int rowIndex, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.ClearUnchecked(rowIndex, 1, 0, Length);
            }

            if (ValueCount == 0)
            {
                return;
            }

            for (int i = 0; i < ValueCount; i++)
            {
                target.Set(rowIndex, Indices[i], Values[i]);
            }
        }

        // COLUMN COPY

        internal override void CopyToColumnUnchecked(LFMatrixStorage<T> target, int columnIndex, DataClearOption existingData)
        {
            if (existingData == DataClearOption.Clear)
            {
                target.ClearUnchecked(0, Length, columnIndex, 1);
            }

            if (ValueCount == 0)
            {
                return;
            }

            for (int i = 0; i < ValueCount; i++)
            {
                target.Set(Indices[i], columnIndex, Values[i]);
            }
        }

        // SUB-VECTOR COPY

        internal override void CopySubVectorToUnchecked(LFVectorStorage<T> target,
            int sourceIndex, int targetIndex, int count, DataClearOption existingData)
        {
            if (target is LFSparseVectorStorage<T> sparseTarget)
            {
                CopySubVectorToUnchecked(sparseTarget, sourceIndex, targetIndex, count, existingData);
                return;
            }

            // FALL BACK

            var offset = targetIndex - sourceIndex;

            var sourceFirst = Array.BinarySearch(Indices, 0, ValueCount, sourceIndex);
            var sourceLast = Array.BinarySearch(Indices, 0, ValueCount, sourceIndex + count - 1);
            if (sourceFirst < 0) sourceFirst = ~sourceFirst;
            if (sourceLast < 0) sourceLast = ~sourceLast - 1;

            if (existingData == DataClearOption.Clear)
            {
                target.Clear(targetIndex, count);
            }

            for (int i = sourceFirst; i <= sourceLast; i++)
            {
                target.Set(Indices[i] + offset, Values[i]);
            }
        }

        void CopySubVectorToUnchecked(LFSparseVectorStorage<T> target,
            int sourceIndex, int targetIndex, int count,
            DataClearOption existingData)
        {
            var offset = targetIndex - sourceIndex;

            var sourceFirst = Array.BinarySearch(Indices, 0, ValueCount, sourceIndex);
            var sourceLast = Array.BinarySearch(Indices, 0, ValueCount, sourceIndex + count - 1);
            if (sourceFirst < 0) sourceFirst = ~sourceFirst;
            if (sourceLast < 0) sourceLast = ~sourceLast - 1;
            int sourceCount = sourceLast - sourceFirst + 1;

            // special case when copying to itself
            if (ReferenceEquals(this, target))
            {
                var values = new T[sourceCount];
                var indices = new int[sourceCount];

                Array.Copy(Values, sourceFirst, values, 0, sourceCount);
                for (int i = 0; i < indices.Length; i++)
                {
                    indices[i] = Indices[i + sourceFirst];
                }

                if (existingData == DataClearOption.Clear)
                {
                    Clear(targetIndex, count);
                }

                for (int i = sourceFirst; i <= sourceLast; i++)
                {
                    Set(indices[i] + offset, values[i]);
                }

                return;
            }

            // special case for empty target - much faster
            if (target.ValueCount == 0)
            {
                var values = new T[sourceCount];
                var indices = new int[sourceCount];

                Array.Copy(Values, sourceFirst, values, 0, sourceCount);
                for (int i = 0; i < indices.Length; i++)
                {
                    indices[i] = Indices[i + sourceFirst] + offset;
                }

                target.ValueCount = sourceCount;
                target.Values = values;
                target.Indices = indices;

                return;
            }

            if (existingData == DataClearOption.Clear)
            {
                target.Clear(targetIndex, count);
            }

            for (int i = sourceFirst; i <= sourceLast; i++)
            {
                target.Set(Indices[i] + offset, Values[i]);
            }
        }
        #endregion

        #region Extract Methods
        public override T[] ToArray()
        {
            var ret = new T[Length];
            for (int i = 0; i < ValueCount; i++)
            {
                ret[Indices[i]] = Values[i];
            }
            return ret;
        }
        #endregion

        #region Enumeration
        public override IEnumerable<T> Enumerate()
        {
            int k = 0;
            for (int i = 0; i < Length; i++)
            {
                yield return k < ValueCount && Indices[k] == i
                    ? Values[k++]
                    : Zero;
            }
        }

        public override IEnumerable<Tuple<int, T>> EnumerateIndexed()
        {
            int k = 0;
            for (int i = 0; i < Length; i++)
            {
                yield return k < ValueCount && Indices[k] == i
                    ? new Tuple<int, T>(i, Values[k++])
                    : new Tuple<int, T>(i, Zero);
            }
        }

        public override IEnumerable<T> EnumerateNonZero()
        {
            return Values.Take(ValueCount).Where(x => !Zero.Equals(x));
        }

        public override IEnumerable<Tuple<int, T>> EnumerateNonZeroIndexed()
        {
            for (var i = 0; i < ValueCount; i++)
            {
                if (!Zero.Equals(Values[i]))
                {
                    yield return new Tuple<int, T>(Indices[i], Values[i]);
                }
            }
        }
        #endregion

        #region Find
        public override Tuple<int, T> Find(Func<T, bool> predicate, ZerosOption zeros)
        {
            for (int i = 0; i < ValueCount; i++)
            {
                if (predicate(Values[i]))
                {
                    return new Tuple<int, T>(Indices[i], Values[i]);
                }
            }
            if (zeros == ZerosOption.NoSkip && ValueCount < Length && predicate(Zero))
            {
                for (int i = 0; i < Length; i++)
                {
                    if (i >= ValueCount || Indices[i] != i)
                    {
                        return new Tuple<int, T>(i, Zero);
                    }
                }
            }
            return null;
        }

        internal override Tuple<int, T, TOther> Find2Unchecked<TOther>(LFVectorStorage<TOther> other, Func<T, TOther, bool> predicate, ZerosOption zeros)
        {
            if (other is LFDenseVectorStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;
                int k = 0;
                for (int i = 0; i < otherData.Length; i++)
                {
                    if (k < ValueCount && Indices[k] == i)
                    {
                        if (predicate(Values[k], otherData[i]))
                        {
                            return new Tuple<int, T, TOther>(i, Values[k], otherData[i]);
                        }
                        k++;
                    }
                    else
                    {
                        if (predicate(Zero, otherData[i]))
                        {
                            return new Tuple<int, T, TOther>(i, Zero, otherData[i]);
                        }
                    }
                }
                return null;
            }

            if (other is LFSparseVectorStorage<TOther> sparseOther)
            {
                int[] otherIndices = sparseOther.Indices;
                TOther[] otherValues = sparseOther.Values;
                int otherValueCount = sparseOther.ValueCount;
                TOther otherZero = LFBuilder<TOther>.Matrix.Zero;

                // Full Scan
                int k = 0, otherk = 0;
                if (zeros == ZerosOption.NoSkip && ValueCount < Length && sparseOther.ValueCount < Length && predicate(Zero, otherZero))
                {
                    for (int i = 0; i < Length; i++)
                    {
                        var left = k < ValueCount && Indices[k] == i ? Values[k++] : Zero;
                        var right = otherk < otherValueCount && otherIndices[otherk] == i ? otherValues[otherk++] : otherZero;
                        if (predicate(left, right))
                        {
                            return new Tuple<int, T, TOther>(i, left, right);
                        }
                    }
                    return null;
                }

                // Sparse Scan
                k = 0;
                otherk = 0;
                while (k < ValueCount || otherk < otherValueCount)
                {
                    if (k == ValueCount || otherk < otherValueCount && Indices[k] > otherIndices[otherk])
                    {
                        if (predicate(Zero, otherValues[otherk++]))
                        {
                            return new Tuple<int, T, TOther>(otherIndices[otherk - 1], Zero, otherValues[otherk - 1]);
                        }
                    }
                    else if (otherk == otherValueCount || Indices[k] < otherIndices[otherk])
                    {
                        if (predicate(Values[k++], otherZero))
                        {
                            return new Tuple<int, T, TOther>(Indices[k - 1], Values[k - 1], otherZero);
                        }
                    }
                    else
                    {
                        if (predicate(Values[k++], otherValues[otherk++]))
                        {
                            return new Tuple<int, T, TOther>(Indices[k - 1], Values[k - 1], otherValues[otherk - 1]);
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
        public override void MapInplace(Func<T, T> f, ZerosOption zeros)
        {
            var indices = new List<int>();
            var values = new List<T>(ValueCount);
            if (zeros == ZerosOption.NoSkip || !Zero.Equals(f(Zero)))
            {
                int k = 0;
                for (int i = 0; i < Length; i++)
                {
                    var item = k < ValueCount && (Indices[k]) == i ? f(Values[k++]) : f(Zero);
                    if (!Zero.Equals(item))
                    {
                        values.Add(item);
                        indices.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < ValueCount; i++)
                {
                    var item = f(Values[i]);
                    if (!Zero.Equals(item))
                    {
                        values.Add(item);
                        indices.Add(Indices[i]);
                    }
                }
            }
            Indices = indices.ToArray();
            Values = values.ToArray();
            ValueCount = values.Count;
        }

        public override void MapIndexedInplace(Func<int, T, T> f, ZerosOption zeros)
        {
            var indices = new List<int>();
            var values = new List<T>(ValueCount);
            if (zeros == ZerosOption.NoSkip)
            {
                int k = 0;
                for (int i = 0; i < Length; i++)
                {
                    var item = k < ValueCount && (Indices[k]) == i ? f(i, Values[k++]) : f(i, Zero);
                    if (!Zero.Equals(item))
                    {
                        values.Add(item);
                        indices.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < ValueCount; i++)
                {
                    var item = f(Indices[i], Values[i]);
                    if (!Zero.Equals(item))
                    {
                        values.Add(item);
                        indices.Add(Indices[i]);
                    }
                }
            }
            Indices = indices.ToArray();
            Values = values.ToArray();
            ValueCount = values.Count;
        }

        internal override void MapToUnchecked<TU>(LFVectorStorage<TU> target, Func<T, TU> f, ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFSparseVectorStorage<TU> sparseTarget)
            {
                var indices = new List<int>();
                var values = new List<TU>();
                if (zeros == ZerosOption.NoSkip || !Zero.Equals(f(Zero)))
                {
                    int k = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        var item = k < ValueCount && (Indices[k]) == i ? f(Values[k++]) : f(Zero);
                        if (!Zero.Equals(item))
                        {
                            values.Add(item);
                            indices.Add(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ValueCount; i++)
                    {
                        var item = f(Values[i]);
                        if (!Zero.Equals(item))
                        {
                            values.Add(item);
                            indices.Add(Indices[i]);
                        }
                    }
                }
                sparseTarget.Indices = indices.ToArray();
                sparseTarget.Values = values.ToArray();
                sparseTarget.ValueCount = values.Count;
                return;
            }

            if (target is LFDenseVectorStorage<TU> denseTarget)
            {
                if (existingData == DataClearOption.Clear)
                {
                    denseTarget.Clear();
                }

                if (zeros == ZerosOption.NoSkip || !Zero.Equals(f(Zero)))
                {
                    int k = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        denseTarget.Data[i] = k < ValueCount && (Indices[k]) == i
                            ? f(Values[k++])
                            : f(Zero);
                    }
                }
                else
                {
                    for (int i = 0; i < ValueCount; i++)
                    {
                        denseTarget.Data[Indices[i]] = f(Values[i]);
                    }
                }
                return;
            }

            // FALL BACK

            base.MapToUnchecked(target, f, zeros, existingData);
        }

        internal override void MapIndexedToUnchecked<TU>(LFVectorStorage<TU> target, Func<int, T, TU> f, ZerosOption zeros, DataClearOption existingData)
        {
            if (target is LFSparseVectorStorage<TU> sparseTarget)
            {
                var indices = new List<int>();
                var values = new List<TU>();
                if (zeros == ZerosOption.NoSkip || !Zero.Equals(f(0, Zero)))
                {
                    int k = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        var item = k < ValueCount && (Indices[k]) == i ? f(i, Values[k++]) : f(i, Zero);
                        if (!Zero.Equals(item))
                        {
                            values.Add(item);
                            indices.Add(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ValueCount; i++)
                    {
                        var item = f(Indices[i], Values[i]);
                        if (!Zero.Equals(item))
                        {
                            values.Add(item);
                            indices.Add(Indices[i]);
                        }
                    }
                }
                sparseTarget.Indices = indices.ToArray();
                sparseTarget.Values = values.ToArray();
                sparseTarget.ValueCount = values.Count;
                return;
            }

            if (target is LFDenseVectorStorage<TU> denseTarget)
            {
                if (existingData == DataClearOption.Clear)
                {
                    denseTarget.Clear();
                }

                if (zeros == ZerosOption.NoSkip || !Zero.Equals(f(0, Zero)))
                {
                    int k = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        denseTarget.Data[i] = k < ValueCount && (Indices[k]) == i
                            ? f(i, Values[k++])
                            : f(i, Zero);
                    }
                }
                else
                {
                    for (int i = 0; i < ValueCount; i++)
                    {
                        denseTarget.Data[Indices[i]] = f(Indices[i], Values[i]);
                    }
                }
                return;
            }

            // FALL BACK

            base.MapIndexedToUnchecked(target, f, zeros, existingData);
        }

        internal override void Map2ToUnchecked(LFVectorStorage<T> target, LFVectorStorage<T> other, Func<T, T, T> f, ZerosOption zeros, DataClearOption existingData)
        {
            var processZeros = zeros == ZerosOption.NoSkip || !Zero.Equals(f(Zero, Zero));

            var denseTarget = target as LFDenseVectorStorage<T>;
            var denseOther = other as LFDenseVectorStorage<T>;

            if (denseTarget == null && (denseOther != null || processZeros))
            {
                // The handling is effectively dense but we're supposed to push
                // to a sparse target. Let's use a dense target instead,
                // then copy it normalized back to the sparse target.
                var intermediate = new LFDenseVectorStorage<T>(target.Length);
                Map2ToUnchecked(intermediate, other, f, zeros, DataClearOption.Skip);
                intermediate.CopyTo(target, existingData);
                return;
            }

            if (denseOther != null)
            {
                T[] targetData = denseTarget.Data;
                T[] otherData = denseOther.Data;

                int k = 0;
                for (int i = 0; i < otherData.Length; i++)
                {
                    if (k < ValueCount && Indices[k] == i)
                    {
                        targetData[i] = f(Values[k], otherData[i]);
                        k++;
                    }
                    else
                    {
                        targetData[i] = f(Zero, otherData[i]);
                    }
                }

                return;
            }

            var sparseOther = other as LFSparseVectorStorage<T>;
            if (sparseOther != null && denseTarget != null)
            {
                T[] targetData = denseTarget.Data;
                int[] otherIndices = sparseOther.Indices;
                T[] otherValues = sparseOther.Values;
                int otherValueCount = sparseOther.ValueCount;

                if (processZeros)
                {
                    int p = 0, q = 0;
                    for (int i = 0; i < targetData.Length; i++)
                    {
                        var left = p < ValueCount && Indices[p] == i ? Values[p++] : Zero;
                        var right = q < otherValueCount && otherIndices[q] == i ? otherValues[q++] : Zero;
                        targetData[i] = f(left, right);
                    }
                }
                else
                {
                    if (existingData == DataClearOption.Clear)
                    {
                        denseTarget.Clear();
                    }

                    int p = 0, q = 0;
                    while (p < ValueCount || q < otherValueCount)
                    {
                        if (q >= otherValueCount || p < ValueCount && Indices[p] < otherIndices[q])
                        {
                            targetData[Indices[p]] = f(Values[p], Zero);
                            p++;
                        }
                        else if (p >= ValueCount || q < otherValueCount && Indices[p] > otherIndices[q])
                        {
                            targetData[otherIndices[q]] = f(Zero, otherValues[q]);
                            q++;
                        }
                        else
                        {
                            Debug.Assert(Indices[p] == otherIndices[q]);
                            targetData[Indices[p]] = f(Values[p], otherValues[q]);
                            p++;
                            q++;
                        }
                    }
                }

                return;
            }

            if (sparseOther != null && target is LFSparseVectorStorage<T> sparseTarget)
            {
                var indices = new List<int>();
                var values = new List<T>();
                int[] otherIndices = sparseOther.Indices;
                T[] otherValues = sparseOther.Values;
                int otherValueCount = sparseOther.ValueCount;

                int p = 0, q = 0;
                while (p < ValueCount || q < otherValueCount)
                {
                    if (q >= otherValueCount || p < ValueCount && Indices[p] < otherIndices[q])
                    {
                        var value = f(Values[p], Zero);
                        if (!Zero.Equals(value))
                        {
                            indices.Add(Indices[p]);
                            values.Add(value);
                        }

                        p++;
                    }
                    else if (p >= ValueCount || q < otherValueCount && Indices[p] > otherIndices[q])
                    {
                        var value = f(Zero, otherValues[q]);
                        if (!Zero.Equals(value))
                        {
                            indices.Add(otherIndices[q]);
                            values.Add(value);
                        }

                        q++;
                    }
                    else
                    {
                        var value = f(Values[p], otherValues[q]);
                        if (!Zero.Equals(value))
                        {
                            indices.Add(Indices[p]);
                            values.Add(value);
                        }

                        p++;
                        q++;
                    }
                }

                sparseTarget.Indices = indices.ToArray();
                sparseTarget.Values = values.ToArray();
                sparseTarget.ValueCount = values.Count;
                return;
            }

            // FALL BACK

            base.Map2ToUnchecked(target, other, f, zeros, existingData);
        }
        #endregion

        #region Functional Combinators: Fold
        internal override TState Fold2Unchecked<TOther, TState>(LFVectorStorage<TOther> other, Func<TState, T, TOther, TState> f, TState state, ZerosOption zeros)
        {
            if (other is LFSparseVectorStorage<TOther> sparseOther)
            {
                int[] otherIndices = sparseOther.Indices;
                TOther[] otherValues = sparseOther.Values;
                int otherValueCount = sparseOther.ValueCount;
                TOther otherZero = LFBuilder<TOther>.Vector.Zero;

                if (zeros == ZerosOption.NoSkip)
                {
                    int p = 0, q = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        var left = p < ValueCount && Indices[p] == i ? Values[p++] : Zero;
                        var right = q < otherValueCount && otherIndices[q] == i ? otherValues[q++] : otherZero;
                        state = f(state, left, right);
                    }
                }
                else
                {
                    int p = 0, q = 0;
                    while (p < ValueCount || q < otherValueCount)
                    {
                        if (q >= otherValueCount || p < ValueCount && Indices[p] < otherIndices[q])
                        {
                            state = f(state, Values[p], otherZero);
                            p++;
                        }
                        else if (p >= ValueCount || q < otherValueCount && Indices[p] > otherIndices[q])
                        {
                            state = f(state, Zero, otherValues[q]);
                            q++;
                        }
                        else
                        {
                            Debug.Assert(Indices[p] == otherIndices[q]);
                            state = f(state, Values[p], otherValues[q]);
                            p++;
                            q++;
                        }
                    }
                }

                return state;
            }

            if (other is LFDenseVectorStorage<TOther> denseOther)
            {
                TOther[] otherData = denseOther.Data;

                int k = 0;
                for (int i = 0; i < otherData.Length; i++)
                {
                    if (k < ValueCount && Indices[k] == i)
                    {
                        state = f(state, Values[k], otherData[i]);
                        k++;
                    }
                    else
                    {
                        state = f(state, Zero, otherData[i]);
                    }
                }

                return state;
            }

            return base.Fold2Unchecked(other, f, state, zeros);
        }
        #endregion

        #endregion
    }
}