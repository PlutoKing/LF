/*──────────────────────────────────────────────────────────────
 * FileName     : MainWindowModel.cs
 * Created      : 2021-06-22 11:15:17
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml;
using LF.Communication;

namespace LF.LinkAssistant.Project.ViewModel
{
    /// <summary>
    /// “主窗口”<seealso cref="MainWindow"/>模型类
    /// </summary>
    public class MainWindowModel:LFNotify
    {
        #region Fields

        /// <summary>
        /// 第一个输入的浮点数内存
        /// </summary>
        private string _inputF1Memory;

        private string _outputF1Raw;

        private string _output1B1;

        private string _output1B2;

        private float _outputF1;

        #endregion

        #region Properties

        /// <summary>
        /// 单精度浮点数1
        /// </summary>
        public float F1 { get; set; } = 35.6f;

        public float Scale11 { get; set; } = 100.0f;
        public string OutputB1
        {
            get { return _output1B1; }
            set { _output1B1 = value; Notify(); }
        }
        public string OutputB2
        {
            get { return _output1B2; }
            set { _output1B2 = value; Notify(); }
        }

        public string B11 { get; set; } = "E8";
        public string B12 { get; set; } = "0D";

        public float Scale21 { get; set; } = 0.01f;

        /// <summary>
        /// 第一个输入的浮点数内存
        /// </summary>
        public string InputF1Memory
        {
            get { return _inputF1Memory; }
            set { _inputF1Memory = value; Notify(); }
        }

        /// <summary>
        /// 第1个输出的浮点数的内存
        /// </summary>
        public string OutputF1Raw
        {
            get { return _outputF1Raw; }
            set { _outputF1Raw = value; Notify(); }
        }

        /// <summary>
        /// 第1个输出的浮点数
        /// </summary>
        public float OutputF1
        {
            get { return _outputF1; }
            set { _outputF1 = value; Notify(); }
        }

        #region Command

        /// <summary>
        /// 计算
        /// </summary>
        public LFCommand CalculateFloatToByte2 { get; set; }

        public LFCommand CalculateByte2ToFloat { get; set; }
        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// “主窗口”<seealso cref="MainWindow"/>模型构造函数
        /// </summary>
        public MainWindowModel()
        {
            /* 计算按钮1 */
            CalculateFloatToByte2 = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    Float2Byte2String();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
            /* 计算按钮1 */
            CalculateByte2ToFloat = new LFCommand()
            {
                DoExecute = new Action<object>((sender) =>
                {
                    Byte2ToFloatString();
                }),
                DoCanExecute = new Func<object, bool>((sender) => { return true; })
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// 浮点数转化为2位无符号整型并转化为字符串
        /// </summary>
        /// <returns></returns>
        public void Float2Byte2String()
        {
            byte[] tmp = LFLink.FloatToByte2(F1,Scale11);
            byte[] mem = LFMemory.GetMemory(F1);
            OutputB1 = tmp[0].ToString("X2");
            OutputB2 = tmp[1].ToString("X2");
            InputF1Memory = "0x";
            for (int i = mem.Length-1; i >=0; i--)
            {
                InputF1Memory+=mem[i].ToString("X2");
            }
        }

        /// <summary>
        /// 2位无符号整型转化为浮点数并转化为字符串
        /// </summary>
        public void Byte2ToFloatString()
        {
            List<byte> buf0 = new List<byte>();
            MatchCollection mc = Regex.Matches(B11, @"(?i)[\dA-F]{2}");
            foreach (Match m in mc)
            {
                buf0.Add(byte.Parse(m.Value, System.Globalization.NumberStyles.HexNumber));
            }
            MatchCollection mc2 = Regex.Matches(B12, @"(?i)[\dA-F]{2}");
            foreach (Match m in mc2)
            {
                buf0.Add(byte.Parse(m.Value, System.Globalization.NumberStyles.HexNumber));
            }
            byte[] tmp = buf0.ToArray();

            OutputF1 = LFLink.Byte2ToFloat(tmp, Scale21);
            OutputF1Raw = LFMemory.FromMemory2(tmp).ToString();
        }
        #endregion
    }
}