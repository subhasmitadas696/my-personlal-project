using CTMS.Web.Areas.AdminConsole.Models.GlobalLink;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IGblLinkServiceProvider
    {
        int ActivateGlobalLink(Global objGloballink);
        IList<Global> GetGlobalLinkDetails(Global objGloballink);
        int InActivateGlobalLink(Global objGloballink);
        int UpdateSlno(Global objGloballink);
        IList<Global> GetAllLocation(Global objGloballink);
        //---------------------------------------------------
        string AddGlobalLink(ViewGlobal objGloballink);
        Task<IEnumerable<ViewGlobal>> GetGlobalLinkById(int id);

        Task<ViewGlobalLink> GetAllActiveGlobalLink();
        Task<ViewGlobalLink> GetAllInActiveGlobalLink();
        string EditGlobalLink(ViewGlobal objGloballink);
        string DeleteGlobalLink(int id, int updatedby);
        string MarkActiveGlobalLink(int id, int updatedby);
        string ModifySlnoGlobalLink(int globalId, int slno, int updatedby);
        int GetGlobalLinkMaxId();
        Task<ViewGlobalLink> GetById(int id);
        Task<ViewGlobalLink> GetMaxId();
    }
}
