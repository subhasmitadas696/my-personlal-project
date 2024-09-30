package app.ewarehouse.serviceImpl;

import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Base64;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;

import app.ewarehouse.dto.AocCompProfDetailsMainDTO;
import app.ewarehouse.dto.AocCompProfDirectorDetDTO;
import app.ewarehouse.dto.AocCompProfileDetDTO;
import app.ewarehouse.dto.AocRemarksDto;
import app.ewarehouse.dto.ApplicationConformityMainRemarksDto;
import app.ewarehouse.dto.ApplicationOfConformityDTO;
import app.ewarehouse.dto.ApplicationOfConformityDirectorDetailsDTO;
import app.ewarehouse.dto.ApplicationOfConformityDirectorResponseDTO;
import app.ewarehouse.dto.ApplicationOfConformityParticularOfApplicantsDTO;
import app.ewarehouse.dto.ApplicationOfConformityParticularOfApplicantsMapper;
import app.ewarehouse.dto.ApplicationOfConformityPaymentDetailsDTO;
import app.ewarehouse.dto.ApplicationOfConformitySupportingDocumentsDTO;
import app.ewarehouse.dto.ApplicationOfConformityWarehouseOperatorViabilityDTO;
import app.ewarehouse.dto.ApprovedAocIdAndShopDto;
import app.ewarehouse.dto.ConformityCertificateDto;
import app.ewarehouse.dto.UpdatedStatusRequest;
import app.ewarehouse.entity.AocCompProfDirectorDetails;
import app.ewarehouse.entity.AocLevels;
import app.ewarehouse.entity.AocRemarksStatus;
import app.ewarehouse.entity.ApplicationOfConformity;
import app.ewarehouse.entity.ApplicationOfConformityCertificate;
import app.ewarehouse.entity.ApplicationOfConformityDirectorDetails;
import app.ewarehouse.entity.ApplicationOfConformityMainRemarks;
import app.ewarehouse.entity.ApplicationOfConformityParticularOfApplicants;
import app.ewarehouse.entity.ApplicationOfConformityPaymentDetails;
import app.ewarehouse.entity.ApplicationOfConformitySupportingDocuments;
import app.ewarehouse.entity.ApplicationOfConformityWarehouseOperatorViability;
import app.ewarehouse.entity.Mrole;
import app.ewarehouse.entity.Nationality;
import app.ewarehouse.entity.PaymentStatus;
import app.ewarehouse.entity.Status;
import app.ewarehouse.entity.Tuser;
import app.ewarehouse.entity.WarehouseStatus;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.AocCompProfDirectorDetRepository;
import app.ewarehouse.repository.ApplicationOfConformityCertificateRepository;
import app.ewarehouse.repository.ApplicationOfConformityCommodityTypeRepository;
import app.ewarehouse.repository.ApplicationOfConformityDirectorDetailsRepository;
import app.ewarehouse.repository.ApplicationOfConformityMainRemarksRepository;
import app.ewarehouse.repository.ApplicationOfConformityParticularOfApplicantsRepository;
import app.ewarehouse.repository.ApplicationOfConformityPaymentDetailsRepository;
import app.ewarehouse.repository.ApplicationOfConformityRepository;
import app.ewarehouse.repository.ApplicationOfConformitySupportingDocumentsRepository;
import app.ewarehouse.repository.ApplicationOfConformityWarehouseOperatorViabilityRepository;
import app.ewarehouse.repository.NationalityMasterRepository;
import app.ewarehouse.service.ConformityParticularService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.DocumentUploadutil;
import app.ewarehouse.util.ErrorMessages;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import app.ewarehouse.util.Mapper;
import app.ewarehouse.util.RoleIdConstants;
import jakarta.persistence.EntityNotFoundException;
import jakarta.persistence.Tuple;

@Service
public class ConformityParticularServiceImpl implements ConformityParticularService {

	private static final String STATUS = "status";
	private static final String AN_UNEXPECTED_ERROR_OCCURRED = "An unexpected error occurred: ";
	private static final String ERROR = "error";

