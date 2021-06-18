/*──────────────────────────────────────────────────────────────
 * FileName     : LFTypeList.cs
 * Created      : 2021-06-16 21:57:58
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
    /// <summary>
    /// 分类列表
    /// </summary>
    public class LFTypeList : ObservableCollection<LFType>, ICloneable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LFTypeList()
        {
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="rhs"></param>
        public LFTypeList(LFTypeList rhs)
        {
            foreach (LFType obj in rhs)
            {
                Add(obj.Clone());
            }
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        public LFTypeList Clone()
        {
            return new LFTypeList(this);
        }
        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Methods

        #region Search
        /// <summary>
        /// 按Value搜索Code 
        /// </summary>
        /// <param name="val">值</param>
        /// <returns>返回搜索到的编码，如未搜索到，返回-1</returns>
        public int GetCode(string val)
        {
            foreach (LFType obj in this)
            {
                if (obj.Value == val)
                {
                    return obj.Code;
                }
            }
            return -1;
        }

        /// <summary>
        /// 按Code 搜索Value
        /// </summary>
        /// <param name="idx">编码</param>
        /// <returns></returns>
        public string GetValue(long idx)
        {
            foreach (LFType obj in this)
            {
                if (obj.Code == idx)
                {
                    return obj.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 按Value搜索childs
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public LFTypeList GetChilds(string val)
        {
            foreach (LFType obj in this)
            {
                if (obj.Value == val)
                {
                    return obj.Childs;
                }
            }
            return null;
        }

        /// <summary>
        /// 按Code 搜索childs
        /// </summary>
        /// <param name="idx">编码</param>
        /// <returns></returns>
        public LFTypeList GetChilds(long idx)
        {
            foreach (LFType obj in this)
            {
                if (obj.Code == idx)
                {
                    return obj.Childs;
                }
            }
            return null;
        }
        /// <summary>
        /// 按Value搜索Type
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public LFType GetType(string val)
        {
            foreach (LFType obj in this)
            {
                if (obj.Value == val)
                {
                    return obj;
                }
            }
            return null;
        }


        /// <summary>
        /// 按Code 搜索Type
        /// </summary>
        /// <param name="idx">编码</param>
        /// <returns></returns>
        public LFType GetType(int idx)
        {
            foreach (LFType obj in this)
            {
                if (obj.Code == idx)
                {
                    return obj;
                }
            }
            return null;
        }

        #endregion

        #region File

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="file">文件名</param>
        public void Save(string path, string file)
        {
            /* 基础操作 */
            XmlDocument xmlDoc = new XmlDocument();                                 // 定义文件
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // 定义声明
            xmlDoc.AppendChild(dec);                                                // 插入声明
            XmlElement root = xmlDoc.CreateElement("Type");                         // 定义根节点
            xmlDoc.AppendChild(root);                                               // 插入根节点

            /* 开始写入 */
            if (this != null)
            {
                WriteTypeList(xmlDoc, root, this);
            }

            /* 保存文件 */
            xmlDoc.Save(path + @"\" + file + ".xml");
        }


        /// <summary>
        /// 写入类别列表
        /// </summary>
        /// <param name="xmlDoc">文件</param>
        /// <param name="root">根节点</param>
        /// <param name="types">类型列表</param>
        public void WriteTypeList(XmlDocument xmlDoc, XmlElement root, LFTypeList types)
        {
            foreach (LFType obj in types)
            {
                XmlElement ele = xmlDoc.CreateElement("Node");
                ele.SetAttribute("Code", obj.Code.ToString());
                ele.SetAttribute("Value", obj.Value);
                ele.SetAttribute("Brief", obj.Brief);

                if (obj.Childs != null)
                {
                    WriteTypeList(xmlDoc, ele, obj.Childs);
                }
                root.AppendChild(ele);
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="file">文件名</param>
        public void Open(string path, string file)
        {
            /* XML基础操作 */
            XmlDocument xmlDoc = new XmlDocument();         // 定义文件
            xmlDoc.Load(path + @"\" + file + ".xml");       // 加载文件
            XmlElement root = xmlDoc.DocumentElement;       // 读取根节点
            XmlNodeList nodes = root.ChildNodes;            // 读取子节点

            /* 开始读取 */
            foreach (XmlNode node in nodes)
            {
                XmlElement ele = (XmlElement)node;
                LFType obj = new LFType
                {
                    Code = Convert.ToInt32(ele.GetAttribute("Code")),
                    Value = ele.GetAttribute("Value"),
                    Brief = ele.GetAttribute("Brief")
                };
                if (ele.ChildNodes != null)
                {
                    if (ele.ChildNodes.Count != 0)
                    {
                        obj.Childs = ReadTypeList(ele);
                    }
                }
                Add(obj);
            }
        }

        /// <summary>
        /// 读取类别列表
        /// </summary>
        /// <param name="root">根节点</param>
        /// <returns>类别列表</returns>
        public LFTypeList ReadTypeList(XmlElement root)
        {
            XmlNodeList nodes = root.ChildNodes;
            LFTypeList result = new LFTypeList();
            foreach (XmlNode node in nodes)
            {
                XmlElement ele = (XmlElement)node;
                LFType obj = new LFType
                {
                    Code = Convert.ToInt32(ele.GetAttribute("Code")),
                    Value = ele.GetAttribute("Value"),
                    Brief = ele.GetAttribute("Brief")
                };
                if (ele.ChildNodes != null)
                {
                    if (ele.ChildNodes.Count != 0)
                    {
                        obj.Childs = ReadTypeList(ele);
                    }
                }

                result.Add(obj);
            }
            return result;
        }
        #endregion

        #endregion
    }
}