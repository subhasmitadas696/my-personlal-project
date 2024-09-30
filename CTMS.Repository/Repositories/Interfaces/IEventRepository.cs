using CTMS.Model.Entities;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<int> ManageEventDetails(EventDetails ED);
        Task<int> M_ManageEventDetails(EventDetails ED);
        List<EventDetails> ViewEventDetails(int EventId, string action,int P_DIST_BLK_ID, string fromDate, string toDate);
        List<EventDetails> GetOneEventDetails(string EventId, string action,string AssignEventId);
        Task<int> DeleteEvent(EventDetails TBL);
        Task<List<EventDetails>> GetEventCalenderDetails(EventDetails audit);
        Task<List<DropdownData>> ViewTroupes(int CatId, int SubCatId, int BlockId);
        Task<List<TroupeData>> M_ViewTroupes(int CatId, int SubCatId, int BlockId);
        Task<string> AssignTroupe(EventDetails eventDetails);
        Task<string> AssignTroupe_Mob(EventDetails_Mob eventDetails);
        Task<List<EventStatus>> GetStatusOfBackDateEvents();
        Task<string> EventStatusUpdate(XElement xml);

        Task<int> DeleteAssignTroupe(EventDetails TBL);
        Task<int> troupeAssignCount(int troupid, string dateofPerform, string starttime);
    }
}
