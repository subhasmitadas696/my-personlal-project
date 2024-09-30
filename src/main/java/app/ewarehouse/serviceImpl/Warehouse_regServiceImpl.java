package app.ewarehouse.serviceImpl;
import java.util.List;
import org.json.JSONObject;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import org.json.JSONArray;
import jakarta.persistence.EntityManager;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import java.io.File;
import java.util.ArrayList;
import java.io.IOException;
import java.util.Objects;
import jakarta.transaction.Transactional;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.text.SimpleDateFormat;import java.util.Date;
import com.google.common.base.Strings;
import java.nio.file.Files;
import org.springframework.data.domain.PageRequest;
 import org.springframework.data.domain.Sort;import org.springframework.data.domain.Pageable;
 import java.util.Map;
 import java.math.BigDecimal;
 import java.sql.Clob;
 import java.util.stream.Collectors;
import java.nio.file.StandardCopyOption;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.repository.Warehouse_regRepository;
import app.ewarehouse.entity.Warehouse_reg;
import app.ewarehouse.service.Warehouse_regService;
@Transactional
@Service
public class Warehouse_regServiceImpl implements Warehouse_regService {
@Autowired 
private Warehouse_regRepository warehouse_regRepository;
@Autowired
	EntityManager entityManager;



Integer parentId = 0;
Object dynamicValue=null;
private static final Logger logger = LoggerFactory.getLogger(Warehouse_regServiceImpl.class);
JSONObject json=new JSONObject();
@Value("${tempUpload.path}")
	private String tempUploadPath;
@Value("${finalUpload.path}")
	private String finalUploadPath;

@Override 
public JSONObject save(String data)throws Exception{
logger.info("Inside save method of Warehouse_regServiceImpl");
ObjectMapper om = new ObjectMapper();
Warehouse_reg warehouse_reg=om.readValue(data, Warehouse_reg.class);List<String> fileUploadList = new ArrayList<String>();
if (!Objects.isNull(warehouse_reg.getIntId())&& warehouse_reg.getIntId()>0) {
Warehouse_reg getEntity = warehouse_regRepository.findByIntIdAndBitDeletedFlag(warehouse_reg.getIntId(), false);
getEntity.setTxtapplicantname(warehouse_reg.getTxtapplicantname());
getEntity.setTxtaddress(warehouse_reg.getTxtaddress());
Warehouse_reg updateData = warehouse_regRepository.save(getEntity);
parentId = updateData.getIntId();
json.put("status", 202);
}
else {
Warehouse_reg saveData = warehouse_regRepository.save(warehouse_reg);
parentId = saveData.getIntId();
json.put("status", 200);
}
json.put("id",parentId);
return json; 
}
@Override 
public JSONObject getById(Integer id)throws Exception{
logger.info("Inside getById method of Warehouse_regServiceImpl");
Warehouse_reg entity=warehouse_regRepository.findByIntIdAndBitDeletedFlag(id,false);

return new JSONObject(entity);
}
@Override 
public JSONObject getAll(String formParams)throws Exception{
logger.info("Inside getAll method of Warehouse_regServiceImpl");
JSONObject jsonData=new JSONObject(formParams);
Integer totalDataPresent = warehouse_regRepository.countByBitDeletedFlag(false);
Pageable pageRequest = PageRequest.of(jsonData.has("pageNo") ? jsonData.getInt("pageNo") - 1 : 0,
				jsonData.has("size") ? jsonData.getInt("size") : totalDataPresent,Sort.by(Sort.Direction.DESC, "intId"));
List<Warehouse_reg> warehouse_regResp=warehouse_regRepository.findAllByBitDeletedFlagAndIntInsertStatus(false,pageRequest);
warehouse_regResp.forEach(entity ->{

});
json.put("status", 200);
json.put("result", new JSONArray(warehouse_regResp));
json.put("count", totalDataPresent);
return json;
}
@Override 
public JSONObject deleteById(Integer id)throws Exception{
logger.info("Inside deleteById method of Warehouse_regServiceImpl");
Warehouse_reg entity=warehouse_regRepository.findByIntIdAndBitDeletedFlag(id,false);

entity.setBitDeletedFlag(true);
warehouse_regRepository.save(entity);
json.put("status", 200);
return json;
}




}