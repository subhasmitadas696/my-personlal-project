using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;

namespace CTMS.Web.Areas.AdminConsole.Models.GlobalLink
{
    public class Global
    {

        public string ActionCode { get; set; }

        public int CreatedBy { get; set; }

        public string DeletedStatus { get; set; }

        public string DeptID { get; set; }

        public List<Global> GloballinkidNName { get; set; }

        public string GlobalLinkName { get; set; }

        public string GlobalLinkNameinAhmaric { get; set; }

        public string IntGlobalLinkID { get; set; }

        public string Location { get; set; }

        public bool OnHomePage { get; set; }

        public int SLNO { get; set; }

        public string GlobalLinkImg { get; set; }

        public string VCHICONCLASS { get; set; }
        public string HierarchyName { get; set; }

        public int HierarchyID { get; set; }

        public int RowNo { get; set; }
        public string showMessage { get; set; }
        public int GintSortNum { get; set; }
        public int GMAxid { get; set; }

    }
    public class ViewGlobal
    {

        public string resultmsg { get; set; }

        public int intGLinkID { get; set; }
        public int intSortNum { get; set; }
        public int MAxid { get; set; }

        public string nvchGLinkName { get; set; }
        public string VCHICONCLASS { get; set; }
        public int updatedBy { get; set; }
        public int GintSortNum { get; set; }
        public int GMAxid { get; set; }
        public string GlobalLinkName { get; set; }
        public int CreatedBy { get; set; }
        public int INTPROJECTLINKID { get; set; }
        public string NVCHPROJECTLINKNAME { get; set; }
        public List<ViewPoject> ProjectList { get; set; }

    }
    public class ViewGlobalLink
    {
        public List<ViewGlobal> ViewGlobalLinkDetailsmodel { get; set; }


    }
    public class GlobalModel
    {
        public ViewGlobal GlobalModelIdwise { get; set; }
        public List<ViewGlobal> GlobalModelList { get; set; }
        public int updatedBy { get; set; }

        public string slnomodifyList { get; set; }


    }
}
