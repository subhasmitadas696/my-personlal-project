package app.ewarehouse.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.OnlineServiceApprovalNotings;
@Repository
public interface OnlineServiceApprovalNotingsRepository extends JpaRepository<OnlineServiceApprovalNotings, Integer> {

		@Query(value = "SELECT  TN.intOnlineServiceId, TN.intFromAuthority, TN.intToAuthority,TN.txtNoting, TN.dtActionTaken,\r\n"
			+ "    		               TN.intCreatedBy, TN.intProcessId,MA.vchActionName,RR.vchRolename,QD.vchDocumentFile,QD.vchDocumentName ,TN.intStatus,UR.vchFullName,TN.inspectionDocId, TN.tinStageCtr \r\n"
			+ "	\r\n"
			+ "    		                FROM t_online_service_approval_notings TN  LEFT JOIN m_approval_actions AS MA ON TN.intStatus = MA.tinApprovalActionId AND MA.bitDeletedFlag = 0\r\n"
			+ "    		                LEFT JOIN m_admin_role AS RR ON TN.intFromAuthority = RR.intId AND MA.bitDeletedFlag = 0\r\n"
			+ "                            LEFT JOIN t_online_service_query_document AS QD ON TN.intOnlineServiceId= QD.intOnlineServiceId AND TN.intNotingsId = QD.intNotingId AND QD.bitDeletedFlag = 0\r\n"
			+" LEFT JOIN m_admin_user UR ON TN.tinQueryTo=UR.intId AND UR.bitDeletedFlag = 0"
			+ "    		                WHERE TN.bitDeletedFlag = 0 AND TN.intOnlineServiceId =:serviceId AND TN.intProcessId =:processId\r\n"
			+ "    		                ORDER BY TN.dtmCreatedOn DESC", nativeQuery = true)
	List<Object[]> getAllDetailsByServiceIdProcessId(Integer serviceId, Integer processId);

	@Query(value = "SELECT * FROM t_online_service_approval_notings Where intOnlineServiceId=:intOnlineServiceId AND intProcessId=:intProcessId AND bitDeletedFlag=0 \r\n"
			+ "order by intNotingsId DESC  limit 1;", nativeQuery = true)
	OnlineServiceApprovalNotings getDataBYUsingprocessIdAndServiceId(Integer intProcessId, Integer intOnlineServiceId);

	Integer countByIntOnlineServiceIdAndIntProcessIdAndIntStatusAndBitDeletedFlag(Integer intId, Integer intProcessId,
			Integer tinStatus, boolean bitDeletedFlag);

	@Query(value = "SELECT H.intOnlineServiceHistoryId,N.intFromAuthority, N.intToAuthority, N.dtActionTaken, N.txtNoting,"
			+ "    TD.vchRolename FROM t_online_service_approval_notings N LEFT JOIN m_admin_role TD ON N.intFromAuthority = TD.intId AND TD.bitDeletedFlag = 0 JOIN "
			+ "    t_online_service_application_H H ON H.intNotingId = N.intNotingsId AND H.intOnlineServiceId = N.intOnlineServiceId\r\n"
			+ "    WHERE N.intOnlineServiceId =:serviceId AND N.intProcessId =:processId AND  N.bitDeletedFlag = 0 AND H.bitDeletedFlag = 0 AND "
			+ "    H.intProcessId =:processId ORDER BY  H.dtmCreatedOn DESC;", nativeQuery = true)
	List<Object[]> getResubmitHistoryByOnlineServiceIdAndProcessId(Integer processId, Integer serviceId);
    @Query(value = " SELECT COUNT(1)\n" + "        FROM t_online_service_approval_notings\n"
			+ "        WHERE  intProcessId =:intProcessId AND intOnlineServiceId =:intOnlineServiceId", nativeQuery = true)
	Integer getCountedData(Integer intProcessId, Integer intOnlineServiceId);

    
    
    String obRespquery=""" 
    		SELECT obRes.vchObResQuId, obRes.vchObRes,obRes.intObResAPId  FROM complaint_observation_response obRes 
          where obRes.intObResRole=:roleId AND  obRes.intObResAPId=:intOnlineServiceId  And intObResLableId=:lableId AND intObResStage=:intObResStage
    		""";
	@Query(value =obRespquery, nativeQuery = true)
	List<Object[]> observationResponse (Integer roleId, Integer intOnlineServiceId,Integer lableId,Integer intObResStage);
    
    
    


	OnlineServiceApprovalNotings findByIntNotingsIdAndBitDeletedFlag(Integer remarkId, boolean b);

}
