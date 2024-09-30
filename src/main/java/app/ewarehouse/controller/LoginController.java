package app.ewarehouse.controller;


import java.io.UnsupportedEncodingException;
import java.net.URLDecoder;
import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

import org.json.JSONArray;
import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import app.ewarehouse.dto.JwtAuthenticationResponse;
import app.ewarehouse.dto.LoginDto;
import app.ewarehouse.dto.ResponseDTO;
import app.ewarehouse.dto.SigninRequest;
import app.ewarehouse.dto.UserDataResponseDto;
import app.ewarehouse.entity.TempUser;
import app.ewarehouse.entity.Tuser;
import app.ewarehouse.service.AuthenticationService;
import app.ewarehouse.service.LogInService;
import app.ewarehouse.service.TuserService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.TokenCreaterAndMatcher;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@RestController
@CrossOrigin(origins = "*")
public class LoginController {

    @Autowired
    LogInService service;
    @Autowired
    public AuthenticationService authenticationService;
    @Autowired
    TuserService userService;
    
    @Autowired
	private ObjectMapper objectMapper;
    
    @Value("${salt}")
    private String salt;
    
    private static final Logger logger = LoggerFactory.getLogger(LoginController.class);

    @PostMapping(value = "/logindetails")
    public ResponseEntity<?> logindetails(@RequestBody String democsmform, HttpServletRequest request, HttpServletResponse reponse) {
        logger.info("Inside logindetails method of LoginController");
        JSONObject requestObj = new JSONObject(democsmform);
        JSONObject output = new JSONObject();
        JSONArray jsonarr = new JSONArray();
        if (CommonUtil.hashRequestMatch(requestObj.getString("REQUEST_DATA"), requestObj.getString("REQUEST_TOKEN"))) {
            String data = CommonUtil.inputStreamDecoder(democsmform);
            JSONObject json = new JSONObject(data);
            LoginDto loginDto = new LoginDto();
            loginDto.setUserId(json.getString("userId"));
            loginDto.setUserPassword(json.getString("userPassword"));
            output = service.loinCheck(loginDto);


            String user_type = (String) request.getHeader("x-user-type");
            SigninRequest siginReq = null;
			siginReq = SigninRequest.builder().email("ewrs@mail.com").password("ewrs").build();


            /*
            if ("ADMIN".equalsIgnoreCase(user_type)) {
                siginReq =
                        SigninRequest.builder().email("admin@gmail.com").password("admin").build();
            } else if ("USER".equalsIgnoreCase(user_type)) {
                siginReq =
                        SigninRequest.builder().email("ewrs@mail.com").password("ewrs").build();
            }
*/
                       String result=output.get("result").toString();
                       Map<String, String> claims=null;
                       try {
						 claims = new ObjectMapper().readValue(result, HashMap.class);
					} catch (JsonMappingException e) {
						// TODO Auto-generated catch block
						 logger.error("Inside logindetails method of LoginController token not matched");
					} catch (JsonProcessingException e) {
						// TODO Auto-generated catch block
						 logger.error("Inside logindetails method of LoginController token not matched");
					}
			
            JwtAuthenticationResponse jwtres = authenticationService.signin(siginReq,claims);

            JSONObject token_jsonObject = new JSONObject();
            token_jsonObject.put("token", jwtres.getToken());
            token_jsonObject.put("refreshtoken", jwtres.getRefreshToken());
            
        
            

            JSONObject login_jsonobj = CommonUtil.inputStreamEncoder(output.toString());


            jsonarr.put(login_jsonobj);
            jsonarr.put(token_jsonObject);

        } else {
            logger.error("Inside logindetails method of LoginController token not matched");
            output.put("msg", "error");
            output.put("status", 417);
        }
        return ResponseEntity.ok(jsonarr.toString());
    }

