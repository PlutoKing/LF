/*──────────────────────────────────────────────────────────────
 * FileName     : LFGraph
 * Created      : 2020-10-13 11:00:09
 * Author       : Xu Zhe
 * Description  : 图
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Mathematics
{
    /// <summary>
    /// 图
    /// </summary>
    public class LFGraph<T>
    {
        #region Fields
        private LFNode<T>[] _nodes;
        private int _edgeCount;
        private double[,] _matrix;
        private bool _isDigraph = false;    // 是否有向图
        #endregion

        #region Properties
        public LFNode<T>[] Nodes { get => _nodes; set => _nodes = value; }
        public int VertexCount { get { return _nodes.Length; } }
        public int EdgeCount { get => _edgeCount; set => _edgeCount = value; }
        public bool IsDigraph { get => _isDigraph; set => _isDigraph = value; }
        public double[,] Matrix { get => _matrix; set => _matrix = value; }

        #endregion

        #region Constructors
        public LFGraph(int n)
        {
            _nodes = new LFNode<T>[n];
            _matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        _matrix[i, j] = 0;
                    else
                        _matrix[i, j] = double.MaxValue;
                }
            }
            _edgeCount = 0;
        }
        #endregion

        #region Methods

        #region Basic Methods
        /// <summary>
        /// 判断顶点<paramref name="v1"/>和顶点<paramref name="v2"/>之间是否有边
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public bool IsEdge(LFNode<T> v1, LFNode<T> v2)
        {
            if (!IsNode(v1) || !IsNode(v2))
            {
                throw new Exception("LFNode is not belong to LFGraph");
            }

            int m = GetIndex(v1);
            int n = GetIndex(v2);
            if (_matrix[m, n] != 0)
                return true;
            else
                return false;
        }

        public bool IsEdge(int m, int n)
        {

            if (_matrix[m, n] != 0 && _matrix[m, n] != double.MaxValue)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 在顶点<paramref name="v1"/>和顶点<paramref name="v2"/>之间设置边
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="w"></param>
        public void SetEdge(LFNode<T> v1, LFNode<T> v2, double w)
        {
            if (!IsNode(v1) || !IsNode(v2))
            {
                throw new Exception("LFNode is not belong to LFGraph");
            }

            int m = GetIndex(v1);
            int n = GetIndex(v2);
            _matrix[m, n] = w;
            if (!IsDigraph)
                _matrix[n, m] = w;
            _edgeCount++;
        }

        public void SetEdge(int m, int n, double w)
        {
            _matrix[m, n] = w;
            if (!IsDigraph)
                _matrix[n, m] = w;
            _edgeCount++;
        }

        /// <summary>
        /// 删除顶点<paramref name="v1"/>和顶点<paramref name="v2"/>之间的边
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        public void DeleteEdge(LFNode<T> v1, LFNode<T> v2)
        {
            if (!IsNode(v1) || !IsNode(v2))
            {
                throw new Exception("LFNode is not belong to LFGraph");
            }

            int m = GetIndex(v1);
            int n = GetIndex(v2);
            if (_matrix[m, n] != 0)
            {
                _matrix[m, n] = double.MaxValue;
                _matrix[n, m] = double.MaxValue;
                _edgeCount--;
            }
        }

        public void DeleteEdge(int m, int n)
        {

            if (_matrix[m, n] != 0)
            {
                _matrix[m, n] = double.MaxValue;
                _matrix[n, m] = double.MaxValue;
                _edgeCount--;
            }
        }

        /// <summary>
        /// 按照索引<paramref name="index"/>获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LFNode<T> GetNode(int index)
        {
            return _nodes[index];
        }

        /// <summary>
        /// 设置顶点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="v"></param>
        public void SetNode(int index, LFNode<T> v)
        {
            _nodes[index] = v;
        }

        /// <summary>
        /// 获取权重
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public double GetWeight(int m, int n)
        {
            return _matrix[m, n];
        }

        /// <summary>
        /// 设置权重
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <param name="w"></param>
        public void SetWeight(int m, int n, double w)
        {
            _matrix[m, n] = w;

            if (!_isDigraph)
                _matrix[n, m] = w;
        }

        /// <summary>
        /// 判断是否是图中顶点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsNode(LFNode<T> v)
        {
            foreach (LFNode<T> node in _nodes)
                return true;

            return false;
        }

        /// <summary>
        /// 获取顶点<paramref name="v"/>的索引
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int GetIndex(LFNode<T> v)
        {
            int i = -1;

            for (i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i].Equals(v))
                    return i;
            }

            return i;
        }

        #endregion

        #region Algorithms

        /// <summary>
        /// Dijkstra算法求解短路路径
        /// </summary>
        /// <param name="pathMatrix"></param>
        /// <param name="shortPath"></param>
        /// <param name="startIndex"></param>
        public void ShortestRoute(ref int[,] pathMatrix, ref double[] shortPath, int startIndex)
        {
            int k = 0;
            bool[] final = new bool[_nodes.Length];

            int count = _nodes.Length;
            // 初始化
            for (int i = 0; i < count; i++)
            {
                final[i] = false;   // 全部是否
                shortPath[i] = _matrix[startIndex, i];

                for (int j = 0; j < count; j++)
                    pathMatrix[i, j] = -1;

                if (shortPath[i] != 0 && shortPath[i] < double.MaxValue)
                {
                    pathMatrix[i, 0] = startIndex;
                    pathMatrix[i, 1] = i;
                }
            }

            // start为源点
            shortPath[startIndex] = 0;
            final[startIndex] = true;

            // 处理从源点到其余顶点的最短路径
            for (int i = 0; i < count; i++)
            {
                double min = double.MaxValue;

                // 从源点start到其余顶点的路径长度
                for (int j = 0; j < count; j++)
                {
                    // 从源点start到j顶点的最短路径还未找到
                    if (!final[j] && shortPath[j] < min)
                    {
                        k = j;
                        min = shortPath[j];
                    }
                }

                // 源点到顶点k的路径长度最小
                final[k] = true;

                // 更新当前最短路径及距离
                for (int j = 0; j < count; j++)
                {
                    if (!final[j] && (min + _matrix[k, j] < shortPath[j]))
                    {
                        shortPath[j] = min + _matrix[k, j];
                        for (int w = 0; w < count; w++)
                        {
                            pathMatrix[j, w] = pathMatrix[k, w];
                            if (pathMatrix[j, w] == -1)
                            {
                                pathMatrix[j, w] = j;
                                break;
                            }
                        }

                    }
                }
            }
        }
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
