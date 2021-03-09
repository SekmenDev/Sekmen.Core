using System.Collections.Generic;
using System.Linq;

namespace Sekmen.Core.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// use it to simplify the Join method for IEnumerable and Arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> self, string separator)
        {
            return string.Join(separator, self.Select(e => e.ToString()).ToArray());
        }

        /// <summary>
        /// converts byte[] to string
        /// </summary>
        public static string ByteToString(this byte[] buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }
    }
}
