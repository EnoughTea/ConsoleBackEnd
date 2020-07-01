using System;
using System.Linq;

namespace ConsoleBackEnd.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="Type" />.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     Unwraps generic type names like "IList`1" into actual names like "IList&lt;String&gt;"
        /// </summary>
        /// <param name="type">Any type.</param>
        /// <returns>Unwrapped type name.</returns>
        public static string UnwrappedName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            string friendlyName = type.Name;
            string genericArrayPart = "";

            if (type.IsGenericType || type.IsArray) {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0) {
                    int iGenericArray = friendlyName.IndexOf('[', iBacktick);
                    if (iGenericArray >= 0) {
                        genericArrayPart = friendlyName.Substring(iGenericArray);
                    }

                    friendlyName = friendlyName.Remove(iBacktick);
                }

                var genericArgs = type.GetGenericArguments();
                if (!type.IsArray || genericArgs.Length != 0)
                    friendlyName += $"<{string.Join(",", type.GetGenericArguments().Select(UnwrappedName))}>";
            }

            return friendlyName + genericArrayPart;
        }
    }
}