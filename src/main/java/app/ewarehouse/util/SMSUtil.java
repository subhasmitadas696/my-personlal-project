package app.ewarehouse.util;

import javax.net.ssl.*;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

public class SMSUtil {

	private static TrustManager[] get_trust_mgr() {
		TrustManager[] certs = new TrustManager[] { new X509TrustManager() {

			@Override
			public X509Certificate[] getAcceptedIssuers() {
				return null;
			}

			@Override
			public void checkServerTrusted(X509Certificate[] chain, String authType) throws CertificateException {

			}

			@Override
			public void checkClientTrusted(X509Certificate[] chain, String authType) throws CertificateException {
			}
		} };
		return certs;
	}

	private String print_content(HttpsURLConnection con) {
		String input = "";
		if (con != null) {
			try {
				BufferedReader br = new BufferedReader(new InputStreamReader(con.getInputStream()));
				while ((input = br.readLine()) != null) {

				}
				br.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
		return input;
	}

		static HostnameVerifier allHostsValid = new HostnameVerifier() {  
		        public boolean verify(String hostname, SSLSession session) {
		            return true;  
		        }
		    };
		public static String validateSmsString(String sms) {
		        if (sms.contains(" ")) {
		            sms = sms.replace(" ", "%20");
		            return sms;
		        } else {
		            return sms;
		        }
		    }

			public String sms(String mobile, String message) throws NoSuchAlgorithmException, KeyManagementException {
				String tmp = "";
				HttpsURLConnection.setDefaultHostnameVerifier(allHostsValid);
				String https_url = "https://164.100.14.211/failsafe/HttpLink?username=estat.auth&pin=Hr%243y*jaq&message="
						+ message + "&mnumber=" + mobile + "&signature=ESTATE&dlt_entity_id=1001663550000020145";
				URL url;
				try {
					SSLContext ssl_ctx = SSLContext.getInstance("TLS");
					TrustManager[] trust_mgr = get_trust_mgr();
					ssl_ctx.init(null, // key manager
							trust_mgr, // trust manager
							new SecureRandom()); // random number generator
					HttpsURLConnection.setDefaultSSLSocketFactory(ssl_ctx.getSocketFactory());
					url = new URL(https_url);
					HttpsURLConnection con = (HttpsURLConnection) url.openConnection();
					tmp = print_content(con);
				} catch (IOException e) {
					e.printStackTrace();
				}
				return tmp;
			}
	
			public String smsWithTemplate(String mobile, String message, String templateId)throws NoSuchAlgorithmException, KeyManagementException {
				String tmp = "";
				HttpsURLConnection.setDefaultHostnameVerifier(allHostsValid);
				String https_url = "https://164.100.14.211/failsafe/HttpLink?username=estat.auth&pin=Hr%243y*jaq&message=" + message
						+ "&mnumber=" + mobile + "&signature=ESTATE&dlt_entity_id=1001663550000020145&dlt_template_id=" + templateId;
				URL url;
				try {
					SSLContext ssl_ctx = SSLContext.getInstance("TLS");
					TrustManager[] trust_mgr = get_trust_mgr();
					ssl_ctx.init(null, // key manager
							trust_mgr, // trust manager
							new SecureRandom()); // random number generator
					HttpsURLConnection.setDefaultSSLSocketFactory(ssl_ctx.getSocketFactory());
					url = new URL(https_url);
					HttpsURLConnection con = (HttpsURLConnection) url.openConnection();
					tmp = print_content(con);
				} catch (IOException e) {
					e.printStackTrace();
				}
				return tmp;
			}
}
