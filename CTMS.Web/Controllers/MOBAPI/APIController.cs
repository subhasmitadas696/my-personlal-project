using AngleSharp.Text;
using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Model.Entities.Approval;
using CTMS.Model.Entities.Common;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.ImageList;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CTMS.Web.Controllers.MOBAPI
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class APIController : ControllerBase
    {
        private readonly IMasterRepository _masterService;
        private readonly IAuthenticationRepository _AuthRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IRegistraionRepository _RegistraionRepository;
        private readonly IEventRepository _EventRepository;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        readonly ApiResponse response = new ApiResponse();
        public APIController(IMasterRepository masterService, IAuthenticationRepository AuthRepository, IApprovalConfigRepository ApprovalConfigRepository, IEventRepository EventRepository, IWebHostEnvironment env, IRegistraionRepository RegistraionRepository)
        {
            _AuthRepository = AuthRepository;
            _env = env;
            _masterService = masterService;
            _EventRepository = EventRepository;
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _RegistraionRepository = RegistraionRepository;
        }
        //Method For Troupe Users In Mobile Application
        [HttpPost]
        public async Task<IActionResult> Get_Otp([FromBody] VerifyUser user)
        {
            try
            {
                int returnVal = 0;
                var userDetail = _AuthRepository.M_CheckUserName(user.MOBILENO, out returnVal);
                if (returnVal == 2)
                {
                    response.Status = "Failed";
                    response.StatusCode = "400";
                    response.Message = "User is not registered with us!!!";
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    if (userDetail != null)
                    {
                        string email = userDetail[0].Email!;
                        response.Status = "Success";
                        response.StatusCode = "200";
                        response.Message = "OTP sent to your registered mobile number";
                        response.LoginResponse = userDetail;
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return !string.IsNullOrEmpty(email) ? Ok(troupe) : (IActionResult)Ok(troupe);
                    }
                    else
                    {
                        response.Status = "Failed";
                        response.StatusCode = "400";
                        response.Message = "User is not registered with us!";
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Error in Sending Otp!";
                CommonHelper.LogError(ex, "OtpSend", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method For verify Troupe Users and Get Troupe Details In Mobile Application
        [HttpPost]
        public async Task<IActionResult> Verify_Otp([FromBody] LoginDto user)
        {
            try
            {
                int retVal = 0;
                var loginDetail = _AuthRepository.M_TroupeLogin(user, out retVal);
                if (loginDetail.Count != 0)
                {
                    response.Status = "Success";
                    response.StatusCode = "200";
                    response.Message = "LogIn Successful";
                    response.LoginResponse = loginDetail;
                    if (loginDetail[0].ROLEID == 5)
                    {
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                    else
                    {
                        response.Status = "Failed";
                        response.StatusCode = "400";
                        response.Message = "Something Went Wrong!!";
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                }
                else
                {
                    response.Status = "Failed";
                    response.StatusCode = "400";
                    response.Message = "Invalid OTP!!";
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "LogIn Failed";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Methods For Blocklogin(Adminstrative Login) and get GramPanchayat details.
        [HttpPost]
        public async Task<IActionResult> BlockLogin([FromBody] LoginDto user)
        {
            try
            {
                int retVal = 0;
                var loginDetail = _AuthRepository.M_Login(user, out retVal);
                if (loginDetail!.Count != 0 && loginDetail != null)
                {
                    string UserEnteredPassword = SHA512Hash.SHa512(user.UPASSWORD!);
                    if (loginDetail[0].Password == UserEnteredPassword)
                    {
                        response.Status = "Success";
                        response.StatusCode = "200";
                        response.Message = "Sign in Successful";
                        List<GpDetails> gpDetailsResult = await _AuthRepository.GpDetails(loginDetail[0].USERID)!;
                        if (gpDetailsResult != null)
                        {
                            List<GpDetails> usersDtos = gpDetailsResult.ToList();
                            response.GpDetails = usersDtos;
                        }
                        response.LoginResponse = loginDetail;
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "\"\"");
                        return Ok(troupe);
                    }
                    else
                    {
                        response.Status = "Failed";
                        response.StatusCode = "400";
                        response.Message = "Invalid Password";
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                }
                else
                {
                    response.Status = "Failed";
                    response.StatusCode = "400";
                    response.Message = "User is not registered with us!";
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "LogIn Failed";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Adminstrative login Method For Users
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginDto user)
        {
            try
            {
                int retVal = 0;
                var loginDetail = _AuthRepository.M_Login(user, out retVal);
                if (loginDetail != null)
                {
                    string UserEnteredPassword = SHA512Hash.SHa512(user.UPASSWORD!);
                    if (loginDetail[0].Password == UserEnteredPassword)
                    {
                        response.Status = "Success";
                        response.StatusCode = "200";
                        response.Message = "Sign in Successful";
                        response.LoginResponse = loginDetail;
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");

                        return Ok(troupe);
                    }
                    else
                    {
                        response.Status = "Failed";
                        response.StatusCode = "400";
                        response.Message = "Invalid Password";
                        string troupe = JsonConvert.SerializeObject(response);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                }
                else
                {
                    response.Status = "Failed";
                    response.StatusCode = "400";
                    response.Message = "User is not registered with us!";
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "LogIn Failed";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //According to TroupeId TroupeDetails and Member Details.
        [HttpPost]
        public IActionResult ProfileDetails(string Troupeid = "")
        {
            try
            {
                var TroupeDetails = _RegistraionRepository.M_TroupeDetails(Troupeid);
                var MemberDetails = _RegistraionRepository.M_MemberDetails(Troupeid).Result.ToList();
                response.Status = "Success";
                response.StatusCode = "200";
                response.Message = "Troupe and Member Details Fetched Successfuly";
                response.TroupeRegistrationView = TroupeDetails;
                response.MemberDetailsView = MemberDetails;
                string troupe = JsonConvert.SerializeObject(response);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch ProfileDetails";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        //Method To Fetch All Master Data(District,Block,Gp,Village,Department,Scheme,TroupeMaster,Category,Subcategory).
        [HttpPost]
        public IActionResult GetMasterData(string BlockId, string UserId)
        {
            Mobilemaster MR = new Mobilemaster();
            try
            {
                var category = _RegistraionRepository.M_Category_Master();
                var subcategory = _RegistraionRepository.M_SubCategory_Master();
                var GP = _RegistraionRepository.M_GP_Master(BlockId);
                var Village = _RegistraionRepository.M_Village_Master(BlockId);
                var Department = _RegistraionRepository.M_Department_Master();
                var Scheme = _RegistraionRepository.M_Scheme_Master();
                var Troupe = _RegistraionRepository.M_Troupe_Master();
                MR.Status = "Success";
                MR.StatusCode = "200";
                MR.Message = "Master Data Fetched Successfuly";
                MR.categorylist = category;
                MR.subcategorylist = subcategory;
                MR.Troupelist = Troupe;
                MR.GPlist = GP;
                MR.Departmentlist = Department;
                MR.Villagelist = Village;
                MR.Schemelist = Scheme;
                string troupe = JsonConvert.SerializeObject(MR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                MR.Status = "Failed";
                MR.StatusCode = "400";
                MR.Message = "Failed to fetch Master Data";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to Create/Update  Event.
        [HttpPost]
        public async Task<IActionResult> Event_Creation([FromBody] EventDetails ED)
        {
            try
            {
                int retval = await _EventRepository.M_ManageEventDetails(ED);
                if (retval == 1)
                {
                    response.Status = "Success";
                    response.StatusCode = "200";
                    response.Message = "Event Created Successfuly";
                    response.EventReturnValue = retval;
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "\"\"");
                    return Content(troupe);
                }
                else if (retval == 2)
                {
                    response.Status = "Success";
                    response.StatusCode = "200";
                    response.Message = "Event Updated Successfuly";
                    response.EventReturnValue = retval;
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "\"\"");
                    return Content(troupe);
                }
                else
                {
                    response.Status = "Failed";
                    response.StatusCode = "400";
                    response.Message = "Error In Adding Troupe Details";
                    string troupe = JsonConvert.SerializeObject(response);
                    troupe = troupe.Replace("null", "\"\"");
                    return Content(troupe);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to Insert Event";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to Get Status of Troupe According to acknowledgmentNumber
        [HttpPost]
        public async Task<IActionResult> Search_by_acknowledgement_number(string? acknowledgmentNumber)
        {
            try
            {
                var res = await _RegistraionRepository.GetAllDetailsByAckNo(acknowledgmentNumber);
                response.Status = "Success";
                response.StatusCode = "200";
                response.Message = "Troupe registration details fetched successfully";
                response.ApplicationStatusByAckNo = res;
                string troupe = JsonConvert.SerializeObject(response);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Troupe registration details ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to get all details(Count,Details) in Block According to BlockId
        [HttpPost]
        public async Task<IActionResult> Block_Dashboard_details(string BlockId, string UserId)
        {
            BlockDetailsResponse BR = new BlockDetailsResponse();
            try
            {
                DashboardDetails DD = new DashboardDetails();
                DD.userid = BlockId.ToInt();
                List<DashboardDetails> result = await _ApprovalConfigRepository.DashboardDetails(DD);
                var DashboardCount = await _ApprovalConfigRepository.Dashboardcount(BlockId!);
                BR.Status = "Success";
                BR.StatusCode = "200";
                BR.Message = "Block details fetched successfully";
                BR.DashboardCount = DashboardCount;
                BR.DashboardDetails = result;
                string troupe = JsonConvert.SerializeObject(BR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Block details";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Event All Details Pending at Block User
        [HttpPost]
        public async Task<IActionResult> Block_Event_List(string BlockId, string UserId)
        {
            EventDetailsResponse BR = new EventDetailsResponse();
            try
            {
                var data = _ApprovalConfigRepository.ViewEventDetails_MOB("TES", BlockId.ToInt());
                BR.Status = "Success";
                BR.StatusCode = "200";
                BR.Message = "Block details fetched successfully";
                BR.EventDetails = data;
                string troupe = JsonConvert.SerializeObject(BR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Block details";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Block Profile According to Troupe Details and respective member details.
        [HttpPost]
        public async Task<IActionResult> Block_Profile(string BlockId, string UserId)
        {
            Block_Profile_Response BR = new Block_Profile_Response();
            List<Block_Profile_Response_lIst> BPRL = new List<Block_Profile_Response_lIst>();
            try
            {
                TroupeRegistrationView TRV = new TroupeRegistrationView();
                TRV.BlockId = BlockId.ToInt();
                TRV.DistrictId = 0;
                TRV.GPId = 0;
                MemberDetailsView dataModel = new MemberDetailsView();
                var TroupeDetails = _ApprovalConfigRepository.ViewNewRegistration(TRV).Result.ToList();
                List<TroupeRegistrationView> TRVL = new List<TroupeRegistrationView>();
                Block_Profile_Response_lIst BPR = new Block_Profile_Response_lIst(); // Create a new instance for each iteration
                foreach (var data in TroupeDetails)
                {
                    dataModel.TroupeId = data.TroupeId;
                    data.MemberDetails = await _ApprovalConfigRepository.MemberDetails(dataModel);
                    TRVL.Add(data);
                }
                BPR.TroupeRegistrationDetails = TRVL;
                BPRL.Add(BPR);
                BR.Block_Profile_Response_lIst = BPRL;
                BR.Status = "Success";
                BR.StatusCode = "200";
                BR.Message = "Block Profile Fetched Successfully";
                string troupe = JsonConvert.SerializeObject(BR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Block Profile";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to Get Count and details of(TroupeRegistrationPendingcount,eventReportPendingCount,eventReportCompletedCount)
        [HttpPost]
        public async Task<IActionResult> Block_PendingCount_List(string BlockId, string UserId)
        {
            BlockPendingList_Response BR = new BlockPendingList_Response();
            try
            {
                TroupeRegistrationView TRV = new TroupeRegistrationView();
                TRV.BlockId = BlockId.ToInt();
                TRV.DistrictId = 0;
                TRV.GPId = 0;
                var TroupeRegistrationPendingcount = _ApprovalConfigRepository.ApprovalView_Mob(TRV).Result.ToList();
                BR.troupeRegistrationPendingCount = TroupeRegistrationPendingcount.Count().ToString();
                var eventReportPendingCount = _EventRepository.ViewEventDetails(0, "E", BlockId.ToInt(), "", "");
                BR.eventReportPendingCount = eventReportPendingCount.Count().ToString();
                var eventReportCompletedCount = await _ApprovalConfigRepository.completedEventsList_MOB(BlockId);
                BR.eventReportCompletedCount = eventReportCompletedCount.Count().ToString();
                BR.Status = "Success";
                BR.StatusCode = "200";
                BR.Message = "Pending Activities Fetched Successfully";
                BR.TroupeRegistrationpendingList = TroupeRegistrationPendingcount;
                BR.eventReportPendingList = eventReportPendingCount;
                BR.eventReportCompletedList = eventReportCompletedCount;
                string troupe = JsonConvert.SerializeObject(BR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Pending Activities";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Events Assigned to Troupe According to troupeid.
        [HttpPost]
        public async Task<IActionResult> Events_for_Troupe(string? Troupeid)
        {
            TroupeEventResponse TR = new TroupeEventResponse();
            try
            {
                TroupeRegistrationView TRV = new TroupeRegistrationView();
                TRV.TroupeId = Troupeid!.ToInt();
                var EventstobePerformed = _RegistraionRepository.TroupeReportingDetails(TRV).Result.ToList();
                var TroupeReportedSuccessful = _RegistraionRepository.TroupeReportingSuccess(TRV).Result.ToList();
                var EventsPaymentSuccessful = _RegistraionRepository.TroupeEventDetails_Mob(TRV).Result.ToList();
                var data = _ApprovalConfigRepository.ViewEventDetails_Troupe("TED", Troupeid!.ToInt());
                TR.Status = "Success";
                TR.StatusCode = "200";
                TR.Message = "Events for Troupe fetched successfully";
                TR.EventDetails_Troupe = data;
                TR.EventsToBePerformedToTroupe = EventstobePerformed;
                TR.EventsPerformedSuccessful = TroupeReportedSuccessful;
                TR.EventsPaymentSuccessful = EventsPaymentSuccessful;
                string troupe = JsonConvert.SerializeObject(TR);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Events for Troupe!!! ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Troupe DashBoard Count(activeMembers,totalPerformed)
        [HttpPost]
        public async Task<IActionResult> Troup_Dashboard_Count(string Troupeid)
        {
            try
            {
                var data = _ApprovalConfigRepository.Troup_Dashboard_Count("TDC", Troupeid.ToInt());
                response.Status = "Success";
                response.StatusCode = "200";
                response.Message = "Troupe dashboard count fetched successfully";
                response.activeMembers = data[0].activeMembers;
                response.totalPerformed = data[0].totalPerformed;
                string troupe = JsonConvert.SerializeObject(response);
                troupe = troupe.Replace("null", "\"\"");
                return Content(troupe);
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.StatusCode = "400";
                response.Message = "Failed to fetch Troupe registration details ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //get Otp fOr reviewer Login
        [HttpPost]
        public async Task<IActionResult> Get_Otp_Reviewer([FromBody] VerifyUser user)
        {
            VerifyOfficerDetails VD = new VerifyOfficerDetails();
            try
            {
                int returnVal = 0;
                var userDetail = _AuthRepository.Check_VerifyOfficerNumber(user.MOBILENO, out returnVal);
                if (returnVal == 2)
                {
                    VD.Status = "Failed";
                    VD.StatusCode = "400";
                    VD.Message = "User is not registered with us!!!";
                    string troupe = JsonConvert.SerializeObject(VD);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    if (userDetail != null)
                    {
                        VD.Status = "Success";
                        VD.StatusCode = "200";
                        VD.Message = "OTP sent to your registered mobile number";
                        VD.VOdetails = userDetail;
                        string troupe = JsonConvert.SerializeObject(VD);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                    else
                    {
                        VD.Status = "Failed";
                        VD.StatusCode = "400";
                        VD.Message = "User is not registered with us!";
                        string troupe = JsonConvert.SerializeObject(VD);
                        troupe = troupe.Replace("null", "[]");
                        return Ok(troupe);
                    }
                }
            }
            catch (Exception ex)
            {
                VD.Status = "Failed";
                VD.StatusCode = "400";
                VD.Message = "Error in Sending Otp!";
                CommonHelper.LogError(ex, "OtpSend", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Verify Otp fOr reviewer Login and get Village Details Assigned to him According to mobile number("L"/"S")
        [HttpPost]
        public async Task<IActionResult> Verify_Otp_Reviewer([FromBody] VerifyOfficerRequest vr)
        {
            VerifyOtp_Reviewer_Response VR = new VerifyOtp_Reviewer_Response();
            try
            {
                var loginDetail = _AuthRepository.VerifyOtpReviewer(vr.mobileno, vr.otp, vr.TYPE)!;
                if (loginDetail.Count != 0)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Otp Verified Successfully";
                    VR.ListEventdetails = loginDetail;

                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "Something Went Wrong!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Otp Verification Failed";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method For Reviewer To Upload 9 Photos and 3 videos against villages assigned to him in base64 string format.
        //[HttpPost]
        //public async Task<IActionResult> Reviewer_SubmitReport_Base64([FromBody] Reviewer_SubmitRequest RSQ)
        //{
        //    Reviewer_SubmitResponse VR = new Reviewer_SubmitResponse();
        //    try
        //    {
        //        var imgfilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //        string Verifyofficerdetails = "VerifyOfficerDetails";
        //        string VOPhotos = "VerifyOfficerPhotos";
        //        string VOallvideos = "VerifyOfficerVideos";
        //        string webrootpath = _env.WebRootPath;
        //        string Verifyofficerdetailspath = Path.Combine(webrootpath, Verifyofficerdetails);
        //        string verifyofficertroupeid = Path.Combine(Verifyofficerdetailspath, RSQ.TroupeId!);
        //        string VOTroupePhotos = Path.Combine(verifyofficertroupeid, VOPhotos);
        //        string VOTroupeallVideos = Path.Combine(verifyofficertroupeid, VOallvideos);
        //        if (!Directory.Exists(Verifyofficerdetailspath))
        //            Directory.CreateDirectory(Verifyofficerdetailspath);
        //        if (!Directory.Exists(verifyofficertroupeid))
        //            Directory.CreateDirectory(verifyofficertroupeid);
        //        if (!Directory.Exists(VOTroupePhotos))
        //            Directory.CreateDirectory(VOTroupePhotos);
        //        if (!Directory.Exists(VOTroupeallVideos))
        //            Directory.CreateDirectory(VOTroupeallVideos);
        //        string retval = RSQ.EventDetails![0].path_photo!;
        //        string retvalvdo = RSQ.EventDetails[0].path_video!;
        //        foreach (var i in RSQ.EventDetails)
        //        {
        //            string base64String = i.Photo!;
        //            byte[] imageBytes = Convert.FromBase64String(base64String);
        //            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        //            {
        //                IFormFile imageFile = new FormFile(memoryStream, 0, memoryStream.Length, "image", imgfilename);
        //                var UploadFolderVOP = i.VillageId!;
        //                string TroupeDocPath = Path.Combine(VOTroupePhotos, UploadFolderVOP);
        //                if (!Directory.Exists(TroupeDocPath))
        //                    Directory.CreateDirectory(TroupeDocPath);
        //                var photo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //                string TroupeDocPath12 = Path.Combine(TroupeDocPath, photo);
        //                if (!Directory.Exists(TroupeDocPath12))
        //                    Directory.CreateDirectory(TroupeDocPath12);
        //                var photoname = i.Photo_name.Replace(i.Photo_name!, TroupeDocPath12);
        //                string filePath = Path.Combine(TroupeDocPath, photoname! + ".png");
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await imageFile.CopyToAsync(stream);
        //                }
        //                if (retval == null)
        //                {
        //                    retval = UploadFolderVOP + "_" + RSQ.TroupeId! + "|" + photo! + ".png";
        //                }
        //                else
        //                {
        //                    retval = retval! + "~" + UploadFolderVOP + "_" + RSQ.TroupeId! + "|" + photo! + ".png";
        //                }
        //            }
        //        }
        //        foreach (var i in RSQ.EventDetails)
        //        {
        //            string base64String = i.Video!;
        //            if (base64String != "" && base64String != null)
        //            {
        //                byte[] imageBytes = Convert.FromBase64String(base64String);
        //                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        //                {
        //                    var contentType = "video/mp4";
        //                    IFormFile imageFile = new FormFile(memoryStream, 0, memoryStream.Length, contentType, imgfilename);
        //                    var UploadFolderVOP = i.VillageId!;
        //                    string villagevideos = Path.Combine(VOTroupeallVideos, UploadFolderVOP);
        //                    if (!Directory.Exists(villagevideos))
        //                        Directory.CreateDirectory(villagevideos);
        //                    var photo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //                    string TroupeDocPath12 = Path.Combine(villagevideos, photo);
        //                    if (!Directory.Exists(TroupeDocPath12))
        //                        Directory.CreateDirectory(TroupeDocPath12);
        //                    var video_name = i.video_name.Replace(i.video_name!, TroupeDocPath12);
        //                    string filePath = Path.Combine(villagevideos, video_name! + ".mp4");
        //                    using (var stream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await imageFile.CopyToAsync(stream);
        //                    }
        //                    if (retvalvdo == null)
        //                    {
        //                        retvalvdo = UploadFolderVOP + "_" + RSQ.TroupeId! + "|" + photo! + ".mp4";
        //                    }
        //                    else
        //                    {
        //                        retvalvdo = retvalvdo! + "~" + UploadFolderVOP + "_" + RSQ.TroupeId! + "|" + photo! + ".mp4";
        //                    }
        //                }
        //            }

        //        }
        //        RSQ.EventDetails[0].Photo_name = retval;
        //        RSQ.EventDetails[0].video_name = retvalvdo;
        //        var loginDetail = _AuthRepository.Reviewer_SubmitReport(RSQ)!;
        //        if (loginDetail == "1")
        //        {
        //            VR.Status = "Success";
        //            VR.StatusCode = "200";
        //            VR.Message = "Reviewer ReportedFor Event Successfully";
        //            VR.ReviewerSubmitStatus = loginDetail.ToInt();
        //            string troupe = JsonConvert.SerializeObject(VR);
        //            troupe = troupe.Replace("null", "[]");
        //            return Ok(troupe);
        //        }
        //        else
        //        {
        //            VR.Status = "Failed";
        //            VR.StatusCode = "400";
        //            VR.Message = "Failed to Submit!!!Something Went Wrong!!";
        //            string troupe = JsonConvert.SerializeObject(VR);
        //            troupe = troupe.Replace("null", "[]");
        //            return Ok(troupe);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VR.Status = "Failed";
        //        VR.StatusCode = "400";
        //        VR.Message = "Failed to Submit Reviewer Report";
        //        CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
        //        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
        //    }
        //}
        //List Of Events Pending at BSSO For Assign Reporting
        [HttpPost]
        public async Task<IActionResult> TroupeReportingForEvent(int TroupeId)
        {
            TroupeReportingForEvent_Response VR = new TroupeReportingForEvent_Response();
            try
            {
                TroupeRegistrationView TRV = new TroupeRegistrationView();
                TRV.TroupeId = TroupeId;
                var TroupeReportingDetails = await _RegistraionRepository.TroupeReportingDetails(TRV);
                if (TroupeReportingDetails.Count != 0)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Pending List For Troupe Reporting Fetched Successfully";
                    VR.TroupeReporting = TroupeReportingDetails;
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "No Pending List For Troupe Reporting Found !!Try with other troupeid!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed Due to technical Issue";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        //List Of Events Pending at BSSO For Assign Reporting With Villageid and village name
        [HttpPost]
        public async Task<IActionResult> TroupeReportingForEventGetDetails(int AssignEventId, int Status)
        {
            TroupeReportingForEvent_Response VR = new TroupeReportingForEvent_Response();
            try
            {
                TroupeReportingView TRV = new TroupeReportingView();
                TRV.AssignEventId = AssignEventId;
                var TroupeReportingDetails = await _RegistraionRepository.TroupeReporting(TRV);
                if (TroupeReportingDetails.Count != 0)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Pending List For Troupe Reporting Details Fetched Successfully";
                    VR.TroupeReporting = TroupeReportingDetails;
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "No Pending List For Troupe Reporting Details Found !!Try with other troupeid!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed Due to technical Issue";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        //Method For Troupe To Upload 9 Photos and 3 videos against Event assigned to him in multipart/form data format.
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> TroupeReporting_WithPhoto_Video([FromForm] TroupeReporting_Mobile TR)
        {
            TroupeReporting_Response VR = new TroupeReporting_Response();
            int iteration = 1;
            string fileName = string.Empty;
            string filePath = string.Empty;
            string villageAndTroupeId = string.Empty;
            string imagePathToStore = string.Empty;
            StringBuilder imagePathsBuilder = new StringBuilder();
            try
            {
                if (TR.TroupeId.ToString() != null && TR.TroupeId != 0)
                {
                    List<string> videoPaths = new List<string>();
                    string Mainflodername = "TroupeReporting";
                    string Subflodername = TR.TroupeId.ToString()!;
                    string webrootpath = _env.WebRootPath;
                    string ProcDocPath = Path.Combine(webrootpath, Mainflodername);
                    if (!Directory.Exists(ProcDocPath))
                        Directory.CreateDirectory(ProcDocPath);
                    string TroupeIdFolder = Path.Combine(ProcDocPath, Subflodername);
                    if (TR.TroupeReportPhotovideolist != null)
                    {
                        foreach (var items in TR.TroupeReportPhotovideolist!)
                        {
                            if (!Directory.Exists(TroupeIdFolder))
                                Directory.CreateDirectory(TroupeIdFolder);
                            var villageId = items.VillageId;
                            string ImageFolder = Path.Combine(TroupeIdFolder, "Image");
                            if (!Directory.Exists(ImageFolder))
                                Directory.CreateDirectory(ImageFolder);
                            string IdFolder = Path.Combine(ImageFolder, villageId + "_" + TR.TroupeId);
                            IFormFile? file = null;
                            for (int i = 1; i <= 3; i++) // Process UpldImg1, UpldImg2, UpldImg3 in groups of three
                            {
                                async Task<string> GetFileExtension(IFormFile? file)
                                {
                                    if (file != null && file.ContentType.Contains("image/"))
                                    {
                                        return "." + file.ContentType.Split('/')[1];
                                    }
                                    return "";
                                }
                                switch (i)
                                {
                                    case 1:
                                        string originalFileName = items.UpldImg1?.FileName!;
                                        string fileExtension = !string.IsNullOrEmpty(originalFileName) ? Path.GetExtension(originalFileName) : string.Empty;
                                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + TR.TroupeId + "_" + iteration + fileExtension;
                                        filePath = Path.Combine(IdFolder, fileName);
                                        if (!Directory.Exists(IdFolder))
                                            Directory.CreateDirectory(IdFolder);
                                        file = items.UpldImg1;
                                        break;
                                    case 2:
                                        string originalFileName1 = items.UpldImg2?.FileName!;
                                        string fileExtension1 = !string.IsNullOrEmpty(originalFileName1) ? Path.GetExtension(originalFileName1) : string.Empty;
                                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + TR.TroupeId + "_" + iteration + fileExtension1;
                                        filePath = Path.Combine(IdFolder, fileName);
                                        if (!Directory.Exists(IdFolder))
                                            Directory.CreateDirectory(IdFolder);
                                        file = items.UpldImg2;
                                        break;
                                    case 3:
                                        string originalFileName2 = items.UpldImg3?.FileName!;
                                        string fileExtension2 = !string.IsNullOrEmpty(originalFileName2) ? Path.GetExtension(originalFileName2) : string.Empty;
                                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + TR.TroupeId + "_" + iteration + fileExtension2;
                                        filePath = Path.Combine(IdFolder, fileName);
                                        if (!Directory.Exists(IdFolder))
                                            Directory.CreateDirectory(IdFolder);
                                        file = items.UpldImg3;
                                        break;
                                }
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file?.CopyToAsync(stream!);
                                }
                                villageAndTroupeId = $"{villageId}_{TR.TroupeId}";
                                imagePathToStore = $"{villageAndTroupeId}|{Path.GetRelativePath(IdFolder, filePath).Replace(Path.DirectorySeparatorChar, '|')}";
                                if (iteration == 1)
                                {
                                    imagePathsBuilder.Append(imagePathToStore);
                                }
                                else
                                {
                                    imagePathsBuilder.Append("~").Append(imagePathToStore);
                                }
                                iteration++;
                            }
                        }
                    }
                    TR.ImgIdPath = imagePathsBuilder.ToString();
                    if (TR.TroupeReportPhotovideolist != null)
                    {
                        int iterationval = 1;
                        foreach (var items in TR.TroupeReportPhotovideolist)
                        {
                            async Task<string> GetFileExtension(IFormFile? file)
                            {
                                if (file != null && file.ContentType.Contains("video/"))
                                {
                                    return "." + file.ContentType.Split('/')[1];
                                }
                                return "";
                            }
                            var villageId = items.VillageId;
                            string originalFileName = items.UpldVdo?.FileName!;
                            string fileExtension = !string.IsNullOrEmpty(originalFileName) ? Path.GetExtension(originalFileName) : string.Empty;
                            fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + TR.TroupeId + "_" + iterationval + fileExtension;
                            string VideoFolder = Path.Combine(TroupeIdFolder, "Video");
                            string IdFolder = Path.Combine(VideoFolder, villageId + "_" + TR.TroupeId);
                            if (!Directory.Exists(VideoFolder))
                                Directory.CreateDirectory(VideoFolder);
                            if (!Directory.Exists(IdFolder))
                                Directory.CreateDirectory(IdFolder);
                            filePath = Path.Combine(IdFolder, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await items.UpldVdo!.CopyToAsync(stream);
                            }
                            StringBuilder videoPathsBuilder = new StringBuilder();
                            villageAndTroupeId = $"{villageId}_{TR.TroupeId}";
                            string videopathToStore = $"{villageAndTroupeId}|{Path.GetRelativePath(IdFolder, filePath).Replace(Path.DirectorySeparatorChar, '|')}";
                            if (iterationval == 1)
                            {
                                videoPathsBuilder.Append(videopathToStore);
                            }
                            else
                            {
                                videoPathsBuilder.Append("~").Append(videopathToStore);
                            }
                            videoPaths.Add(videoPathsBuilder.ToString());
                            TR.VdoIdPath = string.Join("", videoPaths);
                            iterationval++;
                        }
                    }
                }
                string retvalues = await _RegistraionRepository.M_TroupeReportingToBlock(TR);
                if (retvalues == "4")
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Trouope Reported with photo and videos sucessfully";
                    VR.TroupeReportingStatus = retvalues;
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "Failed to Submit Troupe Reporting!!!Something Went Wrong!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed Due to technical Issue";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult
                {
                    state = ResultType.error.ToString(),
                    message = ex.Message
                }.ToJson());
            }
        }
        //Get Troupes According to CAtegory and Subcategory and blockid
        public async Task<IActionResult> GetTroupes_WRT_CatSubcat(int CatId = 0, int SubCatId = 0, int BlockId = 0)
        {
            GeTroupes_WRT_CatSubcatResponse VR = new GeTroupes_WRT_CatSubcatResponse();
            try
            {
                var objDistlist = await _EventRepository.M_ViewTroupes(CatId, SubCatId, BlockId);
                VR.Status = "Success";
                VR.StatusCode = "200";
                VR.Message = "List Of Troupe According to CatId, SubCatId,BlockId Fetched Successfully";
                VR.GetTroupes = objDistlist;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed Due to technical Issue";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to Get All Pending Troupe In BlockLogin(Pending,Approve,Reject,NCT)
        [HttpPost]
        public async Task<IActionResult> GetAllPendingTroupeForBlockApproval(string? BlockId, string? ApprovedStatus)
        {
            TroupeResponse VR = new TroupeResponse();
            try
            {
                TroupeApproval TRV = new TroupeApproval();
                TRV.ApprovedStatus = ApprovedStatus.ToInt();
                TRV.BlockId = BlockId.ToInt();
                var objDistlist = await _ApprovalConfigRepository.ApprovalView(TRV);
                VR.Status = "Success";
                VR.StatusCode = "200";
                VR.Message = "Pending Troupe List For Block Approval Fetched Successfully";
                VR.TroupeDetails = objDistlist;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Submit Reviewer Report";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to Get All Details to Approve/Reject Pending Troupe In BlockLogin(Pending,Approve,Reject,NCT)
        [HttpPost]
        public async Task<IActionResult> GetAllDetails_PendingTroupeForBlockApproval(string TroupeId, int Status)
        {
            TroupeApprovalViewResponse VR = new TroupeApprovalViewResponse();
            try
            {
                TroupeApproval TRV = new TroupeApproval();
                TRV.ApprovedStatus = Status.ToInt();
                TRV.TroupeId = TroupeId.ToInt();
                var objDistlist = await _ApprovalConfigRepository.M_ApprovalTakeActionView(TroupeId, Status);
                VR.Status = "Success";
                VR.StatusCode = "200";
                VR.Message = "Pending Troupe Details For Block Approval Fetched Successfully";
                VR.AllDetails = objDistlist;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Fetch Troupe Details";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //method to take action for Approve,reject and NCTA
        public async Task<IActionResult> Block_ApprovalTakeAction(ApprovalStatus Obj)
        {
            TroupeApprovalViewResponse VR = new TroupeApprovalViewResponse();
            try
            {
                var objresult = await _ApprovalConfigRepository.M_ApprovalTakeAction(Obj);
                VR.ApproveStatus = objresult;
                if (VR.ApproveStatus == 10)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Troupe Approved Successfully";
                }
                else if (VR.ApproveStatus == 11)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Troupe Rejected Successfully";
                }
                else if (VR.ApproveStatus == 12)
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Troupe Not Appeared In Audition!!!";
                }
                else
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Error In Troupe Approval!!!";
                }
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Approve/Reject Troupe Details";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method For Reviewer To Upload 9 Photos and 3 videos against villages assigned to him in base64 string format.
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Reviewer_SubmitReporting([FromForm] Reviewer_SubmitRequest RSQ)
        {
            Reviewer_SubmitResponse VR = new Reviewer_SubmitResponse();
            try
            {
                int iteration = 1;
                string fileName = string.Empty;
                string filePath = string.Empty;
                string villageAndTroupeId = string.Empty;
                string imagePathToStore = string.Empty;
                StringBuilder imagePathsBuilder = new StringBuilder();
                StringBuilder videoPathsBuilder = new StringBuilder();
                string Verifyofficerdetails = "VerifyOfficerDetails";
                string VOPhotos = "VerifyOfficerPhotos";
                string VOallvideos = "VerifyOfficerVideos";
                string webrootpath = _env.WebRootPath;
                string Verifyofficerdetailspath = Path.Combine(webrootpath, Verifyofficerdetails);
                if (!Directory.Exists(Verifyofficerdetailspath))
                    Directory.CreateDirectory(Verifyofficerdetailspath);
                string verifyofficertroupeid = Path.Combine(Verifyofficerdetailspath, RSQ.TroupeId!);
                if (!Directory.Exists(verifyofficertroupeid))
                    Directory.CreateDirectory(verifyofficertroupeid);
                string VOTroupePhotos = Path.Combine(verifyofficertroupeid, VOPhotos);
                if (RSQ.EventDetails != null)
                {
                    if (!Directory.Exists(VOTroupePhotos))
                        Directory.CreateDirectory(VOTroupePhotos);
                    foreach (var items in RSQ.EventDetails!)
                    {
                        var UploadFolderVOP = items.VillageId!;
                        string TroupeDocPath = Path.Combine(VOTroupePhotos, UploadFolderVOP);
                        if (!Directory.Exists(TroupeDocPath))
                            Directory.CreateDirectory(TroupeDocPath);
                        var villageId = items.VillageId;
                        IFormFile? file = null;
                        for (int i = 1; i <= 3; i++) // Process UpldImg1, UpldImg2, UpldImg3 in groups of three
                        {
                            async Task<string> GetFileExtension(IFormFile? file)
                            {
                                if (file != null && file.ContentType.Contains("image/"))
                                {
                                    return "." + file.ContentType.Split('/')[1];
                                }
                                return "";
                            }
                            switch (i)
                            {
                                case 1:
                                    string originalFileName = items.UpldImg1?.FileName!;
                                    string fileExtension = !string.IsNullOrEmpty(originalFileName) ? Path.GetExtension(originalFileName) : string.Empty;
                                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                                    filePath = Path.Combine(TroupeDocPath, fileName);
                                    if (!Directory.Exists(TroupeDocPath))
                                        Directory.CreateDirectory(TroupeDocPath);
                                    file = items.UpldImg1;
                                    break;
                                case 2:
                                    string originalFileName1 = items.UpldImg2?.FileName!;
                                    string fileExtension1 = !string.IsNullOrEmpty(originalFileName1) ? Path.GetExtension(originalFileName1) : string.Empty;
                                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension1;
                                    filePath = Path.Combine(TroupeDocPath, fileName);
                                    if (!Directory.Exists(TroupeDocPath))
                                        Directory.CreateDirectory(TroupeDocPath);
                                    file = items.UpldImg2;
                                    break;
                                case 3:
                                    string originalFileName2 = items.UpldImg3?.FileName!;
                                    string fileExtension2 = !string.IsNullOrEmpty(originalFileName2) ? Path.GetExtension(originalFileName2) : string.Empty;
                                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension2;
                                    filePath = Path.Combine(TroupeDocPath, fileName);
                                    if (!Directory.Exists(TroupeDocPath))
                                        Directory.CreateDirectory(TroupeDocPath);
                                    file = items.UpldImg3;
                                    break;
                            }
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file?.CopyToAsync(stream);
                            }
                            villageAndTroupeId = $"{villageId}_{RSQ.TroupeId}";
                            imagePathToStore = $"{villageAndTroupeId}|{Path.GetRelativePath(TroupeDocPath, filePath).Replace(Path.DirectorySeparatorChar, '|')}";
                            if (iteration == 1)
                            {
                                imagePathsBuilder.Append(imagePathToStore);
                            }
                            else
                            {
                                imagePathsBuilder.Append("~").Append(imagePathToStore);
                            }
                            iteration++;
                        }
                    }
                    RSQ.ImgPath = imagePathsBuilder.ToString();
                }
                if (RSQ.EventDetails != null)
                {
                    int iterationval = 1;
                    foreach (var items in RSQ.EventDetails!)
                    {
                        async Task<string> GetFileExtension(IFormFile? file)
                        {
                            if (file != null && file.ContentType.Contains("video/"))
                            {
                                return "." + file.ContentType.Split('/')[1];
                            }
                            return "";
                        }
                        string VOTroupeallVideos = Path.Combine(verifyofficertroupeid, VOallvideos);
                        var UploadFolderVOP = items.VillageId!;
                        string vovideos = Path.Combine(VOTroupeallVideos, UploadFolderVOP);
                        if (!Directory.Exists(vovideos))
                            Directory.CreateDirectory(vovideos);
                        var villageId = items.VillageId;
                        string originalFileName = items.UpldVdo?.FileName!;
                        string fileExtension = !string.IsNullOrEmpty(originalFileName) ? Path.GetExtension(originalFileName) : string.Empty;
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                        filePath = Path.Combine(vovideos, fileName);
                        if (!Directory.Exists(vovideos))
                            Directory.CreateDirectory(vovideos);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await items.UpldVdo!.CopyToAsync(stream);
                        }
                        villageAndTroupeId = $"{villageId}_{RSQ.TroupeId}";
                        imagePathToStore = $"{villageAndTroupeId}|{Path.GetRelativePath(vovideos, filePath).Replace(Path.DirectorySeparatorChar, '|')}";
                        if (iterationval == 1)
                        {
                            videoPathsBuilder.Append(imagePathToStore);
                        }
                        else
                        {
                            videoPathsBuilder.Append("~").Append(imagePathToStore);
                        }
                        iterationval++;
                    }
                    RSQ.Videopath = videoPathsBuilder.ToString();
                }
                var loginDetail = _AuthRepository.Reviewer_SubmitReport(RSQ)!;
                if (loginDetail == "1")
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Reviewer ReportedFor Event Successfully";
                    VR.ReviewerSubmitStatus = loginDetail.ToInt();
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "Failed to Submit!!!Something Went Wrong!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Submit Reviewer Report";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method For Blockuser Performance Approval Pending List
        [HttpPost]
        public async Task<IActionResult> PerformanceApproval_PendingList(string? BlockId)
        {
            TroupeVwResponse VR = new TroupeVwResponse();
            try
            {
                TroupeRegistrationView TRV = new TroupeRegistrationView();
                TRV.Status = 5;
                TRV.BlockId = BlockId!.ToInt();
                var objDistlist = await _RegistraionRepository.TroupeReportingApprovalListForBlock(TRV);
                VR.Status = "Success";
                VR.StatusCode = "200";
                VR.Message = "Performance Approval Pending List Fetched Successfully";
                VR.TroupeReportedApprovalList = objDistlist;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Fetch Performance Approval Pending List ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to get Details OF performance Approval Pending List...
        //[HttpPost]
        public async Task<IActionResult> PerformanceApproval_PendingList_Details(int TroupeId, int AssignEventId, int STATUS)
        {
            TroupeVwResponse VR = new TroupeVwResponse();
            try
            {
                TroupeReportingView RV = new TroupeReportingView();
                RV.AssignEventId = AssignEventId;
                RV.TroupeId = TroupeId;
                RV.Status = STATUS;
                var EventDetailswithPhotoVideo = await _RegistraionRepository.TroupeReportingForApprovalDetails(RV);
                string[] UploadphotoArray = EventDetailswithPhotoVideo[0]!.UploadPhoto!.ToString().Split('~');
                List<string> Uploadphoto = UploadphotoArray.ToList();
                var groupedData = Uploadphoto
                .Select(entry =>
                {
                    var parts = entry.Split('|');
                    return new
                    {
                        VillageId = parts[0],
                        ImageId = parts[1]
                    };

                })
                .GroupBy(entry => entry.VillageId)
                .Select(group =>
                {
                    var villageId = group.Key;
                    var imageIds = string.Join("~", group.Select(entry => entry.ImageId));
                    return villageId + "|" + imageIds;
                })
                .ToList();
                EventDetailswithPhotoVideo[0].UploadPhoto = groupedData[0];
                EventDetailswithPhotoVideo[1].UploadPhoto = groupedData[1];
                EventDetailswithPhotoVideo[2].UploadPhoto = groupedData[2];
                var UploadVideo = EventDetailswithPhotoVideo[0]!.UploadVideo!.ToString().Split('~');
                EventDetailswithPhotoVideo[0].UploadVideo = UploadVideo[0];
                EventDetailswithPhotoVideo[1].UploadVideo = UploadVideo[1];
                EventDetailswithPhotoVideo[2].UploadVideo = UploadVideo[2];
                var ReviewerPhotolist = await _RegistraionRepository.ReviewersPhotoList(AssignEventId, TroupeId);
                VR.Status = "Success";
                VR.StatusCode = "200";
                VR.Message = "Details OF Performance Approval Pending List Fetched Successfully";
                VR.TroupeReportedApprovalList = EventDetailswithPhotoVideo;
                VR.ReviewersPhotoVideo = ReviewerPhotolist;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Fetch Details OF Performance Approval Pending List ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method to get Approve/Reject/Revert To Applicant OF performance Approval (Particular event)...
        [HttpPost]
        public async Task<IActionResult> PerformanceApproval_TakeAction(TroupeReportingRequests TRA)
        {
            PerformanceApprovalResponse VR = new PerformanceApprovalResponse();
            try
            {
                TroupeReporting TR = new TroupeReporting();
                TR.AssignEventId = TRA.AssignEventId;
                TR.TroupeId = TRA.TroupeId;
                TR.Remark = TRA.Remark;
                TR.Status = TRA.Status;
                TR.CreatedBy = TRA.CreatedBy;
                TR.ImageIdName = TRA.ImageIdName;
                TR.UpldVdo = TRA.UpldVdo;
                TR.CreatedBy = TRA.CreatedBy;
                string retval = await _RegistraionRepository.M_ReportingApproval(TR);
                VR.Status = "Success";
                VR.StatusCode = "200";
                if (retval == "4")
                {
                    VR.Message = "Performance For this event Approved Successfully";
                    retval = "6";
                }
                if (retval == "5")
                {
                    VR.Message = "Performance For this event Rejected Successfully";
                    retval = "7";
                }
                if (retval == "6")
                {
                    VR.Message = "Performance For this event Reverted  To Applicant Successfully";
                    retval = "8";
                }
                VR.PerformanceApprovalStatus = retval;
                string troupe = JsonConvert.SerializeObject(VR);
                troupe = troupe.Replace("null", "[]");
                return Ok(troupe);
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Fetch Details OF Performance Approval Pending List ";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Method for assign troupe submission
        [HttpPost]
        public async Task<IActionResult> AssignedTroupeEvent([FromBody] EventDetails_Mob eventDetails)
        {
            AssignTroupe_SubmitResponse VR = new AssignTroupe_SubmitResponse();
            try
            {
                var componentXml = eventDetails.ComponentXml;

                // Convert the XML data to a suitable format for storage in the database
                var xEle = new XElement("AssignDetails",
                    from emp in componentXml
                    select new XElement("AssignData",
                        new XElement("Village", emp.Village),
                        new XElement("Area", emp.Area),
                        new XElement("DateofPerform", emp.DateofPerform),
                        new XElement("StartTime", emp.StartTime),
                        new XElement("EndTime", emp.EndTime),
                        new XElement("OfficerName", emp.OfficerName),
                        new XElement("OfficerMobNo", emp.OfficerMobNo),
                        new XElement("ImgFolderID", emp.ImgFolderID)
                    )
                );

                // Pass the XElement to your repository method for further processing and storage
                eventDetails.ComponentXmlstring = xEle.ToString();
                string retval = await _EventRepository.AssignTroupe_Mob(eventDetails);
                if (retval == "10")
                {
                    VR.Status = "Success";
                    VR.StatusCode = "200";
                    VR.Message = "Troupe Assigned Successfully";
                    VR.AssignTroupeStatus = retval.ToInt();
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
                else
                {
                    VR.Status = "Failed";
                    VR.StatusCode = "400";
                    VR.Message = "Failed to Assign Troupe!!!Something Went Wrong!!";
                    string troupe = JsonConvert.SerializeObject(VR);
                    troupe = troupe.Replace("null", "[]");
                    return Ok(troupe);
                }
            }
            catch (Exception ex)
            {
                VR.Status = "Failed";
                VR.StatusCode = "400";
                VR.Message = "Failed to Assign Troupe";
                CommonHelper.LogError(ex, "Login", Path.Combine(_env.WebRootPath, "Log"));
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        //Bind in troupe login after photo and video revert
        [HttpPost]
        public IActionResult ReportRevertingMob(int AssignEventId,int TroupeId, int Status)
        {
            List<ImageListmob> ImageList = new List<ImageListmob>();
            List<VideoList> VideoList = new List<VideoList>();
            string bannerImgPath = "";
            if (TroupeId != 0)
            {
                TroupeReportingView Trv = new TroupeReportingView();
                Trv.AssignEventId = AssignEventId;
                Trv.TroupeId = TroupeId;
                Trv.Status = Status;
                var troupeDetails = _RegistraionRepository.TroupeReportReverting(Trv).Result.ToList();
                var troupeDetails1 = _RegistraionRepository.TroupeDetails(new TroupeRegistrationView { TroupeId = TroupeId, Status = 0 }).Result.ToList();

                bannerImgPath = troupeDetails1[0].BannerImg != "NA" ?
                    $"TroupeDetails/{troupeDetails1[0].UploadBannerFolder}/Banner/{troupeDetails1[0].BannerImg}" :
                    $"TroupeDetails/{troupeDetails1[0].UploadBannerFolder}/GroupPhoto/{troupeDetails1[0].GroupPhoto}";

                var memberPerformed = troupeDetails[0].MemberPerformed!.ToString().Split('|');
                foreach (var item in troupeDetails)
                {
                    var membersplit = item.MemberPerformed!.ToString().Split('~')[1];
                    string[] memlist = membersplit.Split(',');
                    int membercount = memlist.Length;
                    item.MemberCount = membercount;
                }
                string[] UploadphotoArray = troupeDetails[0].UploadPhoto!.ToString().Split('~');
                var groupedData = UploadphotoArray
                    .Select(entry =>
                    {
                        var parts = entry.Split('|');
                        return new
                        {
                            VillageId = parts[0],
                            ImageId = parts[1],
                            Status = parts[2]
                        };
                    })
                    .GroupBy(entry => entry.VillageId)
                    .Select(group =>
                    {
                        var villageId = group.Key;
                        var imageIds = string.Join("|", group.Select(entry => entry.ImageId + "_" + entry.Status));
                        return villageId + "|" + imageIds;
                    })
                    .ToList();

                foreach (var entry in groupedData)
                {
                    string modifiedEntry = entry.ReplaceFirst("|", "~");
                    var imageid = modifiedEntry.Split('~')[0];
                    var villageid = imageid.Split('_')[0];
                    var villageName = _masterService.GetVillageNameById(villageid);
                    var imagename = modifiedEntry.Split('~')[1].Replace("~_", "~");
                    ImageList.Add(new ImageListmob { imageid = imageid, imagename = imagename, villageid = villageid, villageName = villageName.ToString() });
                }

                var uploadVideo = troupeDetails[0].UploadVideo!.ToString().Split('~');
                foreach (var item in uploadVideo)
                {
                    var videoid = item.Split('|')[0];
                    var villageid = videoid.Split('_')[0];
                    var villageName = _masterService.GetVillageNameById(villageid);
                    var videoname = item.Split('|')[1];
                    var revstatus = item.Split('|')[2];
                    VideoList.Add(new VideoList { videoid = videoid, videoname = videoname, villageid = villageid, Status = revstatus, villageName = villageName.ToString() });
                }
            }
            return Ok(new { ImageList, VideoList, BannerImg = bannerImgPath ?? "[]" });
        }

    }

}





