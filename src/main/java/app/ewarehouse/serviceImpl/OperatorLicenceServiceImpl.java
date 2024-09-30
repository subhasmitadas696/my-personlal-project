package app.ewarehouse.serviceImpl;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Types;
import java.util.ArrayList;
import java.util.List;

import app.ewarehouse.entity.*;
import app.ewarehouse.repository.ApplicationOfConformityRepository;
import app.ewarehouse.repository.MFinalOperatorlicenseRepository;
import app.ewarehouse.repository.OperatorLicenceRemarkRepository;
import app.ewarehouse.util.Mapper;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.OperatorLicenceDTO;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.dto.StakeHolderActionRequestData;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.OperatorLicenceRepository;
import app.ewarehouse.repository.TuserRepository;
import app.ewarehouse.service.OperatorLicenceService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.JsonFileExtractorUtil;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Slf4j
public class OperatorLicenceServiceImpl implements OperatorLicenceService {

    private final OperatorLicenceRepository operatorLicenceRepository;

	private final MFinalOperatorlicenseRepository mFinalOperatorlicenseRepository;

    private final OperatorLicenceRemarkRepository operatorLicenceRemarkRepository;

	private final ObjectMapper objectMapper;

	private final ApplicationOfConformityRepository aocRepository;

	private final JdbcTemplate jdbcTemplate;
	
	private final TuserRepository userRepo;
    
    private OperatorLicence operatorLicence1;
	ResponseDTO<String> responseDTO = new ResponseDTO<>();

	@Transactional
    @Override
	public String handleAction(String actionRequestData, Stakeholder forwardedTo, Stakeholder actionTakenBy,
			Action action, Status status) throws JsonProcessingException {
		String decodedData = CommonUtil.inputStreamDecoder(actionRequestData);
		StakeHolderActionRequestData actionRequest;
		try {
			actionRequest = objectMapper.readValue(decodedData, StakeHolderActionRequestData.class);
			System.out.println("StakeHolder Action Request data...."+actionRequest);
		} catch (Exception e) {
			throw new CustomGeneralException("Invalid data format: " + e);
		}
		System.out.println(actionRequest.getId());
		

		OperatorLicence operatorLicence = operatorLicenceRepository.findById(actionRequest.getId())
				.orElseThrow(() -> new RuntimeException("Operator Licence not found"));
		
		System.out.println("Fetched Operator Licence by id data ..."+operatorLicence);

		OperatorLicenceRemarks remark = new OperatorLicenceRemarks();
		remark.setOperatorLicence(operatorLicence);
		remark.setRemark(actionRequest.getRemark());
		remark.setCreatedBy(actionRequest.getCreatedBy());
		remark.setStakeholder(actionTakenBy);
//		System.out.println("Remark"+remark);
		operatorLicenceRemarkRepository.save(remark);
		
		if (actionRequest.getInspectionReport() != null && !actionRequest.getInspectionReport().getFilePath().isBlank()) {
	        OperatorLicenceFiles file = actionRequest.getInspectionReport();
	        file.setFilePath(JsonFileExtractorUtil.uploadFile(file.getFilePath()));
	        file.setOperatorLicence(operatorLicence);

	        if (operatorLicence.getFiles() == null) {
	            operatorLicence.setFiles(new ArrayList<>());
	        }
	        operatorLicence.getFiles().add(file);
	        System.out.println("Files...." + operatorLicence.getFiles());
	    }

		if ("forward".equalsIgnoreCase(actionRequest.getAction())) {
			operatorLicence.setForwardedTo(forwardedTo);
			operatorLicence.setAction(action);
			operatorLicence.setStatus(status);
			System.out.println("Current action..."+action);
			System.out.println("Current Status..."+status);
		} else if ("reject".equalsIgnoreCase(actionRequest.getAction())) {
			operatorLicence.setStatus(Status.Rejected);
			operatorLicence.setForwardedTo(Stakeholder.APPLICANT);
			operatorLicence.setAction(Action.Rejected);
		}
		Status newStatus = operatorLicence.getStatus();
		System.out.println("Final Operator Licence data I am saving...."+operatorLicence);

		OperatorLicence savedOperatorLicense = operatorLicenceRepository.save(operatorLicence);
		
		if(Status.Approved.equals(newStatus)) {
			Tuser user = userRepo.findById(operatorLicence.getIntCreatedBy()).get();
			user.setSelRole(6); // Warehouse Operator
			userRepo.save(user);
		}

		if (status.equals(Status.Approved) && savedOperatorLicense.getEnmSaveStatus().equals(SaveStatus.New)) {
			System.out.println("Approving");
			MFinalOperatorLicense finalOperatorLicense = new MFinalOperatorLicense();
			finalOperatorLicense.setOperatorLicenceApplication(savedOperatorLicense);
			finalOperatorLicense.setEnmStatus(CreatedStatus.Created);
			finalOperatorLicense.setIntApprovedBy(actionRequest.getApprovedBy());
			finalOperatorLicense.setApplicationOfConformity(aocRepository.findByIntCreatedByAndEnmStatus(savedOperatorLicense.getIntCreatedBy(), Status.Accepted).get(0));
			mFinalOperatorlicenseRepository.save(finalOperatorLicense);
			System.out.println("Approved");
		}

		ResponseDTO<String> responseDTO = new ResponseDTO<>();
		responseDTO.setStatus(HttpStatus.OK.value());
		responseDTO.setResult("Action successfully completed");
		responseDTO.setMessage("Operator Licence action processed successfully");

		return CommonUtil.inputStreamEncoder(objectMapper.writeValueAsString(responseDTO)).toString();
	}
    
