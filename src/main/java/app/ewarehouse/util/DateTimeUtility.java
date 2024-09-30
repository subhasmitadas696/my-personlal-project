package app.ewarehouse.util;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.Instant;
import java.time.LocalDate;
import java.time.ZoneId;
import java.time.ZoneOffset;
import java.time.format.DateTimeFormatter;
import java.util.Date;


public class DateTimeUtility {
	public static Date getDateAndTime(long timeStamp) {
		return new Date(timeStamp);
	}

	public static String getTodayDate() {
		LocalDate date = LocalDate.now();
		DateTimeFormatter formatter = DateTimeFormatter.ofPattern(AdminPanelConstant.DATE_FORMAT);
		return formatter.format(date);
	}
	
	public static String getDate(SimpleDateFormat sdf, long millisecond) {
		Date date = new Date(millisecond);
		return sdf.format(date);
	}

	public static long getMilliSecond(String strDate, SimpleDateFormat sdf) {
		long millisecond = 0;
		try {
			Date date = sdf.parse(strDate);
			millisecond = date.getTime();
		} catch (ParseException e) {
			e.printStackTrace();
		}
		return millisecond;
	}
	
	public static long convertLocalDateIntoMilliSecond(LocalDate date) {
		long millisecond = date.atStartOfDay(ZoneOffset.UTC).toInstant().toEpochMilli();
		return millisecond;
	}
	
	public static LocalDate convertMillisecondIntoLocalDate(long millisecond) {
		LocalDate date = Instant.ofEpochMilli(millisecond).atZone(ZoneId.systemDefault()).toLocalDate();
		return date;
	}
	
	
}
