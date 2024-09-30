package app.ewarehouse.serviceImpl;

import java.util.Date;
import java.util.List;
import java.util.Random;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import app.ewarehouse.dto.CommonResponseModal;
import app.ewarehouse.dto.OTPRequestDTO;
import app.ewarehouse.dto.OTPValidationRequestDTO;
import app.ewarehouse.entity.OTP;
import app.ewarehouse.repository.IOtpRepository;
import app.ewarehouse.service.OTPService;
import app.ewarehouse.util.DateTimeUtil;

@Service
public class OTPServiceImpl implements OTPService{

	@Autowired
	IOtpRepository  otpRepository;
	@Override
	public CommonResponseModal getOTP(OTPRequestDTO requestDTO) {
	
		CommonResponseModal dto = new CommonResponseModal();
		if (requestDTO.getMobile().equals("") && requestDTO.getEmail().equals("")) {
			dto.setMessage("Email ID or Mobile required");
			dto.setStatus(false);
			return dto;
		} else {
			try {
				String email = requestDTO.getEmail();
				String mobile = requestDTO.getMobile();
				
				  Date d2=otpRepository.currentOTPTime(email);
				  int noOfOTP=otpRepository.getCurrentOtpCount(email); 
				  
				   if(noOfOTP==0&&d2 ==null) {
					   
					   String ip = requestDTO.getIp();
						Random random = new Random();
						int generatedOtp = 100000 + random.nextInt(900000);
						String message = "Your OTP is " + generatedOtp;
						
						OTP otp = new OTP();
						otp.setGeneratedOtp(generatedOtp);
						otp.setEmail(email);
						otp.setMobile(mobile);
						otp.setIpAddress(ip);
						otp.setEdt(new Date());
						otp.setLudt(new Date());
						otp.setFlag(0);
						otp = otpRepository.saveAndFlush(otp);
						dto.setMessage(message);
						dto.setStatus(true);
						return dto;
					   
				   }else {
					        Date d1=new Date();
						  long time=  DateTimeUtil.timeDiff(d1,d2,"Seconds");

						  if(time<5 || time>60) {
							  if(noOfOTP<=2) {
									String ip = requestDTO.getIp();
									Random random = new Random();
									int generatedOtp = 100000 + random.nextInt(900000);
									String message = "Your OTP is " + generatedOtp;
									
									OTP otp = new OTP();
									otp.setGeneratedOtp(generatedOtp);
									otp.setEmail(email);
									otp.setMobile(mobile);
									otp.setIpAddress(ip);
									otp.setEdt(new Date());
									otp.setLudt(new Date());
									otp.setFlag(0);
									otp = otpRepository.saveAndFlush(otp);
									dto.setMessage(message);
									dto.setStatus(true);
									return dto;									
							  }
							  else {
								  
								  
								  dto.setMessage("You have exceeded the allotted time. Please try again after five minutes.");
								  dto.setStatus(false);
								 
								return dto;
							  }
							  
						  }
						  else {
							   
							  
							  List<OTP> otpList = otpRepository.findByFlagAndEmail(0, email);
								if (!otpList.isEmpty()) {
									for (OTP otp : otpList) {
										otp.setFlag(1);
										otp = otpRepository.saveAndFlush(otp);
									}
								}
							  dto.setMessage("You have exceeded the allotted time. Please try again after five minutes.");
							  dto.setStatus(false);
							  System.out.println("i am from time else");
							return dto;
						  }
				   }
				
			} catch (Exception e) {
				dto.setMessage(e.getLocalizedMessage());
				dto.setStatus(false);
				return dto;
			}
		}		
	}
	
	@Override
	public CommonResponseModal validateOTP(OTPValidationRequestDTO otpdto) {
		
		CommonResponseModal dto = new CommonResponseModal();
		OTP otp = otpRepository.findByEmailAndFlag(otpdto.getEmail(), 0);
		if (otp != null) {
			if (!otpdto.getOtp().equals(Integer.toString(otp.getGeneratedOtp()))) {
				
				dto.setMessage("OTP is not matched.");
				dto.setStatus(false);
				return dto;
			}
			
			 Date d2=otpRepository.currentOTPTime(otpdto.getEmail());
			 Date d1=new Date();
			 long time=  DateTimeUtil.timeDiff(d1,d2,"Minutes");
			 
			 if(time>5) {
				 dto.setMessage("OTP is not matched.");
				 dto.setStatus(false);
				 return dto;
			 }
			 			
		}
		
		otp.setFlag(1);
		otp = otpRepository.saveAndFlush(otp);
		dto.setMessage("OTP Validated");
		dto.setStatus(true);
		return dto;	
	}
}
