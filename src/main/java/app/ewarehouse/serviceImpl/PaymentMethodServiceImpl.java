package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.PaymentMethod;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.PaymentMethodRepository;
import app.ewarehouse.service.PaymentMethodService;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.json.JSONArray;
import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;
import java.util.Objects;

@Service
public class PaymentMethodServiceImpl implements PaymentMethodService {

    @Autowired
    private PaymentMethodRepository paymentMethodRepository;
    Integer parentId = 0;
    private static final Logger logger = LoggerFactory.getLogger(PaymentMethodServiceImpl.class);

    @Override
    public JSONObject save(String data) {
        logger.info("Inside save method of PaymentMethodServiceImpl");
        JSONObject json = new JSONObject();
        int countForPaymentMethod=0;
        try {
            ObjectMapper om = new ObjectMapper();
            PaymentMethod paymentMethod = om.readValue(data, PaymentMethod.class);
            paymentMethod.setTxtPaymentMethod(paymentMethod.getTxtPaymentMethod().trim());
            paymentMethod.setTxtDescription(paymentMethod.getTxtDescription().trim());

            if (!Objects.isNull(paymentMethod.getIntId()) && paymentMethod.getIntId() > 0) {
                paymentMethod.setStmUpdatedOn(new Date());
                System.out.println("paymentMethod: " + paymentMethod);
                json.put("status", 202);
            } else {
                json.put("status", 200);
            }

            PaymentMethod saveData = paymentMethodRepository.save(paymentMethod);
            parentId = saveData.getIntId();


            json.put("id", parentId);
        }
        catch (DataIntegrityViolationException e){
            logger.error("Inside save method of PaymentMethodServiceImpl some error occur:" + e.getMessage());
            json.put("status", 401);
        }
        catch (Exception e) {
            logger.error("Inside save method of PaymentMethodServiceImpl some error occur:" + e.getMessage());
            json.put("status", 400);
        }
        return json;
    }

    @Override
    public JSONObject getById(Integer id) {
        logger.info("Inside getById method of PaymentMethodServiceImpl");
        PaymentMethod entity = paymentMethodRepository.findByIntIdAndBitDeletedFlag(id, false);

        return new JSONObject(entity);
    }

    @Override
    public JSONArray getAll(String formParams) {
        logger.info("Inside getAll method of PaymentMethodServiceImpl");
        List<PaymentMethod> tPaymentMethodResp = paymentMethodRepository.findAll();
        return new JSONArray(tPaymentMethodResp);
    }

    @Override
    public JSONArray getAllActive(String formParams) {
        logger.info("Inside getAll method of PaymentMethodServiceImpl");
        List<PaymentMethod> tPaymentMethodResp = paymentMethodRepository.findAllByBitDeletedFlag(false);
        return new JSONArray(tPaymentMethodResp);
    }

    @Override
    public JSONObject deleteById(Integer id) {
        logger.info("Inside deleteById method of PaymentMethodServiceImpl");
        JSONObject json = new JSONObject();
        try {
            PaymentMethod entity = paymentMethodRepository.findById(id).orElseThrow(() -> new CustomGeneralException("Entity not found"));
            entity.setBitDeletedFlag(!entity.getBitDeletedFlag());
            paymentMethodRepository.save(entity);
            json.put("status", 200);
        } catch (Exception e) {
            logger.error("Inside deleteById method of PaymentMethodServiceImpl some error occur:" + e);
            json.put("status", 400);
        }
        return json;
    }

}
