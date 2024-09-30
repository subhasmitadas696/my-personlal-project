using CTMS.Model.Entities.ManageFAQ;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;
namespace CTMS.Repository.Repository
{
    public class FaqRepository : CTMSRepositoryBase, IFaqQRepository
    {
        public FaqRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertManageFAQ(Managefaq TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "I");
            p.Add("@Question", TBL.Question);
            p.Add("@QuestionOD", TBL.QuestionOD);
            p.Add("@Answer", TBL.Answer);
            p.Add("@AnswerOD", TBL.AnswerOD);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> UpdateManageFAQ(Managefaq TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@Question", TBL.Question);
            p.Add("@QuestionOD", TBL.QuestionOD);
            p.Add("@Answer", TBL.Answer);
            p.Add("@AnswerOD", TBL.AnswerOD);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> DeleteManageFAQ(Managefaq TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<List<Managefaq>> ViewManageFAQ(Managefaq TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");
            var results = await Connection.QueryAsync<Managefaq>("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<Managefaq>> EditManageFAQ(Managefaq TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "RO");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<Managefaq>("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<Managefaq>> BindFAQ()
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");
            var results = await Connection.QueryAsync<Managefaq>("ManageFAQ", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
    }
}
