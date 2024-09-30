using AngleSharp.Text;
using CTMS.Core;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.ImageList;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.ReportMaster;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using X.PagedList;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace CTMS.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IRegistraionRepository _RegistraionRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMasterRepository _masterService;
        private readonly IReportRepository _ReportRepository;
        TroupeRegistrationView TRV = new TroupeRegistrationView();
        MemberDetailsView Md = new MemberDetailsView();

        public DashboardController(IMasterRepository masterService, IRegistraionRepository RegistraionRepository, IWebHostEnvironment hostingEnvironment, IReportRepository ReportRepository)
        {
            _RegistraionRepository = RegistraionRepository;
            _ReportRepository = ReportRepository;
            _hostingEnvironment = hostingEnvironment;
            _masterService = masterService;
        }
        public IActionResult ProfileDashboard()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        ViewBag.Distid = TRV.DistrictId;
                        ViewBag.Blockid = TRV.BlockId;
                        ViewBag.Gpid = TRV.GPId;
                        TRV.Status = 0;
                        TRV.TroupeId = TroupeId;
                        ViewBag.Events = null;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                        ViewBag.TroupeEventDetails = _RegistraionRepository.TroupeEventDetails(TRV).Result.ToList();

                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }

                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        public IActionResult ProfileDashboard(TroupeRegistrationView trv)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.Status = 0;
                        TRV.TroupeId = TroupeId;
                        ViewBag.Events = null;
                        TRV.DistrictId = trv.DistrictId;
                        TRV.BlockId = trv.BlockId;
                        TRV.GPId = trv.GPId;
                        TRV.FromDate = trv.FromDate;
                        TRV.ToDate = trv.ToDate;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                        ViewBag.TroupeEventDetails = _RegistraionRepository.TroupeEventDetails(TRV).Result.ToList();
                        ViewBag.Distid = TRV.DistrictId;
                        ViewBag.Blockid = TRV.BlockId;
                        ViewBag.Gpid = TRV.GPId;
                        ViewBag.Fromdate = TRV.FromDate;
                        ViewBag.ToDate = TRV.ToDate;
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }

                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult EventPerformedDashboard()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.TroupeId = TroupeId;
                        EventPerformedDetails epd = new EventPerformedDetails();
                        epd.TroupeId = TroupeId;
                        ViewBag.Events = null;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                        ViewBag.EventPerformDetails = _RegistraionRepository.EventPerformedDetails(epd).Result.ToList();
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewEventDetailForTroupe(int EventId, string action, int P_DIST_BLK_ID, int TroupID)
        {
            int distid = Convert.ToInt32(User.FindFirst("UserID")?.Value);
            try
            {
                var data = _RegistraionRepository.ViewEventDetailForTroupe(0, "R", distid, TroupID);

                return Json(new { sucess = true, responseMessage = "Action taken Sucessfully.", responseText = "Sucess", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Error", data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetSingleEventForTroupe(int Id, int TroupID)
        {
            int distid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        IList<EventDetails> list = _RegistraionRepository.ViewEventDetailForTroupe(Id, "R", distid, TroupID);

                        if (list != null)
                        {
                            var jsonres = JsonConvert.SerializeObject(list);
                            return Json(jsonres);
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
                    }
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult MembersProfileDashboard()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.TroupeId = TroupeId;
                        Md.TroupeId = TroupeId;
                        ViewBag.Events = null;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                        ViewBag.MemberDetails = _RegistraionRepository.MemberDetails(Md).Result.ToList();
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }

        }
        [HttpGet]
        public IActionResult EditProfile(string TroupeId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    TRV.TroupeId = Convert.ToInt32(TroupeId);
                    ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                    ViewBag.Troupeid = Convert.ToInt32(TroupeId);
                    if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                    {
                        ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                    }
                    else
                    {
                        ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(M_Troupe_Registration P_TUS)
        {
            try
            {
                ViewBag.Troupeid = P_TUS.TroupeId;
                ViewBag.ButtonName = (P_TUS.TroupeId == 0) ? "Submit" : "Update";
                var data = HttpContext.Request.Form["Elements"];
                var ResultDtls = JsonConvert.DeserializeObject<List<MemberDetails>>(data!);
                var xEle = new XElement("MemberDetails",
                        from emp in ResultDtls
                        select new XElement("MemberData",
                            new XElement("MemberName", emp.MemberName),
                            new XElement("MemberDob", emp.MemberDob),
                            new XElement("MemberFatherName", emp.MemberFatherName),
                            new XElement("RoleId", emp.RoleId),
                            new XElement("Role", emp.RoleId != 2 ? null : emp.Role),
                            new XElement("Gender", emp.Gender),
                            new XElement("DistId", emp.MemberDistId),
                            new XElement("BlockId", emp.MemberBlockId),
                            new XElement("GpId", emp.MemberGpId),
                            new XElement("AadharNumber", emp.AadharNumber),
                            new XElement("MobileNumber", emp.MobileNumber),
                            new XElement("Email", emp.Email),
                            new XElement("IFSCCode", emp.IFSCCode),
                            new XElement("BankName", emp.BankID),
                            new XElement("BranchName", emp.BranchID),
                            new XElement("BankAccountNo", emp.BankAccountNo),
                            new XElement("IsLeader", emp.IsLeader)
                ));
                string flodername = "TroupeDetails";
                string webrootpath = _hostingEnvironment.WebRootPath;
                string ProcDocPath = Path.Combine(webrootpath, flodername);
                string TroupeDocPath = Path.Combine(ProcDocPath, P_TUS.UploadFolderPath!);
                if (!Directory.Exists(ProcDocPath))
                    Directory.CreateDirectory(ProcDocPath);
                if (!Directory.Exists(TroupeDocPath))
                    Directory.CreateDirectory(TroupeDocPath);
                var uploadbanner = Request.Form.Files["UploadBanner"];
                var uploadregphoto = Request.Form.Files["UpldRegPhtCopy"];
                var groupphoto = Request.Form.Files["GroupPhotoImg"];
                var uploadvideo = Request.Form.Files["Upldvdofile"];
                if (uploadbanner != null)
                {

                    string BannerFolder = Path.Combine(TroupeDocPath, "Banner");
                    if (Directory.Exists(BannerFolder))
                    {
                        Directory.Delete(BannerFolder, true);
                    }
                    if (!Directory.Exists(BannerFolder))
                    {
                        Directory.CreateDirectory(BannerFolder);
                    }
                    var extn = "." + uploadbanner.FileName.Split('.')[uploadbanner.FileName.Split('.').Length - 1];
                    P_TUS.UploadBannerImage = "Banner" + extn;

                    using (var stream = new FileStream(Path.Combine(BannerFolder, P_TUS.UploadBannerImage), FileMode.Create))
                    {
                        await P_TUS.UploadBanner.CopyToAsync(stream);
                    }
                }
                else
                {
                    P_TUS!.UploadBannerImage = P_TUS.UploadBannerImage;
                }
                if (uploadregphoto != null)
                {
                    string BannerFolder = Path.Combine(TroupeDocPath, "RegPhoto");
                    if (Directory.Exists(BannerFolder))
                    {
                        Directory.Delete(BannerFolder, true);
                    }
                    if (!Directory.Exists(BannerFolder))
                    {
                        Directory.CreateDirectory(BannerFolder);
                    }
                    var extn = "." + uploadregphoto.FileName.Split('.')[uploadregphoto.FileName.Split('.').Length - 1];
                    P_TUS.UpldRegPhtCopyImage = "RegPhoto" + extn;
                    using (var stream = new FileStream(Path.Combine(BannerFolder, P_TUS.UpldRegPhtCopyImage), FileMode.Create))
                    {
                        await P_TUS.UpldRegPhtCopy.CopyToAsync(stream);
                    }
                }
                else
                {
                    P_TUS!.UpldRegPhtCopyImage = P_TUS.UpldRegPhtCopyImage;
                }
                if (groupphoto != null)
                {
                    string BannerFolder = Path.Combine(TroupeDocPath, "GroupPhoto");
                    if (Directory.Exists(BannerFolder))
                    {
                        Directory.Delete(BannerFolder, true);
                    }
                    if (!Directory.Exists(BannerFolder))
                    {
                        Directory.CreateDirectory(BannerFolder);
                    }
                    var extn = "." + groupphoto.FileName.Split('.')[groupphoto.FileName.Split('.').Length - 1];
                    P_TUS.GroupPhoto = "GroupPhoto" + extn;
                    using (var stream = new FileStream(Path.Combine(BannerFolder, P_TUS.GroupPhoto!), FileMode.Create))
                    {
                        await P_TUS.GroupPhotoImg.CopyToAsync(stream);
                    }
                }
                else
                {
                    P_TUS!.GroupPhoto = P_TUS.GroupPhoto;
                }
                if (uploadvideo != null)
                {
                    P_TUS.Upldvdofile = uploadvideo;
                    string BannerFolder = Path.Combine(TroupeDocPath, "RegVideo");
                    if (Directory.Exists(BannerFolder))
                    {
                        Directory.Delete(BannerFolder, true);
                    }
                    if (!Directory.Exists(BannerFolder))
                    {
                        Directory.CreateDirectory(BannerFolder);
                    }
                    var extn = "." + uploadvideo.FileName.Split('.')[uploadvideo.FileName.Split('.').Length - 1];
                    P_TUS.UpldVdo = "RegVideo" + extn;
                    using (var stream = new FileStream(Path.Combine(BannerFolder, P_TUS.UpldVdo), FileMode.Create))
                    {
                        await P_TUS.Upldvdofile.CopyToAsync(stream);
                    }
                }
                else
                {
                    P_TUS!.UpldVdo = P_TUS.UpldVdo;
                }

                P_TUS.ComponentXml = xEle;
                P_TUS.Action = (P_TUS.TroupeId != 0) ? "U" : "I";
                P_TUS.CreatedBy = P_TUS.TroupeId;
                string retval = await _RegistraionRepository.TroupeRegistration(P_TUS);
                string[] ackno = retval.Split("|");

                if (ackno[1].ToString() == "1")
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Troupe Details Added Successfully", data = ackno[1], AcknowledgementNo = ackno[0] }.ToJson());
                }
                else if (ackno[1].ToString() == "2")
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Troupe Details Updated Successfully", data = ackno[1], AcknowledgementNo = ackno[0] }.ToJson());
                }
                else if (ackno[1].ToString() == "15")
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Aadhaar No. Already Exists", data = ackno[1], AcknowledgementNo = ackno[0] }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error In Adding Troupe Details", data = retval }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        public IActionResult MembersDashboard()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.TroupeId = TroupeId;
                        Md.TroupeId = TroupeId;
                        ViewBag.Events = null;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TRV).Result.ToList();
                        ViewBag.MemberDetails = _RegistraionRepository.MemberDetails(Md).Result.ToList();
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpGet]
        public IActionResult TotalRegistrationDetails()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("UID")?.Value != null)
                    {
                        return View();
                    }
                    else
                    {
                        return View("UnauthorizedAccess");
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult TotalRegistrationDetails(TroupeRegistrationView TRV)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("UID")?.Value != null)
                    {
                        var BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        if (BlockId != 0)
                        {
                            TRV.BlockId = BlockId;
                            ViewBag.TotalBlkRegistration = _RegistraionRepository.TotalRegsitrationDetails(TRV).Result.ToList();
                        }
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = ViewBag.TotalBlkRegistration }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult TotalEventDetails()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("UID")?.Value != null)
                    {
                        return View();
                    }
                    else
                    {
                        return View("UnauthorizedAccess");
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult TotalEventDetails(EventDetails TRV)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("UID")?.Value != null)
                    {
                        var BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        if (BlockId != 0)
                        {
                            TRV.BlockId = BlockId;
                            ViewBag.TotalBlkRegistration = _RegistraionRepository.TotalEventDetails(TRV).Result.ToList();
                        }
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = ViewBag.TotalBlkRegistration }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult TroupeReportingList()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.TroupeId = TroupeId;
                        ViewBag.TroupeReportingDetails = _RegistraionRepository.TroupeReportingDetails(TRV).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }

                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult TroupeReported()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TRV.TroupeId = TroupeId;
                        ViewBag.TroupeReportingDetails = _RegistraionRepository.TroupeReportingSuccess(TRV).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails[0].GroupPhoto;
                        }

                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult TroupeReporting(int AssignEventId, int Status)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TroupeReportingView Trv = new TroupeReportingView();
                        Trv.TroupeId = TroupeId;
                        Trv.AssignEventId = AssignEventId;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeReporting(Trv).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails1 = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails1[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails1[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails1[0].GroupPhoto;
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetMemberDetails(int TROUPEID = 0)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    try
                    {
                        MemberDetailsView dataModel = new MemberDetailsView();
                        dataModel.TroupeId = TROUPEID;
                        var objresult = await _RegistraionRepository.MemberDetails(dataModel);
                        if (objresult.Any())
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objresult }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Record not found", data = objresult }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
                    }
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> TroupeReporting(TroupeReporting TR)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    TR.CreatedBy = User.FindFirst("TroupeId")?.Value!;
                    TR.Action = "TD";
                    string retval = await _RegistraionRepository.TroupeReportingToBlock(TR);
                    if (retval == "4")
                    {
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Event Reported Successfully", data = retval }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error In Event Reporting", data = retval }.ToJson());
                    }
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(TroupeReporting TR)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    string retval = TR.ImgIdPath!;
                    string retvalvdo = TR.VdoIdPath!;

                    string Mainflodername = "TroupeReporting";
                    string Subflodername = User.FindFirst("TroupeId")?.Value!;
                    string webrootpath = _hostingEnvironment.WebRootPath;
                    string ProcDocPath = Path.Combine(webrootpath, Mainflodername);
                    string TroupeIdFolder = Path.Combine(ProcDocPath, Subflodername);
                    if (!Directory.Exists(ProcDocPath))
                        Directory.CreateDirectory(ProcDocPath);
                    if (!Directory.Exists(TroupeIdFolder))
                        Directory.CreateDirectory(TroupeIdFolder);
                    List<IFormFile> images = TR.ImagesFile!;
                    if (images != null)
                    {
                        foreach (var file in images)
                        {
                            string fileName = file.FileName!;
                            var idvalue = fileName.Split("|")[0];
                            var imagename = fileName.Split("|")[1];
                            string ImageFolder = Path.Combine(TroupeIdFolder, "Image");
                            string IdFolder = Path.Combine(ImageFolder, idvalue);
                            if (!Directory.Exists(ImageFolder))
                                Directory.CreateDirectory(ImageFolder);
                            if (!Directory.Exists(IdFolder))
                                Directory.CreateDirectory(IdFolder);
                            string filePath = Path.Combine(IdFolder, imagename);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                    IFormFile videos = TR.Upldvdofile!;
                    if (videos != null)
                    {
                        string fileName = TR.ImageIdName!;
                        var idvalue = fileName.Split("|")[0];
                        var imagename = fileName.Split("|")[1];
                        string VideoFolder = Path.Combine(TroupeIdFolder, "Video");
                        string IdFolder = Path.Combine(VideoFolder, idvalue);
                        if (!Directory.Exists(VideoFolder))
                            Directory.CreateDirectory(VideoFolder);
                        if (!Directory.Exists(IdFolder))
                            Directory.CreateDirectory(IdFolder);
                        string filePath = Path.Combine(IdFolder, imagename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await TR.Upldvdofile!.CopyToAsync(stream);
                        }
                        if (retvalvdo == null)
                        {
                            retvalvdo = fileName;
                        }
                        else
                        {
                            retvalvdo = retvalvdo! + "~" + fileName;
                        }
                    }
                    else
                    {
                        TR!.UpldVdo = TR.UpldVdo;
                    }

                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Uploaded Successfully", data = retval, data1 = retvalvdo }.ToJson());
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult ReportReverting(int AssignEventId, int Status)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    List<ImageList> ImageList = new List<ImageList>();
                    List<VideoList> VideoList = new List<VideoList>();
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TroupeReportingView Trv = new TroupeReportingView();
                        Trv.AssignEventId = AssignEventId;
                        Trv.TroupeId = TroupeId;
                        Trv.Status = Status;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeReportReverting(Trv).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails1 = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails1[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails1[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails1[0].GroupPhoto;
                        }
                        if (ViewBag.TroupeDetails.Count > 0)
                        {

                            ViewBag.Imgpath = ViewBag.TroupeDetails[0].UploadPhoto.ToString();
                            var Member = ViewBag.TroupeDetails[0].MemberPerformed.ToString().Split('|');
                            for (int i = 0; i < Math.Min(Member.Length, ViewBag.TroupeDetails.Count); i++)
                            {
                                var item = ViewBag.TroupeDetails[i];
                                var membersplit = Member[i].Split('~')[1];
                                string[] memlist = membersplit.Split(',');
                                int membercount = memlist.Length;
                                item.MemberCount = membercount;
                            }
                            string[] UploadphotoArray = ViewBag.TroupeDetails[0].UploadPhoto.ToString().Split('~');
                            List<string> Uploadphoto = UploadphotoArray.ToList();
                            var groupedData = Uploadphoto
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
                            List<string> modifiedArray = new List<string>();
                            foreach (var entry in groupedData)
                            {
                                string modifiedEntry = entry.ReplaceFirst("|", "~");
                                modifiedArray.Add(modifiedEntry);
                            }
                            foreach (var item in modifiedArray)
                            {
                                var imageid = item.Split('~')[0];
                                var villageid = imageid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var imagename = item.Split('~')[1];
                                string pattern = @"(\.[a-zA-Z0-9]+)";
                                string modifiedString = Regex.Replace(imagename, pattern, match => match.Value + "~");
                                imagename = modifiedString.Replace("~_", "~");
                                ImageList.Add(new ImageList { imageid = imageid, imagename = imagename, villageid = villageid, villageName = villageName.ToString() });
                            }
                            ViewBag.Vdopath = ViewBag.TroupeDetails[0].UploadVideo.ToString();
                            var UploadVideo = ViewBag.TroupeDetails[0].UploadVideo.ToString().Split('~');
                            foreach (var item in UploadVideo)
                            {
                                var videoid = item.Split('|')[0];
                                var villageid = videoid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var videoname = item.Split('|')[1];
                                var revstatus = item.Split('|')[2];
                                VideoList.Add(new VideoList { videoid = videoid, videoname = videoname, villageid = villageid, Status = revstatus, villageName = villageName.ToString() });
                            }
                        }
                        ViewBag.ImageList = ImageList;
                        ViewBag.VideoList = VideoList;
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReportReverting(TroupeReporting TR)
        {
            if (User.FindFirst("TroupeId")?.Value != null)
            {

                TR.CreatedBy = User.FindFirst("TroupeId")?.Value!;
                TR.Action = "RRUP";
                string retval = await _RegistraionRepository.TroupeReportingToBlock(TR);
                if (retval == "4")
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Event Reporting Successfully", data = retval }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error In Event Reporting", data = retval }.ToJson());
                }
            }
            else
            {
                return View("UnauthorizedAccess");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RevertUploadImage(TroupeReporting TR)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("TroupeId")?.Value != null)
                    {
                        string retval = TR.ImgIdPath!;
                        string retvalvdo = TR.VdoIdPath!;

                        string Mainflodername = "TroupeReporting";
                        string Subflodername = User.FindFirst("TroupeId")?.Value!;
                        string webrootpath = _hostingEnvironment.WebRootPath;
                        string ProcDocPath = Path.Combine(webrootpath, Mainflodername);
                        string TroupeIdFolder = Path.Combine(ProcDocPath, Subflodername);
                        if (!Directory.Exists(ProcDocPath))
                            Directory.CreateDirectory(ProcDocPath);
                        if (!Directory.Exists(TroupeIdFolder))
                            Directory.CreateDirectory(TroupeIdFolder);

                        List<IFormFile> images = TR.ImagesFile!;
                        IFormFile videos = TR.Upldvdofile!;
                        if (images != null)
                        {
                            foreach (var file in images)
                            {
                                string fileName = file.FileName;

                                string ImageFolder = Path.Combine(TroupeIdFolder, "Image");
                                if (!Directory.Exists(ImageFolder))
                                    Directory.CreateDirectory(ImageFolder);

                                var Uploadphoto = fileName.Split('|');
                                var imageid = Uploadphoto[0];
                                var imagename = Uploadphoto[1];
                                string IdFolder = Path.Combine(ImageFolder, imageid);
                                string filePath = Path.Combine(IdFolder, imagename);
                                if (!Directory.Exists(IdFolder))
                                    Directory.CreateDirectory(IdFolder);
                                if (System.IO.File.Exists(filePath))
                                {
                                    // Delete the image file
                                    System.IO.File.Delete(filePath);
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }

                                }
                                else
                                {
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }

                            }
                        }
                        else
                        {
                            TR!.ImageIdName = TR.ImageIdName;

                        }
                        if (videos != null)
                        {
                            string fileName = TR.ImageIdName!;
                            var idvalue = fileName.Split("|")[0];
                            var imagename = fileName.Split("|")[1];
                            string VideoFolder = Path.Combine(TroupeIdFolder, "Video");
                            string IdFolder = Path.Combine(VideoFolder, idvalue);
                            if (!Directory.Exists(VideoFolder))
                                Directory.CreateDirectory(VideoFolder);
                            if (!Directory.Exists(IdFolder))
                                Directory.CreateDirectory(IdFolder);
                            string VIdFolder = Path.Combine(VideoFolder, idvalue);
                            string filePath = Path.Combine(VIdFolder, imagename);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await TR.Upldvdofile!.CopyToAsync(stream);
                                }
                            }
                            else
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await TR.Upldvdofile!.CopyToAsync(stream);
                                }
                            }
                        }
                        else
                        {
                            TR!.UpldVdo = TR.UpldVdo;
                        }

                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Uploaded Successfully", data = retval, data1 = retvalvdo }.ToJson());
                    }
                    else
                    {
                        return View("UnauthorizedAccess");
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region DashboardReport
        public IActionResult DashboardTroupeRegReportCount(int DISTCODE, string UserType)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                ReportView TRV = new ReportView();
                TRV.DistId = 0;
                TRV.BlockId = 0;
                TRV.GPId = 0;
                ViewBag.DistId = TRV.DistId;
                ViewBag.Blockid = TRV.BlockId;
                ViewBag.GPId = 0;
                ViewBag.CatId = TRV.CatId;
                ViewBag.SubCatId = TRV.SubCatId;
                ViewBag.DistId = 0;
                if (DISTCODE != 5)
                {
                    TRV.DISTCODE = DISTCODE;
                }
                ViewBag.DISTCODE = DISTCODE;
                TRV.UserType = UserType;
                ViewBag.Dist = 0;
                ViewBag.Blockid = 0;
                ViewBag.UserType = TRV.UserType;
                ViewBag.Detail = _ReportRepository.DashboardRegistration(TRV).Result.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public async Task<IActionResult> DashboardTroupeRegReportCountSearch(int DistId, int BlockId)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ReportView TRV= new ReportView();
                    ViewBag.DistId = 0;

                    TRV.UserType = User.FindFirst("URTYPE")?.Value;

                    if (TRV.UserType == "BSSO")
                    {
                        TRV.DISTCODE = BlockId;
                    }
                    else
                    {
                        TRV.DISTCODE = DistId;
                        TRV.BlockId = BlockId;
                    }
                    ViewBag.Dist = DistId;
                    ViewBag.Blockid = BlockId;
                    ViewBag.DISTCODE = User.FindFirst("Userid")?.Value;
                    ViewBag.UserType = TRV.UserType;

                    ViewBag.Detail = _ReportRepository.DashboardRegistration(TRV).Result.ToList();
                    return View("DashboardTroupeRegReportCount");
                }
                else
                {
                    return View("SessionOut");
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        public IActionResult DashboardTroupeDrildown(int DistId, int BlockId, int CatId, int SubCatId)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                ReportView TRV = new ReportView();
                TRV.CatId = CatId;
                TRV.SubCatId = SubCatId;
                TRV.DISTCODE = DistId;
                TRV.BlockId = BlockId;
                ViewBag.CatId = TRV.CatId;
                ViewBag.SubCatId = TRV.SubCatId;
                ViewBag.DistId = DistId;
                ViewBag.Detail = _ReportRepository.ArtFormWiseTroupe(TRV).Result.ToList();
                return View("DashboardTroupeRegReportCount");
            }
            else
            {
                return View("SessionOut");
            }
        }

        public IActionResult DashboardPaymentReport(int DISTCODE, string userrole, string UserType)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                ReportView TRV = new ReportView();
                ViewBag.CatId = TRV.CatId;
                ViewBag.SubCatId = TRV.SubCatId;
                ViewBag.DistId = 0;
                if (DISTCODE != 5)
                {
                    TRV.DISTCODE = DISTCODE;
                }
                ViewBag.DISTCODE = DISTCODE;
                TRV.UserType = UserType;
                ViewBag.Dist = 0;
                ViewBag.Blockid = 0;
                ViewBag.UserType = TRV.UserType;
                ViewBag.Role = Convert.ToInt32(userrole);
                TRV.Role = Convert.ToInt32(userrole);
                ViewBag.Detail = _ReportRepository.DashboardPaymentReport(TRV).Result.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult DashboardPaymentReportSearch(int DISTCODE, int BlockId)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                ReportView TRV = new ReportView();
                ViewBag.CatId = TRV.CatId;
                ViewBag.SubCatId = TRV.SubCatId;
                ViewBag.DistId = 0;
                TRV.UserType = User.FindFirst("URTYPE")?.Value;
                if(BlockId == 0)
                {
                    TRV.DISTCODE = DISTCODE;
                    TRV.Role = 1;
                }
                else
                {
                    TRV.DISTCODE= BlockId;
                    TRV.Role = 2;
                }
                ViewBag.Dist = DISTCODE;
                ViewBag.Blockid = BlockId;
                ViewBag.DISTCODE = User.FindFirst("Userid")?.Value;
                ViewBag.UserType = TRV.UserType;
                ViewBag.Role = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                ViewBag.Detail = _ReportRepository.DashboardPaymentReportSearch(TRV).Result.ToList();
                return View("DashboardPaymentReport");
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult DashboardPaymentReportDetails(int DistId)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                ReportView TRV = new ReportView();

                TRV.DISTCODE = DistId;

                ViewBag.DistId = DistId;
                ViewBag.Detail = _ReportRepository.DashboardPaymentReportDetails(TRV).Result.ToList();
                return View("DashboardPaymentReport");
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult DashboardEventReport(int DISTCODE=0,int userrole=0,string UserType="")
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    var URTYPE = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    if (URTYPE == 2 ||URTYPE==1 ||URTYPE==5)
                    {
                        TRV.URTYPE = URTYPE;
                        TRV.DistId = 0;
                        TRV.BlockId = 0;
                        TRV.GPId = 0;
                        ViewBag.DistId = TRV.DistId;
                        ViewBag.Blockid = TRV.BlockId;
                        ViewBag.GPId = TRV.GPId;
                    }
                    else if(URTYPE == 4)
                    {
                        TRV.URTYPE = URTYPE;
                        ViewBag.DistId = 0;
                        TRV.DistId = DISTCODE;
                        TRV.BlockId = 0;
                        TRV.GPId = 0;
                        ViewBag.Blockid = TRV.BlockId;
                        ViewBag.GPId = TRV.GPId;
                    }
                    else 
                    {
                        TRV.URTYPE = URTYPE;
                        ViewBag.DistId = DISTCODE;
                        TRV.DistId = 0;
                        TRV.BlockId = DISTCODE;
                        TRV.GPId = 0;
                        ViewBag.Blockid = TRV.BlockId;
                        ViewBag.GPId = TRV.GPId;
                    }
                    ViewBag.DISTCODE = DISTCODE;
                    ViewBag.UserType =UserType;
                    ViewBag.Dist = 0;
                    ViewBag.Blockid = 0;
                    ViewBag.Detail = _ReportRepository.EventReport(TRV).Result.ToList();
                    return View();
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        public async Task<IActionResult> DashboardEventReportSearch(int DistId, int BlockId)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;
                    TRV.BlockId = BlockId;
                    var Role = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    TRV.URTYPE = Role;
                    if (Role == 2 || Role == 1 || Role == 5)
                    {
                        ViewBag.DistId = 0;
                    }
                    else
                    {
                        ViewBag.DistId = TRV.DistId;
                        ViewBag.Blockid = BlockId;
                    }
                    ViewBag.Dist = DistId;
                    ViewBag.Blockid = BlockId;
                    ViewBag.Role = Role;
                    ViewBag.DISTCODE = User.FindFirst("Userid")?.Value;
                    ViewBag.UserType = User.FindFirst("URTYPE")?.Value;
                    ViewBag.Detail = _ReportRepository.EventReport(TRV).Result.ToList();
                    return View("DashboardEventReport");
                }
                else
                {
                    return View("SessionOut");
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        #endregion
        public IActionResult TroupeReportingNew(int AssignEventId, int Status)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TroupeReportingView Trv = new TroupeReportingView();
                        Trv.TroupeId = TroupeId;
                        Trv.AssignEventId = AssignEventId;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeReporting(Trv).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails1 = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails1[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails1[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails1[0].GroupPhoto;
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult ReportRevertingNew(int AssignEventId, int Status)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
                {
                    List<ImageList> ImageList = new List<ImageList>();
                    List<VideoList> VideoList = new List<VideoList>();
                    var TroupeId = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                    if (TroupeId != 0)
                    {
                        TroupeReportingView Trv = new TroupeReportingView();
                        Trv.AssignEventId = AssignEventId;
                        Trv.TroupeId = TroupeId;
                        Trv.Status = Status;
                        ViewBag.TroupeDetails = _RegistraionRepository.TroupeReportReverting(Trv).Result.ToList();
                        TroupeRegistrationView TR = new TroupeRegistrationView();
                        TR.TroupeId = TroupeId;
                        TR.Status = 0;
                        ViewBag.TroupeDetails1 = _RegistraionRepository.TroupeDetails(TR).Result.ToList();
                        if (ViewBag.TroupeDetails1[0].BannerImg! != "NA")
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "Banner" + "/" + ViewBag.TroupeDetails1[0].BannerImg;
                        }
                        else
                        {
                            ViewBag.Img = "TroupeDetails" + "/" + ViewBag.TroupeDetails1[0].UploadBannerFolder + "/" + "GroupPhoto" + "/" + ViewBag.TroupeDetails1[0].GroupPhoto;
                        }
                        if (ViewBag.TroupeDetails.Count > 0)
                        {

                            ViewBag.Imgpath = ViewBag.TroupeDetails[0].UploadPhoto.ToString();
                            var Member = ViewBag.TroupeDetails[0].MemberPerformed.ToString().Split('|');
                            for (int i = 0; i < Math.Min(Member.Length, ViewBag.TroupeDetails.Count); i++)
                            {
                                var item = ViewBag.TroupeDetails[i];
                                var membersplit = Member[i].Split('~')[1];
                                string[] memlist = membersplit.Split(',');
                                int membercount = memlist.Length;
                                item.MemberCount = membercount;
                            }
                            string[] UploadphotoArray = ViewBag.TroupeDetails[0].UploadPhoto.ToString().Split('~');
                            List<string> Uploadphoto = UploadphotoArray.ToList();
                            var groupedData = Uploadphoto
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
                            List<string> modifiedArray = new List<string>();
                            foreach (var entry in groupedData)
                            {
                                string modifiedEntry = entry.ReplaceFirst("|", "~");
                                modifiedArray.Add(modifiedEntry);
                            }
                            foreach (var item in modifiedArray)
                            {
                                var imageid = item.Split('~')[0];
                                var villageid = imageid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var imagename = item.Split('~')[1];
                                string pattern = @"(\.[a-zA-Z0-9]+)";
                                string modifiedString = Regex.Replace(imagename, pattern, match => match.Value + "~");
                                imagename = modifiedString.Replace("~_", "~");
                                ImageList.Add(new ImageList { imageid = imageid, imagename = imagename, villageid = villageid, villageName = villageName.ToString() });
                            }
                            ViewBag.Vdopath = ViewBag.TroupeDetails[0].UploadVideo.ToString();
                            var UploadVideo = ViewBag.TroupeDetails[0].UploadVideo.ToString().Split('~');
                            foreach (var item in UploadVideo)
                            {
                                var videoid = item.Split('|')[0];
                                var villageid = videoid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var videoname = item.Split('|')[1];
                                var revstatus = item.Split('|')[2];
                                VideoList.Add(new VideoList { videoid = videoid, videoname = videoname, villageid = villageid, Status = revstatus, villageName = villageName.ToString() });
                            }
                        }
                        ViewBag.ImageList = ImageList;
                        ViewBag.VideoList = VideoList;
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult MyProfile()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("RoleId")?.Value != null)
                {
                    ViewBag.ButtonName = (User.FindFirst("RoleId")?.Value == "3") ? "Submit" : "Update";
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpGet]
        public IActionResult GetPCount()
        {
            string BlockId = User.FindFirst("Userid")?.Value;
            int pcount = _RegistraionRepository.GetProfileCount(BlockId.ToInt()); 
            return Json(new { pcount });
        }
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50_000_000)]
        [HttpPost]
        public async Task<IActionResult> MyProfile([FromForm] MyProfile MP)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("RoleId")?.Value != null)
                {
                    ViewBag.Profileid = MP.PID;
                    List<ExecutiveMember> executives = MP.Executives!;
                    XElement executivesXml = new XElement("Executives",
                        executives.Select(executive =>
                            new XElement("ExecutiveMember",
                                new XElement("ExecutiveMemberName", executive.ExecutiveMemberName),
                                new XElement("ExecutiveMemberMobile", executive.ExecutiveMemberMobile)
                            )
                        )
                    );
                    MP.ExecutiveXml = executivesXml;
                    List<GeneralBody> generals = MP.Generals!;
                    XElement generalsXml = new XElement("Generals",
                        generals.Select(general =>
                            new XElement("GeneralBody",
                                new XElement("GeneralBodyMemberName", general.GeneralBodyMemberName),
                                new XElement("GeneralBodyMobile", general.GeneralBodyMobile)
                            )
                        )
                    );
                    MP.GeneralsXml = generalsXml;
                    ViewBag.ButtonName = (User.FindFirst("RoleId")?.Value == "3") ? "Submit" : "Update";
                    string flodername = User.FindFirst("USERID")?.Value!;                    
                    MP.createdby = User.FindFirst("USERID")?.Value!;
                    MP.BLOCKID = User.FindFirst("USERID")?.Value!;
                    var UploadFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string webrootpath = _hostingEnvironment.WebRootPath;
                    string Profile = "ProfileDetails";
                    string ProfilePath = Path.Combine(webrootpath, Profile);
                    if (!Directory.Exists(ProfilePath))
                        Directory.CreateDirectory(ProfilePath);
                    string RootPath = Path.Combine(ProfilePath, flodername);
                    if (!Directory.Exists(RootPath))
                        Directory.CreateDirectory(RootPath);
                    var societyregcertfile = Request.Form.Files["societyregcertfiles"];
                    if (societyregcertfile != null)
                    {
                        string SocietyRegCertFolder = Path.Combine(RootPath, "SocietyRegCert");
                        if (!Directory.Exists(SocietyRegCertFolder))
                        {
                            Directory.CreateDirectory(SocietyRegCertFolder);
                        }
                        var extn = "." + societyregcertfile.FileName.Split('.')[societyregcertfile.FileName.Split('.').Length - 1];
                        MP.societyregcert = UploadFolder + extn;
                        using (var stream = new FileStream(Path.Combine(SocietyRegCertFolder, MP.societyregcert), FileMode.Create))
                        {
                            await MP.societyregcertfiles.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        MP!.societyregcert = MP.societyregcert;
                    }
                    var bylawfile = Request.Form.Files["bylawfiles"];
                    if (bylawfile != null)
                    {
                        string ByLawfolder = Path.Combine(RootPath, "ByLaw");
                        if (!Directory.Exists(ByLawfolder))
                        {
                            Directory.CreateDirectory(ByLawfolder);
                        }
                        var UploadFolder1 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var extn = "." + bylawfile.FileName.Split('.')[bylawfile.FileName.Split('.').Length - 1];
                        MP.bylawfile = UploadFolder1 + extn;
                        using (var stream = new FileStream(Path.Combine(ByLawfolder, MP.bylawfile), FileMode.Create))
                        {
                            await MP.Bylawfiles.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        MP!.bylawfile = MP.bylawfile;
                    }
                    var bymemorandumfile = Request.Form.Files["bymemorandumfiles"];
                    if (bymemorandumfile != null)
                    {
                        string ByMemorandumfolder = Path.Combine(RootPath, "ByMemorandum");
                        if (!Directory.Exists(ByMemorandumfolder))
                        {
                            Directory.CreateDirectory(ByMemorandumfolder);
                        }
                        var UploadFolder2 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var extn = "." + bymemorandumfile.FileName.Split('.')[bymemorandumfile.FileName.Split('.').Length - 1];
                        MP.bymemorandumfile = UploadFolder2 + extn;
                        using (var stream = new FileStream(Path.Combine(ByMemorandumfolder, MP.bymemorandumfile), FileMode.Create))
                        {
                            await MP.Bymemorandumfiles.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        MP!.bymemorandumfile = MP.bymemorandumfile;
                    }
                    string retval = await _RegistraionRepository.AddMyProfileDetails(MP);
                    if (retval == "1")
                    {
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Profile Details Added sucessfully", data = retval }.ToJson());
                    }
                    else if (retval == "7")
                    {
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Profile Details Updated sucessfully", data = retval }.ToJson());
                    }
                   else
                    {
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error In Adding Profile Details", data = retval }.ToJson());
                    }
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewMyProfile()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                    ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewMyProfile(MyProfile TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("USERID")?.Value != null)
                {
                    try
                    {
                        TRV.BLOCKID = User.FindFirst("Userid")?.Value;
                        ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                        IList<MyProfile> objDistlist = await _RegistraionRepository.ViewMyProfile(TRV);
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing", data = "" }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
                    }
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }

        }
        [HttpGet]
        public async Task<IActionResult> DeleteMyProfile(int Id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { sucess = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {
                MyProfile ob = new MyProfile();
                ob.PID = Id.ToString();
                ob.createdby = User.FindFirst("Userid")?.Value!;
                var data = await _RegistraionRepository.DeleteProfiledetails(ob);
                return Json(new { sucess = true, responseMessage = "Profile deleted successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditMyProfile(int PID,int Blockid)
        {
            ViewBag.GetoneProfileDetails = await _RegistraionRepository.GetoneProfileDetails(PID, Blockid);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetExecutiveMemberDetails(int PID = 0)
        {
            try
            {
                var objresult = await _RegistraionRepository.ExecutiveMemberDetails(PID);
                if (objresult.Any())
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objresult }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Record not found", data = objresult }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        public async Task<IActionResult> GetGeneralBodyDetails(int PID = 0)
        {
            try
            {
                var objresult = await _RegistraionRepository.GeneralMemberDetails(PID);
                if (objresult.Any())
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objresult }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Record not found", data = objresult }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
    }
}
