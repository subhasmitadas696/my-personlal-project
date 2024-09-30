using System.Text;

namespace CTMS.Core
{
    public static partial class Ext
    {
        /// <summary>
        /// Get Format string，Belt minutes, seconds, format："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <param name="isRemoveSecond">Yes.No.Remove seconds</param>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (isRemoveSecond)
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Get Format string，Belt minutes, seconds, format："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <param name="isRemoveSecond">Yes.No.Remove seconds</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        /// <summary>
        /// Get Format string，No. Belt minutes, seconds, format："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// Get Format string，No. Belt minutes, seconds, format："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToDateString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Get Format string，No. Belt minutes, seconds, format："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// Get Format string，No. Belt year Month Day，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Get Format string，No. Belt year Month Day，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// Get Format string，Belt milliseconds, format："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// Get Format string，Belt milliseconds, format："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// Get Format string，No. Belt minutes, seconds, format："yyyy＝mm＝dd"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}Year{1}month{2}day", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// Get Format string，No. Belt minutes, seconds, format："yyyy＝mm＝dd"
        /// </summary>
        /// <param name="dateTime">Date</param>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateString(dateTime.SafeValue());
        }

        /// <summary>
        /// Get Format string，Belt minutes, seconds, format："yyyy＝mm＝dd HH时mm分"
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <param name="isRemoveSecond">Yes.No.Remove seconds</param>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}Year{1}month{2}day", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat("{0}Hour{1}minutes", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
                result.AppendFormat("{0}seconds", dateTime.Second);
            return result.ToString();
        }

        /// <summary>
        /// Get Format string，Belt minutes, seconds, format："yyyy＝mm＝dd HH时mm分"
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <param name="isRemoveSecond">Yes.No.Remove seconds</param>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateTimeString(dateTime.Value);
        }
    }
}