	private static final Logger logger = LoggerFactory.getLogger(ConformityParticularServiceImpl.class);

	private final ApplicationOfConformityParticularOfApplicantsRepository conformityParticularRepository;
	private final ApplicationOfConformityDirectorDetailsRepository directorRepository;

	private final AocCompProfDirectorDetRepository compDirectorRepository;
	private final ApplicationOfConformitySupportingDocumentsRepository supportingDocRepository;
	private final ApplicationOfConformityWarehouseOperatorViabilityRepository operatorViabilityRepository;
	private final ApplicationOfConformityPaymentDetailsRepository paymentRepository;
	private final ApplicationOfConformityRepository aocRepository;
	private final ApplicationOfConformityCommodityTypeRepository aocCommodityRepo;
	private final ApplicationOfConformityCertificateRepository certificateRepo;
	private final ApplicationOfConformityMainRemarksRepository remarksRepo;
	private final JdbcTemplate jdbcTemplate;

	private final ErrorMessages errorMessages;

	public ConformityParticularServiceImpl(
			ApplicationOfConformityParticularOfApplicantsRepository conformityParticularRepository,
			ApplicationOfConformityDirectorDetailsRepository directorRepository,
			ApplicationOfConformitySupportingDocumentsRepository supportingDocRepository,
			ApplicationOfConformityWarehouseOperatorViabilityRepository operatorViabilityRepository,
			ApplicationOfConformityPaymentDetailsRepository paymentRepository,
			ApplicationOfConformityCommodityTypeRepository aocCommodityRepo,
			ApplicationOfConformityRepository aocRepository, JdbcTemplate jdbcTemplate,
			ApplicationOfConformityCertificateRepository certificateRepo,
			ApplicationOfConformityMainRemarksRepository remarksRepo, ErrorMessages errorMessages,
			AocCompProfDirectorDetRepository compDirectorRepository) {
		this.conformityParticularRepository = conformityParticularRepository;
		this.directorRepository = directorRepository;
		this.supportingDocRepository = supportingDocRepository;
		this.operatorViabilityRepository = operatorViabilityRepository;
		this.paymentRepository = paymentRepository;
		this.aocRepository = aocRepository;
		this.aocCommodityRepo = aocCommodityRepo;
		this.jdbcTemplate = jdbcTemplate;
		this.errorMessages = errorMessages;
		this.compDirectorRepository = compDirectorRepository;
		this.certificateRepo = certificateRepo;
		this.remarksRepo = remarksRepo;
	}

	ApplicationOfConformity aoc = new ApplicationOfConformity();

	@Autowired
	private NationalityMasterRepository nationalRepo;

	@Override
	public Long getCountByCreatedByAndDraftStatus(Integer intId) {
		return conformityParticularRepository.countByCreatedByAndDraftStatus(intId);
	}

	@Override
	public ApplicationOfConformityParticularOfApplicants getAocParticularDataById(Integer intId) {
		return conformityParticularRepository.findById(intId).orElse(null);
	}

	@Override
	public void deleteDirectorById(Integer intId) {
		directorRepository.deleteById(intId);
	}

