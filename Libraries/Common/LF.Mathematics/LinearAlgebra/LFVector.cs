/*──────────────────────────────────────────────────────────────
 * FileName     : LFVector.cs
 * Created      : 2021-06-10 18:35:07
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Runtime.Serialization;
using LF.Mathematics.LinearAlgebra.Storage;

namespace LF.Mathematics.LinearAlgebra
{
    public abstract partial class LFVector<T> : IFormattable, IEquatable<LFVector<T>>, IList, IList<T>, ICloneable
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields


        #endregion

        #region Properties
        public LFVectorStorage<T> Storage { get; private set; }

        public int Count { get; private set; }

        #endregion

        #region Constructors
        protected LFVector(LFVectorStorage<T> storage)
        {
            Storage = storage;
            Count = storage.Length;
        }
        #endregion

        #region Methods

        #endregion
    }
}