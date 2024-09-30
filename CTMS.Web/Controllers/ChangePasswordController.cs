using CTMS.Core;
using CTMS.Model.Entities.ChangePassword;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Web.Areas.AdminConsole.Models.SetPermission;
using CTMS.Web.Areas.AdminConsole.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace CTMS.Web.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IChangePasswordRepository _ChangepasswordRepository;
        private readonly IAuthenticationRepository _AuthRepository;
        public ChangePasswordController(IChangePasswordRepository ChangepasswordRepository, IWebHostEnvironment hostingEnvironment, IAuthenticationRepository authRepository)
        {
            _ChangepasswordRepository = ChangepasswordRepository;
            _hostingEnvironment = hostingEnvironment;
            _AuthRepository = authRepository;
        }
        public IActionResult ChangePassword()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var usernameClaim = identity.FindFirst("UserName");
            var useridClaim = identity.FindFirst("Userid");
            ViewBag.UserName = usernameClaim.Value;
            ViewBag.UserId = useridClaim.Value;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changepass)
        {
            try
            {
                string OLDPASSWORD = AESEncrytDecry.DecryptStringAES(changepass.OLDPASSWORD!).ToString();
                string NEWPASSWORD = AESEncrytDecry.DecryptStringAES(changepass.NEWPASSWORD!).ToString();
                changepass.OLDPASSWORD = SHA512Hash.SHa512(OLDPASSWORD);
                changepass.NEWPASSWORD = SHA512Hash.SHa512(NEWPASSWORD);
                var result = await _ChangepasswordRepository.ChangePasswordAsync(changepass);
                if (result == 1)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Password Upadated Successfully.", data = 200 });
                }
                else if (result == 2)
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Your Current Password Do not Match.", data = 101 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Something went wrong. Plese try again.", data = 101 });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ValidUser(OtpTracker otp)
        {
            try
            {
                var result = await _ChangepasswordRepository.ValideOtp(otp);
                if (result.Count== 1)
                {
                    int forgetotp = 0;
                    otp.USERNAME = result[0].USERNAME;
                    otp.USERID = result[0].USERID;
                    otp.MOBILENO = result[0].MOBILENO;
                    Random ran = new Random();
                    forgetotp = ran.Next(1000, 9999);
                    otp.OTP = forgetotp.ToString();
                    var returnVal = await _ChangepasswordRepository.InsertOTPTracker(otp);
                    return Json(new AjaxResult { state = ResultType.success.ToString(), data = otp.MOBILENO, data1 = otp.OTP });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid User Name.", data = 101 });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpPost] 
        public async Task<IActionResult> ValidOTP(string OTP, string MOBILENO)
        {
            try
            {
                OtpTracker otp =new OtpTracker();  
                otp.MOBILENO=MOBILENO;
                otp.OTP=OTP;
                var result =await  _ChangepasswordRepository.OTPAsync(otp);
                if (result == 8)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Please Enter Valid OTP.", data = 101 });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotpass)
        {
            try
            {
                string NEWPASSWORD = AESEncrytDecry.DecryptStringAES(forgotpass.NEWPASSWORD!).ToString();
                forgotpass.NEWPASSWORD = SHA512Hash.SHa512(NEWPASSWORD);

                var result = await _ChangepasswordRepository.ForgotPasswordAsync(forgotpass);
                if (result == 3)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "New Password Set Successfully.", data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Something went wrong. Plese try again.", data = 101 });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
    }
}
