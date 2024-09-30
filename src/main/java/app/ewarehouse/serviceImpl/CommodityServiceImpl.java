package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.Commodity;
import app.ewarehouse.entity.ComplaintType;
import app.ewarehouse.repository.CommodityMasterRepository;
import app.ewarehouse.repository.CommodityTypeRepository;
import app.ewarehouse.repository.SeasonalityRepository;
import app.ewarehouse.service.CommodityService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class CommodityServiceImpl implements CommodityService {
	
	@Autowired
	private CommodityMasterRepository commoditymasterrepository;
	
	@Autowired
	private SeasonalityRepository seasonalityRepository;
	
	@Autowired
	private CommodityTypeRepository commodityTypeRepository;

	private static final Logger logger = LoggerFactory.getLogger(CommodityServiceImpl.class);

	@Transactional
	@Override
	public Integer save(String commodityRequest) {
		String decodedData = CommonUtil.inputStreamDecoder((commodityRequest));
		Commodity commodity;
		 try {
			 commodity = new ObjectMapper().readValue(decodedData, Commodity.class);
			 Commodity ComplaintTypeDetails = commoditymasterrepository.findByNameIgnoreCase(commodity.getName());
			 if (ComplaintTypeDetails == null) {
				 var season = seasonalityRepository.findById((commodity.getSeasonality().getId())).orElse(null);
				 var type = commodityTypeRepository.findById((commodity.getType().getId())).orElse(null);

				 Commodity commodity1 = Commodity.builder()
						 .name(commodity.getName())
						 .charges(commodity.getCharges())
						 .seasonality(season)
						 .type(type)
						 .bitDeleteFlag(false)
						 .build();

				 var savedCommodity = commoditymasterrepository.save(commodity1);

				 if (savedCommodity.getId() > 0) {
					 return 1;
				 } else {
					 return 0;
				 }
			 } else {
				 return 2;
			 }
		 } catch (Exception e) {
			 logger.error(String.valueOf(e));
	            return 0;
	        }
	}

	@Override
	public List<Commodity> getAll() {
		return commoditymasterrepository.findByBitDeleteFlag(false);
	}

	@Override
	public Commodity getById(Integer id) {
		java.util.Optional<Commodity> optionalCommodity = commoditymasterrepository.findById(id);
        return optionalCommodity.orElse(null);
	}

	@Transactional
	@Override
	public Integer update(Integer commodityId, String commodityRequest) {
		String decodedData = CommonUtil.inputStreamDecoder((commodityRequest));
		Commodity commodity;
	    try {
			commodity = new ObjectMapper().readValue(decodedData, Commodity.class);
			var season = seasonalityRepository.findById((commodity.getSeasonality().getId())).orElse(null);
			var type = commodityTypeRepository.findById((commodity.getType().getId())).orElse(null);

			Commodity duplicateCommodityType = commoditymasterrepository.findByNameIgnoreCase(commodity.getName());
			if (duplicateCommodityType != null && !duplicateCommodityType.getId().equals(commodityId)) {
				return 2;
			}

	        Commodity existingCommodity = commoditymasterrepository.findById(commodityId).orElse(null);
	        if (existingCommodity == null) {
				return 0;
			}
	        existingCommodity.setName(commodity.getName());
	        existingCommodity.setCharges(commodity.getCharges());
	        existingCommodity.setSeasonality(season);
	        existingCommodity.setType(type);
			commoditymasterrepository.save(existingCommodity);
			return 1;
	    } catch (Exception e) {
	        return 0;
	    }
	}

	@Override
	public boolean delete(Integer commodityId) throws Exception {
	    try {
	        Commodity existingCommodity = commoditymasterrepository.findById(commodityId).orElse(null);
	        if (existingCommodity == null) {
	            return false;
	        }
			existingCommodity.setBitDeleteFlag(!existingCommodity.getBitDeleteFlag());
			commoditymasterrepository.save(existingCommodity);
	        return true;
	    } catch (Exception e) {
			throw new Exception("Deleting commodity failed");
	    }
	}

	@Override
	public Page<Commodity> getAllCommoditiesList(Pageable pageable) {
		System.out.println("pageable commodites are"+commoditymasterrepository.findAll(pageable).getContent());
		return commoditymasterrepository.getAll(pageable);
	}
}

