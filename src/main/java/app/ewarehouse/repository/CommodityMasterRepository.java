package app.ewarehouse.repository;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.Commodity;

@Repository
public interface CommodityMasterRepository extends JpaRepository<Commodity, Integer> {

    Commodity findByNameIgnoreCase(String name);

    @Query("SELECT c FROM Commodity c where c.bitDeleteFlag = false")
    List<Commodity> findByBitDeleteFlag(boolean bitDeleteFlag);

    @Query("SELECT c FROM Commodity c ORDER BY c.dtmCreatedAt DESC")
    Page<Commodity> getAll(Pageable pageable);

}
