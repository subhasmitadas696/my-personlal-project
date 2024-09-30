using CTMS.Web.Areas.AdminConsole.Models.GlobalLink;
using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;

namespace CTMS.Web.Areas.AdminConsole.Models.PrimaryLink
{
    public class Primary
    {
        public decimal AccessLevel { get; set; }
        public string ActionCode { get; set; }
        public string Browser { get; set; }
        public string DeptId { get; set; }
        public string FileName { get; set; }
        public int FunctionId { get; set; }
        public string ExternalURL { get; set; }
        public int LinkType { get; set; }
        public string FunctionName { get; set; }
        public int GlinkId { get; set; }
        public string GLinkName { get; set; }
        public bool OnHomePage { get; set; }
        public string PlinkId { get; set; }
        public List<Primary> PlinkIdName { get; set; }
        public string PlinkNameinAhmaric { get; set; }
        public string PlinkName { get; set; }
        public int SlNo { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class FunctionMasterModel
    {

        public int INTFUNCTIONID { get; set; }
        public string VCHFUNCTION { get; set; }

    }
    public class UpdatePrimaryLink
    {

        public int INTPLINKID { get; set; }
        public string NVCHPLINKNAME { get; set; }
        public int INTGLINKID { get; set; }
        public int INTFUNCTIONID { get; set; }
        public int INTSLNO { get; set; }
        public int UpdatedBy { get; set; }
        public int INTPROJECTLINKID { get; set; }
        public List<ViewGlobal> GlobalList { get; set; }
        public List<FunctionMasterModel> FunctionList { get; set; }

    }
    public class PrimaryLink
    {

        public int INTPLINKID { get; set; }
        public string NVCHPROJECTLINKNAME { get; set; }
        public string NVCHPLINKNAME { get; set; }
        public string NVCHGLINKNAME { get; set; }
        public string VCHFUNCTION { get; set; }
        public int INTSLNO { get; set; }

    }
    public class PrimaryModel
    {

        public List<ViewGlobal> GlobalList { get; set; }
        public List<FunctionMasterModel> FunctionList { get; set; }
        public List<PrimaryLink> PrimaryLinkList { get; set; }
        public int GlinkId { get; set; }
        public int FunctionId { get; set; }
        public int INTPROJECTID { get; set; }
        public string PlinkName { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string MarkInactivePrimaryId { get; set; }
        public int INTPLINKID { get; set; }
        public string NVCHPLINKNAME { get; set; }
        public int INTGLINKID { get; set; }
        public int INTFUNCTIONID { get; set; }
        public int INTSLNO { get; set; }



        //-----------------
        public ViewGlobal order { get; set; }
        // public int order { get; set; }
        public List<PrimaryLink> orderDetails { get; set; }
        public string slnomodifyList { get; set; }
        public List<ViewPoject> ViewProjectLinkList { get; set; }
        public int INTPROJECTLINKID { get; set; }
        public List<ViewGlobal> ViewGlobalLinkDetailsByProjectId { get; set; }
    }
}
