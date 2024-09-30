namespace CTMS.Web.Areas.AdminConsole.Models.DesignationMaster
{
    public class DesignationMasters
    {
        public int INTDESIGID { get; set; }
        public string UsertypeName { get; set; }
        public int USERTYPE { get; set; }
        public int DISTID { get; set; }
        public int BLOCKID { get; set; }
        public string DESIGNATIONNAME { get; set; }
        public string VCHDESIGNAME { get; set; }
        public string NVCHDESIGNAME { get; set; }
        public string NVCHALIASNAME { get; set; }

        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }
       
       
        public string BlockName { get; set; }
    }
    public class CreateDesignationMaster
    {


        public string NVCHDESIGNAME { get; set; }
        public string NVCHALIASNAME { get; set; }

        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }

    }
    public class DesignationMasterModel
    {
        public List<DesignationMasters> DesignationMasterList { get; set; }
    }
}
