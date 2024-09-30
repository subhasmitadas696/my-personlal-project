package app.ewarehouse.serviceImpl;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.text.SimpleDateFormat;

import java.util.Arrays;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.ArrayList;
import org.json.JSONArray;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import app.ewarehouse.entity.ComplaintMgmtInspScheduleDocs;
import app.ewarehouse.entity.OnlineServiceApprovalNotings;
 import app.ewarehouse.entity.TOnlineServiceApproval;
 import app.ewarehouse.entity.TOnlineServiceQueryDocument;
 import app.ewarehouse.entity.TSetAuthority;
import app.ewarehouse.repository.ComplaintMgmtInspScheduleDocsRepository;
import app.ewarehouse.repository.OnlineServiceApprovalNotingsRepository;
 import app.ewarehouse.repository.TOnlineServiceApprovalRepository;
 import app.ewarehouse.repository.TOnlineServiceQueryDocumentRepository;
 import app.ewarehouse.repository.TSetAuthorityRepository;
 import app.ewarehouse.repository.TmenulinksRepository;
 import app.ewarehouse.service.TOnlineServiceApprovalService;
 import app.ewarehouse.util.FnAuthority;
 import app.ewarehouse.repository.TuserRepository;
 import app.ewarehouse.entity.Tuser;
 import app.ewarehouse.util.CommonUtil;
@Service
public class TOnlineServiceApprovalServiceImpl implements TOnlineServiceApprovalService {

	@Autowired
	private TOnlineServiceApprovalRepository tOnlineServiceApprovalRepo;
	@Autowired
	TSetAuthorityRepository tsetAthorityRepository;

	@Autowired
	private FnAuthority fnAuthority;
	@Autowired

	private TmenulinksRepository tmenulinksRepository;

	@Autowired
	private TOnlineServiceQueryDocumentRepository tOnlineServiceQueryDocumentRepository;

	@Autowired
	private OnlineServiceApprovalNotingsRepository onlineServiceApprovalNotingsRepository;

	@Autowired
	private TuserRepository tuserRepository;
	
	@Autowired
	private ComplaintMgmtInspScheduleDocsRepository docRepo;
	@Value("${tempUpload.path}")
	private String tempUploadPath;

	@Value("${queryFileUpload.path}")
	private String queryFileUploadPath;
	JSONObject respdata = new JSONObject();
	JSONObject getAuthArr = new JSONObject();
	JSONObject rdoGender = new JSONObject("{1:M,2:F}");

	@Override
	public JSONObject getTakeActionDetails(String data) {

		JSONObject jsonObj = new JSONObject(data);
		Integer serviceId = jsonObj.getInt("onlineServiceId");
		Integer roleId = jsonObj.getInt("roleId");
		Integer userId = jsonObj.getInt("intuserId");
		Integer intId = jsonObj.getInt("intId");
		Integer stageNo = jsonObj.getInt("stageNo");
		Integer labelId=Integer.parseInt(jsonObj.getString("labelId"));

		String status = "200";
		String outMsg = "Success!!";

		JSONObject arrActionDetails = new JSONObject();
		JSONArray fwdActionDetails = new JSONArray();

		List<Map<String, Object>> actionList = tOnlineServiceApprovalRepo.getDataByproceeIdAndroleIdAndStageNo(intId,
				stageNo, roleId, serviceId,labelId);

		if (!actionList.isEmpty()) {
			for (Map<String, Object> mapData : actionList) {
				JSONObject actionDetails = new JSONObject();
				actionDetails.put("intSetAuthId", (Integer) mapData.get("intSetAuthId"));
				actionDetails.put("intPrimaryAuth", (Integer) mapData.get("intPrimaryAuth"));
				actionDetails.put("jsnVerifyDocument", mapData.get("jsnVerifyDocument"));
				actionDetails.put("vchAuthTypes", Arrays.asList(mapData.get("vchAuthTypes").toString().split(",")));
				actionDetails.put("tinStatus", (Integer) mapData.get("tinStatus"));
				arrActionDetails.put("actionDetails", actionDetails);
			}
		} else {
			List<TOnlineServiceApproval> allForwardActions = tOnlineServiceApprovalRepo
					.getAllForwardActionData(serviceId, roleId, intId, userId);

			if (allForwardActions != null) {
				for (TOnlineServiceApproval resultTab : allForwardActions) {

					JSONObject fwdAction = new JSONObject();

					fwdAction.put("tinStatus", resultTab.getTinStatus());
					fwdAction.put("vchForwardAllAction", Arrays.asList(resultTab.getVchForwardAllAction().split(",")));
					fwdAction.put("intForwardedUserId", resultTab.getIntForwardedUserId());

					arrActionDetails.put("actionDetails", fwdAction);
				}

			}
		}

		JSONObject resultMap = new JSONObject();
		resultMap.put("status", status);
		resultMap.put("msg", outMsg);
		resultMap.put("result", arrActionDetails);

		return resultMap;
	}

