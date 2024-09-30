using CTMS.Model.Entities;
using CTMS.Model.Entities.PageMaster;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Repository
{
    public class PageMasterRepository : CTMSRepositoryBase, IPageMasterRepository
    {
        public PageMasterRepository(ICTMSConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<int> D_ManagePage(PageMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_HandlePageMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }

        public async Task<int> I_ManagePage(PageMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "I");
            p.Add("@Id", TBL.Id);
            p.Add("@Title", TBL.PageTitle);
            p.Add("@Metatag", TBL.PageTitleOD);
            p.Add("@PageDetail", TBL.PageDescription);
            p.Add("@PageDetailOD", TBL.PageDescriptionOD);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_HandlePageMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }

        public async Task<List<PageMaster>> RO_ManagePage(PageMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "RO");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<PageMaster>("USP_HandlePageMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<PageMaster>> R_ManagePage(PageMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");
            p.Add("@Id", TBL.Id);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryAsync<PageMaster>("USP_HandlePageMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> U_ManagePage(PageMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@Title", TBL.PageTitle);
            p.Add("@Metatag", TBL.PageTitleOD);
            p.Add("@PageDetail", TBL.PageDescription);
            p.Add("@PageDetailOD", TBL.PageDescriptionOD);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_HandlePageMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
    }
}
