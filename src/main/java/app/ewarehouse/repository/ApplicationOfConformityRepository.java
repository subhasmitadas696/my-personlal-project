package app.ewarehouse.repository;

import java.util.Date;
import java.util.List;
import java.util.Optional;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.ApplicationOfConformity;
import app.ewarehouse.entity.Status;
import jakarta.persistence.Tuple;

@Repository
public interface ApplicationOfConformityRepository extends JpaRepository<ApplicationOfConformity, String> {

//	@Query("SELECT a FROM ApplicationOfConformity a WHERE a.bitDeletedFlag = false AND "
//			+ "(:fromDate IS NULL OR a.dtmCreatedOn >= :fromDate) AND "
//			+ "(:toDate IS NULL OR a.dtmCreatedOn <= :toDate) AND " + "(:status IS NULL OR a.enmStatus = :status)")
//	Page<ApplicationOfConformity> findByFilters(@Param("fromDate") Date fromDate, @Param("toDate") Date toDate,
//			@Param("status") Status status, @Param("pendingAt") Integer pendingAt, Pageable pageable);
	
	@Query("SELECT a FROM ApplicationOfConformity a WHERE a.bitDeletedFlag = false AND "
		       + "(:fromDate IS NULL OR a.dtmCreatedOn >= :fromDate) AND "
		       + "(:toDate IS NULL OR a.dtmCreatedOn <= :toDate) AND "
		       + "(:status IS NULL OR a.enmStatus = :status) AND "
		       + "(:pendingAt IS NULL OR a.pendingAt = :pendingAt)")
		Page<ApplicationOfConformity> findByFilters(@Param("fromDate") Date fromDate, 
		                                            @Param("toDate") Date toDate,
		                                            @Param("status") Status status, 
		                                            @Param("pendingAt") Integer pendingAt, 
		                                            Pageable pageable);


	@Query("SELECT DISTINCT aoc FROM ApplicationOfConformity aoc JOIN FETCH aoc.particularOfApplicantsId poa "
			+ "LEFT JOIN FETCH poa.directors dir " + "WHERE aoc.applicationId = :applicationId")
	Optional<ApplicationOfConformity> findByApplicationIdWithDirectors(@Param("applicationId") String applicationId);

	@Query(value = "SELECT COUNT(*) FROM t_application_of_conformity_main WHERE intCreatedBy = :createdBy AND enmStatus = :status", nativeQuery = true)
	Integer countByIntCreatedByAndEnmStatus(@Param("createdBy") Integer createdBy, @Param("status") String status);

	List<ApplicationOfConformity> findByIntCreatedByAndEnmStatus(Integer createdBy, Status status);

	@Query("SELECT a FROM ApplicationOfConformity a WHERE a.bitDeletedFlag = false AND "
			+ "(:fromDate IS NULL OR a.dtmCreatedOn >= :fromDate) AND "
			+ "(:toDate IS NULL OR a.dtmCreatedOn <= :toDate) AND " + "(:userId IS NULL OR a.intCreatedBy = :userId)")
	Page<ApplicationOfConformity> findByUserIdFilters(Date fromDate, Date toDate, Integer userId, Pageable pageable);

	@Query(value = "select tm.vchApplicationId as applicationId , ta.vchShop as shop from t_application_of_conformity_main tm inner join t_application_of_conformity_particular_of_applicants ta\r\n"
			+ "on (tm.intParticularOfApplicantsId = ta.intParticularOfApplicantsId) "
			+ "where tm.enmStatus = 'Accepted' and ta.intCountyId = :countyId and ta.intSubCountyId = :subCountyId", nativeQuery = true)
	List<Tuple> getApprovedApplicationIdAndShop(@Param("countyId") Integer countyId, @Param("subCountyId") Integer subCountyId);

	@Query(value = "SELECT p.vchApplicantFullName " + "FROM t_application_of_conformity_particular_of_applicants p "
			+ "INNER JOIN t_application_of_conformity_main tm "
			+ "ON p.intParticularOfApplicantsId = tm.intParticularOfApplicantsId "
			+ "INNER JOIN m_final_operator_license_master opm " + "ON tm.vchApplicationId = opm.vchConformityId "
			+ "WHERE tm.enmStatus = 'Accepted' " + "AND opm.vchConformityId = :conformityId "
			+ "ORDER BY tm.intParticularOfApplicantsId DESC " + "LIMIT 1", nativeQuery = true)
	String getOperatorFullName(@Param("conformityId") String conformityId);

	@Query("SELECT poa.typeOfCommodities FROM ApplicationOfConformity am JOIN am.particularOfApplicantsId poa WHERE am.applicationId = :id AND am.enmStatus = :status AND am.bitDeletedFlag = :bitDeletedFlag")
	String getCommodityTypes(@Param("id") String id, @Param("status") Status status,
			@Param("bitDeletedFlag") boolean bitDeletedFlag);
}
