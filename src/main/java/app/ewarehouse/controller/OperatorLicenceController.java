package app.ewarehouse.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.OperatorLicenceDTO;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.entity.Action;
import app.ewarehouse.entity.Stakeholder;
import app.ewarehouse.entity.Status;
import app.ewarehouse.service.OperatorLicenceService;
import app.ewarehouse.util.CommonUtil;

@RestController
@RequestMapping("/admin/operator-licences")
@CrossOrigin("*")
public class OperatorLicenceController {

    private final OperatorLicenceService operatorLicenceService;
    
    @Autowired
    private ObjectMapper objectMapper;

    public OperatorLicenceController(OperatorLicenceService operatorLicenceService) {
        this.operatorLicenceService = operatorLicenceService;
    }
    
    @GetMapping("/technicalGetPending")
    public ResponseEntity<String> getPendingApplicationsForTechnical(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> pendingApplications = operatorLicenceService.getApplications(pageable, Status.Pending, Stakeholder.TECHNICAL, Action.Pending, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(pendingApplications)).toString());
    }
    
    @GetMapping("/revenueGetPending")
    public ResponseEntity<String> getPendingApplicationsForRevenue(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> pendingApplications = operatorLicenceService.getApplications(pageable, Status.InProgress, Stakeholder.REVENUE, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(pendingApplications)).toString());
    }

    @GetMapping("/inspectorGetPending")
    public ResponseEntity<String> getPendingApplicationsForInspector(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> pendingApplications = operatorLicenceService.getApplications(pageable, Status.InProgress, Stakeholder.INSPECTOR, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(pendingApplications)).toString());
    }

    @GetMapping("/clcGetPending")
    public ResponseEntity<String> getPendingApplicationsForClc(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> pendingApplications = operatorLicenceService.getApplications(pageable, Status.InProgress, Stakeholder.CLC, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(pendingApplications)).toString());
    }

    @GetMapping("/cecmGetPending")
    public ResponseEntity<String> getPendingApplicationsForCecm(
            Pageable pageable,
    		@RequestParam(defaultValue = "0") Integer page,
    		@RequestParam(defaultValue = "10") Integer size,
    		@RequestParam(required = false) String search,
    		@RequestParam(required = false) String sortColumn,
    		@RequestParam(required = false) String sortDirection) throws JsonProcessingException {
    	Sort sort = Sort.by(Sort.Direction.fromString(sortDirection != null ? sortDirection : "DESC"), sortColumn != null ? sortColumn : "stmUpdatedAt");
        Pageable sortedPageable = PageRequest.of(pageable.getPageNumber(), pageable.getPageSize(), sort);
//        Pageable pageable = PageRequest.of(page, size, Sort.by(Sort.Order.desc("stmUpdatedAt")));
        Page<OperatorLicenceDTO> pendingApplications = operatorLicenceService.getApplications(sortedPageable, Status.InProgress, Stakeholder.CECM, Action.Forwarded,search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(pendingApplications)).toString());
    }
    
    @GetMapping("/getApprovedApplications")
    public ResponseEntity<String> getApprovedApplications(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> approvedApplications = operatorLicenceService.getApplications(pageable, Status.Approved, Stakeholder.APPLICANT, Action.Forwarded, search);
        System.out.println(approvedApplications.getContent());
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(approvedApplications)).toString());
    }

    @GetMapping("/getRejectedApplications")
    public ResponseEntity<String> getRejectedApplications(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> rejectedApplications = operatorLicenceService.getApplications(pageable, Status.Rejected, Stakeholder.APPLICANT, Action.Rejected, search);
        System.out.println(rejectedApplications.getContent());
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(rejectedApplications)).toString());
    }
    
    @GetMapping("/getApprovedAndRejectedApplications")
    public ResponseEntity<String> getApprovedAndRejectedApplications(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> allApplications = operatorLicenceService.getAllApplications(pageable, Stakeholder.APPLICANT, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(allApplications)).toString());
    }
    
    @GetMapping("/clcGetForwarded")
    public ResponseEntity<String> getForwardedApplicationsToCLC(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> forwardedApplications = operatorLicenceService.getApplications(pageable, Status.Forwarded, Stakeholder.CLC, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(forwardedApplications)).toString());
    }

    @GetMapping("/cecmGetForwarded")
    public ResponseEntity<String> getForwardedApplicationsToCECM(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> forwardedApplications = operatorLicenceService.getApplications(pageable, Status.InProgress, Stakeholder.CECM, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(forwardedApplications)).toString());
    }
    
    @GetMapping("/revenueGetForwarded")
    public ResponseEntity<String> getForwardedApplicationsToRevenue(@RequestParam(defaultValue = "0") int page,
    		@RequestParam(defaultValue = "10") int size, @RequestParam(required = false) String search)
    				throws JsonProcessingException {
        Pageable pageable = PageRequest.of(page, size);
        Page<OperatorLicenceDTO> forwardedApplications = operatorLicenceService.getApplications(pageable, Status.InProgress, Stakeholder.REVENUE, Action.Forwarded, search);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(forwardedApplications)).toString());
    }
    
    @PostMapping
    public ResponseEntity<String> createOperatorLicence(@RequestBody String operatorLicence) throws JsonProcessingException {
    	System.out.println(operatorLicence);
        String savedOperatorLicence = operatorLicenceService.saveOperatorLicence(operatorLicence);
        return ResponseEntity.ok(savedOperatorLicence);    
    }
    @PostMapping("/technicalTakeAction")
    public ResponseEntity<String> handleTechnicalAction(@RequestBody String technicalActionRequestData)
            throws JsonProcessingException {
    	System.out.println("Working!!");
        return ResponseEntity.ok(operatorLicenceService.handleAction(technicalActionRequestData, Stakeholder.REVENUE, Stakeholder.TECHNICAL , Action.Forwarded, Status.InProgress));
    }
    @PostMapping("/revenueTakeAction")
    public ResponseEntity<String> handleRevenueAction(@RequestBody String revenueActionRequestData)
            throws JsonProcessingException {
        return ResponseEntity.ok(operatorLicenceService.handleAction(revenueActionRequestData, Stakeholder.INSPECTOR, Stakeholder.REVENUE, Action.Forwarded, Status.InProgress));
    }

    @PostMapping("/inspectorTakeAction")
    public ResponseEntity<String> handleInspectorAction(@RequestBody String inspectorActionRequestData)
            throws JsonProcessingException {
        return ResponseEntity.ok(operatorLicenceService.handleAction(inspectorActionRequestData, Stakeholder.CLC, Stakeholder.INSPECTOR, Action.Forwarded, Status.InProgress));
    }

    @PostMapping("/clcTakeAction")
    public ResponseEntity<String> handleClcAction(@RequestBody String clcActionRequestData)
            throws JsonProcessingException {
        return ResponseEntity.ok(operatorLicenceService.handleAction(clcActionRequestData, Stakeholder.CECM, Stakeholder.CLC, Action.Forwarded, Status.InProgress));
    }

    @PostMapping("/cecmTakeAction")
    public ResponseEntity<String> handleCecmAction(@RequestBody String cecmActionRequestData)
            throws JsonProcessingException {
        return ResponseEntity.ok(operatorLicenceService.handleAction(cecmActionRequestData, Stakeholder.APPLICANT, Stakeholder.CECM, Action.Forwarded, Status.Approved));
    }

    @GetMapping
    public ResponseEntity<String> getOperatorLicences(@RequestParam int page, @RequestParam int size, @RequestParam(required = false) Integer userId, @RequestParam(required = false) Status status) throws JsonProcessingException {
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(operatorLicenceService.getOperatorLicences(status, userId, PageRequest.of(page, size)), objectMapper));
    }

    @GetMapping("/list")
    public ResponseEntity<String> getOperatorLicencesList(@RequestParam(required = false) Status status, @RequestParam(required = false) Integer userId) throws JsonProcessingException {
        return ResponseEntity.ok(CommonUtil.encodedJsonResponse(operatorLicenceService.getOperatorLicences(status, userId), objectMapper));
    }
	
	private <T> String buildJsonResponse(T response) throws JsonProcessingException {
		return objectMapper.writeValueAsString(ResponseDTO.<T>builder().status(200).result(response).build());
	}
	
	@GetMapping("getLicenceDetails")
	public ResponseEntity<String> getOperatorLicence(@RequestParam Integer id) throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(operatorLicenceService.getOperatorLicence(id))).toString());
//		return operatorLicenceService.getOperatorLicence(id);
	}
}
