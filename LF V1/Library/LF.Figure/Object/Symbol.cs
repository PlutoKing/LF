/*──────────────────────────────────────────────────────────────
 * FileName     : Symbol
 * Created      : 2020-01-05 13:04:27
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
    /// 标记
    /// </summary>
    public class Symbol:ICloneable
    {
        #region Fields
        private float _size;
        private SymbolType _type;
        private bool _isAntiAlias;
        private bool _isVisible;
        private Brush _fillBrush;
        private Border _border;
        private GraphicsPath _userSymbol;
        #endregion

        #region Properties
        /// <summary>
        /// 尺寸
        /// </summary>
        public float Size { get => _size; set => _size = value; }

        /// <summary>
        /// 标记类型
        /// </summary>
        public SymbolType Type { get => _type; set => _type = value; }

        /// <summary>
        /// 抗锯齿
        /// </summary>
        public bool IsAntiAlias { get => _isAntiAlias; set => _isAntiAlias = value; }
        
        /// <summary>
        /// 显示
        /// </summary>
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        
        /// <summary>
        /// 填充颜色
        /// </summary>
        public Brush Fill { get => _fillBrush; set => _fillBrush = value; }
        
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Border Border { get => _border; set => _border = value; }
        
        /// <summary>
        /// 自定义标记
        /// </summary>
        public GraphicsPath UserSymbol { get => _userSymbol; set => _userSymbol = value; }

        #endregion

        #region Constructors
        public Symbol()
            : this(SymbolType.Default, Color.Empty)
        {
        }

        public Symbol(SymbolType type, Color color)
        {
            _size = Default.Size;
            _type = type;
            _isAntiAlias = Default.IsAntiAlias;
            _isVisible = Default.IsVisible;
            _border = new Border(Default.IsBorderVisible, color, Default.PenWidth);
            _fillBrush = null;
            _userSymbol = null;
        }

        public Symbol(Symbol rhs)
        {
            _size = rhs._size;
            _type = rhs._type;
            _isAntiAlias = rhs._isAntiAlias;
            _isVisible = rhs._isVisible;
            _fillBrush = (SolidBrush)rhs._fillBrush.Clone();
            _border = rhs.Border.Clone();

            if (rhs.UserSymbol != null)
                _userSymbol = rhs.UserSymbol.Clone() as GraphicsPath;
            else
                _userSymbol = null;
        }

        public Symbol Clone()
        {
            return new Symbol(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        public void Draw(Graphics g,ChartFigure graph,LineItem item, float scaleFactor,bool isSelected)
        {

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleFactor"></param>
        /// <returns></returns>
        public GraphicsPath MakePath(Graphics g,float scaleFactor)
        {
            float scaledSize = _size * scaleFactor;
            float hsize = scaledSize / 2;
            float hsize1 = hsize + 1;

            GraphicsPath path = new GraphicsPath();

            switch (_type == SymbolType.Default || (_type == SymbolType.UserDefined && _userSymbol == null) ? Default.Type : _type)
            {
                case SymbolType.Square:
                    path.AddLine(-hsize, -hsize, hsize, -hsize);
                    path.AddLine(hsize, -hsize, hsize, hsize);
                    path.AddLine(hsize, hsize, -hsize, hsize);
                    path.AddLine(-hsize, hsize, -hsize, -hsize);
                    break;
                case SymbolType.Diamond:
                    path.AddLine(0, -hsize, hsize, 0);
                    path.AddLine(hsize, 0, 0, hsize);
                    path.AddLine(0, hsize, -hsize, 0);
                    path.AddLine(-hsize, 0, 0, -hsize);
                    break;
                case SymbolType.Triangle:
                    path.AddLine(0, -hsize, hsize, hsize);
                    path.AddLine(hsize, hsize, -hsize, hsize);
                    path.AddLine(-hsize, hsize, 0, -hsize);
                    break;
                case SymbolType.Circle:
                    path.AddEllipse(-hsize, -hsize, scaledSize, scaledSize);
                    break;
                case SymbolType.XCross:
                    path.AddLine(-hsize, -hsize, hsize1, hsize1);
                    path.StartFigure();
                    path.AddLine(hsize, -hsize, -hsize1, hsize1);
                    break;
                case SymbolType.Plus:
                    path.AddLine(0, -hsize, 0, hsize1);
                    path.StartFigure();
                    path.AddLine(-hsize, 0, hsize1, 0);
                    break;
                case SymbolType.Star:
                    path.AddLine(0, -hsize, 0, hsize1);
                    path.StartFigure();
                    path.AddLine(-hsize, 0, hsize1, 0);
                    path.StartFigure();
                    path.AddLine(-hsize, -hsize, hsize1, hsize1);
                    path.StartFigure();
                    path.AddLine(hsize, -hsize, -hsize1, hsize1);
                    break;
                case SymbolType.TriangleDown:
                    path.AddLine(0, hsize, hsize, -hsize);
                    path.AddLine(hsize, -hsize, -hsize, -hsize);
                    path.AddLine(-hsize, -hsize, 0, hsize);
                    break;
                case SymbolType.HDash:
                    path.AddLine(-hsize, 0, hsize1, 0);
                    break;
                case SymbolType.VDash:
                    path.AddLine(0, -hsize, 0, hsize1);
                    break;
                case SymbolType.UserDefined:
                    path = _userSymbol.Clone() as GraphicsPath;
                    Matrix scaleTransform = new Matrix(scaledSize, 0.0f, 0.0f, scaledSize, 0.0f, 0.0f);
                    path.Transform(scaleTransform);
                    break;
            }
            return path;

        }

        public void DrawSymbol(Graphics g, ChartFigure graph, int x, int y,float scaleFactor)
        {
            // Only draw if the symbol is visible
            if (_isVisible &&
                    this.Type != SymbolType.None &&
                    x < 100000 && x > -100000 &&
                    y < 100000 && y > -100000)
            {
                SmoothingMode sModeSave = g.SmoothingMode;
                if (_isAntiAlias)
                    g.SmoothingMode = SmoothingMode.HighQuality;

                
                using (GraphicsPath path = this.MakePath(g, scaleFactor))
                {
                    DrawSymbol(g, x, y, path, _border.Line, _fillBrush);
                }
                
                g.SmoothingMode = sModeSave;
            }
        }


        private void DrawSymbol(Graphics g, int x, int y, GraphicsPath path,
                    Pen pen, Brush brush)
        {
            // Only draw if the symbol is visible
            if (_isVisible &&
                    this.Type != SymbolType.None &&
                    x < 100000 && x > -100000 &&
                    y < 100000 && y > -100000)
            {
                Matrix saveMatrix = g.Transform;
                g.TranslateTransform(x, y);

                // Fill or draw the symbol as required
                if (_fillBrush != null)
                    g.FillPath(brush, path);
                //FillPoint( g, x, y, scaleFactor, pen, brush );

                if (_border.IsVisible)
                    g.DrawPath(pen, path);
                //DrawPoint( g, x, y, scaleFactor, pen );

                

                g.Transform = saveMatrix;
            }
        }



        #endregion

        #region Serializations
        #endregion

        #region Defaults
        public struct Default
        {
            public static float Size = 10;
            public static float PenWidth = 1.5F;
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
            public static SymbolType Type = SymbolType.None;
            public static bool IsAntiAlias = false;
            public static bool IsVisible = true;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.Red;
        }
        #endregion
    }

    public enum SymbolType
    {
        /// <summary>
        /// 方形标签
        /// </summary>
        Square,
        /// <summary>
        /// 菱形标签
        /// </summary>
        Diamond,
        /// <summary>
        /// 三角形标签
        /// </summary>
        Triangle,
        /// <summary> Uniform circle <see cref="LF.Figure.Symbol"/> </summary>
        Circle,
        /// <summary> "X" shaped <see cref="LF.Figure.Symbol"/>.  This symbol cannot
        /// be filled since it has no outline. </summary>
        XCross,
        /// <summary> "+" shaped <see cref="LF.Figure.Symbol"/>.  This symbol cannot
        /// be filled since it has no outline. </summary>
        Plus,
        /// <summary> Asterisk-shaped <see cref="LF.Figure.Symbol"/>.  This symbol
        /// cannot be filled since it has no outline. </summary>
        Star,
        /// <summary> Unilateral triangle <see cref="LF.Figure.Symbol"/>, pointing
        /// down. </summary>
        TriangleDown,
        /// <summary>
        /// Horizontal dash <see cref="LF.Figure.Symbol"/>.  This symbol cannot be
        /// filled since it has no outline.
        /// </summary>
        HDash,
        /// <summary>
        /// Vertical dash <see cref="LF.Figure.Symbol"/>.  This symbol cannot be
        /// filled since it has no outline.
        /// </summary>
        VDash,
        /// <summary> A symbol defined by the <see cref="Symbol.UserSymbol"/> propery.
        /// If no symbol is defined, the <see cref="Symbol.Default.Type"/>. symbol will
        /// be used.
        /// </summary>
        UserDefined,
        /// <summary> A Default symbol type (the symbol type will be obtained
        /// from <see cref="Symbol.Default.Type"/>. </summary>
        Default,
        /// <summary> No symbol is shown (this is equivalent to using
        /// <see cref="Symbol.IsVisible"/> = false.</summary>
        None
    }
}