	@Override
	public JSONObject getAllNotingDetails(String data) {
		JSONObject jsonObj = new JSONObject(data);
		Integer processId = jsonObj.getInt("intId");
		Integer serviceId = jsonObj.getInt("onlineServiceId");
		Integer roleId = jsonObj.getInt("roleId");
		Integer lableId = jsonObj.getInt("lableId");
		JSONArray notingDetails = new JSONArray();
		List<Object[]> onlineServiceApprovalNotings = onlineServiceApprovalNotingsRepository.getAllDetailsByServiceIdProcessId(serviceId, processId);
		System.out.println("onlineServiceApprovalNotings++++++++++" + onlineServiceApprovalNotings);
		for (Object[] obj : onlineServiceApprovalNotings) {
			JSONObject newObj = new JSONObject();

			Integer resAPPId=Integer.parseInt(obj[0].toString());
			Integer resRoleId=Integer.parseInt(obj[1].toString());
			
			newObj.put("txtNoting", obj[3]);
			newObj.put("dtActionTaken", obj[4]);
			newObj.put("vchActionName", obj[7]);
			newObj.put("vchRolename", obj[8]);
			newObj.put("vchDocumentFile", obj[9]);
			newObj.put("vchDocumentName", obj[10]);
			Integer tintStatus = Integer.parseInt(obj[11].toString());
			newObj.put("tinStatus", tintStatus);
			if (tintStatus == 4) {
				newObj.put("userName", obj[12].toString());
			}
			if(obj[13] != null) {
			Integer docId = Integer.parseInt(obj[13].toString());
			ComplaintMgmtInspScheduleDocs doc = docRepo.findById(docId).get();
			newObj.put("docName", doc.getDocPath());
			}
			Integer stageNo=Integer.parseInt(obj[14].toString());
			newObj.put("intStageNo", stageNo);
			/*
			 * else { newObj.put("docName", "Document is not Available"); }
			 */
			List<Object[]> obervationResponse = onlineServiceApprovalNotingsRepository.observationResponse(resRoleId,resAPPId,lableId,stageNo);
			JSONArray resDetails = new JSONArray();
			if(obervationResponse!=null) {
				
				for(Object[] obj1 :obervationResponse) {
					JSONObject newObj1 = new JSONObject();
					newObj1.put("ObsQu", obj1[0]);
					newObj1.put("ObsRes", obj1[1]);	
					resDetails.put(newObj1);
				}
			}
			
			newObj.put("objResp", resDetails);
			
			notingDetails.put(newObj);
		}

		JSONObject jsonObject = new JSONObject();
		jsonObject.put("result", notingDetails);
		jsonObject.put("status", 200);
		jsonObject.put("outMsg", "success!!");
		return jsonObject;
	}

	@Override
	public JSONObject getQueryDetailsByUsingData(String data) {
		JSONObject jsObj = new JSONObject(data);
		String outMsg = "";
		String status = "";
		JSONObject newData = new JSONObject();
		JSONObject responseData = new JSONObject();
		try {
			Integer intProcessId = jsObj.getInt("intProcessId");
			Integer intOnlineServiceId = jsObj.getInt("intOnlineServiceId");
			OnlineServiceApprovalNotings queryData = onlineServiceApprovalNotingsRepository
					.getDataBYUsingprocessIdAndServiceId(intProcessId, intOnlineServiceId);

			newData.put("txtNoting", queryData.getTxtNoting());
			newData.put("dtActionTaken",
					new SimpleDateFormat("dd MMM yyyy hh.mm a").format(queryData.getDtActionTaken()));
			newData.put("schemeName", tmenulinksRepository.getVchLinkNameByUsingintProcessId(intProcessId));

			status = "200";
			outMsg = "Success !!!";

		} catch (Exception e) {
			e.printStackTrace();
			status = "400";
			outMsg = "Something Went Wrong !!!!";
		}

		responseData.put("status", status);
		responseData.put("result", newData);
		responseData.put("outMsg", outMsg);

		return responseData;
	}

