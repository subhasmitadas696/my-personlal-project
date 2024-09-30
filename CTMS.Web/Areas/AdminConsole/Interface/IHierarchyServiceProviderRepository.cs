using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IHierarchyServiceProviderRepository
    {
        string AddHierarchy(Hierarchy objhierarchy);
        Task<HierarchyModel> GetAllHierarchy(int id);
        string EditHierarchy(Hierarchy objhierarchy);
        string MarkActive(Hierarchy objhierarchy);
        string MarkInactive(Hierarchy objhierarchy);
        Task<HierarchyModel> GetById(int id);
    }
}
