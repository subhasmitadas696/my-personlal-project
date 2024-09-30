using CTMS.Model.DistMaster;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.ReportMaster;
using CTMS.Model.Tusers;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;

using System.Data;

namespace CTMS.Repository.Repository
{
    public class ReportRepository : CTMSRepositoryBase, IReportRepository
    {
        public ReportRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<List<BlockWiseRegistration>> BlockWiseTroupeRegReport(BlockWiseRegistration Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "BLK");
            objParam.Add("@P_DistId", Bwr.DistId);
            objParam.Add("@P_BlockId", Bwr.BlockId);
            objParam.Add("@P_GPId", Bwr.GPId);
            var results = await Connection.QueryAsync<BlockWiseRegistration>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        } 
        public async Task<List<ReportView>> ArtFormWiseTroupe(ReportView Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "AFTRD");
            objParam.Add("@P_CatId", Bwr.CatId);
            objParam.Add("@P_SubCatId", Bwr.SubCatId);
            objParam.Add("@P_DistId", Bwr.DISTCODE);
            objParam.Add("@P_BlockId", Bwr.BlockId);
            var results = await Connection.QueryAsync<ReportView>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ReportView>> DashboardRegistration(ReportView Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DTRD");
            objParam.Add("@P_CatId", Bwr.CatId);
            objParam.Add("@P_SubCatId", Bwr.SubCatId);
            objParam.Add("@P_DistId", Bwr.DISTCODE);
            if (Bwr.UserType == "BSSO")
            {
                objParam.Add("@P_BlockId", Bwr.DISTCODE);
            }
            else{
                objParam.Add("@P_BlockId", Bwr.BlockId);
            }
            objParam.Add("@P_UserType", Bwr.UserType);
            var results = await Connection.QueryAsync<ReportView>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }

