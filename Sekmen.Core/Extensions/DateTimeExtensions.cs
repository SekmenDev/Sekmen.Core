using System;
using System.Globalization;

namespace Sekmen.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts Date to string format
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <param name="format">string</param>
        /// <returns>string</returns>
        public static string Format(this DateTime value, string format = "dd.MM.yyyy")
        {
            return value == new DateTime() ? "" : value.ToString(format);
        }

        public static string Format(this DateTime? value, string format = "dd.MM.yyyy")
        {
            return value == null ? "" : Format(value.Value, format);
        }

        /// <summary>
        /// Converts DateTime to string format
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <param name="format">string</param>
        /// <returns>string</returns>
        public static string FormatWithTime(this DateTime value, string format = "dd.MM.yyyy HH:mm:ss")
        {
            return value == new DateTime() ? "" : value.ToString(format);
        }

        public static string FormatWithTime(this DateTime? value, string format = "dd.MM.yyyy HH:mm:ss")
        {
            return value == null ? "" : FormatWithTime(value.Value, format);
        }

        /// <summary>
        /// Converts DateTime format to Universal format: 2018-12-31
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>string</returns>
        public static string FormatUniversal(this DateTime value)
        {
            return value == new DateTime() ? "" : $"{value.Year}-{value.Month:00}-{value.Day:00}";
        }

        public static string FormatUniversal(this DateTime? value)
        {
            return value == null ? "" : FormatUniversal(value.Value);
        }

        /// <summary>
        /// Converts DateTime format to Universal format: 2018-12-31 23:59:59
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>string</returns>
        public static string FormatUniversalWithTime(this DateTime value)
        {
            return value == new DateTime() ? "" : $"{value.Year}-{value.Month:00}-{value.Day:00} {value.Hour:00}:{value.Minute:00}:{value.Second:00}";
        }

        public static string FormatUniversalWithTime(this DateTime? value)
        {
            return value == null ? "" : FormatUniversalWithTime(value.Value);
        }

        /// <summary>
        /// Gets a nullable DateTime object from a string input. Good for grabbing date times from user inputs, like textboxes and query strings.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return new DateTime();
            }

            DateTimeFormatInfo dateTimeFormat = new CultureInfo(input.Contains("AM") || input.Contains("PM") ? "en-US" : input.Contains("/") ? "en-GB" : "tr-TR", true).DateTimeFormat;
            bool tryOut = DateTime.TryParse(input.Trim(), dateTimeFormat, DateTimeStyles.None, out DateTime _);
            if (!tryOut)
            {
                return new DateTime();
            }

            try
            {
                DateTime result = Convert.ToDateTime(input, dateTimeFormat);
                return result;
            }
            catch
            {
                return new DateTime();
            }
        }

        public static DateTime ToDateTime(this object input)
        {
            return input.ToString().ToDateTime();
        }
    }
}
