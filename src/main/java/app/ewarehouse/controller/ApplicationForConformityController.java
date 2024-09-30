package app.ewarehouse.controller;

import app.ewarehouse.dto.ApplicationOfConformityDTO;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.entity.ApplicationOfConformity;
import app.ewarehouse.entity.ApplicationOfConformityParticularOfApplicants;
import app.ewarehouse.entity.ApplicationOfConformitySupportingDocuments;
import app.ewarehouse.entity.ApplicationOfConformityWarehouseOperatorViability;
import app.ewarehouse.entity.Status;
import app.ewarehouse.service.ConformityParticularService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@RestController
@RequestMapping("/conformity")
@CrossOrigin("*")
public class ApplicationForConformityController {

	String path = "src/resources/" + FolderAndDirectoryConstant.AOC_PARTICULAR_FOLDER + "/";

	@Autowired
	private ConformityParticularService conformityParticularService;

	@Autowired
	private ObjectMapper objectMapper;

	@GetMapping("/count/applicantdetails/{intId}")
	public ResponseEntity<String> getDraftStatusOfApplicantDetails(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		Long count = conformityParticularService.getCountByCreatedByAndDraftStatus(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(count)).toString());
	}

	@GetMapping("/applicantdetails/{intId}")
	public ResponseEntity<String> getAocParticularDataById(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		ApplicationOfConformityParticularOfApplicants aocParticular = conformityParticularService
				.getAocParticularDataById(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(aocParticular)).toString());
	}

	@DeleteMapping("/deletedirector/{intId}")
	public ResponseEntity<String> deleteDirectorById(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		conformityParticularService.deleteDirectorById(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse("Data Deleted")).toString());
	}

	@PostMapping("/applicantdetails")
	public ResponseEntity<?> saveApplicantDetails(@RequestBody String data)
			throws JsonMappingException, JsonProcessingException {

		JSONObject response = new JSONObject();

		response = conformityParticularService.saveApplicantData(data);

		return ResponseEntity.ok(response.toString());
	}

	@GetMapping("/count/supportingdocs/{intId}")
	public ResponseEntity<String> getDraftStatusOfSupportingDocs(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		Long count = conformityParticularService.getDraftStatusOfSupportingDocs(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(count)).toString());
	}

	@GetMapping("/supportingdocs/{intId}")
	public ResponseEntity<String> getAocSupportindDocDataById(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		ApplicationOfConformitySupportingDocuments aocSupportingDocs = conformityParticularService
				.getAocSupportindDocDataById(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(aocSupportingDocs)).toString());
	}

	@PostMapping("/supportingdocs")
	public ResponseEntity<?> saveSupportingDocs(@RequestBody String data)
			throws JsonMappingException, JsonProcessingException {

		JSONObject response = new JSONObject();

		response = conformityParticularService.saveSupportingDocsData(data);
		return ResponseEntity.ok(response.toString());
	}

	@GetMapping("/count/operatorviability/{intId}")
	public ResponseEntity<String> getDraftStatusOfViability(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		Long count = conformityParticularService.getDraftStatusOfViability(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(count)).toString());
	}

	@GetMapping("/operatorviability/{intId}")
	public ResponseEntity<String> getViabilityDataById(@PathVariable("intId") Integer intId)
			throws JsonProcessingException {
		ApplicationOfConformityWarehouseOperatorViability aocViabilityDocs = conformityParticularService
				.getViabilityDataById(intId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(aocViabilityDocs)).toString());
	}

	@PostMapping("/operatorviability")
	public ResponseEntity<?> saveOperatorViability(@RequestBody String data) throws JsonProcessingException {

		JSONObject response = new JSONObject();

		response = conformityParticularService.saveOperatorViabilityData(data);
		return ResponseEntity.ok(response.toString());
	}

	@PostMapping("/payment")
	public ResponseEntity<?> savePayment(@RequestBody String data)
			throws JsonMappingException, JsonProcessingException {

		JSONObject response = new JSONObject();

		response = conformityParticularService.savePaymentData(data);
		return ResponseEntity.ok(response.toString());
	}

	// Role based view and take action APIs
	@GetMapping("/id/{intApplicantId}")
	public ResponseEntity<String> getDataById(@PathVariable("intApplicantId") String applicationId)
			throws JsonProcessingException {
		ApplicationOfConformity conformityData = conformityParticularService.findById(applicationId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(conformityData)).toString());
	}

	@PostMapping("/remarks")
	public ResponseEntity<?> giveRemarks(@RequestBody String data)
			throws JsonMappingException, JsonProcessingException {
		conformityParticularService.giveRemarks(data);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(data)).toString());
	}
	// End of Role based view and take action APIs

	@PostMapping("/updateStatus")
	public ResponseEntity<?> updateStatus(@RequestBody String data)
			throws JsonMappingException, JsonProcessingException {
		conformityParticularService.updateApplicationStatus(data);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(data)).toString());
	}

	@PostMapping("/getAll")
	public ResponseEntity<String> allcertificate() throws JsonProcessingException {
		List<ApplicationOfConformityDTO> conformityList = conformityParticularService.findAll();
		// System.out.println("in get all controller"+conformityList);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(conformityList)).toString());
	}

	@GetMapping("/getById/{intApplicantId}")
	public ResponseEntity<String> getById(@PathVariable("intApplicantId") String applicationId)
			throws JsonProcessingException {
		ApplicationOfConformity conformity = conformityParticularService
				.findByApplicationIdWithDirectors(applicationId);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(conformity)).toString());
	}

	@GetMapping("/getByIdAndStatus")
	public ResponseEntity<String> getByIdAndStatus(@RequestParam("userId") Integer userId)
			throws JsonProcessingException {
		ApplicationOfConformityDTO conformity = conformityParticularService.findByUserIdAndStatus(userId);
		return ResponseEntity.ok(CommonUtil.encodedJsonResponse(conformity, objectMapper));
	}

	// user based view of all aoc
	@GetMapping("/paginated/{userId}")
	public ResponseEntity<String> getAllPaginatedByUserId(Pageable pageable, @PathVariable("userId") Integer userId,
			@RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date fromDate,
			@RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date toDate)
			throws JsonProcessingException {
		Page<ApplicationOfConformity> applicationsPage = conformityParticularService.getApplicationByUserId(fromDate,
				toDate, userId, pageable);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(applicationsPage)).toString());
	}

	@GetMapping("/paginated")
	public ResponseEntity<String> getAllPaginated(Pageable pageable,
			@RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date fromDate,
			@RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date toDate,
			@RequestParam(required = false) Status status,
			@RequestParam(required = false) Integer pendingAt
			) throws JsonProcessingException {
		Page<ApplicationOfConformity> applicationsPage = conformityParticularService
				.getApplicationByStatusAndRole(fromDate, toDate, status , pendingAt , pageable);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(applicationsPage)).toString());
	}

	@GetMapping("/commodityTypes/{id}")
	public ResponseEntity<?> getAllCommodityTypes(@PathVariable("id") String Id) throws JsonProcessingException {
		String CommodityList = conformityParticularService.getCommodityTypes(Id);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(CommodityList)).toString());
	}

	@GetMapping("/approved/users/{countyId}/{subCountyId}")
	public ResponseEntity<?> getApprovedApplicationIdAndShop(@PathVariable("countyId") Integer countyId,@PathVariable("subCountyId") Integer subCountyId )
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(
						buildJsonResponse(conformityParticularService.getApprovedApplicationIdAndShop(countyId , subCountyId)))
				.toString());
	}

	@GetMapping("/approved/name/{applicantId}")
	public ResponseEntity<?> getOperatorFullName(@PathVariable("applicantId") String applicantId)
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(conformityParticularService.getOperatorFullName(applicantId)))
				.toString());
	}
	
	@GetMapping("/certificate/{applicantId}")
	public ResponseEntity<?> getCertificate(@PathVariable("applicantId") String applicantId)
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(conformityParticularService.getCertificate(applicantId)))
				.toString());
	}
	
	@GetMapping("/remarks/{applicantId}")
	public ResponseEntity<?> getRemarks(@PathVariable("applicantId") String applicantId)
			throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil
				.inputStreamEncoder(buildJsonResponse(conformityParticularService.getRemarks(applicantId)))
				.toString());
	}
	
	

	private <T> String buildJsonResponse(T response) throws JsonProcessingException {
		return objectMapper.writeValueAsString(ResponseDTO.<T>builder().status(200).result(response).build());
	}

}
