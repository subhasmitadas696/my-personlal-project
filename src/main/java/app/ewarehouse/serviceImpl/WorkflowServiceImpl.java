package app.ewarehouse.serviceImpl;

import app.ewarehouse.entity.TSetAuthority;
import app.ewarehouse.entity.TSetWorkFlow;
import app.ewarehouse.repository.TSetAuthorityRepository;
import app.ewarehouse.repository.TSetWorkFlowRepository;
import app.ewarehouse.service.WorkflowService;
import org.apache.commons.text.StringEscapeUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCreator;
import org.springframework.jdbc.core.RowCallbackHandler;
import org.springframework.jdbc.support.GeneratedKeyHolder;
import org.springframework.jdbc.support.KeyHolder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigInteger;
import java.sql.Date;
import java.sql.*;
import java.util.*;
@Service
public class WorkflowServiceImpl implements WorkflowService {
	@Autowired
	TSetAuthorityRepository tSetAuthorityRepository;
	@Autowired
	private JdbcTemplate jdbcTemplate;
	
	@Autowired
	private TSetWorkFlowRepository tSetWorkFlowRepository;
	
	String updateTSetWorkFlow;
	String updateTSetAuthority;

	
	@Override
	public List<Map<String, Object>> getallApprovalAction() {
		try {
			List<Map<String, Object>> myList = new ArrayList<Map<String, Object>>();
			JSONArray array = new JSONArray();
			jdbcTemplate.query(
					" select tinApprovalActionId \"tinApprovalActionId\",vchActionName \"vchActionName\" from m_approval_actions where bitDeletedFlag = 0 ",
					new RowCallbackHandler() {
						public void processRow(ResultSet resultSet) throws SQLException {
							JSONObject object = null;
							do {

								try {
									Map<String, Object> map = new HashMap<String, Object>();

									map.put("tinApprovalActionId", resultSet.getInt("tinApprovalActionId"));
									map.put("vchActionName", resultSet.getString("vchActionName"));
									myList.add(map);

								} catch (Exception e) {
									e.printStackTrace();
								}

							} while (resultSet.next());

						}
					});
			return myList;
		} catch (Exception e) {
			e.printStackTrace();
			JSONObject response = new JSONObject();
			response.put("status", "400");
			response.put("msg", "error");
			return (List<Map<String, Object>>) response;
		}
	

	



}
	
	@Override
	public List<Map<String, Object>> getallOfficersApi() {
		try {
			List<Map<String, Object>> myList = new ArrayList<Map<String, Object>>();
			JSONArray array = new JSONArray();

			jdbcTemplate.query(
					"SELECT intId AS intRoleId, vchRolename AS vchRoleName FROM m_admin_role"
					+ " WHERE bitDeletedFlag = 0 ORDER BY intId ASC",

					new RowCallbackHandler() {
						public void processRow(ResultSet resultSet) throws SQLException {
							JSONObject object = null;
							do {

								try {
									Map<String, Object> map = new HashMap<String, Object>();
									map.put("intRoleId", resultSet.getInt("intRoleId"));
									map.put("vchRoleName", resultSet.getString("vchRoleName"));
									myList.add(map);

								} catch (Exception e) {
									e.printStackTrace();
								}

							} while (resultSet.next());

						}
					});
			return myList;
		} catch (Exception e) {
			e.printStackTrace();
			JSONObject response = new JSONObject();
			response.put("status", "400");
			response.put("msg", "error");
			return (List<Map<String, Object>>) response;
		}

	}
	
	@Transactional
	@Override

