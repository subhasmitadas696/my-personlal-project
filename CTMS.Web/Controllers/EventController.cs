using CTMS.Core;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repository;
using CTMS.Web.Areas.AdminConsole.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Xml.Linq;
#pragma warning disable SCS0016, CS1998
// Since the antiforgery token is globally managed, there's no need to include the "validateAntiforgeryToken" attribute for each POST request.
namespace CTMS.Web
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventRepository _EventRepository;
        private readonly IApprovalConfigRepository _ApprovalConfigRepository;
        private readonly IPaymentStructureRepository _PaymentStructureRepository;
        public EventController(IEventRepository EventRepository, IApprovalConfigRepository ApprovalConfigRepository, IPaymentStructureRepository paymentStructureRepository)
        {
            _EventRepository = EventRepository;
            _ApprovalConfigRepository = ApprovalConfigRepository;
            _PaymentStructureRepository = paymentStructureRepository;
        }
        public IActionResult UnauthorizedAccess()
        {
            return View();
        }
        public IActionResult ViewEvent()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value != "0")
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
        [HttpGet]
        public IActionResult ManageEvent()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value != "0")
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
        public async Task<IActionResult> ManageEvent(EventDetails ED)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value != "0")
                {
                    try
                    {
                        ED.CreatedBy = Convert.ToInt32(User.FindFirst("USERID")?.Value);
                        int retval = await _EventRepository.ManageEventDetails(ED);
                        if (retval == 1)
                        {
                            return Json(new { sucess = true, responseMessage = "Event Created Successfully.", responseText = "Success", data = retval });
                        }
                        else if (retval == 2)
                        {
                            return Json(new { sucess = true, responseMessage = "Event Updated Successfully.", responseText = "Success", data = retval });
                        }
                        else if (retval == 10)
                        {
                            return Json(new { sucess = true, responseMessage = "Troupe already assigned, event cannot be rescheduled. For reschedule you have to cancel the event!", responseText = "warning", data = retval });
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error in creating events", data = retval }.ToJson());
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
        public IActionResult GetEventList(EventDetails ED)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {

                int distid = Convert.ToInt32(User.FindFirst("UserID")?.Value);
                try
                {
                    IList<EventDetails> list = _EventRepository.ViewEventDetails(0, "R", distid, "", "");
                    if (list != null)
                    {
                        return Json(list);
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
                return RedirectToAction("SessionOut", "Home");
            }
        }
        [HttpGet]
        public IActionResult GetSingleEvent(int Id)
        {
            int distid = Convert.ToInt32(User.FindFirst("Userid")?.Value);
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        IList<EventDetails> list = _EventRepository.ViewEventDetails(Id, "R", distid, "", "");
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
        [HttpPost]
        public async Task<JsonResult> DeleteEvent(int Id, int Status)
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
                EventDetails ob = new EventDetails();
                ob.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                ob.EventId = Id;
                ob.Status = Status;
                var data = await _EventRepository.DeleteEvent(ob);
                return Json(new { sucess = true, responseMessage = "successfully.", responseText = "Success", data = data });
            }
        }

        public IActionResult ManageEventCalender()
        {
            ViewBag.Cancel = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        public IActionResult EventCalender()
        {
            int eventid = 0;
            if (User.FindFirst("Userid")?.Value != null)
            {
                ViewBag.urtype = User.FindFirst("URTYPE")?.Value.ToString().ToUpper();
                int P_DIST_BLK_ID = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                ViewBag.CalendarData = _EventRepository.ViewEventDetails(eventid, "R", P_DIST_BLK_ID, "", "");
                ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                return View();
            }
            else
            {
                return View("SessionOut");
            }
        }
        public async Task<IActionResult> GetSchemeWiseEventDetails(int deptid, int scehemeid, int distid, int blockid)
        {

            int distblockid = User.FindFirst("URTYPE")?.Value.ToString().ToUpper() == "DSSO" ? distid : blockid;
            var res = await _ApprovalConfigRepository.DashboardDetailsByScheme(deptid, scehemeid, distblockid);
            if (res.TOTALFUND != null)
            {
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "record fetched sucessfull", data = res }.ToJson());
            }
            else
            {
                return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "record not found", data = "" }.ToJson());
            }

        }
        [HttpGet]
        public async Task<JsonResult> eventlist()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int distid = Convert.ToInt32(User.FindFirst("UserID")?.Value);
                
                try
                {
                    var data = _EventRepository.ViewEventDetails(0, "R", distid, "", "");
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

        public IActionResult AssignTroup()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value != "0")
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
        public IActionResult GetTroup(int Id = 0)
        {
            int distid = Convert.ToInt32(User.FindFirst("UserID")?.Value);
            try
            {
                var data = _EventRepository.ViewEventDetails(Id, "T", distid, "", "");
                return Json(new { sucess = true, responseMessage = "Action taken Sucessfully.", responseText = "Sucess", data = data });


            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        public IActionResult AssignTroupView()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null && User.FindFirst("UID")?.Value != "0")
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
        public IActionResult DetailEventList(string Actioncode,string fromDate,string toDate)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                int distid = Convert.ToInt32(User.FindFirst("UserID")?.Value);
                try
                {
                    IList<EventDetails> list = _EventRepository.ViewEventDetails(0, Actioncode, distid, fromDate, toDate);
                    if (list != null)
                    {
                        return Json(list);
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
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult EditAssignTroupeEvent(string EventId, string AssignEventId)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                    ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                    ViewBag.EventDetails = _EventRepository.GetOneEventDetails(EventId, "EDITAT", AssignEventId);
                    ViewBag.EventId = EventId;
                    ViewBag.AssignEventId = AssignEventId;
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
        public IActionResult AssignTroupeEvent(string eventid,string Type="")
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    int P_DIST_BLK_ID = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    ViewBag.Blockid = User.FindFirst("Userid")?.Value;
                    ViewBag.urtype = User.FindFirst("URTYPE")?.Value;
                    ViewBag.EventDetails = _EventRepository.ViewEventDetails(Convert.ToInt32(eventid), "R", P_DIST_BLK_ID,"","");
                    ViewBag.ButtonName = (Type == "") ? "Submit" : "Update";
                    ViewBag.EventId = eventid;
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
        public async Task<IActionResult> AssignTroupeEvent(EventDetails eventDetails)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    try
                    {
                        var data = HttpContext.Request.Form["Elements"];
                        var ResultDtls = JsonConvert.DeserializeObject<List<TroupeDetails>>(data!);
                        var xEle = new XElement("AssignDetails",
                                from emp in ResultDtls
                                select new XElement("AssignData",
                                    new XElement("Village", emp.Village),
                                    new XElement("Area", emp.Area),
                                    new XElement("DateofPerform", emp.DateofPerform),
                                    new XElement("StartTime", emp.StartTime),
                                    new XElement("EndTime", emp.EndTime),
                                    new XElement("OfficerName", emp.OfficerName),
                                    new XElement("OfficerMobNo", emp.OfficerMobNo),
                                    new XElement("ImgFolderID",emp.ImgFolderID)
                        ));
                        eventDetails.ComponentXml = xEle;
                        eventDetails.CreatedBy = Convert.ToInt32(User.FindFirst("UID")?.Value);
                        string retval = await _EventRepository.AssignTroupe(eventDetails);
                        if (retval == "1")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Troupe Assigned Successfully.", data = Convert.ToInt32(retval) }.ToJson());
                        }
                        else if (retval == "2")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Troupe Updated Successfully.", data = Convert.ToInt32(retval) }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Something went wrong!" }.ToJson());
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
        public IActionResult GetTroupes(int CatId = 0, int SubCatId = 0, int BlockId = 0)
        {

            try
            {
                var objDistlist = _EventRepository.ViewTroupes(CatId, SubCatId, BlockId).Result;
                if (objDistlist != null)
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

        public async Task<IActionResult> RateChartForEvent()
        {
            return View();
        }
        public async Task<IActionResult> RateChart()
        {
            try
            {

                var data = _PaymentStructureRepository.ViewPaymentStructure().Result;

                return Json(data);

            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteAssignTroupe(int Id)
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
                EventDetails ob = new EventDetails();
                ob.EventId = Id;
                ob.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var data = await _EventRepository.DeleteAssignTroupe(ob);
                return Json(new { sucess = true, responseMessage = "AssignTroupe deleted successfully.", responseText = "Success", data = data });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TroupAssignStatus(int troupid,string dateofPerform,string starttime)
        {
            int troupeAssignCount=await _EventRepository.troupeAssignCount(troupid,dateofPerform,starttime);
            if (troupeAssignCount > 0)
            {
                return Json(new AjaxResult {state=ResultType.warning.ToString(),message="this troupe is already assigned to a event on this time slot",data= troupeAssignCount });
            }
            else
            {
                return Json(new AjaxResult { state = ResultType.success.ToString(), message = "no record found", data = troupeAssignCount } );
            }
        }
    }
}
