using CTMS.Model.DTOs;
using CTMS.Model.Entities.ChangePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IChangePasswordRepository
    {
        Task<int> ChangePasswordAsync(ChangePassword changepass);
        //ForgotPassword? ForgotPasswordAsync(ForgotPassword forgotpass, out int returnVal);
        Task<int> OTPAsync(OtpTracker otp);
        Task<int> ForgotPasswordAsync(ForgotPassword forgotpass);
        Task<List<OtpTracker>> ValideOtp(OtpTracker otp);
        Task<int> InsertOTPTracker(OtpTracker otp);

    }
}
