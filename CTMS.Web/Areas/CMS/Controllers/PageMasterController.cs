using CTMS.Model.Entities.PageMaster;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CTMS.Web.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize]
    public class PageMasterController : Controller
    {
        private readonly IPageMasterRepository _pageMaster;
        public PageMasterController(IPageMasterRepository pageMaster)
        {
            _pageMaster = pageMaster;
        }
        [HttpGet]
        public IActionResult ManagePage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManagePageMaster(PageMaster TBL)
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
                    var data = _pageMaster.I_ManagePage(TBL);
                    return Json(new { sucess = true, responseMessage = "Inserted Successfully.", responseText = "Success", data = data });

                }
                else
                {
                    var data = _pageMaster.U_ManagePage(TBL);
                    return Json(new { sucess = true, responseMessage = "Updated Successfully.", responseText = "Success", data = data });

                }
            }
        }
        [HttpGet]
        public IActionResult ViewPage()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Get_ManagePageMaster(PageMaster TBL)
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
                List<PageMaster> lst = await _pageMaster.R_ManagePage(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> D_ManagePageMaster(int Id)
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
                PageMaster ob = new PageMaster();
                ob.Id = Id;

                var data = await _pageMaster.D_ManagePage(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> RO_ManagePageMaster(int Id)
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

                PageMaster ob = new PageMaster();
                ob.Id = Id;
                List<PageMaster> lst = await _pageMaster.RO_ManagePage(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }
    }
}
