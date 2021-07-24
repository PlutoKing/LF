/*──────────────────────────────────────────────────────────────
 * FileName     : LFBlockFactory.cs
 * Created      : 2021-06-27 20:19:20
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace LF.Tetris.Project.Model
{
    public class LFBlockFactory
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFBlockFactory()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// 随机方块工厂
        /// </summary>
        static public LFBlock GetRandomBox(ref Grid grid)
        {
            //return new Box_Z(ref grid);
            Random ran = new Random();
            int index = ran.Next(7);
            switch (index)
            {
                case 0: return new LFBlockO(ref grid);
                case 1: return new LFBlockI(ref grid);
                case 2: return new LFBlockL(ref grid);
                case 3: return new LFBlockJ(ref grid);
                case 4: return new LFBlockS(ref grid);
                case 5: return new LFBlockZ(ref grid);
                case 6: return new LFBlockT(ref grid);
                default: return null;
            }
        }
        #endregion
    }
}