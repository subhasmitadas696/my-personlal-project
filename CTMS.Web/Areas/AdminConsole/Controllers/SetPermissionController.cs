using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.SetPermission;
using CTMS.Web.Areas.AdminConsole.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    [Authorize]
    public class SetPermissionController : Controller
    {
        private readonly ISetPermissionServiceProvider _setpermissionservice;
        private readonly IUserServiceProvider _userService;
        private readonly IProjectLinkServiceProvider _projectLinkService;
        public IPriLinkServiceProvider _priLinkServiceProvider { get; }

        SetPermissionModel model = new SetPermissionModel();
        public IActionResult Index()
        {

            return View();
        }
        public SetPermissionController(ISetPermissionServiceProvider setPermissionProvider, IUserServiceProvider userService, IProjectLinkServiceProvider projectLinkService, IPriLinkServiceProvider priLinkServiceProvider)
        {
            _setpermissionservice = setPermissionProvider;
            _userService = userService;
            _projectLinkService = projectLinkService;
            _priLinkServiceProvider = priLinkServiceProvider;

        }

        public async Task<IActionResult> SetPermission()
        {
            try
            {
                ViewBag.Designation = _userService.GetDesignations();
                ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
               // ViewBag.ByUserDetails = _userService.GetUsers(new User { ActionCode = "UF" });
                model.ProjectList = (await _priLinkServiceProvider.GetAllProjectLink()).ViewProjectLinkList.ToList();
                return View(model);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> BindSetPermissionUser()
        {
            try
            {
                ViewBag.Designation = _userService.GetDesignations();
                ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
              //  ViewBag.ByUserDetails = _userService.GetUsers(new User { ActionCode = "UF" });
                model.ProjectList = (await _priLinkServiceProvider.GetAllProjectLink()).ViewProjectLinkList.ToList();
                return View("SetPermission", model);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> BindSetPermission()
        {
            try
            {
                ViewBag.Designation = _userService.GetDesignations();
                ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
                model.ProjectList = (await _priLinkServiceProvider.GetAllProjectLink()).ViewProjectLinkList.ToList();
                return View("SetPermission", model);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> BindSetPermission(SetPermissionModel objpermission)
        {
            ViewBag.Designation = _userService.GetDesignations();
            ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
            if (objpermission.Checked == true)
            {
                objpermission.DesignId = objpermission.UserDesignId;
            }
            model.GlobalPrimaryList = (await _setpermissionservice.GetAllPrimaryLinkByGlobalLink(objpermission)).GlobalPrimaryList.ToList();
            model.ProjectList = (await _priLinkServiceProvider.GetAllProjectLink()).ViewProjectLinkList.ToList();
            return View("SetPermission", model);
        }


        [HttpPost]
        public async Task<IActionResult> BindSetPermissionUser(SetPermissionModel objpermission)
        {
            ViewBag.Designation = _userService.GetDesignations();
            ViewBag.UserDetails = _userService.GetAllUsers(new User { ActionCode = "VF" });
           // ViewBag.ByUserDetails = _userService.GetUsers(new User { ActionCode = "UF" });
            model.GlobalPrimaryListUser = (await _setpermissionservice.GetAllPrimaryLinkByGlobalLinkUserwise(objpermission)).GlobalPrimaryList.ToList();
            model.ProjectList = (await _priLinkServiceProvider.GetAllProjectLink()).ViewProjectLinkList.ToList();
            return View("SetPermissionUser", model);
        }
        
        #region Set permission
        [HttpPost]
        public IActionResult SetPermissionData([FromBody] SetPermissionModel objpermission)
        {
            try
            {
                var Result = String.Empty;
                string[] permissionlist = objpermission.setpermissionList.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                var designations = _setpermissionservice.DeletePermissionLink_Designation(Convert.ToInt32(permissionlist[0].Split('|')[0]), Convert.ToInt32(permissionlist[0].Split('|')[1]));

                foreach (string item in permissionlist)
                {
                    SetPermissionModel s = new SetPermissionModel();
                    string[] permission = item.Split('|');
                    var projid = Convert.ToInt32(permission[1]);
                    Result = _setpermissionservice.SetPermissionLink_Designation(Convert.ToInt32(permission[0]), Convert.ToInt32(permission[2]), Convert.ToInt32(permission[3]), Convert.ToInt32(User.FindFirst("Userid")!.Value), projid);

                }
                return Json(Result);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]

        public IActionResult SetPermissionDataUser([FromBody] SetPermissionModel objpermission)
        {
            try
            {
                var Result = String.Empty;
                string[] permissionlistUser = objpermission.setpermissionListUser.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                var userid = permissionlistUser[0].Split('|')[0];

                var s = _setpermissionservice.DeletePermissionLink_User(Convert.ToInt32(permissionlistUser[0].Split('|')[0]), Convert.ToInt32(permissionlistUser[0].Split('|')[1]));
                foreach (string item in permissionlistUser)
                {
                    string[] permission = item.Split('|');
                    var uid = Convert.ToInt32(permission[0]);
                    var projid = Convert.ToInt32(permission[1]);
                    var prilink = Convert.ToInt32(permission[2]);
                    var accessid = Convert.ToInt32(permission[3]);
                    Result = _setpermissionservice.SetPermissionLink_User(uid, prilink, accessid, Convert.ToInt32(User.FindFirst("Userid")!.Value), projid);
                }
                return Json(Result);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> RemoveUserPermission(string Status)
        {
            model.UserList = (await _setpermissionservice.GetAllUser()).UserList.ToList();
            TempData["CommandStatus"] = Status;
            return View(model);
        }
        #region Remove User Permission List
        [HttpPost]
        public IActionResult MarkAsInActive(string[] chkbox)
        {
            try
            {
                var Result = String.Empty;
                foreach (string userId in chkbox)
                {
                    Result = _setpermissionservice.RemovePermissionList_User(Convert.ToInt32(userId), Convert.ToInt32(User.FindFirst("Userid")!.Value));
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
