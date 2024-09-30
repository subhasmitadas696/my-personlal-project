namespace CTMS.Core
{
    public static partial class Ext
    {
        /// <summary>
        /// Get Description
        /// </summary>
        /// <param name="value">Boolean value</param>
        public static string Description(this bool value)
        {
            return value ? "Yes." : "No.";
        }
        /// <summary>
        /// Get Description
        /// </summary>
        /// <param name="value">Boolean value</param>
        public static string Description(this bool? value)
        {
            return value == null ? "" : Description(value.Value);
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this int number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return number.ToString();
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this int? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this decimal number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return string.Format("{0:0.##}", number);
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this decimal? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this double number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return string.Format("{0:0.##}", number);
        }
        /// <summary>
        /// Get Format string
        /// </summary>
        /// <param name="number">Value</param>
        /// <param name="defaultValue">Default text displayed with a NULL value</param>
        public static string Format(this double? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// Get Format string,With￥
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatRmb(this decimal number)
        {
            if (number == 0)
                return "￥0";
            return string.Format("￥{0:0.##}", number);
        }
        /// <summary>
        /// Get Format string,With￥
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatRmb(this decimal? number)
        {
            return FormatRmb(number.SafeValue());
        }
        /// <summary>
        /// Get Format string,Belt%
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatPercent(this decimal number)
        {
            if (number == 0)
                return string.Empty;
            return string.Format("{0:0.##}%", number);
        }
        /// <summary>
        /// Get Format string,Belt%
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatPercent(this decimal? number)
        {
            return FormatPercent(number.SafeValue());
        }
        /// <summary>
        /// Get Format string,Belt%
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatPercent(this double number)
        {
            if (number == 0)
                return string.Empty;
            return string.Format("{0:0.##}%", number);
        }
        /// <summary>
        /// Get Format string,Belt%
        /// </summary>
        /// <param name="number">Value</param>
        public static string FormatPercent(this double? number)
        {
            return FormatPercent(number.SafeValue());
        }
    }
}
