package app.ewarehouse.controller;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

import app.ewarehouse.dto.ComplaintDetailsComprehensive;
import app.ewarehouse.dto.ComplaintmanagementResponse;
import app.ewarehouse.entity.ComplaintObservation;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import app.ewarehouse.service.Complaint_managmentService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.Complaint_managmentValidationCheck;

@RestController
@CrossOrigin("*")
@RequestMapping("/complaint-management")
public class Complaint_managmentController {
	@Autowired
	ObjectMapper objectMapper;

	@Autowired
	private Complaint_managmentService complaint_managmentService;
	String data = "";
	private static final Logger logger = LoggerFactory.getLogger(Complaint_managmentController.class);
	JSONObject resp = new JSONObject();
	@Value("${finalUpload.path}")
	private String finalUploadPath;

	@PostMapping("/complaint-management/addEdit")
	public ResponseEntity<?> create(@RequestBody String complaint_managment) throws Exception {
		logger.info("Inside create method of Complaint_managmentController");
		try {
			JSONObject requestObj = new JSONObject(complaint_managment);
			if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
					requestObj.getString("REQUEST_TOKEN"))) {
				data = CommonUtil.inputStreamDecoder(complaint_managment);
				String validationMsg = Complaint_managmentValidationCheck.BackendValidation(new JSONObject(data));
				if (validationMsg != null) {
					resp.put("status", 502);
					resp.put("errMsg", validationMsg);
					logger.warn("Inside create method of Complaint_managmentController Validation Error");
				} else {
					JSONObject jsob = new JSONObject(data);
					data = jsob.toString();
					resp = complaint_managmentService.save(data);
				}
			} else {
				resp.put("msg", "error");
				resp.put("status", 417);
			}
		} catch (Exception e) {
			logger.error("Inside create method of Complaint_managmentController error occur:" + e);
			resp.put("status", 400);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/complaint-management/preview")
	public ResponseEntity<?> getById(@RequestBody String formParams) {
		logger.info("Inside getById method of Complaint_managmentController");
		try {
			JSONObject requestObj = new JSONObject(formParams);
			if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
					requestObj.getString("REQUEST_TOKEN"))) {
				data = CommonUtil.inputStreamDecoder(formParams);
				JSONObject json = new JSONObject(data);
				JSONObject entity = null;
				entity = complaint_managmentService.getById(json.getInt("intId"));
				resp.put("status", 200);
				resp.put("result", entity);
			} else {
				resp.put("msg", "error");
				resp.put("status", 417);
			}
		} catch (Exception e) {
			logger.error("Inside getById method of Complaint_managmentController error occur:" + e);
			resp.put("status", 400);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/complaint-management/all")
	public ResponseEntity<?> getAll(@RequestBody String formParams) {
		logger.info("Inside getAll method of Complaint_managmentController");
		try {
			JSONObject requestObj = new JSONObject(formParams);
			if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
					requestObj.getString("REQUEST_TOKEN"))) {
				resp = complaint_managmentService.getAll(CommonUtil.inputStreamDecoder(formParams));
			} else {
				resp.put("msg", "error");
				resp.put("status", 417);
			}
		} catch (Exception e) {
			logger.error("Inside getAll method of Complaint_managmentController error occur:" + e);
			resp.put("status", 400);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/complaint-management/delete")
	public ResponseEntity<?> delete(@RequestBody String formParams) {
		logger.info("Inside delete method of Complaint_managmentController");
		try {
			JSONObject requestObj = new JSONObject(formParams);
			if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
					requestObj.getString("REQUEST_TOKEN"))) {
				data = CommonUtil.inputStreamDecoder(formParams);
				JSONObject json = new JSONObject(data);
				resp = complaint_managmentService.deleteById(json.getInt("intId"));
			} else {
				resp.put("msg", "error");
				resp.put("status", 417);
			}
		} catch (Exception e) {
			logger.error("Inside delete method of Complaint_managmentController error occur:" + e);
			resp.put("status", 400);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@GetMapping("/complaint-management/download/{name}")
	public ResponseEntity<Resource> download(@PathVariable("name") String name) throws IOException {
		logger.info("Inside download method of Complaint_managmentController");
		File file = new File(finalUploadPath + "complaint-management/" + name);
		Path path = Paths.get(file.getAbsolutePath());
		ByteArrayResource byteArrayResource = new ByteArrayResource(Files.readAllBytes(path));
		return ResponseEntity.ok().headers(headers(name)).contentLength(file.length())
				.contentType(MediaType.parseMediaType("application/octet-stream")).body(byteArrayResource);
	}

	private HttpHeaders headers(String name) {
		HttpHeaders header = new HttpHeaders();
		header.add(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=" + name);
		header.add("Cache-Control", "no-cache, no-store, must-revalidate");
		header.add("Pragma", "no-cache");
		header.add("Expires", "0");
		return header;
	}

	@PostMapping("/complaint-management/getAllAproveAction")
	public ResponseEntity<?> getAllAproveActionById(@RequestBody String formParams) {
		logger.info("Inside getgetAllAproveActionById method of Complaint_managmentController");
		try {
			JSONObject requestObj = new JSONObject(formParams);
			if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
					requestObj.getString("REQUEST_TOKEN"))) {
				data = CommonUtil.inputStreamDecoder(formParams);
				resp = complaint_managmentService.getByActionWise(data);
			} else {
				resp.put("msg", "error");
				resp.put("status", 417);
			}
		} catch (Exception e) {
			logger.error("Inside getgetAllAproveActionById method of Complaint_managmentController error occur:" + e);
			resp.put("msg", "error");
			resp.put("status", 400);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}


	@GetMapping("/paginated")
	ResponseEntity<?> getAllPaginated(
			Pageable pageable,
			@RequestParam(required = false) String search,
			@RequestParam(required = false) String sortColumn,
			@RequestParam(required = false) String sortDirection
	) throws JsonProcessingException {
		logger.info("Inside getAllPaginated method of Complaint_managementController");

		Sort sort = Sort.by(Sort.Direction.fromString(sortDirection != null ? sortDirection : "DESC"),
				sortColumn != null ? sortColumn : "dtmCreatedOn");
		Pageable sortedPageable = PageRequest.of(pageable.getPageNumber(), pageable.getPageSize(), sort);
		logger.info("pages:"+pageable.getPageNumber());
		logger.info("size:"+pageable.getPageSize());
		Page<ComplaintmanagementResponse> response = complaint_managmentService.getFilteredComplaint(search, sortColumn, sortDirection, sortedPageable);
		logger.info("Content: {}", response.getContent());

		return ResponseEntity.ok(CommonUtil.encodedJsonResponse(response,objectMapper));
	}


	@GetMapping("/complaint-management/preview/{id}")
	public ResponseEntity<?> findById(@PathVariable Integer id) {
		logger.info("Inside getById method of Complaint_managementController");
		try {
			ComplaintDetailsComprehensive entity = complaint_managmentService.findById(id);
			if (entity != null) {
				return ResponseEntity.ok(CommonUtil.encodedJsonResponse(entity, objectMapper));
			} else {
				JSONObject errorResponse = new JSONObject();
				errorResponse.put("msg", "error");
				errorResponse.put("status", 404);
				return ResponseEntity.status(HttpStatus.NOT_FOUND).body(errorResponse.toString());
			}
		} catch (Exception e) {
			logger.error("Inside getById method of Complaint_managementController error occurred:", e);
			JSONObject errorResponse = new JSONObject();
			errorResponse.put("status", 500);
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(errorResponse.toString());
		}
	}
	
	
	@GetMapping("/complaintobservation/{lableid}/{roleid}")
	public ResponseEntity<?> findObservation(@PathVariable Integer lableid,@PathVariable Integer roleid) {
		logger.info("Inside getById method of Complaint_managementController");
		try {
			
			List<ComplaintObservation> observationList=complaint_managmentService.getComplaintOservation(lableid,roleid);
			
			if (observationList != null) {
				return ResponseEntity.ok(CommonUtil.encodedJsonResponse(observationList, objectMapper));
			} else {
				JSONObject errorResponse = new JSONObject();
				errorResponse.put("msg", "error");
				errorResponse.put("status", 404);
				return ResponseEntity.status(HttpStatus.NOT_FOUND).body(errorResponse.toString());
			}
			
		} catch (Exception e) {
			logger.error("Inside getById method of Complaint_managementController error occurred:", e);
			logger.error("Inside getById method of Complaint_managementController error occurred:", e);
			JSONObject errorResponse = new JSONObject();
			errorResponse.put("status", 500);
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(errorResponse.toString());
		}
		
	
	}
	
	
	
	



}