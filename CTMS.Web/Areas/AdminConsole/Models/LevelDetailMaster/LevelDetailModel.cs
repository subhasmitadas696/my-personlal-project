using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;
using CTMS.Web.Areas.AdminConsole.Models.LevelMaster;

namespace CTMS.Web.Areas.AdminConsole.Models.LevelDetailMaster
{
    public class LevelDetailMaster
    {
        public string INTLEVELDETAILID { get; set; }
        public int INTHIERARCHYID { get; set; }
        public string NVCHLEVELNAME { get; set; }

        //public string 
        public int INTLEVELID { get; set; }
        public int INTPARENTID { get; set; }
        public string VCHALIAS { get; set; }
        public string VCHTELNO { get; set; }
        public string VCHFAXNO { get; set; }
        public string Office { get; set; }
        public string LineDept { get; set; }
        public string Dept { get; set; }

        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }

        public int INTPARENTID_block { get; set; }
        public int INTPARENTID_gp { get; set; }
        public int INTPARENTID_level5 { get; set; }
        public int ParentPosition { get; set; }

        public List<Hierarchy> HierarchyList { get; set; }
        public List<LevelModel> ParentLevelList { get; set; }
        public List<LevelDetails> ParentLevelDetailsList { get; set; }
        public List<LevelDetails> ParentLevelDetailsList_Edit { get; set; }
    }
    public class LevelDetails
    {
        public int INTLEVELDETAILID { get; set; }
        public string NVCHLEVELNAME { get; set; }
        public int INTLEVELID { get; set; }

        public string VCHALIAS { get; set; }
        public string NVCHLABEL { get; set; }
        public string NVCHHIERARCHYNAME { get; set; }
        public string parentlevelDetails { get; set; }
        public string INTPARENTID { get; set; }
        public string INTPOSITION { get; set; }
        public string ParentPosition { get; set; }
        public string Label2Text { get; set; }
        public string LabelText { get; set; }
    }
    public class LevelDetailsMasterModel
    {
        public List<Hierarchy> HierarchyList { get; set; }
        public List<LevelModel> ParentLevelList { get; set; }
        public List<LevelDetails> ParentLevelDetailsList { get; set; }
        public List<LevelDetails> LevelDetailsList { get; set; }
        public string INTLEVELDETAILID { get; set; }
        public int INTUPDATEDBY { get; set; }
    }
}
