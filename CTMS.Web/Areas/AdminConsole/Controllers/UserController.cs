using CTMS.Core;
using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.User;
using CTMS.Web.Areas.AdminConsole.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class UserController : Controller
    {
        private readonly IUserServiceProvider _userService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public IHierarchyServiceProviderRepository _hirerarchyServiceProvider { get; }
        public UserController(IUserServiceProvider userService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHierarchyServiceProviderRepository hirerarchyServiceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _hirerarchyServiceProvider = hirerarchyServiceProvider;
        }
        [HttpGet]
        public async Task<IActionResult> AddUser(Designation model)
        {
            ViewBag.ParentLoc = (await _hirerarchyServiceProvider.GetAllHierarchy(1)).HierarchyList.ToList();
            ViewBag.Designation = _userService.GetDesignations();
            ViewBag.DesignationName = _userService.GetDesignationsName(model);
            return View("AddUser");
        }

        public async Task<IActionResult> BindDesg(Designation model)
        {
            var Data = _userService.GetDesignationsName(model);
            return Ok(Data);
        }

        [HttpPost]
        public JsonResult AddUserData(User user)
        {
            try
            {
                string UserPasswaord = AESEncrytDecry.DecryptStringAES(user.UPASSWORD!).ToString();
                user.UserPasswaord = SHA512Hash.SHa512(UserPasswaord);
                string Result = string.Empty;
                user.ActionCode = "A";
                user.CreatedBy =Convert.ToInt32(User.FindFirst("Userid")?.Value);
                Result = _userService.CreateUser(user).ToString();
                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        [HttpPost]
        public JsonResult EditUserData(User user)
        {
            try
            {
                user.ActionCode = "U";
                user.CreatedBy = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var result = _userService.EditUser(user);
                TempData["userResult"] = result;
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public JsonResult getLevelDetailsByParentId(int ParentId)
        {
            var result = _userService.GetLevelDetailsByParentId(ParentId);
            return Json(result);
        }
        [HttpGet]   
        public JsonResult getleveldetailsbyhirarchyid(int hirarchyid)
        {
            var result = _userService.GetLevelDetails(hirarchyid);
            return Json(result);
        }
        [HttpGet]
        public IActionResult ViewUser(string msg)
        {
            TempData["userResult"] = msg;
            ViewBag.Designation = _userService.GetDesignations();
            ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
            return View("ViewUser");
        }
        [HttpPost]
        public IActionResult ViewSearchUser(User user)
        {
            ViewBag.DesigId = user.intDesignationID;
            ViewBag.LocId = user.intLocation;
            ViewBag.Mob = user.vchmobtel;
            ViewBag.Uname = user.vchusername;
            ViewBag.Designation = _userService.GetDesignations();
            ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF", intDesignationID = user.intDesignationID, intLocation = user.intLocation, vchmobtel = user.vchmobtel, vchusername = user.vchusername });
            return View("ViewUser");
        }
        public async Task<IActionResult> EditUser(int Id)   
        {
            ViewBag.ParentLoc = (await _hirerarchyServiceProvider.GetAllHierarchy(1)).HierarchyList.ToList();
            ViewBag.PhysicalLoc = _userService.GetLocation();
            ViewBag.Designation = _userService.GetDesignations();
            var usd = _userService.GetUserById(Id, "VS");
            ViewBag.UserDetails = usd;
            return View("EditUser");
        }
        public IActionResult DeleteUser(int Id)
        {
            TempData["userResult"] = _userService.DeleteUser(new User { ActionCode = "I", intuserid = Id, CreatedBy = 1 });
            return Ok(TempData["userResult"]);
        }
    }
}
