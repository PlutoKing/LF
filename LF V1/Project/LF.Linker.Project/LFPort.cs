/*──────────────────────────────────────────────────────────────
 * FileName     : LFPort
 * Created      : 2020-10-28 10:47:18
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;

namespace LF.Linker.Project
{
    /// <summary>
    /// 串口
    /// </summary>
    public class LFPort : SerialPort, INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private string _receiveMessage;
        private string _sendMessage;
        private int _receiveByte;
        private int _sendByte;

        #endregion

        #region Properties

        /// <summary>
        /// 接收消息
        /// </summary>
        public string ReceiveMessage { get => _receiveMessage; set => _receiveMessage = value; }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        public string SendMessage { get => _sendMessage; set => _sendMessage = value; }
        
        /// <summary>
        /// 接收字符数
        /// </summary>
        public int ReceiveByte { get => _receiveByte; set => _receiveByte = value; }
        
        /// <summary>
        /// 发送字符数
        /// </summary>
        public int SendByte { get => _sendByte; set => _sendByte = value; }

        #endregion

        #region Constructors
        public LFPort()
        {
        }
        #endregion

        #region Methods
        #endregion

        #region Events

        /// <summary>
        /// 属性改变事件
        /// </summary>
        /// <param name="info"></param>
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

    }
}
