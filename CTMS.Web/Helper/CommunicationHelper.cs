using CTMS.Core;
using CTMS.Model.Entities.Common;
using CTMS.Model.Entities.Communication;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CTMS.Web.Helper
{
    public class CommunicationHelper
    {
        protected CommunicationHelper()
        {
        }

        protected static String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512")!;
            byte[] pp = sha1.ComputeHash(encPwd);
            System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512")!;
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }


        public static async Task<string> SendMobileSMSNewGateway(string mobile, string context, string senderId, string templateId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(3);

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string url = ApiUrls.otherSmsGateway;
            string smsserviceType = "singlemsg";
            string action = "sendOTPSMS";
            string departmentId = ApiUrls.NewSmsGateWayDeptId;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action", action),
                new KeyValuePair<string, string>("department_id", departmentId),
                new KeyValuePair<string, string>("smsservicetype", smsserviceType),
                new KeyValuePair<string, string>("sms_content", context),
                new KeyValuePair<string, string>("phonenumber", mobile),
                new KeyValuePair<string, string>("source", senderId),
                new KeyValuePair<string, string>("template_id", templateId)
            });

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseFromServer = await response.Content.ReadAsStringAsync();
                return responseFromServer;
            }
            else
            {
                return "HTTP request failed with status code" + response.StatusCode;
            }
        }

        public static async Task<bool> SendEmail(string host, string dispName, string username, string password, int port, EmailMessage mailData)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(mailData.ToEmail!);
            mail.From = new MailAddress(username, dispName);
            mail.Subject = mailData.Subject;
            mail.Body = mailData.Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(host, port);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(username, password);
            await smtp.SendMailAsync(mail);
            return true;
        }
        public static async Task<bool> SendEmailWithAttachment(string host, string dispName, string username, string password, int port, EmailMessage mailData, string pdfFilePath)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(mailData.ToEmail!);
            mail.From = new MailAddress(username, dispName);
            mail.Subject = mailData.Subject;
            mail.Body = mailData.Body;
            mail.IsBodyHtml = true;
            Attachment pdfAttachment = new Attachment(pdfFilePath, MediaTypeNames.Application.Pdf);
            mail.Attachments.Add(pdfAttachment);
            using (pdfAttachment)
            {
                SmtpClient smtp = new SmtpClient(host, port);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
                await smtp.SendMailAsync(mail);
            }
            return true;
        }

        public static async Task<WhatsAppResponseNew> SendWhatsAppSms(string smsTemplate, string mobno, string smstext)
        {
            var apiEndpoint =ApiUrls.OdishaGovtWhatsAppApi;
            var phone = "+91" + mobno;
            var templateName = smsTemplate;
            var bodyText = smstext;

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(3);
            var formData = new MultipartFormDataContent
            {
                { new StringContent("send_template_sms"), "action" },
                { new StringContent(phone), "phone" },
                { new StringContent(templateName), "template_name" },
                { new StringContent(bodyText), "body_text" }
            };

            var response = await httpClient.PostAsync(apiEndpoint, formData);

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<WhatsAppResponseNew>(responseContent);
            return jsonResponse!;
        }
    }
}
