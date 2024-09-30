package app.ewarehouse.service;

import java.util.List;
import java.util.Optional;

import app.ewarehouse.dto.ApprovedSubCountyDto;
import app.ewarehouse.dto.SubCountyResponse;

public interface SubCountyService {
	List<SubCountyResponse> getAllSubCounties();
    Optional<SubCountyResponse> getSubCountyById(Integer id);
    SubCountyResponse createSubCounty(String subCounty);
    SubCountyResponse updateSubCounty(Integer id, String subCounty);
    void deleteSubCounty(Integer id);
    List<SubCountyResponse> getSubCountiesByCountyId(Integer countyId);

    List<SubCountyResponse> getAllSubCountiesActiveAndInactive();
	List<ApprovedSubCountyDto> getApprovedSubCounties(Integer countyId);
}
