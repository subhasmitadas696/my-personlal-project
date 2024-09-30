package app.ewarehouse.service;

import app.ewarehouse.dto.ApprovedCountiesDto;
import app.ewarehouse.dto.CountyResponse;
import app.ewarehouse.entity.County;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.List;

public interface CountyService {
    List<CountyResponse> getAllCounties();
    CountyResponse getCountyById(Integer id);
    County saveCounty(String county);
    void deleteCounty(Integer id);
    County updateCounty(Integer id, String county);
	Page<CountyResponse> getAllCounties(int page, int size);
	Page<CountyResponse> getAllCounties(Pageable pageable);
	List<ApprovedCountiesDto> getApprovedCounties();
}
