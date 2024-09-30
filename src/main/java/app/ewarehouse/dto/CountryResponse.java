package app.ewarehouse.dto;

import lombok.Builder;
import lombok.Data;

import java.util.List;

@Data
@Builder
public class CountryResponse {
    private Integer countryId;
    private String countryName;
    private String countryCode;
    private Boolean isActive;
    private List<CountyResponse> counties;
}
