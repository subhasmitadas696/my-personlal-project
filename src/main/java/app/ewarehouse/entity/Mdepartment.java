package app.ewarehouse.entity;

import jakarta.persistence.*;
import lombok.Data;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.util.Date;
@Data
@Table(name="m_admin_department")
@Entity
public class Mdepartment {
@Id
@GeneratedValue(strategy=GenerationType.IDENTITY)
@Column(name="intId")
private Integer intId;

@Column(name="`vchdeptName`")
private String txtDepartmentName;

@Column(name="`vchAliasName`")
private String txtAliasName;

@Column(name="intCreatedBy")
private Integer intCreatedBy;

@Column(name="intUpdatedBy")
private Integer intUpdatedBy;

@Column(name="dtmCreatedOn")
@CreationTimestamp
private Date dtmCreatedOn;

@Column(name="stmUpdatedOn")
@UpdateTimestamp
private Date stmUpdatedOn;

@Column(name="bitDeletedFlag")
private Boolean bitDeletedFlag=false;


}