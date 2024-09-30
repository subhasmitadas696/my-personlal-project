using CTMS.Model.DTOs;
namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        UsersDto? CheckUserName(string? mobileno, out int returnVal);
        List<UsersDto>? M_CheckUserName(string? mobileno, out int returnVal);
        List<VOResponse>? Check_VerifyOfficerNumber(string? mobileno, out int returnVal);
        Task<int> UpdateFailedCount(string? regno, string? action);
        UsersDto? Login(LoginDto Log, out int returnVal);
        List<UsersDto>? M_Login(LoginDto Log, out int returnVal);
        UsersDto? TroupeLogin(LoginDto Log, out int returnVal);
        List<UsersDto>? M_TroupeLogin(LoginDto Log, out int returnVal);
        Task<List<GpDetails>>? GpDetails(int USERID);
        List<VerifyOtp_Reviewer>? VerifyOtpReviewer(string? mobileno,string? otp,string? TYPE);
        string Reviewer_SubmitReport(Reviewer_SubmitRequest RSQ);
    }
}
