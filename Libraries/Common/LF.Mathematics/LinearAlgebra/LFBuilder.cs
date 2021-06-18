/*──────────────────────────────────────────────────────────────
 * FileName     : LFBuilder.cs
 * Created      : 2021-06-10 16:13:09
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Numerics;
using LF.Mathematics.LinearAlgebra.Storage;
using System.Linq;
using System.Runtime.Serialization;

namespace LF.Mathematics.LinearAlgebra
{
    /// <summary>
    /// 构造器
    /// </summary>
    public abstract class LFBuilder<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields

        static Lazy<Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>> _singleton = new Lazy<Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>>(Create);


        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFBuilder()
        {
        }
        #endregion

        #region Methods
        static Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>> Create()
        {
            if (typeof(T) == typeof(Complex))
            {
                return new Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>(
                    (LFMatrixBuilder<T>)(object)new LFcMatBuilder(),
                    (LFVectorBuilder<T>)(object)new Complex.LFVectorBuilder());
            }

            if (typeof(T) == typeof(Numerics.Complex32))
            {
                return new Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>(
                    (LFMatrixBuilder<T>)(object)new Complex32.LFMatrixBuilder(),
                    (LFVectorBuilder<T>)(object)new Complex32.LFVectorBuilder());
            }

            if (typeof(T) == typeof(double))
            {
                return new Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>(
                    (LFMatrixBuilder<T>)(object)new Double.LFMatrixBuilder(),
                    (LFVectorBuilder<T>)(object)new Double.LFVectorBuilder());
            }

            if (typeof(T) == typeof(float))
            {
                return new Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>(
                    (LFMatrixBuilder<T>)(object)new Single.LFMatrixBuilder(),
                    (LFVectorBuilder<T>)(object)new Single.LFVectorBuilder());
            }

            throw new NotSupportedException(FormattableString.Invariant($"Matrices and vectors of type '{typeof(T).Name}' are not supported. Only Double, Single, Complex or Complex32 are supported at this point."));
        }

        public static void Register(LFMatrixBuilder<T> matrixBuilder, LFVectorBuilder<T> vectorBuilder)
        {
            _singleton = new Lazy<Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>>(() => new Tuple<LFMatrixBuilder<T>, LFVectorBuilder<T>>(matrixBuilder, vectorBuilder));
        }

        public static LFMatrixBuilder<T> Matrix => _singleton.Value.Item1;

        public static LFVectorBuilder<T> Vector => _singleton.Value.Item2;

        #endregion
    }

    #region Mat Builers

    /// <summary>
    /// 整型矩阵构造器
    /// </summary>
    internal class LFiMatBuilder : LFMatrixBuilder<int>
    {
        public override int Zero => 0;

        public override int One => 1;

        internal override int Add(int x, int y)
        {
            return x + y;
        }
    }

    /// <summary>
    /// 双精度浮点型矩阵构造器
    /// </summary>
    internal class LFdMatBuilder : LFMatrixBuilder<double>
    {
        public override double Zero => 0d;

        public override double One => 1d;

        internal override double Add(double x, double y)
        {
            return x + y;
        }
    }

    /// <summary>
    /// 单精度浮点型矩阵构造器
    /// </summary>
    internal class LFfMatBuilder : LFMatrixBuilder<float>
    {
        public override float Zero => 0f;

        public override float One => 1f;

        internal override float Add(float x, float y)
        {
            return x + y;
        }
    }

    /// <summary>
    /// 复数型矩阵构造器
    /// </summary>
    internal class LFcMatBuilder : LFMatrixBuilder<Complex>
    {
        public override Complex Zero => Complex.Zero;

        public override Complex One => Complex.One;

        internal override Complex Add(Complex x, Complex y)
        {
            return x + y;
        }
    }

    #endregion

    #region Vec Builders

    #endregion

    #region LFMatrix & LFVector Builder
    /// <summary>
    /// 矩阵构造器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LFMatrixBuilder<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        #region Fields



        #endregion

        #region Properties
        public abstract T Zero { get; }

        public abstract T One { get; }

        #endregion

        #region Constructors
        public LFMatrixBuilder()
        {
        }
        #endregion

        #region Methods
        internal abstract T Add(T x, T y);

        public LFMatrix<T> FromStorage(LFMatrixStorage<T> storage)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));

            if (storage is LFDenseMatrixStorage<T> dense) return Dense(dense);
            if (storage is LFSparseMatrixStorage<T> sparse) return Sparse(sparse);
            if (storage is LFDiagonalMatrixStorage<T> diagonal) return Diagonal(diagonal);

            throw new NotSupportedException(FormattableString.Invariant($"LFMatrix storage type '{storage.GetType().Name}' is not supported. Only LFDenseMatrixStorage, LFDiagonalMatrixStorage and LFSparseMatrixStorage are supported as this point."));
        }

        #region Same As
        /// <summary>
        /// Create a new matrix with the same kind of the provided example.
        /// </summary>
        public LFMatrix<T> SameAs<TU>(LFMatrix<TU> example, int rows, int columns, bool fullyMutable = false)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var storage = example.Storage;
            if (storage is LFDenseMatrixStorage<T>) return Dense(rows, columns);
            if (storage is LFSparseMatrixStorage<T>) return fullyMutable ? Sparse(rows, columns) : Diagonal(rows, columns);
            if (storage is LFDiagonalMatrixStorage<T>) return Sparse(rows, columns);
            return Dense(rows, columns);
        }

        /// <summary>
        /// Create a new matrix with the same kind and dimensions of the provided example.
        /// </summary>
        public LFMatrix<T> SameAs<TU>(LFMatrix<TU> example)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            return SameAs(example, example.RowCount, example.ColumnCount);
        }

        /// <summary>
        /// Create a new matrix with the same kind of the provided example.
        /// </summary>
        public LFMatrix<T> SameAs(LFVector<T> example, int rows, int columns)
        {
            return example.Storage.IsDense ? Dense(rows, columns) : Sparse(rows, columns);
        }

        /// <summary>
        /// Create a new matrix with a type that can represent and is closest to both provided samples.
        /// </summary>
        public LFMatrix<T> SameAs(LFMatrix<T> example, LFMatrix<T> otherExample, int rows, int columns, bool fullyMutable = false)
        {
            var storage1 = example.Storage;
            var storage2 = otherExample.Storage;
            if (storage1 is LFDenseMatrixStorage<T> || storage2 is LFDenseMatrixStorage<T>) return Dense(rows, columns);
            if (storage1 is LFSparseMatrixStorage<T> && storage2 is LFSparseMatrixStorage<T>) return fullyMutable ? Sparse(rows, columns) : Diagonal(rows, columns);
            if (storage1 is LFDiagonalMatrixStorage<T> || storage2 is LFDiagonalMatrixStorage<T>) return Sparse(rows, columns);
            return Dense(rows, columns);
        }

        /// <summary>
        /// Create a new matrix with a type that can represent and is closest to both provided samples and the dimensions of example.
        /// </summary>
        public LFMatrix<T> SameAs(LFMatrix<T> example, LFMatrix<T> otherExample)
        {
            return SameAs(example, otherExample, example.RowCount, example.ColumnCount);
        }

        #endregion

        #region Random LFMatrix

        #endregion

        #region Dense LFMatrix

        public abstract LFMatrix<T> Dense(LFDenseMatrixStorage<T> storage);


        public LFMatrix<T> Dense(int rows,int columns)
        {
            return Dense(new LFDenseMatrixStorage<T>(rows, columns));
        }

        public LFMatrix<T> Dense(int rows, int columns, T[] storage)
        {
            return Dense(new LFDenseMatrixStorage<T>(rows, columns, storage));
        }

        public LFMatrix<T> Dense(int rows, int columns, T value)
        {
            if (Zero.Equals(value))
                return Dense(rows, columns);

            return Dense(LFDenseMatrixStorage<T>.FromValue(rows, columns, value));
        }

        public LFMatrix<T> Dense(int rows, int columns, Func<int, int, T> init)
        {
            return Dense(LFDenseMatrixStorage<T>.FromIndex(rows, columns, init));
        }

        public LFMatrix<T> DenseDiagonal(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return Dense(rows, columns);
            return Dense(LFDenseMatrixStorage<T>.FromDiagonalIndex(rows, columns, i => value));
        }

        /// <summary>
        /// Create a new diagonal dense matrix and initialize each diagonal value to the same provided value.
        /// </summary>
        public LFMatrix<T> DenseDiagonal(int order, T value)
        {
            if (Zero.Equals(value)) return Dense(order, order);
            return Dense(LFDenseMatrixStorage<T>.FromDiagonalIndex(order, order, i => value));
        }

        /// <summary>
        /// Create a new diagonal dense matrix and initialize each diagonal value using the provided init function.
        /// </summary>
        public LFMatrix<T> DenseDiagonal(int rows, int columns, Func<int, T> init)
        {
            return Dense(LFDenseMatrixStorage<T>.FromDiagonalIndex(rows, columns, init));
        }

        /// <summary>
        /// Create a new diagonal dense identity matrix with a one-diagonal.
        /// </summary>
        public LFMatrix<T> DenseIdentity(int rows, int columns)
        {
            return Dense(LFDenseMatrixStorage<T>.FromDiagonalIndex(rows, columns, i => One));
        }

        /// <summary>
        /// 单位矩阵
        /// </summary>
        public LFMatrix<T> DenseIdentity(int order)
        {
            return Dense(LFDenseMatrixStorage<T>.FromDiagonalIndex(order, order, i => One));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given other matrix.
        /// This new matrix will be independent from the other matrix.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfMatrix(LFMatrix<T> matrix)
        {
            return Dense(LFDenseMatrixStorage<T>.FromMatrix(matrix.Storage));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given two-dimensional array.
        /// This new matrix will be independent from the provided array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfArray(T[,] array)
        {
            return Dense(LFDenseMatrixStorage<T>.FromArray(array));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given indexed enumerable.
        /// Keys must be provided at most once, zero is assumed if a key is omitted.
        /// This new matrix will be independent from the enumerable.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfIndexed(int rows, int columns, IEnumerable<Tuple<int, int, T>> enumerable)
        {
            return Dense(LFDenseMatrixStorage<T>.FromIndexedEnumerable(rows, columns, enumerable));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable.
        /// The enumerable is assumed to be in column-major order (column by column).
        /// This new matrix will be independent from the enumerable.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumnMajor(int rows, int columns, IEnumerable<T> columnMajor)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnMajorEnumerable(rows, columns, columnMajor));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable of enumerable columns.
        /// Each enumerable in the master enumerable specifies a column.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumns(IEnumerable<IEnumerable<T>> data)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnArrays(data.Select(v => (v as T[]) ?? v.ToArray()).ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable of enumerable columns.
        /// Each enumerable in the master enumerable specifies a column.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumns(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnEnumerables(rows, columns, data));
        }

        /// <summary>
        /// Create a new dense matrix of T as a copy of the given column arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumnArrays(params T[][] columns)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnArrays(columns));
        }

        /// <summary>
        /// Create a new dense matrix of T as a copy of the given column arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumnArrays(IEnumerable<T[]> columns)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnArrays((columns as T[][]) ?? columns.ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given column vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumnVectors(params LFVector<T>[] columns)
        {
            var storage = new LFVectorStorage<T>[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                storage[i] = columns[i].Storage;
            }
            return Dense(LFDenseMatrixStorage<T>.FromColumnVectors(storage));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given column vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfColumnVectors(IEnumerable<LFVector<T>> columns)
        {
            return Dense(LFDenseMatrixStorage<T>.FromColumnVectors(columns.Select(c => c.Storage).ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable.
        /// The enumerable is assumed to be in row-major order (row by row).
        /// This new matrix will be independent from the enumerable.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRowMajor(int rows, int columns, IEnumerable<T> columnMajor)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowMajorEnumerable(rows, columns, columnMajor));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable of enumerable rows.
        /// Each enumerable in the master enumerable specifies a row.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRows(IEnumerable<IEnumerable<T>> data)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowArrays(data.Select(v => (v as T[]) ?? v.ToArray()).ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given enumerable of enumerable rows.
        /// Each enumerable in the master enumerable specifies a row.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRows(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowEnumerables(rows, columns, data));
        }

        /// <summary>
        /// Create a new dense matrix of T as a copy of the given row arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRowArrays(params T[][] rows)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowArrays(rows));
        }

        /// <summary>
        /// Create a new dense matrix of T as a copy of the given row arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRowArrays(IEnumerable<T[]> rows)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowArrays((rows as T[][]) ?? rows.ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given row vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRowVectors(params LFVector<T>[] rows)
        {
            var storage = new LFVectorStorage<T>[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                storage[i] = rows[i].Storage;
            }
            return Dense(LFDenseMatrixStorage<T>.FromRowVectors(storage));
        }

        /// <summary>
        /// Create a new dense matrix as a copy of the given row vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfRowVectors(IEnumerable<LFVector<T>> rows)
        {
            return Dense(LFDenseMatrixStorage<T>.FromRowVectors(rows.Select(r => r.Storage).ToArray()));
        }

        /// <summary>
        /// Create a new dense matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfDiagonalVector(LFVector<T> diagonal)
        {
            var m = Dense(diagonal.Count, diagonal.Count);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new dense matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfDiagonalVector(int rows, int columns, LFVector<T> diagonal)
        {
            var m = Dense(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new dense matrix with the diagonal as a copy of the given array.
        /// This new matrix will be independent from the array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfDiagonalArray(T[] diagonal)
        {
            var m = Dense(diagonal.Length, diagonal.Length);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new dense matrix with the diagonal as a copy of the given array.
        /// This new matrix will be independent from the array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DenseOfDiagonalArray(int rows, int columns, T[] diagonal)
        {
            var m = Dense(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new dense matrix from a 2D array of existing matrices.
        /// The matrices in the array are not required to be dense already.
        /// If the matrices do not align properly, they are placed on the top left
        /// corner of their cell with the remaining fields left zero.
        /// </summary>
        public LFMatrix<T> DenseOfMatrixArray(LFMatrix<T>[,] matrices)
        {
            var rowspans = new int[matrices.GetLength(0)];
            var colspans = new int[matrices.GetLength(1)];
            for (int i = 0; i < rowspans.Length; i++)
            {
                for (int j = 0; j < colspans.Length; j++)
                {
                    rowspans[i] = Math.Max(rowspans[i], matrices[i, j].RowCount);
                    colspans[j] = Math.Max(colspans[j], matrices[i, j].ColumnCount);
                }
            }
            var m = Dense(rowspans.Sum(), colspans.Sum());
            int rowoffset = 0;
            for (int i = 0; i < rowspans.Length; i++)
            {
                int coloffset = 0;
                for (int j = 0; j < colspans.Length; j++)
                {
                    m.SetSubMatrix(rowoffset, coloffset, matrices[i, j]);
                    coloffset += colspans[j];
                }
                rowoffset += rowspans[i];
            }
            return m;
        }
        #endregion

        #region Sparse
        /// <summary>
        /// Create a new sparse matrix straight from an initialized matrix storage instance.
        /// The storage is used directly without copying.
        /// Intended for advanced scenarios where you're working directly with
        /// storage for performance or interop reasons.
        /// </summary>
        /// <param name="storage">The LFSparseMatrixStorage</param>
        public abstract LFMatrix<T> Sparse(LFSparseMatrixStorage<T> storage);

        /// <summary>
        /// Create a sparse matrix of T with the given number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public LFMatrix<T> Sparse(int rows, int columns)
        {
            return Sparse(new LFSparseMatrixStorage<T>(rows, columns));
        }

        /// <summary>
        /// Create a new sparse matrix and initialize each value to the same provided value.
        /// </summary>
        public LFMatrix<T> Sparse(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return Sparse(rows, columns);
            return Sparse(LFSparseMatrixStorage<T>.OfValue(rows, columns, value));
        }

        /// <summary>
        /// Create a new sparse matrix and initialize each value using the provided init function.
        /// </summary>
        public LFMatrix<T> Sparse(int rows, int columns, Func<int, int, T> init)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfInit(rows, columns, init));
        }

        /// <summary>
        /// Create a new diagonal sparse matrix and initialize each diagonal value to the same provided value.
        /// </summary>
        public LFMatrix<T> SparseDiagonal(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return Sparse(rows, columns);
            return Sparse(LFSparseMatrixStorage<T>.OfDiagonalInit(rows, columns, i => value));
        }

        /// <summary>
        /// Create a new diagonal sparse matrix and initialize each diagonal value to the same provided value.
        /// </summary>
        public LFMatrix<T> SparseDiagonal(int order, T value)
        {
            if (Zero.Equals(value)) return Sparse(order, order);
            return Sparse(LFSparseMatrixStorage<T>.OfDiagonalInit(order, order, i => value));
        }

        /// <summary>
        /// Create a new diagonal sparse matrix and initialize each diagonal value using the provided init function.
        /// </summary>
        public LFMatrix<T> SparseDiagonal(int rows, int columns, Func<int, T> init)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfDiagonalInit(rows, columns, init));
        }

        /// <summary>
        /// Create a new diagonal dense identity matrix with a one-diagonal.
        /// </summary>
        public LFMatrix<T> SparseIdentity(int rows, int columns)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfDiagonalInit(rows, columns, i => One));
        }

        /// <summary>
        /// Create a new diagonal dense identity matrix with a one-diagonal.
        /// </summary>
        public LFMatrix<T> SparseIdentity(int order)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfDiagonalInit(order, order, i => One));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given other matrix.
        /// This new matrix will be independent from the other matrix.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfMatrix(LFMatrix<T> matrix)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfMatrix(matrix.Storage));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given two-dimensional array.
        /// This new matrix will be independent from the provided array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfArray(T[,] array)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfArray(array));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given indexed enumerable.
        /// Keys must be provided at most once, zero is assumed if a key is omitted.
        /// This new matrix will be independent from the enumerable.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfIndexed(int rows, int columns, IEnumerable<Tuple<int, int, T>> enumerable)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfIndexedEnumerable(rows, columns, enumerable));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given enumerable.
        /// The enumerable is assumed to be in row-major order (row by row).
        /// This new matrix will be independent from the enumerable.
        /// A new memory block will be allocated for storing the vector.
        /// </summary>
        /// <seealso href="http://en.wikipedia.org/wiki/Row-major_order"/>
        public LFMatrix<T> SparseOfRowMajor(int rows, int columns, IEnumerable<T> rowMajor)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowMajorEnumerable(rows, columns, rowMajor));
        }

        /// <summary>
        /// Create a new sparse matrix with the given number of rows and columns as a copy of the given array.
        /// The array is assumed to be in column-major order (column by column).
        /// This new matrix will be independent from the provided array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        /// <seealso href="http://en.wikipedia.org/wiki/Row-major_order"/>
        public LFMatrix<T> SparseOfColumnMajor(int rows, int columns, IList<T> columnMajor)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnMajorList(rows, columns, columnMajor));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given enumerable of enumerable columns.
        /// Each enumerable in the master enumerable specifies a column.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumns(IEnumerable<IEnumerable<T>> data)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnArrays(data.Select(v => (v as T[]) ?? v.ToArray()).ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given enumerable of enumerable columns.
        /// Each enumerable in the master enumerable specifies a column.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumns(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnEnumerables(rows, columns, data));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given column arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumnArrays(params T[][] columns)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnArrays(columns));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given column arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumnArrays(IEnumerable<T[]> columns)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnArrays((columns as T[][]) ?? columns.ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given column vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumnVectors(params LFVector<T>[] columns)
        {
            var storage = new LFVectorStorage<T>[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                storage[i] = columns[i].Storage;
            }
            return Sparse(LFSparseMatrixStorage<T>.OfColumnVectors(storage));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given column vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfColumnVectors(IEnumerable<LFVector<T>> columns)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfColumnVectors(columns.Select(c => c.Storage).ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given enumerable of enumerable rows.
        /// Each enumerable in the master enumerable specifies a row.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRows(IEnumerable<IEnumerable<T>> data)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowArrays(data.Select(v => (v as T[]) ?? v.ToArray()).ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given enumerable of enumerable rows.
        /// Each enumerable in the master enumerable specifies a row.
        /// This new matrix will be independent from the enumerables.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRows(int rows, int columns, IEnumerable<IEnumerable<T>> data)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowEnumerables(rows, columns, data));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given row arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRowArrays(params T[][] rows)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowArrays(rows));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given row arrays.
        /// This new matrix will be independent from the arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRowArrays(IEnumerable<T[]> rows)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowArrays((rows as T[][]) ?? rows.ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given row vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRowVectors(params LFVector<T>[] rows)
        {
            var storage = new LFVectorStorage<T>[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                storage[i] = rows[i].Storage;
            }
            return Sparse(LFSparseMatrixStorage<T>.OfRowVectors(storage));
        }

        /// <summary>
        /// Create a new sparse matrix as a copy of the given row vectors.
        /// This new matrix will be independent from the vectors.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfRowVectors(IEnumerable<LFVector<T>> rows)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfRowVectors(rows.Select(r => r.Storage).ToArray()));
        }

        /// <summary>
        /// Create a new sparse matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfDiagonalVector(LFVector<T> diagonal)
        {
            var m = Sparse(diagonal.Count, diagonal.Count);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new sparse matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfDiagonalVector(int rows, int columns, LFVector<T> diagonal)
        {
            var m = Sparse(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new sparse matrix with the diagonal as a copy of the given array.
        /// This new matrix will be independent from the array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfDiagonalArray(T[] diagonal)
        {
            var m = Sparse(diagonal.Length, diagonal.Length);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new sparse matrix with the diagonal as a copy of the given array.
        /// This new matrix will be independent from the array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> SparseOfDiagonalArray(int rows, int columns, T[] diagonal)
        {
            var m = Sparse(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new sparse matrix from a 2D array of existing matrices.
        /// The matrices in the array are not required to be sparse already.
        /// If the matrices do not align properly, they are placed on the top left
        /// corner of their cell with the remaining fields left zero.
        /// </summary>
        public LFMatrix<T> SparseOfMatrixArray(LFMatrix<T>[,] matrices)
        {
            var rowspans = new int[matrices.GetLength(0)];
            var colspans = new int[matrices.GetLength(1)];
            for (int i = 0; i < rowspans.Length; i++)
            {
                for (int j = 0; j < colspans.Length; j++)
                {
                    rowspans[i] = Math.Max(rowspans[i], matrices[i, j].RowCount);
                    colspans[j] = Math.Max(colspans[j], matrices[i, j].ColumnCount);
                }
            }
            var m = Sparse(rowspans.Sum(), colspans.Sum());
            int rowoffset = 0;
            for (int i = 0; i < rowspans.Length; i++)
            {
                int coloffset = 0;
                for (int j = 0; j < colspans.Length; j++)
                {
                    m.SetSubMatrix(rowoffset, coloffset, matrices[i, j]);
                    coloffset += colspans[j];
                }
                rowoffset += rowspans[i];
            }
            return m;
        }


        // Representation of Sparse LFMatrix
        //
        // LFMatrix A = [ 0 b 0 h 0 0 ]
        //            [ a c e i 0 0 ]
        //            [ 0 0 f j l n ]
        //            [ 0 d g k m 0 ]
        //
        // rows = 4, columns = 6, valueCount = 14
        //
        // (1) COO, Coordinate, ijv, or triplet format:
        //     cooRowIndices     = { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3 }
        //     cooColumnIndices  = { 1, 3, 0, 1, 2, 3, 2, 3, 4, 5, 1, 2, 3, 4 }
        //     cooValues         = { b, h, a, c, e, i, f, j, l, n, d, g, k, m }
        //
        // (2) CSR, Compressed Sparse Row or Compressed Row Storage(CRS) or Yale format:
        //     csrRowPointers    = { 0, 2, 6, 10, 14 }
        //     csrColumnIndices  = { 1, 3, 0, 1, 2, 3, 2, 3, 4, 5, 1, 2, 3, 4 }
        //     csrValues         = { b, h, a, c, e, i, f, j, l, n, d, g, k, m }
        //
        // (3) CSC, Compressed Sparse Column or Compressed Column Storage(CCS) format:
        //     cscColumnPointers = { 0, 1, 4, 7, 11, 13, 14 }
        //     cscRowIndices     = { 1, 0, 1, 3, 1, 2, 3, 0, 1, 2, 3, 2, 3, 2 }
        //     cscValues         = { a, b, c, d, e, f, g, h, i, j, k, l, m, n }

        /// <summary>
        /// Create a new sparse matrix from a coordinate format.
        /// This new matrix will be independent from the given arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="valueCount">The number of stored values including explicit zeros.</param>
        /// <param name="rowIndices">The row index array of the coordinate format.</param>
        /// <param name="columnIndices">The column index array of the coordinate format.</param>
        /// <param name="values">The data array of the coordinate format.</param>
        /// <returns>The sparse matrix from the coordinate format.</returns>
        /// <remarks>Duplicate entries will be summed together and explicit zeros will be not intentionally removed.</remarks>
        public LFMatrix<T> SparseFromCoordinateFormat(int rows, int columns, int valueCount, int[] rowIndices, int[] columnIndices, T[] values)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfCoordinateFormat(rows, columns, valueCount, rowIndices, columnIndices, values));
        }

        /// <summary>
        /// Create a new sparse matrix from a compressed sparse row format.
        /// This new matrix will be independent from the given arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="valueCount">The number of stored values including explicit zeros.</param>
        /// <param name="rowPointers">The row pointer array of the compressed sparse row format.</param>
        /// <param name="columnIndices">The column index array of the compressed sparse row format.</param>
        /// <param name="values">The data array of the compressed sparse row format.</param>
        /// <returns>The sparse matrix from the compressed sparse row format.</returns>
        /// <remarks>Duplicate entries will be summed together and explicit zeros will be not intentionally removed.</remarks>
        public LFMatrix<T> SparseFromCompressedSparseRowFormat(int rows, int columns, int valueCount, int[] rowPointers, int[] columnIndices, T[] values)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfCompressedSparseRowFormat(rows, columns, valueCount, rowPointers, columnIndices, values));
        }

        /// <summary>
        /// Create a new sparse matrix from a compressed sparse column format.
        /// This new matrix will be independent from the given arrays.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="valueCount">The number of stored values including explicit zeros.</param>
        /// <param name="rowIndices">The row index array of the compressed sparse column format.</param>
        /// <param name="columnPointers">The column pointer array of the compressed sparse column format.</param>
        /// <param name="values">The data array of the compressed sparse column format.</param>
        /// <returns>The sparse matrix from the compressed sparse column format.</returns>
        /// <remarks>Duplicate entries will be summed together and explicit zeros will be not intentionally removed.</remarks>
        public LFMatrix<T> SparseFromCompressedSparseColumnFormat(int rows, int columns, int valueCount, int[] rowIndices, int[] columnPointers, T[] values)
        {
            return Sparse(LFSparseMatrixStorage<T>.OfCompressedSparseColumnFormat(rows, columns, valueCount, rowIndices, columnPointers, values));
        }
        #endregion

        #region Diagonal
        /// <summary>
        /// 从对角矩阵内存构造对角矩阵
        /// </summary>
        public abstract LFMatrix<T> Diagonal(LFDiagonalMatrixStorage<T> storage);

        /// <summary>
        /// Create a new diagonal matrix with the given number of rows and columns.
        /// All cells of the matrix will be initialized to zero.
        /// </summary>
        public LFMatrix<T> Diagonal(int rows, int columns)
        {
            return Diagonal(new LFDiagonalMatrixStorage<T>(rows, columns));
        }

        /// <summary>
        /// Create a new diagonal matrix with the given number of rows and columns directly binding to a raw array.
        /// The array is assumed to represent the diagonal values and is used directly without copying.
        /// Very efficient, but changes to the array and the matrix will affect each other.
        /// </summary>
        public LFMatrix<T> Diagonal(int rows, int columns, T[] storage)
        {
            return Diagonal(new LFDiagonalMatrixStorage<T>(rows, columns, storage));
        }

        /// <summary>
        /// Create a new square diagonal matrix directly binding to a raw array.
        /// The array is assumed to represent the diagonal values and is used directly without copying.
        /// Very efficient, but changes to the array and the matrix will affect each other.
        /// </summary>
        public LFMatrix<T> Diagonal(T[] storage)
        {
            return Diagonal(new LFDiagonalMatrixStorage<T>(storage.Length, storage.Length, storage));
        }

        /// <summary>
        /// Create a new diagonal matrix and initialize each diagonal value to the same provided value.
        /// </summary>
        public LFMatrix<T> Diagonal(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return Diagonal(rows, columns);
            return Diagonal(LFDiagonalMatrixStorage<T>.OfValue(rows, columns, value));
        }

        /// <summary>
        /// Create a new diagonal matrix and initialize each diagonal value using the provided init function.
        /// </summary>
        public LFMatrix<T> Diagonal(int rows, int columns, Func<int, T> init)
        {
            return Diagonal(LFDiagonalMatrixStorage<T>.OfInit(rows, columns, init));
        }

        /// <summary>
        /// Create a new diagonal identity matrix with a one-diagonal.
        /// </summary>
        public LFMatrix<T> DiagonalIdentity(int rows, int columns)
        {
            return Diagonal(LFDiagonalMatrixStorage<T>.OfValue(rows, columns, One));
        }

        /// <summary>
        /// Create a new diagonal identity matrix with a one-diagonal.
        /// </summary>
        public LFMatrix<T> DiagonalIdentity(int order)
        {
            return Diagonal(LFDiagonalMatrixStorage<T>.OfValue(order, order, One));
        }


        /// <summary>
        /// Create a new diagonal matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DiagonalOfDiagonalVector(LFVector<T> diagonal)
        {
            var m = Diagonal(diagonal.Count, diagonal.Count);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new diagonal matrix with the diagonal as a copy of the given vector.
        /// This new matrix will be independent from the vector.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DiagonalOfDiagonalVector(int rows, int columns, LFVector<T> diagonal)
        {
            var m = Diagonal(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// Create a new diagonal matrix with the diagonal as a copy of the given array.
        /// This new matrix will be independent from the array.
        /// A new memory block will be allocated for storing the matrix.
        /// </summary>
        public LFMatrix<T> DiagonalOfDiagonalArray(T[] diagonal)
        {
            var m = Diagonal(diagonal.Length, diagonal.Length);
            m.SetDiagonal(diagonal);
            return m;
        }

        /// <summary>
        /// 创建一个新的对角线矩阵，对角线作为给定数组的副本。
        /// 这个新矩阵将独立于数组。
        /// 将分配一个新的内存块来存储矩阵。
        /// </summary>
        public LFMatrix<T> DiagonalOfDiagonalArray(int rows, int columns, T[] diagonal)
        {
            var m = Diagonal(rows, columns);
            m.SetDiagonal(diagonal);
            return m;
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 向量构造器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LFVectorBuilder<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public abstract T Zero { get; }

        public abstract T One { get; }

        public LFVector<T> FromStorage(LFVectorStorage<T> storage)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));

            if (storage is LFDenseVectorStorage<T> dense) return Dense(dense);
            if (storage is LFSparseVectorStorage<T> sparse) return Sparse(sparse);

            throw new NotSupportedException(FormattableString.Invariant($"LFVector storage type '{storage.GetType().Name}' is not supported. Only DenseVectorStorage and SparseVectorStorage are supported as this point."));

        }
    }
    #endregion


}