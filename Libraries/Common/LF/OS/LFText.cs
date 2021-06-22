/*──────────────────────────────────────────────────────────────
 * FileName     : LFText.cs
 * Created      : 2021-06-22 22:01:48
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace LF
{
    public static class LFText
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static Size MeasureString(TextBlock textblock)
        {
            var formattedText = new FormattedText(

            textblock.Text,

            CultureInfo.CurrentUICulture,

            FlowDirection.LeftToRight,

            new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch),

            textblock.FontSize,

            Brushes.Black, VisualTreeHelper.GetDpi(textblock).PixelsPerDip);

            return new Size(formattedText.Width, formattedText.Height);
        }

        #endregion
    }
}