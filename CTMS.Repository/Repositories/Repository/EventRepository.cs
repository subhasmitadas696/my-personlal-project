using CTMS.Model.Entities;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace CTMS.Repository.Repositories.Repository
{
    public class EventRepository : CTMSRepositoryBase, IEventRepository
    {
        public EventRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<int> ManageEventDetails(EventDetails ED)
        {
            var p = new DynamicParameters();
            p.Add("@ActionCode ", ED.ActionCode);
            p.Add("@DepartmentId", ED.DepartmentId);
            p.Add("@SchemeId", ED.SchemeId);
            p.Add("@EventId", ED.EventId);
            p.Add("@EventTitle", ED.EventTitle);
            p.Add("@EventDescription", ED.EventDescription);
            p.Add("@StartDate", ED.StartDate);
            p.Add("@StartTime", ED.StartTime);
            p.Add("@EndDate", ED.EndDate);
            p.Add("@EndTime", ED.EndTime);
            p.Add("@Status", ED.Status);
            p.Add("@Distid", ED.Distid);
            p.Add("@BlockId", ED.BlockId);
            p.Add("@SanctionedAmount", ED.SanctionedAmount);
            p.Add("@CreatedBy", ED.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<int> M_ManageEventDetails(EventDetails ED)
        {
            var p = new DynamicParameters();
            p.Add("@ActionCode ", ED.ActionCode);
            p.Add("@DepartmentId", ED.DepartmentId);
            p.Add("@SchemeId", ED.SchemeId);
            p.Add("@EventId", ED.EventId);
            p.Add("@EventTitle", ED.EventTitle);
            p.Add("@EventDescription", ED.EventDescription);
            p.Add("@StartDate", ED.StartDate);
            p.Add("@StartTime", ED.StartTime);
            p.Add("@EndDate", ED.EndDate);
            p.Add("@EndTime", ED.EndTime);
            p.Add("@Status", ED.Status);
            p.Add("@Distid", ED.Distid);
            p.Add("@BlockId", ED.BlockId);
            p.Add("@SanctionedAmount", ED.SanctionedAmount);
            p.Add("@CreatedBy", ED.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }

        public List<EventDetails> ViewEventDetails(int EventId, string action, int P_DIST_BLK_ID,string fromDate,string toDate)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@EventId", EventId);
            p.Add("@P_DIST_BLK_ID", P_DIST_BLK_ID);
            p.Add("@FROMDATE", fromDate);
            p.Add("@TODATE", toDate);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;

        }
        public List<EventDetails> GetOneEventDetails(string EventId, string action, string AssignEventId)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@EventId", EventId);
            p.Add("@AssignEventId", AssignEventId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;

        }
        public async Task<int> DeleteEvent(EventDetails TBL)
        {
            var p = new DynamicParameters();
            p.Add("@ActionCode ", "D");
            p.Add("@EventId", TBL.EventId);
            p.Add("@Status", TBL.Status);
            p.Add("@CreatedBy", TBL.CreatedBy);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return Convert.ToInt32(returnVal);
        }
        public async Task<List<EventDetails>> GetEventCalenderDetails(EventDetails st) //get all data
        {

            var p = new DynamicParameters();
            p.Add("@ActionCode", "R");
            var results = await Connection.QueryAsync<EventDetails>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
            return results.AsList();

        }

        public async Task<List<DropdownData>> ViewTroupes(int CatId, int SubCatId, int BlockId)
        {
            List<DropdownData> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", "GT");
            p.Add("@CatId", CatId);
            p.Add("@SubCatId", SubCatId);
            p.Add("@BlockId", BlockId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<DropdownData>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }
        public async Task<List<TroupeData>> M_ViewTroupes(int CatId, int SubCatId, int BlockId)
        {
            List<TroupeData> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", "MGT");
            p.Add("@CatId", CatId);
            p.Add("@SubCatId", SubCatId);
            p.Add("@BlockId", BlockId);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<TroupeData>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }

        public async Task<string> AssignTroupe(EventDetails eventDetails)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "AT");
                if(eventDetails.AssignEventId!=0)
                {
                    p.Add("@P_AssignEventId", eventDetails.AssignEventId);
                    p.Add("@P_ActionCode", "UT");
                }
                p.Add("@P_DistId", eventDetails.Distid);
                p.Add("@P_BlockId", eventDetails.BlockId);
                p.Add("@P_GpId", eventDetails.GPId);
                p.Add("@P_Troupeid", eventDetails.TroupID);
                p.Add("@P_EventId", eventDetails.EventId);
                p.Add("P_CreatedBy", eventDetails.CreatedBy);
                p.Add("mytable", eventDetails.ComponentXml);
                p.Add("P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_ASSIGN_TROUPE_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Custom message for the exception", ex);
            }
        }
        private string SerializeComponentXml(List<ComponentXmlDetail> componentXml)
        {
            var xEle = new XElement("AssignDetails",
                from emp in componentXml
                select new XElement("AssignData",
                    new XElement("Village", emp.Village),
                    new XElement("ImgFolderID", emp.ImgFolderID),
                    new XElement("Area", emp.Area),
                    new XElement("DateofPerform", emp.DateofPerform),
                    new XElement("StartTime", emp.StartTime),
                    new XElement("EndTime", emp.EndTime),
                    new XElement("OfficerName", emp.OfficerName),
                    new XElement("OfficerMobNo", emp.OfficerMobNo)
                )
            );

            return xEle.ToString(); // Convert XElement to string
        }
        public async Task<string> AssignTroupe_Mob(EventDetails_Mob eventDetails)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "S");
                p.Add("@P_DistId", eventDetails.Distid);
                p.Add("@P_BlockId", eventDetails.BlockId);
                p.Add("@P_GpId", eventDetails.GPId);
                p.Add("@P_Troupeid", eventDetails.TroupID);
                p.Add("@m_type", 1);
                p.Add("@P_EventId", eventDetails.EventId);
                p.Add("@P_CreatedBy", eventDetails.CreatedBy);
                var componentXmlString = SerializeComponentXml(eventDetails.ComponentXml);
                p.Add("@mytable", componentXmlString);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_ASSIGN_TROUPE_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Custom message for the exception", ex);
            }
        }

        public async Task<int> DeleteAssignTroupe(EventDetails TBL)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ActionCode ", "DAT");
                p.Add("@EventId", TBL.EventId);
                p.Add("@Status", TBL.Status);
                p.Add("@CreatedBy", TBL.CreatedBy);
                p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
                _ = await Connection.ExecuteAsync("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<Int32>("@MsgOut");
                return Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Custom message for the exception", ex);
            }
        }

        public async Task<List<EventStatus>> GetStatusOfBackDateEvents()
        {
            var p = new DynamicParameters();
            p.Add("@ActionCode", "S");
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            var result = await Connection.QueryAsync<EventStatus>("USP_UpdateEventDetails", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            return result.ToList();
        }

        public async Task<string> EventStatusUpdate(XElement xml)
        {
            var p = new DynamicParameters();
            p.Add("@ActionCode ", "U");
            p.Add("@xmldata",xml);
            p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
            _ = await Connection.ExecuteAsync("USP_UpdateEventDetails", p, commandType: CommandType.StoredProcedure);
            var returnVal = p.Get<Int32>("@MsgOut");
            if (returnVal == 1)
            {
                return "Event status updation successfull";
            }
            else
            {

                return "Event status updation unsuccessfull";
            }
        }

        public async Task<int> troupeAssignCount(int troupid, string dateofPerform, string starttime)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ActionCode ", "GTAC");
                p.Add("@TroupeId", troupid);
                p.Add("@StartDate", dateofPerform);
                p.Add("@STARTTIME", starttime);
                p.Add("@MsgOut", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<TroupeData>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.Count();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Custom message for the exception", ex);
            }
        }
    }
}
