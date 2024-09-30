package app.ewarehouse.master.repository;

import app.ewarehouse.master.entity.ComplaintStatusMaster;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

/**
 * @author chinmaya.jena
 * @since 03-07-2024
 */

@Repository
public interface ComplaintStatusMasterRepository extends JpaRepository<ComplaintStatusMaster, Integer> {
	List<ComplaintStatusMaster> findByBitDeletedFlagFalse();
	
	@Modifying
    @Query("UPDATE ComplaintStatusMaster c SET c.bitDeletedFlag = NOT c.bitDeletedFlag WHERE c.id = :id")
    void changeBitDeletedFlag(@Param("id") Integer id);
}
