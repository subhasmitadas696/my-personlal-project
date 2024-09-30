using CTMS.Model.Entities.ManageFAQ;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IFaqQRepository
    {
        Task<int> InsertManageFAQ(Managefaq TBL);

        Task<int> UpdateManageFAQ(Managefaq TBL);

        Task<int> DeleteManageFAQ(Managefaq TBL);

        Task<List<Managefaq>> ViewManageFAQ(Managefaq TBL);

        Task<List<Managefaq>> EditManageFAQ(Managefaq TBL);
        Task<List<Managefaq>> BindFAQ();

    }
}
