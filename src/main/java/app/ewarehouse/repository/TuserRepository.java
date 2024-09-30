package app.ewarehouse.repository;

import java.util.List;
import java.util.Map;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.Tuser;
import jakarta.persistence.Tuple;
@Repository
public interface TuserRepository extends JpaRepository<Tuser, Integer> {
Tuser findByIntIdAndBitDeletedFlag(Integer intId, boolean bitDeletedFlag);

@Query("From Tuser where  bitDeletedFlag=:bitDeletedFlag AND selRole!=0 ")
List<Tuser> findAllByBitDeletedFlag(Boolean bitDeletedFlag);

@Query("From Tuser where selEmployeeType=:id and bitDeletedFlag=:bitDeletedFlag")
List<Tuser> findByUserDataBySelEmpId(@Param("id") Integer id,Boolean bitDeletedFlag);
Tuser findByTxtUserIdAndTxtPasswordAndBitDeletedFlag(String txtUserId, String txtPassword, boolean deletedFlag);
@Query("From Tuser where txtUserId=:txtUserId and bitDeletedFlag=:bitDeletedFlag ")
Tuser getByUserId(String txtUserId,boolean bitDeletedFlag);
@Query("From Tuser where selDepartment=:id and bitDeletedFlag=:bitDeletedFlag")
List<Tuser> getBydeptIdandbitDeletFlag(@Param("id")Integer id, boolean bitDeletedFlag);
@Query("From Tuser where selDesignation=:id and bitDeletedFlag=:bitDeletedFlag")
List<Tuser> getByDesignationIdandBitDeletedFlag(@Param("id")Integer id, boolean bitDeletedFlag);
@Query("From Tuser where selRole=:id and bitDeletedFlag=:bitDeletedFlag")
List<Tuser> getByRoleIdandBitDeletedFlag(@Param("id")Integer id, boolean bitDeletedFlag);
@Query("From Tuser where selGroup=:id and bitDeletedFlag=:bitDeletedFlag")
List<Tuser> getByGroupsIdAndBitDeletedFlag(@Param("id")Integer id, boolean bitDeletedFlag);

@Query("Select txtFullName from Tuser Where intId=:intUserId and bitDeletedFlag=false")
String getUserNameById(@Param("intUserId")Integer intUserId);

@Query("Select chkPrevilege From Tuser where intId=:userId AND bitDeletedFlag=:bitDeletedFlag")
Integer getcheckPrevilegeByUserId(@Param("userId")Integer userId, boolean bitDeletedFlag );
@Query("From Tuser where bitDeletedFlag=:bitDeletedFlag and (:selDepartment=0 or selDepartment=:selDepartment)and (:selRole=0 or selRole=:selRole) and (:selDesignation=0 or selDesignation=:selDesignation) ")
List<Tuser> findBySelDepartmentAndSelRoleAndSelDesignationAndBitDeletedFlag(Integer selDepartment, Integer selRole,
		Integer selDesignation, boolean bitDeletedFlag);

Tuser getByTxtUserIdAndTxtEmailIdAndBitDeletedFlag(String loginId, String emailId, Boolean bitDeletedFlag);
Tuser getByTxtUserIdAndBitDeletedFlag(String loginId,Boolean bitDeletedFlag);
@Query(value=" select mu.intId,mu.vchFullName,mr.vchRoleName from m_admin_user as mu \r\n"
		+ "  left join m_admin_role as mr on mu.intRoleId=mr.intId and mr.bitDeletedFlag=0\r\n"
		+ "  where mu.intRoleId!=:intRoleId and mu.intRoleId!=0 and mu.bitDeletedFlag=0 order by mu.vchFullName asc",nativeQuery = true)
List<Map<String ,Object>> findUserListByRoleId(@Param("intRoleId")Integer intRoleId);

@Query("Select selRole from Tuser Where intId=:userId and bitDeletedFlag=false ")
Integer findByRoleIdByUserId(@Param("userId") Integer userId);

	Tuser getByTxtMobileNoOrTxtEmailId(String mobile, String emailId);

	@Query(value = "SELECT intId , vchFullName FROM ewrsdevdb.m_admin_user where bitDeletedFlag = 0 and intRoleId = (select intId from m_admin_role where vchRoleName = 'Inspector') AND (vchUserStatus IS NULL OR vchUserStatus NOT LIKE 'Suspend')" , nativeQuery = true)
	List<Tuple> getInspectors();
	
	@Query(value = "SELECT intId , vchFullName FROM ewrsdevdb.m_admin_user where bitDeletedFlag = 0 and intRoleId = (select intId from m_admin_role where vchRoleName = 'CECEMInspector') AND (vchUserStatus IS NULL OR vchUserStatus NOT LIKE 'Suspend')" , nativeQuery = true)
	List<Tuple> getCECMInspectors();

	@Query(value = "SELECT intId , vchFullName FROM ewrsdevdb.m_admin_user where bitDeletedFlag = 0 and intRoleId = (select intId from m_admin_role where vchRoleName = 'Collateral Manager') AND (vchUserStatus IS NULL OR vchUserStatus NOT LIKE 'Revoke License')" , nativeQuery = true)
	List<Tuple> getCollateral();
	
	@Query(value = "SELECT intId, vchFullName \r\n"
			+ "FROM ewrsdevdb.m_admin_user \r\n"
			+ "WHERE bitDeletedFlag = 0 \r\n"
			+ "  AND intRoleId = (SELECT intId FROM m_admin_role WHERE vchRoleName = 'Grader')\r\n"
			+ "  AND (vchUserStatus IS NULL OR vchUserStatus NOT LIKE 'Suspend')" , nativeQuery = true)
	List<Tuple> getGrader();
	
	@Query(value="SELECT vchFullName FROM ewrsdevdb.m_admin_user where intId =(SELECT intUserId FROM ewrsdevdb.m_admin_rolepermission where vchLinkId='161' and \r\n"
			+ "intUserId in(SELECT distinct intId FROM ewrsdevdb.m_admin_user where intRoleId=:roleId))",nativeQuery = true)
	String getPendignAtUser(@Param("roleId") Integer roleId);
}
