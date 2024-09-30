package app.ewarehouse.controller;

import app.ewarehouse.service.TuserService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.DocumentUploadutil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import app.ewarehouse.util.TuserValidationCheck;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import org.json.JSONArray;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Base64;
@RestController
@CrossOrigin("*")
@RequestMapping("/adminconsole")
public class TuserController {
	@Autowired
	private TuserService tuserService;
	String path = "src/storage/manage-users/";
	String data = "";
	JSONObject resp = new JSONObject();
	@PostMapping("/manage-users/addEdit")
	public ResponseEntity<?> create(@RequestBody String tuser) throws JsonMappingException, JsonProcessingException {
		data = CommonUtil.inputStreamDecoder(tuser);
		if (TuserValidationCheck.BackendValidation(new JSONObject(data)) != null) {
			resp.put("status", 502);
			resp.put("errMsg", TuserValidationCheck.BackendValidation(new JSONObject(data)));
		} else {
			resp = tuserService.save(data);
		}
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
		       
	}

	@PostMapping("/manage-users/preview")
	public ResponseEntity<?> getById(@RequestBody String formParams) {
		data = CommonUtil.inputStreamDecoder(formParams);
		JSONObject json = new JSONObject(data);
		JSONObject entity = tuserService.getById(json.getInt("intId"));
		resp.put("status", 200);
		resp.put("result", entity);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/manage-users/all")
	public ResponseEntity<?> getAll(@RequestBody String formParams) {
		JSONArray entity = tuserService.getAll(CommonUtil.inputStreamDecoder(formParams));
		resp.put("status", 200);
		resp.put("result", entity);
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(resp.toString()).toString());
	}

	@PostMapping("/manage-users/delete")
	public ResponseEntity<?> delete(@RequestBody String formParams) {
		data = CommonUtil.inputStreamDecoder(formParams);
		JSONObject json = new JSONObject(data);
		JSONObject entity = tuserService.deleteById(json.getInt("intId"));
		return ResponseEntity.ok(CommonUtil.inputStreamEncoder(entity.toString()).toString());
	}
	
	@PostMapping("/manage-users/userList")
	public ResponseEntity<?> userList(@RequestBody String formParams) {
		data = CommonUtil.inputStreamDecoder(formParams);
		
		JSONArray entity = tuserService.findUserList(data);
		resp.put("status", 200);
		resp.put("result", entity);
		
		return ResponseEntity.ok(resp.toString());
	}

	@GetMapping("/manage-users/download/{name}")
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
	

	   @GetMapping("/getfileurl")
	   public ResponseEntity<String> uploadFile() {
		   String msg="/9j/4AAQSkZJRgABAQEAYABgAAD//gATQ3JlYXRlZCB3aXRoIEdJTVD/4gKwSUNDX1BST0ZJTEUAAQEAAAKgbGNtcwQwAABtbnRyUkdCIFhZWiAH5AABAAEADwAZAABhY3NwQVBQTAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9tYAAQAAAADTLWxjbXMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA1kZXNjAAABIAAAAEBjcHJ0AAABYAAAADZ3dHB0AAABmAAAABRjaGFkAAABrAAAACxyWFlaAAAB2AAAABRiWFlaAAAB7AAAABRnWFlaAAACAAAAABRyVFJDAAACFAAAACBnVFJDAAACFAAAACBiVFJDAAACFAAAACBjaHJtAAACNAAAACRkbW5kAAACWAAAACRkbWRkAAACfAAAACRtbHVjAAAAAAAAAAEAAAAMZW5VUwAAACQAAAAcAEcASQBNAFAAIABiAHUAaQBsAHQALQBpAG4AIABzAFIARwBCbWx1YwAAAAAAAAABAAAADGVuVVMAAAAaAAAAHABQAHUAYgBsAGkAYwAgAEQAbwBtAGEAaQBuAABYWVogAAAAAAAA9tYAAQAAAADTLXNmMzIAAAAAAAEMQgAABd7///MlAAAHkwAA/ZD///uh///9ogAAA9wAAMBuWFlaIAAAAAAAAG+gAAA49QAAA5BYWVogAAAAAAAAJJ8AAA+EAAC2xFhZWiAAAAAAAABilwAAt4cAABjZcGFyYQAAAAAAAwAAAAJmZgAA8qcAAA1ZAAAT0AAACltjaHJtAAAAAAADAAAAAKPXAABUfAAATM0AAJmaAAAmZwAAD1xtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAAgAAAAcAEcASQBNAFBtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAAgAAAAcAHMAUgBHAEL/2wBDAAMCAgMCAgMDAwMEAwMEBQgFBQQEBQoHBwYIDAoMDAsKCwsNDhIQDQ4RDgsLEBYQERMUFRUVDA8XGBYUGBIUFRT/2wBDAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wgARCAAFAAUDAREAAhEBAxEB/8QAFAABAAAAAAAAAAAAAAAAAAAAB//EABUBAQEAAAAAAAAAAAAAAAAAAAYH/9oADAMBAAIQAxAAAAFWQQT/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAEFAn//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAEDAQE/AX//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAECAQE/AX//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAY/An//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAE/IX//2gAMAwEAAgADAAAAEL//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAEDAQE/EH//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAECAQE/EH//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAE/EH//2Q==";
		   byte[] decode = Base64.getDecoder().decode(msg.getBytes());
		   
		   String file_url=new DocumentUploadutil().uploadFileByte("test", decode, FolderAndDirectoryConstant.INSP_SUSPENSION_FOLDER);
		   
	      return ResponseEntity.status(HttpStatus.CREATED).body(file_url);
	  } 
	   
}
