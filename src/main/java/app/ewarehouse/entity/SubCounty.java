package app.ewarehouse.entity;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.*;
import lombok.Data;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

@Entity
@Data
@Table(name = "t_sub_county")
public class SubCounty implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "intId")
    private Integer intId;

    @Column(name = "vchSubCountyName", length = 45)
    private String txtSubCountyName;

    @ManyToOne
    @JoinColumn(name = "intCountyId")
    @JsonBackReference
    private County county;

    @OneToMany(mappedBy = "subCounty", fetch = FetchType.LAZY)
    @JsonManagedReference
    private List<Ward> wards;

    @Column(name = "dtmCreatedOn")
    @CreationTimestamp
    private Date dtmCreatedOn;

    @Column(name = "stmUpdatedOn")
    @UpdateTimestamp
    private Date stmUpdatedOn;

    @Column(name = "intCreatedBy")
    private Integer intCreatedBy;

    @Column(name = "intUpdatedBy")
    private Integer intUpdatedBy;

    @Column(name = "bitDeletedFlag")
    private Boolean bitDeletedFlag = false;
}
