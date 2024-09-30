namespace CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster
{
    public class Hierarchy
    {
        public int INTHIERARCHYID { get; set; }
        public int ROLEID { get; set; }
        public string NVCHHIERARCHYNAME { get; set; }
        public string ROLENAME { get; set; }
        public string VCHHIERARCHYALIAS { get; set; }
        public int INTNOLEVEL { get; set; }
        public int LEVELID { get; set; }
        public string NVCHADDRESS { get; set; }
        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }
    }
    public class CreateHierarchy
    {

        public string NVCHHIERARCHYNAME { get; set; }
        public string VCHHIERARCHYALIAS { get; set; }
        public int INTNOLEVEL { get; set; }
        public string NVCHADDRESS { get; set; }
        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }

    }
    public class HierarchyModel
    {
        public List<Hierarchy> HierarchyList { get; set; }
    }
}
