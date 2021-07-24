/*──────────────────────────────────────────────────────────────
 * FileName     : LFPerson.cs
 * Created      : 2021-07-02 10:45:40
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;

namespace LF.SimWorld
{
    /// <summary>
    /// 模拟人
    /// </summary>
    public class LFPerson:LFNotify
    {
        #region Fields
        private int _code;          // 编码
        private int _id;            // ID
        private string _name;       // 名称
        private string _brief;      // 简介
        #endregion

        #region Properties
        /// <summary>
        /// 编码
        /// </summary>
        public int Code
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
        public LFPerson()
        {
        }

        public LFPerson(int code,string name)
        {
            _code = code;
            _name = name;
        }
        #endregion

        #region Methods

        #endregion
    }
}