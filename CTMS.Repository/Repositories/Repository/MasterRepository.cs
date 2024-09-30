using CTMS.Model.Entities.Master;
using CTMS.Model.BlockMaster;
using CTMS.Model.DistMaster;
using CTMS.Model.GpMaster;
using CTMS.Model.VillageMaster;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;
using CTMS.Core;

namespace CTMS.Repository.Repository
{
    public class MasterRepository : CTMSRepositoryBase, IMasterRepository
    {
        public MasterRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<DistMaster> GetAllDistricts()
        {
            throw new NotImplementedException();
        }
        public async Task<BlockMaster> GetAllBlocks(DistMaster MDI)
        {
            throw new NotImplementedException();
        }
        public async Task<GpMaster> GetAllGramPanchayats(BlockMaster MBL)
        {
            throw new NotImplementedException();
        }
        public async Task<VillageMaster> GetAllVillages(GpMaster MGP)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropdownData>> Fill_Dropdown(SearchdownData search)
        {
            DynamicParameters? objParam = new DynamicParameters();
            objParam.Add("Level", search.Level);
            objParam.Add("FilterId", search.FilterId);
            var results = await Connection.QueryAsync<DropdownData>("USP_FILLDROPDOWN_GEODATA", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }

        public async Task<int> GetDistrictByBlockid(int blockid)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@Level", "GDIST");
            objParam.Add("@FilterId", blockid);

            // Use QueryFirstOrDefaultAsync and await to correctly handle asynchronous operations
            int results = await Connection.QueryFirstOrDefaultAsync<int>("USP_FILLDROPDOWN_GEODATA", objParam, commandType: CommandType.StoredProcedure);

            return results;
        }
        public string GetVillageNameById(string villageid)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@Level", "GVILLAGE");
            objParam.Add("@FilterId", villageid);
            var results = Connection.QueryFirstOrDefaultAsync<string>("USP_FILLDROPDOWN_GEODATA", objParam, commandType: CommandType.StoredProcedure).Result;
            return results.ToString();
        }

    }
}
