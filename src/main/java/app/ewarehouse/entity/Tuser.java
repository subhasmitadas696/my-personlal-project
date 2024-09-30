package app.ewarehouse.entity;

import jakarta.persistence.*;
import lombok.Data;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.time.LocalDateTime;
import java.util.Date;

@Data
@Table(name = "m_admin_user")
@Entity
public class Tuser {
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "intId")
	private Integer intId;
	@Column(name = "`vchFullName`")
	private String txtFullName;
	@Column(name = "`intGender`")
	private Integer selGender;
	@Transient
	private String selGenderVal;
	@Column(name = "`vchPhoto`")
	private String filePhoto;
	@Column(name = "`vchMobileNumber`")
	private String txtMobileNo;
	@Column(name = "`vchEmailId`")
	private String txtEmailId;
	@Column(name = "`vchAltrMobileNo`")
	private String txtAlternateMobileNumber;
	@Column(name = "`vchAddress`")
	private String txtrAddress;
	@Column(name = "`intRoleId`")
	private Integer selRole;
	@Transient
	private String selRoleVal;
	@Column(name = "`intDesignantion`")
	private Integer selDesignation;
	@Transient
	private String selDesignationVal;
	@Column(name = "`intEmployeeType`")
	private Integer selEmployeeType;
	@Transient
	private String selEmployeeTypeVal;
	@Column(name = "`intDepartment`")
	private Integer selDepartment;
	@Transient
	private String selDepartmentVal;
	@Column(name = "`intGroup`")
	private Integer selGroup;
	@Transient
	private String selGroupVal;
	@Column(name = "`intHierarchy`")
	private Integer selHierarchy;
	@Transient
	private String selHierarchyVal;
	@Column(name = "`vchLoginId`")
	private String txtUserId;
	@Column(name = "`vchPassword`")
	private String txtPassword;
	@Transient
	private String enPassword;
	@Column(name = "`intPrevilege`")
	private Integer chkPrevilege;
	@Transient
	private String chkPrevilegeVal;
	@Column(name = "intCreatedBy")
	private Integer intCreatedBy;

	@Column(name = "intUpdatedBy")
	private Integer intUpdatedBy;

	@Column(name = "dtmCreatedOn")
	@CreationTimestamp
	private Date dtmCreatedOn;

	@Column(name = "stmUpdatedOn")
	@UpdateTimestamp
	private Date stmUpdatedOn;

	@Column(name = "bitDeletedFlag")
	private Boolean bitDeletedFlag = false;

	@Column(name = "`vchDtOfJoining`")
	private Date txtDateOfJoining;
	@Transient
	private String txtConfirmPassword;

	@Column(name = "`intReportingAuth`")
	private Integer intReportingAuth;
	@Transient
	private String intReportingAuthVal;

	private LocalDateTime dtforgetpasswordstarttime;

   //@Column(name="vchWarehouse")
    //private String warehouseId;

	@Column(name = "vchWarehouse")
	private String warehouseId;

	@Column(name = "vchUserStatus")
	private String vchUserStatus;


}
