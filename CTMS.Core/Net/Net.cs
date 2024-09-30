using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace CTMS.Core
{
    public class Net
    {
        #region Ip(Get Ip)
        /// <summary>
        /// Get Ip
        /// </summary>
        public static string Ip
        {
            get
            {
                var result = string.Empty;
                if (CoreContextProvider.HttpContext != null)
                    result = GetWebClientIp();
                if (result.IsEmpty())
                    result = GetLanIp();
                return result;
            }
        }

        /// <summary>
        /// Get Web client Ip
        /// </summary>
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIP();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Get Client IP
        /// </summary>
        private static string GetWebRemoteIP()
        {
            //https://www.cnblogs.com/dudu/p/5972649.html
            //ServerVariables ==>Headers
            return CoreContextProvider.HttpContext.Request.Headers["X-Forwarded-For"].ToString() ??
                CoreContextProvider.HttpContext.Connection.RemoteIpAddress?.ToString();
            //https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
        }

        /// <summary>
        /// Get LAN IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region Host(Get CPU name)

        /// <summary>
        /// Get CPU name
        /// </summary>
        public static string Host
        {
            get
            {
                return CoreContextProvider.HttpContext == null ? Dns.GetHostName() : GetWebClientHostName();
            }
        }

        /// <summary>
        /// Get Web client host name
        /// </summary>
        private static string GetWebClientHostName()
        {
            if (!CoreContextProvider.HttpContext.Request.IsLocal())
                return string.Empty;
            var ip = GetWebRemoteIP();
            var result = Dns.GetHostEntry(IPAddress.Parse(ip)).HostName;
            if (result == "localhost.localdomain")
                result = Dns.GetHostName();
            return result;
        }

        #endregion

        #region Get mac Address 
        /// <summary>
        /// Returns the object of the network interface on the Description local computer 
        /// (the network interface is also known as the network adapter).
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }
        ///<summary>
        /// Read the network card Mac through NetworkInterface
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }
        #endregion

        #region Ip City (Get Ip City)
        /// <summary>
        /// Get IP Address information
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetLocation(string ip)
        {
            string res = "";
            try
            {
                string url = "http://apis.juhe.cn/ip/ip2addr?ip=" + ip + "&dtype=json&key=b39857e36bee7a305d55cdb113a9d725";
                res = HttpMethods.HttpGet(url);
                var resjson = res.ToObject<objex>();
                res = resjson.result.area + " " + resjson.result.location;
            }
            catch
            {
                res = "";
            }
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            try
            {
                string url = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=" + ip + "&resource_id=6006&ie=utf8&oe=gbk&format=json";
                res = HttpMethods.HttpGet(url, Encoding.GetEncoding("GBK"));
                var resjson = res.ToObject<obj>();
                res = resjson.data[0].location;
            }
            catch
            {
                res = "";
            }
            return res;
        }
        /// <summary>
        /// Baidu interface
        /// </summary>
        public class obj
        {
            public List<dataone> data { get; set; }
        }
        public class dataone
        {
            public string location { get; set; }
        }
        /// <summary>
        /// Aggregate data
        /// </summary>
        public class objex
        {
            public string resultcode { get; set; }
            public dataoneex result { get; set; }
            public string reason { get; set; }
            public string error_code { get; set; }
        }
        public class dataoneex
        {
            public string area { get; set; }
            public string location { get; set; }
        }
        #endregion

        #region Browser(Get Browser information)
        /// <summary>
        /// Get Browser information
        /// </summary>
        public static string Browser
        {
            get
            {
                //if (CoreContextProvider.HttpContext == null)
                //    return string.Empty;
                //var browser = CoreContextProvider.HttpContext.Request.Headers;
                //return string.Format("{0} {1}", browser.Browser, browser.Version);
                return "";
            }
        }
        #endregion
    }
}
