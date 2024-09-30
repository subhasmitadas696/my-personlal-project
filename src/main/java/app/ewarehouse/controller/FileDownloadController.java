package app.ewarehouse.controller;

import app.ewarehouse.util.FileDownloadUtil;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.extern.slf4j.Slf4j;
import org.springframework.core.io.InputStreamResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;

@RestController
@CrossOrigin("*")
@RequestMapping("/file")
@Slf4j
public class FileDownloadController {
    @GetMapping("/download")
    public ResponseEntity<InputStreamResource> download(HttpServletResponse response, HttpServletRequest request, @RequestParam("filePath") String filePath) throws IOException {
        log.info("Inside download method of FileDownloadController");
        String[] parts = filePath.split("/");
        String fileName = parts[parts.length - 1];
        return FileDownloadUtil.fileDownloadUtil(fileName, filePath, response, request);
    }
}
