using CTMS.Model.Entities;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CTMS.Repository.Repository
{
    public class BannerRepository : CTMSRepositoryBase, IBannerRepository
    {
        public BannerRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> I_ManageBanner(ManageBanner TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "I");
            p.Add("@Id", TBL.Id);
            p.Add("@BannerTitle", TBL.BannerTitle);
            p.Add("@BannerTitleOD", TBL.BannerTitleOD);
            p.Add("@BannerDescription", TBL.BannerDescription);
            p.Add("@BannerDescriptionOD", TBL.BannerDescriptionOD);
            p.Add("@BannerImage", TBL.BannerImage);
            p.Add("@BannerPath", TBL.BannerPath);
            p.Add("@StartDate", TBL.StartDate);
            p.Add("@EndDate", TBL.EndDate);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _= await Connection.ExecuteAsync("ManageBanner", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> U_ManageBanner(ManageBanner TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@BannerTitle", TBL.BannerTitle);
            p.Add("@BannerTitleOD", TBL.BannerTitleOD);
            p.Add("@BannerDescription", TBL.BannerDescription);
            p.Add("@BannerDescriptionOD", TBL.BannerDescriptionOD);
            p.Add("@BannerImage", TBL.BannerImage);
            p.Add("@BannerPath", TBL.BannerPath);
            p.Add("@StartDate", TBL.StartDate);
            p.Add("@EndDate", TBL.EndDate);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _= await Connection.ExecuteAsync("ManageBanner", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> D_ManageBanner(ManageBanner TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("ManageBanner", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<ManageBanner>> R_ManageBanner(ManageBanner TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");
            p.Add("@Id", TBL.Id);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryAsync<ManageBanner>("ManageBanner", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<ManageBanner>> RO_ManageBanner(ManageBanner TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "RO");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<ManageBanner>("ManageBanner", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<ManageBanner>> ContentBanner()
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "BV");
            var results = await Connection.QueryAsync<ManageBanner>("ManageBanner", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<ManageDashboardCount> DashBoardCount()
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DV");
                var results = await Connection.QueryAsync<ManageDashboardCount>("USP_REPORT_VIEW", p, commandType: CommandType.StoredProcedure);
                return results.FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

    }
}
