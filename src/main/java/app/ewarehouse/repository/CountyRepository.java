package app.ewarehouse.repository;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.County;

import jakarta.persistence.Tuple;


@Repository
public interface CountyRepository extends JpaRepository<County, Integer> {
	List<County> findByCountryCountryId(Integer countryId);
	Page<County> findAll(Pageable pageable);
	
	
	
	//Get Approved County list
	@Query(value = "SELECT  distinct(cpa.intCountyId) as countyId, county.county_name as countyName FROM ewrsdevdb.t_application_of_conformity_particular_of_applicants cpa \r\n"
			+ "inner join ewrsdevdb.t_county_master county on cpa.intCountyId=county.county_id;" , nativeQuery = true)
	List<Tuple> getApprovedCountyList();
	
}
