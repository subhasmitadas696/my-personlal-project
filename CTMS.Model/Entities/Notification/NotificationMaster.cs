
namespace CTMS.Model.Entities.Notification
{
    public class NotificationMaster
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? TitleOd { get; set; }
        public string? Description { get; set; }
        public string? DescriptionOd { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
