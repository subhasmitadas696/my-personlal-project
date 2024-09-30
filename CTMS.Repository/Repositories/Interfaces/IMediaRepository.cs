using CTMS.Model.Entities.MediaMaster;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IMediaRepository
    {
        Task<int> InsertMedia(MediaMaster TBL);

        Task<int> UpdateMedia(MediaMaster TBL);

        Task<int> DeleteMedia(MediaMaster TBL);

        Task<List<MediaMaster>> ViewMedia(MediaMaster TBL);

        Task<List<MediaMaster>> EditMedia(MediaMaster TBL);
        Task<List<MediaMaster>> MediaBind(MediaMaster TBL);
        Task<List<MediaMaster>> GalleryBind(MediaMaster TBL);
    }
}
