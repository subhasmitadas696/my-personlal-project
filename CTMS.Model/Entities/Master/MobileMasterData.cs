using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.Master
{
    public class Mobilemaster
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<CategoryMaster>? categorylist { get; set; }
        public List<SubCategoryMaster>? subcategorylist { get; set; }
        public List<GramPanchayatMaster>? GPlist { get; set; }
        public List<VlgMaster>? Villagelist { get; set; }
        public List<DeptMaster>? Departmentlist { get; set; }
        public List<SchemeMaster>? Schemelist { get; set; }
        public List<TroupeMaster>? Troupelist { get; set; }
    }
    public class CategoryMaster
    {
        public int categoryid { get; set; }
        public string? categoryname { get; set; }
        public string? categoryname_odia { get; set; }
    }
    public class SubCategoryMaster
    {
        public int categoryid { get; set; }
        public int subcategoryid { get; set; }
        public string? subcategoryname { get; set; }
        public string? subcategoryname_odia { get; set; }
    }
    public class TroupeMaster
    {
        public int categoryid { get; set; }
        public int subcategoryid { get; set; }
        public int troupeid { get; set; }
        public string? troupename { get; set; }
    }
    public class GramPanchayatMaster
    {
        public string? Gpname { get; set; }
        public string? Gpname_odia { get; set; }
        public int Gpid { get; set; }
    }
    public class VlgMaster
    {
        public int Gpid { get; set; }
        public int villageid { get; set; }
        public string? villagename { get; set; }
    }
    public class DeptMaster
    {
        public int Departmentid { get; set; }
        public string? Departmentname { get; set; }
    }
    public class SchemeMaster
    {
        public int Departmentid { get; set; }
        public int Schemeid { get; set; }
        public string? Schemename { get; set; }
    }
}
