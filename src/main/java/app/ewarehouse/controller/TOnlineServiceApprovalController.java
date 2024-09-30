package app.ewarehouse.controller;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import app.ewarehouse.service.Complaint_managmentService;
import app.ewarehouse.service.TOnlineServiceApprovalService;
import app.ewarehouse.util.CommonUtil;
@RestController
@CrossOrigin("*")
public class TOnlineServiceApprovalController {

	@Autowired
	private TOnlineServiceApprovalService tOnlineServiceapprovalService;
	
	@Autowired
	private Complaint_managmentService complaint_reportingService;

	JSONObject resp = new JSONObject();

	@Value("${tempUpload.path}")
	private String tempUploadPath;

	@Value("${queryFileUpload.path}")
	private String queryFileUploadPath;

	@PostMapping("/getActionDetails")
	public ResponseEntity<?> getActionDetails(@RequestBody String formParams) {
		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();
		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			jsonObj = tOnlineServiceapprovalService.getTakeActionDetails(data);
		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());
	}

	@PostMapping("/getPreview")
	public ResponseEntity<?> getPreviewData(@RequestBody String formParams) throws Exception {
		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();

		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			JSONObject coreData = new JSONObject(data);
			Integer serviceId = coreData.getInt("serviceid");
			Integer processId = coreData.getInt("intId");
			switch (processId) {
			
			case(160):
				jsonObj=complaint_reportingService.getPreviewDetails(serviceId);
				break;
			}

		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());
	}

	@PostMapping("/takeAction")
	public ResponseEntity<?> getTakeAction(@RequestBody String formParams) throws Exception {
		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();
		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			JSONObject requestedData = new JSONObject(data);
			Integer dupprocessId = requestedData.optInt("intId", 0);

			switch (dupprocessId) {

			case(160):
				jsonObj=complaint_reportingService.getEventTakeActionDetails(data);
			break;
			
			}

		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());
	}

	@PostMapping("/getNotingDetails")
	public ResponseEntity<?> getNotingDetails(@RequestBody String formParams) {
		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();
		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			jsonObj = tOnlineServiceapprovalService.getAllNotingDetails(data);
		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());
	}

	@PostMapping("/getQueryDetails")
	public ResponseEntity<?> getQueryDetails(@RequestBody String formParams) {

		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();
		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			jsonObj = tOnlineServiceapprovalService.getQueryDetailsByUsingData(data);
		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());

	}

	@PostMapping("/queryReplyInsert")

	public ResponseEntity<?> QueryReplyInsert(@RequestParam("remark") String remark,
			@RequestParam("processId") Integer processId, @RequestParam("serviceId") Integer serviceId,
			@RequestParam("dynamicListArr") String dynamicListArr,
			@RequestParam("arrUploadedFiles") String arrUploadedFiles) {
		JSONObject response = new JSONObject();
		try {

			JSONObject jsObj = new JSONObject();
			jsObj.put("remark", remark);
			jsObj.put("processId", processId);
			jsObj.put("serviceId", serviceId);
			jsObj.put("dynamicListArr", dynamicListArr);
			jsObj.put("arrUploadedFiles", arrUploadedFiles);

			response = tOnlineServiceapprovalService.saveIntoQueryDetails(jsObj);

		} catch (Exception e) {

			e.printStackTrace();
		}
		return ResponseEntity.ok(response.toString());
	}

	@GetMapping("/queryDocs/{name}")
	public ResponseEntity<Resource> download(@PathVariable("name") String name) throws IOException {
		File file = new File(queryFileUploadPath + name);
		Path path = Paths.get(file.getAbsolutePath());
		ByteArrayResource byteArrayResource = new ByteArrayResource(Files.readAllBytes(path));

		HttpHeaders headers = headers(name);

		return ResponseEntity.ok().headers(headers).contentLength(file.length())
				.contentType(MediaType.parseMediaType("application/pdf")).body(byteArrayResource);
	}

	private HttpHeaders headers(String name) {
		HttpHeaders header = new HttpHeaders();
		header.add(HttpHeaders.CONTENT_DISPOSITION, "inline; filename=" + name);
		header.add("Cache-Control", "no-cache, no-store, must-revalidate");
		header.add("Pragma", "no-cache");
		header.add("Expires", "0");
		return header;
	}

	@PostMapping("/noResubmitStatus")
	public ResponseEntity<?> noResubmitStatus(@RequestBody String formParams) {
		JSONObject requestObj = new JSONObject(formParams);
		JSONObject jsonObj = new JSONObject();
		if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
			String data = CommonUtil.inputStreamDecoder(formParams);
			jsonObj = tOnlineServiceapprovalService.noResubmitStatus(data);
		}
		JSONObject roln = CommonUtil.inputStreamEncoder(jsonObj.toString());
		return ResponseEntity.ok(roln.toString());
	}

	@PostMapping("/getAllForwradAuthority")
	public ResponseEntity<?> getAllForwradAuthority(@RequestBody String fromParams) {
		try {
			JSONObject jsb = new JSONObject(fromParams);
			if (CommonUtil.hashRequestMatch(jsb.getString("REQUEST_DATA"), jsb.getString("REQUEST_TOKEN"))) {
				String data = CommonUtil.inputStreamDecoder(fromParams);
				resp = tOnlineServiceapprovalService.getAllForwradAuthority(data);
			}
		} catch (Exception e) {
			resp.put("status", 417);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}
}
