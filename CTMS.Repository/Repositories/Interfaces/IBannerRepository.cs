using CTMS.Model.Entities;
namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IBannerRepository
    {
        Task<int> I_ManageBanner(ManageBanner TBL);

        Task<int> U_ManageBanner(ManageBanner TBL);

        Task<int> D_ManageBanner(ManageBanner TBL);

        Task<List<ManageBanner>> R_ManageBanner(ManageBanner TBL);
        Task<List<ManageBanner>> RO_ManageBanner(ManageBanner TBL); 
        Task<List<ManageBanner>> ContentBanner();
        Task<ManageDashboardCount> DashBoardCount();
    }
}
