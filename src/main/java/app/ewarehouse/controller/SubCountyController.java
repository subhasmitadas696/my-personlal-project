package app.ewarehouse.controller;

import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.dto.SubCountyResponse;
import app.ewarehouse.service.SubCountyService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/subcounties")
public class SubCountyController {

    private static final Logger logger = LoggerFactory.getLogger(SubCountyController.class);

    @Autowired
    private ObjectMapper objectMapper;

	@Autowired
    private SubCountyService subCountyService;

    @GetMapping
    public ResponseEntity<String> getAllSubCounties() throws JsonProcessingException {
        logger.info("Inside getAllSubCounties method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.getAllSubCounties())).toString());
    }

    @GetMapping("/activeAndInactive")
    public ResponseEntity<String> getAllActiveAndInActive() throws JsonProcessingException {
        logger.info("Inside getAllSubCounties method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.getAllSubCountiesActiveAndInactive())).toString());
    }

    @GetMapping("/{id}")
    public ResponseEntity<String> getSubCountyById(@PathVariable Integer id) throws JsonProcessingException {
        logger.info("Inside getSubCountyById method of SubCountyController");
        SubCountyResponse subCounty = subCountyService.getSubCountyById(id).orElseThrow(() -> new RuntimeException("Sub County not found."));
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCounty)).toString());
    }

    @PostMapping
    public ResponseEntity<String> createSubCounty(@RequestBody String subCounty) throws JsonProcessingException {
        logger.info("Inside createSubCounty method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.createSubCounty(subCounty))).toString());
    }

    @PostMapping("/{id}")
    public ResponseEntity<String> updateSubCounty(@PathVariable Integer id, @RequestBody String subCounty) throws JsonProcessingException {
        logger.info("Inside updateSubCounty method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.updateSubCounty(id, subCounty))).toString());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> deleteSubCounty(@PathVariable Integer id) throws JsonProcessingException {
        logger.info("Inside deleteSubCounty method of SubCountyController");
        subCountyService.deleteSubCounty(id);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(id)).toString());
    }
    
    @GetMapping("/county/{countyId}")
    public ResponseEntity<String> getSubCountiesByCountyId(@PathVariable Integer countyId) throws JsonProcessingException {
        logger.info("Inside getSubCountiesByCountyId method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.getSubCountiesByCountyId(countyId))).toString());
    }
    
    
    @GetMapping("/approved/{countyId}")
    public ResponseEntity<String> getApprovedSubCounties(@PathVariable("countyId") Integer countyId) throws JsonProcessingException {
        logger.info("Inside getAllSubCounties method of SubCountyController");
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(subCountyService.getApprovedSubCounties(countyId))).toString());
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