	@Override
	@Transactional
	public JSONObject saveApplicantData(String data) throws JsonProcessingException {
		JSONObject json = new JSONObject();
		ObjectMapper om = new ObjectMapper();
		om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		try {
			// Convert JSON string to DTO
			ApplicationOfConformityParticularOfApplicantsDTO apDto = om.readValue(data,
					ApplicationOfConformityParticularOfApplicantsDTO.class);
			// Map DTO to entity
			ApplicationOfConformityParticularOfApplicants apEntity = ApplicationOfConformityParticularOfApplicantsMapper
					.toEntity(apDto);
			// Save entity
			ApplicationOfConformityParticularOfApplicants savedEntity = conformityParticularRepository.save(apEntity);
			Integer id = savedEntity.getId();
//			aoc.setParticularOfApplicantsId(savedEntity);
//			Integer applicantId = savedEntity.getId();
			// Process Commodity type
//			List<AocTypeOfCommodityDto> commodityTypeList = apDto.getTypeOfCommodities();
//			for(AocTypeOfCommodityDto ct : commodityTypeList) {
//				ct.setParticularOfApplicantsId(id);
//				ApplicationOfConformityCommodityType ctEntity = ApplicationOfConformityParticularOfApplicantsMapper.toEntityCommodityType(ct);
//				aocCommodityRepo.save(ctEntity);
//			}

			// Process director list
			List<ApplicationOfConformityDirectorDetailsDTO> directorList = apDto.getDirectors();
			for (ApplicationOfConformityDirectorDetailsDTO di : directorList) {
				di.setParticularOfApplicantsId(id);

				// Map DTO to entity
				ApplicationOfConformityDirectorDetails apEntityDirector = ApplicationOfConformityParticularOfApplicantsMapper
						.toEntityDirector(di);
				// Save director entity
				directorRepository.save(apEntityDirector);
			}
			json.put(STATUS, 200);
			json.put("intAocParticularId", savedEntity.getId());
		} catch (Exception e) {
			e.printStackTrace();
			json.put(ERROR, AN_UNEXPECTED_ERROR_OCCURRED + e.getMessage());
			json.put(STATUS, 500);
		}
		return json;
	}

	@Override
	public Long getDraftStatusOfSupportingDocs(Integer intId) {
		return supportingDocRepository.getDraftStatusOfSupportingDocs(intId);
	}

	@Override
	public ApplicationOfConformitySupportingDocuments getAocSupportindDocDataById(Integer intId) {
		return supportingDocRepository.findById(intId).orElse(null);
	}

	@Override
	public JSONObject saveSupportingDocsData(String data) throws JsonProcessingException {
		JSONObject json = new JSONObject();
		ObjectMapper om = new ObjectMapper();
		om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		try {
			ApplicationOfConformitySupportingDocumentsDTO supportingDocDto = om.readValue(data,
					ApplicationOfConformitySupportingDocumentsDTO.class);
			ApplicationOfConformitySupportingDocuments supportingDoc = Mapper.toEntity(supportingDocDto);
			ApplicationOfConformitySupportingDocuments savedEntity = supportingDocRepository.save(supportingDoc);
			// supportingDocId = savedEntity.getId();
			// saving on main table
			// aoc.setSupportingDocumentId(supportingDoc);
			json.put(STATUS, 200);
		} catch (Exception e) {
			e.printStackTrace();
			json.put(ERROR, AN_UNEXPECTED_ERROR_OCCURRED + e.getMessage());
			json.put(STATUS, 500);
		}
		return json;
	}

	@Override
	public Long getDraftStatusOfViability(Integer intId) {
		return operatorViabilityRepository.getDraftStatusOfViability(intId);
	}

	@Override
	public ApplicationOfConformityWarehouseOperatorViability getViabilityDataById(Integer intId) {
		return operatorViabilityRepository.findById(intId).orElse(null);
	}

	@Override
	public JSONObject saveOperatorViabilityData(String data) throws JsonMappingException, JsonProcessingException {
		JSONObject json = new JSONObject();
		ObjectMapper om = new ObjectMapper();
		om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		try {
			ApplicationOfConformityWarehouseOperatorViabilityDTO operatorViabilityDto = om.readValue(data,
					ApplicationOfConformityWarehouseOperatorViabilityDTO.class);
			ApplicationOfConformityWarehouseOperatorViability operatorViability = Mapper.toEntity(operatorViabilityDto);
			ApplicationOfConformityWarehouseOperatorViability savedEntity = operatorViabilityRepository
					.save(operatorViability);
			json.put(STATUS, 200);
		} catch (Exception e) {
			e.printStackTrace();
			json.put(ERROR, AN_UNEXPECTED_ERROR_OCCURRED + e.getMessage());
			json.put(STATUS, 500);
		}
		return json;
	}

