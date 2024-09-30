/**
 * 
 */
package app.ewarehouse.controller;

import app.ewarehouse.dto.DepositorResponse;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.entity.Depositor;
import app.ewarehouse.entity.Status;
import app.ewarehouse.service.DepositorService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.format.annotation.DateTimeFormat;
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
import java.util.Date;
import java.util.List;

/**
 * Priyanka Singh
 */
@RestController
@CrossOrigin("*")
@RequestMapping("/admin/depositor")
public class DepositorController {
	
	private static final Logger logger = LoggerFactory.getLogger(DepositorController.class);
    String path = "src/resources/"+ FolderAndDirectoryConstant.DEPOSITOR_REG_FOLDER +"/";

    @Autowired
    private DepositorService depositorService;

    @Autowired
    private ObjectMapper objectMapper;

    @PostMapping("/addEdit")
    public ResponseEntity<String> addEdit(@RequestBody String depositor) throws JsonProcessingException {
        logger.info("Inside createOrUpdatedepositor method of DepositorController");
        String response = depositorService.save(depositor);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(response)).toString());
    }
    

    @GetMapping("/get/{id}")
    public ResponseEntity<String> getById(@PathVariable("id") String id) throws JsonProcessingException {
    	System.out.println(id);
    	logger.info("Inside getById method of DepositorController");
        DepositorResponse depositor = depositorService.getById(id);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(depositor)).toString());
    }

    @GetMapping("/getAllDepositor")
    public ResponseEntity<List<Depositor>> getAll() {
        List<Depositor> depositorList = depositorService.getAll();
        return new ResponseEntity<>(depositorList, HttpStatus.OK);
    }

    @DeleteMapping("/delete/{id}")
    public ResponseEntity<String> delete(@PathVariable("id") String id) throws JsonProcessingException {
        logger.info("Inside delete method of DepoController");
        String response = depositorService.deleteById(id);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(response)).toString());
    }

    @GetMapping("/paginated")
    public ResponseEntity<String> getAllPaginated(
            Pageable pageable,
            @RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date fromDate,
            @RequestParam(required = false) @DateTimeFormat(pattern = "yyyy-MM-dd") Date toDate,
            @RequestParam(required = false) Status status) throws JsonProcessingException {

        Page<DepositorResponse> depositorPage = depositorService.getFilteredDepositors(fromDate, toDate, status, pageable);
        return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(depositorPage)).toString());
    }
    private <T> String buildJsonResponse(T response) throws JsonProcessingException {
        return objectMapper.writeValueAsString(
                ResponseDTO.<T>builder()
                        .status(200)
                        .result(response)
                        .build()
        );
    }
    
    @GetMapping("/download")
    public ResponseEntity<ByteArrayResource> download(@RequestParam("name") String name) throws IOException {
        logger.info("Inside download method of DepositorController");
        Path filePath = Paths.get(path, name);
        File file = filePath.toFile();
        logger.info("The file is: " + file.getAbsolutePath());
        if (!file.exists() || file.isDirectory()) {
            throw new IOException("File not found: " + file.getAbsolutePath());
        }
        ByteArrayResource byteArrayResource = new ByteArrayResource(Files.readAllBytes(filePath));

        return ResponseEntity.ok()
                .headers(headers(file.getName()))
                .contentLength(file.length())
                .contentType(MediaType.parseMediaType("application/octet-stream"))
                .body(byteArrayResource);
    }
    
    private HttpHeaders headers(String name) {
        HttpHeaders header = new HttpHeaders();
        header.add(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=" + name);
        header.add("Cache-Control", "no-cache, no-store, must-revalidate");
        header.add("Pragma", "no-cache");
        header.add("Expires", "0");
        return header;
    }
    
    
    


}
