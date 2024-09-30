using CTMS.Core;
using CTMS.Model.Entities.Master;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web
{
    [Authorize]
    public class MasterController : Controller
    {
        private readonly IMasterRepository _masterService;
        private readonly IMemCache _memCache;
        public MasterController(IMasterRepository masterService, IMemCache memCache)
        {
            _masterService = masterService;
            _memCache = memCache;
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetDistrictByBlockid(int blockid)
        {
            try
            {
                int distid = await _masterService.GetDistrictByBlockid(blockid);
                return Json(new { state = "success", data = distid });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }
        [AllowAnonymous]
        public IActionResult GetDistrict()
        {
            try
            {
                var DistcacheKey = "DistList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(DistcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "DISTNCODE" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, DistcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [AllowAnonymous]
        public IActionResult getPaymentType()
        {
            try
            {
                var paymentcache = "PaymentType";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(paymentcache);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "PM" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, paymentcache);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [AllowAnonymous]
        public IActionResult getPaymentTypeForUpi(int ID)
        {
            try
            {
                var DistcacheKey = "PaymentTypeUPI";

                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "PMU", FilterId = ID }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, DistcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [AllowAnonymous]
        public IActionResult GetBlock(int DISTCODE = 0, int BLOCKCODE = 0, int TYPE = 0)
        {
            try
            {
                var BlockcacheKey = DISTCODE.ToString();
                List<DropdownData> objCacheBlocklist = _memCache.GetCache<List<DropdownData>>(BlockcacheKey);
                if (objCacheBlocklist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheBlocklist }.ToJson());
                }
                else
                {
                    List<DropdownData> objBlocklist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "BLOCKNCODE", FilterId = DISTCODE }).Result;

                    if (objBlocklist != null)
                    {
                        _memCache.SetCache(objBlocklist, BlockcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objBlocklist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        [AllowAnonymous]
        public IActionResult GetGP(int DISTCODE = 0, int BLOCKCODE = 0, int GPCODE = 0)
        {
            try
            {
                var GPcacheKey = BLOCKCODE.ToString();
                List<DropdownData> objCacheGPlist = _memCache.GetCache<List<DropdownData>>(GPcacheKey);
                if (objCacheGPlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheGPlist }.ToJson());
                }
                else
                {
                    List<DropdownData> objGPlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "GPNCODE", FilterId = BLOCKCODE }).Result;

                    if (objGPlist != null)
                    {
                        _memCache.SetCache(objGPlist, GPcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objGPlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }

        [AllowAnonymous]
        public IActionResult GetVillage(int DISTCODE = 0, int BLOCKCODE = 0, int GPCODE = 0, string VLGCODE = "")
        {
            try
            {
                var VlgcacheKey = GPCODE.ToString();
                List<DropdownData> objCacheVillagelist = _memCache.GetCache<List<DropdownData>>(VlgcacheKey);
                if (objCacheVillagelist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheVillagelist }.ToJson());
                }
                else
                {
                    List<DropdownData> objVillagelist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "VLGNCODE", FilterId = GPCODE }).Result;

                    if (objVillagelist != null)
                    {
                        _memCache.SetCache(objVillagelist, VlgcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objVillagelist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetCategory(int CatId = 0)
        {

            try
            {
                var CatcacheKey = "CatList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(CatcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "CATMAS" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, CatcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetSubCategory(int CatId = 0)
        {

            try
            {
                var CatcacheKey = CatId.ToString();
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(CatcacheKey);
                if (objCacheDistlist != null && objCacheDistlist.Count != 0)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "SUBCAT", FilterId = CatId }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, CatcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetBanks()
        {

            try
            {
                var CatcacheKey = "BankList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(CatcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "BANK" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, CatcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetBranch(int BankId = 0)
        {

            try
            {
                var BranchcacheKey = BankId.ToString();
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(BranchcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "BRANCH", FilterId = BankId }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, BranchcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetIFSC(int BranchId = 0)
        {

            try
            {
                var IFSCcacheKey = BranchId.ToString();
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(IFSCcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "IFSC", FilterId = BranchId }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, IFSCcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetBankBranch(string? IFSC)
        {

            try
            {
                var CatcacheKey = "TotalBankList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(CatcacheKey);
                if (objCacheDistlist != null)
                {
                    var cacheData = objCacheDistlist.Find(x => x.ExKeyVal == IFSC);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = cacheData! }.ToJson());
                }
                else
                {
                    List<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "GBB" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, CatcacheKey);
                        var data = objDistlist.Find(x => x.ExKeyVal == IFSC);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = data! }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }
        [AllowAnonymous]
        public IActionResult GetDepartment()
        {

            try
            {
                var DepartmentcacheKey = "DepartmentList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(DepartmentcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDepartmentList = _masterService.Fill_Dropdown(new SearchdownData() { Level = "DEPT" }).Result;
                    if (objDepartmentList != null)
                    {
                        _memCache.SetCache(objDepartmentList, DepartmentcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDepartmentList }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }


        [AllowAnonymous]
        public IActionResult GetSCHEME(int Deptid = 0)
        {

            try
            {
                var CatcacheKey = Deptid.ToString();
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(CatcacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "SCHEME", FilterId = Deptid }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, CatcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }


        [AllowAnonymous]
        public IActionResult GetEvent(int Schemeid = 0)
        {

            try
            {
                var EventCatcacheKey = "Event" + Schemeid.ToString();
                List<DropdownData> objCacheEventlist = _memCache.GetCache<List<DropdownData>>(EventCatcacheKey);
                if (objCacheEventlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheEventlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "EVENT", FilterId = Schemeid }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, EventCatcacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }

        }

        [AllowAnonymous]
        public IActionResult GetRole()
        {
            try
            {
                var RolecacheKey = "RoleList";
                List<DropdownData> objCacheDistlist = _memCache.GetCache<List<DropdownData>>(RolecacheKey);
                if (objCacheDistlist != null)
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
                }
                else
                {
                    IList<DropdownData> objDistlist = _masterService.Fill_Dropdown(new SearchdownData() { Level = "ROLECODE" }).Result;
                    if (objDistlist != null)
                    {
                        _memCache.SetCache(objDistlist, RolecacheKey);
                        return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                    }
                    else
                    {
                        return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetVillageNameById(int villageid)
        {
            try
            {
                var villagename = await _masterService.GetDistrictByBlockid(villageid);
                return Json(new { state = "success", data = villagename });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }
    }
}
