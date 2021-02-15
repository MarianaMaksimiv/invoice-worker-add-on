using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fintech.InvoiceWorker.Business.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Extension method to return an enum value of type T for the given string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string value) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value)) return null;

            if (Enum.TryParse<T>(value, out T enumValue))
            {
                return enumValue;
            }

            return null;
        }
    }
}
