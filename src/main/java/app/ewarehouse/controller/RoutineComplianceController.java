package app.ewarehouse.controller;

import app.ewarehouse.entity.Status;
import app.ewarehouse.service.RoutineComplianceService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Pageable;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Date;

@RestController
@RequestMapping("/admin/routine-compliance")
public class RoutineComplianceController {

    private static final Logger logger = LoggerFactory.getLogger(RoutineComplianceController.class);

    @Autowired
    private RoutineComplianceService routineComplianceService;
    @Autowired
    private ObjectMapper objectMapper;

    @PostMapping
    public ResponseEntity<String> addEdit(@RequestBody String routineCompliance) throws JsonProcessingException {
        logger.info("Inside createOrUpdate method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.save(routineCompliance, 39), objectMapper));
    }

    @PostMapping("/takeAction")
    public ResponseEntity<String> takeAction(@RequestBody String data) throws JsonProcessingException {
        logger.info("Inside takeAction method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.takeAction(data), objectMapper));
    }

    @GetMapping("/{id}")
    public ResponseEntity<String> getById(@PathVariable("id") String id) throws JsonProcessingException {
        logger.info("Inside getById method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.getById(id), objectMapper));
    }

    @GetMapping
    public ResponseEntity<String> getAll() throws JsonProcessingException {
        logger.info("Inside getAll method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.getAll(), objectMapper));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> delete(@PathVariable("id") String id) throws JsonProcessingException {
        logger.info("Inside delete method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.deleteById(id), objectMapper));
    }

    @GetMapping("/paginated")
    public ResponseEntity<String> getAllPaginated(Pageable pageable) throws JsonProcessingException {
        logger.info("Inside getAllPaginated method of RoutineComplianceController");
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.getAll(pageable), objectMapper));
    }

    @GetMapping("/roleView")
    public ResponseEntity<String> getByRoleView(Pageable pageable,
                                                @RequestParam(required = false) Integer roleId,
                                                @RequestParam(required = false) Integer userId,
                                                @RequestParam(required = false) Integer action,
                                                @RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date fromDate,
                                                @RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date toDate,
                                                @RequestParam(required = false) String search,
                                                @RequestParam(required = false, defaultValue = "stmUpdatedAt") String sortColumn,
                                                @RequestParam(required = false, defaultValue = "DESC") String sortDirection) throws JsonProcessingException {
        logger.info("Inside getByRoleView method of RoutineComplianceController");
        logger.info("roleId: " + roleId);
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(routineComplianceService.findByFilters(roleId, fromDate, toDate, search, sortColumn, sortDirection, userId, action, pageable), objectMapper));
    }
}

