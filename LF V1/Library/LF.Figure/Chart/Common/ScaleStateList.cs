﻿/*──────────────────────────────────────────────────────────────
 * FileName     : ScaleStateList
 * Created      : 2019-12-21 14:59:27
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
    public class ScaleStateList : List<ScaleState>, ICloneable
    {
        /// <summary>
        /// Construct a new <see cref="ScaleStateList" /> automatically from an
        /// existing <see cref="YAxisList" />.
        /// </summary>
        /// <param name="list">The <see cref="YAxisList" /> (a list of Y axes),
        /// from which to retrieve the state and create the <see cref="ScaleState" />
        /// objects.</param>
        public ScaleStateList(YAxisList list)
        {
            foreach (Axis axis in list)
                this.Add(new ScaleState(axis));
        }

        /// <summary>
        /// Construct a new <see cref="ScaleStateList" /> automatically from an
        /// existing <see cref="Y2AxisList" />.
        /// </summary>
        /// <param name="list">The <see cref="Y2AxisList" /> (a list of Y axes),
        /// from which to retrieve the state and create the <see cref="ScaleState" />
        /// objects.</param>
        public ScaleStateList(Y2AxisList list)
        {
            foreach (Axis axis in list)
                this.Add(new ScaleState(axis));
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="ScaleStateList"/> object from which to copy</param>
        public ScaleStateList(ScaleStateList rhs)
        {
            foreach (ScaleState item in rhs)
            {
                this.Add(item.Clone());
            }
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
        public ScaleStateList Clone()
        {
            return new ScaleStateList(this);
        }

        /// <summary>
        /// Iterate through the list of <see cref="ScaleState" /> objects, comparing them
        /// to the state of the specified <see cref="YAxisList" /> <see cref="Axis" />
        /// objects.
        /// </summary>
        /// <param name="list">A <see cref="YAxisList" /> object specifying a list of
        /// <see cref="Axis" /> objects to be compared with this <see cref="ScaleStateList" />.
        /// </param>
        /// <returns>true if a difference is found, false otherwise</returns>
        public bool IsChanged(YAxisList list)
        {
            int count = Math.Min(list.Count, this.Count);
            for (int i = 0; i < count; i++)
                if (this[i].IsChanged(list[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Iterate through the list of <see cref="ScaleState" /> objects, comparing them
        /// to the state of the specified <see cref="Y2AxisList" /> <see cref="Axis" />
        /// objects.
        /// </summary>
        /// <param name="list">A <see cref="Y2AxisList" /> object specifying a list of
        /// <see cref="Axis" /> objects to be compared with this <see cref="ScaleStateList" />.
        /// </param>
        /// <returns>true if a difference is found, false otherwise</returns>
        public bool IsChanged(Y2AxisList list)
        {
            int count = Math.Min(list.Count, this.Count);
            for (int i = 0; i < count; i++)
                if (this[i].IsChanged(list[i]))
                    return true;

            return false;
        }
        /*
				/// <summary>
				/// Indexer to access the specified <see cref="ScaleState"/> object by
				/// its ordinal position in the list.
				/// </summary>
				/// <param name="index">The ordinal position (zero-based) of the
				/// <see cref="ScaleState"/> object to be accessed.</param>
				/// <value>A <see cref="ScaleState"/> object reference.</value>
				public ScaleState this[ int index ]  
				{
					get { return (ScaleState) List[index]; }
					set { List[index] = value; }
				}
				/// <summary>
				/// Add a <see cref="ScaleState"/> object to the collection at the end of the list.
				/// </summary>
				/// <param name="state">A reference to the <see cref="ScaleState"/> object to
				/// be added</param>
				/// <seealso cref="IList.Add"/>
				public void Add( ScaleState state )
				{
					List.Add( state );
				}
		*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void ApplyScale(YAxisList list)
        {
            int count = Math.Min(list.Count, this.Count);
            for (int i = 0; i < count; i++)
                this[i].ApplyScale(list[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void ApplyScale(Y2AxisList list)
        {
            int count = Math.Min(list.Count, this.Count);
            for (int i = 0; i < count; i++)
                this[i].ApplyScale(list[i]);
        }
    }

}
