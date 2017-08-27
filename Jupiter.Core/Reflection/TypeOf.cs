using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jupiter.Reflection
{
    /// <summary>
    /// Helper for accessing the <see cref="Type"/> of a generic type.
    /// Using this helper reduces the time for the typeof operator on some systems by half.
    /// </summary>
    /// <typeparam name="T">The type to get the <see cref="Type"/> for.</typeparam>
    static class TypeOf<T>
    {
        /// <summary>
        /// The <see cref="Type"/> which represents the generic parameter <see cref="T"/>.
        /// </summary>
        public static readonly Type Type = typeof(T);
        /// <summary>
        /// The <see cref="TypeInfo"/> which represents the generic parameter <see cref="T"/>.
        /// </summary>
        public static readonly TypeInfo TypeInfo = Type.GetTypeInfo();
    }
}