	@Transactional
	@Override
	public JSONObject savePaymentData(String data) throws JsonProcessingException {
		JSONObject json = new JSONObject();
		ObjectMapper om = new ObjectMapper();
		om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		try {
			ApplicationOfConformityPaymentDetailsDTO paymentDto = om.readValue(data,
					ApplicationOfConformityPaymentDetailsDTO.class);
			ApplicationOfConformityPaymentDetails paymentData = Mapper.toEntity(paymentDto);
			ApplicationOfConformityParticularOfApplicants particularOfApplicants = conformityParticularRepository
					.findById(paymentDto.getIntParticularOfApplicantsId())
					.orElseThrow(() -> new EntityNotFoundException("Particular Of Applicants not found"));
			paymentData.setParticularOfApplicants(particularOfApplicants);
			paymentData.setIntCreatedBy(paymentDto.getUserId());
			// Payment Integration will start from here
			// for this time i am setting payment status as SUCCESS
			paymentData.setEnmPaymentStatus(PaymentStatus.SUCCESS);
			ApplicationOfConformityPaymentDetails savedEntity = paymentRepository.save(paymentData);
			if (savedEntity.getEnmPaymentStatus() == PaymentStatus.SUCCESS) {
				// call the procedure
				String result = (String) saveDataInAocMainTable(savedEntity.getIntCreatedBy());
				if (result.equals("You have not completed the application")
						|| result.equals("You have not started the application.")) {
					json.put(STATUS, 400);
					json.put("id", result);
				} else {
					json.put(STATUS, 200);
					json.put("id", result);
				}
			} else {
				// payment failed.
				json.put(STATUS, 403);
				json.put("id", "Payment Failed");
			}

		} catch (Exception e) {
			e.printStackTrace();
			json.put(ERROR, AN_UNEXPECTED_ERROR_OCCURRED + e.getMessage());
			json.put(STATUS, 500);
		}
		return json;
	}

	private Object saveDataInAocMainTable(Integer intCreatedBy) {

		SimpleJdbcCall simpleJdbcCall = new SimpleJdbcCall(jdbcTemplate).withProcedureName("confirmity_submition");
		Map<String, Object> inParams = new HashMap<>();
		inParams.put("USER_ID", intCreatedBy);
		Map<String, Object> outParams = simpleJdbcCall.execute(inParams);
		return outParams.get("RESULT_MSG");
	}

	@Override
	public List<ApplicationOfConformityDTO> findAll() {
		// List<ApplicationOfConformityDTO> entitiesDto = new ArrayList<>();
		logger.info("inside get all applicationofConformityDTo");
//		try {
//			 List<ApplicationOfConformity> entities = aocRepository.findAll();
//		     System.out.println("inside service"+entities);
//		     entitiesDto = entities.stream().map(ApplicationOfConformityMapper::toDto).collect(Collectors.toList());
//			
//		} catch (StackOverflowError e) {
//			logger.error(AN_UNEXPECTED_ERROR_OCCURRED+e);
//		}
		return null;
	}

	@Override
	public void updateApplicationStatus(String data) {
		String decodedData = CommonUtil.inputStreamDecoder(data);
		UpdatedStatusRequest upStatus;
		try {
			upStatus = new ObjectMapper().readValue(decodedData, UpdatedStatusRequest.class);
			ApplicationOfConformity application = aocRepository.findById(upStatus.getApplicationId())
					.orElseThrow(() -> new RuntimeException("Application not found"));
			application.setEnmStatus(upStatus.getEnmStatus());

//			application.setRole(upStatus.ge);
			aocRepository.save(application);
		} catch (JsonProcessingException e) {
			logger.error("Failed to process JSON data: {} in ConformityParticularService", e.getMessage());
			throw new RuntimeException("Invalid data format", e);
		}
	}

	@Override
	public void updateApplicationStatus(Status status, Integer role, String appId) {
		ApplicationOfConformity application = aocRepository.findById(appId)
				.orElseThrow(() -> new RuntimeException("Application not found"));

		if (status == Status.Rejected) {
			application.setEnmStatus(status);
		} else {
			application.setRole(role + 1);
		}
		aocRepository.save(application);
	}

