package app.ewarehouse.controller;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Iterator;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
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
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;

import app.ewarehouse.service.TmenulinksService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.TmenulinksValidationCheck;
@RestController
@CrossOrigin("*")
@RequestMapping("/adminconsole")
public class TmenulinksController {
	@Autowired
	private TmenulinksService tmenulinksService;
	String path = "src/storage/manage-links/";
	String data = "";
	JSONObject resp = new JSONObject();

	@PostMapping("/manage-links/addEdit")
	public ResponseEntity<?> create(@RequestBody String tmenulinks)
			throws JsonMappingException, JsonProcessingException {
		data = CommonUtil.inputStreamDecoder(tmenulinks);
		if (TmenulinksValidationCheck.BackendValidation(new JSONObject(data)) != null) {
			resp.put("status", 502);
			resp.put("errMsg", TmenulinksValidationCheck.BackendValidation(new JSONObject(data)));
		} else {
			resp = tmenulinksService.save(data);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/manage-links/preview")
	public ResponseEntity<?> getById(@RequestBody String formParams) {
		data = CommonUtil.inputStreamDecoder(formParams);
		JSONObject json = new JSONObject(data);
		JSONObject entity = tmenulinksService.getById(json.getInt("intId"));
		resp.put("status", 200);
		resp.put("result", entity);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/manage-links/all")
	public ResponseEntity<?> getAll(@RequestBody String formParams) {
		JSONArray entity = tmenulinksService.getAll(CommonUtil.inputStreamDecoder(formParams));
		resp.put("status", 200);
		resp.put("result", entity);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/manage-links/delete")
	public ResponseEntity<?> delete(@RequestBody String formParams) {
		data = CommonUtil.inputStreamDecoder(formParams);
		JSONObject json = new JSONObject(data);
		JSONObject entity = tmenulinksService.deleteById(json.getInt("intId"));
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(entity.toString()).toString());
	}

	@GetMapping("/manage-links/download/{name}")
	public ResponseEntity<Resource> download(@PathVariable("name") String name) throws IOException {
		System.out.println(name);
		File file = new File(path + name);
		System.out.println("the file is:" + file);
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
	
	@PostMapping(value = "/getLeftMenuLinks")
	public ResponseEntity<?> getLeftMenuLinks(@RequestBody String data) {
		JSONArray jsArray = new JSONArray();
		try {
			jsArray = tmenulinksService.getByDataUsing(data);
			resp.put("status", 200);
			resp.put("result", jsArray);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());

	}
	@PostMapping("/getAllFormList")
	public ResponseEntity<?> getAllFormList(@RequestBody String formParams) {
		JSONObject jsonObject = tmenulinksService.getAllFormList();
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(jsonObject.toString()).toString());
	}
}
