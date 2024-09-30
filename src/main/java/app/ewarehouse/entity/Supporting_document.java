package app.ewarehouse.entity;


import jakarta.persistence.Entity;
import jakarta.persistence.Column;
import jakarta.persistence.GenerationType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Table;
import jakarta.persistence.Id;
import jakarta.persistence.Transient;
import java.sql.Date;
import java.sql.Timestamp;
import lombok.Data;
import jakarta.persistence.SequenceGenerator;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;
@Data
@Table(name="supporting_document")
@Entity
public class Supporting_document {
@Column(name="bitDeletedFlag", columnDefinition="BIT")
private Boolean bitDeletedFlag=false;

@Column(name="dtmCreatedOn")
@CreationTimestamp
private Timestamp dtmCreatedOn;

@Column(name="intCreatedBy")
private Integer intCreatedBy;

@Id
@GeneratedValue(strategy=GenerationType.IDENTITY)
@Column(name="intId")
private Integer intId;

@Column(name="intInsertStatus")
private Integer intInsertStatus=1;

private Integer intParentId;
@Column(name="intUpdatedBy")
private Integer intUpdatedBy;

@Column(name="`name_document`")
private String amtxtDocumentName;
@Column(name="stmUpdatedOn")
@UpdateTimestamp
private Timestamp stmUpdatedOn;

@Column(name="`uplod_doc_path`")
private String amfileUploadDocuments;
}