	@Override
	public Page<ApplicationOfConformity> getApplicationByStatusAndRole(Date fromDate, Date toDate, Status status, Integer pendingAt ,
			Pageable pageable) {
		logger.info("inside getApplicationByStatusAndRole");
		Page<ApplicationOfConformity> applicationsPage = aocRepository.findByFilters(toDate, toDate, status, pendingAt ,pageable);
		return new PageImpl<>(applicationsPage.getContent(), pageable, applicationsPage.getTotalElements());
	}

	@Override
	@Transactional
	public ApplicationOfConformity findByApplicationIdWithDirectors(String applicationId) {
		logger.info("inside find by id ApplicationOFConformity");
		ApplicationOfConformity application = aocRepository.findByApplicationIdWithDirectors(applicationId)
				.orElse(null);

		if (application != null) {

			ApplicationOfConformityParticularOfApplicants particularOfApplicants = application
					.getParticularOfApplicantsId();
			Integer particularsId = particularOfApplicants.getId();

			Set<ApplicationOfConformityDirectorResponseDTO> directorsdto = directorRepository
					.findDirectorDetailsByParticularOfApplicantsId(particularsId);

			final Set<ApplicationOfConformityDirectorDetails> directors = directorsdto.stream().map(dto -> {
				ApplicationOfConformityDirectorDetails entity = dto.toEntity();
				// entity.setParticularOfApplicants(particularOfApplicants);
				try {
					Nationality nationality = nationalRepo.findById(dto.getNationalityId()).orElseThrow(
							() -> new RuntimeException("Nationality not found with ID: " + dto.getNationalityId()));
					entity.setNationality(nationality);
					// entity = entityManager.merge(entity);
				} catch (Exception e) {
					logger.error("Error setting  Nationality for director!" + dto.getDirectorName());
					e.printStackTrace();
				}
				entity.setParticularOfApplicants(null);
				return entity;
			}).collect(Collectors.toSet());

			particularOfApplicants.getDirectors().addAll(directors);
		}
		return application;
	}

	@Override
	public ApplicationOfConformity findById(String applicationId) {
		return aocRepository.findById(applicationId).orElse(null);
	}

