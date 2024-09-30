using CTMS.Model.Entities.Event;
using CTMS.Repository.Repositories.Interfaces;
using Hangfire;
using System.Xml.Linq;

namespace CTMS.Web.Helper
{
    public class JobSchedulerHelper
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IEventRepository _event;
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public JobSchedulerHelper(IRecurringJobManager recurringJobManager, IEventRepository eventStatus)
        {
            _recurringJobManager = recurringJobManager;
            _event = eventStatus;
        }
        public async Task<string> HandleEventStatus(string timestring)
        {
            try
            {
                _recurringJobManager.AddOrUpdate("event_status_check", () => UpdateEvent(), timestring);
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error("Error during Schedulling : event_status_check Error Message:" + ex.Message);
                return string.Empty;
            }
        }
        public async Task<string> UpdateEvent()
        {
            try
            {
                List<EventStatus> events = _event.GetStatusOfBackDateEvents().Result;
                var xEle = new XElement("EventDetails",
                                    from emp in events
                                    select new XElement("EventData",
                                       new XElement("EventId", emp.EventId),
                                       new XElement("Status", emp.Status)
                                    ));
                var jobid= await Task.Run(() => BackgroundJob.Enqueue(() => _event.EventStatusUpdate(xEle)));
                return $"JobId : {jobid} Completed. Status Updated for events of - " + string.Join(",", events.Select(x=>x.EventId));
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
