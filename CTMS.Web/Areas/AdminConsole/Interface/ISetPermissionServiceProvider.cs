using CTMS.Web.Areas.AdminConsole.Models.SetPermission;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface ISetPermissionServiceProvider
    {
        Task<SetPermissionModel> GetAllPrimaryLinkByGlobalLink(SetPermissionModel objpermission);
        Task<SetPermissionModel> GetAllPrimaryLinkByGlobalLinkUserwise(SetPermissionModel objpermission);
        string SetPermissionLink_Designation(int designationId, int Plinkid, int Intaccess, int user, int projectId);
        string SetPermissionLink_User(int userId, int Plinkid, int Intaccess, int user, int projectId);
        string DeletePermissionLink_Designation(int DesignationId, int projectId);
        string DeletePermissionLink_User(int userId, int projectId);
        Task<SetPermissionModel> GetAllUser();
        string RemovePermissionList_User(int userId, int updatedby);
    }
}
