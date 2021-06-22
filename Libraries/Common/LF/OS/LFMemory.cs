/*──────────────────────────────────────────────────────────────
 * FileName     : LFMemory.cs
 * Created      : 2021-06-22 11:38:11
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF
{
    public class LFMemory
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFMemory()
        {
        }
        #endregion

        #region Methods

        public static byte[] GetMemory(float f)
        {
            return BitConverter.GetBytes(f);
        }

        public static short FromMemory2(byte[] mem)
        {

            byte a1 = mem[0];
            byte a2 = mem[1];

            short temp;
            temp = (short)(a1);
            temp |= (short)(((short)a2) << 8);

            
            return temp;
        }
        #endregion
    }
}