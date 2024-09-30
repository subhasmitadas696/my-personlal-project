using Microsoft.AspNetCore.Http;
using System.Text;

namespace CTMS.Core
{
    public static class WebHelper
    {
        #region CookieOperation 
        //https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">Name</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            CoreContextProvider.HttpContext.Response.Cookies.Append(strName, strValue);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="key">Name</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期 Time.(分钟)</param>
        public static void WriteCookie(string key, string value, int expires)
        {
            
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(expires);
            CoreContextProvider.HttpContext.Response.Cookies.Append(key, value, options);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">Name</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (CoreContextProvider.HttpContext.Request.Cookies != null &&
                CoreContextProvider.HttpContext.Request.Cookies[strName] != null)
            {
                return CoreContextProvider.HttpContext.Request.Cookies[strName].ToString();
            }
            return "";
        }
        /// <summary>
        /// Delete Cookie对象
        /// </summary>
        /// <param name="key">Cookie对象Name</param>
        public static void RemoveCookie(string key)
        {
            CoreContextProvider.HttpContext.Response.Cookies.Delete(key);
        }
        #endregion


        #region SessionOperation 
        /// <summary>
        /// Write Session
        /// </summary>
        /// <typeparam name="T">Session key type</typeparam>
        /// <param name="key">Session key name</param>
        /// <param name="value">Session key value</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (key.IsEmpty())
                return;
            CoreContextProvider.HttpContext.Session.SetString(key, value.ToString());
        }

        /// <summary>
        /// Write Session
        /// </summary>
        /// <param name="key"Session key name></param>
        /// <param name="value">Session key value</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// Read the value of the Session
        /// </summary>
        /// <param name="key">Session key name</param>        
        public static string GetSession(string key)
        {
            if (key.IsEmpty())
                return string.Empty;
            return CoreContextProvider.HttpContext.Session.GetString(key);
        }
        /// <summary>
        /// Delete Session specified
        /// </summary>
        /// <param name="key">Session key name</param>
        public static void RemoveSession(string key)
        {
            if (key.IsEmpty())
                return;
            CoreContextProvider.HttpContext.Session.Remove(key);
        }

        #endregion
    }
}

