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


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LFFigureView), new PropertyMetadata(default(string),OnPropertyChanged));



        #endregion

        #region Constructors
        public LFFigureView()
        {
            InitializeComponent();

            SetCurrentValue(TitleProperty, "Title");

            SizeChanged += LFFigureView_SizeChanged;
        }

        
        #endregion

        #region Methods
        public void Draw()
        {
            root.Children.Clear();

            double w = root.RenderSize.Width;
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.Text = Title;
            root.Children.Add(text);

            Size tmp = LFText.MeasureString(text);
            Canvas.SetLeft(text, (w - tmp.Width) / 2.0);
        }

        
        #endregion

        #region Events
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as LFFigureView).Draw();

        private void LFFigureView_SizeChanged(object sender, SizeChangedEventArgs e) => Draw();
        #endregion
    }
}
