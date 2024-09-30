using CTMS.Model.Entities.Gallery;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.MediaMaster;
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
    public class MediaController : Controller
    {
        private readonly IMediaRepository _MediaRepository;
        private readonly IGalleryRepository _GalleryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MediaController(IMediaRepository MediaRepository, IGalleryRepository galleryRepository, IWebHostEnvironment hostingEnvironment)
        {
            _MediaRepository = MediaRepository;
            _GalleryRepository = galleryRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult MediaMaster()
        {
            return View();
        }
        [HttpPost]
        [RequestSizeLimit(90_000_000)]
        public async Task<IActionResult> MediaMaster(MediaMaster TBL)
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
                    var SubFolder = "Media";
                    var ChildSubfolderImg = "Images";
                    var ChildSubfolderVid = "Videos";
                    string webrootpath = _hostingEnvironment.WebRootPath;
                    string MainDocPath = Path.Combine(webrootpath, foldername);
                    string subDocPath = Path.Combine(MainDocPath, SubFolder);
                    string childSubDocPathImg = Path.Combine(subDocPath, ChildSubfolderImg);
                    string childSubDocPathVid = Path.Combine(subDocPath, ChildSubfolderVid);
                    string MediaPath = Path.Combine(foldername, SubFolder);
                    string MediaPathImg = Path.Combine(MediaPath, ChildSubfolderImg);
                    string MediaPathVideo = Path.Combine(MediaPath, ChildSubfolderVid);
                    if (!Directory.Exists(MainDocPath))
                        Directory.CreateDirectory(MainDocPath);
                    if (!Directory.Exists(subDocPath))
                        Directory.CreateDirectory(subDocPath);
                    if (!Directory.Exists(childSubDocPathImg))
                        Directory.CreateDirectory(childSubDocPathImg);
                    if (!Directory.Exists(childSubDocPathVid))
                        Directory.CreateDirectory(childSubDocPathVid);
                    if (TBL.ImageFile != null)
                    {
                        var extn = "." + TBL.ImageFile.FileName.Split('.')[TBL.ImageFile.FileName.Split('.').Length - 1];
                        TBL.ImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extn;
                        TBL.Path = Path.Combine(MediaPathImg, TBL.ImageName);
                        using (var stream = new FileStream(Path.Combine(childSubDocPathImg, TBL.ImageName), FileMode.Create))
                        {
                            await TBL.ImageFile.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TBL!.ImageName = TBL.ImageName;
                    }
                    if (TBL.VideoFile != null)
                    {
                        var extn = "." + TBL.VideoFile.FileName.Split('.')[TBL.VideoFile.FileName.Split('.').Length - 1];
                        TBL.VideoName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extn;
                        TBL.Path = Path.Combine(MediaPathVideo, TBL.VideoName);
                        using (var stream = new FileStream(Path.Combine(childSubDocPathVid, TBL.VideoName), FileMode.Create))
                        {
                            await TBL.VideoFile.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TBL!.VideoName = TBL.VideoName;
                    }
                    if (TBL.Id == 0)
                    {
                        var data = _MediaRepository.InsertMedia(TBL);
                        return Json(new { sucess = true, responseMessage = "Media Saved Successfully.", responseText = "Success", data = data });

                    }
                    else
                    {
                        var data = _MediaRepository.UpdateMedia(TBL);
                        return Json(new { sucess = true, responseMessage = "Media Updated Successfully.", responseText = "Success", data = data });

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Internal Server Error", data = "" });
            }
        }
        [HttpGet]
        public IActionResult ViewMediaMaster()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetMediaMaster(MediaMaster TBL)
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
                List<MediaMaster> lst = await _MediaRepository.ViewMedia(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> DeleteMediaMaster(int Id)
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
                MediaMaster ob = new MediaMaster();
                ob.Id = Id;

                var data = await _MediaRepository.DeleteMedia(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> EditMediaMaster(int Id)
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
                MediaMaster ob = new MediaMaster();
                ob.Id = Id;
                List<MediaMaster> lst = await _MediaRepository.EditMedia(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }
        [HttpGet]
        public async Task<JsonResult> GetGalleryMaster()
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
                    List<DropdownData> lst = await _GalleryRepository.BindGallery();
                    var jsonres = JsonConvert.SerializeObject(lst);
                    return Json(jsonres);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Internal Server Error", data = "" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> GetGalleryMasterById(int Id)
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
                    GalleryMaster ob = new GalleryMaster();
                    ob.Id = Id;
                    List<DropdownData> lst = await _GalleryRepository.SearchGalleryById(ob);
                    var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                    return Json(jsonres);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, responseMessage = ex.Message, responseText = "Internal Server Error", data = "" });
            }

        }
    }
}
