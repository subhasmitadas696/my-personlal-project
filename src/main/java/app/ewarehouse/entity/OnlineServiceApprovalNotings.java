package app.ewarehouse.entity;

import jakarta.persistence.*;
import lombok.Data;
import org.hibernate.annotations.CreationTimestamp;

import java.util.Date;

@Data
@Table(name="t_online_service_approval_notings")
@Entity
public class OnlineServiceApprovalNotings {
@Id
@GeneratedValue(strategy = GenerationType.IDENTITY) 
private int intNotingsId;
private int intOnlineServiceId;
private int intProfileId;
private int intFromAuthority;
private String intToAuthority;
@CreationTimestamp
private Date dtActionTaken;
private int intStatus;
private String txtNoting;
private int tinResubmitStatus;
private String txtRevertRemark;
private Date dtmResubmitDate;
private int tinStageCtr;
private int tinQueryTo;
private String vchIPaddress;
private Date dtmCreatedOn;
private int intCreatedBy;
private Boolean bitDeletedFlag=false;
private int intProcessId;
private String jsnOtherDetails;
@Transient
private String vchRoleName;
@Transient
private String actionTaken;

private Integer inspectionDocId;

private Integer inspObservation;

private Date dateOfReportGen;

private String isOicReasonValid;

private String observation;

private String conditionsToRemoveRevoc;

private String provideReasonRecomm;

private String wasPrevMitigate;

private String issueWithOperator;

}
