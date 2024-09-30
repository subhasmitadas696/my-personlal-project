package app.ewarehouse.service;

import java.util.Date;
import java.util.List;

import org.json.JSONObject;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import com.fasterxml.jackson.core.JsonProcessingException;

import app.ewarehouse.dto.ApplicationConformityMainRemarksDto;
import app.ewarehouse.dto.ApplicationOfConformityDTO;
import app.ewarehouse.dto.ApprovedAocIdAndShopDto;
import app.ewarehouse.dto.ConformityCertificateDto;
import app.ewarehouse.entity.ApplicationOfConformity;
import app.ewarehouse.entity.ApplicationOfConformityParticularOfApplicants;
import app.ewarehouse.entity.ApplicationOfConformitySupportingDocuments;
import app.ewarehouse.entity.ApplicationOfConformityWarehouseOperatorViability;
import app.ewarehouse.entity.Status;

public interface ConformityParticularService {

	JSONObject saveApplicantData(String data) throws JsonProcessingException;

	JSONObject saveSupportingDocsData(String data) throws JsonProcessingException;

	JSONObject saveOperatorViabilityData(String data) throws JsonProcessingException;

	JSONObject savePaymentData(String data) throws JsonProcessingException;

	List<ApplicationOfConformityDTO> findAll();

	void updateApplicationStatus(String data);

	Page<ApplicationOfConformity> getApplicationByStatusAndRole(Date fromDate, Date toDate, Status status, Integer pendingAt, Pageable pageable);

	ApplicationOfConformity findByApplicationIdWithDirectors(String applicationId);

	void updateApplicationStatus(Status status, Integer role, String appId);

	ApplicationOfConformity findById(String applicationId);

	void giveRemarks(String data) throws JsonProcessingException;

	Long getCountByCreatedByAndDraftStatus(Integer intId);

	ApplicationOfConformityParticularOfApplicants getAocParticularDataById(Integer intId);

	void deleteDirectorById(Integer intId);

	Long getDraftStatusOfSupportingDocs(Integer intId);

	ApplicationOfConformitySupportingDocuments getAocSupportindDocDataById(Integer intId);

	Long getDraftStatusOfViability(Integer intId);

	ApplicationOfConformityWarehouseOperatorViability getViabilityDataById(Integer intId);

	ApplicationOfConformityDTO findByUserIdAndStatus(Integer userId);

	Page<ApplicationOfConformity> getApplicationByUserId(Date fromDate, Date toDate, Integer userId, Pageable pageable);

    String getCommodityTypes(String id);
    
    List<ApprovedAocIdAndShopDto> getApprovedApplicationIdAndShop(Integer countyId, Integer subCountyId);

	String getOperatorFullName(String applicantId);

	ConformityCertificateDto getCertificate(String applicantId);

	List<ApplicationConformityMainRemarksDto> getRemarks(String applicantId);

//	  JSONObject findByApplicantId(String ApplicantId)throws JsonMappingException, JsonProcessingException;

}
