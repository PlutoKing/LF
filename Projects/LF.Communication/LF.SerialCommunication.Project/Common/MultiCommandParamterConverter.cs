/*──────────────────────────────────────────────────────────────
 * FileName     : MultiCommandParamterConverter.cs
 * Created      : 2021-06-23 22:13:42
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Xml;

namespace LF.SerialCommunication.Project
{
    /// <summary>
    /// 多个命令绑定参数转换器
    /// </summary>
    public class MultiCommandParamterConverter:IMultiValueConverter
    {
        #region Fields
        public static object ConverterObject;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        public object Convert(object[] values, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            ConverterObject = values;
            string str = values.GetType().ToString();
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
          object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}