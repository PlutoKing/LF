/*──────────────────────────────────────────────────────────────
 * FileName     : LFFamily.cs
 * Created      : 2021-07-02 10:49:22
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
    /// 家庭
    /// </summary>
    public class LFFamily:LFNotify
    {
        #region Fields
        private int _code;          // 编码
        private string _name = "一个家庭";       // 名称

        private LFPersonList _familyMembers;

        private string _location = "某省某市某县某个地方";

        public string Location
        {
            get { return _location; }
            set { _location = value; Notify(); }
        }


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
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; Notify(); }
        }

        /// <summary>
        /// 家庭成员
        /// </summary>
        public LFPersonList FamilyMembers
        {
            get { return _familyMembers; }
            set { _familyMembers = value; Notify(); }
        }
        #endregion

        #region Constructors
        public LFFamily()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// 打开列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void Open(string path, string file)
        {
            /* XML基础操作 */
            XmlDocument xmlDoc = new XmlDocument();                     // 定义文件
            xmlDoc.Load(path + @"\" + file + ".xml");                   // 加载文件
            XmlElement root = xmlDoc.DocumentElement;                   // 读取根节点

            Code = Convert.ToInt32(root.GetAttribute("Code"));
            Name = root.GetAttribute("Name");

            XmlElement eleMembers = (XmlElement)root.GetElementsByTagName("Members")[0];

            FamilyMembers = new LFPersonList();
            /* 开始读取 */
            foreach (XmlNode node in eleMembers.ChildNodes)
            {
                XmlElement ele = (XmlElement)node;

                int code = Convert.ToInt32(ele.GetAttribute("Code"));
                string name = ele.GetAttribute("Name");

                FamilyMembers.Add(new LFPerson(code, name));
            }
        }
        #endregion
    }
}