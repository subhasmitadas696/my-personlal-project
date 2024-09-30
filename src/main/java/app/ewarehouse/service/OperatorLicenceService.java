package app.ewarehouse.service;

import app.ewarehouse.dto.OperatorLicenceDTO;
import app.ewarehouse.entity.Action;
import app.ewarehouse.entity.OperatorLicence;
import app.ewarehouse.entity.Stakeholder;
import app.ewarehouse.entity.Status;

import com.fasterxml.jackson.core.JsonProcessingException;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;

import java.util.List;

public interface OperatorLicenceService {
	Page<OperatorLicence> getOperatorLicences(Status status, Integer userId, PageRequest of);
	String saveOperatorLicence(String operatorLicence) throws JsonProcessingException;
	Page<OperatorLicenceDTO> getApplications(Pageable pageable, Status status, Stakeholder stakeholder, Action action, String search);
	String handleAction(String technicalActionRequestData, Stakeholder forwardedTo, Stakeholder actionTakenBy, Action action,
			Status status) throws JsonProcessingException;
	Page<OperatorLicenceDTO> getAllApplications(Pageable pageable, Stakeholder forwardedTo, String search);
	OperatorLicence getOperatorLicence(Integer id);
	List<OperatorLicence> getOperatorLicences(Status status, Integer userId);
}
