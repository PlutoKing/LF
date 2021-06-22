/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-06-22 20:06:30
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Xml;

namespace LF.SerialCommunication.Project.ViewModel
{
    public class MainWindowModel:LFNotify
    {
        #region Fields

        private SerialPort _port;           // 串口

        private List<string> _portNames;    // 所有可用串口名称

        private int _receiveCount;          // 接受数据计数

        private int _sendCount;             // 发送数据计数

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; Notify(); }
        }

        private int _baudrate = 9600;

        public int Baudrate
        {
            get { return _baudrate; }
            set { _baudrate = value; Notify(); }
        }

        private int _databits = 0;

        public int Databits
        {
            get { return _databits; }
            set { _databits = value; Notify(); }
        }

        private int _stopbits = 0;

        public int Stopbits
        {
            get { return _stopbits; }
            set { _stopbits = value; Notify(); }
        }

        private int _parity = 0;

        public int Parity
        {
            get { return _parity; }
            set { _parity = value; Notify(); }
        }

        #endregion

        #region Properties
        /// <summary>
        /// 串口
        /// </summary>
        public SerialPort Port
        {
            get { return _port; }
            set { _port = value; Notify(); }
        }

        /// <summary>
        /// 所有可用串口名称
        /// </summary>
        public List<string> PortNames
        {
            get { return _portNames; }
            set { _portNames = value; Notify(); }
        }

        /// <summary>
        /// 接收数据计数
        /// </summary>
        public int ReceiveCount
        {
            get { return _receiveCount; }
            set { _receiveCount = value; Notify(); }
        }

        /// <summary>
        /// 发送数据计数
        /// </summary>
        public int SendCount
        {
            get { return _sendCount; }
            set { _sendCount = value; Notify(); }
        }
        #region Commands
        public LFCommand OpenOrCloseSerialPortCmd { get; set; }
        #endregion
        #endregion

        #region Constructors
        public MainWindowModel()
        {
            /* 打开串口 */
            OpenOrCloseSerialPortCmd = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    OpenOrCloseSerialPort();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// 扫描串口
        /// </summary>
        public void ScanSerialPorts()
        {
            string[] portNames = SerialPort.GetPortNames();
            _portNames = portNames.ToList();
        }

        /// <summary>
        /// 串口配制
        /// </summary>
        public void SerialPortConfig()
        {
            Port = new SerialPort();
            Port.PortName = Name;
            Port.BaudRate = Baudrate;
            Port.DataBits = 8 - Databits;
            switch (Stopbits)
            {
                case 0:
                    Port.StopBits = StopBits.One;
                    break;
                case 1:
                    Port.StopBits = StopBits.OnePointFive;
                    break;
                case 2:
                    Port.StopBits = StopBits.Two;
                    break;
                default:
                    Port.StopBits = StopBits.One;
                    break;
            }

            switch (Parity)
            {
                case 0:
                    Port.Parity = System.IO.Ports.Parity.None;
                    break;
                case 1:
                    Port.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case 2:
                    Port.Parity = System.IO.Ports.Parity.Even;
                    break;
                default:
                    Port.Parity = System.IO.Ports.Parity.None;
                    break;
            }
        }
        
        /// <summary>
        /// 打开或关闭串口
        /// </summary>
        public void OpenOrCloseSerialPort()
        {

            if (_port.IsOpen)
            {
                _port.Close();
                // 计数清零
                ReceiveCount = 0;
                SendCount = 0;
            }
            else
            {
                _port.Open();
            }
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
    }
}