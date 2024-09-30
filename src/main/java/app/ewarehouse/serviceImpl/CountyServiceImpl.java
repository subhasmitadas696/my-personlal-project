package app.ewarehouse.serviceImpl;

import app.ewarehouse.dto.ApprovedCountiesDto;
import app.ewarehouse.dto.CountyResponse;
import app.ewarehouse.entity.Country;
import app.ewarehouse.entity.County;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.exception.CustomEntityNotFoundException;
import app.ewarehouse.repository.CountryRepository;
import app.ewarehouse.repository.CountyRepository;
import app.ewarehouse.service.CountyService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.ErrorMessages;
import app.ewarehouse.util.Mapper;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.persistence.EntityNotFoundException;
import jakarta.persistence.Tuple;

import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Optional;

@Service
public class CountyServiceImpl implements CountyService {

    @Autowired
    private CountyRepository countyRepository;
    @Autowired
    private CountryRepository countryRepository;
    
    @Autowired
    ErrorMessages errorMessages;
    
    @Override
    public List<CountyResponse> getAllCounties() {
       return countyRepository.findAll().stream()
    		   .map(Mapper::mapToCountyResponseWithSubCounties)
    		   .toList();
       
    }
    
    @Override
    public Page<CountyResponse> getAllCounties(int page, int size) {
        PageRequest pageRequest = PageRequest.of(page, size);
        return countyRepository.findAll(pageRequest)
                .map(Mapper::mapToCountyResponseWithSubCounties);
    }
    
    @Override
    public Page<CountyResponse> getAllCounties(Pageable pageable) {
        return countyRepository.findAll(pageable)
                .map(Mapper::mapToCountyResponseWithSubCounties);
    }


    @Override
    public CountyResponse getCountyById(Integer id) {
        County county = countyRepository.findById(id).orElse(null);
        return Mapper.mapToCountyResponseWithSubCounties(county);
    }
    
    @Override
    public County saveCounty(String countyData){
    	
    	String decodedData = CommonUtil.inputStreamDecoder(countyData);
    	County county;
    	
    	try {
			county = new ObjectMapper().readValue(decodedData, County.class);
		} catch (Exception e) {
            throw new CustomGeneralException("Invalid JSON data format");
        }
    	
        Optional.ofNullable(county.getCountry())
                .map(Country::getCountryId)
                .map(countryRepository::findById)
                .orElseThrow(() -> new RuntimeException("Country not found"))
                .ifPresent(county::setCountry);
        try {
            return countyRepository.save(county);
        } catch (DataIntegrityViolationException ex) {
            throw new CustomGeneralException(errorMessages.getDuplicateCountyName());
        }
    }

    @Override
    @Transactional
    public void deleteCounty(Integer id) {
        if (!countyRepository.existsById(id)) {
            throw new EntityNotFoundException("County with id " + id + " not found");
        }
        County county = countyRepository.findById(id).orElseThrow(() -> new EntityNotFoundException("County with id " + id + " not found"));
        county.setBitDeletedFlag(!county.getBitDeletedFlag());
        countyRepository.save(county);
    }

    @Override
    public County updateCounty(Integer id, String countyData) {
    	String decodedData = CommonUtil.inputStreamDecoder(countyData);
    	County county;
    	try {
			county = new ObjectMapper().readValue(decodedData, County.class);
		} catch (Exception e) {
            throw new CustomGeneralException("Invalid JSON data format");
        }
        if (!id.equals(county.getId())) {
            throw new IllegalArgumentException("ID in path must match ID in request body");
        }
        return countyRepository.findById(county.getId())
                .map(existingCounty -> {
                    existingCounty.setName(county.getName());
                    existingCounty.setCountry(county.getCountry());
                    return countyRepository.save(existingCounty);
                })
                .orElseThrow(() -> new CustomEntityNotFoundException("County not found"));
    }

	@Override
	public List<ApprovedCountiesDto> getApprovedCounties() {
		List<ApprovedCountiesDto> countyListDto = new ArrayList<>();
		List<Tuple> countyList = countyRepository.getApprovedCountyList();
		for(Tuple tuple : countyList) {
			ApprovedCountiesDto dto = new ApprovedCountiesDto();
			dto.setCountyId((Integer)tuple.get("countyId"));
			dto.setCountyName((String)tuple.get("countyName"));
			countyListDto.add(dto);
		}
		return countyListDto;
	}
}
