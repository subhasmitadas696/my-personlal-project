using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Model.Entities.Common;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Localization;
#pragma warning disable SCS0016, CS1998
namespace CTMS.Web
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _AuthRepository;
        private readonly IWebHostEnvironment _env;
        public AuthenticationController(IAuthenticationRepository AuthRepository, IWebHostEnvironment env)
        {
            _AuthRepository = AuthRepository;
            _env = env;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn()
        {
            try
            {
                this.Request.HttpContext.Session.Clear();
                await HttpContext.SignOutAsync(SysManageAuthAttribute.SysManageAuthScheme);
                return View();
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "Loginload", Path.Combine(_env.WebRootPath, "Log"));
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginDto user)
        {
            try
            {
                string captcha = AESEncrytDecry.DecryptStringAES(user.captcha!);
                if (!GenerateCaptcha.ValidateCaptchaCode(captcha, HttpContext))
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid Captcha, please re-enter!", data = 300 });
                }
                else
                {
                    int retVal = 0;
                    user.USERNAME = AESEncrytDecry.DecryptStringAES(user.USERNAME!).ToString();
                    user.UPASSWORD = AESEncrytDecry.DecryptStringAES(user.UPASSWORD!).ToString();
                    var loginDetail = _AuthRepository.Login(user, out retVal);
                    if (loginDetail != null)
                    {
                        string UserEnteredPassword = SHA512Hash.SHa512(user.UPASSWORD!);
                        if (loginDetail.Password == UserEnteredPassword)
                        {
                            await generateClaim(loginDetail);
                            return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Sign in successful.", data = 200 });
                        }
                        else
                        {
                            return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid Password !", data = 101 });
                        }
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid Username !" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TroupeLogIn()
        {
            try
            {
                this.Request.HttpContext.Session.Clear();
                await HttpContext.SignOutAsync(SysManageAuthAttribute.SysManageAuthScheme);
                return View();
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "Loginload", Path.Combine(_env.WebRootPath, "Log"));
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> TroupLogIn(LoginDto user)
        {
            try
            {
                int retVal = 0;
                var loginDetail = _AuthRepository.TroupeLogin(user, out retVal);
                if (loginDetail != null)
                {
                    await generateClaimTroupe(loginDetail);
                    if (loginDetail.ROLEID == 5)
                    {
                        return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Sign in successful", data = 200 });
                    }
                    else
                    {
                        return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Something Went Wrong !", data = 400 });
                    }
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid OTP !" }.ToJson());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        [HttpGet]
        public IActionResult GetAuthCode()
        {
            try
            {
                string code = "";
                MemoryStream ms = new CaptchaHelper().Create(out code);
                WebHelper.WriteSession(ConstParameters.VerifyCodeKeyName, Md5Hash.Md5(code.Replace(" ", "").ToString(), 16));
                Response.Body.Dispose();
                return File(ms.ToArray(), @"image/png");
            }
            catch(Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        [HttpGet]
        public IActionResult GetAuthCode1()
        {
            try
            {
                string code = "";
                MemoryStream ms = new CaptchaHelper().Create1(out code);
                WebHelper.WriteSession(ConstParameters.VerifyCodeKeyName, Md5Hash.Md5(code.Replace(" ", "").ToString(), 16));
                Response.Body.Dispose();
                return File(ms.ToArray(), @"image/png");
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerifyUserMobile(VerifyUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage));
                    return Json(new { state = ResultType.error.ToString(), message = message, responseText = "Model State is invalid", data = "" });
                }
                if (WebHelper.GetSession(ConstParameters.VerifyCodeKeyName).IsEmpty()
                   || Md5Hash.Md5(user.captcha!.ToString(), 16) != WebHelper.GetSession(ConstParameters.VerifyCodeKeyName).ToString())
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid Captcha, please re-enter!",data=300 });
                }
                int returnVal = 0;
                var userDetail = _AuthRepository.CheckUserName(user.MOBILENO, out returnVal);
                if (returnVal == 2)
                {
                    return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "User is not registered with us!" }.ToJson());
                }
                else
                {
                    if (userDetail != null)
                    {
                        string phone = userDetail.MOBILENO!;
                        string email = userDetail.Email!;
                        if (!string.IsNullOrEmpty(email))
                        {
                            return Json(new AjaxResult { state = ResultType.success.ToString(), message = "OTP sent to your registered mobile number " + phone.Substring(0, 3) + phone.Substring(phone.Length - 4).PadLeft(phone.Length, '*') + "<br/> & Email Id : " + email.Substring(0, 3) + email.Substring(email.Length - 4).PadLeft(email.Length, '*'), data = 200 });
                        }
                        else
                        {
                            return Json(new AjaxResult { state = ResultType.success.ToString(), message = "OTP sent to your registered mobile number " + phone.Substring(0, 3) + phone.Substring(phone.Length - 4).PadLeft(phone.Length, '*'), data = 200 });
                        }
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "User is not registered with us!" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "OtpSend", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerifyTroupeUserMobile(VerifyUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage));
                    return Json(new { state = ResultType.error.ToString(), message = message, responseText = "Model State is invalid", data = "" });
                }
                if (!GenerateCaptcha.ValidateCaptchaCode(user.captcha!, HttpContext))
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Invalid Captcha, please re-enter!", data = 300 });
                }
                else
                {
                    int returnVal = 0;
                    var userDetail = _AuthRepository.CheckUserName(user.MOBILENO, out returnVal);
                    if (returnVal == 2)
                    {
                        return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "User is not registered with us!" }.ToJson());
                    }
                    else
                    {
                        if (userDetail != null)
                        {
                            string phone = userDetail.MOBILENO!;
                            string email = userDetail.Email!;
                            if (!string.IsNullOrEmpty(email))
                            {
                                return Json(new AjaxResult { state = ResultType.success.ToString(), message = "OTP sent to your registered mobile number " + phone.Substring(0, 3) + phone.Substring(phone.Length - 4).PadLeft(phone.Length, '*') + "<br/> & Email Id : " + email.Substring(0, 3) + email.Substring(email.Length - 4).PadLeft(email.Length, '*'), data = 200 });
                            }
                            else
                            {
                                return Json(new AjaxResult { state = ResultType.success.ToString(), message = "OTP sent to your registered mobile number " + phone.Substring(0, 3) + phone.Substring(phone.Length - 4).PadLeft(phone.Length, '*'), data = 200 });
                            }
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "User is not registered with us!" }.ToJson());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(ex, "OtpSend", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        public async Task generateClaim(UsersDto user)
        {
            var identity = new ClaimsIdentity(SysManageAuthAttribute.SysManageAuthScheme);  // Specify the authentication type
            List<Claim> claims = new List<Claim>(){
                            new Claim("MOBILENO",user.MOBILENO!.ToString()),
                            new Claim("RoleId" , user.ROLEID.ToString()),
                            new Claim(ClaimTypes.Role, user.ROLEID.ToString()),
                            new Claim("IsLeader", user.IsLeader.ToString()),
                            new Claim("Userid", user.USERID.ToString()),
                            new Claim("Uid", user.UID.ToString()),
                            new Claim("FULLNAME", user.FULLNAME !.ToString()),
                            new Claim("URTYPE",user.URTYPE !.ToString()),
                            new Claim("UserName",user.UserName !.ToString()),
                        };
            var isSystem = false;
            identity.AddClaims(claims);
            identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isSystem.ToString()));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(SysManageAuthAttribute.SysManageAuthScheme, principal);
        }
        public async Task generateClaimTroupe(UsersDto user)
        {
            var identity = new ClaimsIdentity(SysManageAuthAttribute.SysManageAuthScheme);  // Specify the authentication type
            List<Claim> claims = new List<Claim>(){
                            new Claim("TroupeId",user.TroupeId.ToString()),
                            new Claim("RoleId" , user.ROLEID.ToString()),
                            new Claim("UID",user.UID.ToString()),
                            new Claim("IsLeader", user.IsLeader.ToString()),
                            new Claim("TroupeName", user.TROUPENAME!),
                            new Claim("CategoryName", user.CategoryName!),
                            new Claim("SubCategoryName", user.SubCategoryName!),
                            new Claim("DistrictName", user.DISTRICTNAME!),
                            new Claim("BlockName", user.BLOCKNAME!),
                            new Claim("MobileNo", user.MOBILENO!),
                        };
            var isSystem = false;
            identity.AddClaims(claims);
            identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isSystem.ToString()));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(SysManageAuthAttribute.SysManageAuthScheme, principal);
        }

        [Authorize]
        public IActionResult ManageUser()
        {
            int intROLEID = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.Role));
            if (intROLEID == (int)CommonHelper.EnRoles.SuperAdmin)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.StateUser)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.DistrictUser)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.BlockUser)
            {
                return RedirectToAction("TroupeRegistrationBsso", "Registration");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.TroupeArtist)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.Administrator)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else if (intROLEID == (int)CommonHelper.EnRoles.Support)
            {
                return RedirectToAction("SessionOut", "Home");
            }
            else
            {
                return View("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OutLogin()
        {

            this.Request.HttpContext.Session.Clear();
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            await HttpContext.SignOutAsync(SysManageAuthAttribute.SysManageAuthScheme);
            if (!Response.Headers.ContainsKey("Cache-Control"))
            {
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1
            }
            if (!Response.Headers.ContainsKey("Pragma"))
            {
                Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0
            }
            if (!Response.Headers.ContainsKey("Expires"))
            {
                Response.Headers.Add("Expires", "0"); // Proxies
            }
            return RedirectToAction("LogIn", "Authentication");
        }
        [HttpGet]
        public async Task<IActionResult> TroupLogout()
        {

            this.Request.HttpContext.Session.Clear();
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            await HttpContext.SignOutAsync(SysManageAuthAttribute.SysManageAuthScheme);
            if (!Response.Headers.ContainsKey("Cache-Control"))
            {
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1
            }
            if (!Response.Headers.ContainsKey("Pragma"))
            {
                Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0
            }
            if (!Response.Headers.ContainsKey("Expires"))
            {
                Response.Headers.Add("Expires", "0"); // Proxies
            }
            return RedirectToAction("TroupeLogIn", "Authentication");
        }
        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult loginotp()
        {
            return View();
        }


        [Route("get-captcha-image")]
        public IActionResult GetImage()
        {
            try
            {
                int width = 90;
                int height = 25;

                var captchaCode = GenerateCaptcha.Captcha();
                var captchaByteData = GenerateCaptcha.GenerateCaptchaImage(width, height, captchaCode);

                HttpContext.Session.SetString("CaptchaCode", captchaCode);

                return File(captchaByteData, "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
