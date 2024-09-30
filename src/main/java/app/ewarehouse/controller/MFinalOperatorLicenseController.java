package app.ewarehouse.controller;

import app.ewarehouse.dto.WarehouseParticularsResponse;
import app.ewarehouse.entity.MFinalOperatorLicense;
import app.ewarehouse.service.MFinalOperatorLicenseService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("admin/finalOperatorLicence")
public class MFinalOperatorLicenseController {

    @Autowired
    MFinalOperatorLicenseService service;
    @Autowired
    ObjectMapper objectMapper;

    private static final Logger logger = LoggerFactory.getLogger( MFinalOperatorLicenseController.class);

    @GetMapping("{id}")
    public ResponseEntity<?> getOperatorLicenceByConformityId(@PathVariable("id") String Id) throws JsonProcessingException {
        logger.info("Inside getOperator License");
        WarehouseParticularsResponse response = service.findByConformityIdAndBitDeleteFlag(Id);
     // logger.info("MFinalOperatorLicense");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }

    @GetMapping()
    public ResponseEntity<?> getOperatorLicenceByConformityId() throws JsonProcessingException {
       List<MFinalOperatorLicense>  response = service.findAllByBitDeleteFlag();
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }

}
