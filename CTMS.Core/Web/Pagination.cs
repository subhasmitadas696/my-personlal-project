namespace CTMS.Core
{
    /// <summary>
    /// Paging information
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Number of lines per page
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// Current page
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// Sort column
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// SortType
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// total
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// total pages
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % this.rows == 0 ? records / this.rows : records / this.rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}

