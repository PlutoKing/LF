/*──────────────────────────────────────────────────────────────
 * FileName     : LFMap2D
 * Created      : 2020-10-21 09:28:36
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.Mathematics;
using LF.Mathematics.IOA;

namespace LF.FlightControl.Guidance
{
    public class LFMap2D
    {
        #region Fields
        private double _width;
        private double _length;

        private List<Obstacle2D> _obstacle2DList = new List<Obstacle2D>();

        private LFPoint2D _start;
        private LFPoint2D _end;
        private List<LFPoint2D> _waypoints = new List<LFPoint2D>();

        private LFMap2D subMap;


        LFRoute2D[,] routeMat;
        LFMatrix distMat;
        #endregion

        #region Properties

        /// <summary>
        /// 地图宽度
        /// </summary>
        public double Width { get => _width; set => _width = value; }

        /// <summary>
        /// 地图长度
        /// </summary>
        public double Length { get => _length; set => _length = value; }

        /// <summary>
        /// 障碍物列表
        /// </summary>
        public List<Obstacle2D> Obstacle2DList { get => _obstacle2DList; set => _obstacle2DList = value; }

        /// <summary>
        /// 起点
        /// </summary>
        public LFPoint2D Start { get => _start; set => _start = value; }

        /// <summary>
        /// 终点
        /// </summary>
        public LFPoint2D End { get => _end; set => _end = value; }

        /// <summary>
        /// 途经点
        /// </summary>
        public List<LFPoint2D> Waypoints { get => _waypoints; set => _waypoints = value; }

        /// <summary>
        /// 子地图
        /// </summary>
        public LFMap2D SubMap { get => subMap; set => subMap = value; }

        /// <summary>
        /// 路径矩阵
        /// </summary>
        public LFRoute2D[,] RouteMat { get => routeMat; set => routeMat = value; }

        /// <summary>
        /// 距离矩阵
        /// </summary>
        public LFMatrix DistMat { get => distMat; set => distMat = value; }
        #endregion

        #region Constructors
        public LFMap2D()
        {
        }

        #endregion

        #region Methods

        #region 路径规划
        public LFRoute2D GetRoute()
        {
            LFRoute2D r = new LFRoute2D();
            r.Add(Start);
            r.Add(End);
            return r;
        }
        #endregion

        #region 路径规划 不考虑障碍物

        /// <summary>
        /// 遗传算法+可视图
        /// </summary>
        /// <returns></returns>
        public LFRoute2D GetRoute_GA(int iter, out double length, out double[] history)
        {
            distMat = GetEuclideanDistanceMatrix();

            LFGa ga = new LFGa(iter);
            ga.FitnessFunction += Ga_FitnessFunction_1;

            LFChromosome chromosome;
            ga.SolveTsp(_waypoints.Count, out chromosome, out length, out history);

            return GetOriginRoute(chromosome.Codes);
        }


        #endregion

        #region 路径规划 方案1 调用函数

        /// <summary>
        /// 可视图+遗传算法
        /// </summary>
        public LFRoute2D GetRoute_VG_GA(int iter, out double length, out double[] history)
        {
            distMat = GetRealDistanceMatrix(out routeMat);

            LFGa ga = new LFGa(iter);
            ga.FitnessFunction += Ga_FitnessFunction_1;

            LFChromosome chromosome;
            ga.SolveTsp(_waypoints.Count, out chromosome, out length, out history);

            return GetRealRoute(chromosome.Codes, routeMat);
        }

        /// <summary>
        /// 方案1 遗传算法适度值函数
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        private double Ga_FitnessFunction_1(LFChromosome chromosome)
        {

            double f = 0;
            f += distMat[0, chromosome.Codes[0] + 1];
            for (int i = 0; i < chromosome.Codes.Length - 1; i++)
            {
                f += distMat[chromosome.Codes[i] + 1, chromosome.Codes[i + 1] + 1];
            }
            f += distMat[chromosome.Codes[chromosome.Codes.Length - 1] + 1, distMat.ColCount - 1];

            return 1000 / f;
        }
        #endregion

        #region 路径规划 方案2 调用函数

        /// <summary>
        /// 遗传算法+可视图
        /// </summary>
        /// <returns></returns>
        public LFRoute2D GetRoute_GA_VG(int iter, out double length, out double[] history)
        {
            distMat = GetEuclideanDistanceMatrix();

            LFGa ga = new LFGa(iter);
            ga.FitnessFunction += Ga_FitnessFunction_2;

            LFChromosome chromosome;
            ga.SolveTsp(_waypoints.Count, out chromosome, out length, out history);

            return GetCorrectedRoute(chromosome.Codes);
        }

        private double Ga_FitnessFunction_2(LFChromosome chromosome)
        {
            double f = 0;
            f += distMat[0, chromosome.Codes[0] + 1];
            for (int i = 0; i < chromosome.Codes.Length - 1; i++)
            {
                f += distMat[chromosome.Codes[i] + 1, chromosome.Codes[i + 1] + 1];
            }
            f += distMat[chromosome.Codes[chromosome.Length - 1] + 1, distMat.ColCount - 1];

            return 1000 / f;
        }

        #endregion

        #region 模拟退火+可视图

        //public LFRoute2D GetRoute_SA_VG(out double length, out double[] history)
        //{
        //    distMat = GetEuclideanDistanceMatrix();

        //    SA sa = new SA();
        //    sa.T0 = 100;
        //    sa.Tmin = 1e-8;
        //    sa.Delta = 0.98;
        //    sa.K = 32;

        //    sa.EnergyFunction += Sa_EnergyFunction;

        //    int[] routeOrder;
        //    sa.SolveTsp(_waypoints.Count, out routeOrder, out length, out history);

        //    return GetCorrectedRoute(routeOrder);

        //}

        private double Sa_EnergyFunction(int[] x)
        {
            double f = 0;
            f += distMat[0, x[0] + 1];
            for (int i = 0; i < x.Length - 1; i++)
            {
                f += distMat[x[i] + 1, x[i + 1] + 1];
            }
            f += distMat[x[x.Length - 1] + 1, distMat.ColCount - 1];

            return f;
        }


        #endregion

        #region 可视图避障算法

        /// <summary>
        /// 生成避障路径
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public LFRoute2D GetRoute(double offset)
        {
            return GetRoute(new LFLine2D(_start, _end), offset);
        }

        /// <summary>
        /// 生成避障路径
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public LFRoute2D GetRoute(LFLine2D line, double offset)
        {

            //// 化简地图
            //SimplyMap(line, offset);

            //if (subMap._obstacle2DList.Count == 0)
            //{
            //    return new LFRoute2D(line.Start, line.End);
            //}

            //LFGraph<LFPoint2D> graph = subMap.GetGraph(line, offset);

            //SimplyGraph(graph, offset);

            LFGraph<LFPoint2D> graph = GetGraph(line, offset);

            int n = graph.Nodes.Length;
            int[,] pathMatrix = new int[n, n];
            double[] shortPath = new double[n];

            graph.ShortestRoute(ref pathMatrix, ref shortPath, 0);

            List<LFPoint2D> route = new List<LFPoint2D>();

            for (int i = 0; i < n; i++)
            {
                if (pathMatrix[1, i] != -1)
                {
                    route.Add(graph.Nodes[pathMatrix[1, i]].Data);
                }
                else
                {
                    break;
                }
            }

            return new LFRoute2D(route);
        }

        /// <summary>
        /// 地图化简
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        public void SimplyMap(LFLine2D line, double offset)
        {
            subMap = new LFMap2D();
            subMap.Start = _start.Clone();
            subMap.End = _start.Clone();
            foreach (LFPoint2D p in _waypoints)
            {
                subMap._waypoints.Add(p.Clone());
            }

            int N = _obstacle2DList.Count;
            bool[] state = new bool[N];

            List<Obstacle2D> otherList = new List<Obstacle2D>();

            for (int i = 0; i < N; i++)
            {
                state[i] = CheckRoute(line);
                if (state[i])
                {
                    subMap._obstacle2DList.Add(_obstacle2DList[i].Clone());
                }
                else
                {
                    otherList.Add(_obstacle2DList[i].Clone());
                }
            }

            if (subMap.Obstacle2DList.Count == 0)
            {
                return;
            }
            else
            {
                List<LFLine2D> boundary = subMap.GetBoundary(line, offset);
                foreach (Obstacle2D ob in otherList)
                {
                    foreach (LFLine2D l in boundary)
                    {
                        if (ob.IsIntersectWith(l))
                        {
                            subMap._obstacle2DList.Add(ob);
                            boundary = subMap.GetBoundary(line, offset);
                            break;
                        }
                    }
                }
            }

        }

        public void SimplyGraph(LFGraph<LFPoint2D> graph, double offset)
        {
            int N = graph.Nodes.Length;
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (graph.IsEdge(i, j))
                    {
                        LFPoint2D p1 = graph.Nodes[i].Data;
                        LFPoint2D p2 = graph.Nodes[j].Data;

                        LFPoint2D newP1 = p1 + 3 * offset / (p1 - p2).Norm() * (p1 - p2);
                        LFPoint2D newP2 = p2 + 3 * offset / (p1 - p2).Norm() * (p2 - p1);

                        if (subMap.CheckRoute(new LFLine2D(newP1, newP2)))
                        {
                            graph.DeleteEdge(i, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成图模型
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public LFGraph<LFPoint2D> GetGraph(LFLine2D line, double offset)
        {
            List<LFPoint2D> points = GetAllNodes(new List<LFPoint2D>() { line.Start, line.End }, offset);
            int n = points.Count;
            LFGraph<LFPoint2D> graph = new LFGraph<LFPoint2D>(n);
            for (int i = 0; i < n; i++)
            {
                graph.Nodes[i] = new LFNode<LFPoint2D>(points[i]);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    //if (i == 1 && j == 6)
                    //{
                    //    LFLine2D test = new LFLine2D(points[i], points[j]);
                    //    bool b = CheckRoute(test);
                    //    Console.WriteLine("stop");
                    //}
                    if (!CheckRoute(new LFLine2D(points[i], points[j])))
                    {
                        graph.SetEdge(i, j, LFPoint2D.Distance(points[i], points[j]));
                    }
                }
            }

            return graph;
        }

        /// <summary>
        /// 路径检测
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool CheckRoute(LFLine2D line)
        {
            foreach (Obstacle2D ob in _obstacle2DList)
            {
                if (ob.IsIntersectWith(line))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取所有顶点
        /// </summary>
        /// <param name="otherPoints"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<LFPoint2D> GetAllNodes(List<LFPoint2D> otherPoints, double offset)
        {
            List<LFPoint2D> points = new List<LFPoint2D>();

            if (otherPoints != null)
            {
                if (otherPoints.Count != 0)
                {
                    foreach (LFPoint2D p in otherPoints)
                    {
                        points.Add(p);
                    }
                }
            }

            for (int i = 0; i < _obstacle2DList.Count; i++)
            {
                for (int j = 0; j < _obstacle2DList[i].Points.Count; j++)
                {
                    points.Add(_obstacle2DList[i].Expand(offset).Points[j]);
                }
            }
            return points;
        }

        /// <summary>
        /// 可视图化简：获取有效障碍物
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<Obstacle2D> GetSubObscale2DList(int[] index)
        {
            List<Obstacle2D> obList = new List<Obstacle2D>();

            for (int i = 0; i < index.Length; i++)
            {
                obList.Add(_obstacle2DList[index[i]].Clone());
            }

            return obList;
        }

        /// <summary>
        /// 获取边界
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<LFLine2D> GetBoundary(LFLine2D line, double offset)
        {
            List<LFLine2D> lines = new List<LFLine2D>();
            List<LFPoint2D> points = GetAllNodes(new List<LFPoint2D>() { line.Start, line.End }, offset);
            List<LFPoint2D> ConvhullPoints = LFPoint2D.GetConvhull(points);
            int n = ConvhullPoints.Count;
            for (int i = 0; i < n; i++)
            {
                LFLine2D l = new LFLine2D(ConvhullPoints[i], ConvhullPoints[(i + 1) % n]);
                lines.Add(l);
            }
            return lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<LFPoint2D> GetAllWaypoints()
        {
            List<LFPoint2D> points = new List<LFPoint2D>();
            // 添加点
            points.Add(_start);
            foreach (LFPoint2D p in _waypoints)
            {
                points.Add(p);
            }
            points.Add(_end);

            return points;
        }
        #endregion

        #region 路径规划

        /// <summary>
        /// 欧氏距离矩阵
        /// </summary>
        private LFMatrix GetEuclideanDistanceMatrix()
        {
            List<LFPoint2D> points = GetAllWaypoints();

            int N = points.Count;
            LFMatrix distMat = new LFMatrix(N, N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    distMat[i, j] = LFPoint2D.Distance(points[i], points[j]);
                }
            }
            return distMat;
        }

        /// <summary>
        /// 获取真实距离矩阵，即路径矩阵
        /// </summary>
        /// <param name="routeMatrix"></param>
        /// <returns></returns>
        public LFMatrix GetRealDistanceMatrix(out LFRoute2D[,] routeMatrix)
        {
            List<LFPoint2D> points = GetAllWaypoints();

            int N = points.Count;
            LFMatrix distMat = new LFMatrix(N, N);
            routeMatrix = new LFRoute2D[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        distMat[i, j] = 0;
                    }
                    else
                    {
                        LFRoute2D r = GetRoute(new LFLine2D(points[i], points[j]), 1);
                        routeMatrix[i, j] = r;
                        distMat[i, j] = r.Length();
                    }

                }
            }
            return distMat;
        }

        /// <summary>
        /// 根据规划获取原始路径，未考虑避障
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public LFRoute2D GetOriginRoute(int[] codes)
        {
            LFRoute2D route = new LFRoute2D(_start);
            for (int i = 0; i < codes.Length; i++)
            {
                route.Add(_waypoints[codes[i]]);
            }
            route.Add(_end);
            return route;
        }

        /// <summary>
        /// 根据规划获取修正路径，考虑避障
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public LFRoute2D GetCorrectedRoute(int[] codes)
        {
            LFRoute2D route = GetOriginRoute(codes);

            LFRoute2D r = new LFRoute2D();
            for (int i = 0; i < route.Count - 1; i++)
            {
                LFLine2D line = new LFLine2D(route[i], route[i + 1]);
                LFRoute2D tmp = GetRoute(line, 1);
                r.AddRoute(tmp);
            }

            return r;
        }

        /// <summary>
        /// 根据真实进行规划，获取真实路径
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="routeMat"></param>
        /// <returns></returns>
        public LFRoute2D GetRealRoute(int[] codes, LFRoute2D[,] routeMat)
        {
            LFRoute2D route = new LFRoute2D();
            route.AddRoute(routeMat[0, codes[0] + 1]);
            for (int i = 0; i < codes.Length - 1; i++)
            {
                route.AddRoute(routeMat[codes[i] + 1, codes[i + 1] + 1]);
            }
            route.AddRoute(routeMat[codes[codes.Length - 1] + 1, routeMat.GetLength(1) - 1]);
            return route;

        }

        #endregion

        #region A*算法
        //public LFRoute2D AStar()
        //{
        //    List<LFPoint2D> Open = new List<LFPoint2D>();
        //    List<LFPoint2D> Closed = new List<LFPoint2D>();

        //    Open.Add(Start);

        //    while (Open.Count != 0)
        //    {
        //        if()
        //    }
        //}

        //public double f(poin)
        #endregion

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
