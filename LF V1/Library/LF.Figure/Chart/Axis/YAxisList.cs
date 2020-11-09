/*──────────────────────────────────────────────────────────────
 * FileName     : YAxisList
 * Created      : 2019-12-21 14:55:26
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    public class YAxisList : List<YAxis>, ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public YAxisList()
        {
        }

        public YAxisList(YAxisList rhs)
        {
            foreach (YAxis item in rhs)
            {
                this.Add(item.Clone());
            }
        }

        public YAxisList Clone()
        {
            return new YAxisList(this);
        }


        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods
        public new YAxis this[int index]
        {
            get { return (((index < 0 || index >= this.Count) ? null : base[index])); }
        }

        public YAxis this[string title]
        {
            get
            {
                int index = IndexOf(title);
                if (index >= 0)
                    return this[index];
                else
                    return null;
            }
        }

        public int IndexOf(string title)
        {
            int index = 0;
            foreach (YAxis axis in this)
            {
                if (String.Compare(axis.Title._text, title, true) == 0)
                    return index;
                index++;
            }

            return -1;
        }

        public int IndexOfTag(string tagStr)
        {
            int index = 0;
            foreach (YAxis axis in this)
            {
                if (axis.Tag is string &&
                    String.Compare((string)axis.Tag, tagStr, true) == 0)
                    return index;
                index++;
            }

            return -1;
        }
        public int Add(string title)
        {
            YAxis axis = new YAxis(title);
            Add(axis);

            return Count - 1;
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
