using CTMS.Core;
using CTMS.Model.Entities.FundMaster;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;


namespace CTMS.Repository.Repositories.Repository
{
    public class FundManagementRepository : CTMSRepositoryBase, IFundManagementRepository
    {
        public FundManagementRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> FundManage(FundMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "I");
            p.Add("@DistId", TBL.DistId);
            p.Add("@BlockId", TBL.BlockId);
            p.Add("@DeptId", TBL.DeptId);
            p.Add("@SchemeId", TBL.SchemeId);
            p.Add("@EventId", TBL.EventId);
            p.Add("@OpeningBalance", TBL.OpeningBalance);
            p.Add("@AvlBalance", TBL.AvlBalance);
            p.Add("@FundTransfer", TBL.FundTransfer);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("[USP_FUND_MANAGE]", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<FundMaster>> GetOpeningBalance(FundMaster FM)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "R");
                p.Add("@DistId", FM.DistId);

                var results = await Connection.QueryAsync<FundMaster>("USP_FUND_MANAGE", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMasterView>> ViewOpeningBalance(int distid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "V");
                p.Add("@DistId", distid);
                var results = await Connection.QueryAsync<FundMasterView>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMasterView>> ViewFund(int distid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "VF");
                p.Add("@DistId", distid);
                var results = await Connection.QueryAsync<FundMasterView>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMasterView>> ViewFundTransfer(int distid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "VFT");
                p.Add("@DistId", distid);
                var results = await Connection.QueryAsync<FundMasterView>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddOpeningBalance(FundMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "I");
            p.Add("@DistId", TBL.DistId);
            //p.Add("@DeptId", TBL.DeptId);
            p.Add("@SchemeId", TBL.SchemeId);
            p.Add("@BankId", TBL.BankId);
            p.Add("@AccountNo", TBL.AccountNo);
            p.Add("@OpeningBalance", TBL.OpeningBalance);
            p.Add("@Date", TBL.Date);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("[USP_FUND_MASTER]", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> AddFund(FundMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "FI");
            p.Add("@DistId", TBL.DistId);
            // p.Add("@DeptId", TBL.DeptId);
            p.Add("@SchemeId", TBL.SchemeId);
            p.Add("@BankId", TBL.BankId);
            p.Add("@AccountNo", TBL.AccountNo);
            p.Add("@FundAmount", TBL.FundAmount);
            p.Add("@Date", TBL.Date);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("[USP_FUND_MASTER]", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> FundTransfer(FundMaster TBL)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "FTB");
                p.Add("@DistId", TBL.DistId);
                //p.Add("@DeptId", TBL.DeptId);
                p.Add("@SchemeId", TBL.SchemeId);
                p.Add("@BankId", TBL.BankId);
                p.Add("@AccountNo", TBL.AccountNo);
                p.Add("@AvailableBalance", TBL.AvlBalance);
                p.Add("@Date", TBL.Date);
                p.Add("@BlockId", TBL.BlockId);
                p.Add("@ReceiverBankId", TBL.RecBankId);
                p.Add("@ReceiverAccountNo", TBL.RecAccNo);
                p.Add("@FundAmount", TBL.FundAmount);
                p.Add("@CreatedBy", TBL.CreatedBy);
                p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("[USP_FUND_MASTER]", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<Int32>("@MsgOut");
                return Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMaster>> GetAccountNo(FundMaster FM)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "AB");
                p.Add("@DistId", FM.DistId);
                //p.Add("@DeptId", FM.DeptId);
                p.Add("@SchemeId", FM.SchemeId);
                var results = await Connection.QueryAsync<FundMaster>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMaster>> GetReceiverAccountNo(FundMaster FM)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "RAB");
                p.Add("@BlockId", FM.BlockId);
                //p.Add("@DeptId", FM.DeptId);
                p.Add("@SchemeId", FM.SchemeId);
                var results = await Connection.QueryAsync<FundMaster>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMaster>> GetAvailableBalance(FundMaster FM)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "ABB");
                p.Add("@DistId", FM.DistId);
                p.Add("@DeptId", FM.DeptId);
                p.Add("@SchemeId", FM.SchemeId);

                var results = await Connection.QueryAsync<FundMaster>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FundMaster>> OpeningBalanceexist(FundMaster FM)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "OBC");
                p.Add("@DistId", FM.DistId);
                //p.Add("@DeptId", FM.DeptId);
                p.Add("@SchemeId", FM.SchemeId);

                var results = await Connection.QueryAsync<FundMaster>("USP_FUND_MASTER", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Payment>> PendingPayment(int Blockid, string fromDate, string toDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "PP");
                p.Add("@BlockId", Blockid);
                p.Add("@FROMDATE", fromDate);
                p.Add("@TODATE", toDate);
                var results = await Connection.QueryAsync<Payment>("USP_FUND_MANAGE", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Payment>> SuccessPaymentDetails(int AssignEventId, int BlockId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "SPD");
                p.Add("@EventId", AssignEventId);
                p.Add("@BlockId", BlockId);
                var results = await Connection.QueryAsync<Payment>("USP_FUND_MANAGE", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Payment>> SuccessPayment(int Blockid, string fromDate, string toDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "SP");
                p.Add("@BlockId", Blockid);
                p.Add("@FROMDATE", fromDate);
                p.Add("@TODATE", toDate);
                var results = await Connection.QueryAsync<Payment>("USP_FUND_MANAGE", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> PaymentAddUpdate(Payment PMT)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Action", "PI");
                p.Add("@EventId", PMT.AssignEventId);
                p.Add("@BlockId", PMT.BlockId);
                p.Add("@Troupeid", PMT.TroupeId);
                p.Add("@Transctionid", PMT.TransctionID);
                p.Add("@PaymentAmount", PMT.PaymentAmount);
                p.Add("@TransctionProof", PMT.UploadFolderPath);
                p.Add("@PaymentType", PMT.PaymentType);
                p.Add("@paymentmode", PMT.PaymentMode);
                p.Add("@Status", PMT.STATUS);
                p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_FUND_MANAGE", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@MsgOut");
                return returnVal.ToInt();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
