using CTMS.Web.Areas.AdminConsole.Models.LevelMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface ILevelServiceProvider
    {
        string AddLevel(CreateLevelMaster objlevel);
        Task<CreateLevelMaster> GetAllLevelByHierarchyId(int id);
        Task<LevelMasterModel> GetAllLevel(int id);
        Task<IEnumerable<CreateLevelMaster>> GetLevelById(int id);
        string EditLevel(CreateLevelMaster objlevel);
        string MarkActive(LevelMasterModel objmodel);
        string MarkInactive(LevelMasterModel objmodel);
    }
}
