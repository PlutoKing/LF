/*──────────────────────────────────────────────────────────────
 * FileName     : LFDenseVectorStorage.cs
 * Created      : 2021-06-10 18:36:39
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
    /// <summary>
    /// 稠密向量内存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LFDenseVectorStorage<T> : LFVectorStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields
        [DataMember(Order = 1)]
        public readonly T[] Data;


        #endregion

        #region Properties
        public override bool IsDense => true;

        #endregion

        #region Constructors
        internal LFDenseVectorStorage(int length)
            : base(length)
        {
            Data = new T[length];
        }

        internal LFDenseVectorStorage(int length, T[] data)
            : base(length)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length != length)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"The given array has the wrong length. Should be {length}.");
            }

            Data = data;
        }
        #endregion

        #region Methods
        public override T Get(int index)
        {
            return Data[index];
        }

        public override void Set(int index, T value)
        {
            Data[index] = value;
        }

        #region Clear
        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        public override void Clear(int index, int count)
        {
            Array.Clear(Data, index, count);
        }
        #endregion

        #region Intialization Methods
        public static LFDenseVectorStorage<T> FromVector(LFVectorStorage<T> vector)
        {
            var storage = new LFDenseVectorStorage<T>(vector.Length);
            vector.CopyToUnchecked(storage, DataClearOption.Skip);
            return storage;
        }

        public static LFDenseVectorStorage<T> FromValue(int length, T value)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must not be negative (zero is ok).");
            }

            var data = new T[length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = value;
            }
            return new LFDenseVectorStorage<T>(length, data);
        }

        public static LFDenseVectorStorage<T> FromIndex(int length, Func<int, T> indexfun)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must not be negative (zero is ok).");
            }

            var data = new T[length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = indexfun(i);
            }
            return new LFDenseVectorStorage<T>(length, data);
        }

        public static LFDenseVectorStorage<T> FromEnumerable(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data is T[] arrayData)
            {
                var copy = new T[arrayData.Length];
                Array.Copy(arrayData, 0, copy, 0, arrayData.Length);
                return new LFDenseVectorStorage<T>(copy.Length, copy);
            }

            var array = data.ToArray();
            return new LFDenseVectorStorage<T>(array.Length, array);
        }

        public static LFDenseVectorStorage<T> FromIndexedEnumerable(int length, IEnumerable<Tuple<int, T>> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var array = new T[length];
            foreach (var item in data)
            {
                array[item.Item1] = item.Item2;
            }
            return new LFDenseVectorStorage<T>(array.Length, array);
        }

        #endregion

        #endregion
    }
}