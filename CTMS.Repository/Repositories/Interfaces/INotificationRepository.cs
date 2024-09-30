using CTMS.Model.Entities.Notification;

namespace CTMS.Repository.Repositories.Interfaces
{
  public interface INotificationRepository
  {
        Task<int> InsertNotification(NotificationMaster TBL);

        Task<int> UpdateNotification(NotificationMaster TBL);

        Task<int> DeleteNotification(NotificationMaster TBL);

        Task<List<NotificationMaster>> ViewNotification(NotificationMaster TBL);

        Task<List<NotificationMaster>> EditNotification(NotificationMaster TBL);

        Task<List<NotificationMaster>> ViewAnnoncement();
    }
}
