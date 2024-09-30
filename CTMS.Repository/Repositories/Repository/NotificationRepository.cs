using CTMS.Model.Entities.Notification;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CTMS.Repository.Repository
{
	public class NotificationRepository:CTMSRepositoryBase,INotificationRepository
	{
		public NotificationRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertNotification(NotificationMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "I");
            p.Add("@Title", TBL.Title);
            p.Add("@TitleOd", TBL.TitleOd);
            p.Add("@Description", TBL.Description);
            p.Add("@DescriptionOd", TBL.DescriptionOd);
            p.Add("@StartDate", TBL.StartDate);
            p.Add("@EndDate", TBL.EndDate);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> UpdateNotification(NotificationMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@Title", TBL.Title);
            p.Add("@TitleOd", TBL.TitleOd);
            p.Add("@Description", TBL.Description);
            p.Add("@DescriptionOd", TBL.DescriptionOd);
            p.Add("@StartDate", TBL.StartDate);
            p.Add("@EndDate", TBL.EndDate);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> DeleteNotification(NotificationMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<NotificationMaster>> ViewNotification(NotificationMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "V");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<NotificationMaster>("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<NotificationMaster>> EditNotification(NotificationMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "S");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<NotificationMaster>("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<NotificationMaster>> ViewAnnoncement()
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "AN");
            var results = await Connection.QueryAsync<NotificationMaster>("USP_NOTIFICATION", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
    }
}
