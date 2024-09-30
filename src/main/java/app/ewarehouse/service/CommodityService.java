package app.ewarehouse.service;

import app.ewarehouse.entity.Commodity;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.List;

public interface CommodityService {
	
	Integer save(String commodityRequest);

    List<Commodity> getAll();

    Commodity getById(Integer id);

    Integer update(Integer id, String updatedCommodity);

    boolean delete(Integer id) throws Exception;

    Page<Commodity> getAllCommoditiesList(Pageable pageable);
}
