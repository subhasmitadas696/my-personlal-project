using CTMS.Model.Entities.FundMaster;
namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IFundManagementRepository
    {
        Task<int> FundManage(FundMaster TBL);
        Task<List<FundMaster>> GetOpeningBalance(FundMaster FM);
        Task<int> AddOpeningBalance(FundMaster TBL);
        Task<List<FundMasterView>> ViewOpeningBalance(int distid);
        Task<List<FundMasterView>> ViewFund(int distid);
        Task<List<FundMasterView>> ViewFundTransfer(int distid);
        Task<int> AddFund(FundMaster TBL);
        Task<int> FundTransfer(FundMaster TBL);
        Task<List<FundMaster>> GetAccountNo(FundMaster FM);
        Task<List<FundMaster>> GetReceiverAccountNo(FundMaster FM);
        Task<List<FundMaster>> GetAvailableBalance(FundMaster FM);
        Task<List<FundMaster>> OpeningBalanceexist(FundMaster FM);
        Task<List<Payment>> PendingPayment(int BlockId, string fromDate, string toDate);
        Task<List<Payment>> SuccessPayment(int BlockId, string fromDate, string toDate);
        Task<List<Payment>> SuccessPaymentDetails(int AssignEventId,int BlockId);
        Task<int> PaymentAddUpdate(Payment PMT);
    }
}
