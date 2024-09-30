using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.DesignationMaster;
using CTMS.Web.Areas.AdminConsole.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class DesignationMasterController : Controller
    {
        private readonly IUserServiceProvider _userService;
        public IDesignationServiceProvider _DesignationServiceProvider { get; }
        DesignationMasters model = new DesignationMasters();
        public DesignationMasterController(IDesignationServiceProvider DesignationServiceProvider, IUserServiceProvider userService)
        {
            _DesignationServiceProvider = DesignationServiceProvider;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult AddDesignationMaster(string Status)
        {
            ViewBag.Designation = _userService.GetDesignationsUT();
            TempData["CommandStatus"] = Status;
            return View();
        }
        #region Add  DesignationMaster
        [HttpPost]
        public IActionResult AddDesignationMaster(DesignationMasters model)
        {

            try
            {
                model.INTCREATEDBY = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var Result = _DesignationServiceProvider.AddDesignation(model);
                return Json(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region view  DesignationMaster
        [HttpGet]
        public async Task<IActionResult> ViewActiveDesignation(string Status)
        {
            try
            {
                DesignationMasterModel vmodel = new DesignationMasterModel();
                vmodel.DesignationMasterList = (await _DesignationServiceProvider.GetAllDesgination(0)).DesignationMasterList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewInActiveDesignation(string Status)
        {
            try
            {
                DesignationMasterModel vmodel = new DesignationMasterModel();
                vmodel.DesignationMasterList = (await _DesignationServiceProvider.GetAllDesgination(1)).DesignationMasterList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region Get  Designation by Id
        public async Task<IActionResult> EditActiveDesignation(int id)
        {
           
            try
            {
                ViewBag.Designation = _userService.GetDesignations();
                var USD = (await _DesignationServiceProvider.GetById(id)).DesignationMasterList.ToList();
                ViewBag.UserDetails = USD;
                return View(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update Designation
        [HttpPost]
        public IActionResult EditDesignation(DesignationMasters model)
        {
            try
            {
                model.INTCREATEDBY = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                model.INTUPDATEDBY = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                var Result = _DesignationServiceProvider.EditDesignation(model);
                return Json(Result);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Mark As Inactive
        [HttpPost]
        public IActionResult MarkAsInactive(string[] chkbox)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    model.INTCREATEDBY = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    model.INTDESIGID = Convert.ToInt32(Id);
                    Result = _DesignationServiceProvider.MarkInactive(model);
                }
                return Ok(Result);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Mark As Active
        [HttpPost]
        public IActionResult MarkAsActive(string[] chkbox)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    model.INTCREATEDBY = Convert.ToInt32(User.FindFirst("Userid")?.Value);
                    model.INTDESIGID = Convert.ToInt32(Id);
                    Result = _DesignationServiceProvider.MarkActive(model);
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
