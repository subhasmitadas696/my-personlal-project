using CTMS.Web.Areas.AdminConsole.Models.LevelDetailMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface ILevelDetailsServiceProvider
    {
        string AddLevelDetails(LevelDetailMaster objlevel);
        Task<LevelDetailMaster> GetAllLevelDetailsByHierarchyId(int id);
        Task<LevelDetailMaster> GetAllLevelParentDetailsByHierarchyId(int id, int hid);
        Task<LevelDetailMaster> GetLevelByParentId(int id);
        Task<LevelDetailMaster> GetLevelById(int id);
        Task<LevelDetailMaster> GetAllLevelDetailsForEdit();
        Task<LevelDetailsMasterModel> GetAllLevelDetails(int id);
        Task<IEnumerable<LevelDetailMaster>> GetLevelDetailsById(int id);
        string EditLevelDetails(LevelDetailMaster objlevel);
        string MarkActive(LevelDetailsMasterModel objmodel);
        string MarkInactive(LevelDetailsMasterModel objmodel);
        Task<LevelDetailMaster> GetHierarchy(); 
    }
}
