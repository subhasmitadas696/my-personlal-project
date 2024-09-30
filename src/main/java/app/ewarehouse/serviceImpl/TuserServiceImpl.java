package app.ewarehouse.serviceImpl;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

import org.json.JSONArray;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.InspectorListDto;
import app.ewarehouse.dto.UserDataResponseDto;
import app.ewarehouse.dto.UserDetailsResponseDto;
import app.ewarehouse.entity.TempUser;
import app.ewarehouse.entity.Tuser;
import app.ewarehouse.repository.TempUserRepository;
import app.ewarehouse.repository.TuserRepository;
import app.ewarehouse.service.TuserService;
import app.ewarehouse.util.CommonUtil;
import jakarta.persistence.EntityManager;
import jakarta.persistence.Tuple;
@Service
public class TuserServiceImpl implements TuserService {
	@Autowired
	private TuserRepository tuserRepository;
	@Autowired
	EntityManager entityManager;
	
	@Autowired
	private TempUserRepository tempUserRepo;

	JSONObject selGender = new JSONObject("{1:Male,2:Female}");
	JSONObject chkPrevilege = new JSONObject("{2:Admin}");

	Integer parentId = 0;
	Object dynamicValue = null;

	
	
	@Override
	public JSONObject save(String data) {
		JSONObject json = new JSONObject();
		try {
			ObjectMapper om = new ObjectMapper();
			Tuser tuser = om.readValue(data, Tuser.class);
			
			List<String> fileUploadList = new ArrayList<String>();
			fileUploadList.add(tuser.getFilePhoto());
			if (!Objects.isNull(tuser.getIntId()) && tuser.getIntId() > 0) {
				Tuser getEntity = tuserRepository.findByIntIdAndBitDeletedFlag(tuser.getIntId(), false);
				getEntity.setTxtFullName(tuser.getTxtFullName());
				getEntity.setSelGender(tuser.getSelGender());
				getEntity.setFilePhoto(tuser.getFilePhoto());
				getEntity.setTxtMobileNo(tuser.getTxtMobileNo());
				getEntity.setTxtEmailId(tuser.getTxtEmailId());
				getEntity.setTxtAlternateMobileNumber(tuser.getTxtAlternateMobileNumber());
				getEntity.setTxtDateOfJoining(tuser.getTxtDateOfJoining());
				getEntity.setTxtrAddress(tuser.getTxtrAddress());
				getEntity.setSelRole(tuser.getSelRole());
				getEntity.setSelDesignation(tuser.getSelDesignation());
				getEntity.setSelEmployeeType(tuser.getSelEmployeeType());
				getEntity.setSelDepartment(tuser.getSelDepartment());
				getEntity.setSelGroup(tuser.getSelGroup());
				getEntity.setSelHierarchy(tuser.getSelHierarchy());
				getEntity.setTxtUserId(tuser.getTxtUserId());
				//getEntity.setTxtPassword(tuser.getEnPassword());
				getEntity.setIntReportingAuth(tuser.getIntReportingAuth());
				getEntity.setChkPrevilege(tuser.getChkPrevilege());
				Tuser updateData = tuserRepository.save(getEntity);
				parentId = updateData.getIntId();
				json.put("status", 202);
			} else {
				Tuser user = tuserRepository.getByUserId(tuser.getTxtUserId(), false);
				if (user == null) {
					tuser.setTxtFullName(tuser.getTxtFullName().trim());
					tuser.setTxtEmailId(tuser.getTxtEmailId().trim());
//					tuser.setTxtPassword(tuser.getTxtPassword().trim());
                    tuser.setTxtPassword(tuser.getEnPassword());
					Tuser saveData = tuserRepository.save(tuser);
					parentId = saveData.getIntId();
					json.put("status", 200);
				} else {
					json.put("status", 401);
				}
			}
			for (String fileUpload : fileUploadList) {
				if (!"".equals(fileUpload)) {
					File f = new File("src/storage/tempfile/" + fileUpload);
					if (f.exists()) {
						File src = new File("src/storage/tempfile/" + fileUpload);
						File dest = new File("src/storage/manage-users/" + fileUpload);
						try {
							Files.copy(src.toPath(), dest.toPath(), StandardCopyOption.REPLACE_EXISTING);
							Files.delete(src.toPath());
						} catch (IOException e) {
							System.out.println("Iniside Error");
						}
					}
				}
			}
			json.put("id", parentId);
		} catch (Exception e) {
			e.printStackTrace();
			json.put("status", 400);
		}
		return json;
	}

