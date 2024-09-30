package app.ewarehouse.dto;

import lombok.Builder;
import lombok.Data;

import java.util.List;

@Data
@Builder
public class CountyResponse {
    private Integer id;
    private String name;
    private CountryResponse country;
    private List<SubCountyResponse> subCounties;
    private Boolean bitDeletedFlag;
}
