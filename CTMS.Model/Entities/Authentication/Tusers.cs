namespace CTMS.Model.Tusers
{
    public class Tusers
    {
        public long UID { get; set; }
        public string? FULLNAME { get; set; }
        public string? UEMAILID { get; set; }
        public string? MOBILENO { get; set; }
        public string? MOBILENOWT { get; set; }
        public short? USERTYPE { get; set; }
        public long USERID { get; set; }
        public string? USERNAME { get; set; }
        public string? UPASSWORD { get; set; }
        public short USERACTIVESTATUS { get; set; }
        public bool? LOCKOUTENABLED { get; set; }
        public DateTime? LOCKOUTDATETIME { get; set; }
        public byte? ACCESSFAILEDCOUNT { get; set; }
        public bool ISRESETPWD { get; set; }
        public long? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public long? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPCREATEDON { get; set; }
        public string? URNAME { get; set; }
        public string? URDESG { get; set; }
        public string? URTYPE { get; set; }
        public short? LOGINATTEMPS { get; set; }
        public short? RESENDOTPATTEMPS { get; set; }
    }
}

