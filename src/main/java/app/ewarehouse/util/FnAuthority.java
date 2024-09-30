package app.ewarehouse.util;

import app.ewarehouse.repository.TSetAuthorityRepository;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Objects;

@Component
public class FnAuthority {
	private static TSetAuthorityRepository tSetAuthorityRepository;

	@Autowired
	public FnAuthority(TSetAuthorityRepository tSetAuthorityRepository) {
		FnAuthority.tSetAuthorityRepository = tSetAuthorityRepository;
	}

	/*
	 * public JSONObject getAuthority(Integer processId, Integer stageNo) {
	 * 
	 * int actualStageNo = stageNo; String calcresult = ""; JSONObject responseJson
	 * = new JSONObject(); try {
	 * 
	 * List<Map<String, Object>> setAuthorityResult = tSetAuthorityRepository
	 * .getAllDatByStageNoAndIntProcessd(actualStageNo, processId);
	 * 
	 * List<Integer> pendingAuths = new ArrayList<>(); for (Map<String, Object>
	 * value : setAuthorityResult) {
	 * 
	 * if (!Objects.isNull(value.get("intATAProcessId"))) { int intATAProcessId =
	 * (int) value.get("intATAProcessId");
	 * 
	 * if (intATAProcessId == 1) { responseJson.put("stageNo", actualStageNo);
	 * responseJson.put("intATAProcessId", intATAProcessId);
	 * responseJson.put("pendingAuthorities", value.get("intPrimaryAuth")); } else {
	 * responseJson.put("stageNo", actualStageNo);
	 * responseJson.put("intATAProcessId", intATAProcessId);
	 * responseJson.put("pendingAuthorities", value.get("intPrimaryAuth")); }
	 * 
	 * }
	 * 
	 * }
	 * 
	 * } catch (Exception e) { e.printStackTrace(); } return responseJson; // Return
	 * an empty JSON object if no results are found }
	 */
	
	
	public JSONObject getAuthority(Integer processId, Integer stageNo,Integer intLabelId) {

		int actualStageNo = stageNo;
		String calcresult = "";
		JSONObject responseJson = new JSONObject();
		try {

			List<Map<String, Object>> setAuthorityResult = tSetAuthorityRepository.getAllDatByStageNoAndIntProcessd(actualStageNo, processId,intLabelId);

			List<Integer> pendingAuths = new ArrayList<>();
			for (Map<String, Object> value : setAuthorityResult) {

				if (!Objects.isNull(value.get("intATAProcessId"))) {
					int intATAProcessId = (int) value.get("intATAProcessId");

					if (intATAProcessId == 1) {
						responseJson.put("stageNo", actualStageNo);
						responseJson.put("intATAProcessId", intATAProcessId);
						responseJson.put("pendingAuthorities", value.get("intPrimaryAuth"));
					} else {
						responseJson.put("stageNo", actualStageNo);
						responseJson.put("intATAProcessId", intATAProcessId);
						responseJson.put("pendingAuthorities", value.get("intPrimaryAuth"));
					}

				}

			}

		} catch (Exception e) {
			e.printStackTrace();
		}
		return responseJson; // Return an empty JSON object if no results are found
	}
	
	
	public JSONObject getAuthorityByCondition(Integer processId, Integer stageNo,Integer intLabelId,String conditionAction) {

		int actualStageNo = stageNo;
		String calcresult = "";
		JSONObject responseJson = new JSONObject();
		try {

			List<Map<String, Object>> setAuthorityResult = tSetAuthorityRepository.getAllDatByStageNoAndIntProcessdBasedOnCondition(actualStageNo, processId,intLabelId,conditionAction);

			List<Integer> pendingAuths = new ArrayList<>();
			for (Map<String, Object> value : setAuthorityResult) {

				if (!Objects.isNull(value.get("intATAProcessId"))) {
					int intATAProcessId = (int) value.get("intATAProcessId");

					if (intATAProcessId == 1) {
						responseJson.put("stageNo", actualStageNo);
						responseJson.put("intATAProcessId", intATAProcessId);
						responseJson.put("pendingAuthorities", value.get("intPrimaryAuth"));
					} else {
						responseJson.put("stageNo", actualStageNo);
						responseJson.put("intATAProcessId", intATAProcessId);
						responseJson.put("pendingAuthorities", value.get("intPrimaryAuth"));
					}

				}

			}

		} catch (Exception e) {
			e.printStackTrace();
		}
		return responseJson; // Return an empty JSON object if no results are found
	}
	
	
	
}
