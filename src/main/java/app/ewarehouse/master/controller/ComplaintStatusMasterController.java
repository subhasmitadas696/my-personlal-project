package app.ewarehouse.master.controller;

import java.util.List;

import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import app.ewarehouse.exception.DuplicateComplaintStatusException;
import app.ewarehouse.master.dto.ComplaintStatusMasterDto;
import app.ewarehouse.master.entity.ComplaintStatusMaster;
import app.ewarehouse.master.service.ComplaintStatusMasterService;

/**
 * @author chinmaya.jena
 * @since 03-07-2024
 */

@RestController
@CrossOrigin("*")
@RequestMapping("/api/master/complaint-status")
public class ComplaintStatusMasterController {

	
	private final ComplaintStatusMasterService service;

    public ComplaintStatusMasterController(ComplaintStatusMasterService service) {
        this.service = service;
    }
    
    private static final Logger logger = LoggerFactory.getLogger(ComplaintStatusMasterController.class);

    @PostMapping("/")
    public ResponseEntity<ComplaintStatusMaster> createComplaintStatus(@RequestBody ComplaintStatusMasterDto complaintStatusMasterDto) {
    	logger.info("Inside createComplaintStatus method of ComplaintStatusMasterController");
        ComplaintStatusMaster createdComplaintStatus = service.createComplaintStatus(complaintStatusMasterDto);
        return ResponseEntity.ok(createdComplaintStatus);
    }
    
    @ExceptionHandler(DuplicateComplaintStatusException.class)
    public ResponseEntity<String> handleDuplicateComplaintStatusException(DuplicateComplaintStatusException ex) {
        return ResponseEntity.status(HttpStatus.CONFLICT).body(ex.getMessage());
    }

    @PostMapping("/{id}")
    public ResponseEntity<ComplaintStatusMaster> updateComplaintStatus(@PathVariable Integer id, @RequestBody ComplaintStatusMasterDto complaintStatusMasterDto) {
    	logger.info("Inside updateComplaintStatus method of ComplaintStatusMasterController");
        ComplaintStatusMaster updatedComplaintStatus = service.updateComplaintStatus(id, complaintStatusMasterDto);
        if (updatedComplaintStatus == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(updatedComplaintStatus);
    }

    @GetMapping("/{id}")
    public ResponseEntity<ComplaintStatusMaster> getComplaintStatusById(@PathVariable Integer id) {
    	logger.info("Inside getComplaintStatusById method of ComplaintStatusMasterController");
        ComplaintStatusMaster complaintStatus = service.getComplaintStatusById(id);
        if (complaintStatus == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(complaintStatus);
    }

    @GetMapping("/")
    public ResponseEntity<List<ComplaintStatusMaster>> getAllComplaintStatuses() {
    	logger.info("Inside getAllComplaintStatuses method of ComplaintStatusMasterController");
        List<ComplaintStatusMaster> complaintStatuses = service.getAllComplaintStatuses();
        return ResponseEntity.ok(complaintStatuses);
    }

    @GetMapping("/change/{id}")
    public ResponseEntity<String> changeComplaintStatus(@PathVariable Integer id) {
    	logger.info("Inside changeComplaintStatus method of ComplaintStatusMasterController");
    	JSONObject response = service.changeComplaintStatus(id);
        return ResponseEntity.ok(response.toString());
    }
	
	
}
