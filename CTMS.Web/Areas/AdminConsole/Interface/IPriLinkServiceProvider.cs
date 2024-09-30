using CTMS.Web.Areas.AdminConsole.Models.PrimaryLink;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IPriLinkServiceProvider
    {
        string AddPrimaryLink(PrimaryModel objPrimaryLink);
        Task<PrimaryModel> GetAllFunctionMaster();
        Task<PrimaryModel> GetAllPrimaryLink(int id);
        string MarkInactivePrimaryLink(PrimaryModel objPrimaryLink);
        Task<IEnumerable<UpdatePrimaryLink>> GetPrimaryLinkById(int id);
        string EditPrimaryLink(PrimaryModel objPrimaryLink);
        string MarkActivePrimaryLink(PrimaryModel objPrimaryLink);
        Task<PrimaryModel> GetAllPrimaryLinkByGlobalLink(PrimaryModel objPrimaryLink);
        string ModifySlnoPrimaryLink(int primaryId, int slno, int updatedby);
        int GetPrimaryLinkMaxId(int Glinkid);
        Task<PrimaryModel> GetAllProjectLink();
        Task<PrimaryModel> GetAllActiveGlobalLinkByProjectId(int ProjectId);
    }
}
