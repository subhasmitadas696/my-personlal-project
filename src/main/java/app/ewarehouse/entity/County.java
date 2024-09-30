package app.ewarehouse.entity;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.*;
import lombok.Data;

import java.util.List;

@Data
@Entity
@Table(name = "t_county_master")
public class County {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "county_id")
    private Integer id;

    @Column(name = "county_name")
    private String name;

    @ManyToOne
    @JoinColumn(name = "country_id")
    @JsonBackReference
    private Country country;
    
    @Column(name = "bitDeletedFlag")
    private Boolean bitDeletedFlag = false;

    @OneToMany(mappedBy = "county", fetch = FetchType.LAZY)
    @JsonManagedReference
    private List<SubCounty> subCounties;
}
