namespace CTMS.Model.M_BRANCH
{
    public class BranchMaster
    {
        public int BRANCHID { get; set; }
        public string? BRANCHNAME { get; set; }
        public int BANKID { get; set; }
        public string? IFSC { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
        public bool? DELETEDFLAG { get; set; }
    }
}

