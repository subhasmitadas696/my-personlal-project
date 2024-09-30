package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.CommitteeDesignation;
import app.ewarehouse.entity.LegalStatus;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.CommitteeDesignationRepository;
import app.ewarehouse.service.CommitteeDesignationService;
import app.ewarehouse.service.LegalStatusService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.validation.ConstraintViolation;
import jakarta.validation.Validator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Set;

@Service
public class CommitteeDesignationServiceImpl implements CommitteeDesignationService {
    @Autowired
    private CommitteeDesignationRepository repository;
    @Autowired
    private Validator validator;
    private static final Logger logger = LoggerFactory.getLogger(CommitteeDesignationServiceImpl.class);

    @Override
    public Integer save(String data) {
        logger.info("Inside save method of CommitteeDesignationServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(data);
        CommitteeDesignation committeeDesignation;

        try {
            committeeDesignation = new ObjectMapper().readValue(decodedData, CommitteeDesignation.class);
        } catch (Exception e) {
            throw new CustomGeneralException("Invalid data format: " + e);
        }

        Set<ConstraintViolation<CommitteeDesignation>> violations = validator.validate(committeeDesignation);
        if (!violations.isEmpty()) {
            throw new CustomGeneralException(violations);
        }

        return repository.save(committeeDesignation).getIntId();
    }

    @Override
    public CommitteeDesignation getById(Integer id) {
        logger.info("Inside getById method of CommitteeDesignationServiceImpl");

        return repository.findByIntIdAndBitDeletedFlag(id, false);
    }

    @Override
    public List<CommitteeDesignation> getAll() {
        logger.info("Inside getAll method of CommitteeDesignationServiceImpl");
        return repository.findAllByBitDeletedFlag(false);
    }
}
