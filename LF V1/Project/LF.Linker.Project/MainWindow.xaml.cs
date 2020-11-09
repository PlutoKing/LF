/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindow
 * Created      : 2020-10-28 10:50:16
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace LF.Linker.Project
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        LFPort port = new LFPort();         // 串口
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = port;

            port.DataReceived += Port_DataReceived;
        }


        #endregion

        #region Methods

        /// <summary>
        /// 扫描串口
        /// </summary>
        public void ScanPort()
        {
            string[] portArr = SerialPort.GetPortNames();
            if (portArr.Length == 0)
            {
                MessageBox.Show("当前无可用串口，请检查串口设备！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CmbPortName.ItemsSource = portArr;
                CmbPortName.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void OpenPort()
        {
            port.PortName = CmbPortName.Text;
            port.BaudRate = Convert.ToInt32(CmbBaudrate.Text);
            port.DataBits = Convert.ToInt32(CmbDataBit.Text);
            switch (CmbParityBit.SelectedIndex)
            {
                case 0:
                    port.Parity = Parity.None;
                    break;
                case 1:
                    port.Parity = Parity.Odd;
                    break;
                case 2:
                    port.Parity = Parity.Even;
                    break;
                default:
                    port.Parity = Parity.None;
                    break;
            }
            switch (CmbStopBit.SelectedIndex)
            {
                case 0:
                    port.StopBits = StopBits.One;
                    break;
                case 1:
                    port.StopBits = StopBits.OnePointFive;
                    break;
                case 2:
                    port.StopBits = StopBits.Two;
                    break;
                default:
                    port.StopBits = StopBits.One;
                    break;
            }
            port.Open();
            BtnOpenClose.Content = "关闭串口";

        }


        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage()
        {
            string SendData = TxbSend.Text;
            byte[] Data = new byte[20];
            for (int i = 0; i < SendData.Length / 2; i++)
            {
                //每次取两位字符组成一个16进制
                Data[i] = Convert.ToByte(TxbSend.Text.Substring(i * 2, 2), 16);
            }
            this.port.Write(Data, 0, Data.Length);
        }
        #endregion

        #region Events

        /// <summary>
        /// 接收消息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int len = this.port.BytesToRead;
            port.ReceiveByte += len;
            byte[] buffer = new byte[len];
            this.port.Read(buffer, 0, len);
            string strData = BitConverter.ToString(buffer, 0, len);
            Dispatcher.Invoke(() =>
            {
                //this.TxbReceive.Text += strData;
                //this.TxbReceive.Text += '\n';   //字符分隔 
            });
        }
        #endregion

        #region Defaults
        #endregion
    }
}
