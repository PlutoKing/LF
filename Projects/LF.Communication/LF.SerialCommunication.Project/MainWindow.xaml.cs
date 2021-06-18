/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindow.cs
 * Created      : 2021-06-11 11:07:07
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
using System.Windows.Shapes;
using System.IO;
using System.Timers;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Media.Animation;

namespace LF.SerialCommunication.Project
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// 串口
        /// </summary>
        private LFSerialPort SerialPort = new LFSerialPort();

        /// <summary>
        /// 常用波特率
        /// </summary>
        private List<int> baudrates = new List<int>() { 4800, 9600, 19200, 38400, 57600, 115200 };

        /// <summary>
        /// 接收串口段落
        /// </summary>
        Paragraph paragraph = new Paragraph();


        /// <summary>
        /// 定时器
        /// </summary>
        private Timer Timer = new Timer();

        private DateTime startTime;
        private Timer PortTimer = new Timer();
        /// <summary>
        /// 字符串构造器
        /// </summary>
        private StringBuilder builder = new StringBuilder();

        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CmbName.ItemsSource = SerialPort.PortNames;
            CmbBaudrate.ItemsSource = baudrates;
            CmbBaudrate.SelectedIndex = 5;

            paragraph = (Paragraph)RtbRece.Document.Blocks.FirstBlock;
            Timer.Elapsed += Timer_Elapsed;
            PortTimer.Interval = 50;
            PortTimer.Elapsed += PortTimer_Elapsed;

            SerialPort.OnReceiveData += SerialPort_OnReceiveData;
            this.DataContext = SerialPort;
            this.Loaded += MainWindow_Loaded;
        }

        private void PortTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SerialPort.Time = (DateTime.Now - startTime).TotalSeconds;
        }


        #endregion

        #region Methods
        /// <summary>
        /// 设置串口打开后控件样式
        /// </summary>
        private void SetPortOpen()
        {
            CmbName.IsEnabled = false;
            CmbBaudrate.IsEnabled = false;
            CmbDatabits.IsEnabled = false;
            CmbParity.IsEnabled = false;
            CmbStopbits.IsEnabled = false;
            SerialPort.Time = 0.0d;
            startTime = DateTime.Now;
            PortTimer.Start();
            BtnOpenClose.Content = "关闭串口";
        }

        /// <summary>
        /// 设置串口关闭后控件样式
        /// </summary>
        private void SetPortClose()
        {
            CmbName.IsEnabled = true;
            CmbBaudrate.IsEnabled = true;
            CmbDatabits.IsEnabled = true;
            CmbParity.IsEnabled = true;
            CmbStopbits.IsEnabled = true;
            PortTimer.Stop();
            BtnOpenClose.Content = "打开串口";
        }

        /// <summary>
        /// 添加消息到接收框
        /// </summary>
        private void AddMessage(string msg)
        {
            AddMessage(msg, Brushes.Black);
        }

        /// <summary>
        /// 添加消息到接收框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        private void AddMessage(string msg,Brush color)
        {
            if (SerialPort.IsNewLine)
            {
                if (paragraph.Inlines.Count != 0)
                {
                    Run tmp = new Run("\n");
                    paragraph.Inlines.Add(tmp);
                }
            }

            // 如果显示时间
            if (SerialPort.IsShowTime)
            {
                string time = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] ";
                Run runTime = new Run(time);
                runTime.Foreground = Brushes.Blue;
                paragraph.Inlines.Add(runTime);
            }

            // 消息内容
            Run run = new Run(msg);
            run.Foreground = color;
            paragraph.Inlines.Add(run);

            RtbRece.ScrollToEnd();
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        private void LoadMessage()
        {
            // 清除
            paragraph.Inlines.Clear();


            foreach(LFMessage msg in SerialPort.Messages)
            {
                // 内容转换
                msg.ReadBuffer(SerialPort.IsShowHex);

                if (SerialPort.IsNewLine)
                {
                    if (paragraph.Inlines.Count != 0)
                    {
                        Run tmp = new Run("\n");
                        paragraph.Inlines.Add(tmp);
                    }
                }

                // 如果显示时间
                if (SerialPort.IsShowTime)
                {
                    string time = "[" + msg.Time.ToString() + "] ";
                    Run runTime = new Run(time);
                    runTime.Foreground = Brushes.Blue;
                    paragraph.Inlines.Add(runTime);

                }

                // 消息内容
                Run run = new Run(msg.Content);
                paragraph.Inlines.Add(run);

                RtbRece.ScrollToEnd();
            }

        }

        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str, bool isFilterChinese)
        {
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

            //清除所有空格
            hex = hex.Replace(" ", "");
            //若字符个数为奇数，补一个0
            hex += hex.Length % 2 != 0 ? "0" : "";

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0, c = result.Length; i < c; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }

        /// <summary>
        /// 转化中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ConvertChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c <= 0 || c >= 127)
                {
                    s.Append(c.ToString("X4"));
                }
                else
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }
        private static string FilterChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c > 0 && c < 127)
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str)
        {
            return StringToHexByte(str, false);
        }

        /// <summary>
        /// 发送函数
        /// </summary>
        private void SendData()
        {
            Dispatcher.Invoke(() =>
            {
                string send = TxbSend.Text;
                // 如果16进制发送，则需要将内容当做16进制处理
                if (SerialPort.IsSendHex)
                {
                    try
                    {
                        byte[] data = StringToHexByte(send);
                        SerialPort.SendData(data);
                    }
                    catch
                    {
                        MessageBox.Show("请正确输入16进制格式！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        CkbAutoSend.IsChecked = false;
                    }

                }
                else
                {
                    SerialPort.SendData(send);
                }


                if (SerialPort.IsShowSend)
                {
                    AddMessage(send, Brushes.Gray);
                }
            });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="richTextBox"></param>
        private static void SaveFile(string filename, RichTextBox richTextBox)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException();
            }
            using (FileStream stream = File.OpenWrite(filename))
            {
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                string ext = System.IO.Path.GetExtension(filename);
                if (String.Compare(ext, ".xaml", true) == 0)
                {
                    dataFormat = DataFormats.Xaml;
                }
                else if (String.Compare(ext, ".rtf", true) == 0)
                {
                    dataFormat = DataFormats.Rtf;
                }
                documentTextRange.Save(stream, dataFormat);
            }
        }

        #region Animation

        ///// <param name="emp"></param>
        //private void PlaySendFlashing()
        //{
        //    var ani = new ColorAnimation();
        //    ani.Duration = new TimeSpan(1000);
        //    ani.From = (Color)ColorConverter.ConvertFromString("#4EEE94");
        //    ani.To = (Color)ColorConverter.ConvertFromString("#EE3B3B");
        //    LedSend.Background.BeginAnimation(SolidColorBrush.ColorProperty, ani);
        //}

        //private void PlayReciveFlashing()
        //{
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        var ani = new ColorAnimation();
        //        ani.Duration = new TimeSpan(1000);
        //        ani.From = (Color)ColorConverter.ConvertFromString("#4EEE94");
        //        ani.To = (Color)ColorConverter.ConvertFromString("#EE3B3B");
        //        LedRece.Fill.BeginAnimation(SolidColorBrush.ColorProperty, ani);
        //    });

        //}

        #endregion
        #endregion

        #region Events

        #region Commom Events
        /// <summary>
        /// 窗口加载时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SerialPort.Scan();
            CmbName.ItemsSource = SerialPort.PortNames;
        } 
        #endregion

        #region Port Config Event
        /// <summary>
        /// 刷新串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SerialPort.Scan();
            CmbName.ItemsSource = SerialPort.PortNames;

            if (SerialPort.PortNames.Count == 0)
            {
                MessageBox.Show("当前无可用串口，请检查串口设备！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// 打开串口与关闭串口按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenClose_Click(object sender, RoutedEventArgs e)
        {
            if (SerialPort.IsOpen)
            {
                SetPortClose();
                SerialPort.Close();
            }
            else
            {
                if (CmbName.Text != "")
                {
                    SerialPort.Open();
                    SetPortOpen();
                }
                else
                {
                    MessageBox.Show("请选择串口！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        /// <summary>
        /// 修改名字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerialPort.PortName = CmbName.SelectedItem.ToString();
        }

        /// <summary>
        /// 修改波特率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBaudrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerialPort.Baudrate = baudrates[CmbBaudrate.SelectedIndex];
        }

        /// <summary>
        /// 修改数据位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbDatabits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SerialPort.DataBits = 8 - CmbDatabits.SelectedIndex;
        }

        /// <summary>
        /// 修改停止位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbStopbits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CmbStopbits.SelectedIndex)
            {
                case 0:
                    SerialPort.StopBits = StopBits.One;
                    break;
                case 1:
                    SerialPort.StopBits = StopBits.OnePointFive;
                    break;
                case 2:
                    SerialPort.StopBits = StopBits.Two;
                    break;
                default:
                    SerialPort.StopBits = StopBits.One;
                    break;
            }
        }

        /// <summary>
        /// 修改校验位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbParity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CmbParity.SelectedIndex)
            {
                case 0:
                    SerialPort.Parity = Parity.None;
                    break;
                case 1:
                    SerialPort.Parity = Parity.Odd;
                    break;
                case 2:
                    SerialPort.Parity = Parity.Even;
                    break;
                default:
                    SerialPort.Parity = Parity.None;
                    break;
            }
        }
        #endregion

        #region Receive Event
        /// <summary>
        /// 接收数据函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_OnReceiveData(string data, int count, object sender, SerialDataReceivedEventArgs e)
        {
            // 读内容
            Dispatcher.Invoke(() =>
            {
                AddMessage(data);
                SerialPort.ReceiveCount += count;
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LoadMessage();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadMessage();
        }

        /// <summary>
        /// 保存接收内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            if (sfd.ShowDialog() == true)
            {
                string file = sfd.FileName;
                SaveFile(file, RtbRece);
            }
        }

        /// <summary>
        /// 清屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            paragraph.Inlines.Clear();
        }
        #endregion

        #region Send Events
        /// <summary>
        /// 发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendData();
        }

        /// <summary>
        /// 自动发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbAutoSend_Checked(object sender, RoutedEventArgs e)
        {
            Timer.Interval = Convert.ToDouble(txbTimeInterval.Text);
            Timer.Start();
        }

        /// <summary>
        /// 停止自动发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbAutoSend_Unchecked(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }
        /// <summary>
        /// 时间循环事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 不断发送数据
            SendData();
        }



        /// <summary>
        /// 16进制发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbSendHex_Checked(object sender, RoutedEventArgs e)
        {
            string tmp = TxbSend.Text;
            // 将内容转化为16进制
            byte[] data = Encoding.ASCII.GetBytes(tmp);
            builder.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("X2") + " ");
            }
            TxbSend.Text = builder.ToString();
        }

        /// <summary>
        /// 字符串发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbSendHex_Unchecked(object sender, RoutedEventArgs e)
        {
            string tmp = TxbSend.Text;
            try
            {
                byte[] data = StringToHexByte(tmp);
                TxbSend.Text = Encoding.ASCII.GetString(data, 0, data.Length);
            }
            catch { }
        }
        #endregion

        #endregion

        private void CkbSet_Checked(object sender, RoutedEventArgs e)
        {
            ColRight.Width = new GridLength(180);
        }

        private void CkbSet_Unchecked(object sender, RoutedEventArgs e)
        {
            ColRight.Width = new GridLength(0);
        }
    }
}