        public async Task<List<ReportView>> DashboardPaymentReport(ReportView Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DPR");
            objParam.Add("@P_DistId", Bwr.DISTCODE);
            objParam.Add("@P_UserType", Bwr.Role);
            var results = await Connection.QueryAsync<ReportView>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ReportView>> DashboardPaymentReportSearch(ReportView Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DPRS");
            objParam.Add("@P_DistId", Bwr.DISTCODE);
            objParam.Add("@P_UserType", Bwr.Role);
            var results = await Connection.QueryAsync<ReportView>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ReportView>> DashboardPaymentReportDetails(ReportView Bwr)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DPRD");
            objParam.Add("@P_CatId", Bwr.CatId);
            objParam.Add("@P_SubCatId", Bwr.SubCatId);
            objParam.Add("@P_DistId", Bwr.DISTCODE);
            objParam.Add("@P_BlockId", Bwr.BlockId);
            var results = await Connection.QueryAsync<ReportView>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }

        public async Task<List<DistWise_TroupeReport>> Distwise_TroupeReg(DistWise_TroupeReport DT)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DR");
            objParam.Add("@P_DistId", DT.Distid);
            objParam.Add("@P_BlockId", DT.Blockid);
            objParam.Add("@P_GPId", DT.Gpid);
            objParam.Add("@P_UserType", DT.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<PaymentReport>> Distwise_PaymentReport(PaymentReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "PR");
            objParam.Add("@P_DistId", TRV.Distid);
            objParam.Add("@P_BlockId", TRV.Blockid);
            objParam.Add("@P_UserType", TRV.UserType);
            var results = await Connection.QueryAsync<PaymentReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventReport(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ER");
            objParam.Add("@P_DistId", TRV.DistId);
            objParam.Add("@P_URTYPE", TRV.URTYPE);
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_GPId", TRV.GPId);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventReportPopUpDetails(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERNE");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventUpcomingDetails(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERUE");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventCompleteDetails(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERCE");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventRescheduleDetails(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERRE");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<EventReport>> EventCancelDetails(EventReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERC");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<EventReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ArtWiseReport>> TotalRegistrationpopup(ArtWiseReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ETR");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<ArtWiseReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ArtWiseReport>> Totalmemberpopup(ArtWiseReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ETM");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<ArtWiseReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ArtWiseReport>> TotalmemberPendingpopup(ArtWiseReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ETP");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<ArtWiseReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ArtWiseReport>> TotalmemberApprovedpopup(ArtWiseReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ETA");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<ArtWiseReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ArtWiseReport>> TotalRejectededpopup(ArtWiseReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ETRj");
            objParam.Add("@P_DistId_BlockId", TRV.DistId);
            objParam.Add("@BlkId", TRV.isdist);
            var results = await Connection.QueryAsync<ArtWiseReport>("USP_REPORT_VIEW", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<DistWise_TroupeReport>> DistTotalRegistrationpopup(DistWise_TroupeReport DTRR)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DTRR");
            objParam.Add("@P_CatId_SubCatId", DTRR.Categoryid);
            objParam.Add("@P_BlockId", DTRR.Blockid);
            objParam.Add("@SubCatId", DTRR.isCat);

            objParam.Add("@P_UserType", DTRR.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<DistWise_TroupeReport>> DistWiseTotalmemberpopup(DistWise_TroupeReport DTRR)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DTTM");
            objParam.Add("@P_CatId_SubCatId", DTRR.Categoryid);
            objParam.Add("@P_BlockId", DTRR.Blockid);
            objParam.Add("@SubCatId", DTRR.isCat);
            objParam.Add("@P_UserType", DTRR.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<DistWise_TroupeReport>> DistWisePendingmemberpopup(DistWise_TroupeReport DTRR)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWPM");
            objParam.Add("@P_CatId_SubCatId", DTRR.Categoryid);
            objParam.Add("@P_BlockId", DTRR.Blockid);
            objParam.Add("@SubCatId", DTRR.isCat);
            objParam.Add("@P_UserType", DTRR.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<DistWise_TroupeReport>> DistWiseApprovememberpopup(DistWise_TroupeReport DTRR)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWAM");
            objParam.Add("@P_CatId_SubCatId", DTRR.Categoryid);
            objParam.Add("@P_BlockId", DTRR.Blockid);
            objParam.Add("@SubCatId", DTRR.isCat);
            objParam.Add("@P_UserType", DTRR.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<DistWise_TroupeReport>> DistWiseRejectededmemberpopup(DistWise_TroupeReport DTRR)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWRM");
            objParam.Add("@P_CatId_SubCatId", DTRR.Categoryid);
            objParam.Add("@P_BlockId", DTRR.Blockid);
            objParam.Add("@SubCatId", DTRR.isCat);
            objParam.Add("@P_UserType", DTRR.UserType);
            var results = await Connection.QueryAsync<DistWise_TroupeReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<PaymentReport>> DistWisePaymentTotalReportedpopup(PaymentReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWP");
            objParam.Add("@P_DistId_BlockId", TRV.Distid);
            objParam.Add("@BlockId", TRV.isdist);
            var results = await Connection.QueryAsync<PaymentReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<PaymentReport>> DistWisePaymentPandingpopup(PaymentReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWPP");
            objParam.Add("@P_DistId_BlockId", TRV.Distid);
            objParam.Add("@BlockId", TRV.isdist);
            var results = await Connection.QueryAsync<PaymentReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<PaymentReport>> DistWisePaymentSuccesspopup(PaymentReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWSP");
            objParam.Add("@P_DistId_BlockId", TRV.Distid);
            objParam.Add("@BlockId", TRV.isdist);
            var results = await Connection.QueryAsync<PaymentReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<PaymentReport>> DistWisePaymentFailurepopup(PaymentReport TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DWFP");
            objParam.Add("@P_DistId_BlockId", TRV.Distid);
            objParam.Add("@BlockId", TRV.isdist);
            var results = await Connection.QueryAsync<PaymentReport>("USP_DISTWISE_TROUPE_REGISTRATION_REPORT", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }

    }
}
