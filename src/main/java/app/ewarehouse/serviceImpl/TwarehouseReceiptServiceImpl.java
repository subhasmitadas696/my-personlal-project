package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.*;
import app.ewarehouse.exception.CustomEntityNotFoundException;
import app.ewarehouse.dto.WarehouseReceiptResponse;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.TreceiveCommodityRepository;
import app.ewarehouse.repository.TwarehouseReceiptRepository;
import app.ewarehouse.service.TwarehouseReceiptService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.ErrorMessages;
import app.ewarehouse.util.Mapper;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;
import java.util.stream.Collectors;

@Service
public class TwarehouseReceiptServiceImpl implements TwarehouseReceiptService {
    @Autowired
    TwarehouseReceiptRepository repo;
    @Autowired
    ErrorMessages errorMessages;
    @Autowired
    TreceiveCommodityRepository treceiveCommodityRepository;

    private static final Logger logger = LoggerFactory.getLogger(TwarehouseReceiptServiceImpl.class);

    @Override
    public List<TwarehouseReceipt> findAll() {
        return repo.findAll();
    }

    @Override
    public WarehouseReceiptResponse getDetailsById(String txtWarehouseReceiptId) {
        try {
            TwarehouseReceipt commodity = repo.findByTxtWarehouseReceiptIdAndBitDeleteFlag(txtWarehouseReceiptId, false);
            if (commodity == null) {
                throw new CustomEntityNotFoundException(errorMessages.getFailedToFetch() + " " + txtWarehouseReceiptId);
            }
            return Mapper.mapToWarehouseReceiptResponse(commodity);
        } catch (CustomEntityNotFoundException notFoundException) {
            throw notFoundException;
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    @Transactional
    @Override
    public TwarehouseReceipt save(String data) {
        try {
            String decodedData = CommonUtil.inputStreamDecoder(data);
            logger.info("decoded data is: {} ", decodedData);
            TwarehouseReceipt twarehouseReceipt = new ObjectMapper().readValue(decodedData, TwarehouseReceipt.class);

            String receiveCId = twarehouseReceipt.getReceiveCommodity().getTxtReceiveCId();
            TwarehouseReceipt existingWarehouseReceipt = repo.findByReceiveCommodity_TxtReceiveCIdAndBitDeleteFlag(receiveCId, false);

            if (existingWarehouseReceipt != null) {
                existingWarehouseReceipt.setStatus(twarehouseReceipt.getStatus());
                existingWarehouseReceipt.setTxtRemark(twarehouseReceipt.getTxtRemark());
                existingWarehouseReceipt.setDtmCreatedOn(new Date());

                repo.save(existingWarehouseReceipt);
            } else {
                twarehouseReceipt.setDtmCreatedOn(new Date());
                repo.save(twarehouseReceipt);
            }

            TreceiveCommodity commodity = treceiveCommodityRepository.findByTxtReceiveCIdAndBitDeleteFlag(twarehouseReceipt.getReceiveCommodity().getTxtReceiveCId(), false);
            if (commodity != null) {
                commodity.setStatus(twarehouseReceipt.getStatus());
                treceiveCommodityRepository.save(commodity);
            } else {
                throw new CustomGeneralException(errorMessages.getFailedToFetch() + " " + twarehouseReceipt.getReceiveCommodity().getTxtReceiveCId());
            }

            return twarehouseReceipt;
        } catch (DataIntegrityViolationException e) {
            throw new CustomGeneralException(errorMessages.getValidCommodityId());
        } catch (JsonProcessingException e) {
            throw new CustomGeneralException(errorMessages.getUnknownError());
        }
    }

    @Override
    public List<WarehouseReceiptResponse> getReceiptForDepositor(String id) {
        logger.info("inside WarehouseReceiptList details");
        List<TwarehouseReceipt> twarehouseReceiptList =  repo.findAllByReceiveCommodity_Depositor_IntIdAndBitDeleteFlagAndStatusAndReceiptStatus(id,false,Status.Approved,enmReceiptStatus.Pending);
        if (twarehouseReceiptList.isEmpty()) {
            throw new CustomGeneralException(errorMessages.getReceiptNotFound());
        }
            return twarehouseReceiptList.stream().map(Mapper::mapToWarehouseReceiptResponse)
                    .collect(Collectors.toList());
    }

    @Override
    public Page<WarehouseReceiptResponse> getFilteredReceipts(Status status, String search, String sortColumn, String sortDirection, Pageable pageable) {
        logger.info("Inside getFilteredReceipts method of TwarehouseReceiptServiceImpl");

        Page<TwarehouseReceipt> response = repo.findByFilters(status, search, pageable);
        List<WarehouseReceiptResponse> warehouseResponse = response.getContent().stream()
                .map(Mapper::mapToWarehouseReceiptResponse)
                .toList();

        return new PageImpl<>(warehouseResponse, pageable, response.getTotalElements());
    }

	@Override
	public List<WarehouseReceiptResponse> getEligibleLossReceiptListByDepositor(String id) {
        List<TwarehouseReceipt> warehouseReceiptList = repo.getEligibleLossReceiptListByDepositor(id,false,Status.Approved,List.of(enmReceiptStatus.Cancelled,enmReceiptStatus.Delivered,enmReceiptStatus.Pledged,enmReceiptStatus.Retired));
        if (warehouseReceiptList.isEmpty()) {
            throw new CustomGeneralException(errorMessages.getReceiptNotFound());
        }
            return warehouseReceiptList.stream().map(Mapper::mapToWarehouseReceiptResponse)
                    .collect(Collectors.toList());
    }

    @Override
    public List<String> getReceiptNoForDepositor(String id) {
        logger.info("inside getReceiptNoForDepositor details");
        List<String> twarehouseReceiptList =  repo.findReceiptNoUsingDepositor(id,false,Status.Approved,enmReceiptStatus.Pending);
        if (twarehouseReceiptList.isEmpty()) {
            throw new CustomGeneralException(errorMessages.getReceiptNotFound());
        }
        return twarehouseReceiptList;
    }
}
