/*──────────────────────────────────────────────────────────────
 * FileName     : ZoomState
 * Created      : 2019-12-21 14:57:08
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
    public class ZoomState : ICloneable
    {
        /// <summary>
        /// An enumeration that describes whether a given state is the result of a Pan or Zoom
        /// operation.
        /// </summary>
        public enum StateType
        {
            /// <summary>
            /// Indicates the <see cref="ZoomState"/> object is from a Zoom operation
            /// </summary>
            Zoom,
            /// <summary>
            /// Indicates the <see cref="ZoomState"/> object is from a Wheel Zoom operation
            /// </summary>
            WheelZoom,
            /// <summary>
            /// Indicates the <see cref="ZoomState"/> object is from a Pan operation
            /// </summary>
            Pan,
            /// <summary>
            /// Indicates the <see cref="ZoomState"/> object is from a Scroll operation
            /// </summary>
            Scroll
        }

        /// <summary>
        /// <see cref="ScaleState"/> objects to store the state data from the axes.
        /// </summary>
        private ScaleState _xAxis, _x2Axis;
        private ScaleStateList _yAxis, _y2Axis;
        /// <summary>
        /// An enum value indicating the type of adjustment being made to the
        /// scale range state.
        /// </summary>
        private StateType _type;

        /// <summary>
        /// Gets a <see cref="StateType" /> value indicating the type of action (zoom or pan)
        /// saved by this <see cref="ZoomState" />.
        /// </summary>
        public StateType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets a string representing the type of adjustment that was made when this scale
        /// state was saved.
        /// </summary>
        /// <value>A string representation for the state change type; typically
        /// "Pan", "Zoom", or "Scroll".</value>
        public string TypeString
        {
            get
            {
                switch (_type)
                {
                    case StateType.Pan:
                        return "Pan";
                    case StateType.WheelZoom:
                        return "WheelZoom";
                    case StateType.Zoom:
                    default:
                        return "Zoom";
                    case StateType.Scroll:
                        return "Scroll";
                }
            }
        }

        /// <summary>
        /// Construct a <see cref="ZoomState"/> object from the scale ranges settings contained
        /// in the specified <see cref="ChartFigure"/>.
        /// </summary>
        /// <param name="graph">The <see cref="ChartFigure"/> from which to obtain the scale
        /// range values.
        /// </param>
        /// <param name="type">A <see cref="StateType"/> enumeration that indicates whether
        /// this saved state is from a pan or zoom.</param>
        public ZoomState(ChartFigure graph, StateType type)
        {

            _xAxis = new ScaleState(graph.XAxis);
            _x2Axis = new ScaleState(graph.X2Axis);
            _yAxis = new ScaleStateList(graph.YAxisList);
            _y2Axis = new ScaleStateList(graph.Y2AxisList);
            _type = type;
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="ZoomState"/> object from which to copy</param>
        public ZoomState(ZoomState rhs)
        {
            _xAxis = new ScaleState(rhs._xAxis);
            _x2Axis = new ScaleState(rhs._x2Axis);
            _yAxis = new ScaleStateList(rhs._yAxis);
            _y2Axis = new ScaleStateList(rhs._y2Axis);
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
        public ZoomState Clone()
        {
            return new ZoomState(this);
        }


        /// <summary>
        /// Copy the properties from this <see cref="ZoomState"/> out to the specified <see cref="ChartFigure"/>.
        /// </summary>
        /// <param name="graph">The <see cref="ChartFigure"/> to which the scale range properties should be
        /// copied.</param>
        public void ApplyState(ChartFigure graph)
        {
            _xAxis.ApplyScale(graph.XAxis);
            _x2Axis.ApplyScale(graph.X2Axis);
            _yAxis.ApplyScale(graph.YAxisList);
            _y2Axis.ApplyScale(graph.Y2AxisList);
        }

        /// <summary>
        /// Determine if the state contained in this <see cref="ZoomState"/> object is different from
        /// the state of the specified <see cref="ChartFigure"/>.
        /// </summary>
        /// <param name="graph">The <see cref="ChartFigure"/> object with which to compare states.</param>
        /// <returns>true if the states are different, false otherwise</returns>
        public bool IsChanged(ChartFigure graph)
        {
            return _xAxis.IsChanged(graph.XAxis) ||
                    _x2Axis.IsChanged(graph.X2Axis) ||
                    _yAxis.IsChanged(graph.YAxisList) ||
                    _y2Axis.IsChanged(graph.Y2AxisList);
        }

    }
}