	@Override
	@Transactional
	public void giveRemarks(String data) {
		try {
			// Decode and parse the input data
			String decodedData = CommonUtil.inputStreamDecoder(data);
			ObjectMapper objectMapper = new ObjectMapper();
			objectMapper.registerModule(new JavaTimeModule());
			AocRemarksDto dto = objectMapper.readValue(decodedData, AocRemarksDto.class);
			ApplicationOfConformity aocData = aocRepository.findById(dto.getIntApplicantId())
					.orElseThrow(() -> new EntityNotFoundException(
							"ApplicationOfConformity not found with ID: " + dto.getIntApplicantId()));
			ApplicationOfConformityMainRemarks remarks = new ApplicationOfConformityMainRemarks();
			switch (dto.getRoleId()) {
			case RoleIdConstants.OIC:
				aocData.setVchOicRemarks(dto.getTxtRemark());
				aocData.setOicRoleId(dto.getRoleId());
				aocData.setOicUserId(dto.getUserId());
				if (dto.getEnmStatus() != null && "Deferred".equals(dto.getEnmStatus())) {
					aocData.setEnmStatus(Status.Deferred);
					remarks.setApplicationStatus(AocRemarksStatus.DEFERRED);
					aocData.setCurrentLevel(AocLevels.OIC);
				} else {
					aocData.setEnmStatus(Status.Pending);
					if(Boolean.TRUE.equals(aocData.getIsRequiredInspection())) {
						aocData.setCurrentLevel(AocLevels.INSPECTOR);
						aocData.setPendingAt(RoleIdConstants.INSPECTOR);
						remarks.setApplicationStatus(AocRemarksStatus.FORWARDED_TO_INSPECTOR);
					}else {
						aocData.setCurrentLevel(AocLevels.APPROVER);
						aocData.setPendingAt(RoleIdConstants.APPROVER);
						remarks.setApplicationStatus(AocRemarksStatus.FORWARDED_TO_APPROVER);
					}
				}
				break;
			case RoleIdConstants.INSPECTOR:
				handleInspectorFormSubmission(dto, aocData , remarks);
				break;
			case RoleIdConstants.OIC_FINANCE:
				aocData.setVchOicFinRemarks(dto.getTxtRemark());
				aocData.setOicFinRoleId(dto.getRoleId());
				aocData.setOicFinUserId(dto.getUserId());
				break;
			case RoleIdConstants.APPROVER:
				aocData.setVchApproverRemarks(dto.getTxtRemark());
				aocData.setTime(dto.getTimeOfInspect());
				aocData.setDate(dto.getDateOfInspect());
				byte[] decodedMomFile = Base64.getDecoder().decode(dto.getTxtInspectorFilePath());
				String momFilePath = DocumentUploadutil.uploadFileByte("AOC_MOM_UPLOAD_" + System.currentTimeMillis(),
						decodedMomFile, FolderAndDirectoryConstant.AOC_MOM_UPLOAD_FOLDER);
				aocData.setMomFilePath(momFilePath);
				aocData.setApproverRoleId(dto.getRoleId());
				aocData.setApproverUserId(dto.getUserId());
				aocData.setCurrentLevel(AocLevels.CEO);
				aocData.setPendingAt(RoleIdConstants.CEO);
				remarks.setApplicationStatus(AocRemarksStatus.FORWARDED_TO_CEO);
				break;
			case RoleIdConstants.CEO:
				aocData.setVchCeoRemarks(dto.getTxtRemark());
				aocData.setCeoRoleId(dto.getRoleId());
				aocData.setCeoUserId(dto.getUserId());
				aocData.setEnmStatus("Accepted".equals(dto.getEnmStatus()) ? Status.Accepted : Status.Rejected);
				remarks.setApplicationStatus("Accepted".equals(dto.getEnmStatus()) ? AocRemarksStatus.ACCEPTED : AocRemarksStatus.REJECTED);
				aocData.setDtDateOfIssue("Accepted".equals(dto.getEnmStatus()) ? LocalDate.now() : null);
				aocData.setPendingAt(null);
				break;
			default:
				logger.warn("Invalid username: {}", dto.getUsername());
				throw new IllegalArgumentException("Invalid username: " + dto.getUsername());
			}
			ApplicationOfConformity savedAocData = aocRepository.save(aocData);
			if (!(dto.getRoleId() == RoleIdConstants.INSPECTOR && (dto.getDateOfInspect() != null
					&& dto.getTimeOfInspect() != null && dto.getTxtDescriptionOfInspection() != null))) {
				
				remarks.setApplicationId(savedAocData);
				remarks.setRemarks(dto.getTxtRemark());
				remarks.setDateOfRemarks(LocalDate.now());
				Tuser user = new Tuser();
				user.setIntId(dto.getUserId());
				remarks.setUser(user);
				Mrole role = new Mrole();
				role.setIntId(dto.getRoleId());;
				remarks.setRole(role);
				//remarks.setApplicationStatus(savedAocData.getEnmStatus());
				remarks.setDeletedFlag(false);
				remarksRepo.save(remarks);
			}
			if (savedAocData.getEnmStatus() == Status.Accepted) {
				ApplicationOfConformityCertificate certificate = new ApplicationOfConformityCertificate();
				certificate.setApplication(savedAocData);
				certificate.setCompany(savedAocData.getProfDet());
				certificate.setCertificateId(getCertificateId("COC"));
				certificate.setUserId(savedAocData.getIntCreatedBy());
				certificate.setValidFrom(savedAocData.getDtDateOfIssue());
				certificate.setValidTo(savedAocData.getDtDateOfIssue().plusYears(1));
				certificate.setEnmPaymentStatus(PaymentStatus.SUCCESS);
				certificate.setEnmWarehouseStatus(WarehouseStatus.Active);
				certificate.setBitDeletedFlag(false);
				certificateRepo.save(certificate);
			}
		} catch (IOException e) {
			logger.error("Failed to process input data", e);
			throw new RuntimeException("Failed to process input data", e);
		} catch (Exception e) {
			logger.error("An unexpected error occurred", e);
			throw new RuntimeException("An unexpected error occurred", e);
		}
	}

