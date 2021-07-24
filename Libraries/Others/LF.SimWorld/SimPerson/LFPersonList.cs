/*──────────────────────────────────────────────────────────────
 * FileName     : LFPersonList.cs
 * Created      : 2021-07-02 10:54:07
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
    /// 人员列表
    /// </summary>
    public class LFPersonList : ObservableCollection<LFPerson>
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFPersonList()
        {
        }
        #endregion

        #region Methods

        #region Basic Methods

        /// <summary>
        /// 按照索引号排序
        /// </summary>
        public void Sort()
        {
            List<LFPerson> list = new List<LFPerson>();
            foreach (LFPerson obj in this)
            {
                list.Add(obj);
            }
            list.Sort(delegate (LFPerson O1, LFPerson O2) { return O1.Code.CompareTo(O2.Code); });
            Clear();
            foreach (LFPerson obj in list)
            {
                Add(obj);
            }
        }

        /// <summary>
        /// 按名称搜索ID
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>返回搜索到的ID，如未搜索到，返回0</returns>
        public long GetCode(string name)
        {
            foreach (LFPerson obj in this)
            {
                if (obj.Name == name)
                {
                    return obj.Code;
                }
            }
            return 0;
        }

        /// <summary>
        /// 按ID搜索名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetName(long code)
        {
            foreach (LFPerson obj in this)
            {
                if (obj.Code == code)
                {
                    return obj.Name;
                }
            }
            return "NaN";
        }

        /// <summary>
        /// 按名称搜索秘籍
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public LFPerson GetPerson(string name)
        {
            foreach (LFPerson obj in this)
            {
                if (obj.Name == name)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 按索引搜索秘籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LFPerson GetPerson(long code)
        {
            foreach (LFPerson obj in this)
            {
                if (obj.Code == code)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取同类项
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public LFPersonList GetSimilarPersons(long code)
        {
            LFPersonList list = new LFPersonList();
            foreach (LFPerson obj in this)
            {
                if (obj.Code / 1000 == code)
                {
                    list.Add(obj);
                }
            }
            return list;
        }
        #endregion


        #region File Methods

        /// <summary>
        /// 保存列表
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path, string file)
        {
            Sort(); // 保存之前先排序

            /* 基础操作 */
            XmlDocument xmlDoc = new XmlDocument();                                 // 定义文件
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // 定义声明
            xmlDoc.AppendChild(dec);                                                // 插入声明
            XmlElement root = xmlDoc.CreateElement("Persons");                        // 定义根节点
            xmlDoc.AppendChild(root);                                               // 插入根节点

            /* 开始写入 */
            if (this != null)
            {
                foreach (LFPerson obj in this)
                {
                    XmlElement ele = xmlDoc.CreateElement("Person");
                    ele.SetAttribute("Code", obj.Code.ToString());
                    ele.SetAttribute("Name", obj.Name);
                    ele.SetAttribute("Brief", obj.Brief);
                    root.AppendChild(ele);
                }
            }

            /* 保存文件 */
            xmlDoc.Save(path + @"\" + file + ".xml");
        }



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
            XmlNodeList nodes = root.ChildNodes;                        // 读取子节点

            /* 开始读取 */
            foreach (XmlNode node in nodes)
            {
                XmlElement ele = (XmlElement)node;

                int code = Convert.ToInt32(ele.GetAttribute("Code"));
                string name = ele.GetAttribute("Name");
                LFPerson obj = new LFPerson(code, name)
                {
                    Brief = ele.GetAttribute("Brief")
                };

                Add(obj);
            }
        }

        #endregion

        #endregion
    }
}