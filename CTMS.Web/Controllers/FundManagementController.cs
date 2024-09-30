using CTMS.Core;
using CTMS.Model.Entities.FundMaster;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Controllers
{
    [Authorize]
    public class FundManagementController : Controller
    {
        private readonly IMemCache _memCache;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IRegistraionRepository _RegistraionRepository;
        private readonly IFundManagementRepository _FundManagementRepository;
        public FundManagementController(IFundManagementRepository FundManagementRepository, IWebHostEnvironment hostingEnvironment, IRegistraionRepository RegistraionRepository, IMemCache memCache)
        {
            _memCache = memCache;
            _RegistraionRepository = RegistraionRepository;
            _FundManagementRepository = FundManagementRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        #region Region Will be Delete
        public IActionResult FundManagement()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("Uid")?.Value == "4")
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
        public IActionResult FundManagement(FundMaster TBL)
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
                TBL.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var data = _FundManagementRepository.FundManage(TBL);
                return Json(new { sucess = true, responseMessage = "Fund Transfer Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetOpeningBalance(FundMaster FM)
        {
            try
            {
                var objresult = await _FundManagementRepository.GetOpeningBalance(FM);
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
        public IActionResult FundAssignForBlock()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("Uid")?.Value == "3")
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
        #endregion
        #region FundManage

        public IActionResult AddOpeningBalance()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("RoleId")?.Value == "4")
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
        public IActionResult AddOpeningBalance(FundMaster TBL)
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
                TBL.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var data = _FundManagementRepository.AddOpeningBalance(TBL);
                return Json(new { sucess = true, responseMessage = "Opening Balance Added Successfully.", responseText = "Success", data = data });

            }
        }
        [HttpPost]
        public async Task<IActionResult> ViewOpeningBalance()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("RoleId")?.Value == "4")
                    {
                        var Distid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        var objDistlist =await _FundManagementRepository.ViewOpeningBalance(Distid);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
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
        public async Task<IActionResult> ViewFund()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("RoleId")?.Value == "4")
                    {
                        var Distid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        var objDistlist =await _FundManagementRepository.ViewFund(Distid);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
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
        public async Task<IActionResult> ViewFundTransfer()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("RoleId")?.Value == "4")
                    {
                        var Distid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        var objDistlist =await _FundManagementRepository.ViewFundTransfer(Distid);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
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
        public IActionResult AddFund()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("RoleId")?.Value == "4")
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
        public IActionResult AddFund(FundMaster TBL)
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
                TBL.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var data = _FundManagementRepository.AddFund(TBL);
                return Json(new { sucess = true, responseMessage = "Fund Added Successfully.", responseText = "Success", data = data });

            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAccountNo(FundMaster FM)
        {
            try
            {
                var objresult = await _FundManagementRepository.GetAccountNo(FM);
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
        [HttpPost]
        public async Task<IActionResult> GetReceiverAccountNo(FundMaster FM)
        {
            try
            {
                var objresult = await _FundManagementRepository.GetReceiverAccountNo(FM);
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
        public IActionResult FundTransfer()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("RoleId")?.Value == "4")
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
        public IActionResult FundTransfer(FundMaster TBL)
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
                TBL.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var data = _FundManagementRepository.FundTransfer(TBL);
                return Json(new { sucess = true, responseMessage = "Fund Transfer Successfully.", responseText = "Success", data = data });

            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAvailableBalance(FundMaster FM)
        {
            try
            {
                var objresult = await _FundManagementRepository.GetAvailableBalance(FM);
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
        [HttpPost]
        public async Task<IActionResult> OpeningBalanceexist(FundMaster FM)
        {
            try
            {
                var objresult = await _FundManagementRepository.OpeningBalanceexist(FM);
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
        #endregion
        [HttpGet]
        public async Task<IActionResult> PendingPayment(string sdate,string edate)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int Blockid = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                ViewBag.FromDate = sdate; ViewBag.ToDate = edate;
                ViewBag.PendingPayment = await _FundManagementRepository.PendingPayment(Blockid, sdate, edate);
            }
            return View();
        }
        [HttpGet]
        public IActionResult TroupePayment(Payment PMT)
        {
            TroupeRegistrationView TRV = new TroupeRegistrationView();
            TRV.TroupeId = PMT.TroupeId;
            TRV.Status = PMT.STATUS;
            TroupeReportingView Trv = new TroupeReportingView();
            Trv.AssignEventId = PMT.AssignEventId;
            ViewBag.TroupeDetails = _RegistraionRepository.TroupeReporting(Trv).Result.ToList();
            ViewBag.TroupeReportingDetails = _RegistraionRepository.TroupeReportingDetails_Payment(TRV).Result.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TroupePaymentADD(Payment PMT)
        {
            int Blockid = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            TroupeRegistrationView TRV = new TroupeRegistrationView();
            TRV.TroupeId = PMT.TroupeId;
            TRV.Status = PMT.STATUS;
            TroupeReportingView Trv = new TroupeReportingView();
            Trv.AssignEventId = PMT.AssignEventId;
            ViewBag.TroupeDetails = _RegistraionRepository.TroupeReporting(Trv).Result.ToList();
            ViewBag.TroupeReportingDetails = _RegistraionRepository.TroupeReportingDetails_Payment(TRV).Result.ToList();
            //File Upload Start
            string flodername = "TroupePayment";
            var UploadFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            PMT.UploadFolderPath = UploadFolder;
            string webrootpath = _hostingEnvironment.WebRootPath;
            string ProcDocPath = Path.Combine(webrootpath, flodername);
            if (!Directory.Exists(ProcDocPath))
                Directory.CreateDirectory(ProcDocPath);
            var UploadProof = Request.Form.Files["UploadProof"];
            string EventId = Path.Combine(ProcDocPath, PMT.AssignEventId.ToString());
            if (!Directory.Exists(EventId))
                Directory.CreateDirectory(EventId);
            string TroupeId = Path.Combine(EventId, PMT.TroupeId.ToString());
            if (!Directory.Exists(TroupeId))
                Directory.CreateDirectory(TroupeId);
            string Datetime = Path.Combine(TroupeId, UploadFolder);
            if (!Directory.Exists(Datetime))
                Directory.CreateDirectory(Datetime);
            if (PMT.UploadProof != null)
            {
                if (!Directory.Exists(Datetime))
                    Directory.CreateDirectory(Datetime);
                var extn = "." + UploadProof.FileName.Split('.')[UploadProof.FileName.Split('.').Length - 1];
                PMT.UploadProofFileName = UploadFolder + extn;
                using (var stream = new FileStream(Path.Combine(Datetime, PMT.UploadProofFileName), FileMode.Create))
                {
                    await PMT.UploadProof.CopyToAsync(stream);
                }
            }
            else
            {
                PMT.UploadProof = PMT.UploadProof;
            }
            PMT.BlockId = Blockid;
            ViewBag.PaymentConfirmation = await _FundManagementRepository.PaymentAddUpdate(PMT);
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Troupe Payment Saved Successfully", data = ViewBag.PaymentConfirmation }.ToJson());
        }
        [HttpGet]
        public async Task<IActionResult> SuccessPayment(string sdate,string edate)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int Blockid = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                ViewBag.FromDate = sdate; ViewBag.ToDate = edate;
                ViewBag.SuccessPayment = await _FundManagementRepository.SuccessPayment(Blockid,sdate,edate);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SuccessPaymentDetails(int AssignEventId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int Blockid = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                ViewBag.SuccessPayment = await _FundManagementRepository.SuccessPaymentDetails(AssignEventId, Blockid);
            }
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.SuccessPayment }.ToJson());
        }
    }
}
