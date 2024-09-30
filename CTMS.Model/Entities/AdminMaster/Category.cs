using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CTMS.Model.Entities.AdminMaster
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? OdiaCategoryName { get; set; }
        public string? CreatedBy { get; set; }

    }
    public class Subcategory
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public string? OdiaSubCategoryName { get; set; }
        public string? CreatedBy { get; set; }
        [NotMapped]

        public string? CategoryName { get; set; }
    }
    public class Scheme
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentIdList { get; set; }
        public string? DeptIdList { get; set; }
        public int SchemeId { get; set; }
        public string? SchemeName { get; set; }
        public string? OdiaSchemeName { get; set; }
        public string? CreatedBy { get; set; }
        [NotMapped]

        public string? DepartmentName { get; set; }
    }
}
