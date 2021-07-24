/*──────────────────────────────────────────────────────────────
 * FileName     : LFChart.cs
 * Created      : 2021-06-22 22:33:32
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace LF.Figure
{
    /// <summary>
    /// 绘图区
    /// </summary>
    public class LFChart:UserControl
    {
        #region Fields
        Canvas root;
        #endregion

        #region Properties

        public LFLabel XLabel
        {
            get { return (LFLabel)GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XLabelProperty =
            DependencyProperty.Register("XLabel", typeof(LFLabel), typeof(LFChart), new PropertyMetadata(default(LFLabel), OnPropertyChanged));




        public LFLabel YLabel
        {
            get { return (LFLabel)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YLabelProperty =
            DependencyProperty.Register("YLabel", typeof(LFLabel), typeof(LFChart), new PropertyMetadata(default(LFLabel), OnPropertyChanged));






        #endregion

        #region Constructors
        public LFChart()
        {
            root = new Canvas();
            Content = root;

            BorderThickness = new Thickness(1);
            BorderBrush = Brushes.Blue;
            Padding = new Thickness(10);

            SetCurrentValue(XLabelProperty, new LFLabel());
            XLabel.Text = "X Title";
            XLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            SetCurrentValue(YLabelProperty, new LFLabel());
            YLabel.Text = "Y Title";
            YLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
        #endregion

        #region Methods
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as LFChart).Draw();

        private void Draw()
        {
            root.Children.Clear();
            root.Background = Brushes.AliceBlue;
            Rectangle rect = new Rectangle();
            rect.Width = 400;
            rect.Height = 300;
            rect.Fill = Brushes.White;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1;
            root.Children.Add(rect);

            //Canvas.SetLeft(rect,-rect.Width / 2);
            this.Width = rect.Width + Padding.Left + Padding.Right + rect.StrokeThickness * 2 + YLabel.Height + 10;
            this.Height = XLabel.RenderSize.Height + rect.Height + 10 + Padding.Top + Padding.Bottom + rect.StrokeThickness * 2;

            if (XLabel != null && YLabel !=null)
            {
                root.Children.Add(XLabel);
                Canvas.SetLeft(XLabel, rect.Width / 2);
                Canvas.SetTop(XLabel, rect.Height+10);

                TransformGroup tg = new TransformGroup();
                tg.Children.Add(new RotateTransform(-90));
                tg.Children.Add(new TranslateTransform(-YLabel.Height-10,(Height - YLabel.Width) / 2));
                YLabel.RenderTransform = tg;
                root.Children.Add(YLabel);
            }



            TextBlock txtw = new TextBlock()
            {
                Text = Width.ToString() + ","+Width.ToString(),
            };
            root.Children.Add(txtw);
        }
        #endregion
    }
}