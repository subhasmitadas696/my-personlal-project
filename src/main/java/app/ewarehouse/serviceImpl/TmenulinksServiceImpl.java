package app.ewarehouse.serviceImpl;

import java.util.List;
import org.json.JSONObject;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import org.json.JSONArray;
import jakarta.persistence.EntityManager;
import org.springframework.beans.factory.annotation.Autowired;
import java.io.File;
import java.util.ArrayList;
import java.io.IOException;
import java.util.Objects;
import jakarta.transaction.Transactional;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;

import app.ewarehouse.entity.Tmenulinks;
 import app.ewarehouse.service.TrolepermissionService;
 import app.ewarehouse.repository.TuserRepository;
import app.ewarehouse.service.TmenulinksService;
 import app.ewarehouse.entity.Trolepermission;
import app.ewarehouse.repository.TmenulinksRepository;
 import app.ewarehouse.util.CommonUtil;
@Service
public class TmenulinksServiceImpl implements TmenulinksService {
	@Autowired
	private TmenulinksRepository tmenulinksRepository;
	@Autowired
	EntityManager entityManager;
	@Autowired
	private TrolepermissionService trolepermissionService;
	@Autowired
	private TuserRepository tuserRepository;

	JSONObject selLinkType = new JSONObject("{1:Global Link,2:Parent Link,3:Secondary Link,4:Button,5:Tab}");

	Integer parentId = 0;
	Object dynamicValue = null;

