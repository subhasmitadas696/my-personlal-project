package app.ewarehouse.controller;

import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.service.CountryService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;


@RestController
@RequestMapping("admin/countries")
public class MCountryController {
    private static final Logger log = LoggerFactory.getLogger(MCountryController.class);
    private final CountryService countryService;
    private final ObjectMapper objectMapper;

    public MCountryController(CountryService countryService, ObjectMapper objectMapper) {
        this.countryService = countryService;
        this.objectMapper = objectMapper;
    }

    @PostMapping
    public ResponseEntity<String> saveOrUpdate(@RequestBody String data) throws JsonProcessingException {
        log.info("Inside saveOrUpdate method of CountryController");
        countryService.saveOrUpdate(data);
        return buildResponse(ResponseDTO.builder().status(HttpStatus.OK.value()).message(HttpStatus.OK.getReasonPhrase()).build());
    }

    @GetMapping
    public ResponseEntity<String> getAll(@RequestParam(value = "page", required = false) Integer pageNumber, @RequestParam(value = "size", required = false) Integer pageSize,  @RequestParam(value = "sortDirection", required = false) String sortDir,  @RequestParam(value = "sortColumn", required = false) String sortCol, @RequestParam(required = false) String search) throws JsonProcessingException {
        log.info("Inside getAll method of CountryController");
        ResponseDTO<Object> responseDTO = new ResponseDTO<>();
        responseDTO.setStatus(HttpStatus.OK.value());
        responseDTO.setResult(countryService.getAll(pageNumber, pageSize, sortCol, sortDir, search));
        return buildResponse(responseDTO);
    }

    @PostMapping("/toggleActivationStatus")
    public ResponseEntity<String> toggleActivationStatus(@RequestBody String data) throws JsonProcessingException {
        log.info("Inside toggleActivationStatus method of CountryController");
        countryService.toggleActivationStatus(data);
        return buildResponse(ResponseDTO.builder().status(HttpStatus.OK.value()).message(HttpStatus.OK.getReasonPhrase()).build());
    }

    private ResponseEntity<String> buildResponse(ResponseDTO<?> responseDTO) throws JsonProcessingException {
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(objectMapper.writeValueAsString(responseDTO)).toString());
    }
}
