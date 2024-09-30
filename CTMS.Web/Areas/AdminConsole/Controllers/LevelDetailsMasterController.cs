using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.LevelDetailMaster;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class LevelDetailsMasterController : Controller
    {
        public readonly IHierarchyServiceProviderRepository _hirerarchyServiceProvider;
        public readonly ILevelDetailsServiceProvider _levelDetailsServiceProvider;
        public readonly ILevelServiceProvider _levelServiceProvider;
        LevelDetailMaster model = new LevelDetailMaster();

        public LevelDetailsMasterController(IHierarchyServiceProviderRepository hirerarchyServiceProvider, ILevelDetailsServiceProvider levelDetailsServiceProvider, ILevelServiceProvider levelServiceProvider)
        {
            _hirerarchyServiceProvider = hirerarchyServiceProvider;
            _levelDetailsServiceProvider = levelDetailsServiceProvider;
            _levelServiceProvider = levelServiceProvider;
        }
        [HttpGet]
        public async Task<IActionResult> AddLevelDetailsMaster(string Status)
        {
            model.HierarchyList = (await _levelDetailsServiceProvider.GetHierarchy()).HierarchyList.ToList();
            model.INTCREATEDBY = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> BindParentLevelByHierarchyId(int LId, int HId)
        {
            var result = (await _levelServiceProvider.GetAllLevelByHierarchyId(HId)).ParentLevelList.ToList();

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindParentLevelDetailsByHierarchyId(int HId, int PLId)
        {
            var result1 = (await _levelDetailsServiceProvider.GetAllLevelParentDetailsByHierarchyId(PLId, HId)).ParentLevelDetailsList.ToList();

            return Json(result1);
        }
        [HttpGet]
        public async Task<JsonResult> BindLevelByParentId(int PLId)
        {
            var result = (await _levelDetailsServiceProvider.GetLevelByParentId(PLId)).ParentLevelDetailsList.ToList();

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetLevelById(int PLId)
        {
            var result = (await _levelDetailsServiceProvider.GetLevelById(PLId)).ParentLevelDetailsList.ToList();

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindParentLevelDetailsForEdit()
        {
            var result = (await _levelDetailsServiceProvider.GetAllLevelDetailsForEdit()).ParentLevelDetailsList.ToList();
            return Json(result);
        }
        #region Add  Level Details
        [HttpPost]
        public IActionResult AddLevelDetailsMaster(LevelDetailMaster objlevel)
        {
            try
            {
                if (objlevel.ParentPosition == 3)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_block;
                }
                else if (objlevel.ParentPosition == 4)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_gp;
                }
                else if (objlevel.ParentPosition == 5)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_level5;
                }
                else
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID;
                }
                var Result = _levelDetailsServiceProvider.AddLevelDetails(objlevel);
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region view  Level Details data
        [HttpGet]
        public async Task<IActionResult> ViewActiveLevelDetails(string Status)
        {
            try
            {
                LevelDetailsMasterModel vmodel = new LevelDetailsMasterModel();
                vmodel.LevelDetailsList = (await _levelDetailsServiceProvider.GetAllLevelDetails(0)).LevelDetailsList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewInActiveLevelDetails(string Status)
        {
            try
            {
                LevelDetailsMasterModel vmodel = new LevelDetailsMasterModel();
                vmodel.LevelDetailsList = (await _levelDetailsServiceProvider.GetAllLevelDetails(1)).LevelDetailsList.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Get  Level Details by Id
        public async Task<IActionResult> EditActiveLevelDetails(int id)
        {
            try
            {
                model.HierarchyList = (await _levelDetailsServiceProvider.GetHierarchy()).HierarchyList.ToList();
                var result = _levelDetailsServiceProvider.GetLevelDetailsById(id).Result;
                model.INTHIERARCHYID = result.ToList()[0].INTHIERARCHYID;
                model.INTLEVELID = result.ToList()[0].INTLEVELID;
                model.INTPARENTID = result.ToList()[0].INTPARENTID;
                model.NVCHLEVELNAME = result.ToList()[0].NVCHLEVELNAME;
                model.VCHALIAS = result.ToList()[0].VCHALIAS;
                model.Dept = (result.ToList()[0].Dept == null) ? "0": result.ToList()[0].Dept;
                model.LineDept = (result.ToList()[0].LineDept == null) ? "0": result.ToList()[0].LineDept;
                model.Office = (result.ToList()[0].Office == null) ? "0": result.ToList()[0].Office;

                model.VCHFAXNO = result.ToList()[0].VCHFAXNO;
                model.VCHTELNO = result.ToList()[0].VCHTELNO;
                model.INTLEVELDETAILID = result.ToList()[0].INTLEVELDETAILID;
                model.ParentPosition = result.ToList()[0].ParentPosition;
                model.INTPARENTID_block = result.ToList()[0].INTPARENTID_block;
                model.INTPARENTID_gp = result.ToList()[0].INTPARENTID_gp;
                model.INTPARENTID_level5 = result.ToList()[0].INTPARENTID_level5;
                return View(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update Level Details
        [HttpPost]
        public IActionResult EditLevelDetails(LevelDetailMaster objlevel)
        {
            try
            {
                if (objlevel.ParentPosition == 3)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_block;
                }
                else if (objlevel.ParentPosition == 4)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_gp;
                }
                else if (objlevel.ParentPosition == 5)
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID_level5;
                }
                else
                {
                    objlevel.INTPARENTID = objlevel.INTPARENTID;
                }
                var Result = _levelDetailsServiceProvider.EditLevelDetails(objlevel);
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
                LevelDetailsMasterModel vmodel = new LevelDetailsMasterModel();
                string LevelId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    vmodel.INTLEVELDETAILID = Id;
                    Result = _levelDetailsServiceProvider.MarkInactive(vmodel);
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

                LevelDetailsMasterModel vmodel = new LevelDetailsMasterModel();
                string LevelId = string.Join(",", chkbox);
                var Result = String.Empty;

                foreach (string Id in chkbox)
                {
                    vmodel.INTLEVELDETAILID = Id;
                    Result = _levelDetailsServiceProvider.MarkActive(vmodel);
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
