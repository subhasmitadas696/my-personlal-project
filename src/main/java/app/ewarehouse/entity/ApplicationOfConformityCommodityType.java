package app.ewarehouse.entity;

import com.fasterxml.jackson.annotation.JsonBackReference;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.FetchType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;
import lombok.Data;

@Data
@Entity
@Table(name = "t_application_of_conformity_commodity_type")
public class ApplicationOfConformityCommodityType {

	@Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "intAocCommodityTypeId")
    private Integer aocCommodityTypeId;

    @ManyToOne
    @JoinColumn(name = "intParticularOfApplicantsId")
    @JsonBackReference
    private ApplicationOfConformityParticularOfApplicants particularOfApplicants;

    @ManyToOne
    @JoinColumn(name = "intCommodityTypeId", nullable = false)
    private Commodity commodityType;
}
