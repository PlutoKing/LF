/*──────────────────────────────────────────────────────────────
 * FileName     : LFLink.cs
 * Created      : 2021-06-22 10:23:26
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.Communication
{
    /// <summary>
    /// 通信协议
    /// </summary>
    public class LFLink
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFLink()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// 单精度浮点数转化为2位无符号整数
        /// </summary>
        /// <param name="f">单精度浮点数</param>
        /// <returns></returns>
        public static byte[] FloatToByte2(float f,float s)
        {
            byte[] buf = new byte[2];
            short temp;
            if (f >= 0)
            {
                temp = (short)(f*s + 0.5);
            }
            else
            {
                temp = (short)(f*s- 0.5);
            }
            buf[0] = (byte)((temp) & 0xFF);
            buf[1] = (byte)((temp >> 8) & 0xFF);
            return buf;
        }

        /// <summary>
        /// 2位无符号整数转化为单精度浮点数
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="s">缩放因子</param>
        /// <returns></returns>
        public static float Byte2ToFloat(byte[] buf, float s)
        {
            byte a1 = buf[0];
            byte a2 = buf[1];

            short temp;
            temp = a1;
            temp |= (short)((a2) << 8);

            return (float)(temp * s);
        }
        #endregion
    }
}