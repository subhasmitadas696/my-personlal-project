/**
 * 
 */
package app.ewarehouse.serviceImpl;


import java.util.Date;
import java.util.List;
import java.util.stream.Collectors;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.BuyerDepositorResponse;
import app.ewarehouse.dto.DepositorResponse;
import app.ewarehouse.entity.Depositor;
import app.ewarehouse.entity.Status;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.DepositorRepository;
import app.ewarehouse.service.DepositorService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import app.ewarehouse.util.JsonFileExtractorUtil;
import app.ewarehouse.util.Mapper;
import jakarta.validation.Validator;

/**
 * Priyanka Singh
 */
@Service
public class DepositorServiceImpl implements DepositorService {

	@Autowired
    private DepositorRepository depositorRepository;
	
    @Autowired
    private Validator validator;
    private static final Logger logger = LoggerFactory.getLogger(MroleServiceImpl.class);

	@Override
	public String save(String details) {
		System.out.println(details+"--data for update");
		logger.info("Inside save method of BuyerServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(details);
        //Buyer buyer = JsonFileExtractorUtil.processRequestData(decodedData, List.of("txtPassportPath", "txtBankProofPath", "txtBusinessRegCertPath"), Buyer.class);
        Depositor depositor;

        try {
        	depositor = new ObjectMapper().readValue(decodedData, Depositor.class);
        	depositor.setTxtPassportPath(JsonFileExtractorUtil.uploadFile(depositor.getTxtPassportPath(), FolderAndDirectoryConstant.DEPOSITOR_REG_FOLDER));
        	depositor.setTxtBankProofPath(JsonFileExtractorUtil.uploadFile(depositor.getTxtBankProofPath(), FolderAndDirectoryConstant.DEPOSITOR_REG_FOLDER));
        	depositor.setTxtBusinessRegCertPath(JsonFileExtractorUtil.uploadFile(depositor.getTxtBusinessRegCertPath(),FolderAndDirectoryConstant.DEPOSITOR_REG_FOLDER));
        
        	 
        } catch (Exception e) {
            throw new CustomGeneralException("Invalid data format: " + e);
        }
        

//        System.out.println(depositor);

//        Set<ConstraintViolation<Depositor>> violations = validator.validate(depositor);
//        if (!violations.isEmpty()) {
//            throw new CustomValidationException(violations);
//        } 
        Depositor saveData = depositorRepository.save(depositor);
  	  return saveData.getIntId();
	}

	@Override
	public DepositorResponse getById(String id) {
		 logger.info("Inside getById method of BuyerServiceImpl");
	        Depositor depositor = depositorRepository.findByIntIdAndBitDeletedFlag(id, false);
	        return Mapper.mapToDepositorResponse(depositor);
	}

//	@Override
//	public List<Depositor> getAll() {
//		 logger.info("Inside getAll method of BuyerServiceImpl");
//		 return depositorRepository.findAll();
//	}

	@Override
	public String deleteById(String id) {
		// TODO Auto-generated method stub
		return null;
	}

	/*
	 * @Override public Page<BuyerResponse> getFilteredBuyers(Date fromDate, Date
	 * toDate, Status status, Pageable pageable) { // TODO Auto-generated method
	 * stub return null; }
	 */
	@Override
	public Object takeAction(String buyer) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public List<Depositor> getAll() {
		return depositorRepository.findAll();
		

	}

	@Override
	public Page<DepositorResponse> getFilteredDepositors(Date fromDate, Date toDate, Status status, Pageable pageable) {
		 logger.info("Inside getFilteredDepositors method of DepositorServiceImpl");
	        Page<Depositor> depositorPage = depositorRepository.findByFilters(fromDate, toDate, status, pageable);
	        logger.info("from: " + fromDate + " to: " + toDate);
	        List<DepositorResponse> depositorResponses = depositorPage.getContent().stream()
	                .map(Mapper::mapToDepositorResponse)
	                .collect(Collectors.toList());

	        return new PageImpl<>(depositorResponses, pageable, depositorPage.getTotalElements());
	}

	@Override
	public Page<BuyerDepositorResponse> getFilteredBuyers(Date fromDate, Date toDate, Status status,
			Pageable pageable) {
		// TODO Auto-generated method stub
		return null;
	}

}
