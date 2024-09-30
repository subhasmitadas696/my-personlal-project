using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IAdminMasterRepository
    {
        //------------------------------------------------------------CATEGORY
        Task<int> InsertCategory(Category category);
        Task<List<Category>> ViewCategory();
        Task<int> DeleteCategory(int CategoryId);
        Task<List<Category>> CategoryBind();
        Task<Category> CategoryGetById(int CategoryId);


        //------------------------------------------------------------SUBCATEGORY

        Task<int> InsertSubcategory(Subcategory subcategory);
        Task<List<Subcategory>> ViewSubcategory();
        Task<int> DeleteSubcategory(int SubCategoryId);
        Task<Subcategory> SubcategoryGetById(int SubCategoryId);


        //-------------------------------------------------------------SCHEME
        Task<int> InsertScheme(Scheme scheme);
        Task<List<Scheme>> ViewScheme();
        Task<int> DeleteScheme(int SchemeId);
        Task<List<Scheme>> DepartmentBind();
        Task<Scheme> SchemeGetById(int SchemeId);
    }
}
