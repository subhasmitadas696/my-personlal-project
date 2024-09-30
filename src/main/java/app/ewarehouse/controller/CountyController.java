package app.ewarehouse.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.CountyResponse;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.entity.County;
import app.ewarehouse.service.CountyService;
import app.ewarehouse.util.CommonUtil;
import jakarta.persistence.EntityNotFoundException;
@RestController
@RequestMapping("/counties")
@CrossOrigin("*")
public class CountyController {

    @Autowired
    private CountyService countyService;
    @Autowired
    private ObjectMapper objectMapper;

    @GetMapping
    public ResponseEntity<?> getAllCounties() throws JsonProcessingException {
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(countyService.getAllCounties(), objectMapper));
    }
    
    @GetMapping("/paginated")
    public ResponseEntity<?> getAllCounties(
        @RequestParam(defaultValue = "0") int page,
        @RequestParam(defaultValue = "10") int size) throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size,Sort.by(Sort.Order.desc("id")));
        Page<CountyResponse> pageResult = countyService.getAllCounties(pageable);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(pageResult, objectMapper));
    }
    
    @GetMapping("/{id}")
    public ResponseEntity<?> getCountyById(@PathVariable Integer id) {
    	var county = countyService.getCountyById(id);
        if (county != null) {
            return ResponseEntity.ok(county);
        } else {
            throw new EntityNotFoundException("County with id " + id + " not found");
        }
    }
    
    @PostMapping
    public ResponseEntity<?> saveCounty(@RequestBody String county) throws JsonProcessingException {
    	System.out.println(county);
    	County response = countyService.saveCounty(county);
    	return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response, objectMapper));
    } 

    @PostMapping("/{id}")
    public ResponseEntity<?> updateCounty(@PathVariable Integer id, @RequestBody String county) throws JsonProcessingException{ 
    	System.out.println("Inside this metho!!");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(countyService.updateCounty(id,county))).toString());
    }
    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCounty(@PathVariable Integer id) throws JsonProcessingException {
    	System.out.println("Inside delete county method!");
        countyService.deleteCounty(id);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(id)).toString());
    }
    
    @GetMapping("/approved")
    public ResponseEntity<?> getApprovedCounties() throws JsonProcessingException {
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(countyService.getApprovedCounties(), objectMapper));
    }
    
    private <T> String buildJsonResponse(T response) throws JsonProcessingException {
        return objectMapper.writeValueAsString(
                ResponseDTO.<T>builder()
                        .status(200)
                        .result(response)
                        .build()
        );
    }
}