	public String setWorkflow(String setWorkflow) {

		try {
			

			long millis = System.currentTimeMillis();
			JSONObject workFlowReq = new JSONObject(setWorkflow);
			byte[] requestData = Base64.getDecoder().decode(workFlowReq.getString("REQUEST_DATA"));
			JSONObject workFlowRequest = new JSONObject(new String(requestData));
			
			JSONArray stageData = null;
			String response = null;
			KeyHolder keyHolder2 = new GeneratedKeyHolder();
			KeyHolder keyHolder3 = new GeneratedKeyHolder();
			KeyHolder keyHolder4 = new GeneratedKeyHolder();

			String arrays = workFlowRequest.get("stageData").toString();
			if (workFlowRequest.has("stageData")) {
				stageData = new JSONArray(arrays);
			}

			try {
				String escapedHTML = workFlowRequest.getString("drawData");
				String ctrlId = workFlowRequest.getString("dynFilterCtrlId");
				String dynFilterDetails = workFlowRequest.getString("dynFilterDetails");
				Integer workFlowCount = 0;
				Integer authorityCount = 0;
				if (dynFilterDetails.length() > 0 && ctrlId.length() > 0) {
					workFlowCount = tSetWorkFlowRepository.countFilterData(workFlowRequest.getInt("serviceId"),
							dynFilterDetails, ctrlId);
				}

				updateTSetWorkFlow = " UPDATE t_set_workflow SET deletedFlag = 1 WHERE serviceId= "
						+ workFlowRequest.getInt("serviceId") +" and intLabelId = "+ workFlowRequest.getInt("workFlowControlId");

				if (workFlowCount > 0) {
					if (workFlowRequest.getString("dynFilterDetails").length() > 0) {
						updateTSetWorkFlow += " and vchDynFilter= '" + workFlowRequest.getString("dynFilterDetails")
								+ "'";
					}
				} else if (ctrlId.length() > 0) {
					updateTSetWorkFlow += " and vchDynFilterCtrlId != '" + ctrlId + "'";
				}

				KeyHolder keyHolder = new GeneratedKeyHolder();
				jdbcTemplate.update(new PreparedStatementCreator() {
					public PreparedStatement createPreparedStatement(Connection connection) throws SQLException {
						PreparedStatement ps = null;
						try {
							ps = connection.prepareStatement(updateTSetWorkFlow, new String[] { "workflowId" });
						} catch (SQLException | JSONException e) {
							e.printStackTrace();
							JSONObject response = new JSONObject();
							response.put("status", "400");
							response.put("msg", "error");
							return (PreparedStatement) response;
						}
						return ps;
					}
				}, keyHolder);

				

				if (dynFilterDetails.length() > 0 && ctrlId.length() > 0) {
					authorityCount = tSetAuthorityRepository
							.countFilterDataByCtrlId(workFlowRequest.getInt("serviceId"), dynFilterDetails, ctrlId);
				}
				updateTSetAuthority = " UPDATE t_set_authority SET bitDeletedFlag = 1 WHERE intProcessId =  "
						+ workFlowRequest.getInt("serviceId") +" and intLabelId = "+ workFlowRequest.getInt("workFlowControlId");

				if (authorityCount > 0) {
					if (workFlowRequest.getString("dynFilterDetails").length() > 0) {
						updateTSetAuthority += " and vchDynFilter= '" + workFlowRequest.getString("dynFilterDetails")
								+ "'";
					}
				} else if (ctrlId.length() > 0) {
					updateTSetAuthority += " and vchDynFilterCtrlId!= '" + ctrlId + "'";
				}

				jdbcTemplate.update(new PreparedStatementCreator() {
					public PreparedStatement createPreparedStatement(Connection connection) throws SQLException {
						PreparedStatement ps = connection.prepareStatement(updateTSetAuthority,
								new String[] { "INTSETAUTHID" });
						try {
						} catch (Exception e) {
							e.printStackTrace();
						}
						return ps;
					}
				}, keyHolder4);

				

				TSetWorkFlow tSetWorkflow = new TSetWorkFlow();
				tSetWorkflow.setProjectId(workFlowRequest.getInt("projectId"));
				tSetWorkflow.setServiceId(workFlowRequest.getInt("serviceId"));
				tSetWorkflow.setIntLabelId(workFlowRequest.getInt("workFlowControlId"));
				
				tSetWorkflow.setCanvasData(escapedHTML);
				tSetWorkflow.setDeletedFlag(0);
				tSetWorkflow.setCreatedBy(20);
				tSetWorkflow.setCreatedOn(new Date(millis));
			
				for (int i = 0; i <= stageData.length() - 1; i++) {
					JSONObject object = stageData.getJSONObject(i);
					if (object.getString("authActions").toString() != "") {
						tSetWorkflow.setVchMailSmsConfigIds(workFlowRequest.get("applicantMailSmsDetails").toString());
					}

				}
				
				if (workFlowRequest.getString("dynFilterDetails").length() > 0) {
					tSetWorkflow.setVchDynFilter(workFlowRequest.getString("dynFilterDetails"));
				}
				TSetWorkFlow tSet = tSetWorkFlowRepository.save(tSetWorkflow);
				
				String authorityId = "";
				if (workFlowRequest.getString("dynFilterCtrlId").length() > 0) {
					tSetWorkflow.setVchDynFilterCtrlId(workFlowRequest.getString("dynFilterCtrlId"));
				} else {
					tSetWorkflow.setVchDynFilterCtrlId("0");
				}
				tSetWorkFlowRepository.save(tSetWorkflow);

				for (int i = 0; i <= stageData.length() - 1; i++) {
					JSONObject object = stageData.getJSONObject(i);
					long random = System.currentTimeMillis();
					String randomNum = String.valueOf(random) + "" + String.valueOf(object.getInt("tinStageNo")) + ""
							+ String.valueOf(i);
					  System.out.println(object);
					BigInteger randomInt = new BigInteger(randomNum);
					TSetAuthority authority = new TSetAuthority();
					authority.setIntHierarchyId(0);
					authority.setIntSetAuthId(0);
					authority.setIntSetAuthLinkId(randomInt.intValue());
					authority.setIntSetLetterLinkId(randomInt.intValue());
					authority.setDtmCreatedOn(new Date(millis));
					authority.setIntATAProcessId(object.getInt("parallel"));
					authority.setIntCreatedBy(20);
					authority.setIntPrimaryAuth(object.getInt("roleId"));
					authority.setIntProcessId(object.getInt("selProcess"));
					authority.setIntProjectId(object.getInt("selProject"));
					authority.setIntRoleId(object.getInt("roleId"));
					authority.setIntLabelId(workFlowRequest.getInt("workFlowControlId"));
					authority.setTinStageNo(object.getInt("tinStageNo"));
					authority.setTinTimeLine(object.getInt("timeline"));
					if (object.has("authLetters") && object.getString("authLetters").toString() != "") {
						authority.setAuthLetters(object.getString("authLetters").toString());
					}
					if (object.has("authMailSmsDetails") && object.getString("authActions").toString() != "") {
						authority.setVchMailSmsConfigIds(object.getString("authMailSmsDetails").toString());
					}
				
					authority.setVchAuthTypes(object.getString("authActions").toString());

					String s = (object.get("approvalDocuments").toString().length() > 0)
							? object.get("approvalDocuments").toString()
							: "";
					authority.setJsnApprovalDocument(s);

					authority.setTinCalcStatus(0);
					authority.setVchParentNode(object.getString("vchParentNodes"));
					authority.setTinCurrentNode(object.getInt("tinCurrentNode"));
					if (workFlowRequest.getString("dynFilterDetails").length() > 0) {
						authority.setVchDynFilter(workFlowRequest.getString("dynFilterDetails"));
					}
					System.out.println("Id---" + workFlowRequest.getString("dynFilterCtrlId"));
					if (workFlowRequest.getString("dynFilterCtrlId").length() > 0) {
						authority.setVchDynFilterCtrlId(workFlowRequest.getString("dynFilterCtrlId"));
					} else {
						authority.setVchDynFilterCtrlId("0");
					}
					authority.setBitDeletedFlag((byte)0);

					TSetAuthority saveAuthority = tSetAuthorityRepository.save(authority);
					authorityId = authorityId + saveAuthority.getIntSetAuthId();
				}
				
			}

			catch (Exception e) {
				e.printStackTrace();
			}
			response = "success";

			return response;
		} catch (Exception e) {
			e.printStackTrace();
			JSONObject response = new JSONObject();
			response.put("status", "400");
			response.put("msg", "error");
			return response.toString();
		}

	}

