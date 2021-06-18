/*──────────────────────────────────────────────────────────────
 * FileName     : LF.cs
 * Created      : 2021-06-10 20:06:58
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
    public static partial class LF
    {
        public static void Swap<T>(IList<T> keys, int a, int b)
        {
            if (a == b)
            {
                return;
            }

            T local = keys[a];
            keys[a] = keys[b];
            keys[b] = local;
        }
    }
}