	@Override
	public JSONObject saveIntoQueryDetails(JSONObject jsObj) {

		JSONObject jsObject = new JSONObject();
		String status = "";
		String outMsg = "";
		try {
			Integer intId = jsObj.getInt("processId");
			Integer onlineServiceId = jsObj.getInt("serviceId");
			String remark = jsObj.getString("remark");
			Long currentTime = System.currentTimeMillis();
			Date date = new Date(currentTime);
			TOnlineServiceApproval onlineServiceApproval = tOnlineServiceApprovalRepo.findByIntProcessIdAndIntOnlineServiceIdAndBitDeletedFlag(intId, onlineServiceId,102);
			System.out.println("onlineServiceApproval" + onlineServiceApproval);

			OnlineServiceApprovalNotings notings = onlineServiceApprovalNotingsRepository.getDataBYUsingprocessIdAndServiceId(intId, onlineServiceId);

			onlineServiceApproval.setTinStatus(9);
			onlineServiceApproval.setDtmStatusDate(date);
			onlineServiceApproval.setIntPendingAt(onlineServiceApproval.getVchATAAuths());
			onlineServiceApproval.setVchATAAuths("0");
			onlineServiceApproval.setIntStageNo(notings.getTinStageCtr());

			tOnlineServiceApprovalRepo.save(onlineServiceApproval);

			Integer action = onlineServiceApproval.getTinStatus();

			OnlineServiceApprovalNotings insertNoting = new OnlineServiceApprovalNotings();
			insertNoting.setIntOnlineServiceId(onlineServiceId);
			insertNoting.setIntProcessId(intId);
			insertNoting.setIntFromAuthority(0);
			insertNoting.setIntToAuthority(String.valueOf(notings.getTinStageCtr()));
			insertNoting.setDtActionTaken(date);
			insertNoting.setIntStatus(action);
			insertNoting.setTxtNoting(remark);
			insertNoting.setTinStageCtr(0);

			OnlineServiceApprovalNotings onlineServiceApprovalNotings = onlineServiceApprovalNotingsRepository
					.saveAndFlush(insertNoting);

			JSONArray jsonArray = new JSONArray(jsObj.getString("dynamicListArr"));
			String comString = jsObj.getString("arrUploadedFiles").replace("\"", "");

			String[] splitString = comString.replace("[", "").replace("]", "").split(",");

			for (int i = 0; i < jsonArray.length(); i++) {
				JSONObject objjs = jsonArray.getJSONObject(i);
				TOnlineServiceQueryDocument queryDetails = new TOnlineServiceQueryDocument();
				queryDetails.setIntNotingId(onlineServiceApprovalNotings.getIntNotingsId());
				queryDetails.setIntOnlineServiceId(onlineServiceId);
				queryDetails.setVchDocumentName(objjs.getString("vchDocumentName"));
				queryDetails.setVchDocumentFile(splitString[i]);
				tOnlineServiceQueryDocumentRepository.save(queryDetails);
			}
			status = "200";
			outMsg = "Success!!!!";

			for (int i = 0; i < splitString.length; i++) {
				String fileUpload = splitString[i];
				if (!fileUpload.equals("")) {
					File f = new File(tempUploadPath + fileUpload);
					if (f.exists()) {
						File src = new File(tempUploadPath + fileUpload);
						File dest = new File(queryFileUploadPath + fileUpload);
						try {
							Files.copy(src.toPath(), dest.toPath(), StandardCopyOption.REPLACE_EXISTING);
							Files.delete(src.toPath());
						} catch (IOException e) {
							System.out.println("Iniside Error");
						}
					}
				}
			}

		} catch (Exception e) {
			e.printStackTrace();
			status = "400";
			outMsg = "Something went wrong!!!!";
		}

		// file shift is pending---------
		jsObject.put("status", status);
		jsObject.put("outMsg", outMsg);
		return jsObject;
	}

	@Override
	public JSONObject noResubmitStatus(String data) {
		JSONObject jsonObject = new JSONObject(data);
		List<Object[]> getResubmitHistory = onlineServiceApprovalNotingsRepository
				.getResubmitHistoryByOnlineServiceIdAndProcessId(jsonObject.getInt("processId"),
						jsonObject.getInt("serviceId"));
		JSONArray resubmitHsitory = new JSONArray();
		for (Object[] obj : getResubmitHistory) {
			resubmitHsitory.put(new JSONObject().put("historyId", obj[0]).put("intFromAuthority", obj[1])
					.put("intToAuthority", obj[2]).put("dtActionTaken", obj[3]).put("txtNoting", obj[4])
					.put("roleName", obj[5].toString()).put("serviceId", jsonObject.getInt("serviceId")));
		}
		JSONObject response = new JSONObject();
		response.put("status", 200);
		response.put("result", resubmitHsitory);
		return response;
	}

	@Override
	public JSONObject getAllForwradAuthority(String decodedStr) {
		JSONObject response = new JSONObject();
		try {
			JSONObject data = new JSONObject(decodedStr);
			Integer roleId = Integer.parseInt(data.get("intRoleId").toString());
			List<Map<String, Object>> userList = tuserRepository.findUserListByRoleId(roleId);
			JSONArray jsArr = new JSONArray();
			if (!userList.isEmpty()) {
				for (Map<String, Object> tuser : userList) {
					JSONObject jsb = new JSONObject();
					jsb.put("intUserId", (Integer) tuser.get("intId"));
					jsb.put("userName", tuser.get("vchFullName").toString());
					jsb.put("roleName", tuser.get("vchRoleName").toString());
					jsArr.put(jsb);
				}
				response.put("result", jsArr);
				response.put("status", 200);

			} else {
				response.put("status", 417);
			}

		} catch (Exception e) {
			response.put("status", 400);
			e.printStackTrace();
			// logger.error("Inside getAllForwradAuthority method of WorkflowServiceImpl
			// error occur " + e);
		}
		return response;
	}

}
