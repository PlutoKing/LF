/*──────────────────────────────────────────────────────────────
 * FileName     : ImageItem
 * Created      : 2020-06-30 11:09:25
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
using LF.Mathematics;

namespace LF.Figure
{
    public class ImageItem : IItem
    {
        #region Fields
        private DataList _datas;

        private Pen _linePen = new Pen(Color.Blue, 1);
        private Image _image;
        private Symbol _symbol;

        private Label _label;
        private bool _isX2Axis;
        private bool _isY2Axis;
        private int _yAxisIndex;

        private bool _isSelected;
        private bool _isSelectable;

        private Overlay _overlay;    // 所属图层
        private bool _isVisible;    // 显示
        private bool _isMouseOver;  // 鼠标悬停
        private bool _isLineVisible;
        private GraphicsPath _graphicsPath;
        private List<PointF> _localPoints = new List<PointF>();

        #endregion

        #region Properties
        public Pen Line { get => _linePen; set => _linePen = value; }
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public Symbol Symbol { get => _symbol; set => _symbol = value; }
        public Label Label { get => _label; set => _label = value; }
        public bool IsX2Axis { get => _isX2Axis; set => _isX2Axis = value; }
        public bool IsY2Axis { get => _isY2Axis; set => _isY2Axis = value; }
        public int YAxisIndex { get => _yAxisIndex; set => _yAxisIndex = value; }
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }
        public bool IsSelectable { get => _isSelectable; set => _isSelectable = value; }
        public Overlay Overlay { get => _overlay; set => _overlay = value; }
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        public bool IsMouseOver { get => _isMouseOver; set => _isMouseOver = value; }
        public GraphicsPath GraphicsPath { get => _graphicsPath; set => _graphicsPath = value; }
        public DataList Datas { get => _datas; set => _datas = value; }
        public List<PointF> LocalPoints { get => _localPoints; set => _localPoints = value; }
        public bool IsLineVisible { get => _isLineVisible; set => _isLineVisible = value; }

        #endregion

        #region Constructors
        public ImageItem()
        {
            _isVisible = true;
            _isLineVisible = true;
            _datas = new DataList();
            _symbol = new Symbol(SymbolType.None, this._linePen.Color);
        }

        public ImageItem(LFPoint2D point,Image image, ChartFigure graph)
            : this()
        {
            Data d = new Data(graph);
            d.X = point.X;
            d.Y = point.Y;
            _datas.Add(d);

            Data d2 = new Data(graph);
            d2.X = point.X+image.Width;
            d2.Y = point.Y+image.Height;
            _datas.Add(d2);
            _image = image;
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

            for (int i = 0; i < LocalPoints.Count; i++)
            {
                PointF point = LocalPoints[i];
                if (Math.Abs(point.X) > 99000 || Math.Abs(point.Y) > 99000)
                    _linePen.DashStyle = DashStyle.Solid;
                if (i == 0)
                {
                    _graphicsPath.AddLine(point.X, point.Y, point.X, point.Y);
                    
                }
                else
                {
                    System.Drawing.PointF p = _graphicsPath.GetLastPoint();
                    //_graphicsPath.AddLine(p.X, p.Y, point.X, point.Y);
                    _graphicsPath.AddRectangle(new RectangleF(p.X, p.Y, point.X- p.X, point.Y- p.Y));
                }
            }
        }

        public void Draw(Graphics g)
        {
            SmoothingMode mode = g.SmoothingMode;
            PixelOffsetMode mode2 = g.PixelOffsetMode;

            #region 图形质量
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;  // 临近插值
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.CompositingMode = CompositingMode.SourceOver;
            //g.CompositingQuality = CompositingQuality.HighSpeed;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            #endregion

            if (_isVisible)
            {
                if (_graphicsPath != null)
                {
                    g.DrawImage(_image, new RectangleF(LocalPoints[0].X, LocalPoints[0].Y, LocalPoints[1].X - LocalPoints[0].X, LocalPoints[1].Y - LocalPoints[0].Y));
                    //g.DrawPath(_linePen, _graphicsPath);
                }
                try
                {
                    foreach (PointF P in _graphicsPath.PathPoints)
                    {
                        _symbol.Border.Line.Color = this.Line.Color;
                        _symbol.DrawSymbol(g, _overlay._graph, (int)P.X, (int)P.Y, 1.0f);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }



            g.SmoothingMode = mode;
            g.PixelOffsetMode = mode2;
        }



        /// <summary>
        /// 判断点(x,y)是否在线上
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal bool IsInside(int x, int y)
        {
            if (_graphicsPath != null)
            {
                return _graphicsPath.IsOutlineVisible(x, y, _linePen);
            }
            return false;
        }


        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
