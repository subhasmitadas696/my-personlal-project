package app.ewarehouse.repository;

import java.util.List;
import java.util.Map;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.TOnlineServiceApproval;
@Repository
public interface TOnlineServiceApprovalRepository extends JpaRepository<TOnlineServiceApproval, Integer> {

	@Query(value = "SELECT osa FROM TOnlineServiceApproval osa WHERE osa.intProcessId = :processId " +
	        "AND LOCATE(CAST(:roleId AS String), osa.intPendingAt) > 0 " +
	        "AND osa.intOnlineServiceId = :serviceId " +
	        "AND osa.intForwardedUserId = :userId " +
	        "AND osa.bitDeletedFlag = false")
	List<TOnlineServiceApproval> getAllForwardActionData(@Param("serviceId") Integer serviceId,
	        @Param("roleId") Integer roleId, @Param("processId") Integer processId, @Param("userId") Integer userId);

	@Query(value = "From TOnlineServiceApproval Where intProcessId=:intId And intOnlineServiceId=:onlineServiceId And bitDeletedFlag = false")
	TOnlineServiceApproval getAllDataByUsingIntIdAndOnlineServiceId(@Param("intId") Integer intId,
			@Param("onlineServiceId") Integer onlineServiceId);

	@Query(value = "From TOnlineServiceApproval Where intProcessId=:intId And intOnlineServiceId=:onlineServiceId And bitDeletedFlag =false And intLabelId=:labelId")
	TOnlineServiceApproval findByIntProcessIdAndIntOnlineServiceIdAndBitDeletedFlag(@Param("intId") Integer intId,
			@Param("onlineServiceId") Integer onlineServiceId,@Param("labelId") Integer labelId);

	@Query("From TOnlineServiceApproval Where intProcessId=:intProcessId And intOnlineServiceId=:intOnlineServiceId And bitDeletedFlag =:bitDeletedFlag ")
	TOnlineServiceApproval findByIntOnlineServiceIdAndIntStageNoAndBitDeletedFlag(Integer intProcessId,Integer intOnlineServiceId, boolean bitDeletedFlag);

	TOnlineServiceApproval getByIntOnlineServiceIdAndIntProcessIdAndBitDeletedFlag(Integer id, Integer processId,
			boolean bitDeletedFlag);

	@Query(value = "SELECT  sa.intPendingAt,sa.tinStatus ,ta.vchAuthTypes ,ta.intPrimaryAuth,ta.jsnApprovalDocument ,ta.jsnVerifyDocument,ta.intSetAuthId  \n"
			+ "FROM t_online_service_approval sa INNER JOIN t_set_authority ta on ta.intProcessId = sa.intProcessId and ta.bitdeletedflag=0 WHERE ta.intProcessId =:intId \n"
			+ " and ta.tinStageNo =:stageNo  and sa.intOnlineServiceId =:serviceId and ta.intPrimaryAuth =:roleId and ta.intLabelId=:labelId and sa.bitdeletedflag=0", nativeQuery = true)

	List<Map<String, Object>> getDataByproceeIdAndroleIdAndStageNo(Integer intId, Integer stageNo, Integer roleId,
			Integer serviceId,Integer labelId);

}
