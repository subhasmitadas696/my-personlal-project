package app.ewarehouse.serviceImpl;

import java.util.List;
import org.json.JSONObject;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import org.json.JSONArray;
import jakarta.persistence.EntityManager;
import org.springframework.beans.factory.annotation.Autowired;
import java.util.ArrayList;
import java.util.Objects;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


import app.ewarehouse.repository.MemployeetypeRepository;
 import app.ewarehouse.entity.Memployeetype;
 import app.ewarehouse.service.MemployeetypeService;
 import app.ewarehouse.entity.Tuser;
 import app.ewarehouse.repository.TuserRepository;
@Service
public class MemployeetypeServiceImpl implements MemployeetypeService {
	@Autowired
	private MemployeetypeRepository memployeetypeRepository;
	@Autowired
	EntityManager entityManager;
	@Autowired
	private TuserRepository tuserRepository;
	private static final Logger logger = LoggerFactory.getLogger(MemployeetypeServiceImpl.class);
	Integer parentId = 0;
	Object dynamicValue = null;

	@Override
	public JSONObject save(String data) {
		logger.info("Inside save method of MemployeetypeServiceImpl");
		JSONObject json = new JSONObject();
		int countForEmpType=0;
		int countForAlias=0;
		try {
			ObjectMapper om = new ObjectMapper();
			Memployeetype memployeetype = om.readValue(data, Memployeetype.class);
			memployeetype.setTxtAliasName(memployeetype.getTxtAliasName().trim());
			memployeetype.setTxtEmployeeType(memployeetype.getTxtEmployeeType().trim());
			List<String> fileUploadList = new ArrayList<String>();
			if (!Objects.isNull(memployeetype.getIntId()) && memployeetype.getIntId() > 0) {
					countForEmpType = memployeetypeRepository.getCountByEmployeeTypeANDbitDeletedFlagNOTIntId(memployeetype.getIntId(),
							memployeetype.getTxtEmployeeType(), false);
					if (memployeetype.getTxtAliasName().length() > 0) {
						countForAlias = memployeetypeRepository.getCountByAliasNameANDbitDeletedFlagNOTIntId(memployeetype.getIntId(),
								memployeetype.getTxtAliasName(), false);
					}
					if (countForEmpType > 0) {
						json.put("status", 401);
						return json;
					}
					if (countForAlias > 0) {
						json.put("status", 408);
						return json;
					}
				
				Memployeetype getEntity = memployeetypeRepository.findByIntIdAndBitDeletedFlag(memployeetype.getIntId(),
						false);
				getEntity.setTxtEmployeeType(memployeetype.getTxtEmployeeType());
				getEntity.setTxtAliasName(memployeetype.getTxtAliasName());
				Memployeetype updateData = memployeetypeRepository.save(getEntity);
				parentId = updateData.getIntId();
				json.put("status", 202);
			} else {
				
				countForEmpType = memployeetypeRepository
						.countByEmployeeTypeANDBitDeletedFlag(memployeetype.getTxtEmployeeType(), false);
				if (memployeetype.getTxtAliasName().length() > 0) {
					countForAlias = memployeetypeRepository
							.countByTxtAliasNameANDBitDeletedFlag(memployeetype.getTxtAliasName(), false);
				}
				if(countForEmpType < 1 && countForAlias < 1) {
				Memployeetype saveData = memployeetypeRepository.save(memployeetype);
				parentId = saveData.getIntId();
				json.put("status", 200);
				}else {
					if (countForEmpType >= 1) {
						json.put("status", 401);
					} else if (countForAlias >= 1) {
						json.put("status", 408);
					}
				}
			}
			json.put("id", parentId);
		} catch (Exception e) {
			logger.error("Inside save method of MemployeetypeServiceImpl some error occur:" + e);
			json.put("status", 400);
		}
		return json;
	}

	@Override
	public JSONObject getById(Integer id) {
		logger.info("Inside getById method of MemployeetypeServiceImpl");
		Memployeetype entity = memployeetypeRepository.findByIntIdAndBitDeletedFlag(id, false);

		return new JSONObject(entity);
	}

	@Override
	public JSONArray getAll(String formParams) {
		logger.info("Inside getAll method of MemployeetypeServiceImpl");
		List<Memployeetype> temployeetypeResp = memployeetypeRepository.findAllByBitDeletedFlag(false);
		
		for (Memployeetype employeetype : temployeetypeResp) {
			if (!(employeetype.getTxtAliasName().length() > 0)) {
				employeetype.setTxtAliasName("---");
			}
		}
		return new JSONArray(temployeetypeResp);
	}

	@Override
	public JSONObject deleteById(Integer id) {
		logger.info("Inside deleteById method of MemployeetypeServiceImpl");
		JSONObject json = new JSONObject();
		try {
			List<Tuser> tuser=tuserRepository.findByUserDataBySelEmpId(id,false);
			if(tuser.isEmpty()) {
			Memployeetype entity = memployeetypeRepository.findByIntIdAndBitDeletedFlag(id, false);
			entity.setBitDeletedFlag(true);
			memployeetypeRepository.save(entity);
			json.put("status", 200);}
			else {
				json.put("status", 300);
			}
		} catch (Exception e) {
			logger.error("Inside deleteById method of MemployeetypeServiceImpl some error occur:" + e);
			json.put("status", 400);
		}
		return json;
	}

}
