/*──────────────────────────────────────────────────────────────
 * FileName     : LogScale
 * Created      : 2019-12-21 14:42:34
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
    public class LogScale : Scale
    {
        #region Fields
        #endregion

        #region Properties
        public override AxisType Type
        {
            get { return AxisType.Log; }
        }

        public override double Min
        {
            get { return _min; }
            set { if (value > 0) _min = value; }
        }
        public override double Max
        {
            get { return _max; }
            set { if (value > 0) _max = value; }
        }

        #endregion

        #region Constructors
        public LogScale(Axis owner)
            : base(owner)
        {
        }
        public LogScale(Scale rhs, Axis owner)
    : base(rhs, owner)
        {
        }

        public override Scale Clone(Axis owner)
        {
            return new LogScale(this, owner);
        }

        #endregion

        #region Methods
        override public void SetupScaleData(ChartFigure graph, Axis axis)
        {
            base.SetupScaleData(graph, axis);

            _minLinTemp = Linearize(_min);
            _maxLinTemp = Linearize(_max);
        }
        override public double Linearize(double val)
        {
            return SafeLog(val);
        }
        override public double DeLinearize(double val)
        {
            return Math.Pow(10.0, val);
        }
        override internal double GetMajorTickValue(double baseVal, double tick)
        {
            return baseVal + (double)tick * CyclesPerStep;

            //	double val = baseVal + (double)tick * CyclesPerStep;
            //	double frac = val - Math.Floor( val );
        }
        override internal double GetMinorTickValue(double baseVal, int iTic)
        {
            double[] dLogVal = { 0, 0.301029995663981, 0.477121254719662, 0.602059991327962,
                                    0.698970004336019, 0.778151250383644, 0.845098040014257,
                                    0.903089986991944, 0.954242509439325, 1 };

            return baseVal + Math.Floor((double)iTic / 9.0) + dLogVal[(iTic + 9) % 9];
        }
        override internal int GetMinorStart(double baseVal)
        {
            return -9;
        }
        override internal double GetBaseTick()
        {
            if (_baseTick != double.MaxValue)
                return _baseTick;
            else
            {
                // go to the nearest even multiple of the step size
                return Math.Ceiling(Scale.SafeLog(_min) - 0.00000001);
            }

        }
        override internal int GetNumTicks()
        {
            int nTicks = 1;

            //iStart = (int) ( Math.Ceiling( SafeLog( this.min ) - 1.0e-12 ) );
            //iEnd = (int) ( Math.Floor( SafeLog( this.max ) + 1.0e-12 ) );

            //nTicks = (int)( ( Math.Floor( Scale.SafeLog( _max ) + 1.0e-12 ) ) -
            //		( Math.Ceiling( Scale.SafeLog( _min ) - 1.0e-12 ) ) + 1 ) / CyclesPerStep;
            nTicks = (int)((Scale.SafeLog(_max) - Scale.SafeLog(_min)) / CyclesPerStep) + 1;

            if (nTicks < 1)
                nTicks = 1;
            else if (nTicks > 1000)
                nTicks = 1000;

            return nTicks;
        }
        private double CyclesPerStep
        {
            //get { return (int)Math.Max( Math.Floor( Scale.SafeLog( _majorStep ) ), 1 ); }
            get { return _majorStep; }
        }
        override public void PickScale(ChartFigure graph, Graphics g, float scaleFactor)
        {
            // call the base class first
            base.PickScale(graph, g, scaleFactor);

            // Majorstep is always 1 for log scales
            if (_majorStepAuto)
                _majorStep = 1.0;

            _mag = 0;       // Never use a magnitude shift for log scales
                            //this.numDec = 0;		// The number of decimal places to display is not used

            // Check for bad data range
            if (_min <= 0.0 && _max <= 0.0)
            {
                _min = 1.0;
                _max = 10.0;
            }
            else if (_min <= 0.0)
            {
                _min = _max / 10.0;
            }
            else if (_max <= 0.0)
            {
                _max = _min * 10.0;
            }

            // Test for trivial condition of range = 0 and pick a suitable default
            if (_max - _min < 1.0e-20)
            {
                if (_maxAuto)
                    _max = _max * 2.0;
                if (_minAuto)
                    _min = _min / 2.0;
            }

            // Get the nearest power of 10 (no partial log cycles allowed)
            if (_minAuto)
                _min = Math.Pow((double)10.0,
                    Math.Floor(Math.Log10(_min)));
            if (_maxAuto)
                _max = Math.Pow((double)10.0,
                    Math.Ceiling(Math.Log10(_max)));

        }
        override internal string MakeLabel(ChartFigure graph, int index, double dVal)
        {
            if (_format == null)
                _format = Scale.Default.Format;

            if (_isUseTenPower)
                return string.Format("{0:F0}", dVal);
            else
                return Math.Pow(10.0, dVal).ToString(_format);
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
