using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.LevelMaster;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class LevelMasterController : Controller
    {



        public readonly IHierarchyServiceProviderRepository _hirerarchyServiceProvider;
        public readonly ILevelServiceProvider _levelServiceProvider;
        CreateLevelMaster model = new CreateLevelMaster();

        public LevelMasterController(IHierarchyServiceProviderRepository hirerarchyServiceProvider, ILevelServiceProvider levelServiceProvider)
        {
            _hirerarchyServiceProvider = hirerarchyServiceProvider;
            _levelServiceProvider = levelServiceProvider;
        }
        [HttpGet]
        public async Task<IActionResult> AddLevelMaster(string Status)
        {
            model.HierarchyList = (await _hirerarchyServiceProvider.GetAllHierarchy(1)).HierarchyList.ToList();
            model.INTCREATEDBY = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> BindParentLevelByHierarchyId(int HId)
        {
            var result = (await _levelServiceProvider.GetAllLevelByHierarchyId(HId)).ParentLevelList.ToList();
            return Json(result);
        }
        #region Add  Level
        [HttpPost]
        public IActionResult AddLevelMaster(CreateLevelMaster objlevel)
        {
            try
            {
                var Result = _levelServiceProvider.AddLevel(objlevel);
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region view  Level data
        [HttpGet]
        public async Task<IActionResult> ViewActiveLevel(string Status)
        {
            try
            {
                LevelMasterModel vmodel = new LevelMasterModel();
                vmodel.LevelList = (await _levelServiceProvider.GetAllLevel(1)).LevelList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewInActiveLevel(string Status)
        {
            try
            {
                LevelMasterModel vmodel = new LevelMasterModel();
                vmodel.LevelList = (await _levelServiceProvider.GetAllLevel(0)).LevelList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Get  Level by Id
        public async Task<IActionResult> EditActiveLevel(int id)
        {
            try
            {
                model.HierarchyList = (await _hirerarchyServiceProvider.GetAllHierarchy(1)).HierarchyList.ToList();
                var result = _levelServiceProvider.GetLevelById(id).Result;
                model.INTHIERARCHYID = result.ToList()[0].INTHIERARCHYID;
                model.INTLEVELID = result.ToList()[0].INTLEVELID;
                model.INTPARENTID = result.ToList()[0].INTPARENTID;
                model.NVCHLABEL = result.ToList()[0].NVCHLABEL;
                model.VCHALLIAS = result.ToList()[0].VCHALLIAS;
                return View(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update Level
        [HttpPost]
        public IActionResult EditLevel(CreateLevelMaster objlevel)
        {
            try
            {
                var Result = _levelServiceProvider.EditLevel(objlevel);
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
                LevelMasterModel vmodel = new LevelMasterModel();
                string LevelId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    vmodel.INTLEVELID = Id;

                    Result = _levelServiceProvider.MarkInactive(vmodel);
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

                LevelMasterModel vmodel = new LevelMasterModel();
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    vmodel.INTLEVELID = Id;
                    Result = _levelServiceProvider.MarkActive(vmodel);
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
