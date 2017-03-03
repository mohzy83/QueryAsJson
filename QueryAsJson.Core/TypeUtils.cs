using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QueryAsJson.Core
{
    /// <summary>
    /// Type Utils
    /// </summary>
    public static class TypeUtils
    {
#if NET_4_0
        /// <summary>
        /// Get the type info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeInfoCompat(this Type type)
        {
            return type;
        }
#else
        /// <summary>
        /// Get the type info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeInfo GetTypeInfoCompat(this Type type)
        {
            return type.GetTypeInfo();
        }
#endif
    }
}
