package app.ewarehouse.dto;

import lombok.Builder;
import lombok.Data;

import java.util.Date;
import java.util.List;

@Data
@Builder
public class SubCountyResponse {
    private Integer intId;
    private String txtSubCountyName;
    private CountyResponse county;
    private List<WardResponse> wards;
    private Date dtmCreatedOn;
    private Date stmUpdatedOn;
    private Integer intCreatedBy;
    private Integer intUpdatedBy;
    private Boolean bitDeletedFlag;
}
