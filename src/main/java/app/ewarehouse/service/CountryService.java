package app.ewarehouse.service;

import app.ewarehouse.dto.CountryResponse;
import org.springframework.data.domain.Page;

import java.util.List;


public interface CountryService {
    void saveOrUpdate(String data);
    Page<CountryResponse> getAll(Integer pageNumber, Integer pageSize, String sortCol, String sortDir, String search);
    List<CountryResponse> getAll();
    CountryResponse getById(Integer countryId);
    void toggleActivationStatus(String data);
}
