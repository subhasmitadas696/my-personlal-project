using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.Gallery;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Repository
{
    public class AdminMasterRepository : CTMSRepositoryBase, IAdminMasterRepository
    {
        public AdminMasterRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> InsertCategory(Category category)
        {
            var p = new DynamicParameters();
            
            if (category.CategoryId != 0)
            {
                p.Add("@ACTIONCODE", "U");
            }
            else
            {
                p.Add("@ACTIONCODE", "I");
            }
            p.Add("@P_CategoryId", category.CategoryId);
            p.Add("@P_CategoryName", category.CategoryName);
            p.Add("@P_OdiaCategoryName", category.OdiaCategoryName);
            p.Add("@P_CreatedBy", category.CreatedBy);
            
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("USP_CategoryMaster", p, commandType: CommandType.StoredProcedure);
            int returnVal = p.Get<Int32>("@MsgOut");
            return returnVal;

        }
        public async Task<List<Category>> ViewCategory()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "VIEW");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<Category>("USP_CategoryMaster", p, commandType: CommandType.StoredProcedure).ToList();
            return x;
            
        }
        public async Task<Category> CategoryGetById(int CategoryId)
        {
            var p = new DynamicParameters();
            p.Add("@P_CategoryId", CategoryId);
            p.Add("@ACTIONCODE", "BYID");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var GetAppById = Connection.Query<Category>("USP_CategoryMaster", p, commandType: CommandType.StoredProcedure).AsList();
            return GetAppById[0];
        }
        public async Task<List<Category>> CategoryBind()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "ALL");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<Category>("USP_CategoryMaster", p, commandType: CommandType.StoredProcedure).ToList();
            return x;
        }
        public async Task<int> DeleteCategory(int CategoryId)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "D");
            p.Add("@P_CategoryId", CategoryId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            Connection.Execute("USP_CategoryMaster", p, commandType: CommandType.StoredProcedure);
            int x = Convert.ToInt32(p.Get<string>("@MsgOut"));
            return x;
        }

        //----------------------------------------------------------------------------------------------------------
        public async Task<int> InsertSubcategory(Subcategory sub)
        {
            var p = new DynamicParameters();

            if (sub.SubCategoryId != 0)
            {
                p.Add("@ACTIONCODE", "U");
            }
            else
            {
                p.Add("@ACTIONCODE", "I");
            }
            p.Add("@P_CategoryId", sub.CategoryId);
            p.Add("@P_SubCategoryId", sub.SubCategoryId);

            p.Add("@P_SubCategoryName", sub.SubCategoryName);
            p.Add("@P_OdiaSubCategoryName", sub.OdiaSubCategoryName);
            p.Add("@P_CreatedBy", sub.CreatedBy);

            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("USP_SubCategoryMaster", p, commandType: CommandType.StoredProcedure);
            int returnVal = p.Get<Int32>("@MsgOut");
            return returnVal;

        }
        public async Task<List<Subcategory>> ViewSubcategory()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "VIEW");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<Subcategory>("USP_SubCategoryMaster", p, commandType: CommandType.StoredProcedure).ToList();
            return x;

        }
        public async Task<Subcategory> SubcategoryGetById(int SubCategoryId)
        {
            var p = new DynamicParameters();
            p.Add("@P_SubCategoryId", SubCategoryId);
            p.Add("@ACTIONCODE", "BYID");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var GetAppById = Connection.Query<Subcategory>("USP_SubCategoryMaster", p, commandType: CommandType.StoredProcedure).AsList();
            return GetAppById[0];
        }
        
        public async Task<int> DeleteSubcategory(int SubCategoryId)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "D");
            p.Add("@P_SubCategoryId", SubCategoryId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            Connection.Execute("USP_SubCategoryMaster", p, commandType: CommandType.StoredProcedure);
            int x = Convert.ToInt32(p.Get<string>("@MsgOut"));
            return x;
        }
        //------------------------------------------------------------------------------------------
        public async Task<int> InsertScheme(Scheme sc)
        {
            var p = new DynamicParameters();

            if (sc.SchemeId != 0)
            {
                p.Add("@ACTIONCODE", "U");
            }
            else
            {
                p.Add("@ACTIONCODE", "I");
            }
            //p.Add("@P_DepartmentId", sc.DepartmentId);
            p.Add("@xml_DeptId", sc.DeptIdList);
            p.Add("@P_SchemeId", sc.SchemeId);

            p.Add("@P_SchemeName", sc.SchemeName);
            p.Add("@P_OdiaSchemeName", sc.OdiaSchemeName);
            p.Add("@P_CreatedBy", sc.CreatedBy);

            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("USP_SCHEME_MASTER", p, commandType: CommandType.StoredProcedure);
            int returnVal = p.Get<Int32>("@MsgOut");
            return returnVal;

        }
        public async Task<List<Scheme>> ViewScheme()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "VIEW");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<Scheme>("USP_SCHEME_MASTER", p, commandType: CommandType.StoredProcedure).ToList();
            return x;

        }
        public async Task<Scheme> SchemeGetById(int SchemeId)
        {
            var p = new DynamicParameters();
            p.Add("@@P_SchemeId", SchemeId);
            p.Add("@ACTIONCODE", "BYID");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var GetAppById = Connection.Query<Scheme>("USP_SCHEME_MASTER", p, commandType: CommandType.StoredProcedure).AsList();
            return GetAppById[0];
        }

        public async Task<int> DeleteScheme(int SchemeId)
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "D");
            p.Add("@@P_SchemeId", SchemeId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            Connection.Execute("USP_SCHEME_MASTER", p, commandType: CommandType.StoredProcedure);
            int x = Convert.ToInt32(p.Get<string>("@MsgOut"));
            return x;
        }
        public async Task<List<Scheme>> DepartmentBind()
        {
            var p = new DynamicParameters();
            p.Add("@ACTIONCODE", "ALL");
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var x = Connection.Query<Scheme>("USP_SCHEME_MASTER", p, commandType: CommandType.StoredProcedure).ToList();
            return x;
        }
    }
}
