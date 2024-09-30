package app.ewarehouse.serviceImpl;

import app.ewarehouse.dto.ApprovedSubCountyDto;
import app.ewarehouse.dto.SubCountyResponse;
import app.ewarehouse.entity.County;
import app.ewarehouse.entity.SubCounty;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.CountyRepository;
import app.ewarehouse.repository.SubCountyRepository;
import app.ewarehouse.service.SubCountyService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.ErrorMessages;
import app.ewarehouse.util.Mapper;
import jakarta.persistence.Tuple;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
public class SubCountyServiceImpl implements SubCountyService {

	@Autowired
    private SubCountyRepository subCountyRepository;

    @Autowired
    ErrorMessages errorMessages;
	
	@Autowired
	private CountyRepository countyRepository;

    private static final Logger logger = LoggerFactory.getLogger(SubCountyServiceImpl.class);

    @Override
    public List<SubCountyResponse> getAllSubCounties() {
        logger.info("Inside getAllSubCounties method of SubCountyServiceImpl");
        return subCountyRepository.findAllByBitDeletedFlag(false).stream().map(Mapper::mapToSubCountyResponse).collect(Collectors.toList());
    }

    @Override
    public List<SubCountyResponse> getAllSubCountiesActiveAndInactive() {
        logger.info("Inside getAllSubCountiesActiveAndInactive method of SubCountyServiceImpl");
        return subCountyRepository.findAll().stream().map(Mapper::mapToSubCountyResponse).collect(Collectors.toList());
    }

    @Override
    public Optional<SubCountyResponse> getSubCountyById(Integer id) {
        logger.info("Inside getSubCountyById method of SubCountyServiceImpl");
        return subCountyRepository.findByIntIdAndBitDeletedFlag(id, false).map(Mapper::mapToSubCountyResponse);
    }

    @Override
    public SubCountyResponse createSubCounty(String data) {
        logger.info("Inside createSubCounty method of SubCountyServiceImpl");

        try {
            String decodedData = CommonUtil.inputStreamDecoder(data);
            SubCounty subCounty = new ObjectMapper().readValue(decodedData, SubCounty.class);
            return Mapper.mapToSubCountyResponse(subCountyRepository.save(subCounty));
        }
        catch (DataIntegrityViolationException ex){
            logger.error("Inside save method of SubCountyServiceImpl some error occur:" + ex.getMessage());
            throw new CustomGeneralException(errorMessages.getSubCountyExists());
        }
        catch (Exception e) {
            logger.error("Inside save method of SubCountyServiceImpl some error occur:" + e.getMessage());
            throw new CustomGeneralException(errorMessages.getUnknownError());
        }
    }

    @Override
    public SubCountyResponse updateSubCounty(Integer id, String data) {
        logger.info("Inside updateSubCounty method of SubCountyServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(data);
        SubCounty subCounty;

        try {
            subCounty = new ObjectMapper().readValue(decodedData, SubCounty.class);
        } catch (Exception e) {
            throw new CustomGeneralException("Invalid JSON data format.");
        }

        if (subCountyRepository.existsById(id)) {
            subCounty.setIntId(id);
            return Mapper.mapToSubCountyResponse(subCountyRepository.save(subCounty));
        } else {
            throw new RuntimeException("SubCounty not found with id " + id);
        }
    }

    @Override
    public void deleteSubCounty(Integer id) {
        logger.info("Inside deleteSubCounty method of SubCountyServiceImpl");
        SubCounty subCounty = subCountyRepository.findById(id).orElseThrow(() -> new CustomGeneralException("Sub county not found."));
        subCounty.setBitDeletedFlag(!subCounty.getBitDeletedFlag());
        subCountyRepository.save(subCounty);
    }
    
    @Override
    public List<SubCountyResponse> getSubCountiesByCountyId(Integer countyId) {
        logger.info("Inside getSubCountiesByCountyId method of SubCountyServiceImpl");
    	Optional<County> countyOpt = countyRepository.findById(countyId);
    	if(countyOpt.isPresent()) {
    		County county = countyOpt.get();
    		return subCountyRepository.findByCountyAndBitDeletedFlag(county, false).stream().map(Mapper::mapToSubCountyResponse).collect(Collectors.toList());
    	}
		return Collections.emptyList();
        
    }

	@Override
	public List<ApprovedSubCountyDto> getApprovedSubCounties(Integer countyId) {
		List<Tuple> result = subCountyRepository.getApprovedSubCountyList(countyId);
		List<ApprovedSubCountyDto> subCountyList = new ArrayList<>();
		for(Tuple tuple : result) {
			ApprovedSubCountyDto dto = new ApprovedSubCountyDto();
			dto.setSubCountyId((Integer)tuple.get("subCountyId"));
			dto.setSubCountyName((String)tuple.get("subCountyName"));
			subCountyList.add(dto);
		}
		return subCountyList;
	}
    
}