	private String getCertificateId(String id) {
		String dbCurrentId = certificateRepo.getId();
		if (dbCurrentId == null) {
			return id + "100000";
		} else {
			Integer idNum = Integer.parseInt(dbCurrentId.substring(3, dbCurrentId.length()));
			AtomicInteger seq = new AtomicInteger(idNum);
			int nextVal = seq.incrementAndGet();
			return id + nextVal;
		}
	}

	private void handleInspectorFormSubmission(AocRemarksDto dto, ApplicationOfConformity aocData, ApplicationOfConformityMainRemarks remarks) {
		// Handle the first form (dateOfInspect, timeOfInspect,
		// txtDescriptionOfInspection)
		if (dto.getDateOfInspect() != null && dto.getTimeOfInspect() != null
				&& dto.getTxtDescriptionOfInspection() != null) {
			aocData.setDtDateOfInspection(dto.getDateOfInspect());
			aocData.setTmTimeOfInspection(dto.getTimeOfInspect());
			aocData.setVchDescriptionOfInspection(dto.getTxtDescriptionOfInspection());
		}

		// Handle the second form (txtInspectorFilePath, txtRemark)
		if (dto.getTxtInspectorFilePath() != null) {
			byte[] decodedFile = Base64.getDecoder().decode(dto.getTxtInspectorFilePath());
			String filePath = DocumentUploadutil.uploadFileByte(
					"AOC_INSPECTOR_INSPECTION_REPORT_" + System.currentTimeMillis(), decodedFile,
					FolderAndDirectoryConstant.AOC_INSPECTOR_REPORT_FOLDER);
			aocData.setVchInspectionReport(filePath);
		} else {
			aocData.setVchInspectionReport(null);
		}

		if (dto.getTxtRemark() != null && !dto.getTxtRemark().trim().isEmpty()) {
			aocData.setVchInspectorRemarks(dto.getTxtRemark());
			if(Boolean.TRUE.equals(aocData.getIsRequiredInspection())){
				aocData.setIsRequiredInspection(false);
				aocData.setCurrentLevel(AocLevels.OIC);
				aocData.setPendingAt(RoleIdConstants.OIC);
				remarks.setApplicationStatus(AocRemarksStatus.FORWARDED_TO_OIC);
			}
		} else {
			aocData.setVchInspectorRemarks(null);
		}

		// Set the inspector's user ID and role ID
		aocData.setInspectorUserId(dto.getUserId());
		aocData.setInspectorRoleId(dto.getRoleId());
	}

	// user based view of aoc
	@Override
	public Page<ApplicationOfConformity> getApplicationByUserId(Date fromDate, Date toDate, Integer userId,
			Pageable pageable) {
		logger.info("inside getApplicationByUserId");
		Page<ApplicationOfConformity> applicationsPage = aocRepository.findByUserIdFilters(toDate, toDate, userId,
				pageable);
		return new PageImpl<>(applicationsPage.getContent(), pageable, applicationsPage.getTotalElements());
	}

	@Override
	public String getCommodityTypes(String id) {
		try {
			String conformityList = aocRepository.getCommodityTypes(id, Status.Accepted, false);
			logger.info("Application of conformity commodities are:" + conformityList);
			return conformityList;
		} catch (Exception e) {
			throw new CustomGeneralException("");
		}

	}

