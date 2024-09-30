package app.ewarehouse.util;

import app.ewarehouse.dto.Mail;

public class MailTest {

	public static void main(String[] args) {
		String otpno = ComplainTokenGenerator.getOtpNumberString();

		String otpmessage =  "\n\n This is test Mail ";

		Mail mail = new Mail();
		mail.setMailSubject("Demo Mail.");
		mail.setContentType("text/html");

		mail.setMailCc("uiidptestmail@gmail.com"); // CC mail id
		mail.setTemplate(otpmessage);
		mail.setMailTo("kipngenovi2001@gmail.com");
		EmailUtil.sendMail(mail);

	}

}
