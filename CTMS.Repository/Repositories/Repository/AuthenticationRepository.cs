using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CTMS.Repository.Repository
{
    public class AuthenticationRepository : CTMSRepositoryBase, IAuthenticationRepository
    {
        public AuthenticationRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }

        public UsersDto? CheckUserName(string? mobileno, out int returnVal)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "A");
            p.Add("@P_MOBILENO", mobileno);
            p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            UsersDto? UserOtp = Connection.Query<UsersDto>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            returnVal = p.Get<int>("@P_MsgOut");
            return UserOtp;
        }
        public List<UsersDto>? M_CheckUserName(string? mobileno, out int returnVal)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "A");
            p.Add("@P_MOBILENO", mobileno);
            p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var UserOtp =Connection.Query<UsersDto>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure);
            returnVal = p.Get<int>("@P_MsgOut");
            return UserOtp?.ToList() ?? new List<UsersDto>();
        }
        public List<VOResponse>? Check_VerifyOfficerNumber(string? mobileno, out int returnVal)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "GO");
            p.Add("@P_MOBILENO", mobileno);
            p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var UserOtp =Connection.Query<VOResponse>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure);
            returnVal = p.Get<int>("@P_MsgOut");
            return UserOtp?.ToList() ?? new List<VOResponse>();
        }
        public List<VerifyOtp_Reviewer>? VerifyOtpReviewer(string? mobileno, string? otp,string? TYPE)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "VO");
                p.Add("@P_MOBILENO", mobileno);
                p.Add("@P_TYPE", TYPE);
                p.Add("@P_OTP", otp);
                p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var UserOtp =Connection.Query<VerifyOtp_Reviewer>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure);
                return UserOtp?.ToList() ?? new List<VerifyOtp_Reviewer>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public UsersDto? Login(LoginDto Log, out int returnVal)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("@ActionCode", "V");
            p.Add("@UserName", Log.USERNAME);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            UsersDto? userDetails = Connection.Query<UsersDto>("USP_USER_LOGIN", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            returnVal = p.Get<int>("MsgOut");
            return userDetails;
        }
        public List<UsersDto>? M_Login(LoginDto Log, out int returnVal)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("@ActionCode", "V");
            p.Add("@UserName", Log.USERNAME);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var  userDetails = Connection.Query<UsersDto>("USP_USER_LOGIN", p, commandType: CommandType.StoredProcedure);
            returnVal = p.Get<int>("MsgOut");
            return userDetails.ToList();
        }
        public UsersDto? TroupeLogin(LoginDto Log, out int returnVal)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "V");
                p.Add("@P_MOBILENO", Log.MOBILENO);
                p.Add("@P_OTP", Log.OTP);
                p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                UsersDto? userDetails = Connection.Query<UsersDto>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                returnVal = p.Get<int>("@P_MsgOut");
                return userDetails;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public List<UsersDto>? M_TroupeLogin(LoginDto Log, out int returnVal)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "V");
                p.Add("@P_MOBILENO", Log.MOBILENO);
                p.Add("@P_OTP", Log.OTP);
                p.Add("@P_MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var userDetails = Connection.Query<UsersDto>("USP_USERLOGIN_VERIFYUSER", p, commandType: CommandType.StoredProcedure);
                returnVal = p.Get<int>("@P_MsgOut");
                return userDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<int> UpdateFailedCount(string? regno, string? action)
        {
            // Create the parameters
            var p = new DynamicParameters();
            p.Add("ActionCode", action);
            p.Add("UserName", regno);
            p.Add("MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
            _ = await Connection.ExecuteAsync("USP_USER_LOGIN", p, commandType: CommandType.StoredProcedure);
            int returnVal = p.Get<int>("MsgOut");
            return returnVal;
        }
        public async Task<List<GpDetails>> GpDetails(int USERID)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("ActionCode", "GP");
                p.Add("UserName", USERID, DbType.Int32);
                p.Add("MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var GpList = await Connection.QueryAsync<GpDetails>("USP_USER_LOGIN", p, commandType: CommandType.StoredProcedure);
                int returnVal = p.Get<int>("MsgOut");
                return GpList.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Reviewer_SubmitReport(Reviewer_SubmitRequest eventDetails)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "TV");
                p.Add("@P_EventId", eventDetails.EventId);
                p.Add("@P_Eventname", eventDetails.EventName);
                p.Add("@P_Troupeid", eventDetails.TroupeId);
                p.Add("@P_Eventstartdate", eventDetails.EventStartDate);
                p.Add("@P_Eventenddate", eventDetails.EventEndDate);
                p.Add("@P_Dateofperformance", eventDetails.DateOfPerformance);
                p.Add("@VerifyOfficerPhoto", eventDetails.ImgPath);
                p.Add("@VerifyOfficerVideo", eventDetails.Videopath);
                p.Add("@P_CreatedBy", eventDetails.CreatedBy);
                //p.Add("@P_CreatedBy", eventDetails.EventDetails.Latitude);
                //p.Add("@P_CreatedBy", eventDetails.EventDetails.Longitude);
                // p.Add("@P_Starttime", eventDetails.EventDetails.EventStartTime);
                //p.Add("@P_Endtime", eventDetails.EventDetails.EventEndTime);
                p.Add("P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                Connection.Execute("USP_ASSIGN_TROUPE_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal =p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
