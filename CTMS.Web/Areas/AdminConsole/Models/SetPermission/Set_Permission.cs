using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;

namespace CTMS.Web.Areas.AdminConsole.Models.SetPermission
{
    public class Set_Permission
    {

        public string Action { get; set; }

        public string ActionCode { get; set; }

        public int FunctionId { get; set; }

        public string GlinkId { get; set; }

        public string GLinkName { get; set; }

        public string GroupDeptId { get; set; }

        public string GroupUserId { get; set; }

        public int ID { get; set; }

        public string PermissionStatus { get; set; }

        public string PermissionType { get; set; }

        public string PlinkAdd { get; set; }

        public int PlinkId { get; set; }

        public string PlinkManage { get; set; }

        public string PLinkName { get; set; }

        public string PlinkView { get; set; }

        public string PlnkId { get; set; }

        public int UpdatedBy { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }
    }
    public class UserDetails
    {

        public int UserID { get; set; }
        public string UserName { get; set; }

    }
    public class SetPermissionDetails
    {

        public int GLOBALID { get; set; }
        public string GLOBALNAME { get; set; }
        public int PRIMARYID { get; set; }
        public string PRIMARYNAME { get; set; }
        public string TINACCESS { get; set; }
        public string PRIMARYLINKCOUNT { get; set; }

        public string PRIMARYLINKPERMISSIONCOUNT { get; set; }
        public string VCHACTION1 { get; set; }

    }
    public class AddupdateSetPermissionDesignation
    {

        public int INTACCESSID { get; set; }
        public int INTDESINATIONID { get; set; }
        public int INTPLINKID { get; set; }
        public string VCHACTION1 { get; set; }
        public string VCHACTION2 { get; set; }
        public string VCHACTION3 { get; set; }
        public int TINACCESS { get; set; }
        public int INTCREATEDBY { get; set; }
        public int INTUPDATEDBY { get; set; }


    }
    public class SetPermissionModel
    {
        public List<ViewPoject> ProjectList { get; set; }
        public List<SetPermissionDetails> GlobalPrimaryList { get; set; }
        public List<SetPermissionDetails> GlobalPrimaryListUser { get; set; }
        public List<UserDetails> UserList { get; set; }
        public AddupdateSetPermissionDesignation command { get; set; }
        public string setpermissionList { get; set; }
        public string setpermissionListUser { get; set; }
        public int DesignId { get; set; }
        public int UserDesignId { get; set; }
        public bool Checked { get; set; }
        public int UserId { get; set; }
        public int INTPROJECTLINKID { get; set; }


    }
}
