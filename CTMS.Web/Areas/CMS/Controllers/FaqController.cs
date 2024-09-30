using CTMS.Core;
using CTMS.Model.Entities.ManageFAQ;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CTMS.Web.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize]
    public class FaqController : Controller
    {

        private readonly IFaqQRepository _FAQRepository;
        private readonly IMemCache _memCache;
        public FaqController(IConfiguration configuration, IFaqQRepository FAQRepository,IMemCache memCache)
        {
            _FAQRepository = FAQRepository;
            _memCache = memCache;
        }
        [HttpGet]
        public IActionResult ManageFAQ()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ManageFAQ(Managefaq TBL)
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
                    var data = _FAQRepository.InsertManageFAQ(TBL);
                    _memCache.RemoveCache("FAQ");
                    return Json(new { sucess = true, responseMessage = "FAQ Added Successfully.", responseText = "Success", data = data });

                }
                else
                {
                    var data = _FAQRepository.UpdateManageFAQ(TBL);
                    _memCache.RemoveCache("FAQ");
                    return Json(new { sucess = true, responseMessage = "FAQ Updated Successfully.", responseText = "Success", data = data });

                }
            }
        }
        [HttpGet]
        public IActionResult ViewManageFAQ()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Get_ManageFAQ(Managefaq TBL)
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
                List<Managefaq> lst = await _FAQRepository.ViewManageFAQ(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> D_ManageFAQ(int Id)
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
                Managefaq ob = new Managefaq();
                ob.Id = Id;

                var data = await _FAQRepository.DeleteManageFAQ(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> RO_ManageFAQ(int Id)
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

                Managefaq ob = new Managefaq();
                ob.Id = Id;
                List<Managefaq> lst = await _FAQRepository.EditManageFAQ(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }

    }
}
