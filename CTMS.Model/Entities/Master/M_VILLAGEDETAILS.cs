namespace CTMS.Model.VillageMaster
{
    public class VillageMaster
    {
        public int VLGID { get; set; }
        public string? VLGCODE { get; set; }
        public string? VLGNAME { get; set; }
        public int? GPCODE { get; set; }
        public int BLOCKCODE { get; set; }
        public short? DISTCODE { get; set; }
        public byte? STATECODE { get; set; }
        public string? STATENAME { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
        public int? TYPE { get; set; }
        public string? LGDVLGCODE { get; set; }
        public string? ISNEWLYADDED { get; set; }
        public string? VLGNAMEOD { get; set; }
    }
}

