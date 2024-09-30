using CTMS.Model.Entities.Gallery;
using CTMS.Model.Entities.Master;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IGalleryRepository
    {
        Task<int> InsertGallery(GalleryMaster TBL);

        Task<int> UpdateGallery(GalleryMaster TBL);

        Task<int> DeleteGallery(GalleryMaster TBL);

        Task<List<GalleryMaster>> ViewGallery(GalleryMaster TBL);

        Task<List<GalleryMaster>> EditGallery(GalleryMaster TBL);
        Task<List<DropdownData>> BindGallery();

        Task<List<DropdownData>> SearchGalleryById(GalleryMaster TBL);
    }
}
