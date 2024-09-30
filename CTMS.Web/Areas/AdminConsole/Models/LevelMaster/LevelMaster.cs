using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;

namespace CTMS.Web.Areas.AdminConsole.Models.LevelMaster
{
    public class LevelModel
    {
        public string INTLEVELID { get; set; }
        public int INTHIERARCHYID { get; set; }
        public string NVCHLABEL { get; set; }
        public string VCHALLIAS { get; set; }
        public int INTPARENTID { get; set; }
        public string VCHHIERARCHYALIAS { get; set; }
        public string NVCHHIERARCHYNAME { get; set; }
        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }
        public string parentlevel { get; set; }
    }
    public class CreateLevelMaster
    {
        public string INTLEVELID { get; set; }
        public int INTHIERARCHYID { get; set; }
        public string NVCHLABEL { get; set; }
        public string VCHALLIAS { get; set; }
        public int INTPARENTID { get; set; }
        public string VCHHIERARCHYALIAS { get; set; }
        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }
        public List<Hierarchy> HierarchyList { get; set; }
        public List<LevelModel> ParentLevelList { get; set; }
    }
    public class LevelMasterModel
    {
        public List<LevelModel> ParentLevelList { get; set; }
        public List<Hierarchy> HierarchyList { get; set; }
        public List<LevelModel> LevelList { get; set; }
        public string INTLEVELID { get; set; }
        public int INTUPDATEDBY { get; set; }
    }
}
