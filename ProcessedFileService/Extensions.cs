using System;
using System.ComponentModel;

namespace ProcessedFileService
{
    public static class Extensions
    {
        /// <summary>
        ///		Returns an enum of the passed type based upon the passed value
        /// </summary>
        /// <typeparam name="T">The enum type to get</typeparam>
        /// <param name="value">The string to convert to the enum</param>
        /// <returns>
        ///		An enum of the passed type based upon the passed value
        /// </returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse(value, out T enumValue))
            {
                return enumValue;
            }

            throw new ArgumentException(@"Invalid enum value specified", nameof(value));
        }

        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }
    }
}
