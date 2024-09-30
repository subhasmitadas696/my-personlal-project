using CTMS.Core;
using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.HierarchyMaster;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class HirerachyMasterController : Controller
    {

        public IHierarchyServiceProviderRepository _hirerarchyServiceProvider { get; }
        Hierarchy model = new Hierarchy();

        public HirerachyMasterController(IHierarchyServiceProviderRepository hirerarchyServiceProvider)
        {
            _hirerarchyServiceProvider = hirerarchyServiceProvider;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddHirerarchy(string Status)
        {
            TempData["CommandStatus"] = Status;
            return View();
        }
        #region Add  Hierarchy
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public IActionResult AddHirerarchy(Hierarchy objhierarchy)
        {

            try
            {
                objhierarchy.INTCREATEDBY = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                var Result = _hirerarchyServiceProvider.AddHierarchy(objhierarchy);
                return Content(new AjaxResult { status = ResultType.success.ToString(), message = "success", data = Result }.ToJson());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region view  Hierarchy
        [HttpGet]
        public async Task<IActionResult> ViewActiveHirerarchy(string Status)
        {
            try
            {
                HierarchyModel vmodel = new HierarchyModel();
                vmodel.HierarchyList = (await _hirerarchyServiceProvider.GetAllHierarchy(1)).HierarchyList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpGet]
        public async Task<IActionResult> ViewInActiveHirerarchy(string Status)
        {
            try
            {
                HierarchyModel vmodel = new HierarchyModel();
                vmodel.HierarchyList = (await _hirerarchyServiceProvider.GetAllHierarchy(0)).HierarchyList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion
        #region Get  Hierarchy by Id
        public async Task<IActionResult> EditActiveHierarchy(int id)
        {
            try
            {
                HierarchyModel vmodel = new HierarchyModel();
                vmodel.HierarchyList = (await _hirerarchyServiceProvider.GetById(id)).HierarchyList.ToList();
                model.ROLEID = vmodel.HierarchyList[0].ROLEID;
                model.ROLENAME = vmodel.HierarchyList[0].ROLENAME;
                model.LEVELID = vmodel.HierarchyList[0].LEVELID;
                return View(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update Hierarchy
        [HttpPost]
        public IActionResult EditHierarchy(Hierarchy objhierarchy)
        {
            try
            {

                var Result = _hirerarchyServiceProvider.EditHierarchy(objhierarchy);
                return Ok(Result);
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
                    model.ROLEID = Convert.ToInt32(Id);
                    Result = _hirerarchyServiceProvider.MarkInactive(model);

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
                    model.ROLEID = Convert.ToInt32(Id);
                    Result = _hirerarchyServiceProvider.MarkActive(model);
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
