package app.ewarehouse.repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.AocCompProfDirectorDetails;
import app.ewarehouse.entity.AocCompProfileDetails;

@Repository
public interface AocCompProfDirectorDetRepository extends JpaRepository<AocCompProfDirectorDetails, String> {
	List<AocCompProfDirectorDetails> findByProfDet(AocCompProfileDetails profDet);
	
	@Query("Select directorId from AocCompProfDirectorDetails order by directorId desc limit 1")
	String getId();
}
