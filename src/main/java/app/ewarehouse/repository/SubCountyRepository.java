package app.ewarehouse.repository;


import java.util.List;
import java.util.Optional;


import app.ewarehouse.entity.County;
import app.ewarehouse.entity.SubCounty;
import jakarta.persistence.Tuple;
import jakarta.transaction.Transactional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;


@Repository
public interface SubCountyRepository extends JpaRepository<SubCounty, Integer> {

    @Modifying
    @Transactional
    @Query(value = "UPDATE t_sub_county SET bitDeletedFlag = 1 WHERE intId = :id", nativeQuery = true)
    void deleteSubCounty(@Param("id") Integer id);
	
	List<SubCounty> findByCountyAndBitDeletedFlag(County county, boolean bitDeletedFlag);

    Optional<SubCounty> findByIntIdAndBitDeletedFlag(Integer intId, boolean bitDeletedFlag);

    List<SubCounty> findAllByBitDeletedFlag(Boolean bitDeletedFlag);
    
    @Query(value = "SELECT DISTINCT(cpa.intSubCountyId) AS subCountyId, subcounty.vchSubCountyName AS subCountyName " +
            "FROM ewrsdevdb.t_application_of_conformity_particular_of_applicants cpa " +
            "INNER JOIN ewrsdevdb.t_sub_county subcounty ON cpa.intSubCountyId = subcounty.intId " +
            "AND subcounty.intCountyId = :countyId", 
            nativeQuery = true)
List<Tuple> getApprovedSubCountyList(@Param("countyId") Integer countyId);
}
