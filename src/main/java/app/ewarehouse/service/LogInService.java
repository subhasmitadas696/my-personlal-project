package app.ewarehouse.service;

import app.ewarehouse.dto.LoginDto;
import org.json.JSONObject;
public interface LogInService {
	
	public JSONObject loinCheck(LoginDto LoginDto);
	JSONObject validateUserIdEmail(String decode);

	JSONObject validateTime(String decode);

	JSONObject saveforgetpasswordchange(String decode);

}
