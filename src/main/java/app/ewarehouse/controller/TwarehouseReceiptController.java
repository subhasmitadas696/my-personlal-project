package app.ewarehouse.controller;

import app.ewarehouse.dto.WarehouseReceiptResponse;
import app.ewarehouse.entity.Status;
import app.ewarehouse.entity.TwarehouseReceipt;
import app.ewarehouse.service.TwarehouseReceiptService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("admin/warehouseReceipt")
public class TwarehouseReceiptController {

    @Autowired
    TwarehouseReceiptService service;
    @Autowired
    ObjectMapper objectMapper;

    private static final Logger logger = LoggerFactory.getLogger(TwarehouseReceiptController.class);

    @GetMapping
    ResponseEntity<?> getAll() throws JsonProcessingException {
        List<TwarehouseReceipt> response = service.findAll();
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }

    @GetMapping("/paginated")
    ResponseEntity<?> getAllPaginated(
            Pageable pageable,
            @RequestParam(required = false) Status status,
            @RequestParam(required = false) String search,
            @RequestParam(required = false) String sortColumn,
            @RequestParam(required = false) String sortDirection
    ) throws JsonProcessingException {
        logger.info("Inside getAllPaginated method of TwarehouseReceiptController");
        logger.info("STATUS IS: {}",status);

        Sort sort = Sort.by(Sort.Direction.fromString(sortDirection != null ? sortDirection : "DESC"),
                sortColumn != null ? sortColumn : "dtmCreatedOn");
        Pageable sortedPageable = PageRequest.of(pageable.getPageNumber(), pageable.getPageSize(), sort);

        logger.info(sortedPageable + " " + status);

        Page<WarehouseReceiptResponse> response = service.getFilteredReceipts( status, search, sortColumn, sortDirection, sortedPageable);
        logger.info("Content: {}", response.getContent());

        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }
    @GetMapping("{id}")
    ResponseEntity<?> getDetailsById(@PathVariable String id ) throws Exception {
        WarehouseReceiptResponse commodity = service.getDetailsById(id);
        logger.info("Warehouse receipt response is : {}",commodity);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(commodity,objectMapper));
    }
    @GetMapping("/getReceiptForDepositor/{id}")
    ResponseEntity<?> getReceiptsForDepositor(@PathVariable String id ) throws JsonProcessingException {
        List<WarehouseReceiptResponse> response = service.getReceiptForDepositor(id);
        logger.info("t WarehouseReceiptList details are: {}", response);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }
    @PostMapping
    ResponseEntity<?> generateReceipt(@RequestBody String data) throws JsonProcessingException {
        TwarehouseReceipt response = service.save(data);
        return  ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }
    @GetMapping("/getEligibleLossReceiptListByDepositor/{id}")
    ResponseEntity<?> getEligibleLossReceiptListByDepositor(@PathVariable String id ) throws JsonProcessingException {
        List<WarehouseReceiptResponse> response = service.getEligibleLossReceiptListByDepositor(id);
        logger.info("t WarehouseReceiptList details are: "+response);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }

    @GetMapping("/getReceiptNo/{id}")
    ResponseEntity<?> getReceiptsNoForDepositor(@PathVariable String id ) throws JsonProcessingException {
        List<String> response = service.getReceiptNoForDepositor(id);
        logger.info("t WarehouseReceiptList details are: {}", response);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
    }
}
