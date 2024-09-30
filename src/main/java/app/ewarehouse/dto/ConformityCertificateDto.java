package app.ewarehouse.dto;

import java.time.LocalDate;

import lombok.Data;

@Data
public class ConformityCertificateDto {

	private String certificateId;
	
	private String applicationId;
	
	private String applicantName;
	
	private LocalDate dateOfIssue;
	
	private LocalDate validFrom;
	
	private LocalDate validTo;
}
