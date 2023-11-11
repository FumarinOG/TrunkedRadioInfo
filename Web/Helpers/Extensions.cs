using System;
using System.Linq;

namespace Web.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool In(this string me, params string[] set)
        {
            return set.Contains(me, StringComparer.OrdinalIgnoreCase);
        }
    }
}