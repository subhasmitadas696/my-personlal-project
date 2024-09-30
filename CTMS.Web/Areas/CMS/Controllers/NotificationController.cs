using CTMS.Core;
using CTMS.Model.Entities.Notification;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CTMS.Web.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _NotificationRepository;
        private readonly IMemCache _memCache;
        public NotificationController(IConfiguration configuration, INotificationRepository NotificationRepository, IMemCache memCache)
        {
            _NotificationRepository = NotificationRepository;
            _memCache = memCache;
        }
        [HttpGet]
        public IActionResult NotificationMaster()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NotificationMaster(NotificationMaster TBL)
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
                if (TBL.Id == 0)
                {
                    var data = _NotificationRepository.InsertNotification(TBL);
                    _memCache.RemoveCache("Notification");
                    return Json(new { sucess = true, responseMessage = "Notification Saved Successfully.", responseText = "Success", data = data });
                }
                else
                {
                    var data = _NotificationRepository.UpdateNotification(TBL);
                    _memCache.RemoveCache("Notification");
                    return Json(new { sucess = true, responseMessage = "Notification Updated Successfully.", responseText = "Success", data = data });
                }
               
            }
        }
        [HttpGet]
        public IActionResult ViewNotificationMaster()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetNotificationMaster(NotificationMaster TBL)
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
                List<NotificationMaster> lst = await _NotificationRepository.ViewNotification(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> DeleteNotificationMaster(int Id)
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
                NotificationMaster ob = new NotificationMaster();
                ob.Id = Id;

                var data = await _NotificationRepository.DeleteNotification(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> EditNotificationMaster(int Id)
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

                NotificationMaster ob = new NotificationMaster();
                ob.Id = Id;
                List<NotificationMaster> lst = await _NotificationRepository.EditNotification(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }
        }
    }
}
