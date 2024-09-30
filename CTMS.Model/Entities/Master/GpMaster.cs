namespace CTMS.Model.GpMaster
{
    public class GpMaster
    {
        public long GPID { get; set; }
        public int? GPCODE { get; set; }
        public string? GPNAME { get; set; }
        public long BLOCKCODE { get; set; }
        public byte? LGDTYPE { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
        public string? GPNAMEOD { get; set; }
    }
}

