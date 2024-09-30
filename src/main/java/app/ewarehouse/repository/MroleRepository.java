package app.ewarehouse.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.Mrole;
 @Repository
public interface MroleRepository extends JpaRepository<Mrole, Integer> {
Mrole findByIntIdAndBitDeletedFlag(Integer intId, boolean bitDeletedFlag);

@Query("From Mrole where bitDeletedFlag=:bitDeletedFlag")
List<Mrole> findAllByBitDeletedFlag(Boolean bitDeletedFlag);

@Query(" Select txtRoleName From Mrole where intId=:intId AND bitDeletedFlag=:bitDeletedFlag")
String  getRoleNameByRoleID(Integer intId,Boolean bitDeletedFlag);

@Query("Select count(*) from Mrole where txtRoleName=:txtRoleName AND bitDeletedFlag=:bitDeletedFlag AND intId !=:intId ")
Integer getCountByRoleNameANDbitDeletedFlagNOTIntId(Integer intId,String txtRoleName, boolean bitDeletedFlag);
@Query("Select count(*) from Mrole where txtAliasName=:txtAliasName AND bitDeletedFlag=:bitDeletedFlag AND intId !=:intId ")
Integer getCountByAliasNameANDbitDeletedFlagNOTIntId(Integer intId,String txtAliasName, boolean bitDeletedFlag);
@Query("Select count(*) From Mrole where txtRoleName=:txtRoleName AND bitDeletedFlag=:bitDeletedFlag")
Integer countByRoleNameANDBitDeletedFlag(String txtRoleName, Boolean bitDeletedFlag);
@Query("Select count(*) From Mrole where txtAliasName=:txtAliasName AND bitDeletedFlag=:bitDeletedFlag")
Integer countByTxtAliasNameANDBitDeletedFlag(String txtAliasName, Boolean bitDeletedFlag);

@Query(value="select m_user.intId,m_user.vchFullName, m_user.vchLoginId from m_admin_user m_user inner join m_admin_role m_role on m_user.intRoleId = m_role.intId AND m_user.intRoleId = ?1",nativeQuery = true)
List<Object[]> getUserByRoleId(int roleid);


}
