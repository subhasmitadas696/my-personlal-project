namespace CTMS.Model.BlockMaster
{
    public class BlockMaster
    {
        public long BLOCKID { get; set; }
        public int BLOCKCODE { get; set; }
        public string? BLOCKNAME { get; set; }
        public short DISTCODE { get; set; }
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
        public int? EXPECTEDSURVEYCENTER { get; set; }
        public string? BLOCKNAMEOD { get; set; }
    }
}

