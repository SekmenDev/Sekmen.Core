using System;
using System.Globalization;
using System.Linq;

namespace Sekmen.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns true if null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value == DBNull.Value)
            {
                return true;
            }

            return value.ToString().Trim() == string.Empty;
        }

        /// <summary>
        /// Converts objects to boolean
        /// </summary>
        public static bool ToBool(this object value)
        {
            try
            {
                string str = value.ToString();
                return str.Equals("1") || bool.Parse(str);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// convert to decimal
        /// </summary>
        public static decimal ToDecimal(this object value, decimal defaultValue = 0.0M)
        {
            try
            {
                return Convert.ToDecimal(value, value.ToString().Contains(".") ? CultureInfo.GetCultureInfo("en-GB") : CultureInfo.GetCultureInfo("tr-TR"));
            }
            catch { return defaultValue; }
        }

        /// <summary>
        /// Convert to integer
        /// </summary>
        public static int ToInteger(this object value, int defaultValue = 0)
        {
            try { return Convert.ToInt32(value); }
            catch { return defaultValue; }
        }

        /// <summary>
        /// Convert to long
        /// </summary>
        public static long ToLong(this object value, long defaultValue = 0)
        {
            try { return Convert.ToInt64(value); }
            catch { return defaultValue; }
        }

        /// <summary>
        /// https://github.com/evoleap/EPPlus/blob/fa5c0622aa43b01d8f694fbd7850af7e2b7b9d36/EPPlus/FormulaParsing/Utilities/ExtensionMethods.cs
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumeric(this object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (obj.GetType().IsPrimitive || obj is double || obj is decimal || obj is DateTime || obj is TimeSpan || obj is string && obj.ToString().All(char.IsDigit));
        }
    }
}
