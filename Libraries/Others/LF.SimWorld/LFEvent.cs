/*──────────────────────────────────────────────────────────────
 * FileName     : LFEvent.cs
 * Created      : 2021-07-03 13:31:41
 * Author       : Xu Zhe
 * Description  : 事件
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.SimWorld
{
    /// <summary>
    /// 事件
    /// </summary>
    public class LFEvent:LFObject,ICloneable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFEvent()
        {
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs">源对象</param>
        public LFEvent(LFEvent rhs) : base(rhs)
        {
            
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns>拷贝的新对象</returns>
        public LFEvent Clone()
        {
            return new LFEvent(this);
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns>拷贝的新对象</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Methods
        public override void Decode()
        {
            throw new NotImplementedException();
        }

        public override void Encode()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}