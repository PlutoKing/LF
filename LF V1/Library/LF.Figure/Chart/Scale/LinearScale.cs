/*──────────────────────────────────────────────────────────────
 * FileName     : LinearScale
 * Created      : 2019-12-21 12:45:48
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
    public class LinearScale : Scale
    {
        #region Fields
        #endregion

        #region properties

        /// <summary>
        /// Return the <see cref="AxisType" /> for this <see cref="Scale" />, which is
        /// <see cref="AxisType.Linear" />.
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Linear; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Default constructor that defines the owner <see cref="Axis" />
        /// (containing object) for this new object.
        /// </summary>
        /// <param name="owner">The owner, or containing object, of this instance</param>
        public LinearScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="LinearScale" /> object from which to copy</param>
        /// <param name="owner">The <see cref="Axis" /> object that will own the
        /// new instance of <see cref="LinearScale" /></param>
        public LinearScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }


        /// <summary>
        /// Create a new clone of the current item, with a new owner assignment
        /// </summary>
        /// <param name="owner">The new <see cref="Axis" /> instance that will be
        /// the owner of the new Scale</param>
        /// <returns>A new <see cref="Scale" /> clone.</returns>
        public override Scale Clone(Axis owner)
        {
            return new LinearScale(this, owner);
        }
        #endregion

        #region Methods
        override public void PickScale(ChartFigure graph, Graphics g, float scaleFactor)
        {
            // call the base class first
            base.PickScale(graph, g, scaleFactor);

            // Test for trivial condition of range = 0 and pick a suitable default
            if (_max - _min < 1.0e-30)
            {
                if (_maxAuto)
                    _max = _max + 0.2 * (_max == 0 ? 1.0 : Math.Abs(_max));
                if (_minAuto)
                    _min = _min - 0.2 * (_min == 0 ? 1.0 : Math.Abs(_min));
            }

            // This is the zero-lever test.  If minVal is within the zero lever fraction
            // of the data range, then use zero.

            if (_minAuto && _min > 0 &&
                _min / (_max - _min) < Default.ZeroLever)
                _min = 0;

            // Repeat the zero-lever test for cases where the maxVal is less than zero
            if (_maxAuto && _max < 0 &&
                Math.Abs(_max / (_max - _min)) <
                Default.ZeroLever)
                _max = 0;

            // Getulate the new step size
            if (_majorStepAuto)
            {
                double targetSteps = (_ownerAxis is XAxis || _ownerAxis is X2Axis) ?
                            Default.TargetXSteps : Default.TargetYSteps;

                // Getulate the step size based on target steps
                _majorStep = GetStepSize(_max - _min, targetSteps);

                if (_isPreventLabelOverlap)
                {
                    // Getulate the maximum number of labels
                    double maxLabels = (double)this.GetMaxLabels(g, graph, scaleFactor);

                    if (maxLabels < (_max - _min) / _majorStep)
                        _majorStep = GetBoundedStepSize(_max - _min, maxLabels);
                }
            }

            // Getulate the new step size
            if (_minorStepAuto)
                _minorStep = GetStepSize(_majorStep,
                    (_ownerAxis is XAxis || _ownerAxis is X2Axis) ?
                            Default.TargetMinorXSteps : Default.TargetMinorYSteps);

            // Getulate the scale minimum
            if (_minAuto)
                _min = _min - MyMod(_min, _majorStep);

            // Getulate the scale maximum
            if (_maxAuto)
                _max = MyMod(_max, _majorStep) == 0.0 ? _max :
                    _max + _majorStep - MyMod(_max, _majorStep);

            SetScaleMag(_min, _max, _majorStep);
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
