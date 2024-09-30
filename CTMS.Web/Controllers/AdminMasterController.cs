using CTMS.Core;
using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.Gallery;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using static CTMS.Core.Net;

namespace CTMS.Web.Controllers
{
    [Authorize]
    public class AdminMasterController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAdminMasterRepository _adminMasterRepository;

        public AdminMasterController(IAdminMasterRepository adminMasterRepository, IWebHostEnvironment hostingEnvironment)
        {
            _adminMasterRepository = adminMasterRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> AddCategory()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var useridClaim = identity.FindFirst("Userid");
            ViewBag.UserId = useridClaim.Value;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            try
            {
                int retMsg = _adminMasterRepository.InsertCategory(category).Result;
                if (retMsg == 1)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Category Saved Successfully.", data = 200 });
                }
                else if (retMsg == 2)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Category Upadated Successfully.", data = 200 });
                }
                else if (retMsg == 3)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Category Deleted Successfully.", data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Category Already Exist.", data = 101 });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        public async Task<IActionResult> ViewCategory()
        {
            return View();
        }
        public async Task<IActionResult> ViewCategoryDt()
        {
            try
            {
                var data = _adminMasterRepository.ViewCategory().Result;
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpPost]
        public IActionResult DeleteCategory(int CategoryId)
        {
            try
            {
                int response = _adminMasterRepository.DeleteCategory(CategoryId).Result;
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpGet]
        public IActionResult Getbyid(int CategoryId)
        {
            try
            {
                var client = _adminMasterRepository.CategoryGetById(Convert.ToInt32(CategoryId)).Result;
                return Ok(JsonConvert.SerializeObject(client));
            }
            catch(Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        //----------------------------------------------------------------------------
        public async Task<IActionResult> AddSubCategory()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var useridClaim = identity.FindFirst("Userid");
            ViewBag.UserId = useridClaim.Value;

            ViewBag.CatList = _adminMasterRepository.CategoryBind().Result;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(Subcategory subCategory)
        {
            try
            {
                int retMsg = _adminMasterRepository.InsertSubcategory(subCategory).Result;
                if (retMsg == 1)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Sub Category Saved Successfully.", data = 200 });
                }
                else if (retMsg == 2)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Sub Category Upadated Successfully.", data = 200 });
                }
                else if (retMsg == 3)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Sub Category Deleted Successfully.", data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Sub Category Already Exist.", data = 101 });

                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }

        public async Task<IActionResult> ViewSubCategory()
        {
            return View();
        }

        public async Task<IActionResult> ViewSubCategoryDt()
        {
            try
            {
                var data = _adminMasterRepository.ViewSubcategory().Result;
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }

        [HttpPost]
        public IActionResult DeleteSubCategory(int SubCategoryId)
        {
            try
            {
                int response = _adminMasterRepository.DeleteSubcategory(SubCategoryId).Result;
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }

        [HttpGet]
        public IActionResult GetSubCategoryById(int subCategoryId)
        {
            try
            {
                var subCategory = _adminMasterRepository.SubcategoryGetById(subCategoryId).Result;
                return Ok(JsonConvert.SerializeObject(subCategory));
            }
            catch(Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
            
        }
        public async Task<IActionResult> AddScheme()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var useridClaim = identity.FindFirst("Userid");
            ViewBag.UserId = useridClaim.Value;

            ViewBag.DeptList = _adminMasterRepository.DepartmentBind().Result;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddScheme(Scheme scheme)
        {
            try
            {
                string[] IdArr = scheme.DepartmentIdList.Split(',').ToArray();
                scheme.DeptIdList = CommonHelper.SerializeToXMLString(IdArr.ToList<string>());
                int retMsg = _adminMasterRepository.InsertScheme(scheme).Result;

                if (retMsg == 1)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Scheme Saved Successfully.", data = 200 });
                }
                else if (retMsg == 2)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Scheme Updated Successfully.", data = 200 });
                }
                else if (retMsg == 3)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Scheme Deleted Successfully.", data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Scheme Already Exist.", data = 101 });

                }
                
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }

        public async Task<IActionResult> ViewScheme()
        {
            return View();
        }

        public async Task<IActionResult> ViewSchemeDt()
        {
            try
            {
                var data = _adminMasterRepository.ViewScheme().Result;
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }

        [HttpPost]
        public IActionResult DeleteScheme(int SchemeId)
        {
            try
            {
                int response = _adminMasterRepository.DeleteScheme(SchemeId).Result;
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpGet]
        public IActionResult GetSchemeById(int schemeId)
        {
            try
            {
                var subCategory = _adminMasterRepository.SchemeGetById(schemeId).Result;
                return Ok(JsonConvert.SerializeObject(subCategory));
                
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
    }
}
