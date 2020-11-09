/*──────────────────────────────────────────────────────────────
 * FileName     : DroneItem
 * Created      : 2020-10-21 20:32:38
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace LF.Figure
{
    public class DroneItem:IItem
    {
        #region Fields
        private DataList _datas;
        private Overlay _overlay;    // 所属图层
        private bool _isVisible;    // 显示
        private List<PointF> _localPoints = new List<PointF>();

        private GraphicsPath _graphicsPath;
        private Pen _linePen = new Pen(Color.Red, 1);
        private Brush _fillBrush = new SolidBrush(Color.FromArgb(64, 255, 0, 0));

        private Size _size;
        private double _theta;

        #endregion

        #region Properties
        public DataList Datas { get => _datas; set => _datas = value; }
        public Overlay Overlay { get => _overlay; set => _overlay = value; }
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        public List<PointF> LocalPoints { get => _localPoints; set => _localPoints = value; }
        public double Theta { get => _theta; set => _theta = value; }
        public Size Size { get => _size; set => _size = value; }
        public Pen LinePen { get => _linePen; set => _linePen = value; }
        public Brush FillBrush { get => _fillBrush; set => _fillBrush = value; }

        #endregion

        #region Constructors
        public DroneItem()
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

            // 飞机图形
            Point[] points = new Point[4];
            points[0] = new Point(0, -14);
            points[1] = new Point(-10, 10);
            points[2] = new Point(0, 6);
            points[3] = new Point(10, 10);

            _graphicsPath.AddPolygon(points);

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
            PointF Offset = new Point(-Size.Width / 2, -Size.Height / 2);

            if (_isVisible)
            {
                if (_graphicsPath != null)
                {
                    Matrix temp = g.Transform;
                    g.TranslateTransform(LocalPoints[0].X - Offset.X, LocalPoints[0].Y - Offset.Y);
                    g.RotateTransform((float)_theta);

                    g.FillPath(_fillBrush, _graphicsPath);
                    g.DrawPath(_linePen, _graphicsPath);

                    g.Transform = temp;
                    
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
