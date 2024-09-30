namespace CTMS.Model.Entities.Master
{
    public class DropdownData
    {
        public string? KeyVal { get; set; }
        public string? TextVal { get; set; }
        public int Type { get; set; }
        public string? OdTextVal { get; set; }

        public string? NewKeyVal { get; set; }
        public string? EncKeyVal { get; set; }
        public string? ExKeyVal { get; set; }
    }

    public class SearchdownData
    {
        public string? Level { get; set; }
        public int FilterId { get; set; }

    }
    public class GeTroupes_WRT_CatSubcatResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public List<TroupeData>? GetTroupes { get; set; }
    }
    public class TroupeData
    {
        public string? TroupeId { get; set; }
        public string? TroupeName { get; set; }
    }
}
