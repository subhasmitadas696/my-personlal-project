using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.PaymentStructure;
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
    public class PaymentStructureRepository : CTMSRepositoryBase, IPaymentStructureRepository
    {
        public PaymentStructureRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertPaymentStructure(List<PaymentStructureMaster> PaymentStructure, string Code)
        {
            try
            {
                var p = new DynamicParameters();
                //if (PaymentStructure.id != 0)
                //{
                //    p.Add("@ACTIONCODE", "U");
                //}
                //else
                //{
                p.Add("@ACTIONCODE", Code);
                //}
                p.Add("@mytable", PaymentStructure[0].PaymentStr);
                p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_PAYMENT_STRUCTURE", p, commandType: CommandType.StoredProcedure);
                int returnVal = p.Get<Int32>("@MsgOut");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<PaymentStructureMaster>> ViewPaymentStructure()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "NEWVIEW");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<PaymentStructureMaster>("USP_PAYMENT_STRUCTURE", p, commandType: CommandType.StoredProcedure).ToList();
            return x;

        }
        public async Task<List<PaymentStructureMaster>> PaymentStructureGetById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("@P_Id", Id);
            p.Add("@ACTIONCODE", "BYID");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var GetAppById = Connection.Query<PaymentStructureMaster>("USP_PAYMENT_STRUCTURE", p, commandType: CommandType.StoredProcedure).AsList();
            return GetAppById;
        }
        public async Task<int> DeletePaymentStructure(int Id)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "D");
            p.Add("@P_Id", Id);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            Connection.Execute("USP_PAYMENT_STRUCTURE", p, commandType: CommandType.StoredProcedure);
            int x = Convert.ToInt32(p.Get<string>("@MsgOut"));
            return x;
        }
    }
}
