/*──────────────────────────────────────────────────────────────
 * FileName     : ScaleState
 * Created      : 2019-12-21 14:58:44
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
    public class ScaleState : ICloneable
    {
        /// <summary>
        /// The axis range data for <see cref="Scale.Min"/>, <see cref="Scale.Max"/>,
        /// <see cref="Scale.MinorStep"/>, and <see cref="Scale.MajorStep"/>
        /// </summary>
        private double _min, _minorStep, _majorStep, _max;
        /// <summary>
        /// The status of <see cref="Scale.MinAuto"/>,
        /// <see cref="Scale.MaxAuto"/>, <see cref="Scale.MinorStepAuto"/>,
        /// and <see cref="Scale.MajorStepAuto"/>
        /// </summary>
        private bool _minAuto, _minorStepAuto,
                            _majorStepAuto, _maxAuto,
                            _formatAuto, _magAuto;

        private string _format;
        private int _mag;

        /// <summary>
        /// Construct a <see cref="ScaleState"/> from the specified <see cref="Axis"/>
        /// </summary>
        /// <param name="axis">The <see cref="Axis"/> from which to collect the scale
        /// range settings.</param>
        public ScaleState(Axis axis)
        {
            _min = axis._scale.Min;
            _minorStep = axis._scale.MinorStep;
            _majorStep = axis._scale.MajorStep;
            _max = axis._scale.Max;

            _format = axis._scale._format;
            _mag = axis._scale.Mag;
            //this.numDec = axis.NumDec;

            _minAuto = axis._scale.MinAuto;
            _majorStepAuto = axis._scale.MajorStepAuto;
            _minorStepAuto = axis._scale.MinAuto;
            _maxAuto = axis._scale.MaxAuto;

            _formatAuto = axis._scale.FormatAuto;
            _magAuto = axis._scale.MagAuto;
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="ScaleState"/> object from which to copy</param>
        public ScaleState(ScaleState rhs)
        {
            _min = rhs._min;
            _majorStep = rhs._majorStep;
            _minorStep = rhs._minorStep;
            _max = rhs._max;
            _format = rhs._format;
            _mag = rhs._mag;

            _minAuto = rhs._minAuto;
            _majorStepAuto = rhs._majorStepAuto;
            _minorStepAuto = rhs._minorStepAuto;
            _maxAuto = rhs._maxAuto;

            _formatAuto = rhs._formatAuto;
            _magAuto = rhs._magAuto;
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
        public ScaleState Clone()
        {
            return new ScaleState(this);
        }

        /// <summary>
        /// Copy the properties from this <see cref="ScaleState"/> out to the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="axis">The <see cref="Axis"/> reference to which the properties should be
        /// copied</param>
        public void ApplyScale(Axis axis)
        {
            axis._scale.Min = _min;
            axis._scale.MajorStep = _majorStep;
            axis._scale.MinorStep = _minorStep;
            axis._scale.Max = _max;
            axis._scale.Format = _format;
            axis._scale.Mag = _mag;

            // The auto settings must be made after the min/step/max settings, since setting those
            // properties actually affects the auto settings.
            axis._scale.MinAuto = _minAuto;
            axis._scale.MinorStepAuto = _minorStepAuto;
            axis._scale.MajorStepAuto = _majorStepAuto;
            axis._scale.MaxAuto = _maxAuto;

            axis._scale.FormatAuto = _formatAuto;
            axis._scale.MagAuto = _magAuto;

        }

        /// <summary>
        /// Determine if the state contained in this <see cref="ScaleState"/> object is different from
        /// the state of the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="axis">The <see cref="Axis"/> object with which to compare states.</param>
        /// <returns>true if the states are different, false otherwise</returns>
        public bool IsChanged(Axis axis)
        {
            return axis._scale.Min != _min ||
                    axis._scale.MajorStep != _majorStep ||
                    axis._scale.MinorStep != _minorStep ||
                    axis._scale.Max != _max ||
                      axis._scale.MinAuto != _minAuto ||
                    axis._scale.MinorStepAuto != _minorStepAuto ||
                    axis._scale.MajorStepAuto != _majorStepAuto ||
                    axis._scale.MaxAuto != _maxAuto;
        }

    }
}
