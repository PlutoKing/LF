/*──────────────────────────────────────────────────────────────
 * FileName     : Y2AxisList
 * Created      : 2019-12-21 13:16:47
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
    public class Y2AxisList : List<Y2Axis>, ICloneable
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Y2AxisList()
        {
        }

        public Y2AxisList(Y2AxisList rhs)
        {
            foreach (Y2Axis item in rhs)
            {
                this.Add(item.Clone());
            }
        }

        public Y2AxisList Clone()
        {
            return new Y2AxisList(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region Methods

        public new Y2Axis this[int index]
        {
            get { return (((index < 0 || index >= this.Count) ? null : base[index])); }
        }
        public Y2Axis this[string title]
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
            foreach (Y2Axis axis in this)
            {
                if (String.Compare(axis.Title.Text, title, true) == 0)
                    return index;
                index++;
            }

            return -1;
        }

        public int IndexOfTag(string tagStr)
        {
            int index = 0;
            foreach (Y2Axis axis in this)
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
            Y2Axis axis = new Y2Axis(title);
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
