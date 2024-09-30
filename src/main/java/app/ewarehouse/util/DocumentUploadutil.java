package app.ewarehouse.util;

import lombok.extern.slf4j.Slf4j;
import org.apache.tika.Tika;
import org.imgscalr.Scalr;
import org.imgscalr.Scalr.Method;
import org.springframework.stereotype.Component;
import org.springframework.web.multipart.MultipartFile;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.*;
import java.nio.file.FileSystems;
import java.nio.file.Path;
import java.nio.file.Paths;

@Slf4j
@Component
public class DocumentUploadutil {

	
	private String uploadFile(String uniqueName, MultipartFile multipartFile, String imagePathFolder) {
		String fileUrl = null;
		String todayDate = DateTimeUtility.getTodayDate();
		String docPath = imagePathFolder + "/" + todayDate;
		Path root = FileSystems.getDefault().getPath("").toAbsolutePath();
		Path file_directory_path = Paths.get(root.toString(),"src", "main", "resources", "uploadedDocuments");
		File file = new File(file_directory_path + "/" + docPath);

		try {
			String name = multipartFile.getOriginalFilename();
			byte[] bytes = multipartFile.getBytes();
			boolean isExists = file.exists();
			file.setExecutable(true, false);
			file.setReadable(true, false);
			file.setWritable(true, false);
			if (!isExists) {
				file.mkdirs();
			}

			String extension = name.substring(name.lastIndexOf(".") + 1);
			String fileName = uniqueName + "." + extension;
			FileOutputStream fos = new FileOutputStream(file + "/" + fileName);
			fos.write(bytes);
			fos.close();
			fileUrl = file_directory_path + "/" + docPath + "/" + fileName;
		} catch (Exception e) {
			e.printStackTrace();
		}

		return fileUrl;
	}
	

	public static String uploadFileByte(String uniquefileName, byte[] multipartFileByteArr, String imagePathFolder) {
		String fileUrl = null;
		String todayDate = DateTimeUtility.getTodayDate();
		String docPath = imagePathFolder + "/" + todayDate;
		Path root = FileSystems.getDefault().getPath("").toAbsolutePath();
		Path file_directory_path = Paths.get(root.toString(),"src", "main", "resources", "uploadedDocuments");
		
		File file = new File(file_directory_path + "/" + docPath);

		try {
			
			boolean isExists = file.exists();
			file.setExecutable(true, false);
			file.setReadable(true, false);
			file.setWritable(true, false);
			if (!isExists) {
				file.mkdirs();
			}
			
			    String MimeType = new Tika().detect(multipartFileByteArr);
			    String[]extension_arr=MimeType.split("/");
			    long filelength = multipartFileByteArr.length;
			    if("application/pdf".equals(MimeType)) {
			    	
			    	if(filelength>0) {
			    		String extension = extension_arr[1];
						String fileName = uniquefileName + "." + extension;
						FileOutputStream fos = new FileOutputStream(file + "/" + fileName);
						fos.write(multipartFileByteArr);
						fos.close();
						fileUrl = docPath + "/" + fileName;
			    	}
			    	else {
			    		return "Document is not correct";
			    	}
			    }
			    else if("image/jpeg".equals(MimeType)||"image/jpg".equals(MimeType)||"image/png".equals(MimeType)) {
			    	
			    	if(filelength>0) {
				    	String extension = extension_arr[1];
				    	String fileName = uniquefileName + "." + extension;
						FileOutputStream fos = new FileOutputStream(file + "/" + fileName);
						InputStream is = new ByteArrayInputStream(multipartFileByteArr);
						       BufferedImage image = ImageIO.read(is);
						       ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
						ImageIO.write(Scalr.resize(image, Method.ULTRA_QUALITY, 1000, Scalr.OP_ANTIALIAS), extension.toUpperCase(), byteArrayOutputStream);
	
						fos.write(byteArrayOutputStream.toByteArray());
						fos.close();
						fileUrl = docPath + "/" + fileName;
			    	}
			    	else {
			    		return "File is not available";
			    	}
			    }
			    else {
			    	fileUrl= "File is not available";
			    }
			    
		} catch (Exception e) {
			log.info("Error: {}", e.getMessage());
		}

		return fileUrl;
	}
	
	
	
}
