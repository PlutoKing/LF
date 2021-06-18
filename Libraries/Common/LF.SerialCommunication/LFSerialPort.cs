/*──────────────────────────────────────────────────────────────
 * FileName     : LFSerialPort.cs
 * Created      : 2021-06-11 11:08:39
 * Author       : Xu Zhe
 * Description  : 管理串口的基本操作和通信方法
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace LF.SerialCommunication
{
    /// <summary>
    /// 串口
    /// </summary>
    public class LFSerialPort : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 接收数据事件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="count">位数</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ReceiveDataHandler(string data,int count,object sender, SerialDataReceivedEventArgs e);
        public ReceiveDataHandler OnReceiveData;

        private SerialPort _port;   // 串口
        private List<string> _portNames;    // 所有可用串口名称
        private double _time;       // 串口时间


        private int _receiveCount;  // 接收量
        private int _sendCount;     // 发送量

        private ObservableCollection<LFMessage> _messages = new ObservableCollection<LFMessage>();  // 接收消息
        private string _sendContent;        // 发送内容

        private bool _isShowTime = true;    // 是否显示时间
        private bool _isSendHex = true;     // 是否以16进制发送
        private bool _isNewLine = true;     // 是否自动换行
        private bool _isShowSend = true;    // 是否显示发送消息
        private bool _isShowHex = true;     // 是否以16进制显示
        /// <summary>
        /// 字符串构造器
        /// </summary>
        private StringBuilder builder = new StringBuilder();
        #endregion

        #region Properties

        //public SerialPort Port { get { return _port; } set { _port = value; } }
        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName
        {
            get { return _port.PortName; }
            set { _port.PortName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public int Baudrate
        {
            get { return _port.BaudRate; }
            set { _port.BaudRate = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits
        {
            get { return _port.DataBits; }
            set { _port.DataBits = value; }
        }

        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity
        {
            get { return _port.Parity; }
            set { _port.Parity = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return _port.StopBits; }
            set { _port.StopBits = value; }
        }

      
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }
        /// <summary>
        /// 接收数据量
        /// </summary>
        public int ReceiveCount { get => _receiveCount;
            set
            {
                _receiveCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReceiveCount"));
            }
        }
        
        /// <summary>
        /// 发送数据量
        /// </summary>
        public int SendCount { get => _sendCount;
            set
            {
                _sendCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SendCount"));
            }
        }
        /// <summary>
        /// 所以可用串口名称
        /// </summary>
        public List<string> PortNames { get => _portNames; set => _portNames = value; }
        public bool IsShowTime { get => _isShowTime; set => _isShowTime = value; }
        public bool IsSendHex { get => _isSendHex; set => _isSendHex = value; }
        public string SendContent { get => _sendContent; set => _sendContent = value; }
        public bool IsNewLine { get => _isNewLine; set => _isNewLine = value; }
        public bool IsShowHex { get => _isShowHex; set => _isShowHex = value; }
        public bool IsShowSend { get => _isShowSend; set => _isShowSend = value; }
        public ObservableCollection<LFMessage> Messages { get => _messages; set => _messages = value; }
        public double Time { get => _time; set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }


        #endregion

        #region Constructors
        public LFSerialPort()
        {
            _port = new SerialPort();
            _port.DataReceived += Port_DataReceived;
        }


        #endregion

        #region Methods

        #region Commom Methods

        /// <summary>
        /// 扫描串口
        /// </summary>
        public void Scan()
        {
            string[] portNames = SerialPort.GetPortNames();
            _portNames = portNames.ToList();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            
            _port.Open();

            // 计数器清零
            _receiveCount = 0;
            _sendCount = 0;
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            _port.Close();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">字符串数据</param>
        public void SendData(string data)
        {
            if (_port.IsOpen)
            {
                byte[] tmp = Encoding.ASCII.GetBytes(data);
                _port.Write(tmp, 0, tmp.Length);
                SendCount += tmp.Length;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">字节数组数据</param>
        public void SendData(byte[] data)
        {
            if (_port.IsOpen)
            {
                _port.Write(data, 0, data.Length);
                SendCount += data.Length;
            }
        }



        #endregion

        #endregion

        #region Events

        /// <summary>
        /// 接收数据是发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            if (OnReceiveData != null)
            {
                byte[] readBuffer = new byte[_port.ReadBufferSize + 1];
                int count = _port.Read(readBuffer, 0, _port.ReadBufferSize);
                
                if (count != 0)
                {
                    LFMessage msg = new LFMessage
                    {
                        Time = DateTime.Now,    // 当前时间
                        Buffer = readBuffer,    // 数据缓冲
                        Count = count
                    };
                    _messages.Add(msg);         // 添加消息

                    string data;
                    // 16进制显示
                    if (IsShowHex)
                    {
                        builder.Clear();
                        for (int i = 0; i < count; i++)
                        {
                            builder.Append(readBuffer[i].ToString("X2") + " ");
                        }
                        data = builder.ToString();
                    }
                    // 字符串显示
                    else
                    {
                        data = Encoding.ASCII.GetString(readBuffer, 0, count);
                    }
                    // 外部接收数据时间
                    OnReceiveData(data, count, sender, e);
                }

                
            }
        }
        #endregion
    }
}