using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class HierarchyServiceProviderRepository : CTMSRepositoryBase, IHierarchyServiceProviderRepository
    {

        private readonly IConfiguration _configuration;
        public HierarchyServiceProviderRepository(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }

        public string AddHierarchy(Hierarchy objhierarchy)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_NVCHHIERARCHYNAME", objhierarchy.ROLENAME);               
                p.Add("@P_INTNOLEVEL", objhierarchy.LEVELID);               
                p.Add("@P_INTCREATEDBY", objhierarchy.INTCREATEDBY);
                p.Add("@P_ACTION", "A");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_HIERARCHYMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }

        public string EditHierarchy(Hierarchy objhierarchy)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {

                var p = new DynamicParameters();
                p.Add("@P_INTHIERARCHYID", objhierarchy.ROLEID);
                p.Add("@P_NVCHHIERARCHYNAME", objhierarchy.ROLENAME);             
                p.Add("@P_INTNOLEVEL", objhierarchy.LEVELID);               
                p.Add("@P_INTUPDATEDBY", objhierarchy.INTUPDATEDBY);
                p.Add("@P_ACTION", "E");
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_HIERARCHYMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }


        public async Task<HierarchyModel> GetAllHierarchy(int id)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@P_intStatus", id);

                p.Add("@P_Action", "V");
                p.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var result = await Connection.QueryMultipleAsync("USP_ADMIN_HIERARCHYMASTER_VIEW", p, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<Hierarchy>();
                HierarchyModel viewModel = new HierarchyModel();
                viewModel.HierarchyList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HierarchyModel> GetById(int id)
        {
            try
            {
                var dyParam = new DynamicParameters();

                dyParam.Add("@P_INTHIERARCHYID", id);
                dyParam.Add("@P_Action", "VI");
                dyParam.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_HIERARCHYMASTER_VIEW";

                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<Hierarchy>();
                HierarchyModel viewModel = new HierarchyModel();
                viewModel.HierarchyList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public string MarkActive(Hierarchy objhierarchy)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;

            try
            {
                var dyParam = new DynamicParameters();

                dyParam.Add("@P_INTHIERARCHYID", objhierarchy.ROLEID);
                dyParam.Add("@P_ACTION", "EA");
                dyParam.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_HIERARCHYMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }

        public string MarkInactive(Hierarchy objhierarchy)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;

            try
            {

                var dyParam = new DynamicParameters(); ;
                dyParam.Add("@P_INTHIERARCHYID", objhierarchy.ROLEID);
                dyParam.Add("@P_ACTION", "I");
                dyParam.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_HIERARCHYMASTER_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
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
