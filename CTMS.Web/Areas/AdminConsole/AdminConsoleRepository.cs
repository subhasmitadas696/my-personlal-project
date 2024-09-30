using Microsoft.AspNetCore.Connections;
using NuGet.Protocol.Plugins;
using System.Data;
using Dapper;
using CTMS.Repository.Factory;
using CTMS.Repository.BaseRepository;
using CTMS.Web.Areas.AdminConsole.Models.User;

namespace CTMS.Web.Areas.AdminConsole
{
    public class AdminConsoleRepository : CTMSRepositoryBase, IAdminConsoleRepository
    {
        public AdminConsoleRepository(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }
        public async Task<int> GetUserId(string VCHUSERNAME)
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
            try
            {
                string sql = "select INTUSERID from M_POR_USER where upper(VCHUSERNAME) ='" + VCHUSERNAME.ToUpper() + "'";
                var userId = await Connection.QueryFirstAsync<int>(sql);
                return userId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IList<LinkAccess> GetLinkAccessByUserId(int UserId, int ProjectId, int UID,  int desgid)
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

            try
            {
                var aParam = new DynamicParameters();
                //UserId = 23;
                aParam.Add("p_action", "VU");
                aParam.Add("p_intintuserid", UserId);
                aParam.Add("p_UID", UID);
                aParam.Add("p_intprojectid", ProjectId);
                aParam.Add("p_intdesignationid",  desgid);               
                var query = "USP_ADMIN_USERMASTER_VIEW";
                var result = Connection.Query<LinkAccess>(query, aParam, commandType: CommandType.StoredProcedure);
                return result.ToList();

            }
            catch (Exception exception)
            {                
                throw exception;                
            }
        }
    }
}
