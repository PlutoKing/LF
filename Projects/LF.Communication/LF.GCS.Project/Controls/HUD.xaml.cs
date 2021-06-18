/*──────────────────────────────────────────────────────────────
 * FileName     : HUD.cs
 * Created      : 2021-06-17 11:39:45
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
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

namespace LF.GCS.Project.Controls
{
    /// <summary>
    /// HUD.xaml 的交互逻辑
    /// </summary>
    public partial class HUD : UserControl
    {
        #region Properties


        /// <summary>
        /// 滚转角
        /// </summary>
        public double Roll
        {
            get { return (double)GetValue(RollProperty); }
            set { SetValue(RollProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Roll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RollProperty =
            DependencyProperty.Register("Roll", typeof(double), typeof(HUD), new PropertyMetadata(0.0d, OnPropertyChanged));


        public double Pitch
        {
            get { return (double)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pitch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PitchProperty =
            DependencyProperty.Register("Pitch", typeof(double), typeof(HUD), new PropertyMetadata(0.0d, OnPropertyChanged));



        public double Yaw
        {
            get { return (double)GetValue(YawProperty); }
            set { SetValue(YawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Yaw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YawProperty =
            DependencyProperty.Register("Yaw", typeof(double), typeof(HUD), new PropertyMetadata(0.0d, OnPropertyChanged));





        #endregion

        #region Constructors
        public HUD()
        {
            InitializeComponent();

            SetCurrentValue(RollProperty, 0.0d);
            SetCurrentValue(PitchProperty, 0.0d);
            SetCurrentValue(YawProperty, 0.0d);

            Loaded += HUD_Loaded;
            SizeChanged += HUD_SizeChanged;
        }




        #endregion

        #region Methods

        /// <summary>
        /// 绘制HUD
        /// </summary>
        public  void Draw()
        {
            root.Children.Clear();



            int fontOffset = (int)(RenderSize.Height / 20);     // 字体大小
            int borderOffset = 10;                              // 边界
            double pitchscale = root.RenderSize.Height / 100;   // 
            double pitchoffset = Pitch * root.RenderSize.Height / 100;  // 对应俯仰角在HUD上的偏移量

            double width = root.RenderSize.Width;
            double height = root.RenderSize.Height;
            double r = height / 2 - 2.2 * fontOffset - borderOffset; // roll 刻度板半径
            #region Background
            TransformGroup skyTrans = new TransformGroup();
            skyTrans.Children.Add(new TranslateTransform(-width / 2, -height / 2));
            skyTrans.Children.Add(new RotateTransform(-Roll, width / 2, height / 2));
            // 蓝天背景矩形
            Rectangle rectSky = new Rectangle()
            {
                Width = width * 2,
                Height = height + pitchoffset,
                Fill = Brushes.SkyBlue,
                RenderTransform = skyTrans,
            };
            root.Children.Add(rectSky);
            // 地面背景矩形
            TransformGroup groundTrans = new TransformGroup();
            groundTrans.Children.Add(new TranslateTransform(-width / 2, height / 2+pitchoffset));
            groundTrans.Children.Add(new RotateTransform(-Roll, width / 2, height / 2));
            Rectangle rectGround = new Rectangle()
            {
                Width = width * 2,
                Height = height - pitchoffset,
                Fill = Brushes.Sienna,
                RenderTransform = groundTrans,
            };
            root.Children.Add(rectGround);
            #endregion

            #region Pitch Angle
            for (int a = -90; a <= 90; a += 5)
            {
                if (a >= Pitch - 15 && a <= Pitch + 15)
                {
                    TransformGroup pitchTrans = new TransformGroup();
                    pitchTrans.Children.Add(new TranslateTransform(width / 2, height / 2));
                    pitchTrans.Children.Add(new RotateTransform(-Roll, width / 2, height / 2));

                    TransformGroup textTrans = new TransformGroup();
                    textTrans.Children.Add(new TranslateTransform(width / 2 - width / 10-fontOffset*2.5, height / 2  - a * pitchscale + pitchoffset-fontOffset/2));
                    textTrans.Children.Add(new RotateTransform(-Roll, width / 2, height / 2));

                    if (a % 10 == 0)
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = -width/10,
                            Y1 = -a * pitchscale + pitchoffset,
                            X2 = width / 10,
                            Y2 = -a * pitchscale + pitchoffset,
                            Stroke = Brushes.White,
                            StrokeThickness=2,
                            RenderTransform = pitchTrans,
                        });

                        // 俯仰角刻度
                        TextBlock textPitch = new TextBlock()
                        {
                            Width = fontOffset * 2,
                            Height = fontOffset,
                            FontSize = fontOffset*0.9,
                            Foreground = Brushes.White,
                            Text = Math.Abs(a).ToString(),
                            TextAlignment = TextAlignment.Right,
                            RenderTransform = textTrans,
                            
                        };
                        root.Children.Add(textPitch);
                    }
                    else
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = -width / 15,
                            Y1 = -a * pitchscale + pitchoffset,
                            X2 = width / 15,
                            Y2 = -a * pitchscale + pitchoffset,
                            Stroke = Brushes.White,
                            StrokeThickness = 1,
                            RenderTransform = pitchTrans,
                        });
                    }
                }
            }
            #endregion

            #region Roll Angle
            int angle = 80;
            for (int a = -180; a <= 180; a += 5)
            {
                if (a >= Roll - angle && a <= Roll + angle)
                {
                    TransformGroup pitchTrans = new TransformGroup();
                    pitchTrans.Children.Add(new TranslateTransform(width / 2, height / 2 +r));
                    pitchTrans.Children.Add(new RotateTransform(a - Roll, width / 2, height / 2));

                    TransformGroup textTrans = new TransformGroup();
                    textTrans.Children.Add(new TranslateTransform(width / 2 - fontOffset, height / 2+fontOffset*1.2 + r));
                    textTrans.Children.Add(new RotateTransform(a - Roll, width / 2, height / 2));


                    if (a % 30 == 0)
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = 0,
                            Y1 = 0,
                            X2 = 0,
                            Y2 = fontOffset,
                            Stroke = Brushes.White,
                            StrokeThickness = 2,
                            RenderTransform = pitchTrans,

                        });

                        TextBlock text = new TextBlock()
                        {
                            Width = fontOffset * 2,
                            Height = fontOffset,
                            FontSize = fontOffset * 0.9,
                            Foreground = Brushes.White,
                            Text = Math.Abs(a).ToString(),
                            TextAlignment = TextAlignment.Center,
                            RenderTransform = textTrans,
                        };
                        root.Children.Add(text);

                    }
                    else
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = 0,
                            Y1 = 0,
                            X2 = 0,
                            Y2 = fontOffset/2,
                            Stroke = Brushes.White,
                            StrokeThickness = 1,
                            RenderTransform = pitchTrans,

                        });
                    }
                }
            }
            #endregion

            #region Yaw Angle
            for (int a = -180; a <= 180; a += 5)
            {
                if (a >= Yaw - angle && a <= Yaw + angle)
                {
                    TransformGroup yawTrans = new TransformGroup();
                    yawTrans.Children.Add(new TranslateTransform(width / 2, height / 2 - r));
                    yawTrans.Children.Add(new RotateTransform(a - Yaw, width / 2, height / 2));

                    TransformGroup textTrans = new TransformGroup();
                    textTrans.Children.Add(new TranslateTransform(width / 2 - fontOffset, height / 2 - fontOffset * 1.2 - r));
                    textTrans.Children.Add(new RotateTransform(a - Yaw, width / 2, height / 2));


                    if (a % 30 == 0)
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = 0,
                            Y1 = 0,
                            X2 = 0,
                            Y2 = fontOffset,
                            Stroke = Brushes.White,
                            StrokeThickness = 2,
                            RenderTransform = yawTrans,

                        });

                        TextBlock text = new TextBlock()
                        {
                            Width = fontOffset * 2,
                            Height = fontOffset,
                            FontSize = fontOffset * 0.9,
                            Foreground = Brushes.White,
                            Text = Math.Abs(a).ToString(),
                            TextAlignment = TextAlignment.Center,
                            RenderTransform = textTrans,
                        };
                        root.Children.Add(text);

                    }
                    else
                    {
                        root.Children.Add(new Line()
                        {
                            X1 = 0,
                            Y1 = 0,
                            X2 = 0,
                            Y2 = fontOffset / 2,
                            Stroke = Brushes.White,
                            StrokeThickness = 1,
                            RenderTransform = yawTrans,

                        });
                    }
                }
            }
            #endregion

            #region Center
            TransformGroup centerTrans = new TransformGroup();
            centerTrans.Children.Add(new TranslateTransform(width / 2 - 5, height / 2-5));
            Border centerRect = new Border()
            {
                Width = 10,
                Height = 10,
                CornerRadius = new CornerRadius(5),
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.Red,
                RenderTransform = centerTrans,
            };
            root.Children.Add(centerRect);
            root.Children.Add(new Line()
            {
                X1 = -5,
                Y1 = 5,
                X2 = 0,
                Y2 = 5,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                RenderTransform = centerTrans,
            });
            root.Children.Add(new Line()
            {
                X1 = 10,
                Y1 = 5,
                X2 = 15,
                Y2 = 5,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                RenderTransform = centerTrans,
            });
            root.Children.Add(new Line()
            {
                X1 = 5,
                Y1 = -5,
                X2 = 5,
                Y2 = 0,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                RenderTransform = centerTrans,
            });
            #endregion

            #region Reading box
            TransformGroup yawRectTrans = new TransformGroup();
            yawRectTrans.Children.Add(new TranslateTransform(width / 2 - fontOffset*2, height / 2 -r-fontOffset*1.2));

            Border yawRect = new Border()
            {
                Width = fontOffset * 4,
                Height = fontOffset*1.2,
                
                BorderThickness = new Thickness(0),
                BorderBrush = Brushes.Red,
                RenderTransform = yawRectTrans,
                //Background = Brushes.LightGray,
                Child = new TextBlock()
                {
                    Text = Yaw.ToString("0.00"),
                    FontSize = fontOffset*0.9,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Black,
                },
            };
            root.Children.Add(yawRect);
            #endregion
        }

        #endregion

        #region Events

        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
=> (d as HUD)?.Draw();

        private void HUD_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }
        private void HUD_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }
        #endregion
    }
}
