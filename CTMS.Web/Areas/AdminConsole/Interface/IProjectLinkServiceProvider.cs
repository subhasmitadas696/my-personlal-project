using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;

namespace CTMS.Web.Areas.AdminConsole.Interface
{
    public interface IProjectLinkServiceProvider
    {
        string AddProjectLink(Project objProjectlink);
        Task<ViewProjectLink> GetAllActiveProjectLink();
        Task<ViewProjectLink> GetAllInActiveProjectLink();
        string InactiveProjectLink(int id, int updatedby);
        string ActiveProjectLink(int id, int updatedby);
        Task<ViewProjectLink> GetById(int id);
        string UpdateProjectLink(Project objProjectlink);
    }
}
