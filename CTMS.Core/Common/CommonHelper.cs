using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CTMS.Core
{
    public static class CommonHelper
    {
        public enum EnRoles
        {
            SuperAdmin = 1,
            StateUser = 2,
            DistrictUser = 3,
            BlockUser = 4,
            TroupeArtist = 5,
            Administrator = 11,
            Support = 12
        }

        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            tw = new XmlTextWriter(sw);
            serializer.Serialize(tw, o);
            sw.Close();
            tw?.Close();
            return sw.ToString();
        }

        public static string Set_Paging(int PageNumber, int PageSize, long TotalRecords, string ClassName, string PageUrl, string DisableClassName)
        {
            StringBuilder ReturnValue = new StringBuilder();

            long TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                {
                    ReturnValue.Append("<a href='").Append(PageUrl.Trim()).Append("?pn=").Append(Convert.ToString(PageNumber - 1)).Append("' class='").Append(ClassName).Append("' >Previous</a>   ");
                }
                else
                {
                    ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                    if (PageUrl.Contains("?"))
                        ReturnValue.Append("&");
                    else
                        ReturnValue.Append("?");
                    ReturnValue.Append("pn=").Append(Convert.ToString(PageNumber - 1)).Append("' class='").Append(ClassName).Append("' >Previous</a>   ");
                }
            }
            else
            {
                ReturnValue.Append("<span class='").Append(DisableClassName).Append("'>Previous</span>   ");
            }

            if ((PageNumber - 3) > 1)
            {
                ReturnValue.Append("<a href='").Append(PageUrl.Trim()).Append("' class='").Append(ClassName).Append("' >1</a> ..... | ");
            }

            for (int i = PageNumber - 3; i <= PageNumber; i++)
            {
                if (i >= 1)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                        if (PageUrl.Contains("?"))
                            ReturnValue.Append("&");
                        else
                            ReturnValue.Append("?");
                        ReturnValue.Append("pn=").Append(i.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(i.ToString()).Append("</a> | ");
                    }
                    else
                    {
                        ReturnValue.Append("<span style='font-weight:bold;'>").Append(i).Append("</span> | ");
                    }
                }
            }

            for (int i = PageNumber + 1; i <= PageNumber + 3; i++)
            {
                if (i <= TotalPages)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                        if (PageUrl.Contains("?"))
                            ReturnValue.Append("&");
                        else
                            ReturnValue.Append("?");
                        ReturnValue.Append("pn=").Append(i.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(i.ToString()).Append("</a> | ");
                    }
                    else
                    {
                        ReturnValue.Append("<span style='font-weight:bold;'>").Append(i).Append("</span> | ");
                    }
                }
            }

            if ((PageNumber + 3) < TotalPages)
            {
                ReturnValue.Append("..... <a href='").Append(PageUrl.Trim());
                if (PageUrl.Contains("?"))
                    ReturnValue.Append("&");
                else
                    ReturnValue.Append("?");
                ReturnValue.Append("pn=").Append(TotalPages.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(TotalPages.ToString()).Append("</a>");
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue.Append("   <a href='").Append(PageUrl.Trim());
                if (PageUrl.Contains("?"))
                    ReturnValue.Append("&");
                else
                    ReturnValue.Append("?");
                ReturnValue.Append("pn=").Append(Convert.ToString(PageNumber + 1)).Append("' class='").Append(ClassName).Append("' >Next</a>");
            }
            else
            {
                ReturnValue.Append("   <span class='").Append(DisableClassName).Append("'>Next</span>");
            }

            return ReturnValue.ToString();
        }



        /// <summary>
        /// Function to check if the file mime type is same as per the extensions allowed in the page
        /// </summary>
        /// <param name="file">The file object that is uploaded by user</param>
        /// <param name="arrAllowedExtension">types of extensions allowed in the page</param>
        /// <returns>Boolean as in true and false</returns>
        public static bool IsFileValid(IFormFile file, string[] arrAllowedExtension)
        {
            string[] arrAllowedMime = new string[arrAllowedExtension.Length];
            for (int cnt = 0; cnt < arrAllowedExtension.Length; cnt++)
            {
                arrAllowedMime[cnt] = GetMimeTypeByFileExtension(arrAllowedExtension[cnt]);
            }
            StringCollection imageTypes = new StringCollection();
            StringCollection imageExtension = new StringCollection();
            imageTypes.AddRange(arrAllowedMime);
            imageExtension.AddRange(arrAllowedExtension);
            string filename = file.FileName;

            //to calculate dots
            int count = filename.Count(f => f == '.');
            string strFiletype = file.ContentType;

            string fileExt = Path.GetExtension(Path.GetExtension(file.FileName).ToLower());// 

            return imageTypes.Contains(strFiletype) && imageExtension.Contains(fileExt) && count == 1;
        }

        /// <summary>
        /// Check if the file name is containing any dots
        /// </summary>
        ///<param name="strFilename">the actual name of file provided by the user</param>
        /// <returns>boolean value</returns>
        public static bool isFilNameHavingDots(string strFilename)
        {
            //to calculate dots
            int count = strFilename.Count(f => f == '.');
            return count <= 0;
        }

        /// <summary>
        /// Function to get the mime type of file by extension
        /// </summary>
        /// <param name="strExtension">File extension with .</param>
        /// <returns>mime type with string format</returns>
        public static string GetMimeTypeByFileExtension(string strExtension)
        {
            string strMimeType = string.Empty;
            switch (strExtension.ToUpper())
            {
                case ".PDF":
                    strMimeType = "application/pdf";
                    break;
                case ".PNG":
                    strMimeType = "image/png";
                    break;
                case ".JPG":
                case ".JPEG":
                    strMimeType = "image/jpeg";
                    break;
                case ".ZIP":
                    strMimeType = "application/x-zip-compressed";
                    break;
                case ".RAR":
                    strMimeType = "application/x-rar-compressed";
                    break;
                case ".DOC":
                    strMimeType = "application/msword";
                    break;
                case ".DOCX":
                    strMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".XLSX":
                    strMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".XLS":
                    strMimeType = "application/vnd.ms-excel";
                    break;
                case ".TXT":
                    strMimeType = "text/plain";
                    break;
                case ".GIF":
                    strMimeType = "image/gif";
                    break;
                case ".CSV":
                    strMimeType = "text/csv";
                    break;
                case ".LOG":
                    strMimeType = "text/plain";
                    break;
                case ".TIF":
                case ".TIFF":
                    strMimeType = "image/tiff";
                    break;
                case ".JSON":
                    strMimeType = "text/plain";
                    break;
            }
            return strMimeType;
        }

        public static void LogError(Exception ex, string strModule, string path)
        {
            string strFileName = "ErrorLog" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            path += "/ErrorLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }

        public static void LogOOAPIResponse(string content, string path)
        {
            string strFileName = "OOAPI" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            message += string.Format("Message: {0}", content);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            path += "/ErrorLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }

        public static async Task<string> SendMobileSMSOld(string mobile, string message, string Password, string userdetails, string OPTINS, string type, string template_id)
        {
            try
            {


                string strUrl = "https://login.bulksmsgateway.in/sendmessage.php?&user=" + userdetails + "&password=" + Password + "&mobile=" + mobile + "&message=" + message + "&sender=" + OPTINS + "&type=" + type + "&template_id=" + template_id;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.WebRequest request = System.Net.WebRequest.Create(strUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                s.Close();
                readStream.Close();
                response.Close();
                return strUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static async Task<string> ReSendMobileSMSOld(string mobile, string message, string Password, string userdetails, string OPTINS, string type, string template_id)
        {
            try
            {


                string strUrl = "https://login.bulksmsgateway.in/sendmessage.php?&user=" + userdetails + "&password=" + Password + "&mobile=" + mobile + "&message=" + message + "&sender=" + OPTINS + "&type=" + type + "&template_id=" + template_id;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.WebRequest request = System.Net.WebRequest.Create(strUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                s.Close();
                readStream.Close();
                response.Close();
                return strUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> SendMobileSMS(string Mobile, string context, string strPassword, string strusername, string senderid, string SecureKey, string template_id)
        {
            try
            {
                //Latest Generated Secure Key
                Stream dataStream;
                //note comment by NSA -SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 not compatable with .NET 4.0
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");

                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                String encryptedPassword = encryptedPasswod(strPassword);
                String NewsecureKey = hashGenerator(strusername.Trim(), senderid.Trim(), context, SecureKey.Trim());
                String smsservicetype = "singlemsg"; //For single message.
                String query = "username=" + HttpUtility.UrlEncode(strusername.Trim()) +
                    "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                    "&content=" + HttpUtility.UrlEncode(context) +
                    "&mobileno=" + HttpUtility.UrlEncode(Mobile.Trim()) +
                    "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                  "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                  "&templateid=" + HttpUtility.UrlEncode(template_id);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static async Task<string> ReSendMobileSMS(string Mobile, string context, string strPassword, string strusername, string senderid, string SecureKey, string template_id)
        {
            try
            {
                //Latest Generated Secure Key
                Stream dataStream;
                //note comment by NSA -SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 not compatable with .NET 4.0
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");

                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                String encryptedPassword = encryptedPasswod(strPassword);
                String NewsecureKey = hashGenerator(strusername.Trim(), senderid.Trim(), context, SecureKey.Trim());
                String smsservicetype = "singlemsg"; //For single message.
                String query = "username=" + HttpUtility.UrlEncode(strusername.Trim()) +
                    "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                    "&content=" + HttpUtility.UrlEncode(context) +
                    "&mobileno=" + HttpUtility.UrlEncode(Mobile.Trim()) +
                    "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                  "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                  "&templateid=" + HttpUtility.UrlEncode(template_id);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1")!;
            byte[] pp = sha1.ComputeHash(encPwd);
            System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        public static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
        public static string SerializeToXMLString<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
        public static string GenerateCronExpression(string Minute, string Hour, string Day)
        {
            // Format: "ss mm HH dd MM ? yyyy"
            // "ss" and "mm" are set to "00" as we only need to specify the hour, day, and month
            if (Hour == "0" && Day == "0")
            {
                return $"*/{Minute} * * * *";
            }
            if (Day == "0")
            {
                return $"{Minute} */{Hour} * * *";
            }
            return $"{Minute} {Hour} */{Day} * *";
        }
    }
}
