/*──────────────────────────────────────────────────────────────
 * FileName     : LFFigureView.cs
 * Created      : 2021-06-22 21:26:26
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LF.Figure
{
    /// <summary>
    /// LFFigureView.xaml 的交互逻辑
    /// </summary>
    public partial class LFFigureView : UserControl
    {
        #region Fields
        public LFLabel Title
        {
            get { return (LFLabel)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lFLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(LFLabel), typeof(LFFigureView), new PropertyMetadata(default(LFLabel),OnPropertyChanged));


        public LFChart Chart
        {
            get { return (LFChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Chart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(LFChart), typeof(LFFigureView), new PropertyMetadata(default(LFChart), OnPropertyChanged));


        #endregion

        #region Constructors
        public LFFigureView()
        {
            InitializeComponent();


            SetCurrentValue(TitleProperty, new LFLabel());

            SetCurrentValue(ChartProperty, new LFChart());


            SizeChanged += LFFigureView_SizeChanged;
        }




        #endregion

        #region Methods

        /// <summary>
        /// 绘制图表
        /// </summary>
        public void Draw()
        {
            root.Children.Clear();

            double w = root.RenderSize.Width;
            double h = root.RenderSize.Height;

            root.Children.Add(Title);
            Canvas.SetLeft(Title, w / 2);
            Canvas.SetTop(Title, 0);

            if(Chart != null)
            {
                root.Children.Add(Chart);
                Canvas.SetLeft(Chart, (w-Chart.Width)/2);
                Canvas.SetTop(Chart, (h - Chart.Height) / 2);
            }

        }

        #endregion

        #region Events
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as LFFigureView).Draw();

        private void LFFigureView_SizeChanged(object sender, SizeChangedEventArgs e) => Draw();
        #endregion
    }
}