    @PostMapping("/validate")
    public ResponseEntity<String> validateUserIdEmail(@RequestBody String data) throws UnsupportedEncodingException {
        logger.info("Inside changePassword method of LoginController");
        JSONObject requestObj = new JSONObject(data);
        byte[] requestData = Base64.getDecoder().decode(requestObj.getString("REQUEST_DATA"));
        Integer status = SQLInjection.sqlInjection(
                URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()), 0);
        JSONObject object = new JSONObject();
        if (status == 1) {
            logger.warn("Inside validateUserIdEmail method of LoginController got SQLInjection");
            object.put("status", 417);
            object.put("msg", "error");
        } else if (TokenCreaterAndMatcher.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
                requestObj.getString("REQUEST_TOKEN"))) {
            object = service.validateUserIdEmail(
                    URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()));
        } else {
            logger.error("Inside validateUserIdEmail method of LoginController  Token mismatch");
            object.put("status", 417);
            object.put("msg", "error");
        }
        JSONObject response = new JSONObject();
        response.put("RESPONSE_DATA", Base64.getEncoder().encodeToString(object.toString().getBytes()));
        response.put("RESPONSE_TOKEN", TokenCreaterAndMatcher
                .getHmacMessage(Base64.getEncoder().encodeToString(object.toString().getBytes())));
        return ResponseEntity.ok(response.toString());
    }

    @PostMapping("/validateTime")
    public ResponseEntity<String> validateTime(@RequestBody String data) throws UnsupportedEncodingException {
        logger.info("Inside validateTime method of LoginController");
        JSONObject requestObj = new JSONObject(data);
        byte[] requestData = Base64.getDecoder().decode(requestObj.getString("REQUEST_DATA"));
        Integer status = SQLInjection.sqlInjection(
                URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()), 0);
        JSONObject object = new JSONObject();
        if (status == 1) {
            logger.warn("Inside validateTime method of LoginController got SQLInjection");
            object.put("status", 417);
            object.put("msg", "error");
        } else if (TokenCreaterAndMatcher.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
                requestObj.getString("REQUEST_TOKEN"))) {
            object = service.validateTime(
                    URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()));
        } else {
            logger.error("Inside validateTime method of LoginController  Token mismatch");
            object.put("status", 417);
            object.put("msg", "error");
        }
        JSONObject response = new JSONObject();
        response.put("RESPONSE_DATA", Base64.getEncoder().encodeToString(object.toString().getBytes()));
        response.put("RESPONSE_TOKEN", TokenCreaterAndMatcher
                .getHmacMessage(Base64.getEncoder().encodeToString(object.toString().getBytes())));
        return ResponseEntity.ok(response.toString());
    }

    @PostMapping("/saveforgetpasswordchange")
    public ResponseEntity<String> saveforgetpasswordchange(@RequestBody String data) throws UnsupportedEncodingException {
        logger.info("Inside saveforgetpasswordchange method of LoginController");
        JSONObject requestObj = new JSONObject(data);
        byte[] requestData = Base64.getDecoder().decode(requestObj.getString("REQUEST_DATA"));
        Integer status = SQLInjection.sqlInjection(
                URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()), 0);
        JSONObject object = new JSONObject();
        if (status == 1) {
            logger.warn("Inside saveforgetpasswordchange method of LoginController got SQLInjection");
            object.put("status", 417);
            object.put("msg", "SQLInjection");
        } else if (TokenCreaterAndMatcher.hashRequestMatch(requestObj.getString("REQUEST_DATA"),
                requestObj.getString("REQUEST_TOKEN"))) {
            object = service.saveforgetpasswordchange(
                    URLDecoder.decode((new String(requestData, StandardCharsets.UTF_8)), StandardCharsets.UTF_8.name()));
        } else {
            logger.error("Inside saveforgetpasswordchange method of LoginController  Token mismatch");
            object.put("status", 417);
            object.put("msg", "Token mismatch");
        }
        JSONObject response = new JSONObject();
        response.put("RESPONSE_DATA", Base64.getEncoder().encodeToString(object.toString().getBytes()));
        response.put("RESPONSE_TOKEN", TokenCreaterAndMatcher
                .getHmacMessage(Base64.getEncoder().encodeToString(object.toString().getBytes())));
        return ResponseEntity.ok(response.toString());
    }
    
    @PostMapping("createTempUser")
    public ResponseEntity<String> createTempUser(@RequestBody String data) throws JsonMappingException, JsonProcessingException {
    	JSONObject requestObj = new JSONObject(data);
    	ObjectMapper mapper = new ObjectMapper();
        byte[] requestData = Base64.getDecoder().decode(requestObj.getString("REQUEST_DATA"));
    	TempUser tempUser = mapper.readValue(new String(requestData), TempUser.class);
    	Tuser userDb = userService.findByMobileOrEmail(tempUser.getMobile(),tempUser.getEmail());
    	JSONObject object = new JSONObject();
    	if(userDb == null) {
	    	tempUser.setUsername(UUID.randomUUID().toString());
	    	authenticationService.tempUserSignUp(tempUser);
	    	
	    	Tuser user = new Tuser();
	    	user.setTxtUserId(tempUser.getEmail());
	    	user.setEnPassword(CommonUtil.getHmacMessage("Admin@123"+salt));
	    	user.setTxtMobileNo(tempUser.getMobile());
	    	user.setTxtEmailId(tempUser.getEmail());
	    	user.setSelRole(43); //Temporary user
	    	user.setTxtFullName(tempUser.getApplicantName());
	    	user.setSelGender(1);
	    	user.setChkPrevilege(3);
	    	user.setSelDesignation(1);
	    	user.setSelEmployeeType(0);
	    	user.setSelDepartment(1);
	    	user.setSelGroup(0);
	    	user.setSelHierarchy(0);
	    	user.setIntReportingAuth(0);
	    	userService.save(mapper.writeValueAsString(user));
	    	object.put("status", 200);
	        object.put("msg", "success");
    	}
    	else {
    		object.put("status", 409);
            object.put("msg", "conflict");
    	}
    	
        JSONObject response = new JSONObject();
        response.put("RESPONSE_DATA", Base64.getEncoder().encodeToString(object.toString().getBytes()));
        response.put("RESPONSE_TOKEN", TokenCreaterAndMatcher
                .getHmacMessage(Base64.getEncoder().encodeToString(object.toString().getBytes())));
        return ResponseEntity.ok(response.toString());
    }
    
    @GetMapping("/getTempUserData/{email}")
    public ResponseEntity<?> getUserData(@PathVariable String email) throws JsonProcessingException{
    	UserDataResponseDto data =  userService.getUserDataByEmailId(email);
    	return ResponseEntity.ok(CommonUtil.inputStreamEncoder(buildJsonResponse(data)).toString());
    }
    
    
    private <T> String buildJsonResponse(T response) throws JsonProcessingException {
		Object result;
		if (response instanceof JSONObject) {
			result = response.toString();
		} else {
			result = response;
		}

		return objectMapper.writeValueAsString(ResponseDTO.builder().status(200).result(result).build());
	}
}
