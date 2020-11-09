/*──────────────────────────────────────────────────────────────
 * FileName     : LFComplex
 * Created      : 2020-10-13 11:05:41
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


namespace LF.Mathematics
{
    /// <summary>
    /// 复数
    /// </summary>
    public class LFComplex : ICloneable
    {
        #region Fields
        private double _real;
        private double _imag;
        #endregion

        #region Properties
        /// <summary>
        /// 实部
        /// </summary>
        public double Real { get => _real; set => _real = value; }

        /// <summary>
        /// 虚部
        /// </summary>
        public double Imag { get => _imag; set => _imag = value; }

        #endregion

        #region Constructors
        public LFComplex()
        {
            _real = 0;
            _imag = 0;
        }

        public LFComplex(double a, double b)
        {
            _real = a;
            _imag = b;
        }

        public LFComplex(LFComplex rhs)
        {
            _real = rhs._real;
            _imag = rhs._imag;
        }

        public LFComplex Clone()
        {
            return new LFComplex(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion

        #region Methods

        public static LFComplex Exp(double d)
        {
            return new LFComplex(Math.Cos(d), Math.Sin(d));
        }

        #region Computation

        public static LFComplex operator +(LFComplex c)
        {
            return new LFComplex(c);
        }

        public static LFComplex operator -(LFComplex c)
        {
            return new LFComplex(-c._real, -c._imag);
        }

        public static LFComplex operator +(LFComplex c1, LFComplex c2)
        {
            return new LFComplex(c1._real + c2._real, c1.Imag + c2._imag);
        }

        public static LFComplex operator -(LFComplex c1, LFComplex c2)
        {
            return new LFComplex(c1._real - c2._real, c1.Imag - c2._imag);
        }

        public static LFComplex operator *(double k, LFComplex c)
        {
            double x = k * c._real;
            double y = k * c._imag;

            return new LFComplex(x, y);
        }

        public static LFComplex operator *(LFComplex c, double k)
        {
            double x = k * c._real;
            double y = k * c._imag;

            return new LFComplex(x, y);
        }

        public static LFComplex operator *(LFComplex c1, LFComplex c2)
        {
            double x = c1._real * c2._real - c1._imag * c2._imag;
            double y = c1._real * c2._imag + c1._imag * c2._real;

            return new LFComplex(x, y);
        }

        public static LFComplex operator /(LFComplex c1, LFComplex c2)
        {
            double e, f, x, y;
            if (Math.Abs(c1._real) >= Math.Abs(c2._imag))
            {
                e = c2._imag / c2._real;
                f = c2._real + e * c2._imag;

                x = (c1._real + c1._imag * e) / f;
                y = (c1._imag - c1._real * e) / f;
            }
            {
                e = c2._real / c2._imag;
                f = c2._imag + e * c2._real;

                x = (c1._real * e + c1._imag) / f;
                y = (c1._imag * e - c1._real) / f;
            }
            return new LFComplex(x, y);
        }

        /// <summary>
        /// 模
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            double x = Math.Abs(_real);
            double y = Math.Abs(_imag);

            if (_real == 0)
                return y;
            if (_imag == 0)
                return x;

            if (x > y)
                return (x * Math.Sqrt(1 + (y / x) * (y / x)));

            return (y * Math.Sqrt(1 + (x / y) * (x * y)));
        }


        #endregion


        public override string ToString()
        {
            return _real.ToString() + _imag.ToString() + "i";
        }

        public string ToString(string format)
        {
            return _real.ToString(format) + _imag.ToString(format) + "i";
        }

        #endregion

        #region Serializations
        #endregion

        #region Defaults
        #endregion
    }
}
