/*──────────────────────────────────────────────────────────────
 * FileName     : LFRole.cs
 * Created      : 2021-07-03 12:00:04
 * Author       : Xu Zhe
 * Description  : 对象编码说明：
 *                R        - R表示角色
 *                20210101 - 8位编码表示生日
 *                01010101 - 8位编码表示籍贯
 *                00       - 2为编码表示种族性别
 *                1        - 1位ID识别码
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.SimWorld
{
    /// <summary>
    /// 角色
    /// </summary>
    public class LFRole:LFObject,ICloneable
    {
        #region Fields
        private DateTime _birthday;     // 生日
        #endregion

        #region Properties
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; Notify(); }
        }

        #endregion

        #region Constructors
        public LFRole()
        {
        }

        /// <summary>
        /// 构造编码为<paramref name="code"/>，名称为<paramref name="name"/>的角色实例
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        public LFRole(string code,string name)
            :base(code,name)
        {
            
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs">源对象</param>
        public LFRole(LFRole rhs):base(rhs)
        {
            _birthday = rhs._birthday;
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns>拷贝的新对象</returns>
        public LFRole Clone()
        {
            return new LFRole(this);
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