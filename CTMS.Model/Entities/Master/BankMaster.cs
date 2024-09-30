namespace CTMS.Model.M_BANK
{
    public class BankMaster
    {
        public int BANKID { get; set; }
        public string? BANKNAME { get; set; }
        public short? BANKTYPE { get; set; }
        public short? MINACCLEN { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
        public short? MAXACCLEN { get; set; }
    }
}

