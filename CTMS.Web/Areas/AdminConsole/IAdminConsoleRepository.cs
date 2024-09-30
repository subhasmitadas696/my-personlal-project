using CTMS.Web.Areas.AdminConsole.Models.User;
using NuGet.Protocol.Core.Types;

namespace CTMS.Web.Areas.AdminConsole
{
    public interface IAdminConsoleRepository 
    {
        Task<int> GetUserId(string VCHUSERNAME);
        IList<LinkAccess> GetLinkAccessByUserId( int UserId, int ProjectId, int UID,  int desgid);
    }
}