	@Override
	public JSONObject save(String data) {
		JSONObject json = new JSONObject();
		try {
			ObjectMapper om = new ObjectMapper();
			Tmenulinks tmenulinks = om.readValue(data, Tmenulinks.class);
			List<String> fileUploadList = new ArrayList<String>();
			if (!Objects.isNull(tmenulinks.getIntId()) && tmenulinks.getIntId() > 0) {
				Tmenulinks getEntity = tmenulinksRepository.findByIntIdAndBitDeletedFlag(tmenulinks.getIntId(), false);
				getEntity.setSelLinkType(tmenulinks.getSelLinkType());
				getEntity.setSelParentLink(tmenulinks.getSelParentLink());
				getEntity.setTxtLinkName(tmenulinks.getTxtLinkName());
				getEntity.setTxtURL(tmenulinks.getTxtURL());
				getEntity.setVchViewUrl(tmenulinks.getTxtURL());		
				getEntity.setTxtCSSClass(tmenulinks.getTxtCSSClass());
				getEntity.setTxtSerialNo(tmenulinks.getTxtSerialNo());
				Tmenulinks updateData = tmenulinksRepository.save(getEntity);
				parentId = updateData.getIntId();
				json.put("status", 202);
			} else {
				tmenulinks.setIntApplicableFor(1);
				tmenulinks.setVchViewUrl(tmenulinks.getTxtURL());
				tmenulinks.setTxtURL(null);
				Tmenulinks saveData = tmenulinksRepository.save(tmenulinks);
				parentId = saveData.getIntId();
				json.put("status", 200);
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
		Tmenulinks entity = tmenulinksRepository.findByIntIdAndBitDeletedFlag(id, false);
		dynamicValue = (selLinkType.has(entity.getSelLinkType().toString()))
				? selLinkType.get(entity.getSelLinkType().toString())
				: "--";
		entity.setSelLinkTypeVal(dynamicValue.toString());
		try {
			dynamicValue = CommonUtil.getDynSingleData(entityManager,
					"select vchLinkName from m_admin_menulinks where intLinkType=" + entity.getSelParentLink());
		} catch (Exception ex) {
			dynamicValue = "--";
		}
		entity.setSelParentLinkVal(dynamicValue.toString());

		return new JSONObject(entity);
	}

	@Override
	public JSONArray getAll(String formParams) {
		JSONObject jsonData = new JSONObject(formParams);
		List<Tmenulinks> tmenulinksResp = tmenulinksRepository.findAllByBitDeletedFlag(false);
		for (Tmenulinks entity : tmenulinksResp) {
			dynamicValue = (selLinkType.has(entity.getSelLinkType().toString()))
					? selLinkType.get(entity.getSelLinkType().toString())
					: "--";
			entity.setSelLinkTypeVal(dynamicValue.toString());
			try {
				dynamicValue = CommonUtil.getDynSingleData(entityManager,
						"select vchLinkName from m_admin_menulinks where intLinkType=" + entity.getSelParentLink());
			} catch (Exception ex) {
				dynamicValue = "--";
			}
			entity.setSelParentLinkVal(dynamicValue.toString());

		}
		return new JSONArray(tmenulinksResp);
	}

	@Override
	public JSONObject deleteById(Integer id) {
		JSONObject json = new JSONObject();
		try {
			Tmenulinks entity = tmenulinksRepository.findByIntIdAndBitDeletedFlag(id, false);
			entity.setBitDeletedFlag(true);
			tmenulinksRepository.save(entity);
			json.put("status", 200);
		} catch (Exception e) {
			e.printStackTrace();
			json.put("status", 400);
		}
		return json;
	}

	public static JSONArray fillselParentLinkList(EntityManager em, String jsonVal) {
		JSONArray mainJSONFile = new JSONArray();
		String query = "Select intId,vchLinkName from m_admin_menulinks where intLinkType=1";
		List<Object[]> dataList = CommonUtil.getDynResultList(em, query);
		for (Object[] data : dataList) {
			JSONObject jsonObj = new JSONObject();
			jsonObj.put("intId", data[0]);
			jsonObj.put("vchLinkName", data[1]);
			mainJSONFile.put(jsonObj);
		}
		return mainJSONFile;
	}

	@Override
	public JSONArray getByDataUsing(String data) {
		JSONObject jsob = new JSONObject(data);
		Integer roleId = jsob.getInt("intRoleId");
		Integer userId = jsob.getInt("intUserId");
		JSONArray makeArray = new JSONArray();
		Integer isAdmin = tuserRepository.getcheckPrevilegeByUserId(userId, false);
		List<Trolepermission> rolePermission = trolepermissionService.getRolePermissionListByUserId(userId);

		if (roleId == 0) {
			List<Object[]> menuList = tmenulinksRepository.getAllDataSupAd();
			for (Object[] obj : menuList) {
				JSONObject jsonObject = new JSONObject();
				jsonObject.put("intId", (Integer) obj[0]);
				jsonObject.put("intLinkType", (Integer) obj[1]);
				jsonObject.put("vchLinkName", obj[2].toString());
				jsonObject.put("intParentLinkId", (Integer) obj[3]);
				jsonObject.put("vchViewUrl", obj[5].toString());
				jsonObject.put("moduleName", obj[6].toString());
				makeArray.put(jsonObject);
			}
		} else {

			if (!rolePermission.isEmpty() && isAdmin != 2) {

				List<Object[]> menuList = tmenulinksRepository.getDataListByUserId(userId);
				for (Object[] obj : menuList) {
					JSONObject jsonObject = new JSONObject();
					jsonObject.put("intId", (Integer) obj[0]);
					jsonObject.put("vchViewUrl", obj[3].toString());
					jsonObject.put("intLinkType", (Integer) obj[4]);
					jsonObject.put("intParentLinkId", (Integer) obj[5]);
					jsonObject.put("trIntId", (Integer) obj[6]);
					jsonObject.put("moduleName", obj[9].toString());
					
					if(obj[9].toString().equals("Approval Application")) {
						if(roleId==2 ||roleId==10||roleId==42) {
							jsonObject.put("vchLinkName", "Assign/Review Complaint");
						}
						else {
							jsonObject.put("vchLinkName", "Review Complaint");
						}
					}
					else {
						jsonObject.put("vchLinkName", obj[1].toString());
					}
					
					
					makeArray.put(jsonObject);
				}
			} else {
				List<Object[]> menuList = tmenulinksRepository.getDataFromLinksandPermission(roleId);
				for (Object[] obj : menuList) {
					JSONObject jsonObject = new JSONObject();
					jsonObject.put("intId", (Integer) obj[0]);
					//jsonObject.put("vchLinkName", obj[1].toString());
					jsonObject.put("vchViewUrl", obj[3].toString());
					jsonObject.put("intLinkType", (Integer) obj[4]);
					jsonObject.put("intParentLinkId", (Integer) obj[5]);
					jsonObject.put("trIntId", (Integer) obj[6]);
					jsonObject.put("moduleName", obj[9].toString());
					if(obj[9].toString().equals("Approval Application")) {
						if(roleId==2 ||roleId==10||roleId==42) {
							jsonObject.put("vchLinkName", "Assign/Review Complaint");
						}
						else {
							jsonObject.put("vchLinkName", "Review Complaint");
						}
					}
					else {
						jsonObject.put("vchLinkName", obj[1].toString());
					}
					makeArray.put(jsonObject);
				}

			}

		}
		return makeArray;
	}

	@Override
	public JSONObject getAllFormList() {
		List<Tmenulinks> tMenuLinksList = tmenulinksRepository
				.findAllByBitDeletedFlagAndIntApplicableForAndSelLinkTypeOrderByDtmCreatedOnDesc(false, 2, 2);
		JSONObject json = new JSONObject();
		json.put("result", new JSONArray(tMenuLinksList));
		json.put("status", 200);
		return json;
	}
}