    @Override
    public Page<OperatorLicenceDTO> getApplications(Pageable pageable, Status status, Stakeholder stakeholder, Action action, String search) {
        Page<OperatorLicence> licences = operatorLicenceRepository.findByStatusAndForwardedToAndAction(status, stakeholder, action, pageable, search);
//        System.out.println(licences.getContent());
        return licences.map(Mapper::convertToDto);
    }

	@Transactional
	@Override
	public String saveOperatorLicence(String operatorLicence) throws JsonProcessingException {	
		String decodedData = CommonUtil.inputStreamDecoder(operatorLicence);
		System.out.println(decodedData);
		try {
			operatorLicence1 = new ObjectMapper().readValue(decodedData, OperatorLicence.class);
		} catch (Exception e) {
			log.error(e.getMessage());
			throw new CustomGeneralException("Invalid data format");
		}
		System.out.println(operatorLicence1);
    	operatorLicence1.setFiles(operatorLicence1.getFiles().stream().map(this::saveFile).toList());
    	System.out.println(operatorLicence1);
    	operatorLicence1.setStatus(Status.Pending); 
    	operatorLicence1.setForwardedTo(Stakeholder.TECHNICAL); 
    	operatorLicence1.setAction(Action.Pending);
		if (!operatorLicence1.getEnmSaveStatus().equals(SaveStatus.Renew)) {
			operatorLicence1.setVchApplicationNo(generateUniqueNo("t_operator_licence", "vchApplicationNo"));
		}
    	OperatorLicence savedOperatorLicence =operatorLicenceRepository.save(operatorLicence1);
		if (savedOperatorLicence.getEnmSaveStatus().equals(SaveStatus.Renew)) {
			operatorLicenceRemarkRepository.deleteAllByOperatorLicence(savedOperatorLicence);
		}
    	Integer operatorId = savedOperatorLicence.getId();

		responseDTO.setStatus(HttpStatus.CREATED.value());
		responseDTO.setResult(savedOperatorLicence.getVchApplicationNo());
		responseDTO.setMessage("Your Operator Licence Number is " + operatorId);
		return CommonUtil.inputStreamEncoder(objectMapper.writeValueAsString(responseDTO)).toString();
	}

    private OperatorLicenceFiles saveFile(OperatorLicenceFiles operatorLicenceFiles) {
    	operatorLicenceFiles.setFilePath(JsonFileExtractorUtil.uploadFile(operatorLicenceFiles.getFilePath()));
    	operatorLicenceFiles.setOperatorLicence(this.operatorLicence1);
        return operatorLicenceFiles;
    }

	@Override
    public Page<OperatorLicence> getOperatorLicences(Status status, Integer userId, PageRequest pageRequest) {
        return operatorLicenceRepository.findByFilters(status, userId, pageRequest);
    }

	@Override
	public List<OperatorLicence> getOperatorLicences(Status status, Integer userId) {
		return operatorLicenceRepository.findByFilters(status, userId);
	}

	@Override
	public Page<OperatorLicenceDTO> getAllApplications(Pageable pageable, Stakeholder forwardedTo, String search) {
		Page<OperatorLicence> licences = operatorLicenceRepository.findByForwardedTo(pageable, forwardedTo, search);
		return licences.map(Mapper::convertToDto);
	}

	@Override
	public OperatorLicence getOperatorLicence(Integer id) {
		return operatorLicenceRepository.findById(id).get();
	}

	public String generateUniqueNo(String tableName, String idName) {
		return jdbcTemplate.execute((Connection connection) -> {
			try (CallableStatement callableStatement = connection.prepareCall("{CALL GenerateCustomID(?, ?, ?)}")) {
				callableStatement.setString(1, tableName);
				callableStatement.setString(2, idName);
				callableStatement.registerOutParameter(3, Types.VARCHAR);
				callableStatement.execute();
				return callableStatement.getString(3);
			} catch (SQLException e) {
				throw new RuntimeException("Error generating custom ID", e);
			}
		});
	}

}
