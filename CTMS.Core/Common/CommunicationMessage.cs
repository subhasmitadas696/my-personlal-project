using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Core
{
    public class CommunicationMessage
    {
    }
    public static class ApiUrls
    {
        public const string
            odigov = "https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT",
            otherSmsGateway = "https://govtsms.odisha.gov.in/api/api.php",
            whatsAppApi = "https://apis.rmlconnect.net",
            whatsAppApiAuthToken = "https://apis.rmlconnect.net/auth/v1/login/",
            whatsAppApiAuthTokenbodyContent = "{\r\n  \"username\":\"OSCBC\",\r\n  \"password\":\"Obs@ac567\"\r\n}\r\n",
            NewSmsGateWayDeptId = "D009005",
            OdishaGovtWhatsAppApi = "https://msg.odisha.gov.in/api/api.php";
    }
}
