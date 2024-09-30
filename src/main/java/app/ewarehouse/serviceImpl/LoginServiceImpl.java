package app.ewarehouse.serviceImpl;

import java.time.Duration;
import java.time.LocalDateTime;

import org.apache.tomcat.util.json.JSONParser;
import org.apache.tomcat.util.json.ParseException;
import org.json.JSONArray;
import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import app.ewarehouse.dto.LoginDto;
import app.ewarehouse.entity.Tuser;
import app.ewarehouse.repository.TuserRepository;
import app.ewarehouse.service.LogInService;
@Service

public class LoginServiceImpl implements LogInService {

	@Autowired
	TuserRepository tuserRepository;

    private static final Logger logger = LoggerFactory.getLogger(LoginServiceImpl.class);

	@Override
	public JSONObject loinCheck(LoginDto loginDto)  {
		logger.info("Inside loinCheck of LoginServiceImpl");
		JSONObject output = new JSONObject();
		Tuser resp = tuserRepository.findByTxtUserIdAndTxtPasswordAndBitDeletedFlag(loginDto.getUserId(),
				loginDto.getUserPassword(), false);
		if (resp != null) {
			JSONObject result = new JSONObject();
			result.put("username", resp.getTxtFullName());
			result.put("email", resp.getTxtEmailId());
			result.put("userid", resp.getIntId());
			result.put("roleId", resp.getSelRole());
			result.put("privilage",resp.getChkPrevilege());
			result.put("warehouseId",resp.getWarehouseId());
			logger.info("warehouseid is :"+resp.getWarehouseId());
			output.put("status", 200);
			output.put("msg", "success");
			output.put("result", result);
		} else {
			logger.warn("Inside loinCheck of LoginServiceImpl not found");
			output.put("status", 400);
			
			
		}
		return output;
	}
	
	@Override
	public JSONObject validateUserIdEmail(String data) {
		logger.info("Inside validateUserIdEmail method of LoginServiceImpl");
		JSONObject response = new JSONObject();
		try {
			JSONObject jsonObject = new JSONObject(data);
			Tuser mAdminUserMaster = tuserRepository.getByTxtUserIdAndTxtEmailIdAndBitDeletedFlag(
					jsonObject.getString("username"), jsonObject.getString("userEmail"), false);
			if (mAdminUserMaster != null) {
				mAdminUserMaster.setDtforgetpasswordstarttime(LocalDateTime.now());
				tuserRepository.save(mAdminUserMaster);
				response.put("status", 200).put("url", jsonObject.getString("userURL"));
			} else {
				response.put("status", 400).put("msg", "Not Found");
			}
		} catch (Exception e) {
			logger.error("Inside validateTime method of LoginServiceImpl error occur:" + e);
			response.put("status", 400);
		}
		return response;
	}

	@Override
	public JSONObject validateTime(String data) {
		logger.info("Inside validateTime method of LoginServiceImpl");
		JSONObject response = new JSONObject();
		try {
			JSONObject jsonObject = new JSONObject(data);
			Tuser mAdminUserMaster = tuserRepository
					.getByTxtUserIdAndBitDeletedFlag(jsonObject.getString("username"), false);
			if (mAdminUserMaster != null) {
				LocalDateTime startTime = mAdminUserMaster.getDtforgetpasswordstarttime();
				LocalDateTime presentDatetime = LocalDateTime.now();
				Duration duration = Duration.between(startTime, presentDatetime);
				if (duration.toDays() <= 0 && duration.toHours() <= 0 && duration.toMinutes() <= 30) {
					response.put("status", 200);
				} else {
					response.put("status", 300).put("msg", "Time Exceed");
				}
			} else {
				response.put("status", 400).put("msg", "Not Found");
			}
		} catch (Exception e) {
			logger.error("Inside validateTime method of LoginServiceImpl error occur:" + e);
			response.put("status", 400);
		}
		return response;
	}

	@Override
	public JSONObject saveforgetpasswordchange(String data) {
		logger.info("Inside saveforgetpasswordchange method of LoginServiceImpl");
		JSONObject response = new JSONObject();
		try {
			JSONObject jsonObject = new JSONObject(data);
			Tuser mAdminUserMaster = tuserRepository
					.getByTxtUserIdAndBitDeletedFlag(jsonObject.getString("username"), false);
			if (mAdminUserMaster != null) {
				LocalDateTime startTime = mAdminUserMaster.getDtforgetpasswordstarttime();
				LocalDateTime presentDatetime = LocalDateTime.now();
				Duration duration = Duration.between(startTime, presentDatetime);
				if (duration.toDays() <= 0 && duration.toHours() <= 0 && duration.toMinutes() <= 30) {
					if (jsonObject.getString("userpassword").equals(jsonObject.getString("txtConfirmPassword"))) {
						mAdminUserMaster.setTxtPassword(jsonObject.getString("userpassword"));
						tuserRepository.save(mAdminUserMaster);
						response.put("status", 200);
					} else {
						response.put("status", 302);
					}
				} else {
					response.put("status", 300).put("msg", "Time Exceed");
				}
			}
		} catch (Exception e) {
			logger.error("Inside saveforgetpasswordchange method of LoginServiceImpl error occur:" + e);
			response.put("status", 400);
		}
		return response;
	}


}
