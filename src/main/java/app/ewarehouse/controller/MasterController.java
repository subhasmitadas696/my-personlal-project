/**
 *
 */
package app.ewarehouse.controller;

import java.util.Map;

import app.ewarehouse.dto.ComplaintTypeResponse;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.entity.ComplaintType;
import app.ewarehouse.service.MasterService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

/**
 * Priyanka Singh
 */
@RestController
@RequestMapping(value = "/master")
@CrossOrigin("*")
public class MasterController {

    private static final Logger logger = LoggerFactory.getLogger(MasterController.class);
    String path = "src/resources/" + FolderAndDirectoryConstant.BUYER_REG_FOLDER + "/";

    @Autowired
    private MasterService masterService;

    @Autowired
    private ObjectMapper objectMapper;

    @ResponseBody
    @PostMapping(value = "/saveComplainttype")
    public ResponseEntity<?>  saveComplainttype(@RequestBody String complaintType) throws JsonProcessingException {
            Integer complainttype= masterService.saveComplainttype(complaintType);
            return ResponseEntity.ok(CommonUtil.encodedJsonResponse(complainttype,objectMapper));
    }

    @GetMapping("/paginated")
    public ResponseEntity<?> getAllCounties(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size) throws JsonProcessingException {
            Pageable pageable = PageRequest.of(page, size);
            Page<ComplaintTypeResponse> pageResult = masterService.getAllcomplaintList(pageable);
            return ResponseEntity.ok(CommonUtil.encodedJsonResponse(pageResult,objectMapper));
    }

    @ResponseBody
    @PostMapping(value = "/updateComplaintType")
    public ResponseEntity<?>  updateComplaintType(@RequestBody String complaintType) throws JsonProcessingException {

            Integer I = masterService.update(complaintType);
            return ResponseEntity.ok(CommonUtil.encodedJsonResponse(I,objectMapper));
    }

    @ResponseBody
    @GetMapping(value = "/getbyComplaintTypeid")
    public ResponseEntity<?>  getbyComplaintTypeid(
            @RequestParam(value = "complaintId", required = false) Integer complaintId) throws JsonProcessingException {
            ComplaintType ComplaintTypeData = masterService.getbyid(complaintId);
            return ResponseEntity.ok(CommonUtil.encodedJsonResponse(ComplaintTypeData,objectMapper));
    }

    private <T> String buildJsonResponse(T response) throws JsonProcessingException {
        return objectMapper.writeValueAsString(ResponseDTO.<T>builder().status(200).result(response).build());
    }


    @DeleteMapping("/{complaintId}")
    public ResponseEntity<?> deleteComplaint(@PathVariable Integer complaintId) {
        masterService.deleteComplaint(complaintId);
        return ResponseEntity.ok(Map.of("status", "DELETED", "msg", "Request successful"));
    }


	@GetMapping
	public ResponseEntity<?> getAllComplaintTypes() throws JsonProcessingException {
		ResponseDTO<Object> responseDTO = ResponseDTO.builder()
				.status(200)
				.result(masterService.getAllComplaintTypes())
				.build();
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(objectMapper.writeValueAsString(responseDTO)).toString());
	}
	
	@GetMapping("/{roleId}")
	public ResponseEntity<?> getComplaintTypesByRoleId(@PathVariable("roleId") Integer roleId) throws JsonProcessingException {
		ResponseDTO<Object> responseDTO = ResponseDTO.builder()
				.status(200)
				.result(masterService.getComplaintTypesByRoleId(roleId))
				.build();
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(objectMapper.writeValueAsString(responseDTO)).toString());
	}

}
