package app.ewarehouse.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.ApplicationOfConformityCommodityType;

@Repository
public interface ApplicationOfConformityCommodityTypeRepository extends JpaRepository<ApplicationOfConformityCommodityType, Integer> {

}