	@Override
	public ApplicationOfConformityDTO findByUserIdAndStatus(Integer userId) {
		int count = aocRepository.countByIntCreatedByAndEnmStatus(userId, Status.Accepted.toString());

		if (count == 0) {
			throw new CustomGeneralException(errorMessages.getApprovedAocNotFound());
		}

		ApplicationOfConformity aoc = aocRepository.findByIntCreatedByAndEnmStatus(userId, Status.Accepted).get(0);
		ApplicationOfConformityDTO aocDto = new ApplicationOfConformityDTO();
		BeanUtils.copyProperties(aoc, aocDto);
		AocCompProfDetailsMainDTO companyDetailsDTO = new AocCompProfDetailsMainDTO();

		AocCompProfileDetDTO profileDTO = new AocCompProfileDetDTO();
		BeanUtils.copyProperties(aoc.getProfDet(), profileDTO);
		companyDetailsDTO.setCompanyProfile(profileDTO);
		companyDetailsDTO.setUserId(aoc.getProfDet().getCreatedBy());

		List<AocCompProfDirectorDetails> directors = compDirectorRepository.findByProfDet(aoc.getProfDet());
		List<AocCompProfDirectorDetDTO> directorDTOs = directors.stream().map(director -> {
			AocCompProfDirectorDetDTO directorDTO = new AocCompProfDirectorDetDTO();
			BeanUtils.copyProperties(director, directorDTO);
			directorDTO.setProfDetId(aoc.getProfDet().getProfDetId());
			return directorDTO;
		}).collect(Collectors.toList());

		companyDetailsDTO.setDirectors(directorDTOs);

		aocDto.setCompanyDetails(companyDetailsDTO);

		return aocDto;
	}

	@Override
	public List<ApprovedAocIdAndShopDto> getApprovedApplicationIdAndShop(Integer countyId , Integer subCountyId) {
		List<Tuple> shopList = aocRepository.getApprovedApplicationIdAndShop(countyId , subCountyId);
		List<ApprovedAocIdAndShopDto> dtoList = new ArrayList<>();
		for (Tuple tuple : shopList) {
			ApprovedAocIdAndShopDto dto = new ApprovedAocIdAndShopDto();
			dto.setApplicationId((String) tuple.get("applicationId"));
			dto.setShop((String) tuple.get("shop"));
			dtoList.add(dto);
		}
		return dtoList;
	}

	@Override
	public String getOperatorFullName(String applicantId) {
		return aocRepository.getOperatorFullName(applicantId);
	}

	@Override
	public ConformityCertificateDto getCertificate(String applicantId) {
		ApplicationOfConformityCertificate result = certificateRepo.findByApplicationId(applicantId);
		ConformityCertificateDto dto = new ConformityCertificateDto();
		dto.setApplicantName(result.getApplication().getParticularOfApplicantsId().getApplicantFullName());
		dto.setApplicationId(result.getApplication().getApplicationId());
		dto.setCertificateId(result.getCertificateId());
		dto.setDateOfIssue(result.getApplication().getDtDateOfIssue());
		dto.setValidFrom(result.getValidFrom());
		dto.setValidTo(result.getValidTo());
		return dto;
	}

	@Override
	public List<ApplicationConformityMainRemarksDto> getRemarks(String applicantId) {
		List<ApplicationOfConformityMainRemarks> remarksData = remarksRepo.findByApplicationId(applicantId);
		
		return remarksData.stream().map(entity -> {
	        ApplicationConformityMainRemarksDto dto = new ApplicationConformityMainRemarksDto();
	        
	        // Assuming getApplicantId() and getApplicantName() methods exist in ApplicationOfConformity
	        dto.setApplicantId(entity.getApplicationId().getApplicationId());
	        dto.setApplicantName(entity.getApplicationId().getParticularOfApplicantsId().getApplicantFullName());
	        dto.setRemark(entity.getRemarks());
	        dto.setDateOfRemark(entity.getDateOfRemarks());
	        dto.setUserId(entity.getUser().getIntId());
	        dto.setRoleId(entity.getRole().getIntId());
	        dto.setUserName(entity.getUser().getTxtFullName());
	        dto.setRoleName(entity.getRole().getTxtRoleName());
	        dto.setApplicationStatus(entity.getApplicationStatus());
	        
	        return dto;
	    }).collect(Collectors.toList());
	}
}
