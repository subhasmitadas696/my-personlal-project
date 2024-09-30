using CTMS.Model.Entities.Master;
using CTMS.Model.BlockMaster;
using CTMS.Model.DistMaster;
using CTMS.Model.GpMaster;
using CTMS.Model.VillageMaster;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IMasterRepository
    {
        Task<DistMaster> GetAllDistricts();
        Task<BlockMaster> GetAllBlocks(DistMaster MDI);
        Task<GpMaster> GetAllGramPanchayats(BlockMaster MBL);
        Task<VillageMaster> GetAllVillages(GpMaster MGP);
        Task<int> GetDistrictByBlockid(int blockid);
        string GetVillageNameById(string villageid);
        Task<List<DropdownData>> Fill_Dropdown(SearchdownData search);
    }
}
