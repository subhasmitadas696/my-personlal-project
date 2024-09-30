using Dapper;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using System.Data;
using CTMS.Model.Entities.MediaMaster;

namespace NEWCMS.Repository.Repositories.Repository
{
    public class MediaRepository: CTMSRepositoryBase,IMediaRepository
    {
        public MediaRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertMedia(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "I");
            p.Add("@GalleryId", TBL.GalleryId);
            p.Add("@ImageName", TBL.ImageName);
            p.Add("@VideoName", TBL.VideoName);
            p.Add("@Description", TBL.Description);
            p.Add("@DescriptionOd", TBL.DescriptionOd);
            p.Add("@Path", TBL.Path);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);

        }
        public async Task<int> UpdateMedia(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "U");
            p.Add("@Id", TBL.Id);
            p.Add("@GalleryId", TBL.GalleryId);
            p.Add("@ImageName", TBL.ImageName);
            p.Add("@VideoName", TBL.VideoName);
            p.Add("@Description", TBL.Description);
            p.Add("@DescriptionOd", TBL.DescriptionOd);
            p.Add("@Path", TBL.Path);
            p.Add("@CreatedBy", 1);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> DeleteMedia(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "D");
            p.Add("@Id", TBL.Id);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<MediaMaster>> ViewMedia(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action", "R");
            p.Add("@Id", TBL.Id);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryAsync<MediaMaster>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<MediaMaster>> EditMedia(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("@Action ", "RO");
            p.Add("@Id", TBL.Id);
            var results = await Connection.QueryAsync<MediaMaster>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();

        }
        public async Task<List<MediaMaster>> MediaBind(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("Action", "BV");
            p.Add("@MediaType", TBL.MediaType);
            p.Add("@GalleryId", TBL.GalleryId);
            var results = await Connection.QueryAsync<MediaMaster>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public async Task<List<MediaMaster>> GalleryBind(MediaMaster TBL)
        {
            var p = new DynamicParameters();
            p.Add("Action", "MCGN");
            p.Add("@MediaType", TBL.MediaType);
            var results = await Connection.QueryAsync<MediaMaster>("USP_CRUD_MediaMaster", p, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
    }
}
