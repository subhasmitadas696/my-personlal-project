using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.LevelDetailMaster;
using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class LeveDetailslServiceProvider : CTMSRepositoryBase, ILevelDetailsServiceProvider
    {

        private readonly IConfiguration _configuration;
        public LeveDetailslServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }
        public string AddLevelDetails(LevelDetailMaster objlevel)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_NVCHLEVELNAME", objlevel.NVCHLEVELNAME);
                p.Add("@P_INTLEVELID", objlevel.INTLEVELID);
                p.Add("@P_INTPARENTID", objlevel.INTPARENTID);
                p.Add("@P_VCHALIAS", objlevel.VCHALIAS);
                p.Add("@P_VCHTELNO", objlevel.VCHFAXNO);
                p.Add("@P_VCHFAXNO", objlevel.VCHTELNO);
                p.Add("@P_INTCREATEDBY", objlevel.INTUPDATEDBY);
                p.Add("@P_INTUPDATEDBY", objlevel.INTCREATEDBY);
                p.Add("@P_ACTION", "A");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                SuccessMessages = Connection.Query<SuccessMessage>("USP_ADMIN_LEVELDETAILSMASTER_MANAGE", p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return strOutput;

        }
        public async Task<LevelDetailMaster> GetAllLevelDetailsByHierarchyId(int id)
        {
            try
            {


                var p = new DynamicParameters();
                p.Add("@P_INTLEVELID", id);
                p.Add("@P_Action", "VLD");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_LEVELDETALISMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.ParentLevelDetailsList = t1.ToList();
                return viewModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<LevelDetailMaster> GetAllLevelParentDetailsByHierarchyId(int id, int hid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELID", id);
                p.Add("@P_INTHIERARCHYID", hid);
                p.Add("@P_Action", "VHP");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_LEVELDETALISMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.ParentLevelDetailsList = t1.AsList();
                return viewModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<LevelDetailMaster> GetLevelByParentId(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELID", id);
                p.Add("@P_Action", "VLP");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_LEVELDETALISMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.ParentLevelDetailsList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LevelDetailMaster> GetLevelById(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELID", id);
                p.Add("@P_Action", "NewVLP");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_LEVELDETALISMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.ParentLevelDetailsList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LevelDetailMaster> GetAllLevelDetailsForEdit()
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@P_Action", "A");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_LEVELDETALISMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.ParentLevelDetailsList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LevelDetailMaster> GetHierarchy()

        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@P_Action", "VH");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETALISMASTER_VIEW";
                var result = await Connection.QueryMultipleAsync(query, p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<Hierarchy>();
                LevelDetailMaster viewModel = new LevelDetailMaster();
                viewModel.HierarchyList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<LevelDetailsMasterModel> GetAllLevelDetails(int id)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@P_Action", "V");
                p.Add("@P_intStatus", id);
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETALISMASTER_VIEW";
                var result = await Connection.QueryMultipleAsync(query, p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<LevelDetails>();
                LevelDetailsMasterModel viewModel = new LevelDetailsMasterModel();
                viewModel.LevelDetailsList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<LevelDetailMaster>> GetLevelDetailsById(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_Action", "VI");
                p.Add("@P_INTLEVELDETAILID", id);
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETALISMASTER_VIEW";
                var result = await Connection.QueryAsync<LevelDetailMaster>(query, p, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public string EditLevelDetails(LevelDetailMaster objlevel)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELDETAILID", objlevel.INTLEVELDETAILID);
                p.Add("@P_NVCHLEVELNAME", objlevel.NVCHLEVELNAME);

                p.Add("@P_INTLEVELID", objlevel.INTLEVELID);
                p.Add("@P_INTPARENTID", objlevel.INTPARENTID);
                p.Add("@P_VCHALIAS", objlevel.VCHALIAS);
                p.Add("@P_VCHTELNO", objlevel.INTCREATEDBY);
                p.Add("@P_VCHFAXNO", objlevel.INTUPDATEDBY);
                p.Add("@P_INTCREATEDBY", objlevel.INTCREATEDBY);
                p.Add("@P_INTUPDATEDBY", objlevel.INTCREATEDBY);
                p.Add("@P_ACTION", "E");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETAILSMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string MarkInactive(LevelDetailsMasterModel objmodel)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELDETAILID", objmodel.INTLEVELDETAILID);
                p.Add("@P_INTUPDATEDBY", objmodel.INTUPDATEDBY);
                p.Add("@P_ACTION", "I");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETAILSMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }
        public string MarkActive(LevelDetailsMasterModel objmodel)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_INTLEVELDETAILID", objmodel.INTLEVELDETAILID);
                p.Add("@P_INTUPDATEDBY", objmodel.INTUPDATEDBY);
                p.Add("@P_ACTION", "EA");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_LEVELDETAILSMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }

    }
}
