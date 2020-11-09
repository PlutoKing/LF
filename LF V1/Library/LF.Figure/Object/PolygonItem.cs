/*──────────────────────────────────────────────────────────────
 * FileName     : PolygonItem
 * Created      : 2020-01-05 14:12:26
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    /// <summary>
    /// 多边形物体
    /// </summary>
    public class PolygonItem:IItem
    {
        #region Fields
        private DataList _datas;
        private Pen _linePen = new Pen(Color.Black, 1);
        private Brush _fillBrush = new SolidBrush(Color.FromArgb(64,64,64,64));

        private Overlay _overlay;    // 所属图层
        private bool _isVisible;    // 显示

        private GraphicsPath _graphicsPath;
        private List<PointF> _localPoints = new List<PointF>();

        #endregion

        #region Properties
        public DataList Datas { get => _datas; set => _datas = value; }
        public Overlay Overlay { get => _overlay; set => _overlay = value; }
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        public List<PointF> LocalPoints { get => _localPoints; set => _localPoints = value; }

        #endregion

        #region Constructors
        public PolygonItem()
        {
            _isVisible = true;
            _datas = new DataList();
        }
        #endregion

        #region Methods
        public void UpdateGraphicsPath()
        {
            if (_graphicsPath == null)
            {
                _graphicsPath = new GraphicsPath();
            }
            else
            {
                _graphicsPath.Reset();
            }

            Point[] pnts = new Point[LocalPoints.Count];
            for (int i = 0; i < LocalPoints.Count; i++)
            {
                Point p2 = new Point((int)LocalPoints[i].X, (int)LocalPoints[i].Y);
                pnts[pnts.Length - 1 - i] = p2;
            }

            if (pnts.Length > 2)
            {
                _graphicsPath.AddPolygon(pnts);
            }
            else if (pnts.Length == 2)
            {
                _graphicsPath.AddLines(pnts);
            }
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            SmoothingMode mode = g.SmoothingMode;

            #region 图形质量
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;  // 临近插值
            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.CompositingMode = CompositingMode.SourceOver;
            //g.CompositingQuality = CompositingQuality.HighSpeed;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            #endregion

            if (_isVisible)
            {
                if (_graphicsPath != null)
                {
                    g.FillPath(_fillBrush, _graphicsPath);
                    g.DrawPath(_linePen, _graphicsPath);
                }
                //_symbol.Draw(g, _overlay._graph,this, 1.0f, false);
            }

            g.SmoothingMode = mode;
        }
        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
