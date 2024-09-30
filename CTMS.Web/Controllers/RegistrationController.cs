using CTMS.Core;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Security.Claims;
using System.Xml.Linq;
#pragma warning disable SCS0016
// Since the antiforgery token is globally managed, there's no need to include the "validateAntiforgeryToken" attribute for each POST request.
namespace CTMS.Web
{
    // [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IRegistraionRepository _RegistraionRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public RegistrationController(IRegistraionRepository RegistraionRepository, IWebHostEnvironment hostingEnvironment)
        {
            _RegistraionRepository = RegistraionRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult UnauthorizedAccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TroupeRegistration(int TroupeId = 0)
        {
            ViewBag.Troupeid = TroupeId;
            ViewBag.ButtonName = (TroupeId == 0) ? "Submit" : "Update";
            return View();

        }
        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TroupeRegistration(M_Troupe_Registration P_TUS)
        {
            try
            {
                ViewBag.Troupeid = P_TUS.TroupeId;
                ViewBag.ButtonName = (P_TUS.TroupeId == 0) ? "Submit" : "Update";
                var data = HttpContext.Request.Form["Elements"];
                var ResultDtls = JsonConvert.DeserializeObject<List<MemberDetails>>(data!);
                // Convert to xml / text
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
                P_TUS.UploadFolderPath = UploadFolder;
                P_TUS.ComponentXml = xEle;
                P_TUS.Action = (P_TUS.TroupeId != 0) ? "U" : "I";
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
        [HttpGet]
        public IActionResult TroupeRegistrationView()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null && User.FindFirst("TroupeId")?.Value !="0")
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
        public IActionResult TroupeRegistrationView(TroupeRegistrationView TRV)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null && User.FindFirst("TroupeId")?.Value !="0")
                {
                    try
                    {
                        IList<TroupeRegistrationView> objDistlist = _RegistraionRepository.TroupeRegistrationView(TRV).Result.ToList();
                        if (objDistlist.Count != 0)
                        {

                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
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
        [HttpPost]
        public async Task<IActionResult> GetMemberDetails(int TROUPEID = 0)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value !="0")
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
        public async Task<IActionResult> GetAllDetails(int TROUPEID = 0)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("TroupeId")?.Value != null && User.FindFirst("TroupeId")?.Value !="0")
                {
                    try
                    {
                        MemberDetailsView dataModel = new MemberDetailsView();
                        dataModel.TroupeId = TROUPEID;
                        var objresult = await _RegistraionRepository.GetAllDetails(dataModel);
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
        public IActionResult TroupeRegistrationBsso(int TroupeId = 0)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("Uid")?.Value != null && User.FindFirst("Uid")?.Value !="0")
                {
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
    }
}
