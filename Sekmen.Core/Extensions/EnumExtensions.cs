using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sekmen.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets all items for an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllItems<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// https://codereview.stackexchange.com/questions/5352/getting-the-value-of-a-custom-attribute-from-an-enum
        /// these methods will give you the attribute value for a given value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute</typeparam>
        /// <param name="value">Enum</param>
        /// <returns>Attribute</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            return type.GetField(name).GetCustomAttribute<TAttribute>();
        }
    }
}
