using CTMS.Web.Areas.AdminConsole.Models.DesignationMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IDesignationServiceProvider
    {
        string AddDesignation(DesignationMasters model);
        Task<DesignationMasterModel> GetAllDesgination(int id);
        string EditDesignation(DesignationMasters model);
        string MarkActive(DesignationMasters model);
        string MarkInactive(DesignationMasters model);
        Task<DesignationMasterModel> GetById(int id);
    }
}
