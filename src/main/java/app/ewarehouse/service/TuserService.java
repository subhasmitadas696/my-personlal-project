package app.ewarehouse.service;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import app.ewarehouse.dto.InspectorListDto;
import app.ewarehouse.dto.UserDataResponseDto;
import app.ewarehouse.dto.UserDetailsResponseDto;
import app.ewarehouse.entity.Tuser;
 

public interface TuserService {
JSONObject save(String tuser);
JSONObject getById(Integer Id);
JSONArray getAll(String formParams);
JSONObject deleteById(Integer id);
JSONArray findUserList(String data);
Tuser findByMobileOrEmail(String mobile, String email);
UserDataResponseDto getUserDataByEmailId(String email);
List<InspectorListDto> getInspectors();
List<InspectorListDto> getCollateral();
List<InspectorListDto> getGrader();
UserDetailsResponseDto getUserDetails(Integer userId);
List<InspectorListDto> getInspectorsByComplaintType(Integer complaintType);
}
