/*──────────────────────────────────────────────────────────────
 * FileName     : LFCatia.cs
 * Created      : 2021-06-12 23:14:47
 * Author       : Xu Zhe
 * Description  : Catia管理类，实现Catia创成式曲面中的基础操作
 *                1. 基本图形绘制
 *                2. 曲面绘制
 *                3. 基本操作：对称、平移、仿射等
 *                4. 曲面操作：桥接
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Runtime.InteropServices;
using INFITF;
using MECMOD;
using HybridShapeTypeLib;

namespace LF.CatiaCSharp
{
    /// <summary>
    /// Catia管理类
    /// </summary>
    public class LFCatia
    {
        #region Fields
        public Application CatiaApp;            // Catia应用程序
        public PartDocument Doc;                // 零件文件
        public Part Part;                       // 零件
        public HybridBodies Bodies;             // 混合体集合
        public HybridBody Body;                 // 混合体
        public HybridShapeFactory Shapes;       // 混合形状
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFCatia()
        {
        }
        #endregion

        #region Methods

        #region Startup
        /// <summary>
        /// 启动CATIA
        /// </summary>
        public void Startup()
        {
            try
            {
                CatiaApp = (INFITF.Application)Marshal.GetActiveObject("CATIA.Application");
            }
            catch
            {
                Type oType = System.Type.GetTypeFromProgID("CATIA.Application");
                CatiaApp = (INFITF.Application)Activator.CreateInstance(oType);
                CatiaApp.Visible = true;
            }

            try
            {
                Doc = (PartDocument)CatiaApp.ActiveDocument;
            }
            catch { }

            if (Doc == null)
            {
                Doc = (PartDocument)CatiaApp.Documents.Add("Part");
            }
            // 初始化
            Part = Doc.Part;
            Bodies = Part.HybridBodies;
            Body = Bodies.Add();
            Shapes = (HybridShapeFactory)Part.HybridShapeFactory;
        }

        #endregion

        #region Operating Methods
        /// <summary>
        /// 设定参考平面XY
        /// </summary>
        /// <returns></returns>
        public Reference SetPlaneXY()
        {
            OriginElements element = Part.OriginElements;
            HybridShapePlaneExplicit shape = (HybridShapePlaneExplicit)element.PlaneXY;
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 设定参考平面YZ
        /// </summary>
        /// <returns></returns>
        public Reference SetPlaneYZ()
        {
            OriginElements element = Part.OriginElements;
            HybridShapePlaneExplicit shape = (HybridShapePlaneExplicit)element.PlaneYZ;
            return Part.CreateReferenceFromObject(shape);
        }
        /// <summary>
        /// 设定参考平面ZX
        /// </summary>
        /// <returns></returns>
        public Reference SetPlaneZX()
        {
            OriginElements element = Part.OriginElements;
            HybridShapePlaneExplicit shape = (HybridShapePlaneExplicit)element.PlaneZX;
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 设定参考平面：与参考面相距d
        /// </summary>
        /// <param name="plane">一直参考面</param>
        /// <param name="d">距离</param>
        /// <returns></returns>
        public Reference SetPlane(Reference plane, double d)
        {
            HybridShapePlaneOffset shape = Shapes.AddNewPlaneOffset(plane, d, false);
            Shapes.GSMVisibility(plane, 0);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();

            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 缝合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Reference Join(Reference[] obj)
        {
            int n = obj.Length;
            HybridShapeAssemble shape = Shapes.AddNewJoin(obj[0], obj[1]);

            shape.SetConnex(true);
            shape.SetManifold(true);
            shape.SetSimplify(false);
            shape.SetSuppressMode(false);
            shape.SetDeviation(0.001);
            shape.SetAngularToleranceMode(false);
            shape.SetAngularTolerance(0.5);
            shape.SetFederationPropagation(0);
            Shapes.GSMVisibility(obj[0], 0);
            Shapes.GSMVisibility(obj[1], 0);
            if (n > 2)
            {
                for (int i = 2; i < n; i++)
                {
                    shape.AddElement(obj[i]);
                    Shapes.GSMVisibility(obj[i], 0);
                }
            }

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();

            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 对称
        /// </summary>
        /// <param name="refshape"></param>
        /// <param name="refPlane"></param>
        /// <returns></returns>
        public Reference GetSymmetry(Reference refshape, Reference refPlane)
        {
            HybridShapeSymmetry shape = Shapes.AddNewSymmetry(refshape, refPlane);
            shape.VolumeResult = false;
            Shapes.GSMVisibility(refPlane, 0);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 裁剪
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cutObj"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Reference Cut(Reference obj, Reference cutObj, int d)
        {
            HybridShapeSplit shape = Shapes.AddNewHybridSplit(obj, cutObj, d);
            Shapes.GSMVisibility(obj, 0);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 倒圆角
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="d1"></param>
        /// <param name="l2"></param>
        /// <param name="d2"></param>
        /// <param name="r"></param>
        /// <param name="boc"></param>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public Reference GetCorner(Reference l1, int d1, Reference l2, int d2, double r, int boc, int o1, int o2)
        {
            HybridShapeDirection hybridShapeDirection1 = Shapes.AddNewDirectionByCoord(0, 0, 0);
            HybridShapeCorner shape = Shapes.AddNew3DCorner(l1, l2, hybridShapeDirection1, r, d1, d2, false);
            shape.DiscriminationIndex = 1;

            shape.BeginOfCorner = boc;
            shape.FirstTangentOrientation = o1;
            shape.SecondTangentOrientation = o2;

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 相交
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="near"></param>
        /// <returns></returns>
        public Reference GetIntersection(Reference s1, Reference s2, Reference near)
        {
            HybridShapeIntersection shape = Shapes.AddNewIntersection(s1, s2);
            shape.PointType = 0;
            Reference r = Part.CreateReferenceFromObject(shape);

            HybridShapeNear shape1 = Shapes.AddNewNear(r, near);

            Shapes.GSMVisibility(s1, 0);
            Shapes.GSMVisibility(s2, 0);
            Shapes.GSMVisibility(near, 0);

            Body.AppendHybridShape(shape1);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape1);
        }

        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Reference Fill(Reference s)
        {
            HybridShapeFill shape = Shapes.AddNewFill();
            shape.AddBound(s);
            Shapes.GSMVisibility(s, 0);
            shape.Continuity = 0;
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Reference Fill(Reference[] s)
        {
            int n = s.Length;
            HybridShapeFill shape = Shapes.AddNewFill();
            for (int i = 0; i < n; i++)
            {
                shape.AddBound(s[i]);
                Shapes.GSMVisibility(s[i], 0);
            }
            shape.Continuity = 0;
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        #region 曲面操作
        /// <summary>
        /// 桥接
        /// </summary>
        /// <param name="s1">参考面1</param>
        /// <param name="l1">参考线1</param>
        /// <param name="d1">方向1</param>
        /// <param name="s2">参考面2</param>
        /// <param name="l2">参考线2</param>
        /// <param name="d2">方向2</param>
        /// <returns></returns>
        public Reference Blend(Reference s1, Reference l1, int d1, Reference s2, Reference l2, int d2)
        {
            HybridShapeBlend shape = Shapes.AddNewBlend();
            shape.Coupling = 1;

            shape.SetCurve(1, l1);
            shape.SetOrientation(1, 1);
            shape.SetSupport(1, s1);
            shape.SetTransition(1, d1);
            shape.SetContinuity(1, 1);
            shape.SetTrimSupport(1, 1);
            shape.SetBorderMode(1, 1);
            shape.SetTensionInDouble(1, 1, 0, 0);


            shape.SetCurve(2, l2);

            shape.SetOrientation(2, 1);
            shape.SetSupport(2, s2);
            shape.SetTransition(2, d2);
            shape.SetContinuity(2, 1);
            shape.SetTrimSupport(2, 1);
            shape.SetBorderMode(2, 1);
            shape.SetTensionInDouble(2, 1, 0, 0);

            shape.SmoothAngleThresholdActivity = false;
            shape.SmoothDeviationActivity = false;
            shape.RuledDevelopableSurface = false;

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        public HybridShapeBlend GetBlend(Reference s1, Reference l1, int d1, Reference s2, Reference l2, int d2)
        {
            HybridShapeBlend shape = Shapes.AddNewBlend();
            shape.Coupling = 1;

            shape.SetCurve(1, l1);
            shape.SetOrientation(1, 1);
            shape.SetSupport(1, s1);
            shape.SetTransition(1, d1);
            shape.SetContinuity(1, 1);
            shape.SetTrimSupport(1, 1);
            shape.SetBorderMode(1, 1);
            shape.SetTensionInDouble(1, 1, 0, 0);


            shape.SetCurve(2, l2);

            shape.SetOrientation(2, 1);
            shape.SetSupport(2, s2);
            shape.SetTransition(2, d2);
            shape.SetContinuity(2, 1);
            shape.SetTrimSupport(2, 1);
            shape.SetBorderMode(2, 1);
            shape.SetTensionInDouble(2, 1, 0, 0);

            shape.SmoothAngleThresholdActivity = false;
            shape.SmoothDeviationActivity = false;
            shape.RuledDevelopableSurface = false;

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return shape;
        } 
        #endregion

        #endregion

        #region Common Drawing Methods

        #region Point Drawing Methods
        /// <summary>
        /// 绘制点：由三维坐标x,y,z绘制点
        /// </summary>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        /// <param name="z">坐标z</param>
        /// <returns>点的参考</returns>
        public Reference DrawPoint(double x, double y, double z)
        {
            HybridShapePointCoord shape = Shapes.AddNewPointCoord(x, y, z);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 绘制点：基于参考点绘制相对坐标点
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Reference DrawPoint(Reference p, double x, double y, double z)
        {
            HybridShapePointCoord shape = Shapes.AddNewPointCoord(x, y, z);
            shape.PtRef = p;
            Shapes.GSMVisibility(p, 0);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 绘制中点：取两点中点
        /// </summary>
        /// <param name="p1">参考点1</param>
        /// <param name="p2">参考点2</param>
        /// <returns>终点的参考对象</returns>
        public Reference DrawCenterPoint(Reference p1, Reference p2)
        {
            HybridShapePointBetween shape = Shapes.AddNewPointBetween(p1, p2, 0.5, 1);
            Shapes.GSMVisibility(p1, 0);
            Shapes.GSMVisibility(p2, 0);
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }
        #endregion

        #region Line Drawing Methods
        /// <summary>
        /// 绘制直线：两点连线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public Reference DrawLine(Reference p1, Reference p2)
        {
            HybridShapeLinePtPt shape = Shapes.AddNewLinePtPt(p1, p2);
            Body.AppendHybridShape(shape);

            /* 隐藏两点 */
            Shapes.GSMVisibility(p1, 0);
            Shapes.GSMVisibility(p2, 0);

            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }
        #endregion

        #region Spline Drawing Methods

        /// <summary>
        /// 绘制样条线：多点无约束
        /// </summary>
        /// <param name="points">参考点</param>
        /// <returns></returns>
        public Reference DrawSpline(Reference[] points)
        {
            int n = points.Length;
            HybridShapeSpline shape = Shapes.AddNewSpline();
            shape.SetSplineType(0);
            shape.SetClosing(0);
            for (int i = 0; i < n; i++)
            {
                shape.AddPointWithConstraintExplicit(points[i], null, -1.0f, 1, null, 0);
                Shapes.GSMVisibility(points[i], 0);
            }
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();

            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 绘制样条线：与某一直线相切
        /// </summary>
        /// <param name="points">参考点</param>
        /// <param name="line">约束直线</param>
        /// <param name="d">相切方向</param>
        /// <returns></returns>
        public Reference DrawSpline(Reference[] points, Reference line, int d)
        {
            int n = points.Length;
            HybridShapeSpline shape = Shapes.AddNewSpline();
            shape.SetSplineType(0);
            shape.SetClosing(0);

            shape.AddPointWithConstraintFromCurve(points[0], line, 1.0f, d, 0);
            Shapes.GSMVisibility(points[0], 0);
            Shapes.GSMVisibility(line, 0);

            for (int i = 1; i < n; i++)
            {
                shape.AddPointWithConstraintExplicit(points[i], null, -1.0f, 1, null, 0);
                Shapes.GSMVisibility(points[i], 0);
            }
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();

            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 绘制样条线：与两条直线相切
        /// </summary>
        /// <param name="points">参考点</param>
        /// <param name="line1">直线1</param>
        /// <param name="d1">相切方向1</param>
        /// <param name="line2">直线2</param>
        /// <param name="d2">相切方向2</param>
        /// <returns></returns>
        public Reference DrawSpline(Reference[] points, Reference line1, int d1, Reference line2, int d2)
        {
            int n = points.Length;
            HybridShapeSpline shape = Shapes.AddNewSpline();
            shape.SetSplineType(0);
            shape.SetClosing(0);
            shape.AddPointWithConstraintFromCurve(points[0], line1, 1.0f, d1, 0);
            Shapes.GSMVisibility(points[0], 0);
            Shapes.GSMVisibility(line1, 0);
            if (n > 2)
            {
                for (int i = 1; i < n - 1; i++)
                {
                    shape.AddPointWithConstraintExplicit(points[i], null, -1.0f, 1, null, 0);
                    Shapes.GSMVisibility(points[i], 0);
                }
            }
            shape.AddPointWithConstraintFromCurve(points[n - 1], line2, 1.0f, d2, 0);
            Shapes.GSMVisibility(points[n - 1], 0);
            Shapes.GSMVisibility(line2, 0);

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();

            return Part.CreateReferenceFromObject(shape);
        }

        #endregion

        #region Circle Drawing Methods
        /// <summary>
        /// 绘制圆：圆心半径
        /// </summary>
        /// <param name="plane">参考平面</param>
        /// <param name="c">圆心</param>
        /// <param name="r">半径</param>
        /// <returns></returns>
        public Reference DrawCircle(Reference plane, Reference c, double r)
        {
            HybridShapeCircleCtrRad shape = Shapes.AddNewCircleCtrRadWithAngles(c, plane, false, r, 0, 360);
            shape.SetLimitation(0);
            Body.AppendHybridShape(shape);
            Shapes.GSMVisibility(c, 0);
            return Part.CreateReferenceFromObject(shape);
        }

        /// <summary>
        /// 绘制圆弧：圆心半径和起止角度
        /// </summary>
        /// <param name="plane">参考平面</param>
        /// <param name="c">圆心</param>
        /// <param name="r">半径</param>
        /// <param name="a1">起始角度</param>
        /// <param name="a2">终止角度</param>
        /// <returns></returns>
        public Reference DrawCircle(Reference plane, Reference c, double r, double a1, double a2)
        {
            HybridShapeCircleCtrRad shape = Shapes.AddNewCircleCtrRadWithAngles(c, plane, false, r, a1, a2);
            shape.SetLimitation(0);
            Body.AppendHybridShape(shape);
            Shapes.GSMVisibility(plane, 0);
            Shapes.GSMVisibility(c, 0);
            return Part.CreateReferenceFromObject(shape);
        }
        #endregion

        #region Loft Drawing Methods
        /// <summary>
        /// 绘制多界面曲面：基于参考面
        /// </summary>
        /// <param name="s">参考面</param>
        /// <returns></returns>
        public Reference DrawLoft(Reference[] s)
        {
            int ns = s.Length;
            HybridShapeLoft shape = Shapes.AddNewLoft();
            shape.SectionCoupling = 1;
            shape.Relimitation = 1;
            shape.CanonicalDetection = 2;

            for (int i = 0; i < ns; i++)
            {
                shape.AddSectionToLoft(s[i], 1, null);
                Shapes.GSMVisibility(s[i], 0);
            }

            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);

        }

        /// <summary>
        /// 绘制多截面曲面：基于参考面和引导线
        /// </summary>
        /// <param name="s">参考面</param>
        /// <param name="g">引导线</param>
        /// <returns></returns>
        public Reference DrawLoft(Reference[] s, Reference[] g)
        {
            int ns = s.Length;
            int ng = g.Length;
            HybridShapeLoft shape = Shapes.AddNewLoft();
            shape.SectionCoupling = 1;
            shape.Relimitation = 1;
            shape.CanonicalDetection = 2;

            for (int i = 0; i < ns; i++)
            {
                shape.AddSectionToLoft(s[i], 1, null);
                Shapes.GSMVisibility(s[i], 0);
            }

            for (int i = 0; i < ng; i++)
            {
                shape.AddGuide(g[i]);
                Shapes.GSMVisibility(g[i], 0);
            }
            Body.AppendHybridShape(shape);
            Part.InWorkObject = shape;
            Part.Update();
            return Part.CreateReferenceFromObject(shape);
        }
        #endregion

        #endregion

        #endregion
    }
}

