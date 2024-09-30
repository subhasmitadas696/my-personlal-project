using CTMS.Model.Entities.NewsAndUpdate;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;


namespace CTMS.Repository.Repositories.Repository
{
    public class NewsAndUpdateRepository : CTMSRepositoryBase,INewsAndUpdateRepository
    {

        public NewsAndUpdateRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }

        public async Task<int> InsertNewsAndUpdate(NewsAndUpdate TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "I");
            p.Add("@NewsTitle", TBL.NewsTitle);
            p.Add("@NewsTitleOD", TBL.NewsTitleOD);
            p.Add("@NewsDescription", TBL.NewsDescription);
            p.Add("@NewsDescriptionOD", TBL.NewsDescriptionOD);
            p.Add("@NewsSource", TBL.NewsSource);
            p.Add("@NewsSourceOD", TBL.NewsSourceOD);
            p.Add("@NewsPublisDate", TBL.NewsPublishDate);
            p.Add("@NewsImage", TBL.NewsPhoto);
            p.Add("@NewsPath", TBL.NewsPhotoPath);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NEWS_AND_UPDATE", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> UpdateNewsAndUpdate(NewsAndUpdate TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@NewsTitle", TBL.NewsTitle);
            p.Add("@NewsTitleOD", TBL.NewsTitleOD);
            p.Add("@NewsDescription", TBL.NewsDescription);
            p.Add("@NewsDescriptionOD", TBL.NewsDescriptionOD);
            p.Add("@NewsSource", TBL.NewsSource);
            p.Add("@NewsSourceOD", TBL.NewsSourceOD);
            p.Add("@NewsPublisDate", TBL.NewsPublishDate);           
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NEWS_AND_UPDATE", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> DeleteNewsAndUpdate(NewsAndUpdate TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_NEWS_AND_UPDATE", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<NewsAndUpdate>> ViewNewsAndUpdate(NewsAndUpdate TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");           
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryAsync<NewsAndUpdate>("USP_NEWS_AND_UPDATE", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<NewsAndUpdate>> EditNewsAndUpdate(NewsAndUpdate TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "RO");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<NewsAndUpdate>("USP_NEWS_AND_UPDATE", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
    }
}
