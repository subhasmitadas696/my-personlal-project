using CTMS.Web.Areas.AdminConsole.Models.FunctionMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IFuncServiceProvider
    {
        int ActiveFunction(FunctionMasterModel objFunctionMaster);
        int AddFunction(FunctionMasterModel objFunctionMaster);
        int EditFunction(FunctionMasterModel objFunctionMaster);
        IList<FunctionMasterModel> GetAllFunction(FunctionMasterModel objFunctionMaster);
        int DeleteFuncImage(string actionCode, int funcId);
        IList<FunctionMasterModel> GetFunctionId(string userId);
        string GetFunctionData(int intFuncId);
        IList<FunctionMasterModel> GetFunctionDetails(FunctionMasterModel objFunctionMaster);
    }
}
