package app.ewarehouse.service;

import org.json.JSONArray;
import org.json.JSONObject;

public interface PaymentMethodService {
    JSONObject save(String paymentMethod);
    JSONObject getById(Integer Id);
    JSONArray getAll(String formParams);

    JSONArray getAllActive(String formParams);

    JSONObject deleteById(Integer id);
}
