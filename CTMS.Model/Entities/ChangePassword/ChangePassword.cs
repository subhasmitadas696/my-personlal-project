using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.ChangePassword
{
    public class ChangePassword
    {
        public long ID { get; set; }
        public long USERID { get; set; }
        public string? USERNAME { get; set; }
        public string? OLDPASSWORD { get; set; }
        public string? NEWPASSWORD { get; set; }
        public long? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
    }
    public class ForgotPassword
    {
        public long ID { get; set; }
        public long USERID { get; set; }
        public string? USERNAME { get; set; }
        public string? NEWPASSWORD { get; set; }
        public long? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }



    }
    public class OtpTracker
    {
        public int ID { get; set; }
        public int USERID { get; set; }
        public string? USERNAME { get; set; }
        public string? OTP { get;set; }
        public string? MOBILENO { get;set; }
        public long? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }

    }
}