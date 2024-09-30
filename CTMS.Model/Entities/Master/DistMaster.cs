namespace CTMS.Model.DistMaster
{
    public class DistMaster
    {
        public short DISTID { get; set; }
        public short? DISTCODE { get; set; }
        public string? DISTNAME { get; set; }
        public byte? LGDTYPE { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
        public int? EXPECTEDENUMERATOR { get; set; }
        public int? EXPECTEDSUPERVISOR { get; set; }
        public int? EXPECTEDRVENUMERATOR { get; set; }
        public int? EXPECTEDRVSUPERVISOR { get; set; }
        public int? MCEXPECTEDENUMERATOR { get; set; }
        public int? MCEXPECTEDSUPERVISOR { get; set; }
        public int? MCEXPECTEDRVENUMERATOR { get; set; }
        public int? MCEXPECTEDRVSUPERVISOR { get; set; }
        public int? EXPECTEDSURVEYCENTER { get; set; }
        public int? EXPECTEDMCSURVEYCENTER { get; set; }
        public string? DISTNAMEOD { get; set; }
    }
}