	@Override
	public JSONObject getById(Integer id) {
		Tuser entity = tuserRepository.findByIntIdAndBitDeletedFlag(id, false);
		dynamicValue = (selGender.has(entity.getSelGender().toString()))
				? selGender.get(entity.getSelGender().toString())
				: "--";
		entity.setSelGenderVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchRoleName from m_admin_role where intId=" + entity.getSelRole());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelRoleVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchDesgName from m_admin_designation where intId=" + entity.getSelDesignation());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelDesignationVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchEmployeeType from m_admin_employee_type where intId=" + entity.getSelEmployeeType());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelEmployeeTypeVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchdeptName from m_admin_department where intId=" + entity.getSelDepartment());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelDepartmentVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchGroupName from m_admin_groups where intId=" + entity.getSelGroup());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelGroupVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchStateName from m_states where intStateId=" + entity.getSelHierarchy());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		
		entity.setSelHierarchyVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchFullName from m_admin_user where intId=" + entity.getIntReportingAuth());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setIntReportingAuthVal(dynamicValue.toString());
		
		dynamicValue = (chkPrevilege.has(entity.getChkPrevilege().toString()))
				? chkPrevilege.get(entity.getChkPrevilege().toString())
				: "--";
		entity.setChkPrevilegeVal(dynamicValue.toString());

		return new JSONObject(entity);
	}

	@Override
	public JSONArray getAll(String formParams) {
		JSONObject jsonData = new JSONObject(formParams);
		List<Tuser> tuserResp = tuserRepository.findAllByBitDeletedFlag(false);
		for (Tuser entity : tuserResp) {
			dynamicValue = (selGender.has(entity.getSelGender().toString()))
					? selGender.get(entity.getSelGender().toString())
					: "--";
			entity.setSelGenderVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchRoleName from m_admin_role where intId=" + entity.getSelRole());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelRoleVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchDesgName from m_admin_designation where intId=" + entity.getSelDesignation());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelDesignationVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchEmployeeType from m_admin_employee_type where intId=" + entity.getSelEmployeeType());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelEmployeeTypeVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchdeptName from m_admin_department where intId=" + entity.getSelDepartment());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelDepartmentVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchGroupName from m_admin_groups where intId=" + entity.getSelGroup());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelGroupVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchStateName from m_states where intStateId=" + entity.getSelHierarchy());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelHierarchyVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchFullName from m_admin_user where intId=" + entity.getIntReportingAuth());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setIntReportingAuthVal(dynamicValue.toString());
			dynamicValue = (chkPrevilege.has(entity.getChkPrevilege().toString()))
					? chkPrevilege.get(entity.getChkPrevilege().toString())
					: "--";
			entity.setChkPrevilegeVal(dynamicValue.toString());

		}
		return new JSONArray(tuserResp);
	}

	@Override
	public JSONObject deleteById(Integer id) {
		JSONObject json = new JSONObject();
		try {
			Tuser entity = tuserRepository.findByIntIdAndBitDeletedFlag(id, false);
			entity.setBitDeletedFlag(true);
			tuserRepository.save(entity);
			json.put("status", 200);
		} catch (Exception e) {
			e.printStackTrace();
			json.put("status", 400);
		}
		return json;
	}

	public static JSONArray fillselRoleList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchRoleName from m_admin_role  where bitDeletedFlag=0";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchRoleName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	public static JSONArray fillselDesignationList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchDesgName from m_admin_designation  where bitDeletedFlag=0";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchDesgName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	public static JSONArray fillselEmployeeTypeList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchEmployeeType from m_admin_employee_type where bitDeletedFlag=0";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchEmployeeType", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	public static JSONArray fillselDepartmentList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchdeptName from m_admin_department  where bitDeletedFlag=0";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchdeptName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	public static JSONArray fillselGroupList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchGroupName from m_admin_groups where bitDeletedFlag=0";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchGroupName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}
	
	public static JSONArray fillUserList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchFullName from m_admin_user  where bitDeletedFlag=0 ";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchFullName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	public static JSONArray fillselHierarchyList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intStateId,vchStateName from m_states";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intStateId", data[0]);
			jsonObj.put("vchStateName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	@Override
	public JSONArray findUserList(String data) {
		JSONObject json = new JSONObject(data);
		JSONArray mainJSONFile = new JSONArray();
		Integer  selDepartment=json.getInt("intDepartment");
		Integer selRole=json.getInt("intRoleId");
		Integer selDesignation=json.getInt("intDesignantion");
		List<Tuser> userList=tuserRepository.findBySelDepartmentAndSelRoleAndSelDesignationAndBitDeletedFlag(selDepartment,selRole,selDesignation,false);
		for(Tuser users:userList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", users.getIntId());
			jsonObj.put("vchFullName", users.getTxtFullName());
			mainJSONFile.put(jsonObj);
		}
		
		return mainJSONFile;
	}

	@Override
	public Tuser findByMobileOrEmail(String mobile, String email) {
		return tuserRepository.getByTxtMobileNoOrTxtEmailId(mobile, email);
	}
	
	
	@Override
	public UserDataResponseDto getUserDataByEmailId(String email) {
		TempUser tempEntity = tempUserRepo.findByEmail(email);
		if(tempEntity != null) {
			UserDataResponseDto dto = new UserDataResponseDto();
			dto.setApplicantName(tempEntity.getApplicantName());
			dto.setMobileNo(tempEntity.getMobile());
			dto.setEmail(tempEntity.getEmail());
			dto.setPostalAddress(tempEntity.getPostalAddress());
			dto.setPostalCode(tempEntity.getPostalCode());
			dto.setTown(tempEntity.getTown());
			return dto;
		}
		return null;
	}

	@Override
	public List<InspectorListDto> getInspectors() {
		List<Tuple> list = tuserRepository.getInspectors();
		List<InspectorListDto> result = new ArrayList<>();
		for(Tuple tuple : list) {
			InspectorListDto dto = new InspectorListDto();
			dto.setId((Integer)tuple.get("intId"));
			dto.setName((String)tuple.get("vchFullName"));
			result.add(dto);
		}
		List<Tuple> list1 = tuserRepository.getCECMInspectors();
		for(Tuple tuple : list1) {
			InspectorListDto dto = new InspectorListDto();
			dto.setId((Integer)tuple.get("intId"));
			dto.setName((String)tuple.get("vchFullName"));
			result.add(dto);
		}
		return result;
	}
	
	@Override
	public List<InspectorListDto> getInspectorsByComplaintType(Integer complaintType) {
//		if(complaintType == 8 || complaintType == 9 || complaintType == 10 || complaintType == 11) {
//			List<Tuple> list = tuserRepository.getCECMInspectors();
//			List<InspectorListDto> result = new ArrayList<>();
//			for(Tuple tuple : list) {
//				InspectorListDto dto = new InspectorListDto();
//				dto.setId((Integer)tuple.get("intId"));
//				dto.setName((String)tuple.get("vchFullName"));
//				result.add(dto);
//			}
//			return result;
//		}else {
		List<Tuple> list = tuserRepository.getInspectors();
		List<InspectorListDto> result = new ArrayList<>();
		for(Tuple tuple : list) {
			InspectorListDto dto = new InspectorListDto();
			dto.setId((Integer)tuple.get("intId"));
			dto.setName((String)tuple.get("vchFullName"));
			result.add(dto);
		}
		return result;
//		}
	}

	@Override
	public List<InspectorListDto> getCollateral() {
		List<Tuple> list = tuserRepository.getCollateral();
		List<InspectorListDto> result = new ArrayList<>();
		for(Tuple tuple : list) {
			InspectorListDto dto = new InspectorListDto();
			dto.setId((Integer)tuple.get("intId"));
			dto.setName((String)tuple.get("vchFullName"));
			result.add(dto);
		}
		return result;
	}

	@Override
	public List<InspectorListDto> getGrader() {
		List<Tuple> list = tuserRepository.getGrader();
		List<InspectorListDto> result = new ArrayList<>();
		for(Tuple tuple : list) {
			InspectorListDto dto = new InspectorListDto();
			dto.setId((Integer)tuple.get("intId"));
			dto.setName((String)tuple.get("vchFullName"));
			result.add(dto);
		}
		return result;
	}

	@Override
	public UserDetailsResponseDto getUserDetails(Integer userId) {
	    return tuserRepository.findById(userId)
	        .map(user -> {
	            UserDetailsResponseDto dto = new UserDetailsResponseDto();
	            dto.setFullName(user.getTxtFullName());
	            dto.setContactNumber(user.getTxtMobileNo());
	            dto.setEmail(user.getTxtEmailId());
	            dto.setAddress(user.getTxtrAddress());
	            return dto;
	        })
	        .orElse(null);
	}

	


	
	

}
