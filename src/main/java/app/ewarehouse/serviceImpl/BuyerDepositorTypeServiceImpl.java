package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.Bank;
import app.ewarehouse.entity.BuyerDepositorType;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.BuyerDepositorTypeRepository;
import app.ewarehouse.service.BankService;
import app.ewarehouse.service.BuyerDepositorTypeService;
import app.ewarehouse.util.CommonUtil;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.validation.ConstraintViolation;
import jakarta.validation.Validator;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Set;

@Service
@Slf4j
public class BuyerDepositorTypeServiceImpl implements BuyerDepositorTypeService {
    @Autowired
    private BuyerDepositorTypeRepository buyerDepositorTypeRepository;
    @Autowired
    private Validator validator;

    @Override
    public Integer save(String data) {
        log.info("Inside save method of BuyerDepositorTypeServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(data);
        BuyerDepositorType buyerDepositorType;

        try {
            buyerDepositorType = new ObjectMapper().readValue(decodedData, BuyerDepositorType.class);
        } catch (Exception e) {
            log.error("Invalid data format: " + e);
            throw new CustomGeneralException("Invalid data format");
        }

        Set<ConstraintViolation<BuyerDepositorType>> violations = validator.validate(buyerDepositorType);
        if (!violations.isEmpty()) {
            throw new CustomGeneralException(violations);
        }

        return buyerDepositorTypeRepository.save(buyerDepositorType).getIntId();
    }

    @Override
    public BuyerDepositorType getById(Integer id) {
        log.info("Inside getById method of BuyerDepositorTypeServiceImpl");

        return buyerDepositorTypeRepository.findByIntIdAndBitDeletedFlag(id, false);
    }

    @Override
    public List<BuyerDepositorType> getAll() {
        log.info("Inside getAll method of BuyerDepositorTypeServiceImpl");
        return buyerDepositorTypeRepository.findAllByBitDeletedFlag(false);
    }
}

