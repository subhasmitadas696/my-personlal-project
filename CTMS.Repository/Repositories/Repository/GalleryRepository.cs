using CTMS.Model.Entities.Gallery;
using CTMS.Model.Entities.Master;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CTMS.Repository.Repository
{
    public class GalleryRepository : CTMSRepositoryBase, IGalleryRepository
    {
        public GalleryRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertGallery(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "C");
            p.Add("@GalleryName", TBL.GalleryName);
            p.Add("@GalleryNameOD", TBL.GalleryNameOD);
            p.Add("@MediaType", TBL.MediaType);
            p.Add("@ThumbnailImg", TBL.ThumbnailImg);
            p.Add("@Path", TBL.Path);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _= await Connection.ExecuteAsync("usp_Gallery", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> UpdateGallery(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@GalleryName", TBL.GalleryName);
            p.Add("@GalleryNameOD", TBL.GalleryNameOD);
            p.Add("@MediaType", TBL.MediaType);
            p.Add("@ThumbnailImg", TBL.ThumbnailImg);
            p.Add("@Path", TBL.Path);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("usp_Gallery", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> DeleteGallery(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("usp_Gallery", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<GalleryMaster>> ViewGallery(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "V");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<GalleryMaster>("usp_Gallery", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public async Task<List<GalleryMaster>> EditGallery(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "S");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<GalleryMaster>("usp_Gallery", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<DropdownData>> BindGallery()
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "H");
            var results = await Connection.QueryAsync<DropdownData>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<DropdownData>> SearchGalleryById(GalleryMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "SH");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<DropdownData>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        
    }
}
