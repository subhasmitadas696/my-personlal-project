namespace CTMS.Core
{
    public class AjaxResult
    {
        /// <summary>
        /// Operation Result Type
        /// </summary>
        public object? state { get; set; }
        /// <summary>
        /// Get Message content
        /// </summary>
        public string? message { get; set; }
        public string? AcknowledgementNo { get; set; }
        /// <summary>
        /// Get Return data
        /// </summary>
        public object? data { get; set; }
        public object? data1 { get; set; }

        public string? starttime { get; set; }
        /// <summary>
        /// Returns id of control if any needs to be focussed
        /// </summary>
        public object? controlid { get; set; }

        public object status { get; set; }
        public string mobno { get; set; }
        public string emailid { get; set; }
    }
    /// <summary>
    /// An enumeration indicating the type of ajax Operation result
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// Message result Type
        /// </summary>
        info,
        /// <summary>
        /// Successful result Type
        /// </summary>
        success,
        /// <summary>
        /// Warning result Type
        /// </summary>
        warning,
        /// <summary>
        /// Abnormal result Type
        /// </summary>
        error
    }
    
}