namespace CTMS.Core
{
    public static partial class Ext
    {

        #region Value conversion
        /// <summary>
        /// Convert to integer
        /// </summary>
        /// <param name="data">Data</param>
        public static int ToInt(this object data)
        {
            if (data == null)
                return 0;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert to nullable integer
        /// </summary>
        /// <param name="data">Data</param>
        public static int? ToIntOrNull(this object data)
        {
            if (data == null)
                return null;
            int result;
            bool isValid = int.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// Convert to a double-precision floating-point number
        /// </summary>
        /// <param name="data">Data</param>
        public static double ToDouble(this object data)
        {
            if (data == null)
                return 0;
            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// Convert to a double-precision floating-point number,And rounding 5 by the specified decimal place of 4
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="digits">Number of decimal places</param>
        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }

        /// <summary>
        /// Convert to an empty double-precision floating-point number
        /// </summary>
        /// <param name="data">Data</param>
        public static double? ToDoubleOrNull(this object data)
        {
            if (data == null)
                return null;
            double result;
            bool isValid = double.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// Convert to high-precision floating-point numbers
        /// </summary>
        /// <param name="data">Data</param>
        public static decimal ToDecimal(this object data)
        {
            if (data == null)
                return 0;
            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// Convert to high-precision floating-point numbers,And rounding 5 by the specified decimal place of 4
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="digits">Number of decimal places</param>
        public static decimal ToDecimal(this object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }

        /// <summary>
        /// Convert to a nullable high-precision floating-point number
        /// </summary>
        /// <param name="data">Data</param>
        public static decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
                return null;
            decimal result;
            bool isValid = decimal.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// Convert to a nullable high-precision floating-point number,And rounding 5 by the specified decimal place of 4
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="digits">Number of decimal places</param>
        public static decimal? ToDecimalOrNull(this object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
                return null;
            return Math.Round(result.Value, digits);
        }

        #endregion

        #region Date conversion
        /// <summary>
        /// Convert to Date
        /// </summary>
        /// <param name="data">Data</param>
        public static DateTime ToDate(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// Convert to nullable Date
        /// </summary>
        /// <param name="data">Data</param>
        public static DateTime? ToDateOrNull(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region Boolean conversion
        /// <summary>
        /// Convert to Boolean value
        /// </summary>
        /// <param name="data">Data</param>
        public static bool ToBool(this object data)
        {
            if (data == null)
                return false;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        private static bool? GetBool(this object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "Yes.":
                    return true;
                case "No.":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert to nullable Boolean value
        /// </summary>
        /// <param name="data">Data</param>
        public static bool? ToBoolOrNull(this object data)
        {
            if (data == null)
                return null;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            bool isValid = bool.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region String conversion
        /// <summary>
        /// Convert to string
        /// </summary>
        /// <param name="data">Data</param>
        public static string ToString(this object data)
        {
            return data == null ? string.Empty : data.ToString().Trim();
        }
        #endregion

        /// <summary>
        /// Safe return value
        /// </summary>
        /// <param name="value">Nullable value</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }
        /// <summary>
        /// Yes.No.Is empty
        /// </summary>
        /// <param name="value">value</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// Yes.No.Is empty
        /// </summary>
        /// <param name="value">value</param>
        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
                return true;
            return IsEmpty(value.Value);
        }
        /// <summary>
        /// Yes.No.Is empty
        /// </summary>
        /// <param name="value">value</param>
        public static bool IsEmpty(this Guid value)
        {
            if (value == Guid.Empty)
                return true;
            return false;
        }
        /// <summary>
        /// Yes.No.Is empty
        /// </summary>
        /// <param name="value">value</param>
        public static bool IsEmpty(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
