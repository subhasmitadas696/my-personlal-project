using System.Collections;

namespace CTMS.Core
{
    public static class ExtList
    {
        /// <summary>
        /// Get the data for a page in the table
        /// </summary>
        /// <param name="data">Table data</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Pagination Size</param>
        /// <param name="allPage">Returns the total number of pages</param>
        /// <returns>Returns when the page table data</returns>
        public static List<T> GetPage<T>(this List<T> data, int pageIndex, int pageSize, out int allPage)
        {
            allPage = 1;
            return null;
        }
        /// <summary>
        /// IList to List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> IListToList<T>(IList list)
        {
            T[] array = new T[list.Count];
            list.CopyTo(array, 0);
            return new List<T>(array);
        }
    }
}
