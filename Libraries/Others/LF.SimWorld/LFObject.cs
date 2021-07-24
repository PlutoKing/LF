/*──────────────────────────────────────────────────────────────
 * FileName     : LFObject.cs
 * Created      : 2021-07-03 11:49:19
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

/// <summary>
/// 对象
/// </summary>
namespace LF.SimWorld
{
    /// <summary>
    /// 对象抽象类
    /// </summary>
    /// <remarks>是所有对象的基础类</remarks>
    public abstract class LFObject : LFNotify,ICloneable
    {
        #region Fields
        private string _code;       // 编码
        private int _id;            // ID
        private string _name;       // 名称
        private string _brief;      // 简介
        #endregion

        #region Properties
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; Notify(); }
        }
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; Notify(); }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; Notify(); }
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string Brief
        {
            get { return _brief; }
            set { _brief = value; Notify(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 构造一个对象势力
        /// </summary>
        public LFObject()
        {
        }

        /// <summary>
        /// 构造编码为<paramref name="code"/>，名称为<paramref name="name"/>的对象实例
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        public LFObject(string code,string name)
        {
            _code = code;
            _name = name;
            Decode();
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs">源对象</param>
        public LFObject(LFObject rhs)
        {
            _code = rhs._code;
            _id = rhs._id;
            _name = rhs._name;
            _brief = rhs._brief;
        }
        
        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            throw new NotImplementedException("抽象类不可复制，其子类可被复制。");
        }
        #endregion

        #region Methods
        /// <summary>
        /// 解码
        /// </summary>
        public abstract void Decode();

        /// <summary>
        /// 编码
        /// </summary>
        public abstract void Encode();
        #endregion
    }
}