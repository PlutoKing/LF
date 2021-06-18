/*──────────────────────────────────────────────────────────────
 * FileName     : LFMessage.cs
 * Created      : 2021-06-17 19:41:31
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace LF.SerialCommunication
{
    /// <summary>
    /// 消息
    /// </summary>
    public class LFMessage : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// 实现接口：属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _time; // 时间戳
        private byte[] _buffer; // 数据缓冲
        private string _content;
        private int _count;     // 数据长度

        /// <summary>
        /// 字符串构造器
        /// </summary>
        private StringBuilder builder = new StringBuilder();

        #endregion

        #region Properties

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get => _time;
            set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Content"));
            }
        }

        /// <summary>
        /// 数据缓冲
        /// </summary>
        public byte[] Buffer
        {
            get => _buffer; 
            set
            {
                _buffer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Buffer"));
            }
        }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Count { get => _count;
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }

        #endregion

        #region Constructors
        public LFMessage()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// 解读缓冲区数据
        /// </summary>
        /// <param name="isHex"></param>
        public void ReadBuffer(bool isHex)
        {
            
            // 16进制显示
            if (isHex)
            {
                builder.Clear();
                for (int i = 0; i < _count; i++)
                {
                    builder.Append(_buffer[i].ToString("X2") + " ");
                }
                Content = builder.ToString();
            }
            // 字符串显示
            else
            {
                Content = Encoding.ASCII.GetString(_buffer, 0, _count);
            }
        }


        #endregion
    }
}