	@Override
	public List<Map<String, Object>> getFormName() {
		try {
			List<Map<String, Object>> myList = new ArrayList<Map<String, Object>>();
			JSONArray array = new JSONArray();

			jdbcTemplate.query(
					"SELECT intId, vchLinkName as vchProcessName FROM m_admin_menulinks"
							+ " WHERE bitDeletedFlag = 0 AND intLinkType=2 AND intForApproval=1 ORDER BY vchLinkName ASC",
					new RowCallbackHandler() {
						public void processRow(ResultSet resultSet) throws SQLException {
							JSONObject object = null;
							do {

								try {
									Map<String, Object> map = new HashMap<String, Object>();
									map.put("intId", resultSet.getInt("intId"));
									map.put("vchProcessName", resultSet.getString("vchProcessName"));
									myList.add(map);

								} catch (Exception e) {
									e.printStackTrace();
								}

							} while (resultSet.next());

						}
					});
			return myList;
		} catch (Exception e) {
			e.printStackTrace();
			JSONObject response = new JSONObject();
			response.put("status", "400");
			response.put("msg", "error");
			return (List<Map<String, Object>>) response;
		}

	}
	
	@Override
	public JSONObject fillWorkflow(String serviceId,Integer workFlowControlId, String dynFilterDetails) {
		try {

			System.out.println("dyn value:" + dynFilterDetails);
			System.out.println("length" + dynFilterDetails.length());

			JSONObject object = new JSONObject();

			String queryFillWorkFlow = " select canvasData from t_set_workflow where deletedFlag = 0 and serviceId= "
					+ serviceId +" and intLabelId = "+workFlowControlId;
			if (dynFilterDetails.length() > 0) {
				queryFillWorkFlow += " and vchDynFilter= '" + dynFilterDetails + "'";
			}
			

			jdbcTemplate.query(queryFillWorkFlow, new RowCallbackHandler() {
				public void processRow(ResultSet resultSet) throws SQLException {

					do {

						try {
							String unEscapedHTML = StringEscapeUtils.unescapeHtml4(resultSet.getString("canvasData"));
							object.put("result",unEscapedHTML );

						} catch (Exception e) {
							e.printStackTrace();
						}

					} while (resultSet.next());

				}

			});

			return object;
		} catch (Exception e) {
			e.printStackTrace();
			JSONObject response = new JSONObject();
			response.put("status", "400");
			response.put("msg", "error");
			return response;
		}
	}




}
