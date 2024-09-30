using Microsoft.AspNetCore.Mvc;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CTMS.Repository.Repository;
using NEWCMS.Repository.Repositories.Repository;
using CTMS.Model.Entities.NewsAndUpdate;
using CTMS.Web.Areas.AdminConsole.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace CTMS.Web.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize]
    public class NewsAndUpdateController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly INewsAndUpdateRepository _NewsAndUpdateRepository;


        public NewsAndUpdateController(INewsAndUpdateRepository NewsAndUpdateRepository, IWebHostEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;
            _NewsAndUpdateRepository = NewsAndUpdateRepository;
        }
        [HttpGet]
        public IActionResult NewsAndUpdate()
        {
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(90_000_000)]
        public async Task<IActionResult> NewsAndUpdate(NewsAndUpdate TBL)
        {
            try
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
                    string foldername = "CMS";
                    var SubFolder = "NewsAndUpdate";
                    var ChildSubfolderImg = "Images";

                    string webrootpath = _hostingEnvironment.WebRootPath;
                    string MainDocPath = Path.Combine(webrootpath, foldername);
                    string subDocPath = Path.Combine(MainDocPath, SubFolder);
                    string childSubDocPathImg = Path.Combine(subDocPath, ChildSubfolderImg);
                    string MediaPath = Path.Combine(foldername, SubFolder);
                    string MediaPathImg = Path.Combine(MediaPath, ChildSubfolderImg);
                    if (!Directory.Exists(MainDocPath))
                        Directory.CreateDirectory(MainDocPath);
                    if (!Directory.Exists(subDocPath))
                        Directory.CreateDirectory(subDocPath);
                    if (!Directory.Exists(childSubDocPathImg))
                        Directory.CreateDirectory(childSubDocPathImg);
                    if(TBL.Id ==0 || TBL.Id == null)
                    {
                        if (TBL.ImageFile != null)
                        {
                            var extn = "." + TBL.ImageFile.FileName.Split('.')[TBL.ImageFile.FileName.Split('.').Length - 1];
                            TBL.NewsPhoto = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extn;
                            TBL.NewsPhotoPath = Path.Combine(MediaPathImg, TBL.NewsPhoto);
                            using (var stream = new FileStream(Path.Combine(childSubDocPathImg, TBL.NewsPhoto), FileMode.Create))
                            {
                                await TBL.ImageFile.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            TBL!.NewsPhoto = TBL.NewsPhoto;
                        }
                    }
                    else
                    {
                        if (TBL.ImageFile != null)
                        {
                            string filePath = Path.Combine(childSubDocPathImg, TBL.NewsPhoto!);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await TBL.ImageFile.CopyToAsync(stream);
                                }

                            }
                            else
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await TBL.ImageFile.CopyToAsync(stream);
                                }
                            }
                        }
                        else
                        {
                            TBL!.NewsPhoto = TBL.NewsPhoto;
                        }
                    }
                    

                    if (TBL.Id == 0 || TBL.Id == null)
                    {
                        TBL.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                        var data = _NewsAndUpdateRepository.InsertNewsAndUpdate(TBL);
                        return Json(new { sucess = true, responseMessage = "News And Update Saved Successfully.", responseText = "Success", data = data });

                    }
                    else
                    {
                        var data = _NewsAndUpdateRepository.UpdateNewsAndUpdate(TBL);
                        return Json(new { sucess = true, responseMessage = "News And Update Updated Successfully.", responseText = "Success", data = data });

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Internal Server Error", data = "" });
            }
        }
        [HttpGet]
        public IActionResult ViewNewsAndUpdate()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetNewsAndUpdate(NewsAndUpdate TBL)
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
                List<NewsAndUpdate> lst = await _NewsAndUpdateRepository.ViewNewsAndUpdate(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> DeleteNewsAndUpdate(int Id)
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
                NewsAndUpdate ob = new NewsAndUpdate();
                ob.Id = Id;

                var data = await _NewsAndUpdateRepository.DeleteNewsAndUpdate(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
         }
        [HttpGet]
        public async Task<JsonResult> EditNewsAndUpdate(int Id)
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
                NewsAndUpdate ob = new NewsAndUpdate();
                ob.Id = Id;
                List<NewsAndUpdate> lst = await _NewsAndUpdateRepository.EditNewsAndUpdate(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }
    }
}
