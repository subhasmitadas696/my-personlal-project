using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Model.Entities.Approval;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Xml.Linq;
#pragma warning disable SCS0016
// Since the antiforgery token is globally managed, there's no need to include the "validateAntiforgeryToken" attribute for each POST request.
namespace CTMS.Web
{
    [Authorize]
    public class ApprovalConfigController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        public ApprovalConfigController(IApprovalConfigRepository ApprovalConfigRepository, IWebHostEnvironment hostingEnvironment)
        {
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult UnauthorizedAccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TroupeApprovalView()
        {
            if (User?.Identity?.IsAuthenticated==true)
            {
                if (User.FindFirst("TroupeId")?.Value != null)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeAction(TakeActionDto Obj)
        {
            try
            {
                var objresult = await _ApprovalConfigRepository.TakeAction(Obj);
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record Found", data = objresult }.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.warning.ToString(), message = ex.Message, data = "" }.ToJson());
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAllDetails(int TROUPEID = 0)
        {
            try
            {

                MemberDetailsView dataModel = new MemberDetailsView();
                dataModel.TroupeId = TROUPEID;
                var objresult = await _ApprovalConfigRepository.GetAllDetails(dataModel);
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
        [HttpGet]
        public IActionResult TroupeApproved()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TroupeRejected()
        {
            return View();
        }
        public async Task<IActionResult>Dashboard(int Year=0)
        {
            try
            {
                DashboardDetails DD = new DashboardDetails();
                if (Convert.ToInt32(User.FindFirst("Userid")?.Value) == 5)
                {
                    ViewBag.Blockid = 0;
                    ViewBag.Name = "District";
                }
                else
                {
                    ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                    ViewBag.Name = "Gram Panchayat";
                }
                if (Convert.ToInt32(User.FindFirst("RoleId")?.Value) == 4)
                {
                    ViewBag.Name = "District";
                }
                else if(Convert.ToInt32(User.FindFirst("RoleId")?.Value) == 3)
                {
                    ViewBag.Name = "Block";
                }
                else
                {
                    ViewBag.Name = "Dept";
                }
                if (Year == 0)
                {
                    DD.Year = DateTime.Now.Year;
                }
                else
                {
                    DD.Year = Year;
                }
                ViewBag.Year = DD.Year;
                DD.userid = User.FindFirst("Userid")?.Value.ToInt();
                ViewBag.UserId = DD.userid;
                ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                DD.urtype = User.FindFirst("URTYPE")?.Value;
                List<DashboardDetails> result = await _ApprovalConfigRepository.DashboardDetails(DD);
                ViewBag.DBC = await _ApprovalConfigRepository.Dashboardcount(User.FindFirst("Userid")?.Value!);
                if(DD.userid==5 || DD.userid==1)
                {
                    DD.userid = 0;
                }
                List<DashboardTroupeDetailsCount> result1= await _ApprovalConfigRepository.DashboardTroupeDetailsCount(DD);
                List<DashboardEventDetailsCount> result2 = await _ApprovalConfigRepository.DashboardEventDetailsCount(DD);
                List<DashboardArtFormDetailsCount> result3 = await _ApprovalConfigRepository.DashboardArtFormDetailsCount(DD);
                List<DashboardArtFormDetailsSummary> result4 = await _ApprovalConfigRepository.DashboardArtFormSummaryDetails(DD);
                List<DashboardUpcommingEvent> result5 = await _ApprovalConfigRepository.DashboardUpcomingEventCount(DD);
                DashboardTroupeEventSummaryCountRes objDistlist = await _ApprovalConfigRepository.DashboardTroupeEventSummaryCount(DD);
                List<DashboardEventSummary> objevent = await _ApprovalConfigRepository.DashboardEventSummaryCount(DD);
                List<DashboardPaymentSummary> objpayment = await _ApprovalConfigRepository.DashboardPaymentSummaryCount(DD);
               
                if (objpayment != null)
                {
                    ViewBag.DashboardPaymentSummaryCount = objpayment;
                }
                if (objevent != null)
                {
                    ViewBag.DashboardEventSummaryCount = objevent;
                }
                if (objDistlist != null)
                {
                    ViewBag.DashboardTroupeBlockEvent = objDistlist.DashboardTroupeBlockEvent;
                    ViewBag.DashboardTroupeDistEvent = objDistlist.DashboardTroupeDistEvent;
                }
                if (result4!=null)
                {
                    ViewBag.DAFS = result4;
                }
                else
                {
                    ViewBag.DAFS = null;
                }
                if (result3 != null)
                {
                    ViewBag.DAFD = result3;
                }
                else
                {
                    ViewBag.DAFD = null;
                }
                if (result2 != null)
                {
                    ViewBag.DED = result2;
                }
                else
                {
                    ViewBag.DED = null;
                }
                if (result1 != null)
                {
                    ViewBag.DTD = result1;
                }
                else
                {
                    ViewBag.DTD = null;
                }
                if (result1 != null)
                {
                    ViewBag.Loginreport = result;
                }
                else
                {
                    ViewBag.Loginreport = null;
                }
                if (result5 != null)
                {
                    ViewBag.DUEC = result5;
                }
                else
                {
                    ViewBag.DUEC = null;
                }
                return View();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> GetData(int Year = 0)
        {
            try
            {
                DashboardDetails DD = new DashboardDetails();
                DD.Year = Year;
                List<DashboardEventSummary> objevent = await _ApprovalConfigRepository.DashboardEventSummaryCount(DD);
                if (objevent != null)
                {
                    ViewBag.DashboardEventSummaryCount = objevent;
                }
                return Content(new AjaxResult { state = ResultType.success.ToString(),  data = objevent }.ToJson());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetTroupDetails(int distid,int blockid,int gpid,string fromdate, string todate)
        {
            var userid = User.FindFirst("Userid")?.Value;
            DashboardDetails DD = new DashboardDetails()
            {
                distid=distid,
                blockid=blockid,
                gpid=gpid,
                fromdate=fromdate,  
                todate=todate
            };
            List<DashboardDetails> result = await _ApprovalConfigRepository.DashboardDetails(DD!);
            ViewBag.DBC = await _ApprovalConfigRepository.Dashboardcount(userid!);
            ViewBag.Loginreport = result;
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Successfully Fetched Records.", data = result }.ToJson());
        }
        [HttpGet]
        public IActionResult AdminTroupeRegistration(int TroupeId = 0)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                    ViewBag.urtype= User.FindFirst("URTYPE")?.Value;
                    ViewBag.Troupeid = TroupeId;
                    ViewBag.ButtonName = (TroupeId == 0) ? "Submit" : "Update";
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
        [RequestSizeLimit(50_000_000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminTroupeRegistration(M_Troupe_Registration P_TUS)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
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
                        var UploadFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        P_TUS.UploadFolderPath = UploadFolder;
                        string webrootpath = _hostingEnvironment.WebRootPath;
                        string ProcDocPath = Path.Combine(webrootpath, flodername);
                        string TroupeDocPath = Path.Combine(ProcDocPath, UploadFolder);
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
                        P_TUS.CreatedBy =Convert.ToInt32(User.FindFirst("UID")?.Value);
                        P_TUS.UploadFolderPath = UploadFolder;
                        P_TUS.ComponentXml = xEle;
                        P_TUS.Action = (P_TUS.TroupeId != 0) ? "AU" : "AI";
                        string retval = await _ApprovalConfigRepository.AdminTroupeRegistration(P_TUS);
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
                        else if (ackno[1].ToString() == "3")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record Already Exists!!!", data = ackno[1], AcknowledgementNo = ackno[0] }.ToJson());
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
        [RequestSizeLimit(50_000_000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(M_Troupe_Registration P_TUS)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
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
                
                P_TUS.CreatedBy = Convert.ToInt32(User.FindFirst("UID")?.Value);
                P_TUS.Action = (P_TUS.TroupeId != 0) ? "AU" : "AI";
                string retval = await _ApprovalConfigRepository.AdminTroupeRegistration(P_TUS);
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
        public IActionResult ApprovalTakeAction()
        {
            return View();
        }
        public IActionResult ApprovalList()
        {
            return View();
        }
        public IActionResult MasterAdd()
        {
            return View();
        }
        public IActionResult MasterView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ViewNewRegistration()
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
        public async Task<IActionResult> ViewNewRegistration(TroupeRegistrationView TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                        ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                        IList<TroupeRegistrationView> objDistlist =await _ApprovalConfigRepository.ViewNewRegistration(TRV);
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing",data="" }.ToJson());
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
        public async Task<IActionResult> GetMemberDetails(int TROUPEID = 0)
        {
            try
            {
                MemberDetailsView dataModel = new MemberDetailsView();
                dataModel.TroupeId = TROUPEID;
                var objresult = await _ApprovalConfigRepository.MemberDetails(dataModel);
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
        [HttpGet]
        public async Task<IActionResult> ApprovalTakeActionView(string TroupeId,int Status)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        if (Status == 0)
                        {
                            ViewBag.Title = "Troupe Approval Pending";
                        }
                        else if (Status == 1)
                        {
                            ViewBag.Title = "Troupe Approval Approved";
                        }
                        else if (Status == 2)
                        {
                            ViewBag.Title = "Troupe Approval Rejected";
                        }
                        else if (Status == 4)
                        {
                            ViewBag.Title = "Troupe Approval Not Appeared In Audition";
                        }

                        TroupeApprovalViewRes objDistlist = await _ApprovalConfigRepository.ApprovalTakeActionView(TroupeId, Status);
                        ViewBag.Data = objDistlist;
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovalTakeAction(ApprovalStatus Obj)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        Obj.Uid = Convert.ToInt32(User.FindFirst("Uid")?.Value);
                        var objresult = await _ApprovalConfigRepository.ApprovalTakeAction(Obj);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record Found", data = objresult }.ToJson());
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.warning.ToString(), message = ex.Message, data = "" }.ToJson());
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
        public IActionResult ApprovalViewPending()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovalViewPending(TroupeApproval TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        int Roleid = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                        if (Roleid == 4)
                        {
                            TRV.DistId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (Roleid == 3)
                        {
                            TRV.BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        IList<TroupeApproval> objDistlist = _ApprovalConfigRepository.ApprovalView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Error occurred while showing",data=null }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message,data=null }.ToJson());
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
        public IActionResult ApprovalViewApproved()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovalViewApproved(TroupeApproval TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        int Roleid = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                        if (Roleid == 4)
                        {
                            TRV.DistId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (Roleid == 3)
                        {
                            TRV.BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        IList<TroupeApproval> objDistlist = _ApprovalConfigRepository.ApprovalView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing", data = null }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message, data = null }.ToJson());
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
        public IActionResult ApprovalViewRejected()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovalViewRejected(TroupeApproval TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        int Roleid = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                        if (Roleid == 4)
                        {
                            TRV.DistId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (Roleid == 3)
                        {
                            TRV.BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        IList<TroupeApproval> objDistlist = _ApprovalConfigRepository.ApprovalView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing",data=null }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message, data = null }.ToJson());
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
        public IActionResult ApprovalViewHold()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovalViewHold(TroupeApproval TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        int Roleid = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                        if (Roleid == 4)
                        {
                            TRV.DistId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (Roleid == 3)
                        {
                            TRV.BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        IList<TroupeApproval> objDistlist = _ApprovalConfigRepository.ApprovalView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing", data = null }.ToJson());
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message , data = null }.ToJson());
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
        public IActionResult ApprovalViewNCTA()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovalViewNCTA(TroupeApproval TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        int Roleid = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                        if (Roleid == 4)
                        {
                            TRV.DistId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (Roleid == 3)
                        {
                            TRV.BlockId = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        IList<TroupeApproval> objDistlist = _ApprovalConfigRepository.ApprovalView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing", data = null }.ToJson()); ;
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message , data = null }.ToJson());
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
        public async Task<JsonResult> TroupeEventList()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int Troupeid = Convert.ToInt32(User.FindFirst("TroupeId")?.Value);
                try
                {
                    var data = _ApprovalConfigRepository.ViewEventDetails("TED", Troupeid);
                    return Json(new { sucess = true, responseMessage = "Action taken Sucessfully.", responseText = "Sucess", data = data });
                }
                catch (Exception ex)
                {
                    return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Error", data = "" });
                }
            }
            else
            {
                return Json(new { sucess = true, responseMessage = "Action taken Sucessfully.", responseText = "Sucess", data = "" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotificationCount()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                DashboardDetails DD = new DashboardDetails();
                DD.userid = User.FindFirst("Userid")?.Value.ToInt();
                List<DashboardTroupeDetailsCount> NotiCount = await _ApprovalConfigRepository.NotificationCount(DD);
                if (NotiCount != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = NotiCount }.ToJson());
                }
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
    }
}
