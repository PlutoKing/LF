/*──────────────────────────────────────────────────────────────
 * FileName     : FigureControl
 * Created      : 2020-10-16 11:57:33
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LF.Figure
{
    public partial class FigureControl : UserControl
    {
        #region Fields
        private ChartFigure _figure;

        private bool _isMouseDown = false;
        Point _mouseDown;
        Point _mouseUp;
        private double _zoomStepFraction = 0.1;
        #endregion

        #region Properties
        public ChartFigure Figure { get => _figure; set => _figure = value; }

        #endregion

        #region Constructors
        public FigureControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            _figure = new ChartFigure(new RectangleF(0, 0, Width, Height), "", "x", "y");   // 图框架

            this.MouseDown += GraphicsControl_MouseDown;
            this.MouseUp += GraphicsControl_MouseUp;
            this.MouseMove += GraphicsControl_MouseMove;
            this.MouseWheel += GraphicsControl_MouseWheel;
            this.KeyDown += GraphicsControl_KeyDown;
            this.KeyUp += GraphicsControl_KeyUp;
            this.Resize += GraphicsControl_Resize;
        }
        #endregion

        #region Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                _figure.Draw(e.Graphics);
            }
            catch { }
        }

        #endregion

        #region Events

        /// <summary>
        /// 尺寸发生变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_Resize(object sender, EventArgs e)
        {
            _figure.ReSize(new RectangleF(0, 0, Width, Height));
            this.Refresh();
        }

        /// <summary>
        /// 鼠标按下时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_MouseDown(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);
            _isMouseDown = true;
            _mouseDown = mousePt;
            _dragStartPt = _mouseDown;
        }

        /// <summary>
        /// 鼠标抬起时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_MouseUp(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);

            _isMouseDown = false;
            _mouseUp = mousePt;

        }

        Point _dragStartPt;


        /// <summary>
        /// 鼠标移动时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);

            if (_isMouseDown && e.Button == MouseButtons.Middle)
            {

                _figure.Pan(mousePt, _dragStartPt);
                _dragStartPt = mousePt;
                this.Refresh();
            }


        }

        /// <summary>
        /// 鼠标滚轮滚动时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_MouseWheel(object sender, MouseEventArgs e)
        {
            PointF centerPoint = new PointF(e.X, e.Y);
            double zoomFraction = (1 + (e.Delta < 0 ? 1.0 : -1.0) * _zoomStepFraction);

            // 缩放
            _figure.Zoom(zoomFraction, centerPoint, true, true);

            this.Refresh();
        }

        /// <summary>
        /// 键盘按键按下时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 键盘按键抬起时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsControl_KeyUp(object sender, KeyEventArgs e)
        {

        }


        #endregion

    }
}
