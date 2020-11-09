/*──────────────────────────────────────────────────────────────
 * FileName     : ScrollRangeList
 * Created      : 2019-12-21 14:52:30
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
    public class ScrollRangeList : List<ScrollRange>, ICloneable
    {
        #region Constructors

        /// <summary>
        /// Default constructor for the collection class.
        /// </summary>
        public ScrollRangeList()
        {
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="ScrollRangeList"/> object from which to copy</param>
        public ScrollRangeList(ScrollRangeList rhs)
        {
            foreach (ScrollRange item in rhs)
                this.Add(new ScrollRange(item));
        }

        /// <summary>
        /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
        /// calling the typed version of <see cref="Clone" />
        /// </summary>
        /// <returns>A deep copy of this object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Typesafe, deep-copy clone method.
        /// </summary>
        /// <returns>A new, independent copy of this class</returns>
        public ScrollRangeList Clone()
        {
            return new ScrollRangeList(this);
        }


        #endregion

        #region List Methods

        /// <summary>
        /// Indexer to access the specified <see cref="ScrollRange"/> object by
        /// its ordinal position in the list.
        /// </summary>
        /// <param name="index">The ordinal position (zero-based) of the
        /// <see cref="ScrollRange"/> object to be accessed.</param>
        /// <value>A <see cref="ScrollRange"/> object instance</value>
        public new ScrollRange this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    return new ScrollRange(false);
                else
                    return (ScrollRange)base[index];
            }
            set { base[index] = value; }
        }

        #endregion

    }
}
