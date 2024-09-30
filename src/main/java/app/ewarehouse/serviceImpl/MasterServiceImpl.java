/**
 * 
 */
package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.ComplaintType;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.ComplaintTypeRepository;
import app.ewarehouse.service.MasterService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.Mapper;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import app.ewarehouse.dto.ComplaintTypeResponse;

import java.util.Calendar;
import java.util.List;

/**
 * Priyanka Singh
 */
@Service
public class MasterServiceImpl implements MasterService {

	private static final Logger logger = LoggerFactory.getLogger(MroleServiceImpl.class);

	@Autowired
	private ComplaintTypeRepository complaintTypeRepository;

	public Integer saveComplainttype(String complaintType) {
		String decodedData = CommonUtil.inputStreamDecoder((complaintType));
		ComplaintType type;
		try {
			type = new ObjectMapper().readValue(decodedData, ComplaintType.class);
			ComplaintType ComplaintTypeDetails = complaintTypeRepository
					.findByComplaintTypeIgnoreCase(type.getComplaintType());
			if (ComplaintTypeDetails == null) {
				Calendar calendar = Calendar.getInstance();
				type.setCreatedOn((calendar.getTime()));
				type.setCreatedBy("ok");
				type.setIsActive(false);
				type = complaintTypeRepository.save(type);
				if (type != null) {
					return 1;
				} else {
					return 0;
				}
			} else {
				return 2;
			}
		} catch (Exception e) {
			logger.info("Inside saveComplainttype method of MasterServiceImpl");
			return 0;
		}
	}



	public Integer update(String complaintType) {
		logger.info("Inside update method of MasterServiceImpl");
		String decodedData = CommonUtil.inputStreamDecoder((complaintType));
		ComplaintType type;
		try {
			type = new ObjectMapper().readValue(decodedData, ComplaintType.class);
			ComplaintType duplicateComplaintType = complaintTypeRepository
					.findByComplaintTypeIgnoreCase(type.getComplaintType());
			ComplaintType existingComplaintType = complaintTypeRepository
					.findByComplaintId(type.getComplaintId());
			if (existingComplaintType == null) {
				return 0; 
			}
			if (duplicateComplaintType != null
					&& !duplicateComplaintType.getComplaintId().equals(type.getComplaintId())) {
				return 2; 
			}
			Calendar calendar = Calendar.getInstance();
			existingComplaintType.setComplaintType(type.getComplaintType());
			existingComplaintType.setComplaintStatus(type.getComplaintStatus());
			existingComplaintType.setUpdatedBy(type.getUpdatedBy());
			existingComplaintType.setUpdatedOn(calendar.getTime());
			complaintTypeRepository.save(existingComplaintType);
			return 1; 
		} catch (Exception e) {
			logger.info("Inside update method of MasterServiceImpl");
			return 0; 
		}
	}

	@Override
	public ComplaintType getbyid(Integer complaintId) {
			return complaintTypeRepository.findById(complaintId).get();
	}


	@Override
	public ComplaintTypeResponse getById(Integer complaintId) {
		logger.info("Inside getById method of MasterServiceImpl");
		ComplaintType complainttype = complaintTypeRepository.findByComplaintId(complaintId);
		return Mapper.mapToComplaintTypeResponse(complainttype);
	}


	@Override
    public Integer resetComplaintId(Integer complaintId) {
		logger.info("Inside resetComplaintId method of MasterServiceImpl");
        try {
            if (complaintId != null) {
                int updatedRows = complaintTypeRepository.resetComplaintId(complaintId);
                return updatedRows > 0 ? 1 : 0;
            }
            return 0;
        } catch (Exception e) {
        	logger.info("Inside resetComplaintId method of MasterServiceImpl");
            return 0;
        }
    }



	@Override
	public Page<ComplaintTypeResponse> getAllcomplaintList(Pageable pageable) {
			return complaintTypeRepository.findAllData(pageable)
					.map(Mapper::mapToComplaintTypeResponse);
	}

	@Override
	public void deleteComplaint(Integer complaintId) {
			ComplaintType entity = complaintTypeRepository.findById(complaintId).orElseThrow(() -> new CustomGeneralException("Entity not found"));
			entity.setIsActive(!entity.getIsActive());
			complaintTypeRepository.save(entity);
	}

	@Override
	public List<ComplaintTypeResponse> getAllComplaintTypes() {
			return complaintTypeRepository.findByIsActive(false).stream()
					.map(Mapper::mapToComplaintTypeResponse)
					.toList();
	}



	@Override
	public List<ComplaintTypeResponse> getComplaintTypesByRoleId(Integer roleId) {
		return complaintTypeRepository.getComplaintTypesByRoleId(roleId).stream()
				.map(Mapper::mapToComplaintTypeResponse)
				.toList();
	}

}
