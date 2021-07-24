/*──────────────────────────────────────────────────────────────
 * FileName     : LFLabel.cs
 * Created      : 2021-06-23 10:28:07
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Media;

namespace LF.Figure
{
    public class LFLabel:UserControl
    {
        #region Fields
        readonly Canvas root;

        #endregion

        #region Properties

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LFLabel), new PropertyMetadata(default(string),OnPropertyChanged));

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public LFLabel()
        {
            root = new Canvas();
            Content = root;

            HorizontalContentAlignment = HorizontalAlignment.Center;
            SetCurrentValue(TextProperty, "Figure Title");
        }


        #endregion

        #region Methods
        /// <summary>
        /// 绘制标签
        /// </summary>
        public void Draw()
        {
            root.Children.Clear();
            TextBlock txt = new TextBlock()
            {
                Text = Text,
                Foreground = Foreground,
                Background = Background,
                FontSize = FontSize,
                FontWeight = FontWeight,
                FontFamily = FontFamily,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
            };
            root.Children.Add(txt);
            
            RenderSize = LFText.MeasureString(txt);
            Width = RenderSize.Width;
            Height = RenderSize.Height;
            if (this.HorizontalContentAlignment == HorizontalAlignment.Right)
                Canvas.SetLeft(txt, -RenderSize.Width);
            else if (HorizontalContentAlignment == HorizontalAlignment.Center)
                Canvas.SetLeft(txt, -RenderSize.Width / 2);
        }
        #endregion

        #region Events
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as LFLabel).Draw();


        #endregion
    }
}