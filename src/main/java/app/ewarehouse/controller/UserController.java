package app.ewarehouse.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.service.TuserService;
import app.ewarehouse.util.CommonUtil;

@RestController
@CrossOrigin("*")
@RequestMapping("/user")
public class UserController {
	
	@Autowired
	private TuserService tuserService;
	
	@Autowired
	private ObjectMapper objectMapper;
	
	
	@GetMapping("/details/{userId}")
	public ResponseEntity<?> getUserDetails(@PathVariable("userId") Integer userId)
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(tuserService.getUserDetails(userId)))
				.toString());
	}
	

	@GetMapping("/getInspectors")
	public ResponseEntity<?> getInspectors()
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(tuserService.getInspectors()))
				.toString());
	}
	
	@GetMapping("/getInspectors/{complaintType}")
	public ResponseEntity<?> getInspectorsByComplaintType(@PathVariable("complaintType") Integer complaintType)
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(tuserService.getInspectorsByComplaintType(complaintType)))
				.toString());
	}
	
	@GetMapping("/getCollateral")
	public ResponseEntity<?> getCollateral()
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(tuserService.getCollateral()))
				.toString());
	}
	
	@GetMapping("/getGrader")
	public ResponseEntity<?> getGrader()
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(tuserService.getGrader()))
				.toString());
	}
	
	
	
	private <T> String buildJsonResponse(T response) throws JsonProcessingException {
		return objectMapper.writeValueAsString(ResponseDTO.<T>builder().status(200).result(response).build());
	}
}
