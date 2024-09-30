using CTMS.Core;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.ReportMaster;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Controllers
{
    [Authorize]
    public class MISReportsController : Controller
    {
        public IConfiguration Configuration;
        private readonly IReportRepository _ReportRepository;
        public MISReportsController(IConfiguration configuration, IReportRepository ReportRepository)
        {
            _ReportRepository = ReportRepository;
            Configuration = configuration;
        }

        public IActionResult Block_Dist_Reg_ReportsView()
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    BlockWiseRegistration TRV = new BlockWiseRegistration();
                    ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    TRV.DistId = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                    TRV.BlockId = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    var Userid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    if (Userid==5){
                        TRV.BlockId = 0;
                    }
                    ViewBag.DistId = TRV.DistId;
                    ViewBag.Blockid = TRV.BlockId;
                    TRV.GPId = 0;
                    ViewBag.GPId = 0;
                    IList<BlockWiseRegistration> objDistlist = _ReportRepository.BlockWiseTroupeRegReport(TRV).Result.ToList();
                    ViewBag.Detail = objDistlist;
                    return View();
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
        public IActionResult Block_Dist_Reg_ReportsView1(int DistId,int BlockId,int GPId)
        {
            try
            {
                BlockWiseRegistration TRV = new BlockWiseRegistration();
                TRV.DistId = DistId;
                TRV.BlockId = BlockId;
                TRV.GPId = GPId;
                ViewBag.DistId = TRV.DistId;
                ViewBag.Blockid = TRV.BlockId;
                ViewBag.GPId = TRV.GPId;
                ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                IList<BlockWiseRegistration> objDistlist = _ReportRepository.BlockWiseTroupeRegReport(TRV).Result.ToList();
                ViewBag.Detail = objDistlist;
                return View("Block_Dist_Reg_ReportsView");
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        public IActionResult Block_Dist_Reg_ReportsViewSec()
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    BlockWiseRegistration TRV = new BlockWiseRegistration();
                    TRV.DistId = 0;
                    TRV.BlockId = 0;
                    TRV.GPId = 0;
                    ViewBag.DistId = TRV.DistId;
                    ViewBag.Blockid = TRV.BlockId;
                    ViewBag.GPId = 0;
                    IList<BlockWiseRegistration> objDistlist = _ReportRepository.BlockWiseTroupeRegReport(TRV).Result.ToList();
                    ViewBag.Detail = objDistlist;
                    return View();
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
        [HttpPost]
        public IActionResult Block_Dist_Reg_ReportsViewSec(BlockWiseRegistration TRV)
        {
            try
            {
                ViewBag.DistId = TRV.DistId;
                ViewBag.Blockid = TRV.BlockId;
                ViewBag.UserType = User.FindFirst("USERTYPE")?.Value;
                ViewBag.GPId = TRV.GPId;
                IList<BlockWiseRegistration> objDistlist = _ReportRepository.BlockWiseTroupeRegReport(TRV).Result.ToList();
                ViewBag.Detail = objDistlist;
                return View();
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        public async Task<IActionResult> EventReport(EventReport TRV)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ViewBag.URTYPE = User.FindFirst("URTYPE")?.Value;
                    ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    if (ViewBag.URTYPE == "Admin" || ViewBag.URTYPE == "DEPT")
                    {
                        if (TRV.DistId == 0 && TRV.BlockId == 0)
                        {
                            TRV.BlockId = 0;
                            TRV.DistId = 0;
                        }
                        else if (TRV.DistId != 0 && TRV.BlockId == 0)
                        {
                            TRV.BlockId = 0;
                        }
                    }
                    else
                    {
                        if (TRV.DistId == 0 && TRV.BlockId == 0)
                        {
                            TRV.DistId = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                            TRV.BlockId = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        }
                        else if (TRV.DistId == 0 && TRV.BlockId != 0)
                        {
                            TRV.DistId = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        }
                    }


                    ViewBag.Distid = TRV.DistId;
                    ViewBag.Blockid = TRV.BlockId;
                    ViewBag.Detail = _ReportRepository.EventReport(TRV).Result.ToList();
                    return View();                    
                }
                else
                {
                    return View("SessionOut");
                }
            }
            catch(Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [HttpPost]
        public async Task<IActionResult> EventReport(int DistId, int BlockId, int GpId)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    ViewBag.DistId = DistId;
                    ViewBag.Blockid = BlockId;
                    TRV.DistId = DistId;
                    TRV.BlockId = BlockId;
                    TRV.GPId = GpId;
                    ViewBag.Detail = _ReportRepository.EventReport(TRV).Result.ToList();
                    return View();
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
        public async Task<IActionResult> EventReportDD(int DistId, int BlockId, int GpId)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                    ViewBag.DistId = DistId;
                    ViewBag.Blockid = BlockId;
                    TRV.DistId = DistId;
                    TRV.BlockId = BlockId;
                    TRV.GPId = GpId;
                    ViewBag.Detail = _ReportRepository.EventReport(TRV).Result.ToList();
                    return View("EventReport");                    
                }
                else
                {
                    return View("SessionOut");
                }
            }
            catch(Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [HttpPost]
        public async Task<IActionResult> EventReportPopUpDetails(int DistId, int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    ViewBag.Detail = await _ReportRepository.EventReportPopUpDetails(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.Detail }.ToJson());
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
        [HttpGet]
        public async Task<IActionResult> TotalRegistrationpopup(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ArtWiseReport TRV = new ArtWiseReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    var objDistlist =await _ReportRepository.TotalRegistrationpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> Totalmemberpopup(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ArtWiseReport TRV = new ArtWiseReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    var objDistlist =await _ReportRepository.Totalmemberpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> TotalmemberPendingpopup(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ArtWiseReport TRV = new ArtWiseReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    var objDistlist =await _ReportRepository.TotalmemberPendingpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> TotalmemberApprovedpopup(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ArtWiseReport TRV = new ArtWiseReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    var objDistlist =await _ReportRepository.TotalmemberApprovedpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> TotalRejectededpopup(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    ArtWiseReport TRV = new ArtWiseReport();
                    TRV.DistId = DistId;
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    var objDistlist =await _ReportRepository.TotalRejectededpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        [HttpPost]
        public async Task<IActionResult> EventUpcomingDetails(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;                   
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    ViewBag.Detail =await _ReportRepository.EventUpcomingDetails(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.Detail }.ToJson());
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
        [HttpPost]
        public async Task<IActionResult> EventCompleteDetails(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;                   
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    ViewBag.Detail =await _ReportRepository.EventCompleteDetails(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.Detail }.ToJson());
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
        [HttpPost]
        public async Task<IActionResult> EventRescheduleDetails(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;                   
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    ViewBag.Detail =await _ReportRepository.EventRescheduleDetails(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.Detail }.ToJson());
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
        [HttpPost]
        public async Task<IActionResult> EventCancelDetails(int DistId,int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    EventReport TRV = new EventReport();
                    TRV.DistId = DistId;                   
                    TRV.isdist = isdist;
                    ViewBag.DistId = DistId;
                    ViewBag.Detail =await _ReportRepository.EventCancelDetails(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = ViewBag.Detail }.ToJson());
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
        public IActionResult ArtFormWiseTroupe(ReportView TRV)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                int DISTCODE = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                string UserType = User.FindFirst("URTYPE")?.Value!;
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
                ViewBag.UserType = TRV.UserType;

                ViewBag.CatId = TRV.CatId;
                ViewBag.SubCatId = TRV.SubCatId;
                ViewBag.Detail = _ReportRepository.DashboardRegistration(TRV).Result.ToList();
                return View();
            }
            else
            {
                return View("SessionOut");
            }
        }
        public IActionResult ArtFormWiseTroupeC(int DistId, int BlockId, int CatId, int SubCatId)
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
                return View("ArtFormWiseTroupe");
            }
            else
            {
                return View("SessionOut");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Distwise_TroupeReg(DistWise_TroupeReport DT)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                DT.UserType = User.FindFirst("URTYPE")?.Value;
                ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
                if (DT.UserType == "Admin" || DT.UserType == "DEPT")
                {
                    if (DT.Distid == 0 && DT.Blockid == 0)
                    {
                        DT.Blockid = 0;
                        DT.Distid = 0;
                    }
                    else if (DT.Distid != 0 && DT.Blockid == 0)
                    {
                        DT.Blockid = 0;
                    }
                }
                else
                {
                    if (DT.Distid == 0 && DT.Blockid == 0)
                    {
                        DT.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DT.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    else if (DT.Distid == 0 && DT.Blockid != 0)
                    {
                        DT.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                    }
                }


                ViewBag.Distid = DT.Distid;
                ViewBag.Blockid = DT.Blockid;
                ViewBag.Gpid = DT.Gpid;
                ViewBag.CatId = 0;
                ViewBag.Distwise_TroupeRegDetails = await _ReportRepository.Distwise_TroupeReg(DT);
                return View();
            }
            else
            {
                return View("SessionOut");
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> PaymentReport(PaymentReport TRV)
        {
            int DISTCODE = Convert.ToInt32(User.FindFirst("Userid")?.Value);
            string UserType = User.FindFirst("URTYPE")?.Value!;
            int UserRole =Convert.ToInt32(User.FindFirst("RoleId")?.Value);
            TRV.UserType = UserType;
            ViewBag.UserType = UserType;
            TRV.UserType = User.FindFirst("URTYPE")?.Value;
            ViewBag.UserType = Convert.ToInt32(User.FindFirst("RoleId")?.Value);
            if (TRV.UserType == "Admin" || TRV.UserType == "DEPT")
            {
                if (TRV.Distid == 0 && TRV.Blockid == 0)
                {
                    TRV.Blockid = 0;
                    TRV.Distid = 0;
                }
                else if (TRV.Distid != 0 && TRV.Blockid == 0)
                {
                    TRV.Blockid = 0;
                }
            }
            else
            {
                if (TRV.Distid == 0 && TRV.Blockid == 0)
                {
                    TRV.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                    TRV.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                }
                else if (TRV.Distid == 0 && TRV.Blockid != 0)
                {
                    TRV.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                }
            }


            ViewBag.Distid = TRV.Distid;
            ViewBag.Blockid = TRV.Blockid;
            ViewBag.PaymentDetail = await _ReportRepository.Distwise_PaymentReport(TRV);
            return View();
        }        
        public IActionResult PaymentReportDD(int Distid, int Blockid)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                PaymentReport TRV = new PaymentReport();
                TRV.Distid = Distid;
                TRV.Blockid = Blockid;
                ViewBag.DistId = Distid;
                ViewBag.Blockid = Blockid;
                ViewBag.PaymentDetail = _ReportRepository.Distwise_PaymentReport(TRV).Result.ToList();
                return View("PaymentReport");
            }
            else
            {
                return View("SessionOut");
            }
        }
        
        public IActionResult FormDistWiseTroupeC(int DistId, int BlockId, int CatId, int SubCatId)
        {
            if (User.FindFirst("Userid")?.Value != null)
            {
                DistWise_TroupeReport TRV = new DistWise_TroupeReport();
                TRV.Distid = DistId;
                TRV.Blockid = BlockId;
                TRV.Categoryid = CatId;
                TRV.SubCategoryid = SubCatId;
                ViewBag.DistId = TRV.Distid;
                ViewBag.BlockId = TRV.Blockid;
                ViewBag.CatId = CatId;
                ViewBag.Distwise_TroupeRegDetails = _ReportRepository.Distwise_TroupeReg(TRV).Result.ToList();
                return View("Distwise_TroupeReg");
            }
            else
            {
                return View("SessionOut");
            }
        }
        public async Task<IActionResult> DistWiseTotalRegistrationpopup(int Categoryid, int isCat)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    DistWise_TroupeReport DWTR = new DistWise_TroupeReport();
                    DWTR.UserType = User.FindFirst("URTYPE")?.Value;
                    if (DWTR.UserType == "Admin" || DWTR.UserType == "DEPT")
                    {
                        DWTR.Blockid = 0;
                        DWTR.Distid = 0;
                    }
                    else
                    {
                        DWTR.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DWTR.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    DWTR.Categoryid = Categoryid;
                    DWTR.isCat = isCat;
                    ViewBag.Categoryid = Categoryid;
                    var objCatlist = await _ReportRepository.DistTotalRegistrationpopup(DWTR);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objCatlist }.ToJson());
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
        public async Task<IActionResult> DistWiseTotalmemberpopup(int Categoryid, int isCat)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    DistWise_TroupeReport DWTR = new DistWise_TroupeReport();
                    DWTR.UserType = User.FindFirst("URTYPE")?.Value;
                    if (DWTR.UserType == "Admin" || DWTR.UserType == "DEPT")
                    {
                        DWTR.Blockid = 0;
                        DWTR.Distid = 0;
                    }
                    else
                    {
                        DWTR.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DWTR.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    DWTR.Categoryid = Categoryid;
                    DWTR.isCat = isCat;
                    ViewBag.Categoryid = Categoryid;
                    var objCatlist = await _ReportRepository.DistWiseTotalmemberpopup(DWTR);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objCatlist }.ToJson());
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
        public async Task<IActionResult> DistWisePendingmemberpopup(int Categoryid, int isCat)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    DistWise_TroupeReport DWTR = new DistWise_TroupeReport();
                    DWTR.UserType = User.FindFirst("URTYPE")?.Value;
                    if (DWTR.UserType == "Admin" || DWTR.UserType == "DEPT")
                    {
                        DWTR.Blockid = 0;
                        DWTR.Distid = 0;
                    }
                    else
                    {
                        DWTR.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DWTR.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    DWTR.Categoryid = Categoryid;
                    DWTR.isCat = isCat;
                    ViewBag.Categoryid = Categoryid;
                    var objCatlist = await _ReportRepository.DistWisePendingmemberpopup(DWTR);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objCatlist }.ToJson());
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
        public async Task<IActionResult> DistWiseApprovememberpopup(int Categoryid, int isCat)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    DistWise_TroupeReport DWTR = new DistWise_TroupeReport();
                    DWTR.UserType = User.FindFirst("URTYPE")?.Value;
                    if (DWTR.UserType == "Admin" || DWTR.UserType == "DEPT")
                    {
                        DWTR.Blockid = 0;
                        DWTR.Distid = 0;
                    }
                    else
                    {
                        DWTR.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DWTR.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    DWTR.Categoryid = Categoryid;
                    DWTR.isCat = isCat;
                    ViewBag.Categoryid = Categoryid;
                    var objCatlist = await _ReportRepository.DistWiseApprovememberpopup(DWTR);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objCatlist }.ToJson());
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
        public async Task<IActionResult> DistWiseRejectededmemberpopup(int Categoryid, int isCat)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    DistWise_TroupeReport DWTR = new DistWise_TroupeReport();
                    DWTR.UserType = User.FindFirst("URTYPE")?.Value;
                    if (DWTR.UserType == "Admin" || DWTR.UserType == "DEPT")
                    {
                        DWTR.Blockid = 0;
                        DWTR.Distid = 0;
                    }
                    else
                    {
                        DWTR.Distid = (ViewBag.UserType == 4) ? Convert.ToInt32(User.FindFirst("Userid")?.Value) : 0;
                        DWTR.Blockid = (ViewBag.UserType == 4) ? 0 : Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    }
                    DWTR.Categoryid = Categoryid;
                    DWTR.isCat = isCat;
                    ViewBag.Categoryid = Categoryid;
                    var objCatlist = await _ReportRepository.DistWiseRejectededmemberpopup(DWTR);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objCatlist }.ToJson());
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
        public async Task<IActionResult> DistWisePaymentTotalReportedpopup(int Distid, int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    PaymentReport TRV = new PaymentReport();
                    TRV.Distid = Distid;
                    TRV.isdist = isdist;
                    ViewBag.DistId = Distid;
                    var objDistlist = await _ReportRepository.DistWisePaymentTotalReportedpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> DistWisePaymentPandingpopup(int Distid, int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    PaymentReport TRV = new PaymentReport();
                    TRV.Distid = Distid;
                    TRV.isdist = isdist;
                    ViewBag.DistId = Distid;
                    var objDistlist = await _ReportRepository.DistWisePaymentPandingpopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> DistWisePaymentSuccesspopup(int Distid, int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    PaymentReport TRV = new PaymentReport();
                    TRV.Distid = Distid;
                    TRV.isdist = isdist;
                    ViewBag.DistId = Distid;
                    var objDistlist = await _ReportRepository.DistWisePaymentSuccesspopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
        public async Task<IActionResult> DistWisePaymentFailurepopup(int Distid, int isdist)
        {
            try
            {
                if (User.FindFirst("Userid")?.Value != null)
                {
                    PaymentReport TRV = new PaymentReport();
                    TRV.Distid = Distid;
                    TRV.isdist = isdist;
                    ViewBag.DistId = Distid;
                    var objDistlist = await _ReportRepository.DistWisePaymentFailurepopup(TRV);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Record found", data = objDistlist }.ToJson());
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
    }
}
