package app.ewarehouse.repository;


import java.util.List;
import java.util.Map;
import java.util.Optional;

import app.ewarehouse.entity.ComplaintApplicationStatus;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.Complaint_managment;
import jakarta.transaction.Transactional; 

@Repository
public interface Complaint_managmentRepository extends JpaRepository<Complaint_managment, Integer> {

	@Query(value="""
			SELECT MT.vchActionOnApplication,MT.ActionCondition,MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, MT.complaint_no as complaintNo ,TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo,TSA.tinVerifiedBy,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
			WHERE MT.bitDeletedFlag = 0
			AND TSA.bitDeletedFlag = 0
			AND TSA.intProcessId =:intId
			AND TSA.tinStatus NOT IN (3, 7, 8)
			AND TSA.intPendingAt != 0
			AND TD.intId =:roleId  and (:txtFullName='0' or MT.complaint_no like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
			ORDER BY TSA.dtmStatusDate DESC""" ,nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
				WHERE MT.bitDeletedFlag = 0
				AND TSA.bitDeletedFlag = 0
				AND TSA.intProcessId =:intId
				AND TSA.tinStatus NOT IN (3, 7, 8)
				AND TSA.intPendingAt != 0
				AND TD.intId =:roleId  and (:txtFullName='0' or MT.complaint_no like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllPendingData(@Param("intId")Integer intId,@Param("roleId") Integer roleId,@Param("userId") Integer userId,@Param("txtFullName") String txtFullName,@Param("txtContactNumber") String txtContactNumber, Pageable pageRequest);

	
	@Query(value="""
			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, MT.complaint_no as complaintNo , TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus = 8
			AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
				WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus = 8
				AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllApprovedData(@Param("intId")Integer intId, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, Pageable pageRequest);

	
	@Query(value="""
			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, MT.complaint_no as complaintNo , TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName ,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus = 7
			AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
				WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus = 7
				AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllRejectedData(@Param("intId")Integer intId, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, Pageable pageRequest);
	
	
	@Query(value="""
			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND MT.applicationStatus = :applicationStatus
			AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
			ORDER BY TSA.dtmStatusDate DESC""", nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
				WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND MT.applicationStatus = :applicationStatus
				AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllApprovedDataNew(Integer intId, String applicationStatus, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, Pageable pageRequest);

	
	@Query(value="""
			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName ,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND MT.applicationStatus = :applicationStatus
			AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
			ORDER BY TSA.dtmStatusDate DESC""", nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
				WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND MT.applicationStatus = :applicationStatus
				AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllRejectedDataNew(Integer intId, String applicationStatus, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, Pageable pageRequest);
	
	
	@Query(value="""
			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, MT.complaint_no as complaintNo , TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName ,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus >=0
			AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
			countQuery = """
					SELECT count(1)
					FROM complaint_managment AS MT
					JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
					LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
					WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus >=0
					AND (:complaintNo='0' or MT.complaint_no like CONCAT('%',:complaintNo,'%'))  and (:contactNo='0' or MT.contact_no like CONCAT('%',:contactNo,'%'))
					ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllSummaryData(@Param("intId")Integer intId, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, Pageable pageRequest);
	
//	@Query(value="""
//			SELECT MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, MT.complaint_no as complaintNo , TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo ,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName ,TSA.intLabelId
//			FROM complaint_managment AS MT
//			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
//			LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
//			WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus >=0
//			AND MT.complaint_no like CONCAT('%',:complaintNo,'%') AND MT.contact_no like CONCAT('%',:contactNo,'%')
//			AND TSA.intPendingAt = :pendingAt
//			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
//			countQuery = """
//					SELECT count(1)
//					FROM complaint_managment AS MT
//					JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
//					LEFT JOIN m_admin_role AS TD ON TSA.intPendingAt = TD.intId
//					WHERE MT.bitDeletedFlag = 0 AND TSA.bitDeletedFlag = 0 AND TSA.intProcessId =:intId AND tinStatus >=0
//					AND MT.complaint_no like CONCAT('%',:complaintNo,'%') AND MT.contact_no like CONCAT('%',:contactNo,'%')
//					AND TSA.intPendingAt = :pendingAt
//					ORDER BY TSA.dtmStatusDate DESC""")
//	Page<Map<String,Object>> getAllSummaryData(@Param("intId")Integer intId, @Param("complaintNo") String complaintNo, @Param("contactNo") String contactNo, @Param("pendingAt") Integer pendingAt,Pageable pageRequest);
	
	
	Complaint_managment findByIntIdAndBitDeletedFlag(Integer intId, boolean bitDeletedFlag);

	
	//@Query("From Complaint_managment where bitDeletedFlag=:bitDeletedFlag  and (:txtFullName='0' or complaintNo like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or txtContactNumber like CONCAT('%',:txtContactNumber,'%')) ")
	@Query("From Complaint_managment where bitDeletedFlag=:bitDeletedFlag  and (:txtFullName='0' or complaintNo = :txtFullName) and (:txtContactNumber='0' or txtContactNumber = :txtContactNumber) and (:intCreatedBy=0 or intCreatedBy = :intCreatedBy)")
	List<Complaint_managment> findAllByBitDeletedFlagAndIntInsertStatus(Boolean bitDeletedFlag,Pageable pageRequest,String txtFullName,String txtContactNumber, Integer intCreatedBy);
	
	
	Integer countByBitDeletedFlag(Boolean bitDeletedFlag);
	
	Integer countByBitDeletedFlagAndIntCreatedBy(boolean b, int i);
	
	
	@Transactional
	@Query(value="UPDATE complaint_managment SET vchActionOnApplication=:actiontype , ActionCondition=:actionCondition WHERE intId=:applicationId",nativeQuery = true)
	@Modifying
	int UpdateActionOnApplication(@Param("actiontype")String actiontype,@Param("actionCondition") String actionCondition,@Param("applicationId")Integer applicationId);
	
	
	@Query(value="Select count(*) as count from t_complaint_mgmt_insp_schedule \n"
			+ "inner join complaint_managment on t_complaint_mgmt_insp_schedule.complaintID=complaint_managment.intId\n"
			+ " where inspectorId=:userId AND intRoleId=:roleId AND t_complaint_mgmt_insp_schedule.bitTakeActionStatus = 0",nativeQuery = true)
	Integer checkInspectorAssign(@Param("userId") Integer userId,@Param("roleId") Integer RoleId);
	
	
	@Query(value="""
			SELECT MT.vchActionOnApplication,MT.ActionCondition,MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn,MT.complaint_no as complaintNo, TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo,TSA.tinVerifiedBy,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			JOIN t_complaint_mgmt_insp_schedule AS insp_schd on insp_schd.complaintID=MT.intId
			LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
			WHERE MT.bitDeletedFlag = 0
			AND TSA.bitDeletedFlag = 0
			AND TSA.intProcessId =:intId
			AND TSA.tinStatus NOT IN (3, 7, 8)
			AND TSA.intPendingAt != 0
			AND TSA.intStageNo NOT IN(2)
			AND insp_schd.inspectorId=:userId
			AND TD.intId =:roleId  and (:txtFullName='0' or MT.complaint_no like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
			ORDER BY TSA.dtmStatusDate DESC""" ,nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				JOIN t_complaint_mgmt_insp_schedule AS insp_schd on insp_schd.complaintID=MT.intId
				LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
				WHERE MT.bitDeletedFlag = 0
				AND TSA.bitDeletedFlag = 0
				AND TSA.intProcessId =:intId
				AND TSA.tinStatus NOT IN (3, 7, 8)
				AND TSA.intPendingAt != 0
				AND TSA.intStageNo NOT IN(2)
				AND insp_schd.inspectorId=:userId
				AND TD.intId =:roleId  and (:txtFullName='0' or MT.complaint_no like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllInspectorPendingData(@Param("intId")Integer intId,@Param("roleId") Integer roleId,@Param("userId") Integer userId,@Param("txtFullName") String txtFullName,@Param("txtContactNumber") String txtContactNumber, Pageable pageRequest);
	
	
	@Query(value="""
			SELECT MT.vchActionOnApplication,MT.ActionCondition,MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo,TSA.tinVerifiedBy,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
			WHERE MT.bitDeletedFlag = 0
			AND TSA.bitDeletedFlag = 0
			AND TSA.intProcessId =:intId
			AND TSA.tinStatus NOT IN (3, 7, 8)
			AND TSA.intPendingAt != 0
			AND TSA.intPendingAt != 0
			AND TSA.intStageNo IN(2)
			AND TD.intId =:roleId  and (:txtFullName='0' or MT.user_name like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
				WHERE MT.bitDeletedFlag = 0
				AND TSA.bitDeletedFlag = 0
				AND TSA.intProcessId =:intId
				AND TSA.tinStatus NOT IN (3, 7, 8)
				AND TSA.intPendingAt != 0
				AND TSA.intPendingAt != 0
				AND TSA.intStageNo IN(2)
				AND TD.intId =:roleId  and (:txtFullName='0' or MT.user_name like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllAssignData(@Param("intId")Integer intId,@Param("roleId") Integer roleId,@Param("userId") Integer userId,@Param("txtFullName") String txtFullName,@Param("txtContactNumber") String txtContactNumber, Pageable pageRequest);
	
	
	@Query(value="""
			SELECT MT.vchActionOnApplication,MT.ActionCondition,MT.intId as intId, MT.dtmCreatedOn as dtmCreatedOn, TSA.dtmStatusDate, TSA.tinStatus, TSA.intPendingAt, TD.vchRolename, TSA.intStageNo,TSA.tinVerifiedBy,TSA.vchApprovalDoc,TSA.tinVerifyLetterGenStatus,(select mu.vchFullName from m_admin_user as mu Where TSA.tinStatus=4 AND mu.intId=TSA.intForwardedUserId and mu.bitDeletedFlag=0) as userName,TSA.intLabelId
			FROM complaint_managment AS MT
			JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
			LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
			WHERE MT.bitDeletedFlag = 0
			AND TSA.bitDeletedFlag = 0
			AND TSA.intProcessId =:intId
			AND TSA.tinStatus NOT IN (3, 7, 8)
			AND TSA.intPendingAt != 0
			AND TSA.intPendingAt != 0
			AND TSA.intStageNo NOT IN(2)
			AND TD.intId =:roleId  and (:txtFullName='0' or MT.user_name like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
			ORDER BY TSA.dtmStatusDate DESC""",nativeQuery=true,
			countQuery = """
				SELECT count(1)
				FROM complaint_managment AS MT
				JOIN t_online_service_approval AS TSA ON MT.intId = TSA.intOnlineServiceId
				LEFT JOIN m_admin_role AS TD ON(FIND_IN_SET(TD.intId, TSA.intPendingAt) > 0 AND TSA.tinStatus != 4) OR (TSA.intForwardedUserId=:userId AND TSA.tinStatus = 4 AND TSA.intForwardedUserId != 0)
				WHERE MT.bitDeletedFlag = 0
				AND TSA.bitDeletedFlag = 0
				AND TSA.intProcessId =:intId
				AND TSA.tinStatus NOT IN (3, 7, 8)
				AND TSA.intPendingAt != 0
				AND TSA.intPendingAt != 0
				AND TSA.intStageNo NOT IN(2)
				AND TD.intId =:roleId  and (:txtFullName='0' or MT.user_name like CONCAT('%',:txtFullName,'%'))  and (:txtContactNumber='0' or MT.contact_no like CONCAT('%',:txtContactNumber,'%'))
				ORDER BY TSA.dtmStatusDate DESC""")
	Page<Map<String,Object>> getAllReviewData(@Param("intId")Integer intId,@Param("roleId") Integer roleId,@Param("userId") Integer userId,@Param("txtFullName") String txtFullName,@Param("txtContactNumber") String txtContactNumber, Pageable pageRequest);
	
	
	@Query("Select complaintNo from Complaint_managment order by complaintNo desc limit 1")
	String getId();
	
	
	@Query("SELECT c FROM Complaint_managment c " +
			"WHERE c.bitDeletedFlag = false AND c.intInsertStatus = 1 " +
			"AND c.applicationStatus IN :applicationStatuses " +
			"AND c.rdoComplaintAgainst = 101 " +
			"AND (:search IS NULL OR " +
			"LOWER(c.selNameofCollateralManager) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR LOWER(c.selWarehouseShopName) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR LOWER(c.txtWarehouseOperator) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR LOWER(c.vchActionOnApplication) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR LOWER(c.applicationStatus) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR LOWER(c.txtFullName) LIKE LOWER(CONCAT('%', :search, '%')) " +
			"OR CAST(c.txtContactNumber AS string) LIKE CONCAT('%', :search, '%')) ")
	Page<Complaint_managment> findByFilters(String search, Pageable pageable, List<ComplaintApplicationStatus> applicationStatuses);

	Optional<Complaint_managment> findBySelNameofCollateralManagerAndBitDeletedFlag(String selNameofCollateralManager, boolean bitDeletedFlag );
	

}