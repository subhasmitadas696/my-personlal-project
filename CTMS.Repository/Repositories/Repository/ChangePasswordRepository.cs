using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Model.Entities.ChangePassword;
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
using static System.Net.WebRequestMethods;

namespace CTMS.Repository.Repositories.Repository
{
    public  class ChangePasswordRepository : CTMSRepositoryBase, IChangePasswordRepository
    {
        public ChangePasswordRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> ChangePasswordAsync(ChangePassword changepass)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "CP");
            p.Add("@P_USERID", changepass.USERID);
            p.Add("@P_OLDPASSWORD", changepass.OLDPASSWORD);
            p.Add("@P_NEWPASSWORD", changepass.NEWPASSWORD);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_CHANGE_PASSWORD", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        
       
        public async Task<int> OTPAsync(OtpTracker otp)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "O");
            p.Add("@P_OTP", otp.OTP);
            p.Add("@P_MOBILENO", otp.MOBILENO);

            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.QueryAsync("USP_CHANGE_PASSWORD", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> ForgotPasswordAsync(ForgotPassword forgotpass)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "FP");
            p.Add("@P_USERNAME", forgotpass.USERNAME);
            p.Add("@P_NEWPASSWORD", forgotpass.NEWPASSWORD);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_CHANGE_PASSWORD", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }

        public async Task<int> InsertOTPTracker(OtpTracker otp)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ACTIONCODE", "I");
                p.Add("@P_USERNAME", otp.USERNAME);
                p.Add("@P_USERID", otp.USERID);
                p.Add("@P_MOBILENO", otp.MOBILENO);
                p.Add("@P_OTP", otp.OTP);
                p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
                _ = await Connection.ExecuteAsync("USP_CHANGE_PASSWORD", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<Int32>("@MsgOut");
                return Convert.ToInt32(returnVal);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<OtpTracker>> ValideOtp(OtpTracker otp)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "OTP");
            p.Add("@P_USERNAME", otp.USERNAME);
            p.Add("@P_OTP", otp.OTP);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryAsync<OtpTracker>("USP_CHANGE_PASSWORD", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
    }
}
