package app.ewarehouse.controller;

import java.io.IOException;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.InputStreamResource;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import app.ewarehouse.dto.CommonResponseModal;
import app.ewarehouse.dto.OTPRequestDTO;
import app.ewarehouse.dto.OTPValidationRequestDTO;
import app.ewarehouse.exception.ResourceNotFoundException;
import app.ewarehouse.repository.UsersRepository;
import app.ewarehouse.service.OTPService;
import app.ewarehouse.util.FileDownloadUtil;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@RestController
public class TestController {

	 Logger logger=LoggerFactory.getLogger(TestController.class);
	 @Autowired
		private UsersRepository usersRepository;
	 @Autowired(required=true)
	 OTPService otpservices;
	  @GetMapping("/getfileurl")
	   public ResponseEntity<String> uploadFile() {


		  
		  String file_url="";
		  
//			try {
//				String msg = "/9j/4AAQSkZJRgABAQEAYABgAAD//gATQ3JlYXRlZCB3aXRoIEdJTVD/4gKwSUNDX1BST0ZJTEUAAQEAAAKgbGNtcwQwAABtbnRyUkdCIFhZWiAH5AABAAEADwAZAABhY3NwQVBQTAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9tYAAQAAAADTLWxjbXMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA1kZXNjAAABIAAAAEBjcHJ0AAABYAAAADZ3dHB0AAABmAAAABRjaGFkAAABrAAAACxyWFlaAAAB2AAAABRiWFlaAAAB7AAAABRnWFlaAAACAAAAABRyVFJDAAACFAAAACBnVFJDAAACFAAAACBiVFJDAAACFAAAACBjaHJtAAACNAAAACRkbW5kAAACWAAAACRkbWRkAAACfAAAACRtbHVjAAAAAAAAAAEAAAAMZW5VUwAAACQAAAAcAEcASQBNAFAAIABiAHUAaQBsAHQALQBpAG4AIABzAFIARwBCbWx1YwAAAAAAAAABAAAADGVuVVMAAAAaAAAAHABQAHUAYgBsAGkAYwAgAEQAbwBtAGEAaQBuAABYWVogAAAAAAAA9tYAAQAAAADTLXNmMzIAAAAAAAEMQgAABd7///MlAAAHkwAA/ZD///uh///9ogAAA9wAAMBuWFlaIAAAAAAAAG+gAAA49QAAA5BYWVogAAAAAAAAJJ8AAA+EAAC2xFhZWiAAAAAAAABilwAAt4cAABjZcGFyYQAAAAAAAwAAAAJmZgAA8qcAAA1ZAAAT0AAACltjaHJtAAAAAAADAAAAAKPXAABUfAAATM0AAJmaAAAmZwAAD1xtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAAgAAAAcAEcASQBNAFBtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAAgAAAAcAHMAUgBHAEL/2wBDAAMCAgMCAgMDAwMEAwMEBQgFBQQEBQoHBwYIDAoMDAsKCwsNDhIQDQ4RDgsLEBYQERMUFRUVDA8XGBYUGBIUFRT/2wBDAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wgARCAAFAAUDAREAAhEBAxEB/8QAFAABAAAAAAAAAAAAAAAAAAAAB//EABUBAQEAAAAAAAAAAAAAAAAAAAYH/9oADAMBAAIQAxAAAAFWQQT/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAEFAn//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAEDAQE/AX//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAECAQE/AX//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAY/An//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAE/IX//2gAMAwEAAgADAAAAEL//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAEDAQE/EH//xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oACAECAQE/EH//xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oACAEBAAE/EH//2Q==";
//
//
//				byte[] decode = Base64.getDecoder().decode(pdfile.getBytes());
//
//				file_url = new DocumentUploadutil().uploadFileByte("test", decode,FolderAndDirectoryConstant.INSP_SUSPENSION_FOLDER);
//
//			} catch (Exception e) {
//
//				logger.info("this is Arthmetic exception" + e.getMessage()); // throw new
//
//				throw new ResourceNotFoundException("Anand Kumar exception");
//
//			}

			return ResponseEntity.status(HttpStatus.CREATED).body(file_url);
		}
	  
	  
	  @GetMapping("/downloadoc")
	  @ResponseBody  
	   public ResponseEntity<InputStreamResource> downloadFile(HttpServletResponse response, HttpServletRequest request) throws IOException,ResourceNotFoundException {
	  
		  
		  
		 return FileDownloadUtil.fileDownloadUtil("bond.pdf", "aoc_particular/12-07-2024/bond.pdf", response, request);
	  }
	  
	 @GetMapping("/getid") 
	 String getId()
	 {
		 
		 return usersRepository.getId();
	 }
	 
	 @PostMapping("/getotp") 
	 CommonResponseModal geOTP(@RequestBody  OTPRequestDTO requestDTO)
	 {
		 System.out.println(requestDTO);
		 return otpservices.getOTP(requestDTO);
	 }
	 
	 
	 @PostMapping("/checkotp") 
	 CommonResponseModal validateOTP(@RequestBody  OTPValidationRequestDTO requestDTO)
	 {
		 
		 return otpservices.validateOTP(requestDTO);
	 }

}