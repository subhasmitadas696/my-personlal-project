using CTMS.Model.Entities.NewsAndUpdate;

namespace CTMS.Repository.Repositories.Interfaces
{
   public interface INewsAndUpdateRepository
    {

        Task<int> InsertNewsAndUpdate(NewsAndUpdate TBL);

        Task<int> UpdateNewsAndUpdate(NewsAndUpdate TBL);

        Task<int> DeleteNewsAndUpdate(NewsAndUpdate TBL);

        Task<List<NewsAndUpdate>> ViewNewsAndUpdate(NewsAndUpdate TBL);

        Task<List<NewsAndUpdate>> EditNewsAndUpdate(NewsAndUpdate TBL);
        //Task<List<NewsAndUpdate>> NewsAndUpdateBind(NewsAndUpdate TBL);
        //Task<List<NewsAndUpdate>> GalleryBind(NewsAndUpdate TBL);
    }
}
