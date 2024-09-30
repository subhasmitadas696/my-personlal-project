using CTMS.Model.Entities.Gallery;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#pragma warning disable SCS0016
// Since the antiforgery token is globally managed, there's no need to include the "validateAntiforgeryToken" attribute for each POST request.
namespace CTMS.Web.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize]
    public class GalleryController : Controller
    {
        private readonly IGalleryRepository _GalleryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GalleryController( IGalleryRepository GalleryRepository, IWebHostEnvironment hostingEnvironment)
        {
            _GalleryRepository = GalleryRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult uspGallery()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> uspGallery(GalleryMaster TBL)
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
                var SubFolder = "GalleryThumbnail";
                string webrootpath = _hostingEnvironment.WebRootPath;
                string MainDocPath = Path.Combine(webrootpath, foldername);
                string subDocPath = Path.Combine(MainDocPath, SubFolder);
                string GalleryPath = Path.Combine(foldername, SubFolder);
                if (!Directory.Exists(MainDocPath))
                    Directory.CreateDirectory(MainDocPath);
                if (!Directory.Exists(subDocPath))
                    Directory.CreateDirectory(subDocPath);
                

                if (TBL.Id == 0 || TBL.Id == null)
                {
                    if (TBL.ThumbnailImageFile != null)
                    {
                        var extn = "." + TBL.ThumbnailImageFile.FileName.Split('.')[TBL.ThumbnailImageFile.FileName.Split('.').Length - 1];
                        TBL.ThumbnailImg = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extn;
                        TBL.Path = Path.Combine(GalleryPath, TBL.ThumbnailImg);
                        using (var stream = new FileStream(Path.Combine(subDocPath, TBL.ThumbnailImg), FileMode.Create))
                        {
                            await TBL.ThumbnailImageFile.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TBL!.ThumbnailImg = TBL.ThumbnailImg;
                    }
                }
                else
                {
                    if (TBL.ThumbnailImageFile != null)
                    {
                        string filePath = Path.Combine(subDocPath, TBL.ThumbnailImg!);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await TBL.ThumbnailImageFile.CopyToAsync(stream);
                            }

                        }
                        else
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await TBL.ThumbnailImageFile.CopyToAsync(stream);
                            }
                        }
                    }
                    else
                    {
                        TBL!.ThumbnailImg = TBL.ThumbnailImg;
                    }
                }

                if (TBL.Id == 0)
                {
                    var data = _GalleryRepository.InsertGallery(TBL);
                    return Json(new { sucess = true, responseMessage = "Gallery Saved Successfully.", responseText = "Success", data = data });

                }
                else
                {
                    var data = _GalleryRepository.UpdateGallery(TBL);
                    return Json(new { sucess = true, responseMessage = "Gallery Updated Successfully.", responseText = "Success", data = data });

                }
            }
        }
        [HttpGet]
        public IActionResult ViewuspGallery()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Get_uspGallery(GalleryMaster TBL)
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
                List<GalleryMaster> lst = await _GalleryRepository.ViewGallery(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> DeleteGallery(int Id)
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
                GalleryMaster ob = new GalleryMaster();
                ob.Id = Id;

                var data = await _GalleryRepository.DeleteGallery(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> EditGallery(int Id)
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

                GalleryMaster ob = new GalleryMaster();
                ob.Id = Id;
                List<GalleryMaster> lst = await _GalleryRepository.EditGallery(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }

    }
}
