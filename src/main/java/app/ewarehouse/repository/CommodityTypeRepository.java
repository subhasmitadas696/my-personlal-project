package app.ewarehouse.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.CommodityType;

@Repository
public interface CommodityTypeRepository extends JpaRepository<CommodityType, Integer> {

}
